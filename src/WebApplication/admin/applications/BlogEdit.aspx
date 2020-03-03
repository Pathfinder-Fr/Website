<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlogEdit.aspx.cs" Inherits="Sueetie.Web.BlogEdit"  ValidateRequest="false" %>
<%@ Register Src="~/controls/editors/adminEditor.ascx" TagPrefix="cc2" TagName="TextEditor" %>
<%@ Register Src="../controls/adminBlogNavLinks.ascx" TagName="adminBlogNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Edit Blogs
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminBlogNavLinks ID="adminBlogNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Blogs</h2>
                <div class="AdminFormDescription">
                    <p>
                        Edit blog descriptions. Use <a href="applications.aspx">Applications Management</a> to create and edit core blog properties like appkey.  Use this form for blog-specific properties like display title, description and any access restrictions.
                    </p>
                </div>
            </div>
        </div>
        <div class="AdminFormLabel">
            Select Blog</div>
        <asp:DropDownList ID="ddlBlogs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBlogs_OnSelectedIndexChanged"
            CssClass="BigDropDown" Width="500px" />
        <div class="SlideshowDropDownNotes">
            To create a new blog use the applications management page</div>
        <div class="AdminFormInner">
            <table width="100%">
                <tr>
                    <td class="formLabel">
                        Title of Blog
                    </td>
                    <td class="formField">
                        <asp:TextBox ID="txtBlogTitle" runat="server" CssClass="BigTextBox" Width="500px" />
                        <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                            ControlToValidate="txtBlogTitle" />
                    </td>
                </tr>
                <tr class="EditorRow">
                    <td class="formLabel">
                        About
                    </td>
                    <td class="formField">
                        <cc2:TextEditor runat="server" ID="txtBlogDescription" />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Blog Owner Role
                    </td>
                    <td class="formField">
                        <asp:DropDownList ID="ddlBlogOwnerRole" runat="server" CssClass="BigDropDown" Width="300px" />
                        <div class="AdminUserFieldInfo">
                            Use <strong>BlogAdministrator</strong> or blank for all public blogs not requiring blog-specific administration.<br />"BlogAdministrator" is the default owner role assigned.</div>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Blog Access Role
                    </td>
                    <td class="formField">
                          <asp:TextBox ID="txtBlogAccessRole" runat="server" CssClass="BigTextBox" Width="200px" />
                        <div class="AdminUserFieldInfo">
                            Use blank for no blog read access restrictions</div>
                    </td>
                </tr>

                <tr>
                    <td class="formLabelChk">
                        
                    </td>
                    <td class="formField">
                        <asp:CheckBox ID="chkIsActive" runat="server" /> Active
                    </td>
                </tr>
                 <tr>
                    <td class="formLabelChk">
                       
                    </td>
                    <td class="formField">
                        <asp:CheckBox ID="chkIncludeInAggregateList" runat="server" />  Display Blog on Aggregate Page
                    </td>
                </tr>
                   <tr>
                    <td class="formLabelChk">
                        
                    </td>
                    <td class="formField">
                        <asp:CheckBox ID="chkRegisteredComments" runat="server" /> Require User Registration to Comment
                    </td>
                </tr>
                <tr><td></td><td>
                <div class="TextButtonBigArea">
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Update" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
                </td></tr>                
            </table>
        </div>
        
    </div>
</asp:Content>