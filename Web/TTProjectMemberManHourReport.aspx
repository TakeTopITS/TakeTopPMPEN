<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectMemberManHourReport.aspx.cs" Inherits="TTProjectMemberManHourReport" %>

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
                            <td height="31" align="center" class="page_topbj" style="padding: 5px 5px 5px 5px;">
                                <table width="80%" border="0"  cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BuMen%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_BelongDepartName" runat="server" Width="125px"></asp:TextBox>
                                            <cc1:ModalPopupExtender ID="TB_BelongDepartName_ModalPopupExtender" runat="server"
                                                Enabled="True" TargetControlID="TB_BelongDepartName" PopupControlID="Panel1"
                                                CancelControlID="IMBT_Close" BackgroundCssClass="modalBackground" Y="150">
                                            </cc1:ModalPopupExtender>
                                            <asp:Label ID="LB_BelongDepartCode" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label>：
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="LB_ProjectID" runat="server"></asp:Label>
                                            <asp:TextBox ID="TB_ProjectName" runat="server" Width="80%"></asp:TextBox>
                                            <asp:Button ID="BT_SelectProject" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe%>" />
                                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                                Enabled="True" TargetControlID="BT_SelectProject" PopupControlID="Panel2"
                                                CancelControlID="IMBT_Close" BackgroundCssClass="modalBackground" Y="150">
                                            </cc1:ModalPopupExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_EndDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                runat="server" TargetControlID="DLC_EndDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ChengYuanMingCheng%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_MemberName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td colspan="4" align="right">

                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" Font-Size="10pt" OnClick="BT_Find_Click"
                                                Text="<%$ Resources:lang,ChaXun%>" />

                                            <asp:Button ID="BT_Export" runat="server" CssClass="inpu" Font-Size="10pt" OnClick="BT_Export_Click"
                                                Text="<%$ Resources:lang,DaoChu%>" />

                                        </td>
                                        <td>
                                            <a href="#" onclick="preview1()">
                                                <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                            </a>

                                        </td>
                                        <td align="left"></td>
                                        <td align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZongShenBaoGongShi%>"></asp:Label>：
                                            <asp:Label ID="LB_TotalManHour" runat="server"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZongQueRenGongShi%>"></asp:Label>：<asp:Label ID="LB_TotalConfirmManHour" runat="server"></asp:Label></strong></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <!--startprint1-->
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 40%; height: 80px; font-size: xx-large; text-align: center;">
                                            <br />
                                            <asp:Label ID="LB_ReportName" runat="server"></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="padding: 5px 5px 5px 5px;">
                                            <div style="width: 1280px; word-break: break-all; word-wrap: break-word;">
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BuMenDaiMa%>"></asp:Label></strong>
                                                                                </td>

                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BuMenMingCheng%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong>
                                                                                </td>

                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,MiangMuID%>"></asp:Label></strong>
                                                                                </td>

                                                                                <td width="20%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></strong>
                                                                                </td>

                                                                                <%--  <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label></strong>
                                                                                </td>--%>

                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShenBaoGongShi%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,QueRenGongShi%>"></asp:Label></strong>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataList ID="DataList1" runat="server" CellPadding="0" DataKeyField="ProjectID"
                                                                ShowHeader="false" ForeColor="#333333" Width="100%" BorderWidth="0">
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <HeaderTemplate>
                                                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleLeft">
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BuMenDaiMa%>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleLeft">
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,BuMenMingCheng%>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleLeft">
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleLeft">
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,MiangMuXuHao%>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 20%; height: 22px; text-align: center" class="tdTitleLeft">
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label>
                                                                            </td>
                                                                            <%-- <td style="width: 10%; height: 22px; text-align: center" class="tdTitleRight">
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,RiQi%>"></asp:Label>
                                                                            </td>--%>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleRight">
                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShenBaoGongShi%>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 10%; height: 22px; text-align: center" class="tdTitleRight">
                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,QueRenGongShi%>"></asp:Label>
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <ItemTemplate>
                                                                    <table cellpadding="4" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td style="width: 10%; text-align: center" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"DepartCode") %>
                                                                            </td>
                                                                            <td style="width: 10%; text-align: left" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"DepartName") %>
                                                                            </td>
                                                                            <td style="width: 10%; text-align: center" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"UserName") %>
                                                                            </td>
                                                                            <td style="width: 10%; text-align: center" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"ProjectID") %>
                                                                            </td>

                                                                            <td style="width: 20%; text-align: left" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"ProjectName") %>
                                                                            </td>


                                                                            <%-- <td style="width: 10%; text-align: center" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"WorkDate","{0:yyyy/MM/dd}") %>
                                                                            </td>--%>


                                                                            <td style="width: 10%; text-align: center" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"ManHour") %>
                                                                            </td>

                                                                            <td style="width: 10%; text-align: left" class="tdLeft">
                                                                                <%#DataBinder .Eval (Container .DataItem ,"ConfirmManHour") %>
                                                                            </td>
                                                                        </tr>

                                                                    </table>
                                                                </ItemTemplate>

                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            </asp:DataList>
                                                            <table cellpadding="5" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td colspan="11" class="tdTopLine">
                                                                        <asp:Label ID="LB_Sql" runat="server"></asp:Label>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
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
