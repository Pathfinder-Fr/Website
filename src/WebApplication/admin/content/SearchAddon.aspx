<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchAddon.aspx.cs" Inherits="Sueetie.Web.SearchAddon" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<%@ Register Src="../controls/adminContentNavLinks.ascx" TagName="adminAddonPackNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Search
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Sueetie Search Management</h2>
        <div class="AdminTextTalk">
            <p>
                Sueetie Search is a Lucene-based engine that enables you to retrieve
                content from all site applications in a single search. Before site content can be searched it must first be indexed, or added to the search database located by default at "/util/index." Indexing is performed on this page in conjunction with the background task you added when setting up the Add-on.</p>
                <p>You can use this page to perform a complete index or 
                add content after a specified date. Leave date blank to perform a complete
                index. If indexing by date, note that any existing post-dated items in the index are deleted
                and recreated, so enter start datetime in format "MM/DD/YYYY."  Additional 
                documentation located in the <a href="http://sueetie.com/wiki/AddonSueetieSearch.ashx">
                    Sueetie Wiki.</a></p>
                <div class="AdminFormFields">
                    <table>
                        <tr>
                            <td class="formLabel">
                                Enter Start Date
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStartDate" class="adminHeavyText" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <div class="TextButtonBigArea">
                                    <asp:Button ID="SubmitButton" runat="server" Text="Index" CssClass="TextButtonBig"
                                        OnClick="btnRebuildIndex_OnClick" />
                                    <asp:Button ID="Button1" runat="server" Text="Show Stats" CssClass="TextButtonBig"
                                        OnClick="btnShowStats_OnClick" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="AdminSrchResultMsgs">
                    <div class="AdminSearchResults">
                        <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
                    </div>
                    <div class="AdminSearchResults">
                        <asp:Label ID="lblResultsDetails" runat="server" Visible="false" CssClass="ResultsMessage" />
                    </div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>