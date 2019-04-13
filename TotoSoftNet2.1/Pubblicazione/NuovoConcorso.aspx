<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="NuovoConcorso.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.NuovoConcorso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

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
		<li><i class="icon-asterisk"></i><a href="#">Nuovo concorso</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Nuovo concorso</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <div class="span8">
                    <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Partita" HeaderText="P." >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Casa">
                                <ItemTemplate>
                                    <%--OnTextChanged ="VisuaImmagineCasa"--%>
                                    <asp:TextBox ID="txtCasa" runat="server" MaxLength ="20" Font-Names="Verdana" AutoPostBack ="true" Font-Size="Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblSerieC" runat="server" Text="" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCasa" runat="server" CssClass="ImmagineGrigliaMedia" OnClick ="CambiaImmagineSquadraCasa" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSceglieCasa" runat="server" CssClass="ImmagineGrigliaMedia" OnClick ="SceltaSquadraCasa" ImageUrl="App_Themes/Standard/Images/Icone/icona_CERCA.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fuori">
                                <ItemTemplate> 
                                    <%--OnTextChanged ="VisuaImmagineFuori"--%>
                                    <asp:TextBox ID="txtFuori" runat="server" MaxLength ="20" Font-Names="Verdana" AutoPostBack ="true" Font-Size="Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblSerieF" runat="server" Text="" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgFuori" runat="server" CssClass="ImmagineGrigliaMedia" OnClick ="CambiaImmagineSquadraFuori" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSceglieFuori" runat="server" CssClass="ImmagineGrigliaMedia" OnClick ="SceltaSquadraFuori" ImageUrl="App_Themes/Standard/Images/Icone/icona_CERCA.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgJolly" runat="server" CssClass="ImmagineGrigliaMedia" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                    </asp:GridView>
                </div>
                <div class="box span4">
                    <div class="span12" style="text-align: center;    margin-left: 0px;">
                        <asp:Label ID="Label1" runat="server" Text="Chiusura concorso" CssClass ="label"></asp:Label>
                        <asp:TextBox ID="txtChiusura" runat="server" CssClass ="input-xlarge"></asp:TextBox>&nbsp;
                    </div>
                    <div class="span12" style="text-align: center;    margin-left: 0px;">
                        <asp:Label ID="Label2" runat="server" Text="Orario chiusura" CssClass ="label "></asp:Label>
                        <asp:TextBox ID="txtOrario" runat="server" CssClass ="input-xlarge"></asp:TextBox>
                    </div>
                    <div class="span12" style="text-align: center; margin-top: 4px; margin-left: 0px;">
                        <asp:Label ID="Label3" runat="server" Text="Partita speciale" CssClass ="label "></asp:Label>
                        <asp:CheckBox ID="chkSpeciale" runat="server" />
                    </div>
                    <div class="box span11" style="text-align: center; margin: 6px;">
                        <asp:Label ID="Label4" runat="server" Text="Salva anche se non trova la squadra sul DB" CssClass ="label "></asp:Label>
                        <asp:CheckBox ID="chkSkipNomiSquadre" runat="server" />
                    </div>
                    <div class="span12" style="text-align: center; margin: 4px; margin-left: 0px; ">
                        <asp:Button ID="cmdAggiornaQuote" runat="server" Text="Aggiorna Quote" class="btn btn-primary"  />
                        <asp:Button ID="cmdCaricaClassifiche" runat="server" Text="Classifiche" class="btn btn-primary"  />
                        <%--<asp:Button ID="cmdCreaSchedina" runat="server" Text="Crea Schedina" CssClass ="bottone" />--%>
                        <asp:Button ID="cmdSalva" runat="server" Text="Salva" class="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="divCambiaStemma" runat="server">
        <div id="divBloccaFinestra" class="bloccafinestra" runat="server" ></div>
        <asp:HiddenField ID="hdnSquadra" runat="server" />
        <asp:HiddenField ID="hdnDove" runat="server" />
        <div id="divPopup" class="popupstemma" runat="server" >
			<fieldset>
                <div style="width: 49%; text-align : center; float:left;">
                    <asp:Image ID="imgStemma" runat="server" Width="90px" />
                </div>
                <div style="width: 49%; text-align : center; float:left;">
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                </div>
                <div class="clearALL "></div>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="cmdOK" runat="server" Text="Salva" CssClass="btn btn-primary" OnClick="cmdOK_Click" />

                        <asp:Button id="cmdAnnulla" class="btn btn-primary" runat="server" Text="Annulla" OnClick="cmdAnnulla_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="cmdOK" />  
                        <asp:PostBackTrigger ControlID="cmdAnnulla" />  
                    </Triggers>
                </asp:UpdatePanel> 
            <//fieldset>
<%--            <asp:Button id="cmdOK" class="bottone" runat="server" Text="OK" />--%>
        </div>
    </div>
    
    <div id="divSceltaSquadra" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" ></div>

        <asp:HiddenField ID="hdnNumRiga" runat="server" />
        <div id="div1" class="popupstemma" runat="server" >
            <ul>
                <li>
                    <asp:Label ID="Label5" runat="server" Text="Categoria" Font-Italic="True" Font-Size="12" Font-Names="Verdana" ForeColor="#006600" Width="200px"></asp:Label>
                    <asp:DropDownList ID="cmbCategoria" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CaricaSquadreCategoria"></asp:DropDownList>
                </li>
                <li>
                    <br />
                </li>
                <li>
                    <asp:Label ID="Label6" runat="server" Text="Squadra" Font-Italic="True" Font-Size="12" Font-Names="Verdana" ForeColor="#006600" Width="200px"></asp:Label>
                    <asp:DropDownList ID="cmbSquadra" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SceltaSquadreCategoria"></asp:DropDownList>
                </li>
                <li>
                    <br />
                </li>
                <li>
                    <asp:Label ID="Label7" runat="server" Text="Squadra scelta" Font-Italic="True" Font-Size="12" Font-Names="Verdana" ForeColor="#006600" Width="200px"></asp:Label>
                    <asp:ImageButton ID="imgScelta" runat="server" CssClass="ImmagineGrigliaMedia" />
                    <asp:Label ID="lblSquadraScelta" runat="server" Text="" Font-Italic="True" Font-Size="12" Font-Names="Verdana" ForeColor="#006600" Width="200px"></asp:Label>
                </li>
                <li>
                    <br />
                </li>
                <li>
                    <asp:Button ID="cmdOkSS" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    <asp:Button id="cmdAnnullaSS" class="btn btn-primary" runat="server" Text="Annulla" />
                </li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">

</asp:Content>
