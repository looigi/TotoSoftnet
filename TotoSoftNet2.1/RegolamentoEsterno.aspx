<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnIISM.Master" CodeBehind="RegolamentoEsterno.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.RegolamentoEsterno" %>

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
	<div class="container-fluid">
        <div id="divTasto" runat="server" class="span12" style="text-align: right; padding: 4px; ">
            <asp:Button ID="cmdRitorno" runat="server" Text="Uscita" CssClass="btn btn-primary " />
        </div>

        <div id="pdf" runat="server" class="row-fluid">				
            <div class="box span12" style="height: 80vh;">
                <object type="application/pdf" 
                    data="App_themes/Standard/Documenti/Regolamento.pdf?#zoom=85&scrollbar=1&toolbar=1&navpanes=0"
                    id="pdf_content" style="width: 100%; height: 99%">
                    <p>Problemi nella visualizzazione del documento PDF</p>
                </object>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
