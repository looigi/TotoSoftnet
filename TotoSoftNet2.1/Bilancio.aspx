<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Bilancio.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Bilancio" %>

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
		<li><i class="icon-money"></i><a href="#">Bilancio</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span9" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Bilancio</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdBilancio" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="imgAvatar" runat="server" width="40" Height="40" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Utente" HeaderText="Utente" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="DataOra" HeaderText="Data" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Importo" HeaderText="€" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipologia" HeaderText="Tip." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Modalita" HeaderText="Mod." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Caricati" HeaderText="Car." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Progressivo" HeaderText="Progr." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgVai"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/Freccetta.Png" OnClick ="SelezionaRiga" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
        </div>
     </div>
   
    <div class="box span3" style="margin-left: 2px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Bilancio</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <ul>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label8" runat="server" Text="Conto Paypal" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblContoPayPal" runat="server" Text="" CssClass ="label-medio" Width="172px"></asp:Label>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label9" runat="server" Text="Buzzico" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblBuzzico" runat="server" Text="" CssClass ="label-medio" Width="172px"></asp:Label>
                </li>
            </ul>
        </div>
    </div>

    <div id="divTesto" runat="server">
        <div id="divBloccaFinestra2" class="bloccafinestra" runat="server" Visible="True"></div>

        <div id="divPopup2" class="popuptesto" runat="server" Visible="true">
            <asp:HiddenField ID="hdnProgressivo" runat="server" />
            <ul>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label4" runat="server" Text="Giocatore" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblGiocatore" runat="server" Text="" CssClass ="label-medio" Width="272px"></asp:Label>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label5" runat="server" Text="Data" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblData" runat="server" Text="" CssClass ="label-medio" Width="272px"></asp:Label>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label6" runat="server" Text="Data" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblImporto" runat="server" Text="" CssClass ="label-medio" Width="272px"></asp:Label>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label1" runat="server" Text="Tipologia" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:Label ID="lblTipologia" runat="server" Text="" CssClass ="label-medio" Width="272px"></asp:Label>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label2" runat="server" Text="Modalità" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:DropDownList ID="cmbModalita" runat="server" CssClass ="label-medio " Width="272px"></asp:DropDownList>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label7" runat="server" Text="Caricati" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:DropDownList ID="cmbCaricati" runat="server" CssClass ="label-medio " Width="272px"></asp:DropDownList>
                </li>
                <li style="margin-bottom: 7px;">
                    <asp:Label ID="label3" runat="server" Text="Note" CssClass ="label-medio rosso" Width="100px"></asp:Label>
                    <asp:TextBox ID="txtNote" runat="server" CssClass="label-medio" Height="59px" TextMode="MultiLine" Width="272px" MaxLength="1024"></asp:TextBox>
                </li>
            </ul>

            <hr />
            <asp:Button id="cmdOK" class="btn btn-primary " runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="btn btn-primary" runat="server" Text="Annulla" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
