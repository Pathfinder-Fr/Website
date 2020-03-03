<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieMessagePage" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div style="width: 100%; text-align: center;">
                <div class="MessageArea">
                    <div class="MessageAreaTitle">
                        <asp:Label ID="lblWelcome" runat="server" />        
                    </div>
                    <div class="MessageAreaMessage">
                      <asp:Label ID="lblMessage" runat="server" />         
                    </div>
            </div>
</div>
</asp:content>
