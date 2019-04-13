<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PartiteGiornata.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PartiteGiornata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-list-alt"></i><a href="#">Partite Giornata</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
       <div class="box span8">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite Giornata</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:HiddenField ID="hdnGiornata" runat="server" />
		        <div class="box-content">
                    <div style="width:15%; float: left; text-align: left;">
                        <asp:Button ID="cmdIndietro" runat="server" Text="<<" CssClass ="btn btn-primary" />
                    </div>
                    <div style="width:40%; float: left; text-align:center;">
                        <asp:Label ID="lblGiornata" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                    </div>
                    <div style="width:15%; float: left; text-align:right;">
                        <asp:Button ID="cmdAvanti" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                    </div>
                    <div style="width:15%; float: left; text-align:center;">
                        <asp:Button ID="cmdNuovo" runat="server" Text="Nuovo" CssClass ="btn btn-primary" />
                    </div>
                    <div style="width:15%; float: left; text-align:center;">
                        <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass ="btn btn-primary" />
                    </div>

                    <div class="clearALL "></div>
                </div>

                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"  PagerSettings-PageButtonCount="10" Page-Size="10"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita" HeaderText="Partita" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Casa" HeaderText="Casa" >
                        </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCasa" runat="server" CssClass="ImmagineGriglia" Enabled="False" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="Fuori" HeaderText="Fuori" >
                        </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgFuori" runat="server" CssClass="ImmagineGriglia" Enabled="False" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="Ris" HeaderText="Ris" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgVai"
                                    runat="server" imageurl="App_Themes/Standard/Images/Icone/Freccetta.Png" OnClick="SelezionaPartita" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>

        <div id="divRisultato" runat="server" class="box span4">
		    <div class="box-content">
                <asp:HiddenField ID="hdnPartita" runat="server" />
                <div style="width:99%;">
                    <div style="width:75%; float: left; height: 115px;">
                        <asp:ImageButton ID="imgCasa" runat="server" CssClass="ImmagineGrigliaMedia" Enabled="False" />&nbsp;
                        <asp:Label ID="lblCasa" runat="server" Text="Label"></asp:Label>
                        <asp:TextBox ID="txtCasa" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                        <br /><br />
                        <asp:ImageButton ID="ImgFuori" runat="server" CssClass="ImmagineGrigliaMedia" Enabled="False" />&nbsp;
                        <asp:Label ID="lblFuori" runat="server" Text="Label"></asp:Label>
                        <asp:TextBox ID="txtFuori" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                    </div>
                    <div style="width:24%; float:right; height: 115px; text-align: center;">
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Risultato"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtRisultato" runat="server" MaxLength ="5" Width="50px"></asp:TextBox>
                    </div>

                    <div class="clearALL "></div>

                    <div style="width: 99%; text-align: right;">
                        <asp:Button ID="cmdElimina" runat="server" Text="Elimina" CssClass ="btn btn-primary" />&nbsp;
                        <asp:Button ID="cmdSalvaRis" runat="server" Text="Salva" CssClass ="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
