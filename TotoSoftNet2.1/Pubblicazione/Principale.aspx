<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Principale.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.prova" %>
<asp:Content ID="contHead" ContentPlaceHolderID ="ContentHead" runat="server">
    <script>
        var _second = 1000;
        var _minute = _second * 60;
        var _hour = _minute * 60;
        var _day = _hour * 24;
        var timer;

        var end;

        var g1;
        var g2;
        var h1;
        var h2;
        var m1;
        var m2;
        var s1;
        var s2;

        function setEnd(Anno, Mese, Giorno, Ora, Minuti, Secondi) {
            end = new Date(Mese + '/' + Giorno + '/' + Anno + ' ' + Ora + ':' + Minuti + ':' + Secondi);

            g1 = document.getElementById('contentCentrale_imgG1');
            g2 = document.getElementById('contentCentrale_imgG2');

            h1 = document.getElementById('contentCentrale_imgH1');
            h2 = document.getElementById('contentCentrale_imgH2');

            m1 = document.getElementById('contentCentrale_imgM1');
            m2 = document.getElementById('contentCentrale_imgM2');

            s1 = document.getElementById('contentCentrale_imgS1');
            s2 = document.getElementById('contentCentrale_imgS2');

            timer = setInterval(showRemaining, 1000);
        }

        function showRemaining() {
            var now = new Date();

            var distance = end - now;
            if (distance < 0) {
                clearInterval(timer);
                document.getElementById('ctl00_contentCentrale_lblScadenzaSch').innerTEXT = 'Concorso chiuso';

                return;
            }

            var days = Math.floor(distance / _day);
            var hours = Math.floor((distance % _day) / _hour);
            var minutes = Math.floor((distance % _hour) / _minute);
            var seconds = Math.floor((distance % _minute) / _second);

            var sd = days.toString();
            if (sd.length == 1) sd = '0' + sd;
            var sh = hours.toString();
            if (sh.length == 1) sh = '0' + sh;
            var sm = minutes.toString();
            if (sm.length == 1) sm = '0' + sm;
            var ss = seconds.toString();
            if (ss.length == 1) ss = '0' + ss;

            g1.src = 'App_Themes/Standard/Images/Icone/' + sd.substring(0, 1) + '.png';
            g2.src = 'App_Themes/Standard/Images/Icone/' + sd.substring(1, 2) + '.png';

            h1.src = 'App_Themes/Standard/Images/Icone/' + sh.substring(0, 1) + '.png';
            h2.src = 'App_Themes/Standard/Images/Icone/' + sh.substring(1, 2) + '.png';

            m1.src = 'App_Themes/Standard/Images/Icone/' + sm.substring(0, 1) + '.png';
            m2.src = 'App_Themes/Standard/Images/Icone/' + sm.substring(1, 2) + '.png';

            s1.src = 'App_Themes/Standard/Images/Icone/' + ss.substring(0, 1) + '.png';
            s2.src = 'App_Themes/Standard/Images/Icone/' + ss.substring(1, 2) + '.png';
        }

        function ImpostaPagamenti(Valore, Tot, Pagato) {
            var d = document.getElementById('contentCentrale_lblPag');
            d.innerText = Pagato + '€ / ' + Tot +'€';

            d = document.getElementById('contentCentrale_inpPagam');
            d.value = Valore;
            d.style.height = "35px";
        }

        //// Chart
        window.VisuaChart = function (posizioni) {
            var pos = [];
            pos = posizioni.split(",");
            var followers = [];
            for (var i = 0; i < pos.length; i++) {
                followers[i] = [i, pos[i]];
            }

            var plot = $.plot($("#PosizioniChart"),
                    [{ data: followers, label: "Pos." }], {
                        series: {
                            lines: {
                                show: true,
                                lineWidth: 2,
                                fill: true, fillColor: { colors: [{ opacity: 0.5 }, { opacity: 0.2 }] }
                            },
                            points: {
                                show: true,
                                lineWidth: 2
                            },
                            shadowSize: 0
                        },
                        grid: {
                            hoverable: true,
                            clickable: true,
                            tickColor: "#f9f9f9",
                            borderWidth: 0
                        },
                        colors: ["#1BB2E9"],
                        xaxis: { ticks: 6, tickDecimals: 0 },
                        yaxis: { ticks: 3, tickDecimals: 0 },
                    });

                function showTooltip(x, y, contents) {
                    $('<div id="tooltip">' + contents + '</div>').css({
                        position: 'absolute',
                        display: 'none',
                        top: y + 5,
                        left: x + 5,
                        border: '1px solid #fdd',
                        padding: '2px',
                        'background-color': '#dfeffc',
                        opacity: 0.80
                    }).appendTo("body").fadeIn(200);
                }

                var previousPoint = null;

                $("#PosizioniChart").bind("plothover", function (event, pos, item) {
                    $("#x").text(pos.x.toFixed(2));
                    $("#y").text(pos.y.toFixed(2));

                    if (item) {
                        if (previousPoint != item.dataIndex) {
                            previousPoint = item.dataIndex;

                            $("#tooltip").remove();
                            var x = item.datapoint[0].toFixed(2),
                                y = item.datapoint[1].toFixed(2);

                            showTooltip(item.pageX, item.pageY,
                                        item.series.label + " di " + x + " = " + y);
                        }
                    }
                    else {
                        $("#tooltip").remove();
                        previousPoint = null;
                    }
                });

        }

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><a href="#"></a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentcentrale" runat="server">
    <asp:Button ID="cmdMischiaAbbinamenti" runat="server" Text="Mischia abbinamenti Quote CUP" Visible="False" />

    <div class="row-fluid">	
<%--        <div id="divAvanzaGiornata" runat="server">
            <a class="quick-button metro red span3" href="Principale.aspx?AvanzaGiornata=1" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-asterisk"></i>
		        <p><asp:Label ID="Label9" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label8" runat="server" Text="Avanza Giornata" CssClass ="label-medio"></asp:Label><br />
                </p>
	        </a>
        </div>

        <div id="divAvanzaSpeciale" runat="server">
            <a class="quick-button metro red span3" href="Principale.aspx?AvanzaSpeciale=1" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-asterisk"></i>
		        <p><asp:Label ID="Label11" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label12" runat="server" Text="Avanza speciale" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>--%>

        <div id="divPagamenti" runat="server">
            <a id="APag" runat="server" href="Cassa.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px; ">
			    <div class="header" style="margin-top: -30px">Percentuale pagamenti</div>
			    <span class="percent" style="margin-top: -5px;"><asp:Label ID="lblPag" runat="server" Text="Euro" CssClass ="label-titolo"></asp:Label></span>
			    <div class="circleStat" >
                    <input id="inpPagam" runat="server" type="text" class="whiteCircle" />
			    </div>
	        </a>
        </div>

        <div id="divControlloTappo" runat="server">
            <a class="quick-button metro red span3" href="Colonnatappo.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-trash"></i>
		        <p><asp:Label ID="Label6" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label7" runat="server" Text="Colonna tappo non compilata" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divEventi" runat="server">
            <a class="quick-button metro black span3" href="#" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px; overflow: auto;">
		        <div class="header" style="margin-top: -30px"><asp:Label ID="lblEventi" runat="server" Text="" CssClass ="label-titolo"></asp:Label></div>
                <br />
                <asp:Literal ID="ltlEventi" runat="server"></asp:Literal>
	        </a>
        </div>

        <div id="divConcorso" runat="server">
            <a class="quick-button metro blue span3" href="#" style="margin-left: 5px; text-align: center; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-eye-open"></i>
		        <p><asp:Label ID="lblGiornata" runat="server" Text="Label" CssClass ="label-titolo"></asp:Label><br />
		        <asp:Label ID="lblModalitaConcorso" runat="server" Text="Label" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divAggiornaRisultati" runat="server">
            <a class="quick-button metro black span3" href="AggiornaConcorso.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px; ">
			    <div class="header" style="margin-top: -30px">Risultati</div>
               <p style="text-align:center; margin-top: 45px;"><asp:ImageButton ID="imgAggiornaRisultati" runat="server" CssClass ="ImmagineGrigliaGrande" ImageUrl ="App_Themes/Standard/Images/Icone/Elabora.png" /><br />
                <asp:Label ID="lblAggiornate" runat="server" Text="Colonna tappo non compilata" CssClass ="label-medio"></asp:Label>
                </p>
	        </a>
        </div>

        <div id="divChiusuraConcorso" runat="server">
            <a class="quick-button metro black span3" href="#" style="margin-left: 5px; height: 191px; opacity: .87; margin-top: 5px;">
		        <i class="icon-time"></i>
		        <p>
                    <asp:Label ID="lblScadenzaSch" runat="server" Text="ddd" CssClass ="label-medio"></asp:Label><br />

                    <asp:Image ID="imgG1" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    <asp:Image ID="imgG2" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    &nbsp;
                    <asp:Image ID="imgH1" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    <asp:Image ID="imgH2" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    &nbsp;
                    <asp:Image ID="imgM1"  runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    <asp:Image ID="imgM2" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    &nbsp;
                    <asp:Image ID="imgS1" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
                    <asp:Image ID="imgS2" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/0.png" Width="20px" Height="20px" />
		        </p>
	        </a>
        </div>

        <div id="divColonnaPropriaDaEffettuare" runat="server">
            <a class="quick-button metro red span3" href="ColonnaPropria.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-pencil"></i>
		        <p><asp:Label ID="Label1" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label2" runat="server" Text="Colonna propria da compilare" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divPartiteGiornata" runat="server">
            <a class="quick-button metro red span3" href="PartiteGiornata.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px; overflow: auto;">
		        <i class="icon-bar-chart"></i>
		        <p><asp:Label ID="Label13" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label14" runat="server" Text="Risultati giornata" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divColonnaPropriaGiaCompilata" runat="server">
            <a class="quick-button metro green span3" href="ColonnaPropria.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-pencil"></i>
		        <p><asp:Label ID="Label3" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="Label4" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divSuddenDeath" runat="server">
            <a id="aSuddendeath" runat="server" href="SuddenDeath.aspx" style="overflow: auto; margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-list-ol" style="margin-top:-25px;"></i>
		        <p style="margin-top: -3px;"><asp:Label ID="lblDescSD" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblSD" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divDerelitti" runat="server">
            <a class="quick-button metro green span3" href="CoppaPippettero.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-list-ol" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblDescDer" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblDer" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divCoppaItalia" runat="server">
            <a class="quick-button metro green span3" href="CoppaPippettero.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-glass" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblDescCI" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblCI" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label><br />
                <asp:Label ID="lblTurnoCI" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label>
		        </p>
	        </a>
        </div>

        <div id="divInterToto" runat="server">
            <a class="quick-button metro green span3" href="CoppaInterToto.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-glass" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblDescIT" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblIT" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label>
		        </p>
	        </a>
        </div>

        <div id="divEuropaLeague" runat="server">
            <a class="quick-button metro green span3" href="CoppaPippettero.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-glass" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblDescEL" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblEL" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label><br />
                <asp:Label ID="lblTurnoEL" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label>
		        </p>
	        </a>
        </div>

        <div id="divChampions" runat="server">
            <a class="quick-button metro green span3" href="Champion.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-glass" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblDescCL" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblCL" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label><br />
                <asp:Label ID="lblTurnoCL" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label>
		        </p>
	        </a>
        </div>

        <div id="divQuoteCUP" runat="server">
            <a class="quick-button metro green span3" href="QuoteCUP.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
		        <i class="icon-glass" style="margin-top:-20px;"></i>
		        <p><asp:Label ID="lblQuoteCUP" runat="server" Text="" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblPartitaQuoteCUP" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label><br />
                <asp:Label ID="lblGiornataQuoteCUP" runat="server" Text="Colonna propria già compilata" CssClass ="label-medio"></asp:Label>
		        </p>
	        </a>
        </div>

        <div id="divVincitore" runat="server">
            <a class="quick-button metro black span3" href="Resoconto.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px;  height: 191px; max-height: 191px;">
			    <div class="header" style="margin-top: -30px; margin-bottom: 5px;">Vincitori concorso</div>
                <ul>
                    <li>
                        <asp:Label ID="lblVincitore" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>&nbsp;<asp:Image ID="imgVincitore" runat="server" Width="45px" height="45px" />
                    </li>
                    <li>
                        <asp:Label ID="lblSecondo" runat="server" Text="Secondo" CssClass ="label-box"></asp:Label>&nbsp;<asp:Image ID="imgSecondo" runat="server" Width="45px" height="45px" />
                    </li>
                    <li>
                        <asp:Label ID="lblUltimo" runat="server" Text="Ultimo" CssClass ="label-box"></asp:Label>&nbsp;<asp:Image ID="imgUltimo" runat="server" Width="45px" height="45px" />
                    </li>
                </ul>
	        </a>
        </div>

        <div id="divClassificaGenerale" runat="server">
            <a class="quick-button metro blue span3" href="ClassificaGenerale.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
			    <div class="header" style="margin-top: -30px; margin-bottom: 5px; color: #03F1E8;">Classifica Generale</div>
                <ul style="margin-top: 25px;">
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosClass1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgClass1" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPunti1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassifica1" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosClass2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgClass2" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPunti2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblclassifica2" runat="server" Text="Secondo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosClass3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgClass3" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPunti3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassifica3" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosClass4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgClass4" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPunti4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassifica4" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosClass5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgClass5" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPunti5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassifica5" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                </ul>
	        </a>
        </div>

        <div id="divClassificaCampionato" runat="server">
            <a class="quick-button metro blue span3" href="ClassificaCampionato.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
			    <div class="header" style="margin-top: -30px; margin-bottom: 5px; color: #03F1E8;">Classifica Campionato</div>
                <ul style="margin-top: 25px;">
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosCamp1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiCamp1" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiCamp1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaCamp1" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosCamp2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiCamp2" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiCamp2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaCamp2" runat="server" Text="Secondo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosCamp3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiCamp3" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiCamp3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaCamp3" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosCamp4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiCamp4" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiCamp4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaCamp4" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosCamp5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiCamp5" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiCamp5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaCamp5" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                </ul>
	        </a>
        </div>

        <div id="divClassificaRisultati" runat="server">
            <a class="quick-button metro blue span3" href="ClassificaRisultati.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
			    <div class="header" style="margin-top: -30px; margin-bottom: 5px; color: #03F1E8;">Classifica Risultati</div>
                <ul style="margin-top: 25px;">
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosRis1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiRis1" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiRis1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaRis1" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosRis2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiRis2" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiRis2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaRis2" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosRis3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiRis3" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiRis3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaRis3" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosRis4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiRis4" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiRis4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaRis4" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosRis5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiRis5" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiRis5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaRis5" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                </ul>
	        </a>
        </div>

        <div id="divClassificaSpeciali" runat="server">
            <a class="quick-button metro blue span3" href="ClassificaSpeciale.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px;">
			    <div class="header" style="margin-top: -30px; margin-bottom: 5px; color: #03F1E8;">Classifica Speciali</div>
                <ul style="margin-top: 25px;">
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosSpec1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiSpec1" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiSpec1" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaSpec1" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosSpec2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiSpec2" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiSpec2" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaSpec2" runat="server" Text="Vincitore" CssClass ="label-box " ></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosSpec3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiSpec3" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiSpec3" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaSpec3" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosSpec4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiSpec4" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiSpec4" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaSpec4" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                    <li style="text-align:left;">
                        <asp:Label ID="lblPosSpec5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Image ID="imgPosiSpec5" runat="server" Width="25px" height="25px" />
                        &nbsp;<asp:Label ID="lblPuntiSpec5" runat="server" Text="Vincitore" CssClass ="label-box" ></asp:Label>
                        &nbsp;<asp:Label ID="lblClassificaSpec5" runat="server" Text="Ultimo" CssClass ="label-box "></asp:Label>
                    </li>
                </ul>
	        </a>
        </div>

        <div id="divSondaggi" runat="server">
            <a class="quick-button metro red span3" href="Principale.aspx?VisualizzaSondaggio=1" style="margin-left: 5px; height: 191px; opacity: .87; margin-top: 5px;">
		        <i class="icon-question-sign"></i>
		        <p><asp:Label ID="Label5" runat="server" Text="Nuovo sondaggio" CssClass ="label-titolo"></asp:Label><br />
                <asp:Label ID="lblGestSondaggio" runat="server" Text="" CssClass ="label-medio"></asp:Label></p>
	        </a>
        </div>

        <div id="divPosizioni" runat="server">
            <a class="quick-button metro yellow span3" href="ClassificaGenerale.aspx" style="margin-left: 5px; opacity: .87; margin-top: 5px;">
                <div class="header" style="margin-top: -30px">Posizioni Classifica Generale</div>
                <div class="content">
					<div id="PosizioniChart" style="margin-left: 5px; opacity: .87; margin-top: 5px; height: 154px; max-height: 191px; " ></div>
				</div>
			</a>
        </div>

        <div id="divRD" runat="server">
            <a class="quick-button metro yellow span3" href="#" style="text-align: left; margin-left: 5px; opacity: .87; margin-top: 5px; height: 191px; max-height: 191px; overflow: auto;">
		        <p style="margin-top: -26px; margin-left: 0px;"><asp:Label ID="Label10" runat="server" Text="Riconosc./Disonori" CssClass ="label-box"></asp:Label>
                <br />
                <asp:Literal ID="ltRicDis" runat="server"></asp:Literal></p>
	        </a>
        </div>
    </div>

    <div id="divSondaggiGest" runat="server" >
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true" style="width: 70%; top: 0px;">
            <asp:HiddenField ID="hdnIdSondaggio" runat="server" />
            <div style="text-align:center; padding-top: 10px;">
                <table style="width: 100%">
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:ImageButton ID="imgSondaggio1" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Sondaggi.png" />
                            <asp:Label ID="lblTitoloSondaggio" runat="server" Text="" CssClass ="label-titolo rosso"></asp:Label>
                            <asp:ImageButton ID="imgSondaggio2" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Sondaggi.png" />
                            <asp:Button ID="cmdChiudeSondaggio" runat="server" Text="Chiude" CssClass="btn btn-primary" />
                            <hr />
                            <asp:Label ID="lblSondaggio" runat="server" Text="" CssClass ="label-medio"></asp:Label>
                            <hr />
                            <div id="idTastiSondaggio" runat ="server" style="width: 100%;">
                                <asp:Button ID="cmdRisposta1" runat="server" Text="Nuovo" CssClass="sinistraconmargine " />
                                <asp:Button ID="cmdRisposta2" runat="server" Text="Nuovo" CssClass="sinistraconmargine " />
                                <asp:Button ID="cmdRisposta3" runat="server" Text="Nuovo" CssClass="sinistraconmargine " />
                                <asp:Button ID="cmdRisposta4" runat="server" Text="Annulla" CssClass="sinistraconmargine " />
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblRisposta" runat="server" Text="" CssClass ="label-titolo rosso"></asp:Label>
                            <br />
                            <asp:Image ID="imgStatSondaggio" runat="server" Width="300px" height="300px"/>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Controllo sondaggi -->
        </div>
    </div>
</asp:Content>