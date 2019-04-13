<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="CampionatiAppartenenza.aspx.vb" Inherits="TotoSoftNet2.CampionatiAppartenenza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="App_Themes/Standard/Griglie.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBarraTasti" runat="server">
    <%--<asp:Button ID="cmdIndietro" runat="server" Text="Indietro" CssClass="bottone" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div style="width: 49%; float: left;">
        <div class="mascherina draggable" style="text-align:center;">
            <asp:GridView ID="grdSquadre" runat="server" AutoGenerateColumns="False" 
                CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" 
                    PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" Width="95%" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="Squadra" HeaderText="Squadra" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        <HeaderStyle CssClass="cella-testata-griglia-grande" />
                        <ItemStyle CssClass="cella-elemento-griglia-grande" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Campionato" HeaderText="Campionato" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModificaNome"
                                runat="server" imageurl="App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.Png" ToolTip ="Modifica Nome Squadra"
                                OnClick ="ModificaNomeSquadra" EnableViewState="False" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
             
            <table id="tabella" runat ="server" width="99%" class ="login">
                <tr>
                    <td width="45%">
                        <asp:HiddenField ID="hdnSquadra" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text="Squadra" CssClass="etichettatitoloaccount " Width="80px"></asp:Label>
                        <asp:TextBox ID="txtSquadra" runat="server" CssClass="casellatesto " Width="200px"></asp:TextBox>
                    </td>
                    <td width="45%">
                        <asp:Label ID="Label2" runat="server" Text="Campionato" CssClass="etichettatitoloaccount" Width="80px"></asp:Label>
                        <asp:DropDownList ID="cmbSerie" runat="server" CssClass="combomedia" Width="200px"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="bottone"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="width: 49%; float: left;">
        <div class="mascherina draggable" style="text-align:center;">
            <asp:GridView ID="grdSerie" runat="server" AutoGenerateColumns="False" 
                CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" 
                    PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" Width="95%" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="Serie" HeaderText="Serie" HeaderStyle-Font-Names="Verdana" HeaderStyle-Font-Size="Small" >
                        <HeaderStyle CssClass="cella-testata-griglia-grande" />
                        <ItemStyle CssClass="cella-elemento-griglia-grande" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgModificaNomeSerie"
                                runat="server" imageurl="App_Themes/standard/images/Icone/icona_MODIFICA-TAG.Png" ToolTip ="Modifica Nome Squadra"
                                OnClick ="ModificaNomeSerie" EnableViewState="False" />
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
             
            <asp:Button ID="cmdNuovaSerie" runat="server" Text="Nuova" CssClass="bottone"/>
            <br />
            <table id="tabella2" runat ="server" width="99%" class ="login">
                <tr>
                    <td width="90%">
                        <asp:HiddenField ID="hdnSerie" runat="server" />
                        <asp:Label ID="Label3" runat="server" Text="Serie" CssClass="etichettatitoloaccount " Width="80px"></asp:Label>
                        <asp:TextBox ID="txtSerie" runat="server" CssClass="casellatesto" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="cmdSalvaS" runat="server" Text="Salva" CssClass="bottone"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="contentFooter" runat="server">
</asp:Content>
