using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminCalendarEdit : SueetieAdminPage
    {
        public AdminCalendarEdit()
            : base("admin_content_calendaredit")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearForm();
            }
        }


        protected void ddlCalendars_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SueetieCalendar sueetieCalendar = SueetieCalendars.GetSueetieCalendar(int.Parse(ddlCalendars.SelectedValue));
            if (sueetieCalendar.CalendarID > 0)
            {
                txtTitle.Text = sueetieCalendar.CalendarTitle;
                txtDescription.Text = sueetieCalendar.CalendarDescription;
                txtCalendarUrl.Text = sueetieCalendar.CalendarUrl;
                chkActive.Checked = sueetieCalendar.IsActive;
                SetButtonState(false);
            }
            else
            {
                SetButtonState(true);
                ClearForm();
            }
        }

        private void SetButtonState(bool forNew)
        {
            if (forNew)
            {
                btnAddNew.Enabled = true;
                btnUpdate.Enabled = false;
                btnManage.Enabled = false;
            }
            else
            {
                btnAddNew.Enabled = false;
                btnUpdate.Enabled = true;
                btnManage.Enabled = true;
            }
        }


        protected void btnAddUpdate_OnCommand(object sender, CommandEventArgs e)
        {
            SueetieCalendar sueetieCalendar = new SueetieCalendar
            {
                CalendarTitle = txtTitle.Text,
                CalendarDescription = txtDescription.Text,
                CalendarUrl = txtCalendarUrl.Text
            };
            if (e.CommandName == "Add")
            {
                if (ddlCalendars.Items.Count > 0)
                {
                    if (int.Parse(ddlCalendars.SelectedValue) < 0)
                    {
                        SueetieCalendars.CreateSueetieCalendar(sueetieCalendar);
                        lblResults.Text = "Calendar Created!";
                    }
                    else
                    {
                        lblResults.Text = "Calendar dropdown selector must be empty when creating a calendar.";
                        return;
                    }
                }
                else
                {
                    SueetieCalendars.CreateSueetieCalendar(sueetieCalendar);
                    lblResults.Text = "Calendar Created!";
                }
            }
            else
            {
                sueetieCalendar.CalendarID = int.Parse(ddlCalendars.SelectedValue);
                sueetieCalendar.IsActive = chkActive.Checked;
                SueetieCalendars.UpdateSueetieCalendar(sueetieCalendar);
                lblResults.Text = "Calendar Updated!";
            }
            SueetieCalendars.ClearSueetieCalendarListCache();
            ClearForm();
        }

        private void ClearForm()
        {
            List<SueetieCalendar> sueetieCalendars = SueetieCalendars.GetSueetieCalendarList();

            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtCalendarUrl.Text = string.Empty;
            chkActive.Checked = true;

            ddlCalendars.DataSource = sueetieCalendars;
            ddlCalendars.DataValueField = "CalendarID";
            ddlCalendars.DataTextField = "CalendarTitle";

            ddlCalendars.DataBind();
            ddlCalendars.Items.Insert(0, new ListItem(string.Empty, "-1"));
            ddlCalendars.Items.FindByValue("-1").Selected = true;

            SetButtonState(true);

        }

        protected void btnManage_OnClick(object sender, EventArgs e)
        {
            if (int.Parse(ddlCalendars.SelectedValue) > 0)
                Response.Redirect("calendarscheduler.aspx?c=" + ddlCalendars.SelectedValue);
            else
                lblResults.Text = "Please select a Calendar to manage.";
        }
    }
}