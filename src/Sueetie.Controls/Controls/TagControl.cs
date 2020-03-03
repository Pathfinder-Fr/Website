
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
    public class TagControl : SueetieBaseControl
    {

        #region Properties

        private HtmlGenericControl _panel;
        private TextBox _textbox;
        private Label _tags;
        private ImageButton _imageButton;
        private bool _DisplayTagsPanel = true;
        private bool _LoadScriptLibraries = true;
        private int _contentID = -1;
        private int _applicationID = -1;
        private int _contentTypeID = -1;
        private int _itemID = -1;
        private int _galleryID = 1;
        private bool _isSueetieMediaObject = false;
        private bool _isSueetieMediaAlbum = false;
        public SueetieWikiPage TagSueetieWikiPage = null;
        public SueetieMediaObject TagSueetieMediaObject = null;
        public SueetieMediaAlbum TagSueetieMediaAlbum = null;
        public SueetieForumTopic TagSueetieForumTopic = null;


        public SueetieUser TagSueetieUser { get; set; }


        private bool _useJqueryPrefix = false;
        public bool UseJqueryPrefix
        {
            get { return _useJqueryPrefix; }
            set { _useJqueryPrefix = value; }
        }

        public int ContentID
        {
            get { return _contentID; }
            set { _contentID = value; }
        }
        public int ApplicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }
        public int GalleryID
        {
            get { return _galleryID; }
            set { _galleryID = value; }
        }
        public int ContentTypeID
        {
            get { return _contentTypeID; }
            set { _contentTypeID = value; }
        }
        public int ItemID
        {
            get { return _itemID; }
            set { _itemID = value; }
        }
        public bool DisplayTagsPanel
        {
            get { return _DisplayTagsPanel; }
            set { _DisplayTagsPanel = value; }
        }
        public bool LoadScriptLibraries
        {
            get { return _LoadScriptLibraries; }
            set { _LoadScriptLibraries = value; }
        }
        public string TagsListCssClass
        {
            get { return tagsListCssClass; }
            set { tagsListCssClass = value; }
        }
        public string EditImageCssClass
        {
            get { return editImageCssClass; }
            set { editImageCssClass = value; }
        }
        public string EditTextBoxCssClass
        {
            get { return editTextBoxCssClass; }
            set { editTextBoxCssClass = value; }
        }
        public string EditPanelCssClass
        {
            get { return editPanelCssClass; }
            set { editPanelCssClass = value; }
        }
        public string EditButtonAreaCssClass
        {
            get { return editButtonAreaCssClass; }
            set { editButtonAreaCssClass = value; }
        }
        public string EditImageUrl
        {
            get { return editImageUrl; }
            set { editImageUrl = value; }
        }
        public string Roles { get; set; }

        private string editImageUrl = "/themes/" + SiteSettings.Instance.Theme + "/images/tagEdit.png";
        private string editImageCssClass = "tagsEditToggleImage";
        private string editTextBoxCssClass = "tagsEditBox";
        private string editPanelCssClass = "tagsEditPanel";
        private string tagsListCssClass = "tagsList";
        private string editButtonAreaCssClass = "tagsEditButtonArea";

        #endregion

        #region Scripts

        private const string JQueryLibraryScript = @"
        <script type='text/javascript' src='/scripts/jquery.js'></script>";

        private const string HANDLER_SCRIPT = @"
 	        <script type='text/javascript'>
   	     
                function Submit{0}_Click(itemid, contentid, contenttypeid) {
                    var categories = jQuery('#{0}').val();
                    if (categories != '') {
                     var ws = new Sueetie.Web.SueetieService();
                     ws.ProcessTags(itemid, contentid, contenttypeid, categories, display{0}Results);
                    }
                }

        function display{0}Results(result) {
            jQuery('#{2}').html(result);
            jQuery('#{1}').toggle();
            jQuery('#{3}').toggle();
        }

        function Toggle{0}_Click() {
            jQuery('#{1}').toggle();
            jQuery('#{3}').toggle();
        }

            jQuery(function () {
                jQuery('#{0}').tagSuggest({
                    url: '/util/json/jsontags.aspx',
                    delay: 300
                });
                jQuery('#{1}').hide();
            });

		</script>";

        private const string MINIHANDLER_SCRIPT = @"
 	        <script type='text/javascript'>
   	  
            jQuery(function () {
                jQuery('#{0}').tagSuggest({
                    url: '/util/json/jsontags.aspx',
                    delay: 300
                });
            });

		</script>";

        #endregion

        #region Utilities

        private bool IsUserTagEditor()
        {
            string _roles = "ContentAdministrator";
            bool isAuthorized = false;
            if (SueetieContext.Current.ContentPage != null)
            {
                SueetieContentPage _sueetieContentPage = SueetieContext.Current.ContentPage;
                if (!string.IsNullOrEmpty(_sueetieContentPage.EditorRoles))
                    _roles = _sueetieContentPage.EditorRoles;
            }
            if (TagSueetieForumTopic != null && TagSueetieForumTopic.SueetieUserIDs != null)
            {
                var _userIDs = TagSueetieForumTopic.SueetieUserIDs.Split('|')
                    .Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n)).ToList();
                foreach (int _userID in _userIDs)
                {
                    if (CurrentSueetieUserID.Equals(_userID))
                        isAuthorized = true;
                }
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
            string tagsList = string.Empty;

            this._tags = new Label();
            this._tags.CssClass = tagsListCssClass;

            if (SueetieContext.Current.ContentPage != null)
            {
                SueetieContentPage _sueetieContentPage = SueetieContext.Current.ContentPage;
                this.ContentID = _sueetieContentPage.ContentID;
                this.ItemID = _sueetieContentPage.ContentPageID;
                this.ContentTypeID = _sueetieContentPage.ContentTypeID;
                tagsList = _sueetieContentPage.Tags;
            }

            if (this.TagSueetieMediaObject != null)
            {
                this.ItemID = TagSueetieMediaObject.MediaObjectID;
                this.ContentID = TagSueetieMediaObject.ContentID;
                this.ContentTypeID = TagSueetieMediaObject.ContentTypeID;
                tagsList = TagSueetieMediaObject.Tags;
            }

            if (this.TagSueetieMediaAlbum != null)
            {
                this.ItemID = TagSueetieMediaAlbum.AlbumID;
                this.ContentID = TagSueetieMediaAlbum.ContentID;
                this.ContentTypeID = TagSueetieMediaAlbum.ContentTypeID;
                tagsList = TagSueetieMediaAlbum.Tags;
            }


            if (this.TagSueetieWikiPage != null)
            {
                this.ItemID = TagSueetieWikiPage.PageID;
                this.ContentID = TagSueetieWikiPage.ContentID;
                this.ContentTypeID = (int)SueetieContentType.WikiPage;
                tagsList = this.TagSueetieWikiPage.Tags;
            }

            if (this.TagSueetieForumTopic != null)
            {
                this.ItemID = TagSueetieForumTopic.TopicID;
                this.ContentID = TagSueetieForumTopic.ContentID;
                this.ContentTypeID = (int)SueetieContentType.ForumTopic;
                tagsList = TagSueetieForumTopic.Tags;

             //   this.Controls.Add(new LiteralControl(TagLibraryScript));
            }

            if (!string.IsNullOrEmpty(tagsList))
                this._tags.Text = SueetieTags.TagUrls(this.ContentID);
            else
                this._tags.Text = SueetieLocalizer.GetString("no_tags");

            this.Controls.Add(_tags);


            if (IsUserTagEditor() && DisplayTagsPanel)
            {

                //if (LoadScriptLibraries)
                //{
                //    bool TagLibraryLoaded = false;
                //    //bool JQueryLibraryLoaded = false;
                //    foreach (Control _control in this.Page.Header.Controls)
                //    {
                //        if (_control.GetType().Name == "LiteralControl")
                //        {
                //            string _js = ((LiteralControl)_control).Text;
                //            if (_js.IndexOf("/scripts/tag.js") > 0)
                //                TagLibraryLoaded = true;
                //            //if (_js.IndexOf("/scripts/jquery.js") > 0)
                //            //    JQueryLibraryLoaded = true;
                //        }
                //    }

                //    //if (!JQueryLibraryLoaded)
                //    //    this.Page.Header.Controls.Add(new LiteralControl(JQueryLibraryScript));

                //    //if (!TagLibraryLoaded)
                //    //    this.Page.Header.Controls.Add(new LiteralControl(TagLibraryScript));
                //}

                this._textbox = new TextBox();
                this._textbox.CssClass = EditTextBoxCssClass;
                if (!string.IsNullOrEmpty(tagsList))
                    this._textbox.Text = SueetieTags.CommaTags(tagsList);
                else
                    this._textbox.Text = string.Empty;

                //this.Controls.Add(_textbox);

                Literal ltImageButton = new Literal();
                Literal ltSubmitButton = new Literal();
                Literal ltCancelButton = new Literal();

                this._panel = new HtmlGenericControl("div");
                this._panel.Attributes.Add("class", EditPanelCssClass);

                this.Controls.Add(_panel);
                _panel.Controls.Add(_textbox);
                _panel.Controls.Add(ltSubmitButton);
                _panel.Controls.Add(ltCancelButton);

                this._imageButton = new ImageButton();
                this._imageButton.ImageUrl = this.EditImageUrl;
                this._imageButton.CssClass = this.EditImageCssClass;
                this._imageButton.OnClientClick = string.Format("Toggle{0}_Click();return false;", this._textbox.ClientID);
                this.Controls.Add(_imageButton);

                string _submitText = SueetieLocalizer.GetString("tags_button_add");
                string _cancelText = SueetieLocalizer.GetString("tags_button_cancel");

                ltSubmitButton.Text = string.Format("<div class='{0}'><input type='submit' onclick='Submit{1}_Click({2},{3},{4});return false;' value='{5}' />", this.EditButtonAreaCssClass, this._textbox.ClientID, this.ItemID, this.ContentID, this.ContentTypeID, _submitText);
                ltCancelButton.Text = string.Format("<input type='submit' onclick='Toggle{0}_Click();return false;' value='{1}' /></div>", this._textbox.ClientID, _cancelText);

                string _script = HANDLER_SCRIPT.Replace("{0}", this._textbox.ClientID);
                _script = _script.Replace("{1}", this._panel.ClientID);
                _script = _script.Replace("{2}", this._tags.ClientID);
                _script = _script.Replace("{3}", this._imageButton.ClientID);

                if (UseJqueryPrefix)
                    _script = _script.Replace("$", "jQuery");

                this.Controls.Add(new LiteralControl(_script));
                //this.Page.Header.Controls.Add(new LiteralControl(_script));
            }
        }

        #endregion

    }
}
