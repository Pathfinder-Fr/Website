<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.SueetieInstall"
    Title="Install Sueetie Version 3.2" %>

<script runat="server">

    void Page_Load(object sender, EventArgs e)
    {
        if (Sueetie.Core.DataHelper.GetIntFromQueryString("DBERROR", -1) > 0)
            lblDBError.Visible = true;
        
        ENABLE_SETUP = true;
        ENABLE_LINKS = true;

        pnlDisabled.Visible = !ENABLE_SETUP;
        pnInstall.Visible = ENABLE_SETUP;

    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="install.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlDisabled" runat="server" Visible="false">
        <div class="SueetieInstallBodyAreaOuter">
            <div class="SueetieInstallBodyArea">
                <div class="SueetieInstallBodyAreaInner">
                    <div class="InstallerDisabled">
                        <h1>
                            Sueetie Installer disabled...</h1>
                        <p>
                            Re-enable setup by setting ENABLE_SETUP value to "true" in this .aspx page
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnInstall" runat="server" Visible="false">
        <div class="SueetieInstallBodyAreaOuter">
            <div class="SueetieInstallBodyArea">
                <div class="SueetieInstallBodyAreaInner">
                    <h1>
                        Install Sueetie Version 3.2</h1>
                    <div class="SeeWiki">
                        Complete setup details are available on the <a href="http://sueetie.com/wiki/GummyBearSetup.ashx">
                            Sueetie Installation Guide.</a> Also visit the <a href="http://sueetie.com/forum/default.aspx?g=topics&f=14">
                                Sueetie Setup and Configuration Forum</a> for assistance and to share your
                        experience with others.
                    </div>
                                        <h2 class="InstallPrep">
                        Setup Preparation</h2>
                    <div class="SueetieInstallColumnArea">
                        <div class="LeftBodyArea">
                            <p>
                                <img src="sueetie30iis.png" /></p>
                        </div>
                        <div class="RightBodyArea">
                        <asp:Label ID="lblDBError" CssClass="DBErrorArea" Visible="false" runat="server">
                        Uh-oh! Your database connection is not valid, preventing you from installing Sueetie.  Perhaps you haven't updated your connection strings yet.  Please confirm you've completed the pre-flight instructions and then attempt to load http://yoursite's home page, which will re-direct you here.  If your root database connection is valid you will be redirected to this page, but without this nasty notice.
                        </asp:Label>
                            <h4>
                                1) Verify that the .NET Framework Version you're using is .NET 4.0</h4>
                            <p>
                                Sueetie 3.2 requires .NET 4.0 (Version 4.0.30319.)
                            </p>
                            <h4>
                                2) Configure IIS application directories</h4>
                            <p>
                                At left is a screenshot of a Sueetie 3.2 Website in IIS (atomo) showing directories
                                that must be set as IIS applications. Remember to use the same .NET 4.0 application
                                pool when configuring the directories.</p>
                            <h4>
                                3) Update Connection Strings</h4>
                            <p>
                                It is assumed you already created and configured your SQL database to support the
                                website. Copy down your connection string to update the Sueetie SQL connection strings
                                found in the locations below. You can retrieve those files quickly in Notepad++
                                or Visual Studio by searching for "SueetieDB" with a *.config filter. The Sueetie
                                Connection String is identical in all locations so you can perform a single search
                                and replace operation.</p>
                            <p class="ConnectionStrings">
                                \util\config\connections.config
                                <br />
                                \util\config\Sueetie.config
                                <br />
                                \forum\db.config
                                <br />
                                \media\connections.config
                                <br />
                                \wiki\Web.config
                            </p>
                        </div>
                    </div>

                    <div class="SetupMenu">
                        <h2 class="InstallPrep">
                            Install Sueetie</h2>
                        <p>
                            Now that we're done with Setup Prep, let's <a href="/forum/install/sueetie.aspx">install Sueetie!</a></p>
                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>
    </form>
</body>
</html>
