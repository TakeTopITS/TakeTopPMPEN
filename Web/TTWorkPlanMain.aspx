<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWorkPlanMain.aspx.cs" Inherits="TTWorkPlanMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

        });
    </script>

</head>
<frameset id="TakeTopMDI" rows="*,1" cols="*">
   <frameset id="bodyFrame" cols="50,*" frameborder="yes" border="1" framespacing="0" class="bian">
     <frame name="Left" src="TTWorkPlan.aspx?ProjectID=<%=Request.QueryString["ProjectID"].ToString()%>">   
     <frame name="Right" src="TTWorkPlanGanttForProject.aspx?pid=<%=Request.QueryString["ProjectID"].ToString()%>"> 
   </frameset> 
    <frame name="bottom" src="TTWorkPlanGanttForStandardProjectPlan.aspx?pid=<%=Request.QueryString["ProjectID"].ToString()%>"> 
</frameset>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
