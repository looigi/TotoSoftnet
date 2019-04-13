<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PulsantieraStatistiche.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PulsantieraStatistiche" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid" style="padding: 3px;">	
		<a id="a0" runat ="server" class="quick-button metro yellow span4" href="Commenti.aspx" style="margin: 2px;" >
			<i class="icon-reply"></i>
			<p>Commenti</p>
		</a>

		<a id="a1" runat ="server" class="quick-button metro blue span4" href="Prese.aspx" style="margin: 2px;" >
			<i class="icon-table"></i>
			<p>Prese</p>
		</a>

		<a id="a2" runat ="server" class="quick-button metro green span4" href="Records.aspx" style="margin: 2px;" >
			<i class="icon-time"></i>
			<p>Records</p>
		</a>

		<a id="aSegniUsciti" runat ="server" class="quick-button black yellow span4" href="SegniUsciti.aspx" style="margin: 2px;" >
			<i class="icon-signal"></i>
			<p>Segni Usciti</p>
		</a>

		<a id="aColonne" runat ="server" class="quick-button metro red span4" href="Colonne.aspx" style="margin: 2px;" >
			<i class="icon-list"></i>
			<p>Colonne</p>
		</a>

		<div class="clearfix"></div>
	</div><!--/row-->
 </asp:Content>