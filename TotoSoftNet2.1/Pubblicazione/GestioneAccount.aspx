<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="GestioneAccount.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.GestioneAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-user"></i>
			<a href="PulsantieraPersonale.aspx">Personale</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Gestione dati personali</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12">
	    <div class="box-header" data-original-title>
		    <h2><i class="halflings-icon white edit"></i><span class="break"></span>Dati Account</h2>
        </div>
        <div class="box-content">
<%--		    <form class="form-horizontal">--%>
			<fieldset>
				<div class="control-group">
                        <asp:Label ID="Label2" runat="server" Text="Nick" CssClass ="control-label"  Width="102px"></asp:Label>
				    <div class="controls">                
                        <asp:TextBox ID="txtNick" runat="server" CssClass ="input-xlarge focused" MaxLength="50"></asp:TextBox>
				    </div>
				</div>
                <div class="control-group">
                        <asp:Label ID="Label8" runat="server" Text="Password" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:TextBox ID="txtPassword" runat="server" CssClass ="input-xlarge focused" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label3" runat="server" Text="Nome" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:TextBox ID="txtNome" runat="server" CssClass ="input-xlarge focused" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label4" runat="server" Text="Cognome" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:TextBox ID="txtCognome" runat="server" CssClass ="input-xlarge focused" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label5" runat="server" Text="E-Mail" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:TextBox ID="txtEMail" runat="server" CssClass ="input-xlarge focused" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label7" runat="server" Text="Controllo schedina completo" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls" style="margin-top: 10px;">
                        <asp:CheckBox ID="chkControlloCompleto" runat="server" />  
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label6" runat="server" Text="Motto" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:TextBox ID="txtMotto" runat="server" CssClass ="input-xlarge focused" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label1" runat="server" Text="Notifiche" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:CheckBox ID="chkNotifiche" runat="server" AutoPostBack="True" />
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label9" runat="server" Text="Avatar" CssClass ="control-label" Width="102px"></asp:Label>
                    <div class="controls">   
                        <asp:FileUpload ID="FileUpload3" runat="server" />
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label ID="Label10" runat="server" Text="Sfondo" CssClass ="control-label" Width="102px" Visible="False"></asp:Label>
                    <div class="controls">   
                        <asp:FileUpload ID="FileUpload2" runat="server" Visible="False" />
                    </div>
                </div>
                <div style=" padding: 19px 20px 20px; background-color: #EEE;    border-top: 1px solid #eee; ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary" OnClick="cmdSalva_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="cmdSalva" />  
                        </Triggers>
                    </asp:UpdatePanel> 
				</div>
            </fieldset>
        </div>
    </div>
<%--    </div>--%>
</asp:Content>
