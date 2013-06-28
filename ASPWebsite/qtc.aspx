<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qtc.aspx.cs" Inherits="qtc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>QTC</title>
    <link rel="stylesheet" href="css/jquery-ui.css" />
    <script src="js/jquery-1.9.1.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#tabs").tabs();
        });
        ;
    </script>

    <script type="text/javascript">

        function openNewWindow() {
            document.forms[0].target = "_blank";
            //window.open("url", "windowname", "width=600,height=370,shortcuts=no,toolbar=no,location=no,status=no,directories=no,resizable=no,copyhistory=no,menubar=no,scrollbars=no,top=10,left=10")
            //javascript: window.open("ViewPage.aspx?uniqueID=" + uniqueID.Value , "windowname", "width=600,height=370,shortcuts=no,toolbar=no,location=no,status=no,directories=no,resizable=no,copyhistory=no,menubar=no,scrollbars=no,top=10,left=10")

        }

    </script>
    <link rel="stylesheet" type="text/css" href="qtc.css" />
</head>
<body>
    <img src="images/qtc_logo.png" alt="Logo" />
    <br />
    <br />

    <form id="Form1" runat="server">
        <div id="tabs">
            <ul>
                <!--<li><a href="#tabs-1">Incoming Requests</a></li>-->
                <li><a href="#tabs-1" style="color: white">Incoming Requests</a></li>
                <li><a href="#tabs-2" style="color: white">Triage</a></li>
                <li><a href="#tabs-3" style="color: white">Followup</a></li>
            </ul>
            <div id="tabs-1">
                <h3>Incoming Requests</h3>
                <div style="width: 100%; height: 500px; overflow: auto;">
                    <asp:Repeater ID="incomingRepeater" runat="server" OnItemCommand=" incomingRepeater_ItemCommand">
                        <HeaderTemplate>
                            <table width="100%" style="font: 8pt verdana" border="1">
                                <tr style="background-color: DFA894">
                                    <!--Document ID, Claim #, Contract #, LOB, Date of Request, Request Type, Created By, Actions-->
                                    <th>Document ID</th>
                                    <th>Claim #</th>
                                    <th>Contract #</th>
                                    <th>LOB</th>
                                    <th>Date of Request</th>
                                    <th>Request Type</th>
                                    <th colspan="3">Actions</th>
                                </tr>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="background-color: FFECD8">

                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem,"document_id") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "claim_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "contract_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "LOB") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "date_of_request") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "request_type") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center>

                                <asp:HiddenField ID="uniqueID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                <asp:Button ID="ViewButton" runat="server" Text="View" UseSubmitBehavior="false" CommandName="ViewButton" OnClientClick="openNewWindow()" />
                                <!--onclientclick="aspnetForm.target='_blank';"-->
                                <!--/onclientclick="callThis()-->

                                <!--<div id="popup_element" style="display: none">

                                    <a class="b-close">x<a />
                                        <p>DocumentId: <%# DataBinder.Eval(Container.DataItem,"document_id") %> </p>
                                        <p>Claim #: <%# DataBinder.Eval(Container.DataItem, "claim_number") %> </p>
                                        <p>Contract #: <%# DataBinder.Eval(Container.DataItem, "contract_number") %> </p>
                                        <p>Exam Date of Request: </p>
                                </div>-->
                                </center>
                                </td>
                                <td>
                                    <center><asp:Button ID="Approve" runat="server" Text="Approve" UseSubmitBehavior="false" CommandName="Approve" /></center>
                                </td>
                                <td>
                                    <center><asp:Button ID="Reject" runat="server" Text="Reject" UseSubmitBehavior="false" CommandName="Reject" /></center>
                                </td>


                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </Table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div id="tabs-2">
                <h3>Approved Requests</h3>
                <div style="width: 100%; height: 500px; overflow: auto;">
                    <asp:Repeater ID="triageRepeater" runat="server">
                        <HeaderTemplate>
                            <table width="100%" style="font: 8pt verdana" border="1">
                                <tr style="background-color: DFA894">
                                    <th>Document Id</th>
                                    <th>Claim #</th>
                                    <th>Contract #</th>
                                    <th>LOB</th>
                                    <th>Date of Request</th>
                                    <th>Request Type</th>
                                </tr>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="background-color: FFECD8">
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem,"document_id") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "claim_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "contract_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "LOB") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "date_of_request") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "request_type") %></center>
                                </td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </Table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div id="tabs-3">
                <h3>Rejected Requests</h3>
                <div style="width: 100%; height: 500px; overflow: auto;">
                    <asp:Repeater ID="followupRepeater" runat="server">
                        <HeaderTemplate>
                            <table width="100%" style="font: 8pt verdana" border="1">
                                <tr style="background-color: DFA894">
                                    <th>Document Id</th>
                                    <th>Claim #</th>
                                    <th>Contract #</th>
                                    <th>LOB</th>
                                    <th>Date of Request</th>
                                    <th>Request Type</th>
                                </tr>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="background-color: FFECD8">
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem,"document_id") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "claim_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "contract_number") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "LOB") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "date_of_request") %></center>
                                </td>
                                <td style="text-align: justify">
                                    <center><%# DataBinder.Eval(Container.DataItem, "request_type") %></center>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </Table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <br />
        <div class="subFooter" style="background-color: #a6a9ab; padding-bottom: 55px; color: #FFFFFF">
            <div style="float: left; padding-top: 18px; padding-left: 10px">
                <small><a href="/contact/pages/privacy.aspx" style="color: #FFFFFF">Privacy</a> | 
							<a href="/contact/Pages/Terms-of-Service.aspx" style="color: #FFFFFF">Terms of Use</a> | QTC 2013 &copy; </small>
            </div>
            <div style="float: right; padding-top: 18px; padding-right: 10px;">
                <img src="/images/QTCM_footerLogo.png" alt="Footer Logo" class="footerLogo" />
            </div>
        </div>
        <!--<div id="footerBG"></div>-->
    </form>
</body>
</html>

