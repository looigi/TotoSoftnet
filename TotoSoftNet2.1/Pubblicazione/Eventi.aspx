<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Eventi.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Eventi1" %>

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
		<li><i class="icon-bell"></i><a href="#">Eventi</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divDettaglio" runat="server" class="box span8" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Eventi</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdEventi" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Giornata" HeaderText="Giornata" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Progressivo" HeaderText="Progressivo" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Evento" >
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
            <asp:HiddenField ID="hdnUtente" runat="server" />
            <ul>
                <li>
                    <asp:Label ID="Label1" runat="server" Text="Giornata" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtGiornata" runat="server" CssClass ="label-medio " MaxLength="2" Width="100px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Evento" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtDescrizione" runat="server" CssClass ="label-medio" MaxLength="20" Enabled="False" Width="300px"></asp:TextBox>
                </li>
            </ul>
            <hr />
            <asp:Button id="cmdOK" class="btn btn-primary " runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="btn btn-primary" runat="server" Text="Annulla" />

            <div class="clear"></div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
