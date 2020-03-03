<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Analytics.Pages.AnalyticsSettingsPage" %>

<%@ Register Src="../controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server">
</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Analytics Settings
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2> 
            Analytics Settings</h2>
             <div class="AdminFormDescription">
                <p>
                    Specify Sueetie Analytics settings here.  All fields are required.</p>
            </div>
        <div class="AdminFormInner">
            <table width="60%" class="SettingsAnalyticsTable">
           <tr class="rwAddSpace">
                    <td class="formLabel">
                        Sueetie Analytics
                    </td>
                    <td class="formField">
                        <asp:Label ID="lblVersion" runat="server"  CssClass="BigFormText"  />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                       Display Member-Only Activity By Default
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkMembersOnlyDisplay" runat="server" />
                    </td>
                </tr>
                  <tr>
                    <td class="formLabel">
                        Display Content Items By Default
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkContentOnlyDisplay" runat="server" />
                    </td>
                </tr>
                    <tr>
                    <td class="formLabel">
                        Report on Sueetie Admin Activity
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkLogAdminActivity" runat="server" />
                        <div class="AdminUserFieldInfo"><strong>Note:</strong> Admin page requests will not be logged if unchecked</div>
                    </td>
                </tr>
                  <tr>
                    <td class="formLabel">
                        Log Analytics Background Task Results
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkLogBackgroundTasks" runat="server" />
                        <div class="AdminUserFieldInfo">Enter successful completion of Analytics background tasks to Sueetie Event Log</div>
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
