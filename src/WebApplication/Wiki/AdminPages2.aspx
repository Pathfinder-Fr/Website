<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPages2.aspx.cs" Inherits="ScrewTurn.Wiki.AdminPages2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administration des pages v2</title>
    <style>
        body, table, input, select {
            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
            font-size: 11px;
        }

        fieldset label {
            padding-left: 10px;
            display: table-cell;
            vertical-align: middle;
            height: 22px;
        }

            fieldset label span {
                display: inline-block;
            }

        fieldset input[type='text'], fieldset select {
            border: 1px solid #666;
            padding: 2px;
        }

        fieldset input[type='submit'] {
        }

        table.grid {
            border-collapse: collapse;
            border: 1px solid #000;
        }

            table.grid .center {
                text-align: center;
            }

            table.grid td, table.grid th {
                border: 1px solid #ffffff;
            }

            table.grid thead {
            }

                table.grid thead th {
                    background-color: #ddd;
                }



            table.grid tbody tr:hover td, table.grid tbody tr:hover th {
                background-color: #f6f8c4;
            }

            table.grid tbody th {
                text-align: left;
                background-color: #ddd;
            }

            table.grid tbody td {
                background-color: #eee;
            }
    </style>
    <script src="http://code.jquery.com/jquery-1.10.1.min.js"></script>
</head>
<body>
    <div>
        <div id="debugPanel" runat="server" />
        <form>
            <fieldset>
                <legend>Filtrage</legend>
                <label>
                    <span>Namespace</span>
                    <select name="namespace" id="namespace" runat="server"></select>
                </label>
                <label>
                    <input name="orphanOnly" type="checkbox" id="orphanOnly" value="true" runat="server" />
                    <span>Orphelin</span>
                </label>
                <label>
                    <span>Nom</span>
                    <input name="name" type="text" id="name" runat="server" placeholder="nom système" />
                </label>
                <label>
                    <span>Titre</span>
                    <input name="title" type="text" id="title" runat="server" placeholder="titre affiché" />
                </label>
                <!--
                <label>
                    <span>Tri</span>
                    <select id="sortKey" name="sortKey" runat="server"></select>
                </label>
                <label>
                    <input id="sortDesc" name="sortDesc" type="checkbox" runat="server" value="true" />
                    <span>Décroissant</span>
                </label>
                -->
                <label>
                    <span>Nombre de résultat max</span>
                    <select id="pageSize" runat="server"></select>
                </label>
                <%--
                <label>
                    <span>Pages sortantes de</span>
                    <input id="incomingLink" type="text" placeholder="nom de la page" runat="server" />
                </label>
                <label>
                    <span>Page entrantes vers</span>
                    <input id="outgoingLink" type="text" placeholder="nom de la page" runat="server" />
                </label>
                --%>
                <div>
                    <input type="submit" />
                </div>
            </fieldset>

            <table style="width: 100%" class="grid">
                <thead>
                    <tr>
                        <th>Nom</th>
                        <th>Titre</th>
                        <th>Date création</th>
                        <th>Créé par</th>
                        <th>Date modification</th>
                        <th>Modifié par</th>
                        <th>Versions</th>
                        <th>Liens</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody id="tableBody" runat="server">
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="8">
                            <%--
                            <a id="firstLink" runat="server">&laquo;</a>
                            <a id="previousLink" runat="server">&lt;</a>
                            <a id="nextLink" runat="server">&gt;</a>
                            --%>
                            <asp:Literal ID="tableFooterRemark" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("table.grid a.delLink").on("click", function () { return confirm("Êtes-vous sur de vouloir supprimer cette page ? Cette action ne peut être annulée"); });
        });
    </script>
</body>
</html>
