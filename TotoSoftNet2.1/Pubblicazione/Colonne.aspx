<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Colonne.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Colonne" %>

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
		<li><i class="icon-list"></i><a href="#">Colonne</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="padding: 2px; display: inline-block;">
        <div style="width: 50%; float: left; text-align: center;">
            <asp:Image ID="imgUtente" runat="server" Width="70px" Height="70px" />
        </div>
        <div style="width: 49%; float: left; text-align: center;">
            <asp:Label ID="lbl1" runat="server" Text="Giocatore" CssClass ="label-medio rosso " ></asp:Label>
            <br />
            <asp:DropDownList ID="cmbGiocatore" runat="server" CssClass="label-medio" AutoPostBack="True" Width="200px" ></asp:DropDownList>
        </div>
        <div class="clear clearALL "></div>
    </div>

    <div id="divColonna" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Squadre Prese</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>

		    <div class="box-content" style="overflow: auto;">
                <asp:HiddenField ID="hdnGiocatore" runat="server" />

                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita" HeaderText="P." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Casa">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCasa" runat="server" MaxLength ="15" Font-Names="Verdana" Enabled="False" Font-Size="Small" Width="150px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblSerieC" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgCasa" runat="server" Enabled="False"  CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fuori">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFuori" runat="server" MaxLength ="15" Font-Names="Verdana" Enabled="False" Font-Size="Small" Width="150px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblSerieF" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFuori" runat="server" Enabled="False" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="1">
                            <ItemTemplate>
                                <asp:CheckBox id="chk1" runat="server" Enabled="False"></asp:CheckBox>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="X">
                            <ItemTemplate>
                                <asp:CheckBox id="chkX" runat="server" Enabled="False"></asp:CheckBox>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="2">
                            <ItemTemplate>
                                <asp:CheckBox id="chk2" runat="server" Enabled="False"></asp:CheckBox>
                            </ItemTemplate>                                    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ris.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRisultato" runat="server" Width="30" MaxLength ="6" Font-Names="Verdana" Font-Size="Small" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgJolly" runat="server" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Image ID="imgFR" runat="server" CssClass="ImmagineGriglia" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
