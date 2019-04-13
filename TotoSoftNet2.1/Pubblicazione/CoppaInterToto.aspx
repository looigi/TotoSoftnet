<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="CoppaInterToto.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.CoppaInterToto" %>

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
		<li><i class="icon-glass"></i><a href="#">Intertoto</a></li>
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

    <div id="divVincente" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px; ">
            <div class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincitore" CssClass ="label-medio"></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/Intertoto.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/Intertoto.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="label-medio"></asp:Label>
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
                            <asp:MenuItem Text="Qualificati" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Partite" Value="1"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </fieldset> 
            </div>
        </div>
    </div>

<%--    <div id="divApertoChiuso" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="span12" style="text-align:center;">
            <asp:Label ID="lblAvviso" runat="server" Text="La competizione non è ancora cominciata" CssClass ="label-titolo"></asp:Label>
        </div>
    </div>--%>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="TabGirone" runat="server"  >
            <div id="divDettaglio" runat="server" class="box span12" style="margin-left: 0px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Giocatori qualificati</h2>
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
                            <asp:BoundField DataField="Provenienza" HeaderText="Prov." HtmlEncode="False">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="tabPartite" runat="server"  >
            <div id="divDettaglio2" runat="server" class="box span12" style="margin-left:0px;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <div class="span12" style="text-align:center;">
                        <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-medio rosso"></asp:Label>
                    </div>
                    <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatarC" runat="server" CssClass="ImmagineGrigliaGrande"/>
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
                            <asp:BoundField DataField="RisAndata" HeaderText="Ris. Andata" HtmlEncode="False">
                            </asp:BoundField>
                            <asp:BoundField DataField="RisRitorno" HeaderText="Ris. Ritorno" HtmlEncode="False">
                            </asp:BoundField>
                            <asp:BoundField DataField="Vincente" HeaderText="Vincente" >
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
