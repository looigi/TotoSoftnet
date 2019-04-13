<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="TsnIISM.Master" CodeBehind="Default.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21._Default" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">

    <script type ="text/javascript">
        <!--
    function Accende(oggetto) {
        var oggettino = document.getElementById(oggetto);

        oggettino.style.opacity = .95; //For real browsers;
        oggettino.style.filter = "alpha(opacity=95)"; //For IE;
    }

    function Spegne(oggetto) {
        var oggettino = document.getElementById(oggetto);

        oggettino.style.opacity = .45; //For real browsers;
        oggettino.style.filter = "alpha(opacity=45)"; //For IE;
    }

    function ApreLibro(Numero) {
        myWindow = window.open("http://www.looigi.it/Libro.aspx?Libro=" + Numero, "Libro");
    }
    //-->
    </script>

</asp:Content>--%>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentMenuNavigazione" runat="server">
    <ul class="breadcrumb">
		<li>
			<i class="icon-home"></i>
			<a href="Principale.aspx">Home</a> 
			<i class="icon-angle-right"></i>
		</li>
		<li><a href="#"></a></li>
	</ul>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="contentCentrale" runat="server">
    <%--    <asp:Button ID="Button1" runat="server" Text="Button" />
--%>
<div class="container-fluid-full" >
    <div class="row-fluid" >
        <div class="login-box">
        <div class="control-group" title="Anno">
        <asp:Label ID="Label3" runat="server" Text="Anno" CssClass ="control-label"></asp:Label>
            <div class="controls">
                 <asp:DropDownList ID="cmbAnno" runat="server"  AutoPostBack="True"></asp:DropDownList>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="input-prepend" title="Username">
            <span class="add-on"><i class="halflings-icon user"></i></span>
            <asp:Label ID="Label1" runat="server" Text="Utente" CssClass ="etichettafissa" ForeColor="#FFFFFF"></asp:Label>
            <asp:TextBox ID="txtUtente" runat="server" CssClass ="input-large span10" placeholder="Username" ></asp:TextBox>
        </div>
        <div class="clearfix"></div>
        <div class="input-prepend" title="Password">
            <span class="add-on"><i class="halflings-icon lock"></i></span>
            <asp:Label ID="Label2" runat="server" Text="Password" CssClass ="etichettafissa" ForeColor="#FFFFFF"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server"  CssClass ="input-large span10" TextMode="Password" placeholder="Password"></asp:TextBox>
        </div>
        <div class="clearfix"></div>

        <%--<label class="remember" for="remember"><asp:CheckBox ID="chkRicordami" runat="server"  />Ricordami</label>--%>
		<div class="button-login">	        
            <asp:Button ID="cmdLogin" runat="server" Text="Login" CssClass ="btn btn-primary" />
            <asp:Button ID="cmdReg" runat="server" Text="Nuovo utente" CssClass ="btn btn-primary" />
            <asp:Button ID="cmdRicorda" runat="server" Text="Ricorda pwd" CssClass ="btn btn-primary" />
            <asp:Button ID="cmdRegolamento" runat="server" Text="Regolamento" CssClass ="btn btn-primary" />
        </div>
    </div>

    </div>
</div>
<%--    <div style="position: fixed; padding: 4px; left: 20px; bottom: 90px; ">
        <div class="mascherina draggable" style="float: left; opacity: .45;" id="libro1" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro1');" onmouseout="Spegne('ctl00_contentCentrale_libro1');">
            <a href="javascript:ApreLibro('1');" title="Storia di un viaggio coatto" ><img src="App_Themes/Standard/Images/Icone/Viaggio.jpg" width="100" height="120" /></a>
        </div>
        <div class="mascherina draggable" style="float: left; margin-left: 5px; opacity: .45;" id="libro2" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro2');" onmouseout="Spegne('ctl00_contentCentrale_libro2');">
            <a href="javascript:ApreLibro('2');" title="Storia di un campionato coatto" ><img src="App_Themes/Standard/Images/Icone/Sducc.png" width="100" height="120" /></a>
        </div>
        <div class="mascherina draggable" style="float: left; margin-left: 5px; opacity: .45;" id="libro3" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro3');" onmouseout="Spegne('ctl00_contentCentrale_libro3');">
            <a href="javascript:ApreLibro('3');" title="Storie coatte"><img src="App_Themes/Standard/Images/Icone/Rio2.jpg" width="100" height="120" /></a>
        </div>
        <div class="mascherina draggable" style="float: left; margin-left: 5px; opacity: .45;" id="libro4" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro4');" onmouseout="Spegne('ctl00_contentCentrale_libro4');">
            <a href="javascript:ApreLibro('4');" title="Storie coatte 2 - L'isola dei camici"><img src="App_Themes/Standard/Images/Icone/Cope2.jpg" width="100" height="120" /></a>
        </div>
        <div class="mascherina draggable" style="float: left; margin-left: 5px; opacity: .45;" id="libro5" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro5');" onmouseout="Spegne('ctl00_contentCentrale_libro5');">
            <a href="javascript:ApreLibro('5');" title="Mescola - Ultimo (co)atto"><img src="App_Themes/Standard/Images/Icone/Copertina2.png" width="100" height="120" /></a>
        </div>
        <div class="mascherina draggable" style="float: left; margin-left: 5px; opacity: .45;" id="libro6" runat ="server" onmouseover ="Accende('ctl00_contentCentrale_libro6');" onmouseout="Spegne('ctl00_contentCentrale_libro6');">
            <a href="javascript:ApreLibro('6');" title="Mescola - Ultimo (co)atto"><img src="App_Themes/Standard/Images/Icone/Casa.jpg" width="100" height="120" /></a>
        </div>
    </div>--%>

<%--<div style="position: fixed; top: 10px; right: 15px; z-index: 998; opacity:.87;">
        <div style="background: transparent url(App_Themes/Standard/Images/Icone/Pannello.png); width: 224px; height: 90px; background-size: 100% 100%;">
            <div style="width: 99%; display: block;">
                <div style="float:left; margin-top: 20px; margin-left: 20px; ">
                    <asp:Image ID="img1" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/1.png" Width="37px" Height="54px" />
                </div>
                <div style="float:left; margin-top: 20px; ">
                    <asp:Image ID="img2" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/1.png" Width="37px" Height="54px" />
                </div>
                <div style="float:left; margin-top: 20px; ">
                    <asp:Image ID="img3" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/1.png" Width="37px" Height="54px" />
                </div>
                <div style="float:left; margin-top: 20px; ">
                    <asp:Image ID="img4" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/1.png" Width="37px" Height="54px" />
                </div>
                <div style="float:left; margin-top: 20px; ">
                    <asp:Image ID="img5" runat="server" ImageUrl ="App_Themes/Standard/Images/Icone/1.png" Width="37px" Height="54px" />
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
