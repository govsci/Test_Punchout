<%@ Page Title="" Language="C#" MasterPageFile="~/Test_Punchout.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Test_Punchout._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Test Punchout</h2>
    <div style="color:red;font-weight:bold" id="error" runat="server"></div>
    <table>
        <tbody>
            <tr>
                <td><asp:Label ID="deploymentLabel" runat="server" AssociatedControlID="deploymentDropDown" Text="Select Deployment:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="deploymentDropDown" runat="server" OnSelectedIndexChanged="deploymentDropDown_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Test" Value="Test"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="customerLabel" runat="server" AssociatedControlID="customerDropDown" Text="Select Customer:"></asp:Label></td>
                <td><asp:DropDownList ID="customerDropDown" runat="server" OnSelectedIndexChanged="customerDropDown_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><asp:Label ID="itemLabel" runat="server" AssociatedControlID="" Text="Select Item:"></asp:Label></td>
                <td><asp:DropDownList ID="itemDropDown" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="submit" runat="server" Text="Test Punchout" OnClick="submit_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    
</asp:Content>
