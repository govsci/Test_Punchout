<%@ Page Title="" Language="C#" MasterPageFile="~/Test_Punchout.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Test_Punchout.cart._default" ValidateRequest="false" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        this.onload = function () {
            try {
                var parentWindow = parent.location.href;
                if (parentWindow.indexOf("/testPunchout/cart/", 1) == -1)
                    parent.location.href = "/testPunchout/cart/?errorID=1";
            }
            catch (err) { ; }
        }
    </script>
    <style type="text/css">
        .cartContents {
            border-collapse:collapse;
        }

        .cartContents td, .cartContents th {
            border:1px solid #c4c2c2;
            padding:5px;
        }

        .cartContents th {
            background-color:#f3f3f3;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a href="../">Home</a>
    <h2>Cart Contents</h2>
    <div id="error" runat="server" style="color:red;font-weight:bold;"></div>
    <asp:Table ID="cartContents" runat="server" CssClass="cartContents">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Manufacturer Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Manufacturer Part ID</asp:TableHeaderCell>
            <asp:TableHeaderCell>Supplier Part ID</asp:TableHeaderCell>
            <asp:TableHeaderCell>Supplier Part Auxiliary ID</asp:TableHeaderCell>
            <asp:TableHeaderCell>Description</asp:TableHeaderCell>
            <asp:TableHeaderCell>Unit of Measure</asp:TableHeaderCell>
            <asp:TableHeaderCell>Quantity</asp:TableHeaderCell>
            <asp:TableHeaderCell>Unit Price</asp:TableHeaderCell>
            <asp:TableHeaderCell>Classification</asp:TableHeaderCell>
            <asp:TableHeaderCell>Extrinsics</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table><br />
    <asp:Button ID="sendPoRequestButton" runat="server" Visible="false" OnClick="sendPoRequestButton_Click" Text="Submit PO" />
</asp:Content>
