<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSettings.aspx.cs" Inherits="Sueetie.Web.EmailSettings" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
    <%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Email Settings
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Email Settings</h2>
             <div class="AdminFormDescription">
                <p>
                    Specify email settings here.</p>
            </div>
        <div class="AdminFormInner">
            <table width="100%">
           <tr class="rwAddSpace">
                 
                    <td class="formLabel">
                        Site Contact Email
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtContactEmail" class="adminHeavyText" Width="225px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtContactEmail" />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        From Email Address
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtFromEmail" class="adminHeavyText" Width="225px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtFromEmail" />
                        <div class="AdminUserFieldInfo"><strong>Note:</strong> Most SMTP servers require this address to be a valid, known account</div>
                        
                    </td>
                </tr>     
                <tr>
                    <td class="formLabel">
                        From Email Name
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtFromName" class="adminHeavyText" Width="225px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtFromName" />
                         <div class="AdminUserFieldInfo"><strong>Ex:</strong> Community Support Services.  Used in conjunction with From Email Address.</div>
                    </td>
                </tr>                                
                 <tr class="rwAddSpace">
                 
                    <td class="formLabel">
                        SMTP Server
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSmtpServer" class="adminHeavyText" Width="225px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtSMTPServer" />
                               <div class="AdminUserFieldInfo"><strong>Ex:</strong> smtpmailserver.myhost.com</div>
                    </td>
                </tr>
                         <tr>
                    <td class="formLabel">
                        Server Username
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSmtpUserName" class="adminHeavyText" Width="155px" />
                    </td>
                </tr>     
                            <tr>
                    <td class="formLabel">
                        Server Password
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSmtpPassword" class="adminHeavyText" Width="155px" />
                    </td>
                </tr>         
            <tr>
                    <td class="formLabel">
                        Port
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtSmtpServerPort" class="adminHeavyText" Width="65px" />
                         <div class="AdminUserFieldInfo">If Port 25 is not used</div>
                    </td>
                </tr>                                
     <tr>
                    <td class="formLabel">
                        Enable SSL
                    </td>
                    <td class="formCheckField">
                        <asp:CheckBox ID="chkEnableSSL" runat="server" />
                    </td>
                </tr>
             <tr class="rwAddSpace">
                 
                    <td class="formLabel">
                        Error Email Recipients
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtErrorEmails" class="adminHeavyText" Width="345px" />
                         <div class="AdminUserFieldInfo"><strong>Ex:</strong> bob@yoursite.com, tim@yoursite.com  (separated by commas)</div>
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
</asp:Content>
