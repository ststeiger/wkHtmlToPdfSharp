<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HtmlToPdfWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtHtml" style="width: 100%; height: 500px;" runat="server" TextMode="MultiLine" />
        <br />
        <asp:Button ID="btnConvert" runat="server" Text="Convert HTML to PDF" OnClick="btnConvert_Click" />
        
    </div>
    </form>
</body>
</html>
