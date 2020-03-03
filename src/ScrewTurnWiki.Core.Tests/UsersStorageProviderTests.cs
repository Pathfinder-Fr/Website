using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    public class UsersStorageProviderTests : UsersStorageProviderTestScaffolding
    {
        public override IUsersStorageProviderV30 GetProvider()
        {
            var prov = new UsersStorageProvider();
            prov.Init(MockHost(), "");
            return prov;
        }

        [Test]
        public void Init()
        {
            var prov = GetProvider();
            prov.Init(MockHost(), "");

            Assert.IsNotNull(prov.Information, "Information should not be null");
        }

        [Test]
        public void Init_Upgrade()
        {
            var testDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);

            var mocks = new MockRepository();
            var host = mocks.DynamicMock<IHostV30>();
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.AtLeastOnce();
            Expect.Call(host.GetSettingValue(SettingName.AdministratorsGroup)).Return("Administrators").Repeat.Once();
            Expect.Call(host.GetSettingValue(SettingName.UsersGroup)).Return("Users").Repeat.Once();

            Expect.Call(host.UpgradeSecurityFlagsToGroupsAcl(null, null)).IgnoreArguments().Repeat.Times(1).Return(true);

            mocks.Replay(host);

            var file = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "Users.cs");

            File.WriteAllText(file,
                "user|PASSHASH|user@users.com|Inactive|2008/10/31 15:15:15|USER\r\nsuperuser|SUPERPASSHASH|superuser@users.com|Active|2008/10/31 15:15:16|ADMIN");

            var prov = new UsersStorageProvider();
            prov.Init(host, "");

            var users = prov.GetUsers().ToArray();

            Assert.AreEqual(2, users.Length, "Wrong user count");

            Assert.AreEqual("superuser", users[0].Username, "Wrong username");
            Assert.AreEqual("superuser@users.com", users[0].Email, "Wrong email");
            Assert.IsTrue(users[0].Active, "User should be active");
            Assert.AreEqual("2008/10/31 15:15:16", users[0].DateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"),
                "Wrong date/time");
            Assert.AreEqual(1, users[0].Groups.Count(), "Wrong user count");
            Assert.AreEqual("Administrators", users[0].Groups.First(), "Wrong group");

            Assert.AreEqual("user", users[1].Username, "Wrong username");
            Assert.AreEqual("user@users.com", users[1].Email, "Wrong email");
            Assert.IsFalse(users[1].Active, "User should be inactive");
            Assert.AreEqual("2008/10/31 15:15:15", users[1].DateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"),
                "Wrong date/time");
            Assert.AreEqual(1, users[1].Groups.Count(), "Wrong user count");
            Assert.AreEqual("Users", users[1].Groups.First(), "Wrong group");

            mocks.Verify(host);

            Directory.Delete(testDir, true);
        }
    }
}