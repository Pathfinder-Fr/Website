using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.IO;

namespace Sueetie.Controls
{

    public class ContentPagePart : SueetieBaseControl
    {

        #region Properties

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string TitleCssClass
        {
            get { return titleCssClass; }
            set { titleCssClass = value; }
        }
        public string WindowClass
        {
            get { return windowClass; }
            set { windowClass = value; }
        }
        public string CloseButtonImageUrl
        {
            get { return closeButtonImageUrl; }
            set { closeButtonImageUrl = value; }
        }
        public string IconCssClass
        {
            get { return iconCssClass; }
            set { iconCssClass = value; }
        }
        public bool ShowCloseButton
        {
            get { return showCloseBtn; }
            set { showCloseBtn = value; }
        }
        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }
        public string PencilImageUrl
        {
            get { return pencilImageUrl; }
            set { pencilImageUrl = value; }
        }
        public string PencilImageCssClass
        {
            get { return pencilImageCssClass; }
            set { pencilImageCssClass = value; }
        }
        public string Roles { get; set; }
        public string ContentName
        {
            get { return contentName; }
            set { contentName = value; }
        }
        private string contentName = "?69?";
        private string title = String.Empty;
        private string titleCssClass = "modalPanelTitle";
        private string windowClass = "modalPanel";
        private string closeButtonImageUrl = "/themes/" + SiteSettings.Instance.Theme + "/images/toolbar_close.gif";
        private string pencilImageUrl = "/themes/" + SiteSettings.Instance.Theme + "/images/pencil.png";
        private string pencilImageCssClass = "modalPencil";

        private string iconUrl = String.Empty;
        private string iconCssClass = "titleIcon";


        #endregion

        #region Controls

        Panel _panel = null;
        LiteralControl _text = null;
        TextBox txtEditor = null;
        CheckBox checkBoxLog = null;
        SueetieContentPart _sueetieContentPart = null;
        SueetieContentPage _sueetieContentPage = null;

        #endregion

        #region Modal Editor Construction

        private bool IsUserEditor()
        {
            string _roles = "ContentAdministrator";
            bool isAuthorized = false;
            if (!string.IsNullOrEmpty(_sueetieContentPage.EditorRoles))
                _roles = _sueetieContentPage.EditorRoles;
            if (!string.IsNullOrEmpty(Roles))
                _roles = Roles;
            if (SueetieUIHelper.IsUserAuthorized(_roles) || SueetieUIHelper.IsUserAuthorized("ContentAdministrator"))
                isAuthorized = true;
            return isAuthorized;
        }


        protected override void OnInit(EventArgs e)
        {

            #region Get SueetieContentPage

            int _contentPageID = DataHelper.GetIntFromQueryString("pg", -1);
            if (_contentPageID == -1)
                return;

            _sueetieContentPage = SueetieContentParts.GetSueetieContentPage(_contentPageID);
            ContentName = _sueetieContentPage.ApplicationKey + "." + _sueetieContentPage.PageKey + "." + this.ContentName;

            #endregion

            #region Create tinyMCE Script

            bool TinyMCELoaded = false;
            foreach (Control _control in this.Page.Header.Controls)
            {
                if (_control.GetType().Name == "LiteralControl")
                {
                    string _js = ((LiteralControl)_control).Text;
                    if (_js.IndexOf("tinyMCE.init") > 0)
                    {
                        int _elementPoint = _js.IndexOf("elements:");
                        int _endElement = _js.IndexOf("'", _elementPoint);
                        string _newJs = _js.Insert(_endElement + 1, this.ClientID + "_txt" + this.ContentName + ",");
                        LiteralControl _newJsLiteralControl = new LiteralControl(_newJs);
                        this.Page.Header.Controls.Remove(_control);
                        this.Page.Header.Controls.Add(_newJsLiteralControl);
                        TinyMCELoaded = true;
                    }
                }
            }
            if (!TinyMCELoaded)
                this.Page.Header.Controls.Add(new LiteralControl(TinyMCEScript.Replace("{0}", this.ClientID + "_txt" + this.ContentName)));

            #endregion

            #region Load Content Part

            _sueetieContentPart = SueetieContentParts.GetSueetieContentPart(this.ContentName);
            if (string.IsNullOrEmpty(_sueetieContentPart.ContentText) && this.ContentName != "?69?")
            {
                _sueetieContentPart.ContentText = "Empty";
                _sueetieContentPart.ContentName = this.ContentName;
            }
            else if (this.ContentName == "?69?")
                _sueetieContentPart.ContentText = "<b><font color='#FF0000'>No ContentName Specified with Sueetie Content Page Part Control</font></b>";

            #endregion

            #region ImageButton Display Modal Editor

            ImageButton btnOpen = new ImageButton();
            btnOpen.Click += new ImageClickEventHandler(btnOpen_Click);
            btnOpen.ImageUrl = this.PencilImageUrl;
            btnOpen.CssClass = this.PencilImageCssClass;
            if (IsUserEditor() && this.ContentName != "?69?")
                this.Controls.Add(btnOpen);

            #endregion

            #region Content Text

            _text = new LiteralControl();
            _text.Text = _sueetieContentPart.ContentText;
            this.Controls.Add(_text);

            #endregion

            #region Panel Creation

            _panel = new Panel();
            _panel.CssClass = windowClass;
            _panel.ID = "pnl" + this.ContentName;
            _panel.Height = 590;
            _panel.Width = 800;

            #endregion

            #region DragTop Table Row

            // Add the table
            Table table = new Table();
            table.CellSpacing = 0;
            table.CellPadding = 0;
            table.Width = new Unit("100%");

            // Add the title Row
            TableRow row = new TableRow();
            row.CssClass = titleCssClass;

            // Add th title cell
            TableCell cell = new TableCell();
            cell.Width = 780;
            cell.Attributes.Add("name", "dragger");

            if (iconUrl.Length > 0)
            {
                Image img = new Image();
                img.CssClass = iconCssClass;
                img.ImageAlign = ImageAlign.AbsMiddle;
                img.ImageUrl = iconUrl;
                cell.Controls.Add(img);
            }

            cell.Controls.Add(new LiteralControl(this.title));
            row.Controls.Add(cell);

            TableCell cellClose = new TableCell();
            ImageButton btn = new ImageButton();
            btn.ImageUrl = closeButtonImageUrl;
            btn.Click += new ImageClickEventHandler(btn_Click);
            cellClose.Controls.Add(btn);
            cellClose.Attributes.Add("text-align", "right");
            cellClose.Width = 20;
            row.Controls.Add(cellClose);

            table.Controls.Add(row);
            _panel.Controls.AddAt(0, table);

            #endregion

            #region Add Init_Script

            if (this.ID == null) this.ID = this.UniqueID;
            _panel.Controls.AddAt(0, new LiteralControl(String.Format(INIT_SCRIPT, this.ClientID + "_pnl" + this.ContentName)));

            #endregion

            #region Editor Row

            txtEditor = new TextBox();
            txtEditor.ID = "txt" + this.ContentName;
            txtEditor.Text = _sueetieContentPart.ContentText;
            txtEditor.Height = new Unit(500);
            txtEditor.Width = new Unit("100%");
            txtEditor.CssClass = "EditorFont";
            txtEditor.TextMode = TextBoxMode.MultiLine;

            TableRow rowbody = new TableRow();
            TableCell cellbody = new TableCell();
            cellbody.ColumnSpan = 2;
            rowbody.Controls.Add(cellbody);
            cellbody.Controls.Add(txtEditor);
            table.Controls.Add(rowbody);

            #endregion

            #region Button Row

            TableRow rowButtons = new TableRow();

            checkBoxLog = new CheckBox();
            checkBoxLog.Checked = true;
            checkBoxLog.Text = SueetieLocalizer.GetString("modal_contentpage_log");
            checkBoxLog.CssClass = "modalPanelCheckbox";

            TableCell cellButtons = new TableCell();
            cellButtons.ColumnSpan = 2;
            cellButtons.CssClass = "modalPanelButtonCell";

            Button btnSave = new Button();
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSave.CssClass = "modalPanelButtonSave";
            btnSave.Text = SueetieLocalizer.GetString("content_save");

            Button btnCancel = new Button();
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnCancel.CssClass = "modalPanelButtonCancel";
            btnCancel.Text = SueetieLocalizer.GetString("content_cancel");

            cellButtons.Controls.Add(checkBoxLog);
            cellButtons.Controls.Add(btnCancel);
            cellButtons.Controls.Add(btnSave);

            rowButtons.CssClass = "modalPanelButtonRow";
            rowButtons.Controls.Add(cellButtons);
            table.Controls.Add(rowButtons);

            #endregion

            this.Controls.Add(_panel);
            _panel.Visible = false;
            base.OnInit(e);

        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "DRAGSCRIPT", HANDLER_SCRIPT);
            //this.Page.RegisterClientScriptBlock("DRAGSCRIPT", HANDLER_SCRIPT);
            base.OnPreRender(e);
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        #endregion

        #region Private Events

        private bool showCloseBtn = true;
        private void btn_Click(object sender, ImageClickEventArgs e)
        {
            this._panel.Visible = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SueetieContentPage _page = SueetieContentParts.CurrentContentPage;
            _text.Text = txtEditor.Text;
            _sueetieContentPart.ContentText = txtEditor.Text;
            _sueetieContentPart.ContentPageID = _page.ContentPageID;
            _sueetieContentPart.ContentPageGroupID = _page.ContentPageGroupID;
            _sueetieContentPart.Permalink = "/" + _page.ApplicationKey + "/" + _page.PageSlug + ".aspx";
            _sueetieContentPart.LastUpdateUserID = this.CurrentSueetieUserID;
            _sueetieContentPart.ApplicationID = _page.ApplicationID;
            SueetieContentParts.ClearContentPartCache(_sueetieContentPart.ContentName);
            int _contentID = SueetieContentParts.UpdateSueetieContentPart(_sueetieContentPart);

            if (checkBoxLog.Checked)
                SueetieLogs.LogUserEntry(UserLogCategoryType.CMSPageUpdated, _contentID, CurrentSueetieUserID);

            this._panel.Visible = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._panel.Visible = false;
        }
        void btnOpen_Click(object sender, ImageClickEventArgs e)
        {

            _panel.Visible = true;
        }

        #endregion

        #region TinyMCEScript

        private const string TinyMCEScript = @"
        <script type='text/javascript' src='/util/editors/tiny_mce3/tiny_mce.js'></script>
		<script language='javascript' type='text/javascript'>
            tinyMCE.init({
                mode: 'exact',
                elements: '{0}',
                theme: 'advanced',
                plugins: 'inlinepopups,fullscreen,contextmenu,cleanup,emotions,iespell,advlink',
                convert_urls: false,
                theme_advanced_buttons1: 'fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,removeformat,cleanup,emotions,format,charmap,',
                theme_advanced_buttons2: 'removeformat,formatselect,fontselect,fontsizeselect,styleprops,hr,|,forecolor,backcolor,foredcolorpicker,backcolorpicker,image,',
                theme_advanced_buttons3: '',
                theme_advanced_toolbar_location: 'top',
                theme_advanced_toolbar_align: 'left',
                theme_advanced_statusbar_location: 'bottom',
                theme_advanced_resizing: false,

                tab_focus: ':prev,:next'
            });
		</script>";

        #endregion

        #region ModalPanel Scripts

        private const string HANDLER_SCRIPT = @"
 	<script language='javascript' type='text/javascript'>
	<!--

			var ie=document.all;
			var ns6=document.getElementById && !document.all;
			var dragapproved = false;
			var z,x,y;
			
			if(ie)
			{
				document.attachEvent('onmousedown', drags);
				document.attachEvent('onmouseup', stop);
			}
			else
			{
				document.onmousedown = drags;
				document.onmouseup = stop;
			}
			
			function stop(e)
			{
				dragapproved = false;
				if(z)
				{
					if(findPosY(z)<0 || findPosX(z)<0)return;

					setCookie(z.id + '_TOP', z.style.top);
					setCookie(z.id + '_LEFT', z.style.left);
				}
			}
			
			function findPosX(obj)
			{
				var curleft = 0;
				if (obj.offsetParent)
				{
					while (obj.offsetParent)
					{
						curleft += obj.offsetLeft
						obj = obj.offsetParent;
					}
				}
				else if (obj.x)
					curleft += obj.x;
				return curleft;
			}

			function findPosY(obj)
			{
				var curtop = 0;
				if (obj.offsetParent)
				{
					while (obj.offsetParent)
					{
						curtop += obj.offsetTop
						obj = obj.offsetParent;
					}
				}
				else if (obj.y)
					curtop += obj.y;
				
				return curtop;
			}
			
			function move(e)
			{
				if (dragapproved)
				{
					var eventX = (ns6 ? e.clientX : event.clientX);
					var eventY = (ns6 ? e.clientY : event.clientY);

					z.style.left = parseInt(z.style.left) + (eventX - x);
					z.style.top = parseInt(z.style.top) + (eventY - y);

					x = eventX;					
					y = eventY;

					return false
				}
			}

			function drags(e)
			{
	
				if (!ie && !ns6) return;
				
				var firedobj = ns6? e.target : event.srcElement;
				var topelement = ns6? 'HTML' : 'BODY';
				
				var firedobjName = ns6 ? firedobj.attributes['name'].value : firedobj.name;
				if(firedobjName != 'dragger') return;

				while (firedobj.tagName!=topelement && firedobj.className!='modalPanel'){
					firedobj=ns6? firedobj.parentNode : firedobj.parentElement;
				}

				if (firedobj.className=='modalPanel'){
					dragapproved=true

					z = firedobj
					x = (ns6 ? e.clientX: event.clientX);
					y = (ns6 ? e.clientY: event.clientY);

					document.onmousemove = move;
					return false;
				}
			}

			function positionDiv(elId)
			{
				var el = document.getElementById(elId);
				
				if(getCookie(el.id + '_TOP'))
				{
					el.style.top = getCookie(el.id + '_TOP');
					el.style.left = getCookie(el.id + '_LEFT');
				}				
				else
				{
					var thinkY = el.offsetTop;
					var realY = findPosY(el);
					var thinkX = el.offsetLeft;
					var realX = findPosX(el);

					var centerLeft  = ns6 ? (window.innerWidth - el.offsetWidth) /2 : (document.body.clientWidth - el.offsetWidth) / 2;
					var centerTop = ns6 ? (window.innerHeight - el.offsetHeight) /2 : (document.body.clientHeight- el.offsetHeight) / 2;
					
					el.style.top = thinkY + (centerTop - realY);
				    el.style.left = thinkX + (centerLeft - realX);
				}				
			}

			function getCookie (name) 
			{
				var dcookie = document.cookie; 
				var cname = name + '=';
				var clen = dcookie.length;
				var cbegin = 0;
				while (cbegin < clen) 
				{
					var vbegin = cbegin + cname.length;
					if (dcookie.substring(cbegin, vbegin) == cname) 
					{ 
						var vend = dcookie.indexOf (';', vbegin);
						if (vend == -1) vend = clen;
						return unescape(dcookie.substring(vbegin, vend));
					}
	        
					cbegin = dcookie.indexOf(' ', cbegin) + 1;
					if (cbegin == 0) break;
				}
				return null;
			}

			function setCookie (name, value) 
			{
				document.cookie = name + '=' + escape (value) + '; expires=Thu, 01-Jan-14 00:00:01 GMT; path=/';
			}

			//-->
		</script>";

        private const string INIT_SCRIPT = @"
		<script language='javascript' type='text/javascript'>
		    <!--
				positionDiv('{0}');
			//-->
		</script>";

        #endregion

    }
}