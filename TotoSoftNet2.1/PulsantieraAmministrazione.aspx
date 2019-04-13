<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PulsantieraAmministrazione.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PulsantieraAmministrazione" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid" style="padding: 3px;">	
		<a id="aBilancio" runat ="server" class="quick-button metro yellow span4" href="Bilancio.aspx" style="margin: 2px;" >
			<i class="icon-money"></i>
			<p>Bilancio</p>
		</a>
		<a id="aCambioUtente" runat ="server" class="quick-button metro blue span4" href="CambioUtente.aspx"  style="margin: 2px;">
			<i class="icon-user"></i>
			<p>Cambio Utente</p>
		</a>
		<a id="aEventi" runat ="server" class="quick-button metro red span4" href="Eventi.aspx" style="margin: 2px;">
			<i class="icon-bell"></i>
			<p>Eventi</p>
		</a>
		<a id="aGestioneCoppe" runat ="server" class="quick-button metro green span4" href="GestioneCoppe.aspx" style="margin: 2px;">
			<i class="icon-glass"></i>
			<p>Gestione Coppe</p>
		</a>
		<a id="aGestRiconDison" runat ="server" class="quick-button metro black span4" href="GestRiconDison.aspx" style="margin: 2px;">
			<i class="icon-coffee"></i>
			<p>Ricon./Dison.</p>
		</a>
		<a id="aModificaAnno" runat ="server" class="quick-button metro blue span4" href="ModificaAnno.aspx" style="margin: 2px;">
			<i class="icon-calendar"></i>
			<p>Modifica Anno</p>
		</a>
		<a id="aModificaUtente" runat ="server" class="quick-button metro red span4" href="ModificaUtente.aspx" style="margin: 2px;">
			<i class="icon-user"></i>
			<p>Modifica Utente</p>
		</a>
		<a id="aNuovoAnno" runat ="server" class="quick-button metro green span4" href="NuovoAnno.aspx" style="margin: 2px;">
			<i class="icon-asterisk"></i>
			<p>Nuovo Anno</p>
		</a>
		<a id="aChiusuraAnno" runat ="server" class="quick-button metro black span4" href="ChiusuraAnno.aspx" style="margin: 2px;">
			<i class="icon-lock"></i>
			<p>Chiusura Anno</p>
		</a>
		<a id="aPercentuali" runat ="server" class="quick-button metro red span4" href="Percentuali.aspx" style="margin: 2px;">
			<i class="icon-adjust"></i>
			<p>Percentuali</p>
		</a>
		<a id="aRegistranti" runat ="server" class="quick-button metro blue span4" href="Registranti.aspx" style="margin: 2px;">
			<i class="icon-star-half"></i>
			<p>Registranti</p>
		</a>
		<a id="aSondaggi" runat ="server" class="quick-button metro green span4" href="Sondaggi.aspx" style="margin: 2px;">
			<i class="icon-question-sign"></i>
			<p>Sondaggi</p>
		</a>

		<div class="clearfix"></div>
	</div><!--/row-->
 </asp:Content>