using NUnit.Framework;
using System;
using System.Linq;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public class AuthToolsTests
    {
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void Static_PrepareUsername_InvalidUsername(string s)
        {
            AuthTools.PrepareUsername(s);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void Static_PrepareGroups_InvalidElement(string e)
        {
            AuthTools.PrepareGroups(new[] { e });
        }

        [TestCase(null, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", false, ExpectedException = typeof(ArgumentException))]
        [TestCase("G", false, ExpectedException = typeof(ArgumentException))]
        [TestCase("G.", true)]
        [TestCase("g.", true)]
        [TestCase("G.Blah", true)]
        [TestCase("g.Blah", true)]
        [TestCase("U.", false)]
        [TestCase("u.", false)]
        [TestCase("U.Blah", false)]
        [TestCase("u.Blah", false)]
        public void Static_IsGroup(string subject, bool result)
        {
            Assert.AreEqual(result, AuthTools.IsGroup(subject), "Wrong result");
        }

        [Test]
        public void Static_PrepareGroups()
        {
            var groups = AuthTools.PrepareGroups(new string[0]).ToList();
            Assert.AreEqual(0, groups.Count, "Wrong result length");

            string[] input = { "Group", "G.Group" };
            var output = AuthTools.PrepareGroups(input).ToList();

            Assert.AreEqual(input.Length, output.Count, "Wrong result length");
            for (var i = 0; i < input.Length; i++)
            {
                Assert.AreEqual("G." + input[i], output[i], "Wrong value");
            }
        }

        [Test]
        public void Static_PrepareUsername()
        {
            Assert.AreEqual("U.User", AuthTools.PrepareUsername("User"), "Wrong result");
            Assert.AreEqual("U.U.User", AuthTools.PrepareUsername("U.User"), "Wrong result");
        }
    }
}