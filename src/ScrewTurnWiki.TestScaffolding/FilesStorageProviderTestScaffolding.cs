using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public abstract class FilesStorageProviderTestScaffolding
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
                //Console.WriteLine("Test: could not delete temp directory");
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

        protected IPagesStorageProviderV30 MockPagesProvider()
        {
            var prov = mocks.DynamicMock<IPagesStorageProviderV30>();

            mocks.Replay(prov);

            return prov;
        }

        public abstract IFilesStorageProviderV30 GetProvider();

        private Stream FillStream(string content)
        {
            var ms = new MemoryStream();
            var buff = Encoding.UTF8.GetBytes(content);
            ms.Write(buff, 0, buff.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void StoreFile_InvalidFullName(string fn)
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile(fn, s, false);
            }
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RetrieveFile_InvalidFile(string f)
        {
            var prov = GetProvider();

            using (var s = new MemoryStream())
            {
                prov.RetrieveFile(f, s, false);
            }
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetFileDetails_InvalidFile(string f)
        {
            var prov = GetProvider();

            prov.GetFileDetails(f);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void SetFileRetrievalCount_InvalidFile(string f)
        {
            var prov = GetProvider();

            prov.SetFileRetrievalCount(f, 10);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void DeleteFile_InvalidFile(string f)
        {
            var prov = GetProvider();

            prov.DeleteFile(f);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenameFile_InvalidFile(string f)
        {
            var prov = GetProvider();

            prov.RenameFile(f, "/Blah.txt");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenameFile_InvalidName(string n)
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
            }

            prov.RenameFile("/File.txt", n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void CreateDirectory_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        [TestCase("/", ExpectedException = typeof(ArgumentException))]
        public void DeleteDirectory_InvalidDirectory(string d)
        {
            var prov = GetProvider();

            prov.DeleteDirectory(d);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        [TestCase("/", ExpectedException = typeof(ArgumentException))]
        public void RenameDirectory_InvalidDirectory(string d)
        {
            var prov = GetProvider();

            prov.RenameDirectory(d, "/Dir/");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        [TestCase("/", ExpectedException = typeof(ArgumentException))]
        public void RenameDirectory_InvalidNewDir(string n)
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Dir");

            prov.RenameDirectory("/Dir/", n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void StorePageAttachment_InvalidName(string n)
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, n, s, false);
            }
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RetrievePageAttachment_InvalidName(string n)
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var ms = new MemoryStream())
            {
                prov.RetrievePageAttachment(pi, n, ms, false);
            }
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetPageAttachmentDetails_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.GetPageAttachmentDetails(new PageInfo("Page", null, DateTime.Now), n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void SetPageAttachmentRetrievalCount_InvalidFile(string f)
        {
            var prov = GetProvider();

            prov.SetPageAttachmentRetrievalCount(new PageInfo("Page", null, DateTime.Now), f, 10);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void DeletePageAttachment_InvalidName(string n)
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            prov.DeletePageAttachment(pi, n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenamePageAttachment_InvalidName(string n)
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            prov.RenamePageAttachment(pi, n, "File2.txt");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenamePageAttachment_InvalidNewName(string n)
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            prov.RenamePageAttachment(pi, "File.txt", n);
        }

        [Test]
        public void CompleteTestsForCaseInsensitivity_Attachments()
        {
            var prov = GetProvider();

            var page = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StorePageAttachment(page, "Attachment.TXT", s, false),
                    "StorePageAttachment should return true");
                Assert.IsFalse(prov.StorePageAttachment(page, "ATTACHMENT.txt", s, false),
                    "StorePageAttachment should return false");
            }

            Assert.IsNotNull(prov.GetPageAttachmentDetails(page, "attachment.txt"),
                "GetPageAttachmentDetails should return a value");

            var ms = new MemoryStream();
            Assert.IsTrue(prov.RetrievePageAttachment(page, "Attachment.txt", ms, false),
                "RetrievePageAttachment should return true");

            Assert.IsTrue(prov.RenamePageAttachment(page, "Attachment.txt", "NEWATT.txt"),
                "RenamePageAttachment should return true");

            Assert.IsTrue(prov.DeletePageAttachment(page, "newatt.TXT"), "DeletePageAttachment should return true");
        }

        [Test]
        public void CompleteTestsForCaseInsensitivity_Files()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StoreFile("/File.TXT", s, false), "StoreFile should return true");
                Assert.IsFalse(prov.StoreFile("/file.txt", s, false), "StoreFile should return false");
                prov.CreateDirectory("/", "Sub");
                Assert.IsTrue(prov.StoreFile("/Sub/File.TXT", s, false), "StoreFile should return true");
                Assert.IsFalse(prov.StoreFile("/SUB/File.TXT", s, false), "StoreFile should return false");
            }

            Assert.IsNotNull(prov.GetFileDetails("/file.TXT"), "GetFileDetails should return something");
            Assert.IsNotNull(prov.GetFileDetails("/suB/fILe.TXT"), "GetFileDetails should return something");

            var ms = new MemoryStream();
            Assert.IsTrue(prov.RetrieveFile("/FILE.tXt", ms, false), "RetrieveFile should return true");
            ms = new MemoryStream();
            Assert.IsTrue(prov.RetrieveFile("/SuB/FILe.tXt", ms, false), "RetrieveFile should return true");

            Assert.IsTrue(prov.RenameFile("/FILE.TXT", "/NEWfile.txt"), "RenameFile should return true");
            Assert.IsTrue(prov.RenameFile("/SUB/FILE.TXT", "/sub/NEWfile.txt"), "RenameFile should return true");

            Assert.IsTrue(prov.DeleteFile("/newfile.txt"), "DeleteFile should return true");
            Assert.IsTrue(prov.DeleteFile("/sub/newfile.txt"), "DeleteFile should return true");
        }

        [Test]
        public void CreateDirectory_ExistentName()
        {
            var prov = GetProvider();

            Assert.IsTrue(prov.CreateDirectory("/", "Dir"), "CreateDirectory should return true");
            Assert.Throws<ArgumentException>(() =>
            {
                prov.CreateDirectory("/", "Dir");
            });
        }

        [Test]
        public void CreateDirectory_InexistentDirectory()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.CreateDirectory("/Inexistent/Dir/", "Sub");
            });
        }

        [Test]
        public void CreateDirectory_ListDirectories()
        {
            var prov = GetProvider();

            Assert.IsTrue(prov.CreateDirectory("/", "Dir1"), "CreateDirectory should return true");
            Assert.IsTrue(prov.CreateDirectory("/", "Dir2"), "CreateDirectory should return true");
            Assert.IsTrue(prov.CreateDirectory("/Dir1", "Sub"), "CreateDirectory should return true");

            var dirs = prov.ListDirectories("/").ToList();
            Assert.AreEqual(2, dirs.Count(), "Wrong dir count");
            Assert.AreEqual("/Dir1/", dirs.First(), "Wrong dir");
            Assert.AreEqual("/Dir2/", dirs.Skip(1).First(), "Wrong dir");

            dirs = prov.ListDirectories("/Dir1/").ToList();
            Assert.AreEqual(1, dirs.Count(), "Wrong dir count"); ;
            Assert.AreEqual("/Dir1/Sub/", dirs[0], "Wrong dir");
        }

        [Test]
        public void CreateDirectory_NullDirectory()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.CreateDirectory(null, "Dir");
            });
        }

        [Test]
        public void DeleteDirectory()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Dir");
            prov.CreateDirectory("/", "Dir2");
            prov.CreateDirectory("/Dir/", "Sub");

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/Dir/File.txt", s, false);
            }
            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/Dir/Sub/File.txt", s, false);
            }

            Assert.IsTrue(prov.DeleteDirectory("/Dir"), "DeleteDirectory should return true");
            Assert.AreEqual("/Dir2/", prov.ListDirectories("/").First(), "Wrong directory");
            Assert.IsTrue(prov.DeleteDirectory("/Dir2"), "DeleteDirectory should return true");
            Assert.AreEqual(0, prov.ListDirectories("/").Count(), "Wrong dir count");
        }

        [Test]
        public void DeleteDirectory_InexistentDirectory()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.DeleteDirectory("/Inexistent/");
            });
        }

        [Test]
        public void DeleteFile()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Sub");

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
                prov.StoreFile("/Sub/File.txt", s, false);
            }

            Assert.IsTrue(prov.DeleteFile("/File.txt"), "DeleteFile should return true");
            Assert.IsTrue(prov.DeleteFile("/Sub/File.txt"), "DeleteFile should return true");
            Assert.AreEqual(0, prov.ListFiles("/").Count(), "Wrong file count");
            Assert.AreEqual(0, prov.ListFiles("/Sub/").Count(), "Wrong file count");
        }

        [Test]
        public void DeleteFile_InexistentFile()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.DeleteFile("/File.txt");
            });
        }

        [Test]
        public void DeletePageAttachment()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            Assert.IsTrue(prov.DeletePageAttachment(pi, "File.txt"), "DeletePageAttachment should return true");

            Assert.IsNull(prov.GetPageAttachmentDetails(pi, "File.txt"), "GetPageAttachmentDetails should return null");
            Assert.AreEqual(0, prov.ListPageAttachments(pi).Count, "Wrong attachment count");
        }

        [Test]
        public void DeletePageAttachment_InexistentName()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            Assert.Throws<ArgumentException>(() =>
            {
                prov.DeletePageAttachment(pi, "File222.txt");
            });
        }

        [Test]
        public void DeletePageAttachment_InexistentPage()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            Assert.Throws<ArgumentException>(() =>
            {
                prov.DeletePageAttachment(pi, "File.txt");
            });
        }

        [Test]
        public void DeletePageAttachment_NullPage()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.DeletePageAttachment(null, "File.txt");
            });
        }

        [Test]
        public void GetFileDetails_DeleteDirectory()
        {
            var prov = GetProvider();

            var now = DateTime.Now;
            prov.CreateDirectory("/", "Dir");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir/File.txt", s, false);
            }
            prov.CreateDirectory("/Dir/", "Sub");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir/Sub/File.txt", s, false);
            }
            prov.CreateDirectory("/", "Dir2");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir2/File.txt", s, false);
            }

            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir/File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir2/File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir/Sub/File.txt", s, true);
            }

            prov.DeleteDirectory("/Dir/");

            var details = prov.GetFileDetails("/Dir2/File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            Assert.IsNull(prov.GetFileDetails("/Dir/File.txt"), "GetFileDetails should return null");
            Assert.IsNull(prov.GetFileDetails("/Dir/Sub/File.txt"), "GetFileDetails should return null");
        }

        [Test]
        public void GetFileDetails_InexistentFile()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetFileDetails("/Inexistent.txt"), "GetFileDetails should return null");
        }

        [Test]
        public void GetFileDetails_RenameDirectory()
        {
            var prov = GetProvider();

            var now = DateTime.Now;
            prov.CreateDirectory("/", "Dir");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir/File.txt", s, false);
            }
            prov.CreateDirectory("/Dir/", "Sub");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir/Sub/File.txt", s, false);
            }
            prov.CreateDirectory("/", "Dir100");
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/Dir100/File.txt", s, false);
            }

            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir/File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir/Sub/File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/Dir100/File.txt", s, true);
            }

            prov.RenameDirectory("/Dir/", "/Dir2/");

            FileDetails details;

            details = prov.GetFileDetails("/Dir100/File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            Assert.IsNull(prov.GetFileDetails("/Dir/File.txt"), "GetFileDetails should return null");

            details = prov.GetFileDetails("/Dir2/File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            Assert.IsNull(prov.GetFileDetails("/Dir/Sub/File.txt"), "GetFileDetails should return null");

            details = prov.GetFileDetails("/Dir2/Sub/File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void GetFileDetails_RenameFile()
        {
            var prov = GetProvider();

            var now = DateTime.Now;
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/File.txt", s, false);
            }

            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/File.txt", s, true);
            }

            prov.RenameFile("/File.txt", "/File2.txt");

            var details = prov.GetFileDetails("/File.txt");
            Assert.IsNull(details, "GetFileDetails should return null");

            details = prov.GetFileDetails("/File2.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void GetFileDetails_SetFileRetrievalCount()
        {
            var prov = GetProvider();

            var now = DateTime.Now;
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/File.txt", s, false);
            }
            using (var s = FillStream("Content"))
            {
                prov.StoreFile("/File2.txt", s, false);
            }

            var details = prov.GetFileDetails("/File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/File.txt", s, false);
            }
            details = prov.GetFileDetails("/File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/File2.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrieveFile("/File.txt", s, true);
            }
            details = prov.GetFileDetails("/File.txt");
            Assert.AreEqual(2, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetFileDetails("/File2.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            prov.SetFileRetrievalCount("/File2.txt", 0);

            details = prov.GetFileDetails("/File.txt");
            Assert.AreEqual(2, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetFileDetails("/File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            prov.DeleteFile("/File.txt");

            Assert.IsNull(prov.GetFileDetails("/File.txt"), "GetFileDetails should return null");

            details = prov.GetFileDetails("/File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong file retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong file size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void GetPageAttachmentDetails_InexistentAttachment()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetPageAttachmentDetails(new PageInfo("Inexistent", null, DateTime.Now), "File.txt"),
                "GetPageAttachmentDetails should retur null");
        }

        [Test]
        public void GetPageAttachmentDetails_NotifyPageRenaming()
        {
            var prov = GetProvider();

            var page = new PageInfo("Page", null, DateTime.Now);
            var page2 = new PageInfo("Page2", null, DateTime.Now);
            var newPage = new PageInfo("newPage", null, DateTime.Now);

            var now = DateTime.Now;
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page, "File.txt", s, false);
            }
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page2, "File.txt", s, false);
            }

            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page, "File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page2, "File.txt", s, true);
            }

            prov.NotifyPageRenaming(page, newPage);

            FileDetails details;

            Assert.IsNull(prov.GetPageAttachmentDetails(page, "File.txt"), "GetPageAttachmentDetails should return null");

            details = prov.GetPageAttachmentDetails(page2, "File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(newPage, "File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void GetPageAttachmentDetails_NullPage()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.GetPageAttachmentDetails(null, "File.txt");
            });
        }

        [Test]
        public void GetPageAttachmentDetails_RenamePageAttachment()
        {
            var prov = GetProvider();

            var page = new PageInfo("Page", null, DateTime.Now);

            var now = DateTime.Now;
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page, "File.txt", s, false);
            }

            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page, "File.txt", s, true);
            }

            prov.RenamePageAttachment(page, "File.txt", "File2.txt");

            FileDetails details;

            Assert.IsNull(prov.GetPageAttachmentDetails(page, "File.txt"), "GetPageAttachmentDetails should return null");

            details = prov.GetPageAttachmentDetails(page, "File2.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void GetPageAttachmentDetails_SetPageAttachmentRetrievalCount()
        {
            var prov = GetProvider();

            var page1 = new PageInfo("Page1", null, DateTime.Now);
            var page2 = new PageInfo("Page2", null, DateTime.Now);

            var now = DateTime.Now;
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page1, "File.txt", s, false);
            }
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page2, "File.txt", s, false);
            }
            using (var s = FillStream("Content"))
            {
                prov.StorePageAttachment(page1, "File2.txt", s, false);
            }

            FileDetails details;

            details = prov.GetPageAttachmentDetails(page1, "File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page2, "File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page1, "File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page1, "File.txt", s, false);
            }
            details = prov.GetPageAttachmentDetails(page1, "File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page1, "File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page1, "File.txt", s, true);
            }
            using (Stream s = new MemoryStream())
            {
                prov.RetrievePageAttachment(page2, "File.txt", s, true);
            }

            details = prov.GetPageAttachmentDetails(page1, "File.txt");
            Assert.AreEqual(2, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page2, "File.txt");
            Assert.AreEqual(1, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page1, "File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            prov.SetPageAttachmentRetrievalCount(page2, "File.txt", 0);

            details = prov.GetPageAttachmentDetails(page1, "File.txt");
            Assert.AreEqual(2, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page2, "File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page1, "File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            prov.DeletePageAttachment(page1, "File.txt");

            Assert.IsNull(prov.GetPageAttachmentDetails(page1, "File.txt"),
                "GetPageAttachmentDetails should return null");

            details = prov.GetPageAttachmentDetails(page2, "File.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);

            details = prov.GetPageAttachmentDetails(page1, "File2.txt");
            Assert.AreEqual(0, details.RetrievalCount, "Wrong attachment retrieval count");
            Assert.AreEqual(7, details.Size, "Wrong attachment size");
            Tools.AssertDateTimesAreEqual(now, details.LastModified, true);
        }

        [Test]
        public void Init_NullConfig()
        {
            var prov = GetProvider();
            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.Init(MockHost(), null);
            });
        }

        [Test]
        public void Init_NullHost()
        {
            var prov = GetProvider();
            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.Init(null, "");
            });
        }

        [Test]
        public void ListDirectories_InexistentDirectory()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.ListDirectories("/Inexistent/");
            });
        }

        [Test]
        public void ListDirectories_NullOrEmptyDirectory()
        {
            var prov = GetProvider();

            var dirs = prov.ListDirectories(null);
            Assert.AreEqual(0, dirs.Count(), "Wrong dir count");

            prov.CreateDirectory("/", "Dir");

            dirs = prov.ListDirectories(null);
            Assert.AreEqual(1, dirs.Count(), "Wrong dir count");
            Assert.AreEqual("/Dir/", dirs.First(), "Wrong dir");

            dirs = prov.ListDirectories("");
            Assert.AreEqual(1, dirs.Count(), "Wrong dir count");
            Assert.AreEqual("/Dir/", dirs.First(), "Wrong dir");
        }

        [Test]
        public void ListFiles_InexistentDirectory()
        {
            var prov = GetProvider();
            Assert.Throws<ArgumentException>(() =>
            {
                prov.ListFiles("/dir/that/does/not/exist");
            });
        }

        [Test]
        public void ListFiles_NullOrEmptyDirectory()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StoreFile("/File.txt", s, false), "StoreFile should return true");
            }

            var files = prov.ListFiles(null);
            Assert.AreEqual(1, files.Count(), "Wrong file count");
            Assert.AreEqual("/File.txt", files.First(), "Wrong file");

            files = prov.ListFiles("");
            Assert.AreEqual(1, files.Count(), "Wrong file count");
            Assert.AreEqual("/File.txt", files.First(), "Wrong file");
        }

        [Test]
        public void ListPageAttachments_InexistentPage()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.ListPageAttachments(new PageInfo("Page", MockPagesProvider(), DateTime.Now)).Count,
                "Wrong attachment count");
        }

        [Test]
        public void ListPageAttachments_NullPage()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.ListPageAttachments(null);
            });
        }

        [Test]
        public void NotifyPageRenaming()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(p1, "File1.txt", s, false);
                prov.StorePageAttachment(p1, "File2.txt", s, false);
            }

            prov.NotifyPageRenaming(p1, p2);

            Assert.AreEqual(0, prov.ListPageAttachments(p1).Count(), "Wrong attachment count");

            var attachs = prov.ListPageAttachments(p2);
            Assert.AreEqual(2, attachs.Count(), "Wrong attachment count");
            Assert.AreEqual("File1.txt", attachs[0], "Wrong attachment");
            Assert.AreEqual("File2.txt", attachs[1], "Wrong attachment");
        }

        [Test]
        public void NotifyPageRenaming_ExistentNewPage()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(p1, "File1.txt", s, false);
                prov.StorePageAttachment(p2, "File2.txt", s, false);
            }

            Assert.Throws<ArgumentException>(() =>
            {
                prov.NotifyPageRenaming(p1, p2);
            });
        }

        [Test]
        public void NotifyPageRenaming_InexistentOldPage()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            prov.NotifyPageRenaming(p1, p2);

            // Nothing specific to verify
        }

        [Test]
        public void NotifyPageRenaming_NullNewPage()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.NotifyPageRenaming(p1, null);
            });
        }

        [Test]
        public void NotifyPageRenaming_NullOldPage()
        {
            var prov = GetProvider();

            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.NotifyPageRenaming(null, p2);
            });
        }

        [Test]
        public void RenameDirectory()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Dir");
            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/Dir/File.txt", s, false);
            }

            Assert.IsTrue(prov.RenameDirectory("/Dir/", "/Dir2/"), "RenameDirectory should return true");

            Assert.AreEqual("/Dir2/", prov.ListDirectories("/").First(), "Wrong directory");

            var thisHouldBeTrue = false;
            try
            {
                Assert.AreEqual(0, prov.ListFiles("/Dir/").Count(), "Wrong file count");
            }
            catch (ArgumentException)
            {
                thisHouldBeTrue = true;
            }
            Assert.IsTrue(thisHouldBeTrue, "ListFiles did not throw an exception");
            Assert.AreEqual("/Dir2/File.txt", prov.ListFiles("/Dir2/").First(), "Wrong file");

            prov.CreateDirectory("/Dir2/", "Sub");
            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/Dir2/Sub/File.txt", s, false);
            }

            Assert.IsTrue(prov.RenameDirectory("/Dir2/Sub/", "/Dir2/Sub2/"), "RenameDirectory should return true");

            Assert.AreEqual("/Dir2/Sub2/", prov.ListDirectories("/Dir2/").First(), "Wrong dir");

            thisHouldBeTrue = false;
            try
            {
                Assert.AreEqual(0, prov.ListFiles("/Dir/Sub/").Count(), "Wrong file count");
            }
            catch (ArgumentException)
            {
                thisHouldBeTrue = true;
            }
            Assert.IsTrue(thisHouldBeTrue, "ListFiles did not throw an exception");
            Assert.AreEqual("/Dir2/Sub2/File.txt", prov.ListFiles("/Dir2/Sub2/").First(), "Wrong file");
        }

        [Test]
        public void RenameDirectory_ExistentNewDir()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Dir");
            prov.CreateDirectory("/", "Dir2");

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenameDirectory("/Dir/", "/Dir2/");
            });
        }

        [Test]
        public void RenameDirectory_InexistentDirectory()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenameDirectory("/Inexistent/", "/Inexistent2/");
            });
        }

        [Test]
        public void RenameFile()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Sub");

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
                prov.StoreFile("/Sub/File.txt", s, false);
            }

            Assert.IsTrue(prov.RenameFile("/File.txt", "/File2.txt"), "RenameFile should return true");
            Assert.IsTrue(prov.RenameFile("/Sub/File.txt", "/Sub/File2.txt"), "RenameFile should return true");

            var files = prov.ListFiles("/");
            Assert.AreEqual(1, files.Count(), "Wrong file count");
            Assert.AreEqual("/File2.txt", files.First(), "Wrong file");

            files = prov.ListFiles("/Sub/");
            Assert.AreEqual(1, files.Count(), "Wrong file count");
            Assert.AreEqual("/Sub/File2.txt", files.First(), "Wrong file");
        }

        [Test]
        public void RenameFile_ExistentName()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
                prov.StoreFile("/File2.txt", s, false);
            }

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenameFile("/File.txt", "/File2.txt");
            });
        }

        [Test]
        public void RenameFile_InexistentFile()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenameFile("/Blah.txt", "/Blah2.txt");
            });
        }

        [Test]
        public void RenamePageAttachment()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            Assert.IsTrue(prov.RenamePageAttachment(pi, "File.txt", "File2.txt"),
                "RenamePageAttachment should return true");

            var attachs = prov.ListPageAttachments(pi);
            Assert.AreEqual(1, attachs.Count, "Wrong attachment count");
            Assert.AreEqual("File2.txt", attachs[0], "Wrong attachment");
        }

        [Test]
        public void RenamePageAttachment_ExistentNewName()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
                prov.StorePageAttachment(pi, "File2.txt", s, false);
            }

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenamePageAttachment(pi, "File.txt", "File2.txt");
            });
        }

        [Test]
        public void RenamePageAttachment_InexistentName()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenamePageAttachment(pi, "File1.txt", "File2.txt");
            });
        }

        [Test]
        public void RenamePageAttachment_InexistentPage()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            Assert.Throws<ArgumentException>(() =>
            {
                prov.RenamePageAttachment(pi, "File.txt", "File2.txt");
            });
        }

        [Test]
        public void RenamePageAttachment_NullPage()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.RenamePageAttachment(null, "File.txt", "File2.txt");
            });
        }

        [Test]
        public void RetrieveFile_ClosedStream()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
            }

            var s2 = new MemoryStream();
            s2.Close();
            Assert.Throws<ArgumentException>(() =>
            {
                prov.RetrieveFile("/File.txt", s2, false);
            });
        }

        [Test]
        public void RetrieveFile_InexistentFile()
        {
            var prov = GetProvider();

            using (var s = new MemoryStream())
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    prov.RetrieveFile("/Inexistent.txt", s, false);
                });
            }
        }

        [Test]
        public void RetrieveFile_NullStream()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                prov.StoreFile("/File.txt", s, false);
            }

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.RetrieveFile("/File.txt", null, false);
            });
        }

        [Test]
        public void RetrievePageAttachment_ClosedStream()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            var ms = new MemoryStream();
            ms.Close();
            Assert.Throws<ArgumentException>(() =>
            {
                prov.RetrievePageAttachment(pi, "File.txt", ms, false);
            });
        }

        [Test]
        public void RetrievePageAttachment_InexistentName()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var ms = new MemoryStream())
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    prov.RetrievePageAttachment(pi, "File.txt", ms, false);
                });
            }
        }

        [Test]
        public void RetrievePageAttachment_InexistentPage()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            pi = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            using (var ms = new MemoryStream())
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    prov.RetrievePageAttachment(pi, "File.txt", ms, false);
                });
            }
        }

        [Test]
        public void RetrievePageAttachment_NullPage()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            using (var ms = new MemoryStream())
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    prov.RetrievePageAttachment(null, "File.txt", ms, false);
                });
            }
        }

        [Test]
        public void RetrievePageAttachment_NullStream()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                prov.StorePageAttachment(pi, "File.txt", s, false);
            }

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.RetrievePageAttachment(pi, "File.txt", null, false);
            });
        }

        [Test]
        public void SetFileRetrievalCount_NegativeCount()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.SetFileRetrievalCount("/File.txt", -1);
            });
        }

        [Test]
        public void SetPageAttachmentRetrievalCount_NegativeCount()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.SetPageAttachmentRetrievalCount(new PageInfo("Page", null, DateTime.Now), "File.txt", -1);
            });
        }

        [Test]
        public void SetPageAttachmentRetrievalCount_NullPage()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.SetPageAttachmentRetrievalCount(null, "File.txt", 10);
            });
        }

        [Test]
        public void StoreFile_ClosedStream()
        {
            var prov = GetProvider();

            var s = FillStream("Blah");
            s.Close();

            Assert.Throws<ArgumentException>(() =>
            {
                prov.StoreFile("Blah.txt", s, false);
            });
        }

        [Test]
        public void StoreFile_ListFiles()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.ListFiles("/").Count(), "Wrong file count");

            using (var s = FillStream("File1"))
            {
                Assert.IsTrue(prov.StoreFile("/File1.txt", s, false), "StoreFile should return true");
            }
            using (var s = FillStream("File2"))
            {
                Assert.IsTrue(prov.StoreFile("/File2.txt", s, true), "StoreFile should return true");
            }

            var files = prov.ListFiles("/");
            Assert.AreEqual(2, files.Count(), "Wrong file count");
            Assert.AreEqual("/File1.txt", files.First(), "Wrong file");
            Assert.AreEqual("/File2.txt", files.Skip(1).First(), "Wrong file");
        }

        [Test]
        public void StoreFile_NullStream()
        {
            var prov = GetProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.StoreFile("/Blah.txt", null, false);
            });
        }

        [Test]
        public void StoreFile_Overwrite_RetrieveFile()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StoreFile("/File.txt", s, false), "StoreFile should return true");
            }

            using (var s = FillStream("Blah222"))
            {
                Assert.IsFalse(prov.StoreFile("/File.txt", s, false), "StoreFile should return false");
            }

            var ms = new MemoryStream();
            prov.RetrieveFile("/File.txt", ms, false);
            ms.Seek(0, SeekOrigin.Begin);
            var c = Encoding.UTF8.GetString(ms.ToArray());
            Assert.AreEqual("Blah", c, "Wrong content (seems modified");

            using (var s = FillStream("Blah222"))
            {
                Assert.IsTrue(prov.StoreFile("/File.txt", s, true), "StoreFile should return true");
            }

            ms = new MemoryStream();
            prov.RetrieveFile("/File.txt", ms, false);
            ms.Seek(0, SeekOrigin.Begin);
            c = Encoding.UTF8.GetString(ms.ToArray());
            Assert.AreEqual("Blah222", c, "Wrong content (seems modified");
        }

        [Test]
        public void StoreFile_SubDir()
        {
            var prov = GetProvider();

            prov.CreateDirectory("/", "Test");

            Assert.AreEqual(0, prov.ListFiles("/Test").Count(), "Wrong file count");

            using (var s = FillStream("File1"))
            {
                Assert.IsTrue(prov.StoreFile("/Test/File1.txt", s, false), "StoreFile should return true");
            }
            using (var s = FillStream("File2"))
            {
                Assert.IsTrue(prov.StoreFile("/Test/File2.txt", s, true), "StoreFile should return true");
            }

            var files = prov.ListFiles("/Test");
            Assert.AreEqual(2, files.Count(), "Wrong file count");
            Assert.AreEqual("/Test/File1.txt", files.First(), "Wrong file");
            Assert.AreEqual("/Test/File2.txt", files.Skip(1).First(), "Wrong file");
        }

        [Test]
        public void StorePageAttachment_ClosedStream()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            var ms = new MemoryStream();
            ms.Close();
            Assert.Throws<ArgumentException>(() =>
            {
                prov.StorePageAttachment(pi, "File.txt", ms, false);
            });
        }

        [Test]
        public void StorePageAttachment_ExistentName()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StorePageAttachment(pi, "File.txt", s, false),
                    "StorePageAttachment should return true");
                Assert.IsFalse(prov.StorePageAttachment(pi, "File.txt", s, false),
                    "StorePageAttachment should return false");
            }
        }

        [Test]
        public void StorePageAttachment_ListPageAttachments_GetPagesWithAttachments()
        {
            var prov = GetProvider();

            var pi1 = new PageInfo("MainPage", null, DateTime.Now);
            var pi2 = new PageInfo("Page2", null, DateTime.Now);

            Assert.AreEqual(0, prov.ListPageAttachments(pi1).Count, "Wrong attachment count");

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StorePageAttachment(pi1, "File.txt", s, false),
                    "StorePageAttachment should return true");
                Assert.IsTrue(prov.StorePageAttachment(pi1, "File2.txt", s, false),
                    "StorePageAttachment should return true");
                Assert.IsTrue(prov.StorePageAttachment(pi2, "File.txt", s, false),
                    "StorePageAttachment should return true");
            }

            var attachs = prov.ListPageAttachments(pi1);
            Assert.AreEqual(2, attachs.Count, "Wrong attachment count");
            Assert.AreEqual("File.txt", attachs[0], "Wrong attachment");
            Assert.AreEqual("File2.txt", attachs[1], "Wrong attachment");

            attachs = prov.ListPageAttachments(pi2);
            Assert.AreEqual(1, attachs.Count, "Wrong attachment count");
            Assert.AreEqual("File.txt", attachs[0], "Wrong attachment");

            var pages = prov.GetPagesWithAttachments();
            Assert.AreEqual(2, pages.Count(), "Wrong page count");
            Assert.AreEqual(pi1.FullName, pages.First(), "Wrong page");
            Assert.AreEqual(pi2.FullName, pages.Skip(1).First(), "Wrong page");
        }

        [Test]
        public void StorePageAttachment_NullPage()
        {
            var prov = GetProvider();

            using (var s = FillStream("Blah"))
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    prov.StorePageAttachment(null, "File.txt", s, false);
                });
            }
        }

        [Test]
        public void StorePageAttachment_NullStream()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.StorePageAttachment(pi, "File.txt", null, false);
            });
        }

        [Test]
        public void StorePageAttachment_Overwrite_RetrievePageAttachment()
        {
            var prov = GetProvider();

            var pi = new PageInfo("Page", MockPagesProvider(), DateTime.Now);

            using (var s = FillStream("Blah"))
            {
                Assert.IsTrue(prov.StorePageAttachment(pi, "File.txt", s, false),
                    "StorePageAttachment should return true");
            }

            using (var s = FillStream("Blah222"))
            {
                Assert.IsTrue(prov.StorePageAttachment(pi, "File.txt", s, true),
                    "StorePageAttachment should return true");
            }

            var ms = new MemoryStream();
            Assert.IsTrue(prov.RetrievePageAttachment(pi, "File.txt", ms, false),
                "RetrievePageAttachment should return true");
            ms.Seek(0, SeekOrigin.Begin);

            Assert.AreEqual("Blah222", Encoding.UTF8.GetString(ms.ToArray()), "Wrong attachment content");
        }
    }
}