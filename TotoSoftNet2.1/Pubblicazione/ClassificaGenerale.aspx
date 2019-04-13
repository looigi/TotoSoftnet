<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ClassificaGenerale.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ClassificaGenerale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <%--    <script src="Js/jquery-1.11.3.min.js"></script>
    <script src="Js/jquery-ui.js"></script>
    <link href="App_Themes/Standard/jquery-ui.css" rel="stylesheet" />    
    <script src="Js/highcharts.js"></script>	
    <script src="Js/modules/exporting.js"></script>
    
    <script type="text/javascript">
        function DisegnaStatistica(nomeDiv, testo, TitAsseY, TitSeries, Suffisso) {
            var pos1 = document.getElementById("ctl00_contentCentrale_hdnPassaggio1");
            var pos2 = document.getElementById("ctl00_contentCentrale_hdnPassaggio2");

            var pp1 = pos1.value.toString().split(",");
            var pp2 = pos2.value.split(",");

            var i;

            var p1 = new Array();
            for (i = 0; i < pp1.length; i++) {
                p1[i] = pp1[i];
                while (p1[i].indexOf("'") > -1) {
                    p1[i] = p1[i].replace("'", "");
                }
            }
            var p2 = new Array();
            for (i = 0; i < pp2.length; i++) {
                p2[i] = eval(pp2[i]);
            }

            $('#ctl00_contentCentrale_' + nomeDiv).highcharts({
                title: {
                    text: testo,
                    x: 0 //center
                },
                subtitle: {
                    text: '',
                    x: 0
                },
                xAxis: {
                    categories: p1
                },
                yAxis: {
                    title: {
                        text: TitAsseY
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: Suffisso
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: TitSeries,
                    data: p2
                }]
            })
        }
    </script>--%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-list-ol"></i>
			<a href="PulsantieraClassifiche.aspx">Classifiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-list"></i><a href="#">Classifica Generale</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <asp:HiddenField ID="hdnGiornata" runat="server" />

    <div class="row-fluid sortable ui-sortable">				
       <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Generale</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>

		    <div class="box-content" style="text-align:center; overflow: auto">
<%--                <asp:Label ID="Label5" runat="server" Text="Posizioni" CssClass ="etichettatitolodett " ></asp:Label>
                &nbsp;<asp:ImageButton ID="imgStatPosizioni" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Statistiche.png" ToolTip="Visualizza le statistiche sui jolly" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Statistiche sui Jolly" CssClass ="etichettatitolodett " ></asp:Label>
                &nbsp;<asp:ImageButton ID="imgStatJolly" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Statistiche.png" ToolTip="Visualizza le statistiche sui jolly" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Statistiche sui Segni" CssClass ="etichettatitolodett " ></asp:Label>
                &nbsp;<asp:ImageButton ID="imgStatSegni" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Statistiche.png" ToolTip="Visualizza le statistiche sui segni" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="Statistiche sui Risultati" CssClass ="etichettatitolodett " ></asp:Label>
                &nbsp;<asp:ImageButton ID="imgStatRisultati" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Statistiche.png" ToolTip="Visualizza le statistiche sui risultati" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" Text="Statistiche sui punti totali" CssClass ="etichettatitolodett " ></asp:Label>
                &nbsp;<asp:ImageButton ID="imgStatPunti" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Statistiche.png" ToolTip="Visualizza le statistiche sui punti totali" />
                <hr />--%>

		        <div class="box-content">
                    <div style="width:15%; float: left; text-align: left;">
                        <asp:Button ID="cmdIndietro" runat="server" Text="<<" CssClass ="btn btn-primary" />
                    </div>
                    <div style="width:70%; float: left; text-align:center;">
          
                        <asp:Label ID="lblGiornata" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                    </div>
                    <div style="width:15%; float: right; text-align:right;">
                        <asp:Button ID="cmdAvanti" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                    </div>

                    <div class="clearALL "></div>
                </div>

                <asp:GridView ID="grdClassifica" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered" 
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Posizione"  HeaderText="P." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiTot" HeaderText="Tot." >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiJolly" HeaderText="J." >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiQuote" HeaderText="Q." >
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiFR" HeaderText="FR" >
                        </asp:BoundField>
                        <asp:BoundField DataField="UltRis" HeaderText="Ult.Ris." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Precedente" HeaderText="Prec." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Giocate" HeaderText="G." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Vittorie" HeaderText="1" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Secondo" HeaderText="2" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Ultimo" HeaderText="Ult." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Tappi" HeaderText="T." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Media" HeaderText="Media" >
                        </asp:BoundField>
                        <asp:BoundField DataField="RiconDison" HeaderText="R/D" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Diff." >
                            <ItemTemplate>
                                <asp:Image ID="imgDifferenza" runat="server" CssClass="ImmagineGriglia" />
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>            
                     </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

<%--    <div class="mascherinalarga draggable">
        <div style="width:15%; float: left;">
            <asp:Button ID="cmdIndietro" runat="server" Text="<<" CssClass ="bottone " />
        </div>
        <div style="width:67%; float: left;">
            <asp:Label ID="lblGiornata" runat="server" Text="Commento" CssClass ="etichettatitolocoppe " ></asp:Label>
        </div>
        <div style="width:15%; float: left;">
            <asp:Button ID="cmdAvanti" runat="server" Text="&gt;&gt;" CssClass ="bottone " />
        </div>

        <div class="clearALL "></div>
    </div>--%>

<%--     <div class="mascherinalarga draggable" style="text-align: right;">
    </div>
   <div class="mascherina draggable">
    </div>--%>

    <div id="divStatistiche" runat="server" >
        <asp:HiddenField ID="hdnQualeStat" runat="server" />
        <asp:HiddenField ID="hdnPassaggio1" runat="server" />
        <asp:HiddenField ID="hdnPassaggio2" runat="server" />

        <div class="bloccafinestra">
        </div>
        <div class="Statistiche draggable">
            <div style="width: 99%;">
                <div class="barratasti " style="height: 115px;">
                    <div style="width: 80px; float: left; text-align: left;">
                        <asp:Image ID="imgUtente" runat="server" Width="70px" Height="70px" />
                    </div>
                    <div style="width: 50%; float: left; text-align: center; margin-top: 25px;">
                        <asp:Label ID="lbl1" runat="server" Text="Giocatore" CssClass ="etichettatitolocoppe " ></asp:Label>
                        &nbsp;<asp:DropDownList ID="cmbGiocatore" runat="server" CssClass="combomedia " AutoPostBack="True" ></asp:DropDownList>
                    </div>
                    
                    <div style="width: 8%; float: right; text-align: right;">
                        <asp:ImageButton ID="imgChiudiStat" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/elimina_quadrato.png" />
                    </div>
                    <div class="clearALL "></div>
                    <hr />
                </div>
            </div>
            <div id="divVisualizzato" runat="server" style="width: 99%;">

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
