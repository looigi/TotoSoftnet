<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="SupercoppaItaliana.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.SupercoppaItaliana" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-glass"></i>
			<a href="PulsantieraCoppe.aspx">Coppe</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Supercoppa Italiana</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
		<div class="box span12">
            <div class="box-content">
                <fieldset>
                    <div class="span12" style="text-align: center;">
                        <img src="App_Themes/Standard/Images/Icone/SupercoppaItaliana.png" width="50" />
                        <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-titolo"></asp:Label>
                        <img src="App_Themes/Standard/Images/Icone/SupercoppaItaliana.png" width="50" />
                    </div>
                </fieldset> 
            </div>
        </div>
    </div>

    <div class="row-fluid sortable ui-sortable">				
        <div class="span12">
            <div id="divApertoChiuso" runat="server" class="mascherinaavvisi">				
                <asp:Label ID="lblAvviso" runat="server" Text="La competizione non è ancora cominciata" CssClass ="label-titolo"></asp:Label>
            </div>
        </div>
    </div>

    <div class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px; margin-top: -55px;">
            <div id="divVincente" runat="server" class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincente" CssClass ="label-medio"></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/Vincitore.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/Vincitore.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="label-medio"></asp:Label>
            </div>
        </div>
    </div>

    <div id="divDettaglio" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="box span12">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Supercoppa Italiana</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="Partita"  HeaderText="Partita" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarC" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Casa"  HeaderText="Casa" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatarF" runat="server" width="80" Height="80" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="Fuori" HeaderText="Fuori" >
                        </asp:BoundField>
                        <asp:BoundField DataField="RisAndata" HeaderText="Risultato" HtmlEncode="False">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
