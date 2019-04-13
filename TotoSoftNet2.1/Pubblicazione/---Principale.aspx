<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="---Principale.aspx.vb" Inherits="TotoSoftNet21.Principale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <link href="App_Themes/Standard/Griglie.css" rel="stylesheet" />

    <!-- Countdown -->
    <script src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300" />
    <%--<link rel="stylesheet" href="App_Themes/Standard/countdown/css/styles.css" />--%>
    <link rel="stylesheet" href="App_Themes/Standard/jquery.countdown.css" />
    <!-- Countdown -->

     <script type="text/javascript">
         function AzionaCountdown(Anno, MeseMenoUno, Giorno, Ora, Minuti, Secondi) {
             var datella = Giorno + '/' + (MeseMenoUno + 1) + '/' + Anno + ' ' + Ora + ':' + Minuti + ':' + Secondi;

             var note = $('#ctl00_contentCentrale_note'),
              ts = new Date(Anno, MeseMenoUno, Giorno, Ora, Minuti, Secondi),
              newYear = true;

             if ((new Date()) > ts) {
                 ts = (new Date()).getTime() + 10 * 24 * 60 * 60 * 1000;
                 //newYear = false;
             }

             $('#ctl00_contentCentrale_countdown').countdown({
                 timestamp	: ts,
                 callback	: function(days, hours, minutes, seconds){
			
                     var message = "Tempo rimanente per la compilazione della colonna<br /><br />";
			
                     //message += days + " Giorni " +  ", ";
                     //message += hours + " Ore " + ", ";
                     //message += minutes + " Minuti " + " e ";
                     //message += seconds + " Secondi " + " <br />";
			
                     message += datella;

                     //if(newYear){
                     //	message += "left until the new year!";
                     //}
                     //else {
                     //	message += "left to 10 days from now!";
                     //}
			
                     note.html(message);
                 }
             });
         }
        //});
        // -->
    </script> 

</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenuNavigazione" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div style="width: 15%; float: left; ">
         
       <div id="BachecaTrofei" runat ="server" class="bacheca draggable">
           <div class="bachecaint">
                <asp:Literal ID="ltRicDis" runat="server"></asp:Literal>
           </div>
       </div>

       <ul class ="mascherinaavvisi draggable" style="padding:3px;">
            <li>
                &nbsp;
            </li>
            <li>
                <asp:Label ID="lblDesc1" runat="server" Text="Pos. per Punti Tot" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblPosTot" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
            <li>
                <hr />
            </li>
            <li>
                <asp:Label ID="lblDesc2" runat="server" Text="Pos. per Risultati" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblPosRis" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
            <li>
                <hr id="barra" runat="server" />
                <asp:Label ID="lblDesc3" runat="server" Text="Pos. per Campionato" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblPosCamp" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
            <li id="liSpazioCamp" runat="server">
                &nbsp;
            </li>
        </ul>

        <ul id="ulIntertoto" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescIT" runat="server" Text="Intertoto" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblIT" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
        </ul>

        <ul id="ulSuddenDeath" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescSD" runat="server" Text="Sudden Death" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblSD" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
        </ul>

        <ul id="ulCoppaItalia" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescCI" runat="server" Text="Coppa Italia" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblCI" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label><br />
                <asp:Label ID="lblTurnoCI" runat="server" Text="1" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
        </ul>

        <ul id="ulEuropaleague" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescEL" runat="server" Text="" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblEL" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label><br />
                <asp:Label ID="lblTurnoEL" runat="server" Text="1" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
        </ul>

        <ul id="ulChampions" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescCL" runat="server" Text="" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblCL" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label><br />
                <asp:Label ID="lblTurnoCL" runat="server" Text="1" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
        </ul>

        <ul id="ulDerelitti" runat="server" class ="mascherinaavvisi draggable" style="padding:3px; margin-top: 2px;">
            <li>
                <asp:Label ID="lblDescDer" runat="server" Text="" CssClass ="etichettatitolodett" ></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblDer" runat="server" Text="1" CssClass ="etichettaprincdett2" ></asp:Label>
            </li>
        </ul>
    </div>

    <div style="width: 83%; float: right;">
        <div id="divCountdown" runat="server">
            <div id="divNote" runat="server" class="mascherinacountdown draggable">
                <p id="note" runat="server" class ="etichettaprincdett2" style="text-align:center;"></p>
            </div>
            <div class ="clear clearALL "></div>
            <div id="countdown" runat="server" class="mascherinacountdown draggable" style="padding-top: 25px; padding-left: 10px; padding-right: 10px;">
                <script src="Js/jquery.countdown.js"></script>
            </div>
            <div class ="clear clearALL "></div>
        </div>

        <div id="divControlloTappo" runat="server" class="mascherinarossa draggable">
            <!-- Controllo inserimento colonna tappo -->
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgTappo1" runat="server" Width="40px" />
            </div>
            <div style="width: 300px; float: left; text-align:center; padding-top: 10px;">
                <a href="ColonnaTappo.aspx">
                    <asp:Label ID="lblTappo" runat="server" Text="Label" CssClass ="etichettatitoloschedina"></asp:Label>
                </a>
            </div>
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgTappo2" runat="server" Width="40px" />
            </div>
        </div>
        <div id="divMessaggi" runat="server" class="mascherina draggable">
            <!-- Controllo messaggi in arrivo -->
            <div style="width: 60px; float: left;">
                <a href="LetturaMessaggi.aspx">
                    <asp:ImageButton ID="imgMessaggi1" runat="server" Width="40px" />
                </a>
            </div>
            <div style="width: 300px; float: left; text-align:center; padding-top: 10px;">
                <a href="LetturaMessaggi.aspx">
                    <asp:Label ID="lblMessaggi" runat="server" Text="" CssClass ="etichettatitoloschedina"></asp:Label>
                </a>
            </div>
            <div style="width: 60px; float: left;">
                <a href="LetturaMessaggi.aspx">
                    <asp:ImageButton ID="imgMessaggi2" runat="server" Width="40px" />
                </a>
            </div>
            <!-- Controllo messaggi in arrivo -->
        </div>

        <div id="divControlloSchedina" runat="server" class="mascherina draggable">
            <!-- Controllo schedina su concorso aperto -->
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgSchedina1" runat="server" Width="40px" />
            </div>
            <div style="width: 300px; float: left; text-align:center; padding-top: 10px;">
                <a href="ColonnaPropria.aspx">
                    <asp:Label ID="lblSchedina" runat="server" Text="Label" CssClass ="etichettatitoloschedina"></asp:Label>
                </a>
            </div>
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgSchedina2" runat="server" Width="40px" />
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="Avanza giornata" />
        <div id="divVincitoriPerdenti" runat="server" style="width: 99%; text-align:center;">
            <div class="mascherinaprimi draggable" style="width: 31%; float: left; text-align:center; padding-top: 10px; padding-bottom: 10px; margin-left:3px; overflow:auto;">
                <asp:Label ID="lbl1" runat="server" Text="Primi" CssClass ="etichettatitolopremi"></asp:Label>
                 
                <asp:GridView ID="grdVincitori" runat="server" AutoGenerateColumns="False" 
                    CssClass="griglia intestazioneverde" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiTot" HeaderText="Tot" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiJolly" HeaderText="Jolly" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiQuote" HeaderText="Quote" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="mascherinasecondi draggable" style="width: 31%; float: left; text-align:center; padding-top: 10px; padding-bottom: 10px; margin-left:3px; overflow:auto;">
                <asp:Label ID="lbl2" runat="server" Text="Secondi" CssClass ="etichettatitolopremi"></asp:Label>
                 
                <asp:GridView ID="grdSecondi" runat="server" AutoGenerateColumns="False" 
                    CssClass="griglia intestazioneblu" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiTot" HeaderText="Tot" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiJolly" HeaderText="Jolly" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiQuote" HeaderText="Quote" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="mascherinarossa draggable" style="width: 31%; float: left; text-align:center; padding-top: 10px; padding-bottom: 10px; margin-left:3px; overflow:auto;">
                <asp:Label ID="lbl3" runat="server" Text="Ultimi" CssClass ="etichettatitolopremi"></asp:Label>
                 
                <asp:GridView ID="grdUltimi" runat="server" AutoGenerateColumns="False" 
                    CssClass="griglia intestazionerossa" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiTot" HeaderText="Tot" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiSegni" HeaderText="1X2" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiRis" HeaderText="Ris." >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiJolly" HeaderText="Jolly" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="PuntiQuote" HeaderText="Quote" >
                            <HeaderStyle CssClass="cella-testata-griglia-princ" />
                            
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div id="divSondaggi" runat="server" class="mascherina draggable">
            <!-- Controllo sondaggi -->
            <asp:HiddenField ID="hdnIdSondaggio" runat="server" />
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgSondaggio1" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Sondaggi.png" />
            </div>
            <div style="width: 300px; float: left; text-align:center; padding-top: 10px;">
                <asp:Label ID="lblTitoloSondaggio" runat="server" Text="" CssClass ="etichettatitoloschedina"></asp:Label>
                <hr />
                <asp:Label ID="lblSondaggio" runat="server" Text="" CssClass ="etichettatitoloschedina"></asp:Label>
                <hr />
                <div id="idTastiSondaggio" runat ="server">
                    <asp:Button ID="cmdRisposta1" runat="server" Text="Nuovo" CssClass="bottone" />
                    <asp:Button ID="cmdRisposta2" runat="server" Text="Nuovo" CssClass="bottone" />
                    <asp:Button ID="cmdRisposta3" runat="server" Text="Nuovo" CssClass="bottone" />
                </div>
                <div id="idRispostaSondaggio" runat ="server">
                    <asp:Label ID="lblRisposta" runat="server" Text="" CssClass ="etichettatitoloschedina"></asp:Label>
                    <hr />
                    <asp:Image ID="imgStatSondaggio" runat="server" Width="300px" height="300px"/>
                </div>
            </div>
            <div style="width: 60px; float: left;">
                <asp:ImageButton ID="imgSondaggio2" runat="server" Width="40px" ImageUrl ="App_Themes/Standard/Images/Icone/Sondaggi.png" />
            </div>
            <!-- Controllo sondaggi -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>