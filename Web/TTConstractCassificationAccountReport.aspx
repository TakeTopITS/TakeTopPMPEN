<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTConstractCassificationAccountReport.aspx.cs" Inherits="TTConstractCassificationAccountReport" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #topNav {
            /*background-color:#096;*/
            z-index: 999;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            /*_position: absolute; /* for IE6 */ */
            /*_top: expression(documentElement.scrollTop + documentElement.clientHeight-this.offsetHeight); /* for IE6 */
            overflow: visible;
        }

        #bottomNav {
            /*background-color:#096;*/
            z-index: -2;
            position: relative;
            top: 225px;
            left: 0;
            width: 100%;
            /*_position: absolute; /* for IE6 */ */
            /*_top: expression(documentElement.scrollTop + documentElement.clientHeight-this.offsetHeight); /* for IE6 */
            overflow: visible;
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });

        function preview() {
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
            <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <%--   <div id="topNav">--%>

                    <table cellpadding="3" cellspacing="0" class="formBgStyle" style="width: 800px;">
                        <tr>
                            <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,HeTongHao%>"></asp:Label>��
                            </td>
                            <td width="20%" class="formItemBgStyleForAlignLeft">
                                <asp:TextBox ID="TB_ConstractCode" runat="server" Width="99%"></asp:TextBox>
                            </td>

                            <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label>��
                            </td>
                            <td class="formItemBgStyleForAlignLeft">
                                <asp:TextBox ID="TB_ConstractName" runat="server" Width="99%"></asp:TextBox>

                            </td>
                            <td class="formItemBgStyleForAlignLeft" style="width: 10%; ">
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,HeTongLeiXing%>"></asp:Label>��
                            </td>
                            <td class="formItemBgStyleForAlignLeft" width="20%" >
                                <asp:DropDownList ID="DL_ConstractType" runat="server" DataTextField="Type" DataValueField="Type" AutoPostBack="true" Height="25px" OnSelectedIndexChanged="DL_ConstractType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>

                            <td class="formItemBgStyleForAlignLeft">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>��
                            </td>
                            <td class="formItemBgStyleForAlignLeft" >
                                <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server" Width="139px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_StartTime">
                                </ajaxToolkit:CalendarExtender>

                            </td>
                            <td class="formItemBgStyleForAlignLeft">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>��
                            </td>
                            <td class="formItemBgStyleForAlignLeft" >

                                <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server" Width="139px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_EndTime">
                                </ajaxToolkit:CalendarExtender>

                            </td>
                              <td class="formItemBgStyleForAlignLeft" >
                                <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                            </td>
                            <td class="formItemBgStyleForAlignLeft" >
                                <a href="#" onclick="preview()">
                                <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
                            </a></td>

                        </tr>
                    </table>
                    <br />
                    <!--startprint1-->
                    <table width="1200px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="5" style="width: 1200px; height: 80px; font-size: xx-large;" align="center">
                                <br />
                                <asp:Label ID="LB_ConstractType" runat="server"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HeTongFengLiTaiZhuang%>"></asp:Label>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <table width="1200px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label3" Text="<%$ Resources:lang,ZongFengShu%>" runat="server"></asp:Label>
                                :
                                <asp:Label ID="LB_CopyNumber" runat="server"></asp:Label>
                                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>:
                                <asp:Label ID="LB_TotalAmount" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <%-- <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,YingShouZongE%>"></asp:Label>��<asp:Label ID="LB_ReceivablesAmount" runat="server"></asp:Label>
                                &nbsp;
                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ShiShouZongE%>"></asp:Label>��<asp:Label ID="LB_ReceiverAmount" runat="server"></asp:Label>

                                &nbsp;
                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,WeiShouZongE%>"></asp:Label>��<asp:Label ID="LB_UNReceiveAmount" runat="server"></asp:Label>--%>

                                <asp:Label ID="LB_PrintTime" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <%-- <table width="1200px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                    <tr>
                                        <td width="7">
                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>

                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label14" runat="server" Text="PartyAUnit"></asp:Label></strong>
                                                    </td>
                                                    <td width="12%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label16" runat="server" Text="MainContent"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label17" runat="server" Text="ContractType"></asp:Label></strong>
                                                    </td>
                                                    <td width="9%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label18" runat="server" Text="ContractAmount"></asp:Label></strong>
                                                    </td>

                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label21" runat="server" Text="EndTime"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,QianDingShiJian%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShouKuanJiHua%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,FuKuanJiHua%>"></asp:Label></strong>
                                                    </td>
                                                    <td width="7%" align="left">
                                                        <strong>
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YiChangMiaoShu%>"></asp:Label></strong>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="6" align="right">
                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>--%>
                    <%--  </div>
                    <br />
                 
                    <div id="bottomNav">--%>
                    <table width="1200px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                                    Height="1px" Width="100%"
                                    CellPadding="4" ForeColor="#333333" GridLines="Both">
                                    <Columns>
                                        <asp:BoundColumn DataField="ConstractCode" HeaderText="<%$ Resources:lang,HeTongDaiMa%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="7%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PartA" HeaderText="<%$ Resources:lang,JiaFangDanWei%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ConstractName" HeaderText="<%$ Resources:lang,HeTongMingCheng%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle CssClass="" HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="MainContent" HeaderText="<%$ Resources:lang,HeTongNeiRong%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle HorizontalAlign="Left" Width="9%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Type" HeaderText="<%$ Resources:lang,HeTongLeiXing%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="7%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Amount" HeaderText="<%$ Resources:lang,HeTongJinE%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="9%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Startdate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="<%$ Resources:lang,KaiShiRiQi%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="7%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="<%$ Resources:lang,JieShuRiQi%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="7%" />
                                        </asp:BoundColumn>

                                        <asp:BoundColumn DataField="SignDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="<%$ Resources:lang,QianDingRiQi%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle Horizontalalign="left" Width="7%" />
                                        </asp:BoundColumn>

                                        <asp:BoundColumn DataField="ReceivablesPlan" HeaderText="<%$ Resources:lang,ShouKuanJiHua%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle HorizontalAlign="Left" Width="7%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PayablePlan" HeaderText="<%$ Resources:lang,FuKuanJiHua%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle HorizontalAlign="Left" Width="7%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Exception" HeaderText="<%$ Resources:lang,YiChangMiaoShu%>" HeaderStyle-Horizontalalign="left">
                                            <ItemStyle HorizontalAlign="Left" Width="7%" />
                                        </asp:BoundColumn>
                                    </Columns>
                                    <ItemStyle CssClass="itemStyle" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditItemStyle BackColor="#2461BF" />
                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>


                    <asp:Label ID="LB_UserCode" runat="server"
                        Visible="False"></asp:Label>
                    <asp:Label ID="LB_UserName" runat="server"
                        Visible="False"></asp:Label>
                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>

                    <!--endprint1-->
                    <%--  </div>--%>
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
