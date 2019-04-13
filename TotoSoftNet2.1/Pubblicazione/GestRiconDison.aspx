<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="GestRiconDison.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.GestRiconDison" %>

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
		<li><i class="icon-coffee"></i><a href="#">Gestione Riconoscimenti/Disonori</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divDettaglio" runat="server" class="span8" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Eventi</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdRiconDison" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="img" runat="server" width="40" height="40" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="idPremio" HeaderText="id" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Immagine" HeaderText="Immagine" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Punti" HeaderText="Punti" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModifica" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" width="40" OnClick ="AggiornaEvento" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>

    <div id="divModifica" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnId" runat="server" />
            <ul>
                <li>
                    <asp:Label ID="Label1" runat="server" Text="Descrizione" CssClass ="label-medio rosso" Width="133px"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtDescrizione" runat="server" CssClass ="label-medio " MaxLength="50" Height="62px" TextMode="MultiLine" Width="250px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Immagine" CssClass ="label-medio rosso" Width="133px"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtImmagine" runat="server" CssClass ="label-medio" MaxLength="50" Width="250px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label2" runat="server" Text="Premio" CssClass ="label-medio rosso" Width="133px"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtPremio" runat="server" CssClass ="label-medio" MaxLength="6" Width="250px"></asp:TextBox>
                </li>
            </ul>
            <hr />
            <asp:Button id="cmdOK" class="btn btn-primary " runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="btn btn-primary " runat="server" Text="Annulla" />

            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
