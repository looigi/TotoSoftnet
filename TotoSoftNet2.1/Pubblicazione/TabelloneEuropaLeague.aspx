<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="TabelloneEuropaLeague.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.TabelloneEuropaLeague" %>

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
		<li><i class="icon-glass"></i><a href="#">Europa League</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentHead" runat="server">
    <style type="text/css">
        #ctl00_contentCentrale_Menu1 {
            margin-bottom: 2px;
            width:99%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divVincente" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px;">
            <div class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincitore" CssClass ="label-medio"></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/EuropaLeague.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/EuropaLeague.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="label-medio"></asp:Label>
            </div>
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
                            <asp:MenuItem Text="Qualificati" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Girone" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Fase Finale" Value="2"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </fieldset> 
            </div>
        </div>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="TabGirone" runat="server"  >
            <div class="row-fluid sortable ui-sortable">				
                <div class="box span12" style="margin-left: 0px;">
	                <div class="box-header">
			            <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Giocatori Qualificati e classifica</h2>
			            <div class="box-icon">
				            <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			            </div>
		            </div>
		            <div class="box-content">
                        <asp:GridView ID="grdQualificate" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                            <Columns>
                                <asp:BoundField DataField="Posizione"  HeaderText="Pos." >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                                    </ItemTemplate>
                                </asp:TemplateField>            
                                <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Punti" HeaderText="Punti" >
                                </asp:BoundField>
                                <asp:BoundField DataField="GF" HeaderText="G. Fatti">
                                </asp:BoundField>
                                <asp:BoundField DataField="GS" HeaderText="G. Subiti">
                                </asp:BoundField>
                                <asp:BoundField DataField="Giocate" HeaderText="Giocate">
                                </asp:BoundField>
                                <asp:BoundField DataField="Provenienza" HeaderText="Prov." HtmlEncode="False">
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:View>

        <asp:View ID="Girone" runat="server"  >
            <div id="divDettaglio2" runat="server" class="box span12" style="margin-left: 0px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Girone</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:HiddenField ID="hdnGiornata" runat="server" />

                    <div class="span12" >
                        <div class="span3" style="text-align: left;">
                            <asp:Button ID="cmdIndietroG" runat="server" Text="<<" CssClass ="btn btn-primary" />
                        </div>
                        <div class="span5" style="text-align: center;">
                            <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-medio rosso" ></asp:Label>
                            </div>
                        <div class="span3" style="text-align: right;">
                            <asp:Button ID="cmdAvantiG" runat="server" Text=">>" CssClass ="btn btn-primary" />
                        </div>
                    </div>

                    <asp:GridView ID="grdPartiteTurni" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatarC" runat="server" width="80" Height="80" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Casa"  HeaderText="Casa" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatarF" runat="server" CssClass="ImmagineGrigliaGrande"/>
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Fuori" HeaderText="Fuori" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Risultato" HeaderText="Risultato" HtmlEncode="False">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>

        <asp:View ID="TabFinale" runat="server">
            <div class="box span12" style="text-align:center; margin-left:0px">
                <div class="span11" id="divScontriDiretti" runat="server" style="text-align: center; overflow: auto; ">
                </div>
            </div>
        </asp:View>
    </asp:MultiView>

<%--    <div class="row-fluid sortable ui-sortable">				
        <div class="span12">
            <div id="divApertoChiuso" runat="server" class="mascherinaavvisi">				
                <asp:Label ID="lblAvviso" runat="server" Text="La competizione non è ancora cominciata" CssClass ="label-titolo"></asp:Label>
            </div>
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
