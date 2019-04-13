<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Commenti.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Commenti" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-star"></i>
			<a href="PulsantieraStatistiche.aspx">Statistiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-reply"></i><a href="#">Commenti</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divCommento" runat="server" class="box span12">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Commenti</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <div style="width:15%; float: left; text-align:left;">
                <asp:Button ID="cmdIndietroA" runat="server" Text="<<" CssClass ="btn btn-primary  " />
            </div>
            <div style="width:67%; float: left; text-align:center;">
                <asp:Label ID="lblCommento" runat="server" Text="Commento" CssClass ="label-medio " ></asp:Label>
            </div>
            <div style="width:15%; float: left; text-align:right;">
                <asp:Button ID="cmdAvantiA" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary " />
            </div>
        </div>

        <div style="width:99%; overflow: auto;">
            <FTB:FreeTextBox id="FreeTextBox1" runat="Server"  Width="99%" Height="500px"
                ToolbarLayout=" ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu, 
                     FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline,
                     Strikethrough, Superscript, Subscript, CreateLink, Unlink, 
                     RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList, 
                     NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, 
                     ieSpellCheck, StyleMenu, SymbolsMenu, InsertHtmlMenu, InsertRule, InsertDate, 
                     InsertTime, WordClean, InsertImage, Preview, SelectAll, EditStyle" />
        </div>

        <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary " />
    </div>

    <div id="divCommenti" runat="server" class="box span12">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Commenti</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:HiddenField ID="hdnGiornata" runat="server" />
            <div style="width:15%; float: left; text-align:left;">
                <asp:Button ID="cmdIndietroG" runat="server" Text="<<" CssClass ="btn btn-primary  " />
            </div>
            <div style="width:67%; float: left; text-align:center;">
                <asp:Label ID="lblAttuale" runat="server" Text="Label" CssClass ="label-medio" ></asp:Label>
            </div>
            <div style="width:15%; float: left; text-align:right;">
                <asp:Button ID="cmdAvantiG" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary  " />
            </div>
        </div>
        <hr />

        <FTB:FreeTextBox id="FreeTextBox2" runat="Server"  Width="99%" Height="500px" EnableToolbars="False"></FTB:FreeTextBox>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
