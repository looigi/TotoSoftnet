<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnIISM.Master" CodeBehind="NuovoUtente.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.NuovoUtente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-asterisk"></i>
			<a href="#">Nuovo Utente</a> 
			<i class="icon-angle-right"></i>
		</li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="padding: 5px; text-align:center; margin-top: 40px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Nuovo Utente</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box-content">
            <asp:Label ID="lblAvviso" runat="server" Text="Registrazione nuovo utente" CssClass ="label-titolo rosso"></asp:Label>
            <hr />
            <ul>
                <li>
                    <asp:Label ID="Label1" runat="server" Text="Nick" CssClass ="label-medio rosso" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtNick" runat="server" CssClass ="label-medio " MaxLength="20"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label2" runat="server" Text="Password" CssClass ="label-medio rosso" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass ="label-medio " MaxLength="30" TextMode="Password"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Nome" CssClass ="label-medio rosso" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtNome" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label4" runat="server" Text="Cognome" CssClass ="label-medio rosso" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtCognome" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label5" runat="server" Text="E-Mail" CssClass ="label-medio rosso" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtEMail" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                </li>
            </ul>
            <div class="span12" style="margin-top: 4px;">
                <asp:Label ID="lblMessaggio" runat="server" Text="E-Mail" CssClass ="label-medio rosso" Width="99%"></asp:Label>
                <asp:Button ID="cmdRegistra" runat="server" Text="Registra" CssClass ="btn btn-primary " />
                <asp:Button ID="cmdIndietro" runat="server" Text="Indietro" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</asp:Content>
