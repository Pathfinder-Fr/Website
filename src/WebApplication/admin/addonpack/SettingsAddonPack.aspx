<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.AddonPackSettingsPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server">
</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Addon Pack Settings
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2> 
            Addon Pack Settings</h2>
             <div class="AdminFormDescription">
                <p>
                    Specify Addon Pack settings here.  All fields are required.</p>
            </div>
        <div class="AdminFormInner">
            <table width="100%" class="SettingsAddonPackTable">
           <tr class="rwAddSpace">
                    <td class="formLabel">
                        Addon Pack Release
                    </td>
                    <td class="formField">
                        <asp:Label ID="lblVersion" runat="server"  CssClass="BigFormText"  />
                    </td>
                </tr>
                    <tr>
                    <td class="formLabel">
                        Report Requests
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtRequestReportRecs" class="adminHeavyText" Width="100px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtRequestReportRecs" />
                          <div class="AdminUserFieldInfo">Records retrieved in Site Access Control Recent Requests Report. Default: <strong>500</strong></div>
                    </td>
                </tr>
                    <tr>
                    <td class="formLabel">
                        Enable Forum Answers
                    </td>
                    <td class="formField">
                        <asp:CheckBox runat="server" ID="chkEnableForumAnswers" />
                            <div class="AdminUserFieldInfo">Forum Answers enable users to mark posts as answers. Default: <strong>false</strong></div>
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
