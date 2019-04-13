<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="CoppaChampionsLeague.aspx.vb" Inherits="TotoSoftNet21.CoppaChampionsLeague" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-glass"></i>
			<a href="PulsantieraCoppe.aspx">Coppe</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Champion's League</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
		<div class="box span12">
            <div class="box-content">
                <fieldset>
                    <div class="span7" style="text-align: center;">
                        <img src="App_Themes/Standard/Images/Icone/Champions.png" width="50" />
                        <asp:Label ID="Label2" runat="server" Text="Champion's League" CssClass ="label-titolo"></asp:Label>
                        <img src="App_Themes/Standard/Images/Icone/Champions.png" width="50" />
                    </div>

                    <div class="span5" style="text-align: right;">
                        <asp:Button ID="cmdTurniA" runat="server" Text="Turni Girone A" CssClass="btn btn-primary" />
                        <asp:Button ID="cmdTurniB" runat="server" Text="Turni Girone B" CssClass="btn btn-primary" />
                        <asp:Button ID="cmdSemifinale" runat="server" Text="Semifinale" CssClass="btn btn-primary" />
                        <asp:Button ID="cmdFinale" runat="server" Text="Finale" CssClass="btn btn-primary" />
                    </div>
                </fieldset> 
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnGirone" runat="server" />

    <div class="row-fluid sortable ui-sortable">				
        <div class="span12">
            <div id="divApertoChiuso" runat="server" class="mascherinaavvisi">				
                <asp:Label ID="lblAvviso" runat="server" Text="La competizione non è ancora cominciata" CssClass ="label "></asp:Label>
            </div>
        </div>
    </div>

    <div class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px; margin-top: -55px;">
            <div id="divVincente" runat="server" class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincente" CssClass ="etichettatitolocoppe "></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/Vincitore.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/Vincitore.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="etichettatitolocoppe "></asp:Label>
            </div>
        </div>
    </div>

    <div id="divQualificate" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="box span5" style="margin-left: 0px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Giocatori Qualificati</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdQualificate" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Posizione"  HeaderText="Pos." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" width="50" Height="50" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Provenienza" HeaderText="Prov." HtmlEncode="False">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div id="divDettaglio" runat ="server" class="box span7" style="margin-left: 4px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:HiddenField ID="hdnGiornata" runat="server" />

                <div class="span12" >
                    <div class="span3" style="text-align: left;">
                        <asp:Button ID="cmdIndietroG" runat="server" Text="<<" CssClass ="btn btn-primary" />
                    </div>
                    <div class="span6" style="text-align: center;">
                        <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                    <div class="span3" style="text-align: right;">
                        <asp:Button ID="cmdAvantiG" runat="server" Text=">>" CssClass ="btn btn-primary" />
                    </div>
                </div>

                <asp:GridView ID="grdPartiteTurni" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarC" runat="server" width="50" Height="50" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Casa"  HeaderText="Casa" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarF" runat="server" width="50" Height="50" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Fuori" HeaderText="Fuori" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Risultato" HeaderText="Risultato" HtmlEncode="False">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="grdPartiteDiretti" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarC" runat="server" width="50" Height="50" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Casa"  HeaderText="Casa" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarF" runat="server" width="50" Height="50" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Fuori" HeaderText="Fuori" >
                        </asp:BoundField>
                        <asp:BoundField DataField="RisAndata" HeaderText="Ris. Andata" HtmlEncode="False">
                        </asp:BoundField>
                        <asp:BoundField DataField="RisRitorno" HeaderText="Ris. Ritorno" HtmlEncode="False">
                        </asp:BoundField>
                        <asp:BoundField DataField="Vincente" HeaderText="Vincente" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
