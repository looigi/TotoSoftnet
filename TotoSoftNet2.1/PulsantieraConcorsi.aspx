<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PulsantieraConcorsi.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PulsantieraConcorsi" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid" style="padding: 3px;">	
		<a id="aNuovoConcorso" runat ="server" class="quick-button metro yellow span4" href="NuovoConcorso.aspx" style="margin: 2px;" >
			<i class="icon-asterisk"></i>
			<p>Nuovo Concorso</p>
		</a>

		<a id="aAggiornaConcorso" runat ="server" class="quick-button metro blue span4" href="AggiornaConcorso.aspx" style="margin: 2px;" >
			<i class="icon-pencil"></i>
			<p>Aggiorna Concorso</p>
		</a>

		<a id="aModificaConcorso" runat ="server" class="quick-button metro red span4" href="NuovoConcorso.aspx?Modifica=True" style="margin: 2px;" >
			<i class="icon-adjust"></i>
			<p>Modifica Concorso</p>
		</a>

		<a id="aChiudeConcorso" runat ="server" class="quick-button metro green span4" href="ChiudeConcorso.aspx" style="margin: 2px;" >
			<i class="icon-asterisk"></i>
			<p>Chiude Concorso</p>
		</a>

		<a id="aControlloSchedine" runat ="server" class="quick-button metro black span4" href="ControlloSchedine.aspx" style="margin: 2px;" >
			<i class="icon-search"></i>
			<p>Controllo Schedine</p>
		</a>

		<a id="aRiapreConcorso" runat ="server" class="quick-button metro yellow span4" href="RiapreConcorso.aspx" style="margin: 2px;" >
			<i class="icon-asterisk"></i>
			<p>Riapre Concorso</p>
		</a>

		<div class="clearfix"></div>
	</div><!--/row-->
 </asp:Content>