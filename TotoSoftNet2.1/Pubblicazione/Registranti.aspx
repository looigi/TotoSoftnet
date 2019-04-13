<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Registranti.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Registranti" %>

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
		<li><i class="icon-star-half"></i><a href="#">Registranti</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Registranti</h2>
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
                    <asp:BoundField DataField="Giocatore" HeaderText="Giocatore" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Nome" HeaderText="Nome" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Cognome" HeaderText="Cognome" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="EMail" HeaderText="E-Mail" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgRegistra"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/Registranti.Png" width="50" OnClick="AggiungeGiocatore"  />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgElimina"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/Elimina.Png" width="50" OnClick="EliminaGiocatore" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
