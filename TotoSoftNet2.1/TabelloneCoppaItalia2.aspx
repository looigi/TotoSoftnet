<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="TabelloneCoppaItalia2.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.TabelloneCoppaItalia2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
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
		<li><i class="icon-glass"></i><a href="#">Coppa Italia</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divVincente" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px; ">
            <div class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincitore" CssClass ="label-medio"></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/CoppaItalia.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/CoppaItalia.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="label-medio"></asp:Label>
            </div>
        </div>
    </div>

    <div class="box span12" style="text-align:center; margin-left:0px;">
        <div class="span11" id="divContenuto" runat="server" style="text-align: center; overflow: auto; ">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentFooter" runat="server">
</asp:Content>
