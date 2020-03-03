using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{

	[DefaultEvent("Click")]
	public class SueetieButton : WebControl
	{
		private bool m_booCausesValidataion;
		public event EventHandler Click;

		private string _defaultText = null;
		private string _toggleText = null;

		private string _toggleCSSClass = null;
		
		private string _promptText = "Press OK to verify the deletion of the checked item(s).";
		private string	_scriptToRun = null;

		private bool	_prompt = false;

		private HtmlButton _button;

		private bool isToggled 
		{
			get 
			{
				return this.Toggled;
			}
			set
			{
				this.ViewState["TOGGLED"] = value;
			}
		}

		public bool Toggled 
		{
			get 
			{
				return (this.ViewState["TOGGLED"] == null) ? false : 
					(bool) this.ViewState["TOGGLED"];
			}
		}

		[
			Bindable(true),
				Category("Behavior"),
				Description("True or false to spefify a prompt with okay/cancel")]
		public bool UsePrompt
		{
			get 
			{
				return this._prompt;
			}
			set
			{
				this._prompt = value;
			}
		}

		[
			Bindable(true),
				Category("Behavior"),
				Description("Specifies a script to run before the post back event."),
				DefaultValue("")
		]
		public string ScriptToRun
		{
			set
			{
				_scriptToRun = value;
			}

			get 
			{
				return _scriptToRun;
			}
		}

		[
			Bindable(true),
				Category("Appearance"),
				Description("The text of this control"),
				DefaultValue("")
		]
		public string Text 
		{
			set 
			{
				_defaultText = value;
			}

			get 
			{
				return _defaultText;
			}
		}

		[
			Bindable(true),
				Category("Appearance"),
				Description("Specifies the text that appears when the button is toggled."),
				DefaultValue("")
		]
		public string ToggleText 
		{
			set 
			{
				_toggleText = value;
			}
			get 
			{
				return (_toggleText != null) ? _toggleText : _defaultText;
			}
		}


		[
			Bindable(true),
				Category("Appearance"),
				Description("Specifies the css class to use when the button is toggled."),
				DefaultValue("")
		]
		public string ToggleCssClass
		{
			get
			{
				return (_toggleCSSClass != null) ? _toggleCSSClass : this.CssClass;
			}
			set
			{
				_toggleCSSClass = value;
			}
		}

		/// <summary>
		/// Creates a new <see cref="SueetieButton"/> instance.
		/// </summary>
		public SueetieButton()
		{}

		/// <summary>
		/// Creates a new <see cref="SueetieButton"/> instance.
		/// </summary>
		/// <param name="text">Text.</param>
		public SueetieButton(string text)
		{
			this._defaultText = text;
		}

		public bool Toggle()
		{
			this.isToggled = !this.isToggled;
			return this.isToggled;
		}

		public void Reset()
		{
			this.isToggled = false;
		}

		[
			Bindable(true),
				Category("Behavior"),
				Description("The prompt to use if _prompt is true."),
				DefaultValue("")]
		public string Prompt
		{
			get
			{
				return _promptText;
			}

			set
			{
				_promptText = value;
			}
		}

		
		protected override void OnInit(EventArgs e)
		{
			this._button = new HtmlButton();
			this._button.ServerClick += new EventHandler(_button_ServerClick);
			this.Controls.Add(this._button);
		}
					

		protected override void OnPreRender(EventArgs e)
		{

			string text = (this.isToggled) ? this._toggleText : this._defaultText;
			string css = (this.isToggled) ? this._toggleCSSClass : this.CssClass;

			// Validate, Prompt, Extra, Progress, Submit...

			//string postback = this.Page.GetPostBackEventReference(this._button) +";";
            PostBackOptions options = new PostBackOptions(this._button);
            options.PerformValidation = false;
            string postback = this.Page.ClientScript.GetPostBackEventReference(options) + ";";

			if(this._scriptToRun != null)
				postback = this._scriptToRun + ";" + postback;

			if(this._prompt)
				postback = "if(confirm('" + this._promptText + "')){" + postback +"}";

			if(this.CausesValidation && this.Page.Validators.Count > 0)
				postback = "if (typeof(Page_ClientValidate) != 'function' || Page_ClientValidate()){" + postback +"}"; 
  
			if(this.Enabled == false)
				this._button.Attributes["disabled"] = "Disabled";
			else
				this._button.Attributes.Remove("disabled");

			this._button.Attributes["onClick"] = postback;
			this._button.Attributes["type"] = "Button";
			this._button.Attributes["class"] = css;
			this._button.Attributes["value"] = (text != null && text.Length > 0) ? text : this.ID;
		}

		/// <summary>
		///		Checks to see if we are running in design mode. 
		/// </summary>
		/// <value>
		///     <para>
		///         bool
		///     </para>
		/// </value>
		/// <remarks>
		///     Useful when devleoping controls.  This is true when you are developing 
		///     within the VS.NET environment.
		/// </remarks>
        //public bool DesignMode
        //{
        //    get
        //    {
        //        object design;
        //        try	{ design = Site.DesignMode; } 
        //        catch { design = null; }
        //        if(design!=null)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

		protected override void Render(HtmlTextWriter writer)
		{
			if(DesignMode)
			{
				this._button.InnerText = (this._defaultText != null) ? this._defaultText: this.ID;
				this._button.RenderControl(writer);
			}
			else
			{
				HtmlGenericControl hgc = new HtmlGenericControl("input");
				foreach(string key in _button.Attributes.Keys)
					hgc.Attributes.Add(key, _button.Attributes[key]);
				hgc.RenderControl(writer);
			}
		}

		public override void RenderBeginTag(HtmlTextWriter writer){}

		public override void RenderEndTag(HtmlTextWriter writer){}


		private void _button_ServerClick(object sender, EventArgs e)
		{
			if(this.Click != null)
				this.Click(this, new EventArgs());
		}

		public bool CausesValidation
		{
			get
			{
				return m_booCausesValidataion;
			}
			set
			{
				m_booCausesValidataion = value;
			}
		}

	}
}
