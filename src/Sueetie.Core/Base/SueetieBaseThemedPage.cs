// -----------------------------------------------------------------------
// <copyright file="SueetieBaseThemedPage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    public class SueetieBaseThemedPage : SueetieBasePage
    {
        public SueetieBaseThemedPage()
        {
        }

        public SueetieBaseThemedPage(string _pageKey)
            : base(_pageKey)
        {
        }

        public string SueetieMasterPage { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            var _sueetieMasterPage = this.SueetieMasterPage ?? "sueetie.master";
            this.MasterPageFile = "\\themes\\" + SueetieContext.Current.Theme + "\\masters\\" + _sueetieMasterPage;
            base.OnPreInit(e);
        }
    }
}