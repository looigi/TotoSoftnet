<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="CambioUtente.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.CambioUtente" %>

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
		<li><i class="icon-user"></i><a href="#">Cambio utente</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="row-fluid sortable ui-sortable">				
       <div class="box span4">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Cambio utente</h2>
			    <div class="box-icon">
    <%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
    <%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdUtenti" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered"  PagerSettings-PageButtonCount="10" 
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="imgAvatar" runat="server" width="80" Height="80" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Utente" HeaderText="Utente" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgVai"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/Freccetta.Png" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
