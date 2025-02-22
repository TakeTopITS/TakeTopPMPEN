<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="TTAIHandlerByDeepSeek.aspx.cs" Inherits="TTAIHandlerByDeepSeek" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Local API Integration</title>

    <script>
        function AIURLClick() {

            document.getElementById('HL_AIURL').click();
        }


        //文本框加回车键功能
        document.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') {
                // 检查焦点是否在输入框内
                if (document.activeElement === document.getElementById('txtPrompt')) {
                    // 模拟点击按钮
                    document.getElementById('btnGenerateText').click();
                }
            }
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 100%; text-align: center;">
                    <center>
                        <table style="text-align: center;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPrompt" runat="server" Width="500px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnGenerateText" ImageUrl="ImagesSkin/AIGenerate.png" runat="server" Text="生成" OnClick="btnGenerateText_Click" OnClientClick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';" />
                                </td>
                                <td>
                                    <img id="IMG_Waiting" src="Images/Processing.gif" alt="Loading,please wait..." style="text-align: center; display: none;" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnStopSeek" ImageUrl="ImagesSkin/AIStop.png" runat="server" Text="停止" OnClick="btnStopAI_Click" OnClientClick="javascript:document.getElementById('IMG_Waiting').style.display = 'none';" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <asp:Label ID="lblGeneratedText" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <br />
                <div style="display: none;">
                    <asp:HyperLink ID="HL_AIURL" runat="server" Target="_blank"></asp:HyperLink>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 50%; top: 50%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <%--  <img src="Images/Processing.gif" alt="Loading,please wait..." />--%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>

</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
