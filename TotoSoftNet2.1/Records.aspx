<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Records.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Records" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-star"></i>
			<a href="PulsantieraStatistiche.aspx">Statistiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-time"></i><a href="#">Records</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style=" margin: 3px; padding: 3px; text-align: center;">
        <div class="span2"><asp:CheckBox ID="chkSegni" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged ="Ricarica" Text="Punti Segni" CssClass ="label-medio " /></div>
        <div class="span3"><asp:CheckBox ID="chkRisultati" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged ="Ricarica" Text="Punti Risultati" CssClass ="label-medio " /></div>
        <div class="span2"><asp:CheckBox ID="chkJolly" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged ="Ricarica" Text="Punti Jolly" CssClass ="label-medio " /></div>
        <div class="span2"><asp:CheckBox ID="chkQuote" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged ="Ricarica" Text="Punti Quote" CssClass ="label-medio " /></div>
        <div class="span2"><asp:CheckBox ID="chkFR" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged ="Ricarica" Text="Punti FR" CssClass ="label-medio " /></div>
    </div>

    <div class="row-fluid sortable ui-sortable">				
        <div class="box span6" style="margin-left: 0px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max Punti Giornata</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMaxPunti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Giornata" HeaderText="Giornata" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Min Punti Giornata</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMinPunti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Giornata" HeaderText="Giornata" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max Punti Campionato</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMaxPuntiCampionato" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Partite" HeaderText="Partite" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Min Punti Campionato</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMinPuntiCampionato" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Partite" HeaderText="Partite" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max Punti Storico</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMaxPuntiStorico" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Partite" HeaderText="Partite" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Min Punti Storico</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMinPuntiStorico" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Punti" HeaderText="Punti" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Partite" HeaderText="Partite" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte primo campionato</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPrimiCampionato" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte secondo campionato</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdSecondiCampionato" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte ultimo campionato</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdUltimoCampionato" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte primo storico</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPrimiStorico" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte secondo storico</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdSecondiStorico" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Max volte ultimo storico</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdUltimoStorico" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Giocate</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdGiocate" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quante" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div class="box span6" style="margin-left: 2px; margin-top: 2px;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Numero tappi</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdTappi" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quanti" HeaderText="Volte" >
                        </asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
