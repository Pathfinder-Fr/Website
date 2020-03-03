<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieActivatePage" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div style="width: 100%; text-align: center;">
                <div class="MessageArea">
                    <div class="MessageAreaTitle">
                        <SUEETIE:SueetieLocal ID="SueetieLocal2" Key="activate_title" runat="server" />
                    </div>
                    <div class="MessageAreaMessage">
                        <p>
                            <asp:PlaceHolder ID="phActivated" runat="server">
                                <p>
                                <SUEETIE:SueetieLocal Key="activate_thankyou" runat="server" />
                                    </p>
                                
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phNot" runat="server">
                                <p>
                                <SUEETIE:SueetieLocal ID="SueetieLocal1" Key="activate_problem" runat="server" />
                                   </p>
                            </asp:PlaceHolder>
                        </p>
                    </div>
                </div>
            </div>

</asp:content>
