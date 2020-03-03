
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Sueetie.Core;
using System.Runtime.Serialization;

namespace Sueetie.Controls
{
    public class CalendarControl : SueetieBaseControl
    {

        #region General Properties


        private string _existingStartDate;
        private string _existingEndDate;
        private string _existingEndRepeatDate;
        private string _exisitngStartTime;
        private string _exisitngEndTime;

        private HtmlGenericControl _panel;
        private TextBox _txtEventTitle;
        private TextBox _txtEventDescription;
        private TextBox _calendarStart;
        private TextBox _calendarEnd;
        private TextBox _calendarRepeatEnd;

        private TextBox _eventStartTime;
        private TextBox _eventEndTime;

        private Label _lblNotes;
        private Label _lblRepeatDate;
        private Label _lblStartEndTime;
        private Label _lblStartDate;

        private ImageButton _imageButton;
        private bool _DisplayCalendarPanel = true;
        private bool _LoadScriptLibraries = true;
        private int _sourceContentID = -1;
        private bool _isSueetieMediaObject = false;
        private bool _isSueetieMediaAlbum = false;
        public SueetieWikiPage CalendarSueetieWikiPage = null;
        public SueetieMediaObject CalendarSueetieMediaObject = null;
        public SueetieMediaAlbum CalendarSueetieMediaAlbum = null;
        public SueetieForumTopic CalendarSueetieForumTopic = null;
        public SueetieBlogPost CalendarSueetieBlogPost = null;

        public SueetieUser CalendarSueetieUser { get; set; }
        public int SourceContentID
        {
            get { return _sourceContentID; }
            set { _sourceContentID = value; }
        }

        public bool DisplayCalendarsPanel
        {
            get { return _DisplayCalendarPanel; }
            set { _DisplayCalendarPanel = value; }
        }
        public bool LoadScriptLibraries
        {
            get { return _LoadScriptLibraries; }
            set { _LoadScriptLibraries = value; }
        }

        private bool _useJqueryPrefix = false;
        public bool UseJqueryPrefix
        {
            get { return _useJqueryPrefix; }
            set { _useJqueryPrefix = value; }
        }
        public string EditImageUrl
        {
            get { return editImageUrl; }
            set { editImageUrl = value; }
        }
        public string Roles { get; set; }
        public string Permalink { get; set; }
        public string SourceTitle { get; set; }
        #endregion

        #region Styling Properties

        public string EditImageCssClass
        {
            get { return editImageCssClass; }
            set { editImageCssClass = value; }
        }
        public string EventTitleTextBoxCssClass
        {
            get { return eventTitleTextBoxCssClass; }
            set { eventTitleTextBoxCssClass = value; }
        }
        public string EventDescriptionTextAreaCssClass
        {
            get { return eventDescriptionTextAreaCssClass; }
            set { eventDescriptionTextAreaCssClass = value; }
        }
        public string EventStartDateTextBoxCssClass
        {
            get { return eventStartDateTextBoxCssClass; }
            set { eventStartDateTextBoxCssClass = value; }
        }
        public string EventEndDateTextBoxCssClass
        {
            get { return eventEndDateTextBoxCssClass; }
            set { eventEndDateTextBoxCssClass = value; }
        }
        public string EventStartTimeTextBoxCssClass
        {
            get { return eventStartTimeTextBoxCssClass; }
            set { eventStartTimeTextBoxCssClass = value; }
        }
        public string EventEndTimeTextBoxCssClass
        {
            get { return eventEndTimeTextBoxCssClass; }
            set { eventEndTimeTextBoxCssClass = value; }
        }
        public string EditPanelCssClass
        {
            get { return editPanelCssClass; }
            set { editPanelCssClass = value; }
        }
        public string EditNotesAreaCssClass
        {
            get { return editNotesAreaCssClass; }
            set { editNotesAreaCssClass = value; }
        }
        public string EditRepeatDateCssClass
        {
            get { return editRepeatDateCssClass; }
            set { editRepeatDateCssClass = value; }
        }
        public string EditStartEndTimeCssClass
        {
            get { return editStartEndTimeCssClass; }
            set { editStartEndTimeCssClass = value; }
        }
        public string EditStartEndTimeDivCssClass
        {
            get { return editStartEndTimeDivCssClass; }
            set { editStartEndTimeDivCssClass = value; }
        }
        public string EditStartDateDivCssClass
        {
            get { return editStartDateDivCssClass; }
            set { editStartDateDivCssClass = value; }
        }
        public string EditButtonAreaCssClass
        {
            get { return editButtonAreaCssClass; }
            set { editButtonAreaCssClass = value; }
        }
        public string EditStartDateLabelCssClass
        {
            get { return editStartDateLabelCssClass; }
            set { editStartDateLabelCssClass = value; }
        }
        public string EventRepeatEndDateTextBoxCssClass
        {
            get { return eventRepeatEndDateTextBoxCssClass; }
            set { eventRepeatEndDateTextBoxCssClass = value; }
        }

        public string RepeatEndDateDivCssClass
        {
            get { return repeatEndDateDivCssClass; }
            set { repeatEndDateDivCssClass = value; }
        }
        private string editImageUrl = "/themes/" + SiteSettings.Instance.Theme + "/images/calendarEdit.png";
        private string editImageCssClass = "calendarEditToggleImage";
        private string eventTitleTextBoxCssClass = "calendarEditTitleTextBox";
        private string eventDescriptionTextAreaCssClass = "calendarEditDescriptionTextArea";
        private string eventStartDateTextBoxCssClass = "calendarEditStartDateTextBox";
        private string eventEndDateTextBoxCssClass = "calendarEditEndDateTextBox";
        private string eventRepeatEndDateTextBoxCssClass = "calendarEditRepeatEndDateTextBox";
        private string eventStartTimeTextBoxCssClass = "calendarEditStartTimeTextBox";
        private string eventEndTimeTextBoxCssClass = "calendarEditEndTimeTextBox";
        private string editPanelCssClass = "calendarEditPanel";
        private string editButtonAreaCssClass = "calendarEditButtonArea";
        private string editNotesAreaCssClass = "calendarNotesArea";
        private string editRepeatDateCssClass = "calendarRepeatDateLabel";
        private string editStartEndTimeCssClass = "calendarStartEndTimeLabel";
        private string editStartEndTimeDivCssClass = "calendarStartEndTimeDiv";
        private string editStartDateLabelCssClass = "calendarStartDateLabel";
        private string editStartDateDivCssClass = "calendarStartDateDiv";
        private string repeatEndDateDivCssClass = "calendarRepeatEndDateDiv";


        #endregion

        #region Scripts

        private const string EMPTYGUID = "00000000-0000-0000-0000-000000000000";

        private const string CalendarLibraryScript = @"
        <script type='text/javascript' src='/scripts/SimpleDatePicker.js'></script>
        <script type='text/javascript' src='/scripts/jquery.alerts.js'></script>

            <script type='text/javascript'>
            
                function CreateCalendarEventComplete(result) {
                    jAlert(result, 'Success!');
                }

                function CreateCalendarEventFailed(result) {
                    jAlert('Calendar Processing Failed For Some Very Good Reason, I\'m sure.\n\nCheck your date and time formatting. Also remember that Event Title and Start Date are required.');
                }

                function DeleteCalendarEventComplete(result) {
                    jAlert('Calendar Event was Successfully Deleted!');
                }

                function DeleteCalendarEventFailed(result) {
                    jAlert('Event Deletion Failed For Some Very Good Reason, I\'m sure.');
                }

            </script>
            ";

        private const string JQueryLibraryScript = @"
        <script type='text/javascript' src='/scripts/jquery.js'></script>";

        private const string HANDLER_SCRIPT = @"
 	        <script type='text/javascript'>
	     
         function Submit{0}_Click(sourcecontentid, permalink) {
                    var _title = $('#{3}').val();
                    var _description = $('#{4}').val();
                    var _startDate = $('#{5}').val();
                    var _endDate = $('#{6}').val();
                    var _endRepeatDate = $('#{7}').val();
                    var _startTime = $('#{8}').val();
                    var _endTime = $('#{9}').val();
                    var ws = new Sueetie.Web.SueetieService();
                     ws.CreateUpdateCalendarEvent(sourcecontentid, _title, _description, _startDate, _endDate, _endRepeatDate, _startTime, _endTime, permalink, CreateCalendarEventComplete, CreateCalendarEventFailed);
  display{0}Results();
                }

        function display{0}Results(result) {
            $('#{1}').toggle();
            $('#{2}').toggle();
        }

        function Toggle{0}_Click() {
            $('#{1}').toggle();
            $('#{2}').toggle();
        }
        

         function delete{0}_Click(eventguid) {
                    var ws = new Sueetie.Web.SueetieService();
                     ws.DeleteCalendarEvent(eventguid, 1, DeleteCalendarEventComplete, DeleteCalendarEventFailed);
  display{0}Results();
                }



            $(function () {
                $('#{5}').simpleDatepicker();
                $('#{6}').simpleDatepicker();    
                $('#{7}').simpleDatepicker();                    
                $('#{1}').hide();
            });

		</script>";

        #endregion

        #region Utilities

        private bool IsUserCalendarEditor()
        {
            string _roles = "ContentAdministrator";
            bool isAuthorized = false;
            if (SueetieContext.Current.ContentPage != null)
            {
                SueetieContentPage _sueetieContentPage = SueetieContext.Current.ContentPage;
                if (!string.IsNullOrEmpty(_sueetieContentPage.EditorRoles))
                    _roles = _sueetieContentPage.EditorRoles;
            }
            if (!string.IsNullOrEmpty(Roles))
                _roles = Roles;
            if (SueetieUIHelper.IsUserAuthorized(_roles) || SueetieUIHelper.IsUserAuthorized("ContentAdministrator"))
                isAuthorized = true;
            return isAuthorized;
        }

        #endregion

        #region OnInit - Generate HTML

        protected override void OnInit(EventArgs e)
        {

            #region Populate Event from Context Objects

            if (SueetieContext.Current.ContentPage != null)
            {
                this.SourceContentID = SueetieContext.Current.ContentPage.ContentID;
                this.Permalink = SueetieContext.Current.ContentPage.Permalink;
                this.SourceTitle = SueetieContext.Current.ContentPage.PageTitle;
            }

            if (this.CalendarSueetieMediaObject != null)
            {
                this.SourceContentID = CalendarSueetieMediaObject.ContentID;
                this.Permalink = CalendarSueetieMediaObject.MediaObjectUrl;
                this.SourceTitle = CalendarSueetieMediaObject.MediaObjectTitle;
            }

            if (this.CalendarSueetieBlogPost != null)
            {
                this.SourceContentID = CalendarSueetieBlogPost.ContentID;
                this.Permalink = CalendarSueetieBlogPost.Permalink;
                this.SourceTitle = CalendarSueetieBlogPost.Title;
            }

            if (this.CalendarSueetieMediaAlbum != null)
            {
                this.SourceContentID = CalendarSueetieMediaAlbum.ContentID;
                this.Permalink = CalendarSueetieMediaAlbum.Permalink;
                this.SourceTitle = CalendarSueetieMediaAlbum.AlbumTitle;
            }

            if (this.CalendarSueetieWikiPage != null)
            {
                this.SourceContentID = CalendarSueetieWikiPage.ContentID;
                this.Permalink = CalendarSueetieWikiPage.Permalink;
                this.SourceTitle = CalendarSueetieWikiPage.PageTitle;
            }

            if (this.CalendarSueetieForumTopic != null)
            {
                this.SourceContentID = CalendarSueetieForumTopic.ContentID;
                this.Permalink = CalendarSueetieForumTopic.Permalink;
                this.SourceTitle = CalendarSueetieForumTopic.Topic;
            }

            

            #endregion


            if (IsUserCalendarEditor() && DisplayCalendarsPanel)
            {
                //this.Controls.Add(new LiteralControl(CalendarLibraryScript));

                #region Get Calendar Event if Exists

                // Calendar Control uses Default Calendar Only in v2.0
                SueetieCalendarEvent _sueetieCalendarEvent = SueetieCalendars.GetSueetieCalendarEvent(SourceContentID, 1, false);
                if (_sueetieCalendarEvent == null)
                {
                    _sueetieCalendarEvent = new SueetieCalendarEvent
                    {
                        EventGuid = new Guid(EMPTYGUID)
                    };
                }
                else
                {
                    _existingStartDate = _sueetieCalendarEvent.StartDateTime.ToShortDateString();

                    if (_sueetieCalendarEvent.StartDateTime.Hour > 0)
                        _exisitngStartTime = _sueetieCalendarEvent.StartDateTime.ToShortTimeString();
                    if (_sueetieCalendarEvent.EndDateTime.Hour > 0)
                        _exisitngEndTime = _sueetieCalendarEvent.EndDateTime.ToShortTimeString();
                    if (_sueetieCalendarEvent.RepeatEndDate.Year > 1969)
                        _existingEndRepeatDate = _sueetieCalendarEvent.RepeatEndDate.ToShortDateString();
                    if (_sueetieCalendarEvent.EndDateTime.Date > _sueetieCalendarEvent.StartDateTime.Date)
                        _existingEndDate = _sueetieCalendarEvent.EndDateTime.ToShortDateString();
                }

                #endregion

                #region Load Script Logic

                // Moved to page to reduce duplication

                //if (LoadScriptLibraries)
                //{
                //    bool CalendarLibraryLoaded = false;
                //    bool JQueryLibraryLoaded = false;
                //    foreach (Control _control in this.Page.Header.Controls)
                //    {
                //        if (_control.GetType().Name == "LiteralControl")
                //        {
                //            string _js = ((LiteralControl)_control).Text;
                //            if (_js.IndexOf("/SimpleDatePicker.js") > 0)
                //                CalendarLibraryLoaded = true;
                //            if (_js.IndexOf("/jquery.js") > 0)
                //                JQueryLibraryLoaded = true;
                //        }
                //    }

                //    if (!JQueryLibraryLoaded)
                //        this.Page.Header.Controls.Add(new LiteralControl(JQueryLibraryScript));

                //    if (!CalendarLibraryLoaded)
                //        this.Page.Header.Controls.Add(new LiteralControl(CalendarLibraryScript));
                //}

                #endregion

                #region Create Form Elements

                this._lblNotes = new Label();
                this._lblNotes.Text = SueetieLocalizer.GetString("calendar_editnotes");
                this._lblNotes.CssClass = EditNotesAreaCssClass;

                this._lblRepeatDate = new Label();
                this._lblRepeatDate.Text = SueetieLocalizer.GetString("calendar_label_repeatdate");
                this._lblRepeatDate.CssClass = EditRepeatDateCssClass;

                this._lblStartEndTime = new Label();
                this._lblStartEndTime.Text = SueetieLocalizer.GetString("calendar_label_startendtime");
                this._lblStartEndTime.CssClass = EditStartEndTimeCssClass;

                this._lblStartDate = new Label();
                this._lblStartDate.Text = SueetieLocalizer.GetString("calendar_label_startdate");
                this._lblStartDate.CssClass = EditStartDateLabelCssClass;

                this._txtEventTitle = new TextBox();
                this._txtEventTitle.CssClass = EventTitleTextBoxCssClass;
                this._txtEventTitle.Text = !string.IsNullOrEmpty(_sueetieCalendarEvent.EventTitle) ? _sueetieCalendarEvent.EventTitle : SourceTitle;

                this._txtEventDescription = new TextBox();
                this._txtEventDescription.TextMode = TextBoxMode.MultiLine;
                this._txtEventDescription.CssClass = EventDescriptionTextAreaCssClass;
                this._txtEventDescription.Text = _sueetieCalendarEvent.EventDescription;

                this._calendarStart = new TextBox();
                this._calendarStart.CssClass = EventStartDateTextBoxCssClass;
                this._calendarStart.Text = _existingStartDate;

                this._calendarEnd = new TextBox();
                this._calendarEnd.CssClass = EventEndDateTextBoxCssClass;
                this._calendarEnd.Text = _existingEndDate;

                this._calendarRepeatEnd = new TextBox();
                this._calendarRepeatEnd.CssClass = EventRepeatEndDateTextBoxCssClass;
                this._calendarRepeatEnd.Text = _existingEndRepeatDate;

                this._eventStartTime = new TextBox();
                this._eventStartTime.CssClass = EventStartTimeTextBoxCssClass;
                this._eventStartTime.Text = _exisitngStartTime;

                this._eventEndTime = new TextBox();
                this._eventEndTime.CssClass = EventEndTimeTextBoxCssClass;
                this._eventEndTime.Text = _exisitngEndTime;

                Literal ltStartEndDiv = new Literal();
                ltStartEndDiv.Text = string.Format("<div class='{0}'></div>", this.EditStartEndTimeDivCssClass);

                Literal ltStartDateDiv = new Literal();
                ltStartDateDiv.Text = string.Format("<div class='{0}'></div>", this.EditStartDateDivCssClass);


                Literal ltRepeatEndDateDiv = new Literal();
                ltRepeatEndDateDiv.Text = string.Format("<div class='{0}'></div>", this.RepeatEndDateDivCssClass);

                Literal ltAsteriskTitle = new Literal();
                ltAsteriskTitle.Text = "<span class='calendarAsterisk'>*</span>";

                Literal ltAsteriskDate = new Literal();
                ltAsteriskDate.Text = "<span class='calendarAsterisk'>*</span>";

                Literal ltSubmitButton = new Literal();
                Literal ltDeleteButton = new Literal();
                Literal ltCancelButton = new Literal();

                #endregion

                #region Add Form Elements

                this._panel = new HtmlGenericControl("div");
                this._panel.Attributes.Add("class", EditPanelCssClass);

                this.Controls.Add(_panel);
                _panel.Controls.Add(_lblNotes);
                _panel.Controls.Add(_txtEventTitle);
                _panel.Controls.Add(ltAsteriskTitle);

                _panel.Controls.Add(_txtEventDescription);

                _panel.Controls.Add(ltStartDateDiv);
                _panel.Controls.Add(_lblStartDate);
                _panel.Controls.Add(_calendarStart);
                _panel.Controls.Add(ltAsteriskDate);
                _panel.Controls.Add(_calendarEnd);

                //_panel.Controls.Add(ltRepeatEndDateDiv);
                _panel.Controls.Add(_lblRepeatDate);
                _panel.Controls.Add(_calendarRepeatEnd);

                _panel.Controls.Add(ltStartEndDiv);
                _panel.Controls.Add(_lblStartEndTime);
                _panel.Controls.Add(_eventStartTime);
                _panel.Controls.Add(_eventEndTime);

                _panel.Controls.Add(ltSubmitButton);
                _panel.Controls.Add(ltDeleteButton);
                _panel.Controls.Add(ltCancelButton);

                this._imageButton = new ImageButton();
                this._imageButton.ImageUrl = this.EditImageUrl;
                this._imageButton.CssClass = this.EditImageCssClass;
                this._imageButton.OnClientClick = string.Format("Toggle{0}_Click();return false;", this._txtEventTitle.ClientID);
                this.Controls.Add(_imageButton);

                #endregion

                #region Parse Script

                string _script = HANDLER_SCRIPT.Replace("{0}", this._txtEventTitle.ClientID);
                _script = _script.Replace("{1}", this._panel.ClientID);
                _script = _script.Replace("{2}", this._imageButton.ClientID);
                _script = _script.Replace("{3}", this._txtEventTitle.ClientID);
                _script = _script.Replace("{4}", this._txtEventDescription.ClientID);
                _script = _script.Replace("{5}", this._calendarStart.ClientID);
                _script = _script.Replace("{6}", this._calendarEnd.ClientID);
                _script = _script.Replace("{7}", this._calendarRepeatEnd.ClientID);
                _script = _script.Replace("{8}", this._eventStartTime.ClientID);
                _script = _script.Replace("{9}", this._eventEndTime.ClientID);

                if (UseJqueryPrefix)
                    _script = _script.Replace("$", "jQuery");
      
                this.Controls.Add(new LiteralControl(_script));
                
                #endregion
                
                #region Submit

                string _submitText = SueetieLocalizer.GetString("calendar_button_add");
                string _cancelText = SueetieLocalizer.GetString("calendar_button_cancel");
                string _deleteText = SueetieLocalizer.GetString("calendar_button_delete");

                //ws.CreateUpdateCalendarEvent(eventID, title, description, start, end, allDay, endRepeat, startTime, endTime);

                ltSubmitButton.Text = string.Format("<div class='{0}'><input type='submit' onclick='Submit{1}_Click(\"{2}\",\"{3}\");return false;' value='{4}' />", this.EditButtonAreaCssClass, this._txtEventTitle.ClientID, this.SourceContentID, this.Permalink, _submitText);
                ltDeleteButton.Text = string.Format("<input type='submit' onclick='delete{0}_Click(\"{1}\");return false;' value='{2}' />", this._txtEventTitle.ClientID, _sueetieCalendarEvent.EventGuid, _deleteText);
                ltCancelButton.Text = string.Format("<input type='submit' onclick='Toggle{0}_Click();return false;' value='{1}' /></div>", this._txtEventTitle.ClientID, _cancelText);

                #endregion

            }
        }

        #endregion

    }
}
