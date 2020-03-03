using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using AttributeCollection = System.Web.UI.AttributeCollection;

using Sueetie.Core;

namespace Sueetie.Controls
{
    /// <summary>
    /// Shows content based on UserRole
    /// </summary>
    public class MultiRolePlaceHolder : CompositeControl, INamingContainer
    {

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            SueetieUser user = SueetieContext.Current.User;
       
            if (IsInViewableRole(user) || (user.IsAnonymous && this.IsAnonymous == IsAnonymousPlaceHolder.True))
            {
                TrueContentTemplate.InstantiateIn(this);
                base.Render(writer);
            }
            else
            {
                if (FalseContentTemplate != null)
                {
                    FalseContentTemplate.InstantiateIn(this);
                    base.Render(writer);
                }
                else
                    this.Visible = false;
            }
        }

        private bool IsInViewableRole(SueetieUser _user)
        {
            bool _isInViewableRole = false;
            if (this.Roles != null)
            {
                string[] rolesList = this.Roles.Split(',');
                foreach (string role in rolesList)
                {
                    if (_user.IsInRole(role))
                        _isInViewableRole = true;
                }
            }
            return _isInViewableRole;
        }

        public IsAnonymousPlaceHolder IsAnonymous
        {
            get { return (IsAnonymousPlaceHolder)(ViewState["IsAnonymous"] ?? IsAnonymousPlaceHolder.False); }
            set { ViewState["IsAnonymous"] = value; }
        }

        public string Roles
        {
            get { return (string)(ViewState["Roles"]); }
            set { ViewState["Roles"] = value; }
        }

        [TemplateContainer(typeof(SueetieUser)), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate TrueContentTemplate { get; set; }

        [TemplateContainer(typeof(SueetieUser)), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FalseContentTemplate { get; set; }

        public enum IsAnonymousPlaceHolder
        {
            True,
            False
        }

    }



}

