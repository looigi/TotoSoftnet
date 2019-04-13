<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ClassificaSpeciale.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ClassificaSpeciale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-list-ol"></i>
			<a href="PulsantieraClassifiche.aspx">Classifiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-list"></i><a href="#">Classifica Speciali</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <asp:HiddenField ID="hdnGiornata" runat="server" />

    <div class="row-fluid sortable ui-sortable">				
       <div class="box span12" style="overflow: auto;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Speciale</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <div class="span12" style="text-align: center;">
                    <asp:Label ID="lblGiornata" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                </div>

                <asp:GridView ID="grdClassifica" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"  PagerSettings-PageButtonCount="10" 
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Posizione" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" >
                            <ItemStyle CssClass="center"  />
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Giocatore"  HeaderText="Gioc." >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Label ID="lblMotto" runat="server" Text="Label" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="PuntiRis" HeaderText="Punti" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
             </div>  
        </div>
   </div>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>--%>
