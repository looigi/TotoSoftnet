<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Regolamento.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Regolamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><a href="#">Regolamento</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
<%--    <asp:Button ID="cmdIndietro" runat="server" Text="Indietro" CssClass="bottone" />--%>
    <div id="pdf" runat="server" class="row-fluid">				
        <div class="box span12" style="height: 80vh;">
            <object type="application/pdf" 
                data="App_themes/Standard/Documenti/Regolamento.pdf?#zoom=85&scrollbar=1&toolbar=1&navpanes=0"
                id="pdf_content" style="width: 100%; height: 99%">
                <p>Problemi nella visualizzazione del documento PDF</p>
            </object>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
