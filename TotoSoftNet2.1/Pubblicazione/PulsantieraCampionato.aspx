<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PulsantieraCampionato.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PulsantieraCampionato" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid" style="padding: 3px;">	
		<a id="aBilancio" runat ="server" class="quick-button metro yellow span4" href="ClassificaGenerale.aspx" style="margin: 2px;" >
			<i class="icon-list"></i>
			<p>Classifica Generale</p>
		</a>

		<a id="a1" runat ="server" class="quick-button metro blue span4" href="ClassificaRisultati.aspx" style="margin: 2px;" >
			<i class="icon-list"></i>
			<p>Classifica Risultati</p>
		</a>

		<a id="a2" runat ="server" class="quick-button metro green span4" href="ClassificaCampionato.aspx" style="margin: 2px;" >
			<i class="icon-list"></i>
			<p>Classifica Campionato</p>
		</a>

		<a id="a3" runat ="server" class="quick-button metro green span4" href="ClassificaSpeciale.aspx" style="margin: 2px;" >
			<i class="icon-list"></i>
			<p>Classifica Speciali</p>
		</a>

		<div class="clearfix"></div>
	</div><!--/row-->
 </asp:Content>