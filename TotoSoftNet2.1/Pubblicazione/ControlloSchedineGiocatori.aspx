<%@ Page Title="" Language="vb" EnableEventValidation="true" AutoEventWireup="false" MasterPageFile="TsnIISM.Master" CodeBehind="ControlloSchedineGiocatori.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ControlloSchedineGiocatori" %>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
			<i class="icon-play"></i>
			<a href="PulsantieraConcorsi.aspx">Concorsi</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-eye-open"></i><a href="#">Controllo concorso</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphNoAjax" runat="server">
    <%--<div class="row-fluid">--%>
        <div class="span12" style=" margin-left: 0px; margin-bottom: 5px;">
            <div style="width: 50%; text-align: center; float: left;">
                <asp:Button ID="cmdControlla" runat="server" Text="Controlla" CssClass="btn btn-primary"/>
                <%--<asp:Button ID="cmdAggiorna" runat="server" Text="Aggiorna" CssClass="btn btn-primary" />--%>
                <asp:Button ID="cmdMostraTutti" runat="server" Text="Mostra tutti" CssClass="btn btn-primary" />
                <%--<asp:Button ID="cmdAvvisa" runat="server" Text="Manda Mail" CssClass="btn btn-primary" />--%>
                <%--<asp:Button ID="cmdRiepilogo" runat="server" Text="Conferma" CssClass="btn btn-primary" />--%>
            </div>
            <div id="div1" runat="server" style="width: 40%; text-align: center; float: left;">
		        <asp:Label ID="lblSpeciale" runat="server" Text="Concorso speciale" CssClass ="label-titolo" ForeColor="#AA0000"></asp:Label>
            </div>
            <div style="width: 8%; text-align: center; float: left;">
                <asp:Button ID="cmdUscita" runat="server" Text="Uscita" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="clear clearALL "></div>

        <div class="box span7" style="margin-left: 0px; ">
	        <div id="divColonna" runat="server" class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content" style="overflow: auto;">
                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-condensed "
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Partita" HeaderText="P." >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Casa">
                                <ItemTemplate>
                                    <asp:Label ID="txtCasa" runat="server" MaxLength ="20" Font-Names="Verdana" Font-Size="Small" Enabled="false" Width="150px"></asp:Label>
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
                                    <asp:Label ID="txtFuori" runat="server" MaxLength ="20" Font-Names="Verdana" Font-Size="Small" Enabled="False" Width="150px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblSerieF" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgFuori" runat="server" CssClass="ImmagineGriglia" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ris.">
                                <ItemTemplate>
                                    <asp:Label ID="txtRisultato" runat="server" Width="30" MaxLength ="6" Font-Names="Verdana" Font-Size="Small" Enabled="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S.">
                                <ItemTemplate>
                                    <asp:Label ID="txtSegno" runat="server" Width="13" MaxLength ="6" Font-Names="Verdana" Font-Size="Small" Enabled="False" ></asp:Label>
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

        <div class="box span5" id="divRisultati" runat="server" style="margin-left: 2px;" >
            <div id="divClass" runat ="server">
                <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content" style="overflow: auto;">
                    <asp:GridView ID="grdClassvinc" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-condensed "
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                            <Columns>
                                <asp:BoundField DataField="Posizione" HeaderText="P." >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Image ID="imgUtente" runat="server" CssClass="ImmagineGriglia" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Giocatore" HeaderText="Nick" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiTotali" HeaderText="Tot." >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiJolly" HeaderText="J" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiQuote" HeaderText="Q" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiFR" HeaderText="FR" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiSomma" HeaderText="PS" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiDifferenza" HeaderText="PD" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Pagante" HeaderText="Pag." >
                                </asp:BoundField>
                                <asp:BoundField DataField="Tappato" HeaderText="T." >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgMostra" runat="server" CssClass="ImmagineGriglia" src="App_Themes/Standard/Images/Icone/visualizzato_tondo.png" OnClick ="MostraGioc" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns> 
                    </asp:GridView>
        <%--            <asp:Button ID="cmdMostraTutti" runat="server" Text="Mostra tutti" CssClass="bottone" />
                    <asp:Button ID="cmdRiepilogo" runat="server" Text="Riepilogo" CssClass="bottone" />--%>
                </div>
            </div>

            <div id="divVincitori" runat ="server"  style="margin-left: 2px;" >
                <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Vincitori</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content" style="text-align:center; style="overflow: auto;"">
                    <asp:Label ID="lblPremioVincitore" runat="server" Text="Label" CssClass ="label-medio rosso"></asp:Label>
                    <br />
                    <asp:Label ID="lblPremioSecondo" runat="server" Text="Label" CssClass ="label-medio rosso"></asp:Label>
                 
                    <asp:GridView ID="grdVincitori" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-condensed "
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Posizione" HeaderText="P." >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Image ID="imgUtente" runat="server" CssClass="ImmagineGriglia" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Giocatore" HeaderText="Nick" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiTotali" HeaderText="Tot." >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiJolly" HeaderText="J" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiQuote" HeaderText="Q" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiFR" HeaderText="FR" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiSomma" HeaderText="PS" >
                            </asp:BoundField>
                            <asp:BoundField DataField="PuntiDifferenza" HeaderText="PD" >
                            </asp:BoundField>
                        </Columns> 
                    </asp:GridView>
                </div>
            </div>

            <div id="divUltimoPosto" runat ="server" style="margin-left: 2px;">
                <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Ultimi</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content" style="overflow: auto;">
                    <asp:GridView ID="grdUltimi" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered table-condensed "
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                            <Columns>
                                <asp:BoundField DataField="Posizione" HeaderText="P." >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Image ID="imgUtente" runat="server" CssClass="ImmagineGriglia" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Giocatore" HeaderText="Nick" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiTotali" HeaderText="Tot." >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiJolly" HeaderText="J" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiQuote" HeaderText="Q" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiFR" HeaderText="FR" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiSomma" HeaderText="PS" >
                                </asp:BoundField>
                                <asp:BoundField DataField="PuntiDifferenza" HeaderText="PD" >
                                </asp:BoundField>
                            </Columns> 
                    </asp:GridView>
                 
        <%--            <asp:Button ID="cmdAggiorna" runat="server" Text="Aggiorna" CssClass="bottone" />--%>
                </div>
            </div>
        <%--</div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
</asp:Content>
