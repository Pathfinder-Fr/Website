<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.AddonPack.Pages.BlockByIpPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>
<%@ Import Namespace="Sueetie.AddonPack" %>
<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Manually Entered Blocked IP Ranges
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Ranges">
            <div class="AdminTextTalk">
                <h2>
                    Site Access By Remote IP</h2>
                <div class="AdminFormDescription">
                    <p>
                        Edit, Add and Delete IP ranges not found in the imported IPs<br />
                    </p>
                </div>
            </div>
            <div id="RangesLeft">
                <asp:GridView ID="RangesGridView" CssClass="fatgridviewMain rangesTable" runat="server"
                    AutoGenerateColumns="False" DataKeyNames="IpID" EmptyDataText="No records found."
                    AllowSorting="True" DataSourceID="RangesDataSource" OnRowUpdating="RangesGridView_OnRowUpdating"
                    OnRowDataBound="RangesGridView_OnRowDataBound">
                    <RowStyle CssClass="gridRowStyle" />
                    <SelectedRowStyle CssClass="gridrowSelectedBG" />
                    <HeaderStyle CssClass="gridheaderBG" />
                    <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
                    <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
                    <Columns>
                        <asp:TemplateField HeaderText="IpStart" ItemStyle-CssClass="rangesCodeColumn" HeaderStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIpStart" runat="server" Text='<%# Bind("IpStart") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("IpStart")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IpEnd" ItemStyle-CssClass="rangesCodeColumn" HeaderStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIpEnd" runat="server" Text='<%# Bind("IpEnd") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("IpEnd")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Country" ItemStyle-CssClass="rangesCodeColumn" HeaderStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddBlockedIpCountries" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("CountryDescription")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" UpdateText="Update"
                            ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="RangesDataSource" runat="server" TypeName="Sueetie.AddonPack.IpBlocker.BlockedIPs"
                    SelectMethod="GetManualIpRangeList" UpdateMethod="UpdateManualIpRange" InsertMethod="CreateManualIpRange"
                    DeleteMethod="DeleteManualIpRange">
                    <UpdateParameters>
                        <asp:Parameter Name="IpStart" Type="String" />
                        <asp:Parameter Name="IpEnd" Type="String" />
                        <asp:Parameter Name="CountryID" Type="Int16" />
                        <asp:Parameter Name="IpID" Type="Object" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="IpStart" Type="String" />
                        <asp:Parameter Name="IpEnd" Type="String" />
                        <asp:Parameter Name="CountryID" Type="Int16" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <div class="AddActivitiesForm">
                    <div class="UploadTitle">
                        <strong>Add IP Range</strong> (All fields are required)</div>
                    <asp:DetailsView ID="UpdateRangesDetailsView" runat="server" DataSourceID="RangesDataSource"
                        DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="RangesDetailsView_ItemInserting"
                        CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                        OnDataBound="UpdateRangesDetailsView_OnDataBound">
                        <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                        <Fields>
                            <asp:BoundField DataField="IpStart" ShowHeader="true" HeaderText="IpStart" ItemStyle-CssClass="aaMedium" />
                            <asp:BoundField DataField="IpEnd" ShowHeader="true" HeaderText="IpEnd" ItemStyle-CssClass="aaMedium" />
                            <asp:TemplateField HeaderText="Country">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddBlockedIpCountries" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                                ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                        </Fields>
                        <InsertRowStyle Width="100%"></InsertRowStyle>
                    </asp:DetailsView>
                    <div class="refreshPageLink">
                        <a href="ManualIpRanges.aspx">Refresh Page</a>
                    </div>
                </div>
            </div>
            <div id="RangesRight">
                <table border="1" id="AgentsTable" class="AgentsTable">
                    <thead>
                        <tr>
                            <th>
                                Count
                            </th>
                            <th>
                                Remote IP
                            </th>
                            <th>
                                UserAgent
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptAgents" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <tr id='<%# Eval("logid") %>'>
                                <td class="AgentsTableCountCol">
                                    <%# Eval("count") %>
                                </td>
                                <td>
                                    <a href='<%# AddonPackHelper.FormatGeoUrl(Eval("remoteIP").ToString()) %>' target='_blank'>
                                        <%# Eval("remoteIP")%></a>
                                </td>
                                <td class="lightUrlCol">
                                    <a href='RemoteIpPages.aspx?ip=<%# Eval("remoteIP") %>' target='_blank'>
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
						{ "sSortDateType": "dom-text", "sType": "numeric" },
						null,
                        null
					]
                    });

                    function Submit_Click(rowID) {
                        $("#" + rowID).hide();
                        //$("#lblResult").text(rowID);
                        if (rowID != '') {
                            var ws = new Sueetie.Web.SueetieService();
                            ws.DeleteRequestIPs(rowID, displayResults);
                        }
                    }

                    function displayResults(result) {
                        $("#lblResult").text(result);
                    }
                </script>
                <asp:Label ID="lblResult" ClientIDMode="Static" runat="server" CssClass="AgentsTableMessage" />
            </div>
        </div>
    </div>
</asp:Content>
