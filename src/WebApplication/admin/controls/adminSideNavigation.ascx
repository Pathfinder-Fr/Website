<%@ Control Language="C#" AutoEventWireup="true" %>
<script runat="server" language="C#">
    public int ActiveAccordionPanel = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        AccordionAdminMenu.SelectedIndex = ActiveAccordionPanel;
    }
   
</script>
<AjaxControlToolkit:Accordion ID="AccordionAdminMenu" runat="server" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent" FadeTransitions="true" FramesPerSecond="60" TransitionDuration="60" AutoSize="None">
    <Panes>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane1" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Membership</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/users/default.aspx">Membership Home</a></div>
                <div class="accordionLink"><a href="/admin/users/RegisteredUsers.aspx">Registered Users</a></div>
                <div class="accordionLink"><a href="/admin/users/AddUsers.aspx">Add New Users</a></div>
                <div class="accordionLink"><a href="/admin/users/SearchUsers.aspx">Search Users</a></div>
                <div class="accordionLink"><a href="/admin/users/NewUsers.aspx">Display New Users</a></div>
                <div class="accordionLink"><a href="/admin/users/RoleAdmin.aspx">Role Management</a></div>
                <div class="accordionLink"><a href="/admin/users/ApproveUsers.aspx">Approve Users</a></div>
                <div class="accordionLink"><a href="/admin/users/BannedIPs.aspx">Banned IPs</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane2" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Site Settings</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/site/default.aspx">Site Settings Home</a></div>
                <div class="accordionLink"><a href="/admin/site/GeneralSettings.aspx">General Site Settings</a></div>
                <div class="accordionLink"><a href="/admin/site/Seo.aspx">SEO Settings</a></div>
                <div class="accordionLink"><a href="/admin/site/EmailSettings.aspx">Email Settings</a></div>
                <div class="accordionLink"><a href="/admin/site/Themes.aspx">Update Current Theme</a></div>
                <div class="accordionLink"><a href="/admin/site/RestartApp.aspx">Restart Sueetie</a></div>
                <div class="accordionLink"><a href="/admin/site/Licenses.aspx">Sueetie Product Keys</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane5" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Content Management</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/content/default.aspx">Content Management Home</a></div>
                <div class="accordionLink"><a href="/admin/content/ContentParts.aspx">Content Parts</a></div>
                <div class="accordionLink"><a href="/admin/content/ActivityLogging.aspx">Logged Activity Categories</a></div>
                <div class="accordionLink"><a href="/admin/content/ContentGroups.aspx">Content Pages</a></div>
                <div class="accordionLink"><a href="/admin/content/SearchAddon.aspx">Sueetie Search</a></div>
                <div class="accordionLink"><a href="/admin/content/CalendarEdit.aspx">Sueetie Event Calendars</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane3" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Applications</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/applications/default.aspx">Applications Home</a></div>
                <div class="accordionLink"><a href="/admin/applications/Applications.aspx">Site Applications</a></div>
                <div class="accordionLink"><a href="/admin/applications/blogs.aspx">Blogs</a></div>
                <div class="accordionLink"><a href="/admin/applications/wikis.aspx">Wikis</a></div>
                <div class="accordionLink"><a href="/admin/applications/media.aspx">Media and Documents</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane4" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Reports</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/reports/default.aspx">Reports Home</a></div>
                <div class="accordionLink"><a href="/admin/reports/BackgroundTasks.aspx">Background Tasks</a></div>
                <div class="accordionLink"><a href="/admin/reports/EventLogs.aspx">Event Logs</a></div>
                <div class="accordionLink"><a href="/admin/reports/Downloads.aspx">Downloads</a></div>
                <div class="accordionLink"><a href="/admin/reports/UserActivities.aspx">User Activities</a></div>
                <div class="accordionLink"><a href="/admin/reports/Subscribers.aspx">Newsletter Subscribers</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane6" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Addon Pack</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/addonpack/default.aspx">Addon Pack Home</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/settingsaddonpack.aspx">Addon Pack Settings</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/siteaccesscontrol/default.aspx">Site Access Control</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/slideshows/default.aspx">Slideshows</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/blogs/default.aspx">Blog Post Thumbnails</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/forumanswers/default.aspx">Forum Answers</a></div>
                <div class="accordionLink"><a href="/admin/addonpack/mediasets/default.aspx">Media Sets</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane7" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Analytics</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/analytics/default.aspx">Analytics Home</a></div>
                <div class="accordionLink"><a href="/admin/analytics/settingsanalytics.aspx">Analytics Settings</a></div>
                <div class="accordionLink"><a href="/admin/analytics/maintenance/default.aspx">Data Maintenance</a></div>
                <div class="accordionLink"><a href="/admin/analytics/reports/default.aspx">Reports</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
        <AjaxControlToolkit:AccordionPane ID="AccordionPane8" runat="server">
            <Header>
                <a href="" onclick="return false;">
                    <img src="/images/admin/reddown.gif" alt="" border="0px" align="right" />Marketplace</a>
            </Header>
            <Content>
                <div class="accordionLink"><a href="/admin/marketplace/default.aspx">Marketplace Home</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/manageproducts.aspx">Manage Products</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/addnewproduct.aspx">Add New Product</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/managecategories.aspx">Manage Categories</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/paymentservices.aspx">Payment Services</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/activityreport.aspx">Activity Report</a></div>
                <div class="accordionLink"><a href="/admin/marketplace/settingsmarketplace.aspx">Marketplace Settings</a></div>
            </Content>
        </AjaxControlToolkit:AccordionPane>
    </Panes>
</AjaxControlToolkit:Accordion>
