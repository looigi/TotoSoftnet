<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="AggiornaConcorso.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.AggiornaConcorso" %>

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
		<li><i class="icon-pencil"></i><a href="#">Aggiorna concorso</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12" style="text-align:right; margin-bottom: 3px;">
        &nbsp;<asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary" />
        &nbsp;<asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" />
    </div>

<%--	<div class="box-header">
		<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Concorso</h2>
		<div class="box-icon">
			<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
		</div>
	</div>--%>

    <div class="box span10" style="margin-left: 0px; ">
	    <div id="divColonna" runat="server" class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Concorso</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
	    <div class="box-content " style="overflow: auto;">
            <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Partita" HeaderText="P." >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Casa">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCasa" runat="server" MaxLength ="20" Font-Names="Verdana" Font-Size="Small" Enabled="False"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblSerieC" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgCasa" runat="server" Class="ImmagineGrigliaMedia" Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fuori">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFuori" runat="server" MaxLength ="20" Font-Names="Verdana" Font-Size="Small" Enabled="False"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblSerieF" runat="server" Text="Label" Font-Italic="True" Font-Size="8" Font-Names="Verdana" ForeColor="#006600"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgFuori" runat="server" Class="ImmagineGrigliaMedia" Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ris.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRisultato" runat="server" Width="30" MaxLength ="6" Font-Names="Verdana" Font-Size="Small" ></asp:TextBox><%--OnTextChanged="AggiornaSegno" AutoPostBack="True"--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Segno">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSegno" runat="server" Width="30" MaxLength ="6" Font-Names="Verdana" Font-Size="Small" Enabled="False"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="imgJolly" runat="server" CssClass="ImmagineGriglia" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sospesa">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSospesa" runat="server" OnCheckedChanged ="SpegneTesto" CssClass ="casellacheck " AutoPostBack="True" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
