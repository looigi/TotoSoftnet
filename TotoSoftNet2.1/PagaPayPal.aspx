<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PagaPayPal.aspx.vb" MaintainScrollPositionOnPostBack = "true" Inherits="TotoSoftNet21.PagaPayPal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="payForm" method="post" action="<%Response.Write (URL)%>">
        <input type="hidden" name="cmd" value="<%Response.Write (cmd)%>">
        <input type="hidden" name="business" value="<%Response.Write (business)%>">
        <input type="hidden" name="item_name" value="<%Response.Write (item_name)%>">
        <input type="hidden" name="amount" value="<%Response.Write (amount)%>">
        <input type="hidden" name="no_shipping" value="<%Response.Write (no_shipping)%>">
        <input type="hidden" name="return" value="<%Response.Write (return_url)%>">
        <input type="hidden" name="rm" value="<%Response.Write (rm)%>">
        <input type="hidden" name="notify_url" value="<%Response.Write (notify_url)%>">
        <input type="hidden" name="cancel_return" value="<%Response.Write (cancel_url)%>">
        <input type="hidden" name="currency_code" value="<%Response.Write (currency_code)%>">
        <input type="hidden" name="custom" value="<%Response.Write (request_id)%>">
    </form>

    <script language="javascript">
        document.forms["payForm"].submit();
    </script>    
</body>
</html>
