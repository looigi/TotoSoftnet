<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="SegniUsciti.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.SegniUsciti" %>

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
		<li><i class="icon-signal"></i><a href="#">Segni usciti</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span8">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Segni usciti</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita" HeaderText="P." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Casa">
                            <ItemTemplate>
                                <asp:Label ID="txtCasa" runat="server" MaxLength ="15" Font-Names="Verdana" Font-Size="Small" Enabled="False" Width="150px"></asp:Label>
                                <br />
                                <asp:Label ID="lblSerieC" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgCasa" runat="server" CssClass="ImmagineGriglia" Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fuori">
                            <ItemTemplate>
                                <asp:Label ID="txtFuori" runat="server" MaxLength ="15" Font-Names="Verdana" Font-Size="Small" Enabled="False" Width="150px"></asp:Label>
                                <br />
                                <asp:Label ID="lblSerieF" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFuori" runat="server" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgJolly" runat="server" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span4">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Squadre</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdSquadre" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgSquadra" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Squadra" HeaderText="Squadra" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Segno1" HeaderText="1" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="SegnoX" HeaderText="X" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Segno2" HeaderText="2" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgVai"
                                    runat="server" imageurl="App_Themes/Standard/Images/Icone/Freccetta.Png" 
                                    onclick="VisualizzaDettaglio" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div id="divDettaglio" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnSquadra" runat="server" />
            <asp:HiddenField ID="hdnDove" runat="server" />
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Squadre</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
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
                    </Columns>
                </asp:GridView>   
            </div>     

            <asp:Button id="cmdChiudeDettaglio" class="btn btn-primary" runat="server" Text="OK" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
