<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnII.Master" CodeBehind="QuoteCUP.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.QuoteCUP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
    <style type="text/css">
        #ctl00_contentCentrale_Menu1 {
            width:99%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li>
            <i class="icon-glass"></i>
			<a href="PulsantieraCoppe.aspx">Coppe</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><i class="icon-glass"></i><a href="#">Quote CUP</a></li>
	</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentCentrale" runat="server">
    <div id="divVincente" runat="server" class="row-fluid sortable ui-sortable">				
        <div class="span12" style="margin-left: 1px; ">
            <div class="mascherinaavvisi">
                <asp:Label ID="Label1" runat="server" Text="Vincitore" CssClass ="label-medio"></asp:Label>
                <img src="App_Themes/Standard/Images/Icone/quotecup.png" width="60" height="60" />
                <asp:Image ID="imgVincente" runat="server" Width ="80px" />
                <img src="App_Themes/Standard/Images/Icone/quotecup.png" width="60" height="60" />
                <asp:Label ID="lblVincitore" runat="server" Text="Label" CssClass ="label-medio"></asp:Label>
            </div>
        </div>
    </div>    
    
    <div class="row-fluid sortable ui-sortable">				
		<div class="span12">
            <div class="box-content">
                <fieldset>
                    <asp:Menu
                        ID="Menu1"
                        runat="server"
                        RenderingMode="Table"
                        Orientation="Horizontal"
                        StaticMenuItemStyle-CssClass="MenuItem"
                        StaticSelectedStyle-CssClass="MenuItem_selected"
                        StaticEnableDefaultPopOutImage="False"
                        OnMenuItemClick="Menu1_MenuItemClick">
                        <Items>
                            <asp:MenuItem Text="Partite proprie " Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Gruppo 1 - Girone A " Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Gruppo 1 - Girone B " Value="2"></asp:MenuItem>
                            <asp:MenuItem Text="Gruppo 2 - Girone C " Value="3"></asp:MenuItem>
                            <asp:MenuItem Text="Gruppo 2 - Girone D " Value="4"></asp:MenuItem>
                            <asp:MenuItem Text="Classifiche " Value="5"></asp:MenuItem>
                            <asp:MenuItem Text="Scontri diretti " Value="6"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </fieldset> 
            </div>
        </div>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="TabProprie" runat="server"  >
            <div id="div8" runat="server" class="box span12" style="margin-left: 0px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite proprie</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
		            <div class="box-content">
                        <div style="width:20%; float: left; text-align:center;">
                            <asp:Label ID="Label2" runat="server" Text="Giocatore" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                        <div style="width:48%; float: left; text-align:center;">
                            <asp:DropDownList ID="cmbGiocatori" CssClass="label-medio" AutoPostBack="True" runat="server"></asp:DropDownList>
                        </div>
                        <div style="width:30%; float: left; text-align:center;">
                            <asp:Label ID="lblGirone" runat="server" Text="Giocatore" CssClass ="label-medio rosso" ></asp:Label>
                        </div>

                        <div class="clearALL "></div>
                    </div>

                    <asp:GridView ID="grdProprie" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Giornata"  HeaderText="Giornata" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarC" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraC"  HeaderText="Casa" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarF" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraF"  HeaderText="Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Ris1"  HeaderText="Ris. Casa" ></asp:BoundField>
                            <asp:BoundField DataField="Ris2"  HeaderText="Ris. Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Segno"  HeaderText="Segno" ></asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Punti" ></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Tab1A" runat="server"  >
            <div id="divDettaglio" runat="server" class="box span5" style="margin-left: 0px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Gruppo 1 - Girone A</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grd1A" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                            <asp:BoundField DataField="Provenienza" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="div4" runat="server" class="box span7" style="margin-left: 3px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite Gruppo 1 - Girone A</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
		            <div class="box-content">
                        <div style="width:15%; float: left; text-align: left;">
                            <asp:Button ID="cmdIndietro1A" runat="server" Text="<<" CssClass ="btn btn-primary" />
                        </div>
                        <div style="width:70%; float: left; text-align:center;">
          
                            <asp:Label ID="lblGiornata1A" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                        <div style="width:15%; float: right; text-align:right;">
                            <asp:Button ID="cmdAvanti1A" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                        </div>
                        <asp:HiddenField ID="hdnGiornata1A" runat="server" />
                        <div class="clearALL "></div>
                    </div>
                    <asp:GridView ID="grdP1A" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarC" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraC"  HeaderText="Casa" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarF" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraF"  HeaderText="Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Ris1"  HeaderText="Ris. Casa" ></asp:BoundField>
                            <asp:BoundField DataField="Ris2"  HeaderText="Ris. Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Segno" ></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Tab1B" runat="server"  >
            <div id="div1" runat="server" class="box span5" style="margin-left: 0px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Gruppo 1 - Girone B</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grd1B" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                            <asp:BoundField DataField="Provenienza" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="div5" runat="server" class="box span7" style="margin-left: 3px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite Gruppo 1 - Girone B</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
		            <div class="box-content">
                        <div style="width:15%; float: left; text-align: left;">
                            <asp:Button ID="cmdIndietro1B" runat="server" Text="<<" CssClass ="btn btn-primary" />
                        </div>
                        <div style="width:70%; float: left; text-align:center;">
          
                            <asp:Label ID="lblGiornata1B" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                        <div style="width:15%; float: right; text-align:right;">
                            <asp:Button ID="cmdAvanti1B" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                        </div>
                        <asp:HiddenField ID="hdnGiornata1B" runat="server" />
                        <div class="clearALL "></div>
                    </div>
                    <asp:GridView ID="grdP1B" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarC" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraC"  HeaderText="Casa" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarF" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraF"  HeaderText="Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Ris1"  HeaderText="Ris. Casa" ></asp:BoundField>
                            <asp:BoundField DataField="Ris2"  HeaderText="Ris. Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Segno" ></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Tab2C" runat="server"  >
            <div id="div2" runat="server" class="box span5" style="margin-left: 0px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Gruppo 2 - Girone C</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grd2C" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                            <asp:BoundField DataField="Provenienza" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="div6" runat="server" class="box span7" style="margin-left: 3px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite Gruppo 2 - Girone C</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
		            <div class="box-content">
                        <div style="width:15%; float: left; text-align: left;">
                            <asp:Button ID="cmdIndietro2C" runat="server" Text="<<" CssClass ="btn btn-primary" />
                        </div>
                        <div style="width:70%; float: left; text-align:center;">
          
                            <asp:Label ID="lblGiornata2C" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                        <div style="width:15%; float: right; text-align:right;">
                            <asp:Button ID="cmdAvanti2C" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                        </div>
                        <asp:HiddenField ID="hdnGiornata2C" runat="server" />
                        <div class="clearALL "></div>
                    </div>
                    <asp:GridView ID="grdP2C" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarC" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraC"  HeaderText="Casa" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarF" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraF"  HeaderText="Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Ris1"  HeaderText="Ris. Casa" ></asp:BoundField>
                            <asp:BoundField DataField="Ris2"  HeaderText="Ris. Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Segno" ></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Tab2D" runat="server"  >
            <div id="div3" runat="server" class="box span5" style="margin-left: 0px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Gruppo 2 - Girone D</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
                    <asp:GridView ID="grd2D" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                            <asp:BoundField DataField="Provenienza" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="div7" runat="server" class="box span7" style="margin-left: 3px; overflow: auto;">
	            <div class="box-header">
			        <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Partite Gruppo 2 - Girone D</h2>
			        <div class="box-icon">
				        <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			        </div>
		        </div>
		        <div class="box-content">
		            <div class="box-content">
                        <div style="width:15%; float: left; text-align: left;">
                            <asp:Button ID="cmdIndietro2D" runat="server" Text="<<" CssClass ="btn btn-primary" />
                        </div>
                        <div style="width:70%; float: left; text-align:center;">
          
                            <asp:Label ID="lblGiornata2D" runat="server" Text="Commento" CssClass ="label-medio rosso" ></asp:Label>
                        </div>
                        <div style="width:15%; float: right; text-align:right;">
                            <asp:Button ID="cmdAvanti2D" runat="server" Text="&gt;&gt;" CssClass ="btn btn-primary" />
                        </div>
                        <asp:HiddenField ID="hdnGiornata2D" runat="server" />
                        <div class="clearALL "></div>
                    </div>
                    <asp:GridView ID="grdP2D" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-bordered"
                            RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarC" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraC"  HeaderText="Casa" ></asp:BoundField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatarF" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                            <asp:BoundField DataField="SquadraF"  HeaderText="Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Ris1"  HeaderText="Ris. Casa" ></asp:BoundField>
                            <asp:BoundField DataField="Ris2"  HeaderText="Ris. Fuori" ></asp:BoundField>
                            <asp:BoundField DataField="Punti"  HeaderText="Segno" ></asp:BoundField>
                       </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="TabClassifiche" runat="server"  >
            <div class="span12" style="margin-left: 0px;">
                <div id="div9" runat="server" class="box span6" style="margin-left: 0px; overflow: auto; float: left; margin-right: 3px;">
	                <div class="box-header">
			            <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Gruppo 1</h2>
			            <div class="box-icon">
				            <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			            </div>
		            </div>
		            <div class="box-content">
                        <asp:GridView ID="grdClassGruppo1" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered"
                                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                            <Columns>
                                <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                                <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                                <asp:BoundField DataField="Punti" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                                <asp:BoundField DataField="PuntiTot" HeaderText="Punti Tot" HtmlEncode="False"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <div id="div10" runat="server" class="box span6" style="margin-left: 0px; overflow: auto; float: left;">
	                <div class="box-header">
			            <h2><i class="halflings-icon white align-justify"></i><span class="break"></span>Classifica Gruppo 2</h2>
			            <div class="box-icon">
				            <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
			            </div>
		            </div>
		            <div class="box-content">
                        <asp:GridView ID="grdClassGruppo2" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered"
                                RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia">
                            <Columns>
                                <asp:BoundField DataField="Posizione"  HeaderText="Pos." ></asp:BoundField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Image ID="imgAvatar" runat="server" width="60" Height="60" /></ItemTemplate></asp:TemplateField>            
                                <asp:BoundField DataField="Squadra"  HeaderText="Giocatore" ></asp:BoundField>
                                <asp:BoundField DataField="Punti" HeaderText="Punti" HtmlEncode="False"></asp:BoundField>
                                <asp:BoundField DataField="PuntiTot" HeaderText="Punti Tot" HtmlEncode="False"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="clear clearALL "></div>
        </asp:View> 
        <asp:View ID="TabSD" runat="server"  >
            <div class="box span12" style="text-align:center; margin-left: 0px;">
                <div class="span11" id="divScontriDiretti" runat="server" style="text-align: center; overflow: auto; ">
                </div>
            </div>
        </asp:View>
    </asp:MultiView>   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentFooter" runat="server">
</asp:Content>
