<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>
<script runat="server">
    void Page_Load()
    {
        byte[] delay = new byte[1];
        RandomNumberGenerator prng = new RNGCryptoServiceProvider();

        prng.GetBytes(delay);
        Thread.Sleep((int)delay[0]);

        IDisposable disposable = prng as IDisposable;
        if (disposable != null)
        {
            disposable.Dispose();
        }
        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.IsInRole("SueetieAdministrator"))
            {
                pnlSueetieAdmins.Visible = true;
                OutputErrorDetails();
            }
            else
            {
                pnlUsers.Visible = true;
            }
        }
    }

    private void OutputErrorDetails()
    {
        string contextItemKey = "LastErrorDetails";

        string errorDetails = (string)HttpContext.Current.Items[contextItemKey];

        if (!string.IsNullOrEmpty(errorDetails))
        {
            pDetails.InnerHtml = Server.HtmlEncode(errorDetails);
            pDetails.InnerHtml = errorDetails.Replace(Environment.NewLine, "<br />");
        }
    }
   
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type='text/css'>
        body
        {
            margin-top: 40px;
            font-size: 1.1em;
            font-family: "Lucida Grande" ,Helvetica,Arial,Verdana,sans-serif;
        }
        .ErrorTextOuter
        {
            width: 100%;
            text-align: center;
        }
        .ErrorText
        {
            width: 700px;
            margin-left: auto;
            margin-right: auto;
            text-align: left;
            display: block;
            overflow: hidden;
        }
        .ErrorText img
        {
            float: left;
        }
        .Text
        {
            float: left;
            margin-left: 20px;
            margin-top: 10px;
        }
        .TitleArea
        {
            overflow: hidden;
        }
        .Title
        {
            color: #D50031;
            float: left;
            font-size: 1.4em;
            letter-spacing: -0.05em;
            margin-top: 10px;
            padding-left: 20px;
        }
        .ExceptionDetails .ErrorType
        {
            color: #666;
            font-size: 1.6em;
            margin-bottom: 2em;
            font-weight: bold;
        }
        .ExceptionDetailsOuter
        {
            width: 100%;
            overflow: hidden;
            text-align: center;
        }
        .ExceptionDetails
        {
            width: 990px;
            text-align: left;
            clear: both;
            overflow:hidden;
            margin-top: 60px;
            font-size: .8em;
            font-family: Verdana;
            margin-left: auto;
            margin-right: auto;
        }
        .errorLabel
        {
            color: #D50031;
            font-weight: bold;
            font-size: 1em;
            width: 100%;
            overflow: hidden;
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <asp:Panel ID="pnlUsers" runat="server" Visible="false">
        <div class="ErrorTextOuter">
            <div class="ErrorText">
                <img src="/images/avatars/0t.jpg" />
                <div class="Text">
                    We're sorry, an error occurred while processing your request.<br />
                    Please use the contact form to let us know if it persists.</div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSueetieAdmins" runat="server" Visible="false">
        <div class="ErrorTextOuter">
            <div class="ErrorText">
                <img src="/images/avatars/0t.jpg" />
                <div class="Title">
                    Sueetie Administrator Eyes Only Error Display
                </div>
            </div>
        </div>
        <div class="ExceptionDetailsOuter">
        <div class="ExceptionDetails">
            <div class="ErrorType">
                Exception Details
            </div>
            <p id="pDetails" runat="server" >
            </p>
            </div>
        </div>
    </asp:Panel>
</body>
</html>
