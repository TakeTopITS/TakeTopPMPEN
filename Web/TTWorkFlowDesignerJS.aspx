﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWorkFlowDesignerJS.aspx.cs" Inherits="TTWorkFlowDesignerJS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>TakeTopWF.Designer</title>
    <style type="text/css">
        #AboveDiv {
            min-width: 2300px;
            width: expression (document.body.clientWidth <= 2300? "2300px" : "auto" ));
        }

        html, body {
            height: 100%;
            overflow: auto;
        }

        body {
            padding: 0;
            margin: 0;
        }

        #silverlightControlHost {
            height: 100%;
            text-align: center;
        }

        .divWaitingIcon {
            position: fixed; /* 使图标固定在视口中 */
            top: 200px; /* 距离顶部10像素 */
            /*transform: translateX(-50%);*/ /* 水平位置校正 */
            z-index: 1000; /* 确保图标在最顶层 */
           /* box-shadow: 0px 0px 10px 10px #888888;*/
            padding-left:150px;
            text-align:center;
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

            autoheight();
        });

        function autoheight() { //函数：获取尺寸
            //获取浏览器窗口高度
            var winHeight = 0;
            if (window.innerHeight)
                winHeight = window.innerHeight;
            else if ((document.body) && (document.body.clientHeight))
                winHeight = document.body.clientHeight;

            if (document.documentElement && document.documentElement.clientHeight)
                winHeight = document.documentElement.clientHeight;

            document.getElementById("_WFDesignerFrame").style.height = (winHeight + 200) + "px";
            document.getElementById("_WFSetAreaFrame").style.height = (winHeight + 200) + "px";
        }

        window.onresize = autoheight; //浏览器窗口发生变化时同时变化DIV高度

    </script>
</head>
<body>
    <form runat="server" style="height: 100%;">
        <div id="AboveDiv">
            <asp:Label ID="LB_Sql" runat="server"></asp:Label>
            <table style="height: 100%; width: 100%;">
                <tr>
                    <td id="nodeDesign" style="width: 60%; height: 100%; vertical-align: top;">
                        <iframe id="_WFDesignerFrame" src="WFDesigner/TTTakeTopWFDesignerJSWorker.aspx?IdentifyString=<%=Request.QueryString["IdentifyString"].ToString()%>" style="width: 100%;"></iframe>
                    </td>

                    <td id="nodeDefinition" style="height: 100%; vertical-align: top;">
                        <div id="divImgWaiting" class="divWaitingIcon" style="display: none;" >
                            <img  src="Images/Processing.gif" alt="Loading,please wait..." />
                        </div>
                        <iframe id="_WFSetAreaFrame" src="TTWFTemplateView.aspx?DesignType=JS&IdentifyString=<%=Request.QueryString["IdentifyString"].ToString()%>" style="width: 100%;"></iframe>
                    </td>
                </tr>

                <tr style="display: none;">
                    <td colspan="2">
                        <asp:TextBox ID="TB_CopyRight" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_WFIdentifyString" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_WFXML" runat="server"></asp:TextBox>
                        <asp:TextBox ID="TB_WFName" runat="server" Style="width: 50px; visibility: hidden;"></asp:TextBox>

                        <asp:TextBox ID="TB_WFChartString1" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_WFChartString2" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_WFChartString3" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_WFChartString4" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:Button ID="BT_SaveWFDefinition" runat="server" CssClass="inpuLong" Text="--&gt;保存工作流模板定义" OnClick="BT_SaveWFDefinition_Click" Style="visibility: hidden" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
