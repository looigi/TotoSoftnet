<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Errore.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Errore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Errore</title>

    <link rel="stylesheet" href="App_Themes/standard/Commercial.css" />
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div style="border-style: solid; border-width: thin; background-color: #CCCCCC; padding: 10px; margin:10px;">
            <asp:Label ID="Label1" runat="server" Text="...ERRORE..." Font-Bold="True" 
                Font-Names="Verdana" Font-Size="30pt" ForeColor="Red" ></asp:Label>
            <br />
            <span class="etichetta">Errore: </span>
            <asp:Label ID="lblErrore" runat="server" Text="Label" Font-Names="Verdana" Font-Size="15pt"></asp:Label>
            <br />
            <span class="etichetta">Utente: </span>
            <asp:Label ID="lblUtente" runat="server" Text="Label" Font-Names="Verdana" Font-Size="15pt"></asp:Label>
            <br />
            <span class="etichetta">Chiamante: </span>
            <asp:Label ID="lblChiamante" runat="server" Text="Label" Font-Names="Verdana" Font-Size="15pt"></asp:Label>
            <br />
            <span class="etichetta">SQL: </span>
            <asp:Label ID="lblSQL" runat="server" Text="Label" Font-Names="Verdana" Font-Size="15pt"></asp:Label>
        </div>
    </center>
    </form>
</body>
</html>
