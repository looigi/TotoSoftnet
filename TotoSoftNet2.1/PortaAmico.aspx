<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="PortaAmico.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PortaAmico" %>

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
		<li><i class="icon-asterisk"></i><a href="#">Porta amico</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12">
        <div class="box span6" style="padding: 4px;">
            <asp:Label ID="lblPres" runat="server" Text="Mail presentate" CssClass ="label-medio rosso"></asp:Label>
            <hr />
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Presentati</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdUtenti" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered"
                        RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="EMail" HeaderText="E-Mail" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Registrato" HeaderText="Registrato" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div id="divModifica" runat="server" class="box span6" style="padding: 4px;">
            <asp:Label ID="lblTesto" runat="server" Text="Per ogni amico che presenterai e che si registrerà,<br />avrai uno sconto di 3 Euro sul totale dei pagamenti" CssClass ="label-medio"></asp:Label>
            <hr />
            <ul>
                <li>
                    <asp:Label ID="Label7" runat="server" Text="E-Mail Amico" CssClass ="label-medio rosso" Width="145px"></asp:Label>
                    <asp:TextBox ID="txtEMail" runat="server" CssClass ="label-medio" MaxLength="50"></asp:TextBox>
                </li>
            </ul>
            <hr />
            <asp:Button ID="cmdInvia" runat="server" Text="Invia Mail" CssClass ="btn btn-primary" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
