<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.AddonPack.Pages.BlockByAgentPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Agent Access
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Agents">
            <div class="AdminTextTalk">
                <h2>
                    Site Access By User Agent</h2>
                <div class="AdminFormDescription">
                    <p>
                        Enter crawler agents you want to block or prevent being logged. Agent matching processing
                        is case-insensitive. Use right-hand agent request table as a basis for your agent
                        filter strings. Can also clear all existing request data of the selected browser
                        agent.<br />
                    </p>
                </div>
            </div>
        </div>
        <div id="AgentsLeft"> 
            <asp:GridView ID="AgentsGridView" CssClass="fatgridviewMain agentTable" runat="server"
                AutoGenerateColumns="False" DataKeyNames="AgentID" EmptyDataText="No records found."
                AllowSorting="True" DataSourceID="AgentsDataSource" OnRowUpdating="AgentsGridView_OnRowUpdating"
                OnRowDataBound="AgentsGridView_OnRowDataBound">
                <RowStyle CssClass="gridRowStyle" />
                <SelectedRowStyle CssClass="gridrowSelectedBG" />
                <HeaderStyle CssClass="gridheaderBG" />
                <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
                <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="gridheaderBG" Width="1px" />
                        <ItemStyle CssClass="gridheaderBG" Width="1px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            ID
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("AgentID")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agent Excerpt">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAgent" runat="server" Text='<%# Bind("AgentExcerpt") %>' CssClass="ApplicationMediumInput"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%#Eval("AgentExcerpt")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Can Access Site" HeaderStyle-HorizontalAlign="Center">
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsBlocked" runat="server" ToolTip="Clear to block" Checked='<%#Flip(Eval("IsBlocked")) %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <img src='/images/shared/sueetie/<%#Flip(Eval("IsBlocked"))%>.png' alt='<%#Flip(Eval("IsBlocked"))%>' />
                        </ItemTemplate>
                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" UpdateText="Update"
                        DeleteText="Remove" ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="AgentsDataSource" runat="server" TypeName="Sueetie.AddonPack.AgentBlocker.BlockedAgents"
                SelectMethod="GetCrawlerAgentList" UpdateMethod="UpdateCrawlerAgent" InsertMethod="CreateCrawlerAgent"
                DeleteMethod="DeleteCrawlerAgent">
                <UpdateParameters>
                </UpdateParameters>
                <InsertParameters>
                </InsertParameters>
            </asp:ObjectDataSource>
            <div class="AddActivitiesForm">
                <div class="UploadTitle">
                    <strong>Add Crawler Agent</strong></div>
                <asp:DetailsView ID="UpdateAgentsDetailsView" runat="server" DataSourceID="AgentsDataSource"
                    DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="AgentsDetailsView_ItemInserting"
                    CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                    OnDataBound="UpdateAgentsDetailsView_OnDataBound">
                    <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                    <Fields>
                        <asp:BoundField DataField="AgentExcerpt" ShowHeader="true" HeaderText="Agent Excerpt"
                            ItemStyle-CssClass="aaMedium" />
                        <asp:TemplateField HeaderText="Can Access Site">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsBlocked" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                            ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                    </Fields>
                    <InsertRowStyle Width="100%"></InsertRowStyle>
                </asp:DetailsView>
            </div>
        </div>
        <div id="AgentsRight">
            <table border="1" id="AgentsTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            Count
                        </th>
                        <th>
                            UserAgent
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptAgents" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr id='<%# Eval("logid") %>'>
                            <td class="AgentsTableCountCol">
                                <%# Eval("count") %>
                            </td>
                            <td>
                            <a href="AgentPages.aspx?logid=<%# Eval("logid") %>" target="_blank">
                                <%# Eval("useragent") %></a>
                            </td>
                            <td>
                                <input type="button" onclick="Submit_Click('<%# Eval("logid") %>');return false;"
                                    value="Delete" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <script type="text/javascript">

                $('#AgentsTable').dataTable({
                    "aaSorting": [[0, "desc"]],
                    "aoColumns": [
						null,
						null
					]
                });

                function Submit_Click(rowID) {
                    $("#" + rowID).hide();
                    //$("#lblResult").text(rowID);
                    if (rowID != '') {
                        var ws = new Sueetie.Web.SueetieService();
                        ws.DeleteRequestAgents(rowID, displayResults);
                    }
                }

                function displayResults(result) {
                    $("#lblResult").text(result);
                }
            </script>
            <asp:Label ID="lblResult" ClientIDMode="Static" runat="server" CssClass="AgentsTableMessage" />
        </div>
    </div>
</asp:Content>
