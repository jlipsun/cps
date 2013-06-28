<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPage.aspx.cs" Inherits="ViewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Page</title>
    <script>
        function resizeWindow() {
            window.resizeTo(500, 750);
            toolbar = no;
            resizable = no;
        }
    </script>
</head>
<body onload="resizeWindow()";>
    <form id="form1" runat="server">
         <!-- Render C# code -->
    </form>
</body>
</html>
