<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="LetturaMessaggi.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.LetturaMessaggi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-comments-alt"></i><a href="#">Lettura Messaggi</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="span12" style="margin-left: 0px;">
        <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Messaggio</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content" style="height: 300px; ">
            <div id="divTesto" runat ="server">
                <div style="width:15%; text-align:center; float: left;">
                    <asp:Label ID="lblUtente" runat="server" Text="" CssClass ="label-titolo rosso "></asp:Label>
                    <hr />
                    <asp:Image ID="imgAvatarM" runat="server" width="80" Height="80" />
                </div>
                <div style="width:78%; text-align:center; float: left; padding: 3px;">
                    <asp:TextBox ID="txtMessaggio" runat="server" Width="99%" Height ="290px" TextMode="MultiLine" CssClass ="label-medio"></asp:TextBox>
                </div>
                <div style="width:2%; text-align:center; float: left; ">
                    <asp:ImageButton ID="imgRispondi"
                        runat="server" imageurl="App_Themes/Standard/Images/Icone/ScritturaMessaggi.Png" 
                        ToolTip ="Rispondi" CssClass="ImmagineGrigliaGrande"/>
                </div>
            </div>
        </div>
    </div>

    <div class="span12" style="margin-left: 0px; margin-top: 2px;">
	    <div class="box-header">
			<h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Lista Messaggi</h2>
			<div class="box-icon">
<%--				<a href="#" class="btn-setting"><i class="halflings-icon white wrench"></i></a>--%>
				<a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
<%--				<a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>--%>
			</div>
		</div>
		<div class="box-content">
            <asp:GridView ID="grdMessaggi" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered"
                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                <Columns>
                    <asp:BoundField DataField="Progressivo" HeaderText="Progressivo" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="imgAvatarM" runat="server" CssClass="ImmagineGrigliaGrande"/>
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:BoundField DataField="Mittente" HeaderText="Mittente" >
                    </asp:BoundField>
                    <asp:BoundField DataField="DataInvio" HeaderText="DataInvio" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Testo" HeaderText="Testo" >
                    </asp:BoundField>
                    <asp:BoundField DataField="Letto" HeaderText="Letto" >
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgLeggi"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/visualizzato_tondo.Png" 
                                ToolTip ="Visualizza il messaggio" Width="35" Height="35"
                                OnClick ="VisualizzaMessaggio" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgRispondi"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/ScritturaMessaggi.Png" 
                                ToolTip ="Rispondi" Width="35" Height="35"
                                OnClick="RispondiMessaggio" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgElimina"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/Elimina.Png" 
                                ToolTip ="Elimina" Width="35" Height="35"
                                OnClick ="EliminaMessaggio" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div id="divRispondi" runat="server">
        <div id="divBloccaFinestra2" class="bloccafinestra" runat="server" Visible="True"></div>

        <div id="divPopup2" class="popuptesto draggable" runat="server" Visible="true">
            <asp:HiddenField ID="hdnProgressivo" runat="server" />
            <asp:Label ID="lblCampo" runat="server" Text="Risposta" CssClass ="label-medio rosso"></asp:Label>
            &nbsp;<asp:TextBox ID="txtCampo" runat="server" TextMode="MultiLine" CssClass ="label-medio " Height="117px" Width="357px"></asp:TextBox>
            <hr />
            <asp:Button id="cmdOK" class="btn btn-primary " runat="server" Text="OK" />
            <asp:Button id="cmdAnnulla" class="btn btn-primary" runat="server" Text="Annulla" />

            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
