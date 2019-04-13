<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ModificaUtente.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ModificaUtente" %>

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
		<li><i class="icon-bell"></i><a href="#">Modifica Utente</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span4">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Eventi</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdUtenti" runat="server" AutoGenerateColumns="False" 
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
        </div>
    </div>

    <div id="divModifica" runat="server" class="span8">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Eventi</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <div class="row-fluid">	
                <asp:Label ID="lblUtente" runat="server" Text="Label" CssClass ="label-titolo rosso"></asp:Label>
                <hr />
                <div class="span12">
                    <ul>
                        <li>
                            <asp:Label ID="Label7" runat="server" Text="Nick" CssClass ="label-medio rosso " Width="77px"></asp:Label>
                            <asp:TextBox ID="txtNick" runat="server" CssClass ="label-medio " MaxLength="20"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label ID="Label8" runat="server" Text="Password" CssClass ="label-medio rosso " Width="77px"></asp:Label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass ="label-medio " MaxLength="20"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label ID="Label3" runat="server" Text="Nome" CssClass ="label-medio rosso " Width="77px"></asp:Label>
                            <asp:TextBox ID="txtNome" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label ID="Label4" runat="server" Text="Cognome" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:TextBox ID="txtCognome" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label ID="Label5" runat="server" Text="E-Mail" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:TextBox ID="txtEMail" runat="server" CssClass ="label-medio " MaxLength="50"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Label ID="Label9" runat="server" Text="Controllo completo" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:CheckBox ID="chkControlloCompleto" runat="server" />
                        </li>
                        <li>
                            <asp:Label ID="Label6" runat="server" Text="Motto" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:TextBox ID="txtMotto" runat="server" CssClass ="label-medio" TextMode="MultiLine"></asp:TextBox>
                        </li>
                    </ul>
                </div>

                <div class="span12">
                    <ul>
                        <li>
                            <asp:Label ID="Label1" runat="server" Text="Ruolo" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:DropDownList ID="cmdPermessi" runat="server" CssClass ="label-medio" Width="217px"></asp:DropDownList>
                        </li>
                        <li>
                            <br />
                        </li>
                        <li>
                            <asp:Label ID="Label2" runat="server" Text="Pagante" CssClass ="label-medio rosso" Width="77px"></asp:Label>
                            <asp:DropDownList ID="cmbPagante" runat="server" CssClass ="label-medio" Width="217px"></asp:DropDownList>
                        </li>
                    </ul>
                </div>

                <div class="box span10" style="margin-top: 3px;">
                    <%--<asp:Label ID="Label9" runat="server" Text="Avatar" CssClass ="label-medio rosso"></asp:Label>--%>
                    <div class="span6" style="text-align:center;">
                        <asp:Image ID="imgAvatar" runat="server" Width="80" />
                    </div>
                    <div class="span6" style="text-align:center;">
                        <asp:Label ID="Label11" runat="server" Text="Avatar" CssClass ="label-medio rosso"></asp:Label><br />
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
                </div>

                <asp:HiddenField ID="hdnUtenteScelto" runat="server" />
<%--                <div class="box span10" style="margin-top: 3px; padding: 3px;">
                    <div class="span6" style="text-align:center;">
                        <asp:Image ID="imgSfondo" runat="server" Width="80" />
                    </div>
                    <div class="span6" style="text-align:center;">
                        <asp:Label ID="Label12" runat="server" Text="Sfondo" CssClass ="label-medio rosso"></asp:Label><br />
                        <asp:FileUpload ID="FileUpload2" runat="server" />
                    </div>
                </div>--%>

                <div class="span10" style="margin-top: 3px; padding: 3px;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass ="btn btn-primary" OnClick="cmdSalva_Click" />
                            <asp:Button ID="cmdEliminaColonna" runat="server" Text="Elimina Colonna" CssClass ="btn btn-primary " OnClick="cmdEliminaColonna_Click" />
                            <asp:Button ID="cmdElimina" runat="server" Text="Elimina" CssClass ="btn btn-primary " OnClick="cmdElimina_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="cmdSalva" />  
                            <asp:PostBackTrigger ControlID="cmdElimina" />  
                        </Triggers>
                    </asp:UpdatePanel> 
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
