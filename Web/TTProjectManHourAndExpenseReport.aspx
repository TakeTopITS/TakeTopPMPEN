<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectManHourAndExpenseReport.aspx.cs" Inherits="TTProjectManHourAndExpenseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目成员任务报表</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

        });

        function preview1() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint1-->";
            eprnstr = "<!--endprint1-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            document.body.innerHTML = bdhtml;
            return false;
        }
    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                     <div id="AboveDiv">
                    <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                        <tr>
                            <td height="31" align="left" class="page_topbj">
                                <table width="900px" border="0" cellpadding="0" cellspacing="0" style="padding: 5px 5px 5px 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMuMingChen%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="LB_ProjectID" runat="server"></asp:Label>
                                            <asp:TextBox ID="TB_ProjectName" runat="server"></asp:TextBox>
                                            <asp:Button ID="BT_SelectProject" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe%>" />
                                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                                Enabled="True" TargetControlID="BT_SelectProject" PopupControlID="Panel2"
                                                CancelControlID="IMBT_Close" BackgroundCssClass="modalBackground" Y="150">
                                            </cc1:ModalPopupExtender>
                                        </td>

                                        <td>
                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,XiangMuFuZeRen%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TB_PMName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JinChangRiJi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ChuChangRiJi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_EndDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                runat="server" TargetControlID="DLC_EndDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" align="right">

                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" Font-Size="10pt" OnClick="BT_Find_Click"
                                                Text="<%$ Resources:lang,ChaXun%>" />

                                            <asp:Button ID="BT_Export" runat="server" CssClass="inpu" Font-Size="10pt" OnClick="BT_Export_Click"
                                                Text="<%$ Resources:lang,DaoChu%>" />
                                        </td>
                                        <td align="center" style="padding-top: 5px;">
                                            <a href="#" onclick="preview1()">
                                                <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                            </a>
                                        </td>
                                        <td align="center" style="padding-top: 5px;">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,JiLuShu%>"></asp:Label>
                                            ：<asp:Label ID="LB_ResultNumber" runat="server"></asp:Label>&nbsp; 
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,Tiao%>"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZuBie%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_BelongDepartName" runat="server" Width="125px"></asp:TextBox>
                                            <cc1:ModalPopupExtender ID="TB_BelongDepartName_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="IMBT_Close" Enabled="True" PopupControlID="Panel1" TargetControlID="TB_BelongDepartName" Y="150">
                                            </cc1:ModalPopupExtender>
                                            <asp:Label ID="LB_BelongDepartCode" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="DL_Status" DataTextField="HomeName" DataValueField="Status" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <!--startprint1-->
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 80px; font-size: xx-large; text-align: center;">
                                            <br />
                                            <asp:Label ID="LB_ReportName" runat="server"></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="padding: 5px 5px 5px 5px;">
                                            <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <!--endprint1-->
                                <br />
                            </td>
                        </tr>
                    </table>

                         </div>


                    <asp:Label ID="LB_DepartString" runat="server" Visible="false"></asp:Label>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="ImageButton1" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BT_Export" />
                </Triggers>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 50%; top: 50%;">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="Images/Processing.gif" alt="Loading,please wait..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
