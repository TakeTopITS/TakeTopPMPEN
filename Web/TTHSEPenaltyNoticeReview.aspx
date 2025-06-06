<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTHSEPenaltyNoticeReview.aspx.cs" Inherits="TTHSEPenaltyNoticeReview" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

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
                    <div id="AboveDiv">
                        <table cellpadding="0" cellspacing="0" width="100%" align="left" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChuFaTongZhiPingShen%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="padding-top: 5px;">

                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="75%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ChuFaXinXi%>"></asp:Label>：<asp:TextBox ID="TextBox1" runat="server" Width="150px"></asp:TextBox>

                                                <asp:Label ID="Label18" runat="server"></asp:Label>：<asp:TextBox ID="TextBox2" runat="server" Width="150px"></asp:TextBox>

                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ChuFaRiQi%>"></asp:Label>：<asp:TextBox ID="TextBox3" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox3">
                                                </cc1:CalendarExtender>
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,Zhi%>"></asp:Label><asp:TextBox ID="TextBox4" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox4">
                                                </cc1:CalendarExtender>

                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                        <tr>
                                            <td   class="formItemBgStyleForAlignLeft">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ChuFaTongZhiLieBiao%>"></asp:Label>：</td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhengGaiMingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,PingShenBuMen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ChuFaJinE%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,PingShenRiQi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="18%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,PingShenYiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="18%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,YanZhengJieGuo%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px"
                                                    OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None" ShowHeader="false" AllowPaging="true" PageSize="10" OnPageIndexChanged="DataGrid2_PageIndexChanged">

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_Code" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Code").ToString().Trim() %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RectificationName" HeaderText="整改名称">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AuditDepartCode" HeaderText="评审部门">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PenaltyMoney" HeaderText="处罚金额">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Currency" HeaderText="Currency">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PenaltyDate" HeaderText="处罚日期" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AuditOpinion" HeaderText="评审意见">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="VerificationResults" HeaderText="验证结果">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label2" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table cellpadding="2" cellspacing="0" class="formBgStyle" width="900px">
                                <tr>
                                    <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                    </td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_Code" runat="server"></asp:Label>
                                        <asp:TextBox ID="TB_Name" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td  style="width: 150px; height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,AnQuanYinHuanZhengGai%>"></asp:Label>：
                                    </td>
                                    <td  style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_RectificationCode" runat="server" DataTextField="Name" DataValueField="Code" Enabled="False" CssClass="shuru">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ChuFaDanWei%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_PenaltyDepartCode" runat="server" Enabled="False" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ChuFaJinE%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox ID="NB_PenaltyMoney" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" Width="120px" Enabled="False" CssClass="shuru">0.00</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:DropDownList ID="DL_Currency" runat="server" DataTextField="Type" DataValueField="Type" CssClass="shuru">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZeRenRen%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_Auditor" runat="server" Enabled="False" CssClass="shuru"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ChuFaRiQi%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="DLC_PenaltyDate" runat="server" ReadOnly="false" Enabled="False" CssClass="shuru"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_PenaltyDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ChuFaNeiRong%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                        <asp:TextBox ID="TB_PenaltyRemark" runat="server" Height="40px" TextMode="MultiLine" Width="90%" Enabled="False" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,JieJueCuoShi%>"></asp:Label>： </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px" colspan="3">
                                        <asp:TextBox ID="TB_Measures" runat="server" CssClass="shuru" Height="40px" TextMode="MultiLine" Width="90%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">&nbsp;<asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,PingShenBuMen%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_AuditDepartCode" runat="server" CssClass="shuru"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,PingShenRiQi%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="DLC_AuditDate" runat="server" ReadOnly="false" CssClass="shuru"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_AuditDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:DropDownList ID="DL_Status" runat="server" Enabled="False" CssClass="shuru">
                                            <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                            <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">&nbsp;</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">&nbsp;</td>
                                </tr>
                                <tr style="color: #000000;">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,PingShenYiJian%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px" colspan="3">

                                        <asp:TextBox ID="TB_AuditOpinion" runat="server" CssClass="shuru" Height="40px" TextMode="MultiLine" Width="90%"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr style="color: #000000;">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,YanZhengJieGuo%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                        <asp:TextBox ID="TB_VerificationResults" runat="server" Height="40px" TextMode="MultiLine" Width="90%" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width: 150px;" class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Button ID="BT_Update" runat="server" OnClick="BT_Update_Click" CssClass="inpu" Text="<%$ Resources:lang,HeGe%>" Enabled="False" />&nbsp;
                                        <asp:Button ID="BT_Delete" runat="server" OnClick="BT_Delete_Click" CssClass="inpu"
                                            Text="<%$ Resources:lang,BuHeGe%>" Enabled="False" />
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>

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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
