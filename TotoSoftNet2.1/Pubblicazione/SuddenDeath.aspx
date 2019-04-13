<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="SuddenDeath.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.SuddenDeath" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-glass"></i>
			<a href="PulsantieraCoppe.aspx">Coppe</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Sudden death</a></li>
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
<%--    <div class="row-fluid sortable ui-sortable">				
		<div class="box span12">
            <div class="box-content">
                <fieldset>
                    <div class="span12" style="text-align: center;">
                        <img src="App_Themes/Standard/Images/Icone/Suddendeath.png" width="50" />
                        <asp:Label ID="lblAttuale" runat="server" Text="Sudden death" CssClass ="label-warning"></asp:Label>
                        <img src="App_Themes/Standard/Images/Icone/Suddendeath.png" width="50" />
                    </div>
                </fieldset> 
            </div>
        </div>
    </div>--%>

<%--    <div class="row-fluid sortable ui-sortable">				
        <div class="span12">
            <div id="divApertoChiuso" runat="server" class="mascherinaavvisi">				
                <asp:Label ID="lblAvviso" runat="server" Text="La competizione non è ancora cominciata" CssClass ="label"></asp:Label>
            </div>
        </div>
    </div>--%>

    <div class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px;">
            <div id="divVincente" runat="server" class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincitore" CssClass ="etichettatitolocoppe "></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/Suddendeath.png" width="100" height="80" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/Suddendeath.png" width="100" height="80" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="etichettatitolocoppe "></asp:Label>
            </div>
        </div>
    </div>
    
    <div class="row-fluid sortable ui-sortable">				
		<div class="span12">
            <div class="box-content">
                <fieldset>
<%--                    <div class="span12" style="text-align:center;">
                        <img src="App_Themes/Standard/Images/Icone/Intertoto.png" width="50" />
                        <asp:Label ID="lblTitolo" runat="server" Text="Intertoto" CssClass ="label-titolo"></asp:Label>
                        <img src="App_Themes/Standard/Images/Icone/Intertoto.png" width="50" />
                    </div>--%>
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
                            <asp:MenuItem Text="Giocatori" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Esclusi" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Dettaglio" Value="2"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </fieldset> 
            </div>
        </div>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="tabGiocatori" runat="server"  >
            <div id="divDettaglio1" runat ="server" class="box span12" style="margin-left: 0px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdClassifica" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Giocatore"  HeaderText="Giocatore" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Punti" HeaderText="Punti" >
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="tabEsclusi" runat="server"  >
            <div id="divDettaglio2" runat="server" class ="box span12" style="margin-left: 2px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Esclusi</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdEsclusi" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Giornata"  HeaderText="Giornata" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Giocatore"  HeaderText="Giocatore" >
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="tabdettaglio" runat="server"  >
            <div id="divDettaglio3" runat="server" class ="box span12" style="margin-left: 2px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Squadre assegnate</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:dropdownlist id="cmbGiocatori" runat="server" class="combomedia" autopostback="true"></asp:dropdownlist>
                    <hr />
                    <asp:GridView ID="grdSquadre" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Giornata"  HeaderText="Giornata" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgSquadra" runat="server" CssClass="ImmagineGriglia" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Squadra"  HeaderText="Squadra" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Risultato"  HeaderText="Risultato" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Punti" >
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>    
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
