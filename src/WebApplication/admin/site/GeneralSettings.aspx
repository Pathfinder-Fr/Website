<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralSettings.aspx.cs"
    Inherits="Sueetie.Web.GeneralSettings" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server">
</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    General Settings
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2> 
            General Site Settings</h2>
             <div class="AdminFormDescription">
                <p>
                    Specify general settings here.  All fields are required.</p>
            </div>
        <div class="AdminFormInner">
            <table width="100%" class="GeneralSettingsTable">
           <tr class="rwAddSpace">
                    <td class="formLabel">
                        Community Framework
                    </td>
                    <td class="formField">
                        <asp:Label ID="lblVersion" runat="server"  CssClass="BigFormText"  />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Site Name
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSiteName" class="adminHeavyText" Width="325px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtSiteName" />
                    </td>
                </tr>
                        <tr>
                    <td class="formLabel">
                        Site Page Title Lead
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSitePageTitleLead" class="adminHeavyText" Width="325px" />
                        <div class="AdminUserFieldInfo"><strong>Ex:</strong> [Sueetie]: Site Search. Precedes Page Titles on Sueetie non-application pages. (optional)</div>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Registration Type
                    </td>
                    <td class="formField">
                        <asp:RadioButtonList ID="rblRegistrationType" runat="server" />
                    </td>
                </tr>
             <tr>
                    <td class="formLabel">
                        Create Wiki User Account
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkCreateWikiAccount" runat="server" />
                        <div class="AdminUserFieldInfo">Creates user account in Screwturn Wiki /public/users.cs file on user registration</div>
                    </td>
                </tr>
         <tr>
                    <td class="formLabel">
                        Groups Folder Name
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtGroupsFolderName" class="adminHeavyText" Width="185px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtGroupsFolderName" />
                        <div class="AdminUserFieldInfo"><strong>Ex:</strong> http://site.com/<strong>[groupfoldername]</strong>/groupname</div>
                        
                    </td>
                </tr>
        <tr>
                    <td class="formLabel">
                        Default Language
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtDefaultLanguage" class="adminHeavyText" Width="125px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtDefaultLanguage" />
                        <div class="AdminUserFieldInfo">Name of /util/languages/<strong>[lang]</strong>/ folder. Default: en-US</div>
                        
                    </td>
                </tr>
        <tr>
                    <td class="formLabel">
                        Default TimeZone
                    </td>
                    <td class="formField">
                         <asp:DropDownList ID="ddTimeZones" runat="server" CssClass="BigDropDown" Width="500px" />
                        <div class="AdminUserFieldInfo">Default TimeZone to appear on site registration form</div>
                    </td>
                </tr>   
                 <tr>
                    <td class="formLabel">
                        Record Analytics Data
                    </td>
                    <td class="formField">
                        <asp:CheckBox ID="chkRecordAnalytics" runat="server" />
                        <div class="AdminUserFieldInfo">Clear if not intending to use Sueetie Analytics. Reduces size of database if not recording activity.</div>
                    </td>
                    </tr>             
                <tr>
                       <td class="formLabel">
                        www Handling
                        </td>
                        <td class="formField">
                            <asp:RadioButtonList ID="rblWwwSubdomain" runat="server" RepeatLayout="flow" RepeatDirection="horizontal">
                                <asp:ListItem Text="Remove" Value="remove" />
                                <asp:ListItem Text="Enforce" Value="add" />
                                <asp:ListItem Text="Ignore" Value="" Selected="true" />
                            </asp:RadioButtonList>
                    </td></tr>
                       <tr>
                    <td class="formLabel">
                        Geo Lookup Service Url
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtIpGeoLookupUrl" class="adminHeavyText" Width="325px" />
                          <div class="AdminUserFieldInfo">Used in conjunction with IP for lookup. Ex: <strong>http://mylookupservice.com/?ip=</strong>1.2.3.4</div>
                    </td>
                </tr>
                    <tr>
                    <td>
                    </td>
                    <td>
                        <div class="TextButtonBigArea">
                            <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="TextButtonBig"
                                OnClick="Submit_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:content>
