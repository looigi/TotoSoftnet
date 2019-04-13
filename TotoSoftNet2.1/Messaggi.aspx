<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Messaggi.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Messaggi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-comments-alt"></i><a href="#">Messaggi</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span4">
            <div id="divDaScegliere" runat="server">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Da Inserire</h2>
			        <div class="box-icon">
        <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
        <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdUtentiTutti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Utente" HeaderText="Utente" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVai"
                                        runat="server" imageurl="App_Themes/Standard/Images/Icone/Freccetta.Png" OnCliCk="SelezionaUtente" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                        </Columns>
                    </asp:GridView>

                    <asp:Button ID="cmdTutti" runat="server" Text="Tutti" CssClass ="btn btn-primary" />
                </div>
            </div>
        </div>

        <div class="box span4">
            <div id="divScelti" runat="server">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Inseriti</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grdUtentiScelti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVai"
                                        runat="server" imageurl="App_Themes/Standard/Images/Icone/FreccettaIndietro.Png" OnCliCk="DeSelezionaUtente" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                            <asp:BoundField DataField="Utente" HeaderText="Utente" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                                </ItemTemplate>
                            </asp:TemplateField>            
                        </Columns>
                    </asp:GridView>
            
                    <asp:Button ID="cmdNessuno" runat="server" Text="Nessuno" CssClass ="btn btn-primary" />
                </div>
            </div>
        </div>

        <div class="box span4">
            <div id="divMessaggio" runat="server" >
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Messaggio</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:TextBox ID="txtMessaggio" runat="server" CssClass ="label-medio" Height="259px" TextMode="MultiLine" Width="97%"></asp:TextBox>
                    <hr />
                    <asp:Button ID="cmdInvia" runat="server" Text="Invia" CssClass ="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
