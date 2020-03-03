<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Trash.aspx.cs" Inherits="BlogEngine.Admin.Trash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
	<div class="content-box-outer">
		<div class="content-box-full">

            <h1>Trash</h1>
            <% if (true){ %>
            <div class="tableToolBox"> 
                Show : <a id="All" class="current" href="#" onclick="LoadTrash(this)">All</a> | 
                <a id="Post" href="#" onclick="LoadTrash(this)">Posts</a> | 
                <a id="Page" href="#" onclick="LoadTrash(this)">Pages</a> |
                <a id="Comment" href="#" onclick="LoadTrash(this)">Comments</a>
                <div class="Pager"></div>
            </div>
            <%} %>
            <div id="Container"></div>
            <div class="Pager"></div>

        </div>
    </div>

    <script type="text/javascript">
        LoadTrash(null);
    </script>

</asp:Content>
