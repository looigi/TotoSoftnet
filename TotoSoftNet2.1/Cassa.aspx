<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Cassa.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Cassa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
			<i class="icon-user"></i>
			<a href="PulsantieraPersonale.aspx">Personale</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-money"></i><a href="#">Cassa</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentHead"  runat="server">
    <style type="text/css">
        #ctl00_contentCentrale_Menu1 {
            margin-bottom: 2px;
            width:99%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="text-align:center;">
        <asp:Label ID="Label3" runat="server" Text="Pagamenti On-Line" CssClass ="label-info" Width="130px" ></asp:Label>
        <asp:ImageButton ID="imgMostraNascondePag" runat="server" ImageUrl="App_Themes/Standard/Images/icone/PayPal.png" width="60px" Height="60px"/>
        <br />
        <div id="divPagamento" runat="server" class="mascherinarossa ">
            <ul>
                <li>
                    <asp:Label ID="Label2" runat="server" Text="Importo" CssClass ="etichettatitoloaccount" Width="80px"></asp:Label>
                    <asp:TextBox ID="txtImporto" runat="server" CssClass ="casellatesto " MaxLength="5"></asp:TextBox>
                </li>
            </ul>
            <hr />
            <asp:Label ID="Label4" runat="server" Text="Nota: PayPal prevde un addebito per la ricarica di 40 centesimi di euro per le operazioni effettuate on-line.<br />Se si vuole evitare questo pagamento si dovrà effettuare la ricarica tramite il proprio conto PayPal, se se ne possiede uno." CssClass ="etichettaavviso"  Width="330px" ></asp:Label>
            <hr />
            <asp:Button id="cmdPaga" class="bottone" runat="server" Text="Paga con PayPal" />
        </div>
    </div>

    <div class="row-fluid sortable ui-sortable">				
		<div class="span12">
            <div class="box-content">
                <fieldset>
                    <asp:Menu
                        ID="Menu1"
                        runat="server"
                        RenderingMode="Table"
                        Orientation="Horizontal"
                        StaticMenuItemStyle-CssClass="MenuItem"
                        StaticSelectedStyle-CssClass="MenuItem_selected"
                        StaticEnableDefaultPopOutImage="False"
                        OnMenuItemClick="Menu1_MenuItemClick">
                        <Items>
                            <asp:MenuItem Text="Dettaglio" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Vincitori" Value="1"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </fieldset> 
            </div>
        </div>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="tabDettaglio" runat="server"  >
	        <div class="box-header">
		        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Bilancio</h2>
		        <div class="box-icon">
			        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
		        </div>
	        </div>
	        <div class="box box-content" style="overflow:auto;">
                <asp:GridView ID="grdBilancio" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                        </asp:BoundField>
                        <asp:BoundField DataField="TotDare" HeaderText="Pagamenti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Reali" HeaderText="Reali" >
                            <HeaderStyle CssClass="cella-testata-griglia"  />
                        </asp:BoundField>
                        <asp:BoundField DataField="Vinti" HeaderText="Vinti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Presi" HeaderText="Presi" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Bilancio" HeaderText="Bilancio" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Amici" HeaderText="Pres. Amici" >
                        </asp:BoundField>
<%--                        <asp:BoundField DataField="Totale" HeaderText="Totale" >
                        </asp:BoundField>--%>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:ImageButton ID="imgPaga" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/Pagamento.png" OnClick="EffettuaPagamento" ToolTip ="Incassa i soldi dal giocatore" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIncassa" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/Incasso.png" OnClick="EffettuaIncasso" ToolTip ="Versa i soldi al giocatore" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDettaglio" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/icona_CERCA.png" OnClick="VisualizzaDettaglio" ToolTip ="Visualizza il dettaglio movimenti del giocatore" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:ImageButton ID="imgModifica" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" OnClick="ModificaTotale" ToolTip ="Modifica il totale da pagare per il giocatore" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View> 
        <asp:View ID="tabVincitori" runat="server"  >
            <div class="box span12" style="margin-left: 0px; margin-top: 4px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Vincitori</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdVittorie" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgTipologia" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Tipologia" HeaderText="Tipologia" >
                        </asp:BoundField>
                        <asp:BoundField DataField="TotDareV" HeaderText="Importo" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Vincitore" HeaderText="Vincitore" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarV" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                    </Columns>
                </asp:GridView>        
                </div>
            </div>
        </asp:View> 
    </asp:MultiView>

    <div id="divTesto" runat="server">
        <div id="divBloccaFinestra2" class="bloccafinestra" runat="server" Visible="True"></div>

        <div id="divPopup2" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnTipoCampo" runat="server" />
            <asp:HiddenField ID="hdnModalita" runat="server" />
            <asp:Label ID="lblCampo" runat="server" Text="Label" CssClass ="etichettatitoloschedina"></asp:Label>
            <asp:TextBox ID="txtCampo" runat="server"></asp:TextBox>
            <hr />
            <asp:Button id="cmdOK" class="bottone" runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="bottone" runat="server" Text="Annulla" />

            <div class="clear"></div>
        </div>
    </div>

    <div id="divDettaglio" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnUtente" runat="server" />
            <asp:GridView ID="grdDettaglio" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="DataOra" HeaderText="Data" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Importo" HeaderText="Importo" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipologia" HeaderText="Tipologia" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Image ID="imgTipologia" runat="server" CssClass="ImmagineGrigliaGrande"/>
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>        
            <hr />
            <asp:Button id="cmdChiudeDettaglio" class="bottone" runat="server" Text="OK" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
