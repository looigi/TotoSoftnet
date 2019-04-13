<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="NuovoAnno.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.NuovoAnno" %>

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
		<li><i class="icon-asterisk"></i><a href="#">Nuovo Anno</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Dati nuovo anno</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <ul>
                    <li>
                        <asp:Label ID="Label7" runat="server" Text="Numero nuovo anno" CssClass ="label-medio rosso" Width="200px"></asp:Label>
                        <asp:TextBox ID="txtNumeroAnno" runat="server" CssClass ="label-medio " MaxLength="2" Enabled="False" Width="20px"></asp:TextBox>
                    </li>
                    <li>
                        <asp:Label ID="Label8" runat="server" Text="Descrizione" CssClass ="label-medio rosso" Width="200px"></asp:Label>
                        <asp:TextBox ID="txtDescrizione" runat="server" CssClass ="label-medio " MaxLength="50" Width="200px"></asp:TextBox>
                    </li>
                    <li>
                        <asp:Label ID="Label1" runat="server" Text="Anno inizio" CssClass ="label-medio rosso" Width="200px"></asp:Label>
                        <asp:TextBox ID="txtAnnoInizio" runat="server" CssClass ="label-medio " MaxLength="4" Width="80px"></asp:TextBox>
                    </li>
                </ul>
                <hr />
                <div class="barratasti draggable">
                    <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass ="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
