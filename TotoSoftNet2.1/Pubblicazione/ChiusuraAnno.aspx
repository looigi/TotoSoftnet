<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ChiusuraAnno.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ChiusuraAnno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-user-md"></i>
			<a href="PulsantieraAmministrazione.aspx">Amministrazione</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-lock"></i><a href="#">Chiusura Anno</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Bilancio giocatori</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdBilancio" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                    </asp:BoundField>
                    <asp:BoundField DataField="TotDare" HeaderText="Pagamenti" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Reali" HeaderText="Reali" >
                        <HeaderStyle CssClass="cella-testata-griglia"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Vinti" HeaderText="Vinti" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Presi" HeaderText="Presi" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Bilancio" HeaderText="Bilancio" >
                    </asp:BoundField>
                        <asp:BoundField DataField="Amici" HeaderText="Pres. Amici" >
                    </asp:BoundField>
                        <asp:BoundField DataField="Totale" HeaderText="Totale" >
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="span12" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Vittorie</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdVittorie" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Image ID="imgTipologia" runat="server" CssClass="ImmagineGrigliaGrande"/>
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Tipologia" HeaderText="Tipologia" >
                    </asp:BoundField>
                    <asp:BoundField DataField="TotDareV" HeaderText="Importo" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Vincitore" HeaderText="Vincitore" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Image ID="imgAvatarV" runat="server" CssClass="ImmagineGrigliaGrande"/>
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>  
        </div>      
    </div>

    <div class="span12" style="margin-left: 0px;">
        <asp:Button id="cmdChiudeAnno" class="btn btn-primary " runat="server" Text="Chiude Anno" />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
