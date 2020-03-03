<%@ Application Language="C#" %>
<%@ Import Namespace="Sueetie.Core" %>

<script RunAt="server">

    private SueetieTaskScheduler _scheduler = null;

    void Application_Start(object sender, EventArgs e)
    {
        bool isSiteInstalled = SueetieCommon.IsSiteInstalled();
        if (!isSiteInstalled)
        {
            return;
        }

        SueetieLogs.LogSiteEntry(SiteLogType.General, SiteLogCategoryType.AppStartStop, "Sueetie WebApplication Started");

        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(Server.MapPath("~/util/config/tasks.config"));
        this._scheduler = new SueetieTaskScheduler(doc);
        this._scheduler.StartTasks();
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        // SueetieLogs.LogRequest(sender as HttpApplication);
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {

    }
</script>

