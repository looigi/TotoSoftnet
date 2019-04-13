<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="RiapreConcorso.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.RiapreConcorso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
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
		<li><i class="icon-key"></i><a href="#">Riapre concorso</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12" style="text-align: center;">
        <asp:Button ID="cmdRiapre" runat="server" Text="Riapre concorso" CssClass="btn btn-primary " />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
