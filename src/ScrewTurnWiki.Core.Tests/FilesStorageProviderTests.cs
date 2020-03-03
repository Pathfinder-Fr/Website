using NUnit.Framework;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    public class FilesStorageProviderTests : FilesStorageProviderTestScaffolding
    {
        public override IFilesStorageProviderV30 GetProvider()
        {
            var prov = new FilesStorageProvider();
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
    }
}