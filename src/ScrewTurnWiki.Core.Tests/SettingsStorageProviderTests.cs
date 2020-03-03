﻿using NUnit.Framework;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    public class SettingsStorageProviderTests : SettingsStorageProviderTestScaffolding
    {
        public override ISettingsStorageProviderV30 GetProvider()
        {
            var prov = new SettingsStorageProvider();
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