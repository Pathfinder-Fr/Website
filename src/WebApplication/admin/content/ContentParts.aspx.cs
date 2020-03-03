using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using AjaxControlToolkit;

namespace Sueetie.Web
{
    public partial class AdminContentParts : SueetieAdminPage
    {

        public AdminContentParts()
            : base("admin_content_contentparts")
        {
        }
        private int paneIndex = 0;
        
        protected void Page_Init(object sender, EventArgs e)
        {

            List<SueetieContentPart> sueetieContentParts = SueetieContentParts.GetSueetieContentPartList(-1);

            foreach (SueetieContentPart sueetieContentPart in sueetieContentParts)
            {
                if (Request["ContentName"] != null)
                    if (Request["ContentName"].ToString() == sueetieContentPart.ContentName.Trim())
                        ContentParts1.SelectedIndex = paneIndex;
                AccordionPane pane = new AccordionPane();
                pane.Header = new TemplateBuilder(HeaderBuilder(sueetieContentPart));
                pane.Content = new TemplateBuilder(ContentBuilder(sueetieContentPart));
                pane.ID = sueetieContentPart.ContentName;
                ContentParts1.Panes.Add(pane);
                // paneIndex is used to determine which pane is being worked on when using FindControl
                paneIndex++;
            }
        }

        private Panel HeaderBuilder(SueetieContentPart content)
        {
            Panel header = new Panel();
            header.CssClass = "header";
            header.Controls.Add(new LiteralControl(string.Format("<span class=\"contentid\">{0}</span>", content.ContentName)));
            header.Controls.Add(new LiteralControl(string.Format("<span class=\"lastchange\">{0}</span>", content.LastUpdateDateTime)));
            return header;
        }

        private Panel ContentBuilder(SueetieContentPart content)
        {
            Panel body = new Panel();
            body.CssClass = "content";
            AjaxControlToolkit.TabContainer tabs = new TabContainer();
            tabs.ID = "tabs";
            tabs.CssClass = "tabs";
            AjaxControlToolkit.TabPanel tabEdit = new TabPanel();
            tabEdit.HeaderText = "Edit";
            tabs.Tabs.Add(tabEdit);
            AjaxControlToolkit.TabPanel tabUsers = new TabPanel();
            tabUsers.HeaderText = "Users";
            // Removed the addition of the Users Tab
            //tabs.Tabs.Add(tabUsers);
            UpdatePanel udpEditor = new UpdatePanel();
            udpEditor.UpdateMode = UpdatePanelUpdateMode.Always;
            udpEditor.ChildrenAsTriggers = true;
            udpEditor.ContentTemplate = new TemplateBuilder(EditorBuilder(content));
            tabEdit.ContentTemplate = new TemplateBuilder(udpEditor);
            body.Controls.Add(tabs);
            return body;
        }

        private Panel EditorBuilder(SueetieContentPart content)
        {
            Panel body = new Panel();
            body.CssClass = "editor";
            TextBox txtContent = new TextBox();
            txtContent.ID = "txtContent";
            txtContent.TextMode = TextBoxMode.MultiLine;
            txtContent.Height = new Unit("340px");
            txtContent.Width = new Unit("98%");
            txtContent.Text = content.ContentText;
            body.Controls.Add(txtContent);
            Panel buttons = new Panel();
            Button cmdClear = new Button();
            cmdClear.Text = "Undo Changes";
            cmdClear.ID = string.Concat("Clear_", paneIndex.ToString());
            // We use the CommandArguement property to pass data around about which content part we are working on
            cmdClear.CommandArgument = string.Concat(content.ContentName, "_", paneIndex);
            cmdClear.Click += new EventHandler(cmdClear_Click);
            buttons.Controls.Add(cmdClear);
            Button cmdSave = new Button();
            cmdSave.Text = "Save";
            cmdSave.ID = string.Concat("Save_", paneIndex.ToString());
            // We use the CommandArguement property to pass data around about which content part we are working on
            cmdSave.CommandArgument = string.Concat(content.ContentName, "_", paneIndex);
            cmdSave.Click += new EventHandler(cmdSave_Click);
            buttons.Controls.Add(cmdSave);
            buttons.CssClass = "contentGridViewPager";
            CheckBox chkEditor = new CheckBox();
            chkEditor.Text = "HTML Mode";
            chkEditor.Attributes.Add("onclick", "ToggleEditor(this);");
            chkEditor.Checked = true;
            // Message is used to display 'Saved' it is a panel so we can add jQuery effects to make it fade etc.
            Panel subButtons = new Panel();
            Panel message = new Panel();
            message.ID = "message";
            message.CssClass = "ContentSavedMessage";
            //buttons.Controls.Add(message);
            buttons.Controls.Add(chkEditor);
            body.Controls.Add(buttons);
            Literal lineBreak = new Literal();
            lineBreak.Text = "<div style='clear: both;'></div>";
            subButtons.Controls.Add(lineBreak);
            subButtons.Controls.Add(message);

            body.Controls.Add(subButtons);
            return body;

        }

        private SueetieContentPart GetContentPart(string commandArgs, out string[] args)
        {
            args = commandArgs.Split('_');
            return DbSueetieDataProvider.Provider.GetSueetieContentPart(args[0].Trim());
        }

        private Control GetControl(string ControlID, int AccordianIndex, int TabIndex)
        {
            return (((TabContainer)ContentParts1.Panes[AccordianIndex].FindControl("tabs"))).Tabs[TabIndex].FindControl(ControlID);
        }

        private string[] GetSelectedItems(ListBox lb)
        {
            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
            foreach (ListItem li in lb.Items)
                if (li.Selected)
                    items.Add(li.Text);
            return items.ToArray();
        }

        private void RemoveItem(object sender, string sourceLb)
        {
            AddItem(sender, sourceLb, string.Empty);
        }

        private void AddItem(object sender, string sourceLb, string destinationLb)
        {
            // Get the content part, ContentName and accordian pane index
            string[] args;
            SueetieContentPart content = GetContentPart(((Button)sender).CommandArgument, out args);
            // Get the ListBox that the item currently resides in
            ListBox lbSource = (ListBox)GetControl(sourceLb, int.Parse(args[1]), 1);
            // Get the ListBox to add the item to. Null if just removing an item.
            ListBox lbDestination = null;
            if (!string.IsNullOrEmpty(destinationLb))
                lbDestination = (ListBox)GetControl(destinationLb, int.Parse(args[1]), 1);

            // Save changes.
            content.ContentPageID = -1;
            DbSueetieDataProvider.Provider.UpdateSueetieContentPart(content);
        }

        void cmdClear_Click(object sender, EventArgs e)
        {
            string[] args;
            SueetieContentPart content = GetContentPart(((Button)sender).CommandArgument, out args);
            TextBox txtContent = (TextBox)GetControl("txtContent", int.Parse(args[1]), 0);
            txtContent.Text = content.ContentText;
        }

        void cmdSave_Click(object sender, EventArgs e)
        {
            string[] args;
            SueetieContentPart sueetieContentPart= GetContentPart(((Button)sender).CommandArgument, out args);
            TextBox txtContent = (TextBox)GetControl("txtContent", int.Parse(args[1]), 0);
            Panel message = (Panel)GetControl("message", int.Parse(args[1]), 0);
            message.Controls.Clear();
            try
            {
                sueetieContentPart.ContentText = txtContent.Text;
                sueetieContentPart.ContentPageID = -1;
                SueetieDataProvider.Provider.UpdateSueetieContentPart(sueetieContentPart);
                SueetieContentParts.ClearContentPartCache(sueetieContentPart.ContentName);

                // -1 indicates content parts not associated with Content Pages
                SueetieContentParts.ClearSueetieContentPartListCache(-1);  
                message.Controls.Add(new LiteralControl("<span>Saved</span>"));
            }
            catch
            {
                message.Controls.Add(new LiteralControl("<span>Failed</span>"));
            }
        }
    }

    class TemplateBuilder : ITemplate
    {
        Control _cont = null;

        public TemplateBuilder(Control cont)
        {
            _cont = cont;
        }


        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            container.Controls.Add(_cont);
        }

        #endregion
    }
}
