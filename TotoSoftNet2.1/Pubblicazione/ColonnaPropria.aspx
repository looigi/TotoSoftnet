<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnIISM.Master" CodeBehind="ColonnaPropria.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ColonnaPropria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-pencil"></i><a href="#">Compilazione colonna propria</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divApertoChiuso" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="box span12" style="text-align: center;">
            <asp:Label ID="lblAvviso" runat="server" Text="Il concorso non è aperto" CssClass ="label-info "></asp:Label>
        </div>
    </div>

   <div id="divContenitoreColonna" runat="server" >
        <div id="divTasti" runat="server" class="span5" style="text-align: left; padding: 4px; margin-top: -26px; z-index: 1010; overflow: auto;">
            <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary" />
            <asp:Button ID="cmdUsaTappo" runat="server" Text="Usa Tappo" CssClass="btn btn-primary " />
            <asp:Button ID="cmdRandom" runat="server" Text="Random" CssClass="btn btn-primary " />
            <!-- <asp:Button ID="cmdRandomRis" runat="server" Text="Rnd Ris." CssClass="btn btn-primary " /> -->
            <asp:Button ID="cmdStat" runat="server" Text="Per Stat" CssClass="btn" style="background-color: #eb6d6d; border-color: #e68c8c;" />
            <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" />
        </div>
        <div id="div1" runat="server" class="span5" style="text-align: center; padding: 4px; margin-top: -26px;">
		    <asp:Label ID="lblSpeciale" runat="server" Text="Concorso speciale" CssClass ="label-titolo" ForeColor="#AA0000"></asp:Label>
        </div>
        <div id="divTasti2" runat="server" class="span2" style="text-align: right; padding: 4px; margin-top: -26px;">
            <asp:Button ID="cmdRitorno" runat="server" Text="Uscita" CssClass="btn btn-primary " />
        </div>

        <div class="box span7" style="margin-left: 0px; overflow: auto; ">
	        <div id="divColonna" runat="server" class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Colonna</h2>&nbsp;&nbsp;
                <asp:Label ID="lblTotQuote" runat="server" Text="" CssClass ="label-box" ForeColor="#DDDDDD"></asp:Label>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content" style="overflow:auto;">
                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-condensed "
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita" HeaderText="P." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Casa">
                            <ItemTemplate>
                                <div style="width:99%; margin-top: 2px; border-bottom: 1px solid #aaa;">
                                    <div style="width:95%; float:left; text-align: center; border-bottom: 1px solid #aaa; margin-bottom: 3px; padding: 4px;">
                                        <asp:Label ID="txtCasa" runat="server" Width="150px" Font-Italic="True" Font-Size="14px" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                    </div>
                                    <div style="width:25%; float: left; margin-top: 3px;">
                                       <asp:ImageButton ID="imgCasaPrec" runat="server" Enabled="True" src="App_Themes/Standard/Images/Icone/Apri-Chiudi.png" Width="20px" OnClick="ApriChiudiCasa" />
                                    </div>
                                    <div style="width:74%; float: left; margin-top: 4px; margin-bottom: 10px;">
                                        <asp:Label ID="lblSerieC" runat="server" Text="Label" Width="150px" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                    </div>
                                    <asp:HiddenField ID="hdnCasa" runat="server" />
                                    <div id="divPrecCasa" runat="server" style="width:95%; float:left; padding:3px; margin-top: 12px;">

                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgCasa" runat="server" Enabled="False" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fuori">
                            <ItemTemplate>
                                <div style="width:99%; margin-top: 2px; border-bottom: 1px solid #aaa;">
                                    <div style="width:95%; float:left; text-align: center; border-bottom: 1px solid #aaa; margin-bottom: 3px; padding: 4px;">
                                        <asp:Label ID="txtFuori" runat="server" Width="150px" Font-Italic="True" Font-Size="14px" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                    </div>
                                    <div style="width:25%; float: left; margin-top: 3px;">
                                        <asp:ImageButton ID="imgFuoriPrec" runat="server" Enabled="True" src="App_Themes/Standard/Images/Icone/Apri-Chiudi.png" Width="20px" OnClick="ApriChiudiFuori" /> &nbsp;
                                    </div>
                                    <div style="width:74%; float: left; margin-top: 4px; margin-bottom: 10px; ">
                                        <asp:Label ID="lblSerieF" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                    </div>
                                    <div id="divPrecFuori" runat="server" style="width:95%; float:left; padding:3px; margin-top: 12px;">

                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFuori" runat="server" Enabled="False" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="1">
                            <ItemTemplate>
                                <asp:CheckBox id="chk1" runat="server" ></asp:CheckBox> <%--AutoPostBack="True" OnCheckedChanged ="CalcolaQuoteCheck"--%>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="X">
                            <ItemTemplate>
                                <asp:CheckBox id="chkX" runat="server" ></asp:CheckBox> <%--AutoPostBack="True" OnCheckedChanged ="CalcolaQuoteCheck"--%>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="2">
                            <ItemTemplate>
                                <asp:CheckBox id="chk2" runat="server" ></asp:CheckBox><%--AutoPostBack="True" OnCheckedChanged ="CalcolaQuoteCheck"--%>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ris.">
                            <ItemTemplate>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txtRisultato" runat="server" Width="30" MaxLength ="6" Font-Names="Verdana" Font-Size="Small"></asp:TextBox> <%--OnTextChanged="CalcolaQuoteText" AutoPostBack="True"--%>
                                    <br />
                                    <asp:Label ID="lblStat" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600" Enabled="False" Width="60px"></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FR">
                            <ItemTemplate>
                                <asp:CheckBox id="chkFR" runat="server" ></asp:CheckBox><%-- AutoPostBack="True"OnCheckedChanged ="CalcolaQuoteCheck"--%>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgJolly" runat="server" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgStorico" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/Storico.png" OnClick="StoricoPartita" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgSeleziona" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/Freccetta.png" OnClick="SelezionaPartita" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div id="divStat" runat="server" class="span5" >
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Statistiche</h2>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
		    </div>
		    <div class="box-content">
                <div id ="divDifferenze" runat="server">
                </div>
            </div>
        </div>

       <div id="divVelo" runat ="server" class="bloccafinestra"></div>
       <div id="divStorico" runat="server" class="mascherinaavvisi" style="position: absolute; left: 50%; top: 40%; margin-left: -200px; width: 400px; margin-top: -300px; height: auto; max-height: 450px; overflow: auto; padding: 5px; text-align:center;z-index: 1000;">
            <div class="row-fluid sortable ui-sortable">	
               <div class="span12" style="margin-bottom:4px;">
                   <div class="span9" style="text-align:center;">
                        <asp:ImageButton ID="imgCasa" runat="server" Enabled="False"  CssClass="ImmagineGriglia" />
                        <asp:Label ID="lblStorico" runat="server" Text="" CssClass ="label-medio rosso" ></asp:Label>
                        <asp:ImageButton ID="imgFuori" runat="server" Enabled="False"  CssClass="ImmagineGriglia" />
                   </div>
                   <div class="span3" style="text-align:right;">
                        <asp:ImageButton ID="imgChiudi"
                            runat="server" imageurl="App_Themes/Standard/Images/Icone/elimina_quadrato.Png" />
                   </div>
               </div>
            </div>
            <div class="box span11">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Storico incontri</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdStorico" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-condensed "
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Campionato" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Giornata" HeaderText="Giornata" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Segno" HeaderText="Segno" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Risultato" HeaderText="Risultato" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                            </asp:BoundField>
                        </Columns> 
                    </asp:GridView>
                </div>
            </div>
       </div>

<%--        <div id="divTastino" runat="server" style="position: fixed; left: 22px; top: 18px; z-index: 101;">
            <asp:ImageButton ID="imgApreChiudes" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/PosizioneMigliore.png" />
        </div>--%>
    </div>
</asp:Content>

