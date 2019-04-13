<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="RiconDison.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.RiconDison" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenuNavigazione" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divDettaglio" runat="server" class="box span8" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Riconoscimenti / Disonori</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box box-content">
            <asp:GridView ID="grdRiconDison" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="imgImmagine" runat="server" width="80" Height="80" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Giornata" HeaderText="Giornata" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Punti" HeaderText="Punti" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
