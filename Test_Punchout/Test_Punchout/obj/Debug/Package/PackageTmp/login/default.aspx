<%@ Page Title="" Language="C#" MasterPageFile="~/Test_Punchout.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Test_Punchout.login._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Login</h2>
    <div id="error" runat="server" style="color:red;font-weight:bold"></div>
    <table>
        <tbody>
            <tr>
                <td><asp:Label ID="usernameLabel" runat="server" Text="Username:" AssociatedControlID="userNameTextbox"></asp:Label></td>
                <td><asp:TextBox id="userNameTextbox" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="passwordLabel" runat="server" Text="Password:" AssociatedControlID="passwordTextbox"></asp:Label></td>
                <td><asp:TextBox id="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="submit" runat="server" Text="Login" OnClick="submit_Click" /></td>
            </tr>
        </tbody>
    </table>
</asp:Content>
