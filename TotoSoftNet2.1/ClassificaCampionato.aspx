<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ClassificaCampionato.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ClassificaCampionato" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
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
		<li><i class="icon-list"></i><a href="#">Classifica Campionato</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span7" style="overflow: auto;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Campionato</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>

            <div class="span12" >
                <div class="span4" style="text-align: left;">
                    <asp:Button ID="cmdIndietro" runat="server" Text="<<" CssClass ="btn btn-primary" />
                </div>
                <div class="span3" style="text-align: center;">
                    <asp:Label ID="lblGiornata" runat="server" Text="Label" CssClass ="label-medio rosso" ></asp:Label>
                    </div>
                <div class="span4" style="text-align: right;">
                    <asp:Button ID="cmdAvanti" runat="server" Text=">>" CssClass ="btn btn-primary" />
                </div>
            </div>

            <asp:HiddenField ID="hdnGiorn" runat="server" />

            <asp:GridView ID="grdClassifica" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"  PagerSettings-PageButtonCount="10" 
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Posizione"  HeaderText="Pos." >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="imgAvatar" runat="server" CssClass="ImmagineGrigliaGrande"/>
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Giocatore" HeaderText="Gioc." >
                    </asp:BoundField>
                    <asp:BoundField DataField="Punti" HeaderText="Punti" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Giocate" HeaderText="Gioc." >
                    </asp:BoundField>
                    <asp:BoundField DataField="Media" HeaderText="Media" >
                    </asp:BoundField>
                    <asp:BoundField DataField="GoalFatti" HeaderText="G.F." >
                    </asp:BoundField>
                    <asp:BoundField DataField="GoalSubiti" HeaderText="G.S." >
                    </asp:BoundField>
                    <asp:BoundField DataField="DifferenzaGoal" HeaderText="Diff." >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Image ID="imgDifferenza" runat="server" width="40" Height="40" />
                            <br />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
        </div>

        <div class="box span5" style="overflow: auto;">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Scontri</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:HiddenField ID="hdnGiornata" runat="server" />    
                <asp:HiddenField ID="hdnMaxGiornate" runat="server" />    
                <asp:HiddenField ID="hdnMinGiornate" runat="server" />    

                <div class="span12" >
                    <div class="span4" style="text-align: left;">
                        <asp:Button ID="cmdGIndietro" runat="server" Text="<<" CssClass ="btn btn-primary" />
                    </div>
                    <div class="span3" style="text-align: center;">
                        <asp:Label ID="lblGiornataVis" runat="server" Text="Label" CssClass ="label-medio rosso" ></asp:Label>
                     </div>
                   <div class="span4" style="text-align: right;">
                        <asp:Button ID="cmdGAvanti" runat="server" Text=">>" CssClass ="btn btn-primary" />
                    </div>
                </div>

                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"  PagerSettings-PageButtonCount="10" 
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarC" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="GiocatoreC" HeaderText="Casa" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarF" runat="server" CssClass="ImmagineGrigliaGrande"/>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="GiocatoreF" HeaderText="Fuori" >
                        </asp:BoundField>
                        <asp:BoundField DataField="RisultatoU" HeaderText="Ris. Camp." >
                        </asp:BoundField>
                        <asp:BoundField DataField="RisultatoR" HeaderText="Ris. Goal" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
