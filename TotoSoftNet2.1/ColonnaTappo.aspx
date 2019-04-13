<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ColonnaTappo.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ColonnaTappo" %>

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
		<li><i class="icon-trash"></i><a href="#">Compilazione colonna tappo</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="text-align: center; padding: 3px;">
        <asp:Button ID="cmdRandom" runat="server" Text="Genera Random" CssClass="btn btn-primary " />
        <asp:Button ID="cmdUsa" runat="server" Text="Usa" CssClass="btn btn-primary" />
        <asp:Button ID="cmdOk" runat="server" Text="Modifica" CssClass="btn btn-primary" />
    </div>

    <div class="box span6" style="margin-left: 0px; overflow: auto;">
	    <div id="divColonna" runat="server" class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Colonna tappo</h2>
			<div class="box-icon">
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered table-condensed "
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Partita" HeaderText="Partita" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Ris.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSegno" width="30" runat="server" MaxLength ="1" Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns> 
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
