using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public abstract class UsersStorageProviderTestScaffolding
    {
        [TearDown]
        public void TearDown()
        {
            try
            {
                Directory.Delete(testDir, true);
            }
            catch
            {
            }
        }

        private readonly MockRepository mocks = new MockRepository();

        private readonly string testDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"),
            Guid.NewGuid().ToString());

        protected IHostV30 MockHost()
        {
            if (!Directory.Exists(testDir)) Directory.CreateDirectory(testDir);

            var host = mocks.DynamicMock<IHostV30>();
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.AtLeastOnce();

            mocks.Replay(host);

            return host;
        }

        public abstract IUsersStorageProviderV30 GetProvider();

        private void AssertUserInfosAreEqual(UserInfo expected, UserInfo actual, bool checkProvider)
        {
            Assert.AreEqual(expected.Username, actual.Username, "Wrong username");
            Assert.AreEqual(expected.DisplayName, actual.DisplayName, "Wrong display name");
            Assert.AreEqual(expected.Email, actual.Email, "Wrong email");
            Assert.AreEqual(expected.Active, actual.Active, "Wrong activation status");
            Tools.AssertDateTimesAreEqual(expected.DateTime, actual.DateTime, true);
            if (checkProvider) Assert.AreSame(expected.Provider, actual.Provider, "Different provider instances");
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void AddUser_InvalidUsername(string u)
        {
            var prov = GetProvider();

            prov.AddUser(u, null, "pass", "email@server.com", true, DateTime.Now);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void AddUser_InvalidPassword(string p)
        {
            var prov = GetProvider();

            prov.AddUser("user", null, p, "email@server.com", true, DateTime.Now);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void AddUser_InvalidEmail(string e)
        {
            var prov = GetProvider();

            prov.AddUser("user", null, "pass", e, true, DateTime.Now);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void ModifyUser_InvalidNewEmail(string e)
        {
            var prov = GetProvider();

            var user = new UserInfo("username", null, "email@server.com", true, DateTime.Now, prov);
            prov.AddUser(user.Username, user.DisplayName, "password", user.Email, user.Active, user.DateTime);

            prov.ModifyUser(user, "Display Name", null, e, false);
        }

        private void AssertUserGroupsAreEqual(UserGroup expected, UserGroup actual, bool checkProvider)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Wrong name");
            Assert.AreEqual(expected.Description, actual.Description, "Wrong description");
            Assert.AreEqual(expected.Users.Length, actual.Users.Length, "Wrong user count");
            for (var i = 0; i < expected.Users.Length; i++)
            {
                Assert.AreEqual(expected.Users[i], actual.Users[i], "Wrong user");
            }
            if (checkProvider)
            {
                Assert.AreSame(expected.Provider, actual.Provider, "Wrong provider");
            }
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void AddUserGroup_InvalidName(string n)
        {
            var prov = GetProvider();
            prov.AddUserGroup(n, "Description");
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void GetUser_InvalidUsername(string u)
        {
            var prov = GetProvider();

            prov.GetUser(u);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void GetUserByEmail_InvalidEmail(string e)
        {
            var prov = GetProvider();

            prov.GetUserByEmail(e);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void StoreUserData_InvalidKey(string k)
        {
            var prov = GetProvider();

            var user = new UserInfo("User", "User", "user@users.com", true, DateTime.Now, prov);

            prov.StoreUserData(user, k, "Value");
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveUserData_InvalidKey(string k)
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            prov.RetrieveUserData(user, k);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void GetUsersWithData_InvalidKey(string k)
        {
            var prov = GetProvider();

            prov.GetUsersWithData(k);
        }

        [Test]
        public void AddUser_GetUsers()
        {
            var prov = GetProvider();

            var u1 = new UserInfo("user", "User", "email@server.com", true, DateTime.Now.AddDays(-1), prov);
            var u2 = new UserInfo("john", null, "john@john.com", false, DateTime.Now, prov);

            var u1Out = prov.AddUser(u1.Username, u1.DisplayName, "password", u1.Email, u1.Active, u1.DateTime);
            Assert.IsNotNull(u1Out, "AddUser should return something");
            AssertUserInfosAreEqual(u1, u1Out, true);

            var u2Out = prov.AddUser(u2.Username, u2.DisplayName, "password", u2.Email, u2.Active, u2.DateTime);
            Assert.IsNotNull(u2Out, "AddUser should return something");
            AssertUserInfosAreEqual(u2, u2Out, true);

            Assert.IsNull(prov.AddUser("user", null, "pwd999", "dummy@server.com", false, DateTime.Now),
                "AddUser should return false");

            var users = prov.GetUsers().ToArray();
            Array.Sort(users, delegate(UserInfo x, UserInfo y) { return x.Username.CompareTo(y.Username); });

            Assert.AreEqual(2, users.Length, "Wrong user count");

            AssertUserInfosAreEqual(u2, users[0], true);
            AssertUserInfosAreEqual(u1, users[1], true);
        }

        [Test]
        public void AddUserGroup_GetUserGroups()
        {
            var prov = GetProvider();

            var group1 = prov.AddUserGroup("Group1", "Test1");
            var expected1 = new UserGroup("Group1", "Test1", prov);
            var group2 = prov.AddUserGroup("Group2", "Test2");
            var expected2 = new UserGroup("Group2", "Test2", prov);

            Assert.IsNull(prov.AddUserGroup("Group1", "Test"), "AddUserGroup should return null");

            AssertUserGroupsAreEqual(expected1, group1, true);
            AssertUserGroupsAreEqual(expected2, group2, true);

            var allGroups = prov.GetUserGroups().ToArray();
            Assert.AreEqual(2, allGroups.Length, "Wrong group count");
            Array.Sort(allGroups, delegate(UserGroup x, UserGroup y) { return x.Name.CompareTo(y.Name); });

            AssertUserGroupsAreEqual(expected1, allGroups[0], true);
            AssertUserGroupsAreEqual(expected2, allGroups[1], true);
        }

        [Test]
        public void GetUser()
        {
            var prov = GetProvider();

            var user = prov.AddUser("user", null, "password", "email@server.com", true, DateTime.Now);

            Assert.IsNull(prov.GetUser("inexistent"), "TryGetUser should return null");

            var output = prov.GetUser("user");

            AssertUserInfosAreEqual(user, output, true);
        }

        [Test]
        public void GetUserByEmail()
        {
            var prov = GetProvider();

            var user1 = prov.AddUser("user1", null, "password", "email1@server.com", true, DateTime.Now);
            var user2 = prov.AddUser("user2", null, "password", "email2@server.com", true, DateTime.Now);

            Assert.IsNull(prov.GetUserByEmail("inexistent@server.com"), "TryGetUserByEmail should return null");

            var output = prov.GetUserByEmail("email1@server.com");

            AssertUserInfosAreEqual(user1, output, true);
        }

        [Test]
        public void GetUsersWithData()
        {
            var prov = GetProvider();

            var user1 = prov.AddUser("user1", "User1", "password", "user1@users.com", true, DateTime.Now);
            var user2 = prov.AddUser("user2", "User2", "password", "user2@users.com", true, DateTime.Now);
            var user3 = prov.AddUser("user3", "User3", "password", "user3@users.com", true, DateTime.Now);
            var user4 = prov.AddUser("user4", "User4", "password", "user4@users.com", true, DateTime.Now);

            Assert.AreEqual(0, prov.GetUsersWithData("Key").Count, "Wrong user count");

            prov.StoreUserData(user1, "Key", "Value");
            prov.StoreUserData(user2, "Key2", "Value");
            prov.StoreUserData(user4, "Key", "Value2");

            var data = prov.GetUsersWithData("Key");

            Assert.AreEqual(2, data.Count, "Wrong user count");

            var users = new UserInfo[data.Count];
            data.Keys.CopyTo(users, 0);

            AssertUserInfosAreEqual(user1, users[0], true);
            AssertUserInfosAreEqual(user4, users[1], true);

            Assert.AreEqual("Value", data[users[0]], "Wrong data");
            Assert.AreEqual("Value2", data[users[1]], "Wrong data");
        }

        [Test]
        public void ModifyUser()
        {
            var prov = GetProvider();

            var user = new UserInfo("username", null, "email@server.com", false, DateTime.Now, prov);
            prov.AddUser(user.Username, user.DisplayName, "password", user.Email, user.Active, user.DateTime);
            prov.AddUser("zzzz", null, "password2", "email@server2.com", false, DateTime.Now);

            // Set new password
            var expected = new UserInfo(user.Username, "New Display", "new@server.com", true, user.DateTime, prov);
            var result = prov.ModifyUser(user, "New Display", "newpass", "new@server.com", true);
            AssertUserInfosAreEqual(expected, result, true);

            var allUsers = prov.GetUsers().ToArray();
            Assert.AreEqual(2, allUsers.Length, "Wrong user count");
            Array.Sort(allUsers, delegate(UserInfo x, UserInfo y) { return x.Username.CompareTo(y.Username); });
            AssertUserInfosAreEqual(expected, allUsers[0], true);

            Assert.IsTrue(prov.TestAccount(user, "newpass"), "TestAccount should return true");

            // Set null display name
            expected = new UserInfo(user.Username, null, "new@server.com", true, user.DateTime, prov);
            result = prov.ModifyUser(user, null, null, "new@server.com", true);
            AssertUserInfosAreEqual(expected, result, true);

            allUsers = prov.GetUsers().ToArray();
            Assert.AreEqual(2, allUsers.Length, "Wrong user count");
            Array.Sort(allUsers, delegate(UserInfo x, UserInfo y) { return x.Username.CompareTo(y.Username); });
            AssertUserInfosAreEqual(expected, allUsers[0], true);

            Assert.IsTrue(prov.TestAccount(user, "newpass"), "TestAccount should return true");
        }

        [Test]
        public void ModifyUserGroup()
        {
            var prov = GetProvider();

            var group1 = prov.AddUserGroup("Group1", "Description1");
            var group2 = prov.AddUserGroup("Group2", "Description2");

            Assert.IsNull(prov.ModifyUserGroup(new UserGroup("Inexistent", "Descr", prov), "New"),
                "ModifyUserGroup should return null");

            prov.SetUserMembership(prov.AddUser("user", "user", "pass", "user@server.com", true, DateTime.Now),
                new[] {"Group2"});

            var group2Out = prov.ModifyUserGroup(new UserGroup("Group2", "Description2", prov), "Mod");

            var expected = new UserGroup("Group2", "Mod", prov);
            expected.Users = new[] {"user"};

            AssertUserGroupsAreEqual(expected, group2Out, true);

            var allGroups = prov.GetUserGroups().ToArray();
            Assert.AreEqual(2, allGroups.Length, "Wrong group count");
            Array.Sort(allGroups, delegate(UserGroup x, UserGroup y) { return x.Name.CompareTo(y.Name); });

            AssertUserGroupsAreEqual(new UserGroup("Group1", "Description1", prov), allGroups[0], true);
            AssertUserGroupsAreEqual(expected, allGroups[1], true);
        }

        [Test]
        public void RemoveUser()
        {
            var prov = GetProvider();

            var user = prov.AddUser("user", null, "password", "email@server.com", false, DateTime.Now);

            Assert.IsFalse(
                prov.RemoveUser(new UserInfo("user1", "Joe", "email1@server.com", false, DateTime.Now, prov)),
                "RemoveUser should return false");

            Assert.IsTrue(prov.RemoveUser(user), "RemoveUser should return true");

            Assert.AreEqual(0, prov.GetUsers().Count(), "Wrong user count");
        }

        [Test]
        public void RemoveUserGroup()
        {
            var prov = GetProvider();

            var group1 = prov.AddUserGroup("Group1", "Description1");
            var group2 = prov.AddUserGroup("Group2", "Description2");

            Assert.IsFalse(prov.RemoveUserGroup(new UserGroup("Inexistent", "Descr", prov)),
                "RemoveUserGroup should return false");

            Assert.IsTrue(prov.RemoveUserGroup(new UserGroup("Group1", "Desc", prov)), "RemoveUser should return true");

            var allGroups = prov.GetUserGroups().ToArray();
            Assert.AreEqual(1, allGroups.Length, "Wrong group count");

            AssertUserGroupsAreEqual(group2, allGroups[0], true);
        }

        [Test]
        public void RetrieveAllUserData()
        {
            var prov = GetProvider();

            Assert.AreEqual(0,
                prov.RetrieveAllUserData(new UserInfo("Inexistent", "Inex", "inex@users.com", true, DateTime.Now, prov))
                    .Count, "Wrong data count");

            var user1 = prov.AddUser("user1", "User1", "password", "user1@users.com", true, DateTime.Now);
            var user2 = prov.AddUser("user2", "User2", "password", "user2@users.com", true, DateTime.Now);

            Assert.AreEqual(0, prov.RetrieveAllUserData(user1).Count, "Wrong data count");

            prov.StoreUserData(user1, "Key", "Value");
            prov.StoreUserData(user1, "Key2", "Value2");
            prov.StoreUserData(user2, "Key", "Value3");

            var data = prov.RetrieveAllUserData(user1);
            Assert.AreEqual(2, data.Count, "Wrong data count");
            Assert.AreEqual("Value", data["Key"], "Wrong data");
            Assert.AreEqual("Value2", data["Key2"], "Wrong data");
        }

        [Test]
        public void RetrieveUserData_InexistentKey()
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            Assert.IsNull(prov.RetrieveUserData(user, "Inexistent"), "RetrieveUserData should return null");
        }

        [Test]
        public void SetUserMembership()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;

            var user = prov.AddUser("user", "user", "pass", "user@server.com", true, dt);
            var group1 = prov.AddUserGroup("Group1", "");
            var group2 = prov.AddUserGroup("Group2", "");

            Assert.IsNull(
                prov.SetUserMembership(
                    new UserInfo("user222", "user222", "user222@server.com", true, DateTime.Now, prov), new string[0]),
                "SetUserMembership should return null");

            Assert.IsNull(prov.SetUserMembership(user, new[] {"Group2", "Inexistent"}),
                "SetUserMembership should return null");

            var output = prov.SetUserMembership(user, new[] {"Group2", "Group1"});
            AssertUserInfosAreEqual(new UserInfo("user", "user", "user@server.com", true, dt, prov), output, true);
            Assert.AreEqual(2, output.Groups.Count(), "Wrong group count");
            var orderedGroups = output.Groups.OrderBy(g => g).ToList();
            Assert.AreEqual("Group1", orderedGroups[0], "Wrong group");
            Assert.AreEqual("Group2", orderedGroups[1], "Wrong group");

            var allUsers = prov.GetUsers().ToArray();
            Assert.AreEqual(2, allUsers[0].Groups.Count(), "Wrong group count");
            orderedGroups = allUsers[0].Groups.OrderBy(g => g).ToList();
            Assert.AreEqual("Group1", orderedGroups[0], "Wrong group");
            Assert.AreEqual("Group2", orderedGroups[1], "Wrong group");

            // Also test ModifyUser
            var info = prov.ModifyUser(output, output.Username, "Pass", output.Email, output.Active);
            orderedGroups = info.Groups.OrderBy(g => g).ToList();
            Assert.AreEqual("Group1", orderedGroups[0], "Wrong group");
            Assert.AreEqual("Group2", orderedGroups[1], "Wrong group");

            var allGroups = prov.GetUserGroups().ToArray();

            Assert.AreEqual(2, allGroups.Length, "Wrong group count");

            var expected1 = new UserGroup("Group1", "", prov);
            expected1.Users = new[] {"user"};
            var expected2 = new UserGroup("Group2", "", prov);
            expected2.Users = new[] {"user"};

            Array.Sort(allGroups, delegate(UserGroup x, UserGroup y) { return x.Name.CompareTo(y.Name); });
            AssertUserGroupsAreEqual(expected1, allGroups[0], true);
            AssertUserGroupsAreEqual(expected2, allGroups[1], true);

            output = prov.SetUserMembership(user, new string[0]);
            AssertUserInfosAreEqual(new UserInfo("user", "user", "user@server.com", true, dt, prov), output, true);
            Assert.AreEqual(0, output.Groups.Count(), "Wrong group count");

            allGroups = prov.GetUserGroups().ToArray();

            Assert.AreEqual(2, allGroups.Length, "Wrong group count");

            expected1 = new UserGroup("Group1", "", prov);
            expected2 = new UserGroup("Group2", "", prov);

            Array.Sort(allGroups, delegate(UserGroup x, UserGroup y) { return x.Name.CompareTo(y.Name); });
            AssertUserGroupsAreEqual(expected1, allGroups[0], true);
            AssertUserGroupsAreEqual(expected2, allGroups[1], true);

            allUsers = prov.GetUsers().ToArray();
            Assert.AreEqual(0, allUsers[0].Groups.Count(), "Wrong group count");

            // Also test ModifyUser
            info = prov.ModifyUser(output, output.Username, "Pass", output.Email, output.Active);
            Assert.AreEqual(0, info.Groups.Count(), "Wrong group count");
        }

        [Test]
        public void StoreUserData_RetrieveUserData()
        {
            var prov = GetProvider();

            var user = new UserInfo("User", "User", "user@users.com", true, DateTime.Now, prov);

            Assert.IsFalse(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return false");

            user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key2", "Value2"), "StoreUserData should return true");
            var value = prov.RetrieveUserData(user, "Key");
            Assert.AreEqual("Value", value, "Wrong value");
            var value2 = prov.RetrieveUserData(user, "Key2");
            Assert.AreEqual("Value2", value2, "Wrong value");
        }

        [Test]
        public void StoreUserData_RetrieveUserData_EmptyValue()
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key2", "Value2"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key", ""), "StoreUserData should return true");

            var value = prov.RetrieveUserData(user, "Key");
            Assert.AreEqual("", value, "Wrong value");
            var value2 = prov.RetrieveUserData(user, "Key2");
            Assert.AreEqual("Value2", value2, "Wrong value");
        }

        [Test]
        public void StoreUserData_RetrieveUserData_NullValue()
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key2", "Value2"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key", null), "StoreUserData should return true");

            var value = prov.RetrieveUserData(user, "Key");
            Assert.IsNull(value, "Wrong value");
            var value2 = prov.RetrieveUserData(user, "Key2");
            Assert.AreEqual("Value2", value2, "Wrong value");
        }

        [Test]
        public void StoreUserData_RetrieveUserData_Overwrite()
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);

            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value1"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key2", "Value2"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return true");
            var value = prov.RetrieveUserData(user, "Key");
            Assert.AreEqual("Value", value, "Wrong value");
            var value2 = prov.RetrieveUserData(user, "Key2");
            Assert.AreEqual("Value2", value2, "Wrong value");
        }

        [Test]
        public void StoreUserData_RetrieveUserData_RemoveUser()
        {
            var prov = GetProvider();

            var user = prov.AddUser("User", "User", "password", "user@users.com", true, DateTime.Now);
            var user2 = prov.AddUser("User2", "User2", "password2", "user2@users.com", true, DateTime.Now);

            Assert.IsTrue(prov.StoreUserData(user, "Key", "Value"), "StoreUserData should return true");
            Assert.IsTrue(prov.StoreUserData(user2, "Key", "Value"), "StoreUserData should return true");
            prov.RemoveUser(user);

            var value = prov.RetrieveUserData(user, "Key");
            Assert.IsNull(value, "Wrong value");
            var value2 = prov.RetrieveUserData(user2, "Key");
            Assert.AreEqual(value2, "Value", "Wrong value");
        }

        [Test]
        public void TestAccount()
        {
            var prov = GetProvider();

            var u1 = prov.AddUser("user1", null, "password", "email1@server.com", true, DateTime.Now);
            var u2 = prov.AddUser("user2", "User", "password", "email2@server.com", false, DateTime.Now);

            Assert.IsTrue(prov.TestAccount(u1, "password"), "TestAccount should return true");
            Assert.IsFalse(
                prov.TestAccount(
                    new UserInfo(u1.Username.ToUpperInvariant(), null, "email1@server.com", true, DateTime.Now, prov),
                    "password"), "TestAccount should return false");
            Assert.IsFalse(prov.TestAccount(u2, "password"),
                "TestAccount should return false because the account is disabled");
            Assert.IsFalse(
                prov.TestAccount(new UserInfo("blah", null, "email30@server.com", true, DateTime.Now, prov), "blah"),
                "TestAccount should return false");
            Assert.IsFalse(prov.TestAccount(u1, "password222"), "TestAccount should return false");
            Assert.IsFalse(prov.TestAccount(u1, ""), "TestAccount should return false");
        }

        [Test]
        public void TryManualLogin()
        {
            var prov = GetProvider();

            var user = prov.AddUser("user", null, "password", "email@server.com", true, DateTime.Now);
            prov.AddUser("user2", null, "password", "email2@server.com", false, DateTime.Now);

            var output = prov.TryManualLogin("inexistent", "password");
            Assert.IsNull(output, "TryManualLogin should return null");

            output = prov.TryManualLogin("inexistent", "");
            Assert.IsNull(output, "TryManualLogin should return null");

            output = prov.TryManualLogin("", "password");
            Assert.IsNull(output, "TryManualLogin should return null");

            output = prov.TryManualLogin("user2", "password");
            Assert.IsNull(output, "TryManualLogin should return null because the account is inactive");

            output = prov.TryManualLogin("user", "password");
            AssertUserInfosAreEqual(user, output, true);
        }
    }
}