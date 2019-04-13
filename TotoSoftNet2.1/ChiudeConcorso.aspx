<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ChiudeConcorso.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ChiudeConcorso" %>

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
		<li><i class="icon-lock"></i><a href="#">Chiude concorso</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12">
        <div id="divMancanti" runat="server" class="box span6">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Mancanti</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdMancanti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>      
            </div>  
        </div>

        <div class="box span6" style="margin-left: 4px; text-align: center; padding-top: 13px;">
            <asp:Button ID="cmdAvvisa1" runat="server" Text="Avvisa i mancanti SOFT" CssClass="btn btn-primary" style="margin-bottom: 14px;" Width="214px" />
            <asp:Button ID="cmdAvvisa2" runat="server" Text="Avvisa i mancanti HARD" CssClass="btn btn-primary" style="margin-bottom: 14px; " Width="214px" />
            <asp:Button ID="cmdAvvisa3" runat="server" Text="Avvisa i mancanti LAST" CssClass="btn btn-primary" style="margin-bottom: 14px; " Width="214px" />
            <asp:Button ID="cmdChiude" runat="server" Text="Chiude concorso" CssClass="btn btn-primary" style="margin-bottom: 14px; " Width="214px" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
