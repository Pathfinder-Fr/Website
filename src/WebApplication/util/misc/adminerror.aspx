<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Threading" %>
<script runat="server">
    void Page_Load()
    {

        lblErrorFrom.Text = Request["Err"].ToString();
        lblErrorMessage.Text = Request["InnerErr"].ToString();

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
        #ErrorTextOuter
        {
            width: 100%;
            text-align: center;
        }
        #ErrorText
        {
            width: 700px;
            margin-left: auto;
            margin-right: auto;
            text-align: left;
        }
        #ErrorText img
        {
            float: left;
        }
        #Text
        {
            clear: both;
            margin-left: 20px;
            margin-top: 30px;
        }
        #TitleArea
        {
            overflow: hidden;
        }
        #Title
        {
            padding-left: 20px;
            font-size: 1.4em;
            color: #d50031;
            margin-top: 15px;
            letter-spacing: -.03em;
        }
        .ErrorType
        {
            color: #666;
            font-size: 1.2em;
            margin-top: 3em;
        }
    </style>
</head>
<body>
    <div id="ErrorTextOuter">
        <div id="ErrorText">
            <div id="TitleArea">
                <img src="/images/avatars/0t.jpg" />
                <div id="Title">
                    Sueetie Administrator Eyes Only Error Display
                </div>
            </div>
            <div id="Text">
                <div class="ErrorType">
                    Exception Inner Message:
                </div>
                <p>
                    <asp:Label ID="lblErrorFrom" runat="server" Text="Label" /></p>
                <div class="ErrorType">
                    Exception Message:
                </div>
                <p>
                    <asp:Label ID="lblErrorMessage" runat="server" Text="Label" /></p>
            </div>
        </div>
    </div>
</body>
</html>
