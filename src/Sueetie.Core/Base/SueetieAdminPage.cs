// -----------------------------------------------------------------------
// <copyright file="SueetieAdminPage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    public class SueetieAdminPage : SueetieBasePage
    {
        public SueetieAdminPage()
        {
        }

        public SueetieAdminPage(string pageTitle)
            : base(pageTitle)
        {
        }

        protected override void OnPreInit(EventArgs e)
        {
            this.MasterPageFile = "\\themes\\" + SueetieConfiguration.Get().Core.AdminTheme + "\\masters\\admin.master";
            base.OnPreInit(e);
        }
    }
}