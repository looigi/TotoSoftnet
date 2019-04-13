<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Resoconto.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Resoconto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><a href="#">Resoconto</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
        <div class="box span12">
            <asp:HiddenField ID="hdnGiornata" runat="server" />
            <div class="span4" style="text-align: left;">
                <asp:Button ID="cmdIndietroG" runat="server" Text="<<" CssClass ="btn btn-primary" />
            </div>
            <div class="span3" style="text-align: center;">
                <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-medio rosso" ></asp:Label>
            </div>
            <div class="span4" style="text-align: right;">
                <asp:Button ID="cmdAvantiG" runat="server" Text=">>" CssClass ="btn btn-primary" />
            </div>
            <div class="span12" style="text-align:center;">
                <asp:Label ID="Label1" runat="server" Text="Normale" CssClass ="label-medio" ></asp:Label>&nbsp;<asp:RadioButton ID="optNormale" runat="server" AutoPostBack="True" />&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Speciale" CssClass ="label-medio" ></asp:Label>&nbsp;<asp:RadioButton ID="optSpeciale" runat="server" AutoPostBack="True" />
            </div>
<%--    </div>

    <div class="row-fluid sortable ui-sortable" >				--%>
        <%--<div class="box span12">--%>
            <div id="divTesto" runat="server" style="text-align:center;">

            </div>
        <%--</div>--%>
         </div>
   </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
