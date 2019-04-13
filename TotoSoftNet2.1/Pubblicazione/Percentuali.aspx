<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Percentuali.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Percentuali" %>

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
		<li><i class="icon-adjust"></i><a href="#">Percentuali</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Percentuali</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdPercentuali" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Percentuale" HeaderText="Percentuale" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModifica" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" width="40" OnClick ="AggiornaPercentuale" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>

    <div class="box span12" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Speciali</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdSpeciali" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Euro" HeaderText="Euro Per Giocatore" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModificaS" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" width="40" OnClick ="AggiornaSpeciali" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>

    <div id="divModifica" runat="server">
        <asp:HiddenField ID="hdnModalita" runat="server" />
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnUtente" runat="server" />
            <ul>
                <li>
                    <asp:Label ID="Label1" runat="server" Text="Descrizione" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtDescrizione" runat="server" CssClass ="label-medio " Enabled ="false" Width="300px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Percentuale" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtPercentuale" runat="server" CssClass ="label-medio" MaxLength="3" ></asp:TextBox>
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
