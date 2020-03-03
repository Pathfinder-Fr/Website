using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;

namespace Sueetie.Web 
{
    public partial class RoleAdmin : SueetieAdminPage
    {
        public RoleAdmin()
            : base("admin_users_roleadmin")
        {
        }
        private bool createRoleSuccess = true;


        public void AddRole(object sender, EventArgs e)
        {
            try
            {
                Roles.CreateRole(NewRole.Text);
                SueetieRole sueetieRole = new SueetieRole
                {
                    RoleName = NewRole.Text,
                    RoleID = SueetieRoles.GetAspnetRoleID(NewRole.Text),
                    IsGroupAdminRole = chkIsGroupAdminRole.Checked,
                    IsGroupUserRole = chkIsGroupUserRole.Checked,
                    IsBlogOwnerRole = chkIsBlogOwnerRole.Checked
                };
                SueetieRoles.CreateSueetieRole(sueetieRole);
                SueetieRoles.ClearRolesListCache();
                ActivitiesDataSource.Select();
                UserRoles.DataBind();

                createRoleSuccess = true;
            }
            catch
            {
                createRoleSuccess = false;
            }
            NewRole.Text = string.Empty;
            chkIsGroupAdminRole.Checked = false;
            chkIsGroupUserRole.Checked = false;
            chkIsBlogOwnerRole.Checked = false;
        }


        protected void UserRoles_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = UserRoles.Rows[e.RowIndex];
            SueetieRole sueetieRole = new SueetieRole
            {
                IsGroupAdminRole = ((CheckBox)(row.Cells[2].Controls[1])).Checked,
                IsGroupUserRole = ((CheckBox)(row.Cells[3].Controls[1])).Checked,
                IsBlogOwnerRole = ((CheckBox)(row.Cells[4].Controls[1])).Checked
            };

            e.NewValues.Remove("RoleID");
            e.NewValues.Add("isGroupAdminRole", sueetieRole.IsGroupAdminRole);
            e.NewValues.Add("isGroupUserRole", sueetieRole.IsGroupUserRole);
            e.NewValues.Add("isBlogOwnerRole", sueetieRole.IsBlogOwnerRole);
        }
    }

}
