<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ClassificaRisultati.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ClassificaRisultati" %>

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
		<li><i class="icon-list"></i><a href="#">Classifica Risultati</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <asp:HiddenField ID="hdnGiornata" runat="server" />

    <div class="row-fluid sortable ui-sortable">				
       <div class="box span12" style="overflow: auto;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Risultati</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <div class="span12">
                    <div class="span4" style="text-align: left;">
                        <asp:Button ID="cmdIndietro" runat="server" Text="<<" CssClass ="btn btn-primary" />
                    </div>
                    <div class="span4" style="text-align: center;">
                        <asp:Label ID="lblGiornata" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                    </div>
                    <div class="span4" style="text-align: right;">
                        <asp:Button ID="cmdAvanti" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                    </div>
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
                        <asp:BoundField DataField="PuntiRis" HeaderText="Risultato" >
                        </asp:BoundField>
                        <asp:BoundField DataField="UltRis" HeaderText="Ult.Ris."  >
                        </asp:BoundField>
                        <asp:BoundField DataField="Precedente" HeaderText="Prec." >
                        </asp:BoundField>
                        <asp:BoundField DataField="Giocate" HeaderText="Giocate" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Media" HeaderText="Media" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Image ID="imgDifferenza" runat="server" CssClass="ImmagineGrigliaGrande"/>
                                <br />
                                <asp:Label ID="lblDiff" runat="server" Text="Label"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>            
                    </Columns>
                </asp:GridView>
             </div>  
        </div>
   </div>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>--%>
