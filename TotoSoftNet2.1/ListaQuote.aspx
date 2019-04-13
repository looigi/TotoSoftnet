<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="ListaQuote.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.ListaQuote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-star"></i>
			<a href="PulsantieraStatistiche.aspx">Statistiche</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-list"></i><a href="#">Lista Quote</a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div class="box span12" style="margin-left: 0px; ">
        <div class="box span12" style="float: left; overflow: auto; ">
	        <div id="div1" runat="server" class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdPartite" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-condensed "
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgSeleziona" runat="server" CssClass="ImmagineGriglia" ImageUrl ="App_Themes/Standard/Images/Icone/Freccetta.png" OnClick="SelezionaQuota" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Partita" HeaderText="P." ></asp:BoundField>
                        <asp:BoundField DataField="Casa" HeaderText="Casa" ></asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgCasa" runat="server" Class="ImmagineGrigliaMedia" Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Fuori" HeaderText="Fuori" ></asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFuori" runat="server" Class="ImmagineGrigliaMedia" Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="r1" HeaderText="1" ItemStyle-Height ="45px" ></asp:BoundField>
                        <asp:BoundField DataField="r1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="rX" HeaderText="X" ></asp:BoundField>
                        <asp:BoundField DataField="rXb" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2" HeaderText="2" ></asp:BoundField>
                        <asp:BoundField DataField="r2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1X" HeaderText="1X" ></asp:BoundField>
                        <asp:BoundField DataField="r1Xb" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r12" HeaderText="12" ></asp:BoundField>
                        <asp:BoundField DataField="r12b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="rX2" HeaderText="X2" ></asp:BoundField>
                        <asp:BoundField DataField="rX2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r0_0" HeaderText="0-0" ></asp:BoundField>
                        <asp:BoundField DataField="r0_0b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_1" HeaderText="0-1" ></asp:BoundField>
                        <asp:BoundField DataField="r0_1b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_2" HeaderText="0-2" ></asp:BoundField>
                        <asp:BoundField DataField="r0_2b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_3" HeaderText="0-3" ></asp:BoundField>
                        <asp:BoundField DataField="r0_3b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_4" HeaderText="0-4" ></asp:BoundField>
                        <asp:BoundField DataField="r0_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r1_0" HeaderText="1-0" ></asp:BoundField>
                        <asp:BoundField DataField="r1_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_1" HeaderText="1-1" ></asp:BoundField>
                        <asp:BoundField DataField="r1_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_2" HeaderText="1-2" ></asp:BoundField>
                        <asp:BoundField DataField="r1_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_3" HeaderText="1-3" ></asp:BoundField>
                        <asp:BoundField DataField="r1_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_4" HeaderText="1-4" ></asp:BoundField>
                        <asp:BoundField DataField="r1_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r2_0" HeaderText="2-0" ></asp:BoundField>
                        <asp:BoundField DataField="r2_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_1" HeaderText="2-1" ></asp:BoundField>
                        <asp:BoundField DataField="r2_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_2" HeaderText="2-2" ></asp:BoundField>
                        <asp:BoundField DataField="r2_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_3" HeaderText="2-3" ></asp:BoundField>
                        <asp:BoundField DataField="r2_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_4" HeaderText="2-4" ></asp:BoundField>
                        <asp:BoundField DataField="r2_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r3_0" HeaderText="3-0" ></asp:BoundField>
                        <asp:BoundField DataField="r3_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_1" HeaderText="3-1" ></asp:BoundField>
                        <asp:BoundField DataField="r3_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_2" HeaderText="3-2" ></asp:BoundField>
                        <asp:BoundField DataField="r3_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_3" HeaderText="3-3" ></asp:BoundField>
                        <asp:BoundField DataField="r3_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_4" HeaderText="3-4" ></asp:BoundField>
                        <asp:BoundField DataField="r3_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r4_0" HeaderText="4-0" ></asp:BoundField>
                        <asp:BoundField DataField="r4_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_1" HeaderText="4-1" ></asp:BoundField>
                        <asp:BoundField DataField="r4_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_2" HeaderText="4-2" ></asp:BoundField>
                        <asp:BoundField DataField="r4_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_3" HeaderText="4-3" ></asp:BoundField>
                        <asp:BoundField DataField="r4_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_4" HeaderText="4-4" ></asp:BoundField>
                        <asp:BoundField DataField="r4_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="rAltro" HeaderText="Altro" ></asp:BoundField>
                    </Columns> 
                </asp:GridView> 
            </div> 
        </div>
        <!-- <div class="box span7" style="float: left; overflow: auto; margin-left: 0px;">
	        <div id="divColonna" runat="server" class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Lista Quote</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <asp:GridView ID="grdListaQuote" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-condensed "
                    RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                    <Columns>
                        <asp:BoundField DataField="r1" HeaderText="1" ItemStyle-Height ="45px" ></asp:BoundField>
                        <asp:BoundField DataField="r1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="rX" HeaderText="X" ></asp:BoundField>
                        <asp:BoundField DataField="rXb" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2" HeaderText="2" ></asp:BoundField>
                        <asp:BoundField DataField="r2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1X" HeaderText="1X" ></asp:BoundField>
                        <asp:BoundField DataField="r1Xb" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r12" HeaderText="12" ></asp:BoundField>
                        <asp:BoundField DataField="r12b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="rX2" HeaderText="X2" ></asp:BoundField>
                        <asp:BoundField DataField="rX2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r0_0" HeaderText="0-0" ></asp:BoundField>
                        <asp:BoundField DataField="r0_0b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_1" HeaderText="0-1" ></asp:BoundField>
                        <asp:BoundField DataField="r0_1b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_2" HeaderText="0-2" ></asp:BoundField>
                        <asp:BoundField DataField="r0_2b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_3" HeaderText="0-3" ></asp:BoundField>
                        <asp:BoundField DataField="r0_3b" HeaderText=" "  ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r0_4" HeaderText="0-4" ></asp:BoundField>
                        <asp:BoundField DataField="r0_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r1_0" HeaderText="1-0" ></asp:BoundField>
                        <asp:BoundField DataField="r1_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_1" HeaderText="1-1" ></asp:BoundField>
                        <asp:BoundField DataField="r1_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_2" HeaderText="1-2" ></asp:BoundField>
                        <asp:BoundField DataField="r1_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_3" HeaderText="1-3" ></asp:BoundField>
                        <asp:BoundField DataField="r1_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r1_4" HeaderText="1-4" ></asp:BoundField>
                        <asp:BoundField DataField="r1_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r2_0" HeaderText="2-0" ></asp:BoundField>
                        <asp:BoundField DataField="r2_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_1" HeaderText="2-1" ></asp:BoundField>
                        <asp:BoundField DataField="r2_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_2" HeaderText="2-2" ></asp:BoundField>
                        <asp:BoundField DataField="r2_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_3" HeaderText="2-3" ></asp:BoundField>
                        <asp:BoundField DataField="r2_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r2_4" HeaderText="2-4" ></asp:BoundField>
                        <asp:BoundField DataField="r2_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r3_0" HeaderText="3-0" ></asp:BoundField>
                        <asp:BoundField DataField="r3_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_1" HeaderText="3-1" ></asp:BoundField>
                        <asp:BoundField DataField="r3_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_2" HeaderText="3-2" ></asp:BoundField>
                        <asp:BoundField DataField="r3_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_3" HeaderText="3-3" ></asp:BoundField>
                        <asp:BoundField DataField="r3_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r3_4" HeaderText="3-4" ></asp:BoundField>
                        <asp:BoundField DataField="r3_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="r4_0" HeaderText="4-0" ></asp:BoundField>
                        <asp:BoundField DataField="r4_0b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_1" HeaderText="4-1" ></asp:BoundField>
                        <asp:BoundField DataField="r4_1b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_2" HeaderText="4-2" ></asp:BoundField>
                        <asp:BoundField DataField="r4_2b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_3" HeaderText="4-3" ></asp:BoundField>
                        <asp:BoundField DataField="r4_3b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>
                        <asp:BoundField DataField="r4_4" HeaderText="4-4" ></asp:BoundField>
                        <asp:BoundField DataField="r4_4b" HeaderText=" " ItemStyle-BackColor ="#afa9a9" HeaderStyle-BackColor ="#afa9a9"></asp:BoundField>

                        <asp:BoundField DataField="rAltro" HeaderText="Altro" ></asp:BoundField>
                    </Columns> 
                </asp:GridView>
            </div>
        </div> -->
    </div>

    
    <div id="divVelo" runat ="server" class="bloccafinestra"></div>
    <div id="divModifica" runat="server" class="mascherinaavvisi" style="position: absolute; left: 50%; top: 40%; margin-left: -300px; width: 600px; margin-top: -300px; height: auto; max-height: 600px; overflow: auto; padding: 5px; text-align:center;z-index: 1000;">
        <div class="row-fluid sortable ui-sortable">	
            <div class="span12" style="margin-bottom:4px;">
                <div class="span9" style="text-align:center;">
                    <asp:ImageButton ID="imgCasa" runat="server" Enabled="False"  CssClass="ImmagineGriglia" />
                    <asp:Label ID="lblPartita" runat="server" Text="" CssClass ="label-medio rosso" ></asp:Label>
                    <asp:ImageButton ID="imgFuori" runat="server" Enabled="False"  CssClass="ImmagineGriglia" />
                </div>
                <div class="span3" style="text-align:right;">
                    <asp:ImageButton ID="imgChiudi"
                        runat="server" imageurl="App_Themes/Standard/Images/Icone/elimina_quadrato.Png" />
                </div>
            </div>
        </div>
        <div class="box span11">
	        <div class="box-header">
			    <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Modifica quote</h2>
			    <div class="box-icon">
				    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			    </div>
		    </div>
		    <div class="box-content">
                <ul>
                    <li>
                        <asp:Label ID="Label1" runat="server" Text="" CssClass ="label-medio" >1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt1" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="" CssClass ="label-medio" >X</asp:Label>&nbsp;
                        <asp:TextBox ID="txtX" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label3" runat="server" Text="" CssClass ="label-medio" >2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt2" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label4" runat="server" Text="" CssClass ="label-medio" >1X</asp:Label>&nbsp;
                        <asp:TextBox ID="txtS1X" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label5" runat="server" Text="" CssClass ="label-medio" >12</asp:Label>&nbsp;
                        <asp:TextBox ID="txtS12" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label6" runat="server" Text="" CssClass ="label-medio" >X2</asp:Label>&nbsp;
                        <asp:TextBox ID="txtSX2" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label7" runat="server" Text="" CssClass ="label-medio" >0-0</asp:Label>&nbsp;
                        <asp:TextBox ID="txt00" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label8" runat="server" Text="" CssClass ="label-medio" >0-1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt01" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label9" runat="server" Text="" CssClass ="label-medio" >0-2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt02" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label10" runat="server" Text="" CssClass ="label-medio" >0-3</asp:Label>&nbsp;
                        <asp:TextBox ID="txt03" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label11" runat="server" Text="" CssClass ="label-medio" >0-4</asp:Label>&nbsp;
                        <asp:TextBox ID="txt04" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label12" runat="server" Text="" CssClass ="label-medio" >1-0</asp:Label>&nbsp;
                        <asp:TextBox ID="txt10" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label13" runat="server" Text="" CssClass ="label-medio" >1-1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt11" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label14" runat="server" Text="" CssClass ="label-medio" >1-2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt12" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label15" runat="server" Text="" CssClass ="label-medio" >1-3</asp:Label>&nbsp;
                        <asp:TextBox ID="txt13" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label16" runat="server" Text="" CssClass ="label-medio" >1-4</asp:Label>&nbsp;
                        <asp:TextBox ID="txt14" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label17" runat="server" Text="" CssClass ="label-medio" >2-0</asp:Label>&nbsp;
                        <asp:TextBox ID="txt20" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label18" runat="server" Text="" CssClass ="label-medio" >2-1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt21" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label19" runat="server" Text="" CssClass ="label-medio" >2-2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt22" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label20" runat="server" Text="" CssClass ="label-medio" >2-3</asp:Label>&nbsp;
                        <asp:TextBox ID="txt23" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label21" runat="server" Text="" CssClass ="label-medio" >2-4</asp:Label>&nbsp;
                        <asp:TextBox ID="txt24" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label22" runat="server" Text="" CssClass ="label-medio" >3-0</asp:Label>&nbsp;
                        <asp:TextBox ID="txt30" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label23" runat="server" Text="" CssClass ="label-medio" >3-1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt31" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label24" runat="server" Text="" CssClass ="label-medio" >3-2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt32" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label25" runat="server" Text="" CssClass ="label-medio" >3-3</asp:Label>&nbsp;
                        <asp:TextBox ID="txt33" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label26" runat="server" Text="" CssClass ="label-medio" >3-4</asp:Label>&nbsp;
                        <asp:TextBox ID="txt34" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label27" runat="server" Text="" CssClass ="label-medio" >4-0</asp:Label>&nbsp;
                        <asp:TextBox ID="txt40" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label28" runat="server" Text="" CssClass ="label-medio" >4-1</asp:Label>&nbsp;
                        <asp:TextBox ID="txt41" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label29" runat="server" Text="" CssClass ="label-medio" >4-2</asp:Label>&nbsp;
                        <asp:TextBox ID="txt42" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label30" runat="server" Text="" CssClass ="label-medio" >4-3</asp:Label>&nbsp;
                        <asp:TextBox ID="txt43" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label31" runat="server" Text="" CssClass ="label-medio" >4-4</asp:Label>&nbsp;
                        <asp:TextBox ID="txt44" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                    <li style="margin-top: 4px;">
                        <asp:Label ID="Label32" runat="server" Text="" CssClass ="label-medio" >Altro</asp:Label>&nbsp;
                        <asp:TextBox ID="txtAltro" runat="server" CssClass="text" MaxLength="5" Width="50"></asp:TextBox>
                    </li>
                </ul>
                <asp:HiddenField ID="hdnPartita" runat="server" />
                <div id="divTasti" runat="server" class="span11" style="text-align: right; padding: 4px; ">
                    <asp:Button ID="cmdSalva" runat="server" Text="Salva" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
