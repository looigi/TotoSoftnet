<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="Sondaggi.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.Sondaggi" %>

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
		<li><i class="icon-question-sign"></i><a href="#">Sondaggi</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span7" style="margin-left: 0px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Sondaggi</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdSondaggi" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="idSondaggio" HeaderText="N." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="TestoSondaggio" HeaderText="Sondaggio" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Risposta1" HeaderText="Risp. 1" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Risposta2" HeaderText="Risp. 2" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Risposta3" HeaderText="Risp. 3" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="DataInizio" HeaderText="Aper." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:BoundField DataField="DataFine" HeaderText="Chius." HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgMostra"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/icona_CERCA.Png" 
                                width="30px"
                                onclick="MostraStatisticheSondaggio" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgChiude"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/ChiusuraConcorso.Png"
                                width="30px"
                                onclick="ChiudeSondaggio" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>

            <asp:Button ID="cmdNuovo" runat="server" Text="Nuovo" CssClass="btn btn-primary" />
        </div>
    </div>

    <div id="divModifica" runat="server" class="box span5" style="margin-left: 2px;">
 	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Sondaggio</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:Image ID="imgStatSondaggio" runat="server" Width="300px" height="300px"/>

            <ul id="ulModifica" runat ="server">
                <li>
                    <asp:Label ID="Label2" runat="server" Text="Sondaggio" CssClass ="label-medio rosso" Width="102px"></asp:Label>
                    <asp:TextBox ID="txtSondaggio" runat="server" CssClass ="label-medio " MaxLength="1024" TextMode="MultiLine" Height="78px" Width="326px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label8" runat="server" Text="Risposta 1" CssClass ="label-medio rosso" Width="102px"></asp:Label>
                    <asp:TextBox ID="txtRisposta1" runat="server" CssClass ="label-medio " MaxLength="100" Width="326px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label3" runat="server" Text="Risposta 2" CssClass ="label-medio rosso" Width="102px"></asp:Label>
                    <asp:TextBox ID="txtRisposta2" runat="server" CssClass ="label-medio " MaxLength="100" Width="326px"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="Label4" runat="server" Text="Risposta 3" CssClass ="label-medio rosso" Width="102px"></asp:Label>
                    <asp:TextBox ID="txtRisposta3" runat="server" CssClass ="label-medio " MaxLength="100" Width="326px"></asp:TextBox>
                </li>
                <li>
                    <hr />
                </li>
                <li>
                    <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary " />
                    <asp:Button ID="cmdAnnulla" runat="server" Text="Annulla" CssClass="btn btn-primary" />
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
