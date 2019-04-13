<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="GestioneCoppe.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.GestioneCoppe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-user-md"></i>
			<a href="PulsantieraAmministrazione.aspx">Amministrazione</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Gestione Coppe</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divDettaglio" runat="server" class="box span8" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Giocatori qualificati</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdPercentuali" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Numero Utenti" >
                    </asp:BoundField>
                    <asp:BoundField DataField="CL" HeaderText="CL" >
                    </asp:BoundField>
                    <asp:BoundField DataField="PassCL" HeaderText="Pass. CL" >
                    </asp:BoundField>
                    <asp:BoundField DataField="EL" HeaderText="EL" >
                    </asp:BoundField>
                    <asp:BoundField DataField="PassEL" HeaderText="Pass. EL" >
                    </asp:BoundField>
                    <asp:BoundField DataField="IT" HeaderText="IT" >
                    </asp:BoundField>
                    <asp:BoundField DataField="PassIT" HeaderText="Pass. IT" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Der" HeaderText="Pippettero" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModifica" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" width="40" OnClick ="AggiornaPercentuale" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>

    <div id="divModifica" runat="server">
        <div id="div2" class="bloccafinestra" runat="server" Visible="True"></div>
        
        <div id="div3" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnUtente" runat="server" />
            <ul>
                <li>
                    <asp:Label ID="Label1" runat="server" Text="Numero Utenti" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtNumeroUtenti" runat="server" CssClass ="label-medio " Enabled ="false" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Qualif. CL" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtCL" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="Pass. CL" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtPassCL" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label4" runat="server" Text="Qualif. EL" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtEL" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px"  ></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="Pass. EL" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtPassEL" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label6" runat="server" Text="Qualif. IT" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtIT" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                    <asp:Label ID="Label7" runat="server" Text="Pass. IT" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtPassIT" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label8" runat="server" Text="Qualif. Pippettero" CssClass ="label-medio rosso"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtDer" runat="server" CssClass ="label-medio" MaxLength="2" Width="100px" ></asp:TextBox>
                </li>
            </ul>
            <hr />
            <asp:Button id="cmdOK" class="btn btn-primary " runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="btn btn-primary" runat="server" Text="Annulla" />

            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
