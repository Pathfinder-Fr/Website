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
    public class UserRolePlaceHolder : CompositeControl, INamingContainer
    {

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            SueetieUser user = SueetieContext.Current.User;
       
            if (user.IsInRole(Role.ToString()) || (user.IsAnonymous && this.Role == UserRole.NonMember))
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

        public UserRole Role
        {
            get { return (UserRole)(ViewState["Role"] ?? UserRole.NonMember); }
            set { ViewState["Role"] = value; }
        }

        [TemplateContainer(typeof(SueetieUser)), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate TrueContentTemplate { get; set; }

        [TemplateContainer(typeof(SueetieUser)), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FalseContentTemplate { get; set; }

        public enum UserRole
        {
            Registered,
            BlogAdministrator,
            MediaAdministrator,
            SueetieAdministrator,
            ForumAdministrator,
            WikiAdministrator,
            ContentAdministrator,
            NonMember
        }

    }



}

