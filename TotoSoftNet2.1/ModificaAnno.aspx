<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ModificaAnno.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ModificaAnno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-user-md"></i>
			<a href="PulsantieraAmministrazione.aspx">Amministrazione</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-coffee"></i><a href="#">Gestione Anno</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span8">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Dati personali</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <ul>
                <li>
                    <asp:Label ID="Label8" runat="server" Text="Descrizione Anno" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtDescAnno" runat="server" CssClass ="label-medio " MaxLength="30" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label10" runat="server" Text="Inizio Anno" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtInizioAnno" runat="server" CssClass ="label-medio " MaxLength="4" ></asp:TextBox>
                </li>

                <li>
                    <asp:Label ID="Label1" runat="server" Text="Giornate" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtGiornate" runat="server" CssClass ="label-medio " MaxLength="2" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Max Doppie" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtDoppie" runat="server" CssClass ="label-medio" MaxLength="2" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label2" runat="server" Text="Costo giocata" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtCostoGiocata" runat="server" CssClass ="label-medio" MaxLength="5" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label4" runat="server" Text="P/V" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtPuntoVirgola" runat="server" CssClass ="label-medio" MaxLength="1" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label5" runat="server" Text="Giornata Semifinale" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtGiornataSemifinale" runat="server" CssClass ="label-medio" MaxLength="2" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label6" runat="server" Text="Giornata Finale" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtGiornataFinale" runat="server" CssClass ="label-medio" MaxLength="2" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label7" runat="server" Text="Costo per speciali" CssClass ="label-medio rosso" Width="194px"></asp:Label>
                    <asp:TextBox ID="txtSpeciali" runat="server" CssClass ="label-medio" MaxLength="5" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label9" runat="server" Text="Sfondo" CssClass ="label-medio rosso"></asp:Label>
                    <br />
                    <asp:Image ID="imgSfondo" runat="server" Width="80" />
                    <br />
                    <asp:Label ID="Label11" runat="server" Text="Sfondo" CssClass ="label-medio rosso"></asp:Label>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
<%--                    &nbsp;
                    <asp:ImageButton ID="imgUpload1" runat="server" src="App_Themes/Standard/Images/Icone/icona_DOWNLOAD-TAG.png" Width="20px" />--%>
                </li>
            </ul>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="cmdOK" runat="server" Text="Salva" CssClass="btn btn-primary" OnClick="cmdOK_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="cmdOK" />  
                </Triggers>
            </asp:UpdatePanel> 
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
