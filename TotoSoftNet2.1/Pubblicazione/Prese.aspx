<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Prese.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Prese" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-star"></i>
			<a href="PulsantieraStatistiche.aspx">Statistiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-table"></i><a href="#">Prese</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Squadre Prese</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPrese" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image id="imgSquadra" runat="server" width="70" height="70"></asp:Image>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Squadra" HeaderText="Squadra" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Volte" HeaderText="Volte" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Presa" HeaderText="Presa" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton id="img" runat="server" width="70" height="70" imageurl="App_Themes/Standard/Images/Icone/icona_CERCA.png" onclick="VisualizzaDettaglio"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>            
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div id="divDettaglio" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto" runat="server" Visible="true">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Dettaglio</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:HiddenField ID="hdnSquadra" runat="server" />
                <asp:GridView ID="grdDettaglio" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Giornata" HeaderText="Giornata" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Partita" HeaderText="Partita" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Segno" HeaderText="Segno" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Risultato" HeaderText="Risultato" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Pronostico" HeaderText="Pronostico" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>        
                <hr />
                <asp:Button id="cmdChiudeDettaglio" class="btn btn-primary" runat="server" Text="OK" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
