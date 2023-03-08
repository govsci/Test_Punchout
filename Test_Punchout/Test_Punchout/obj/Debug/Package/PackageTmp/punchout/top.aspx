<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="Test_Punchout.punchout.top" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function returnGSS() {
	        parent.location.href="/testPunchout/default.aspx";
        }
    </script>
    <style type="text/css">
        body {
            margin:0;
        }

        #returnToGssDiv {
            width: 100%;
            height: 30px;
            background: #f3f3f3;
        }

        #returnToGssA {
            text-decoration: none;
            font-weight: normal;
            border: 1px solid #f58220;
            font-family: MyriadPro-Cond;
            font-size: 25px;
            color: #FFFFFF;
            background-color: #e66e08;
            border-radius: 5px;
            padding: 0px 10px;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
            -o-transition: all 0.5s;
            transition: all 0.5s;
        }
    </style>
</head>
<body style="background-color:#002157;">
    <div id="returnToGssDiv">
        <center>
	        <a id="returnToGssA" href="javascript:returnGSS()">Return to Test Site</a>
        </center>
    </div>
</body>
</html>
