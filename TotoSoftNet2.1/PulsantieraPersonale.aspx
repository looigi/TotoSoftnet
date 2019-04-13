<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PulsantieraPersonale.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PulsantieraPersonale" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid" style="padding: 3px;">	
		<a id="aPropria" runat ="server" class="quick-button metro yellow span4" href="ColonnaPropria.aspx" style="margin: 2px;" >
			<i class="icon-pencil"></i>
			<p>Colonna Propria</p>
		</a>

		<a id="a1" runat ="server" class="quick-button metro blue span4" href="Cassa.aspx" style="margin: 2px;" >
			<i class="icon-money"></i>
			<p>Cassa</p>
		</a>

		<a id="a2" runat ="server" class="quick-button metro red span4" href="GestioneAccount.aspx" style="margin: 2px;" >
			<i class="icon-user"></i>
			<p>Dati personali</p>
		</a>

		<a id="aPortaAmico" runat ="server" class="quick-button metro red span4" href="PortaAmico.aspx" style="margin: 2px;" >
			<i class="icon-asterisk"></i>
			<p>Presenta amico</p>
		</a>

		<a id="a4" runat ="server" class="quick-button metro red span4" href="ColonnaTappo.aspx" style="margin: 2px;" >
			<i class="icon-trash"></i>
			<p>Colonna tappo</p>
		</a>

		<div class="clearfix"></div>
	</div><!--/row-->
 </asp:Content>