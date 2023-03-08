<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="punchout-shopping-request.aspx.cs" Inherits="Test_Punchout.punchout.punchout_shopping_request" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Security-Policy" content="upgrade-insecure-requests"/>
</head>
<frameset rows="30px,*" style="border:none;" frameborder="0" framespacing="0" sandbox="allow-forms allow-scripts allow-same-origin allow-top-navigation">
    <frame src="top.aspx" name="top" id="top" />
    <frame id="shopping" name="shopping" runat="server" src="" 
        sandbox="allow-forms allow-scripts allow-same-origin allow-top-navigation" />
</frameset>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
