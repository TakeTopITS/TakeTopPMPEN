<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectDailyWorkReportForCustomer.aspx.cs" Inherits="TTProjectDailyWorkReportForCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目工作日志</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function preview() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint-->";
            eprnstr = "<!--endprint-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            document.body.innerHTML = bdhtml;
            return false;
        }

        function setValue(m_strValue) {
            if (m_strValue != null && m_strValue != undefined) {
                txtReturnValue.value = m_strValue;
            }
        }
    </script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            

        });

    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="980" class="bian">
                        <tr>
                            <td height="31" class="page_topbj" style="padding: 5px 5px 5px 5px;">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongZuoZongJieHuiZong%>"></asp:Label>
                                        </td>
                                        <td>（<asp:Button ID="BT_AllMember" runat="server" CssClass="inpuLong" OnClick="BT_AllMember_Click"
                                            Text="<%$ Resources:lang,ChaKanSuoYouGongZuoZongJie%>" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label>：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="DLC_EndDate" ReadOnly="false" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                runat="server" TargetControlID="DLC_EndDate">
                                            </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="BT_DateFind" runat="server" CssClass="inpu" Font-Size="10pt" OnClick="BT_DateFind_Click"
                                                Text="<%$ Resources:lang,ChaXun%>" />
                                            &nbsp;&nbsp;  <a href="#" onclick="preview()">
                                                <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                            </a>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="6">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="text-align: left; width: 65%; height: 25px;">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ChaXunFanWeiShi%>"></asp:Label>：<asp:Label ID="LB_QueryScope" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right; width: 35%; height: 25px;">
                                                        <asp:Label ID="LB_Operator" runat="server" Text="<%$ Resources:lang,Operator%>" />
                                                        :
                                                        <asp:Label ID="LB_UserCode" runat="server"></asp:Label>
                                                        &nbsp;
                                                        <asp:Label ID="LB_UserName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>

                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <!--startprint-->
                                <table width="100%">

                                    <tr>
                                        <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">

                                            <br />
                                            <asp:Label ID="LB_ReportName" runat="server"></asp:Label>
                                            <br />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XiangMuBianHao%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_ProjectID" runat="server"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_ProjectName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XiangMuJingLi%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_PMName" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,LeiJiRenTian1%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShenBaoGongShi%>"></asp:Label>:
                                                        <asp:Label ID="LB_ManHour" runat="server"></asp:Label>
                                                        &nbsp;
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XiangMuGongShiYuSuan%>"></asp:Label>(:
                                                        <asp:Label ID="LB_ProManHour" runat="server"></asp:Label>
                                                        ) </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,LiChengBei%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,RiQiFanWei%>"></asp:Label></td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TimeScope" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">

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
                                                                            <td width="8%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                            </td>
                                                                            <td width="15%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GongZuoRiQi%>"></asp:Label></strong>
                                                                            </td>
                                                                            <td width="55%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label></strong>
                                                                            </td>
                                                                            <td width="12%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ChengYuan%>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="10%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShiJiGongShi%>"></asp:Label></strong>
                                                                            </td>
                                                                           
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="7" align="right">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataList ID="DataList1" runat="server" CellPadding="0" DataKeyField="WorkID"
                                                            ShowHeader="false" Width="100%" BorderWidth="0">
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <ItemTemplate>
                                                                <table cellpadding="4" cellspacing="0" width="100%">
                                                                    <tr style="font-weight: bold;">
                                                                        <td style="width: 8%; text-align: center" class="tdLeft">
                                                                            <%#DataBinder .Eval (Container .DataItem ,"WorkID") %>
                                                                        </td>
                                                                        <td style="width: 15%; text-align: center" class="tdLeft">
                                                                            <%#DataBinder .Eval (Container .DataItem ,"WorkDate","{0:yyyy/MM/dd}") %>
                                                                        </td>
                                                                        <td style="width: 55%; text-align: left" class="tdLeft">
                                                                            <%#DataBinder .Eval (Container .DataItem ,"ProjectName") %>
                                                                        </td>
                                                                        <td style="width: 12%; text-align: center" class="tdLeft">
                                                                          <%#DataBinder .Eval (Container .DataItem ,"UserName") %>
                                                                        </td>
                                                                        <td style="width: 10%; text-align: center" class="tdLeft">
                                                                            <%#DataBinder .Eval (Container .DataItem ,"ConfirmManHour") %>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" valign="top" style="width: 8%; padding-left: 5px" class="tdLeft">
                                                                            <span style="font-size: 10pt">
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,MeiRiZongJie%>"></asp:Label></span>
                                                                        </td>
                                                                        <td colspan="2"  valign="top" style="text-align: left; padding-left: 5px"
                                                                            class="tdLeft">
                                                                            <span style="font-size: 9pt"><%#DataBinder .Eval (Container .DataItem ,"DailySummary") %></span></td>
                                                                        <td colspan="2" valign="top" style="text-align: left; padding-left: 5px"
                                                                            class="tdLeft" rowspan="2">
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label>： <span style="font-size: 9pt"><%#DataBinder .Eval (Container .DataItem ,"Address") %></span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="tdLeft" style="width: 8%; padding-left: 5px" valign="top">
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ChengGuo%>"></asp:Label></td>
                                                                        <td class="tdLeft" colspan="4" style="text-align: left; padding-left: 5px" valign="top" width="80%">
                                                                            <span style="font-size: 9pt"><%#DataBinder .Eval (Container .DataItem ,"Achievement") %></span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            
                                                            <ItemStyle />
                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        </asp:DataList>
                                                        <table cellpadding="5" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,GongShiTongJi%>"></asp:Label> </td>
                                                                <td class="tdTopLine" align="left">
                                                                    <asp:Label ID="LB_ConfirmManHour" runat="server"></asp:Label>
                                                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 2px; text-align: left; padding-top: 5px">
                                                        <table cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 250px; background-color: White;">
                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,KeFangQueRen%>"></asp:Label>：
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 250px; background-color: White;">
                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,WoFangYiJian%>"></asp:Label>：
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>

                                            <br />

                                        </td>
                                    </tr>

                                </table>
                                <!--endprint-->
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
