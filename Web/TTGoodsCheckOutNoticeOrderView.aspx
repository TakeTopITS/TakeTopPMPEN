<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsCheckOutNoticeOrderView.aspx.cs" Inherits="TTGoodsCheckOutNoticeOrderView" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        body {
            font-family: 微软雅黑,宋体;
            font-size: small;
        }
    </style>

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
        <form id="form2" runat="server">
            <div style="position: relative; top: 50px;">
                <table width="100%">
                    <tr>
                        <td width="" align="right">
                            <a href="#" onclick="preview1()">
                                <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                            </a>
                        </td>

                    </tr>
                </table>
                <!--startprint1-->
                <table style="width: 980px;">
                    <tr>
                        <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                            <table width="100%">
                                <tr>
                                    <td width="200px">
                                        <img src="Logo/FormLogo.png" /></td>
                                    <td width="550px" style="font-size: xx-large; text-align: center;" class="auto-style1">
                                        <br />
                                        <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,ChuHuoTongZhiDan%>"></asp:Label>
                                        <br />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: center; width: 980px;">
                            <asp:DataList ID="DataList1" runat="server" Height="1px" Width="980px" CellPadding="0"
                                CellSpacing="0">
                                <ItemTemplate>
                                    <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" style="width: 98%;">
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" width="15%">

                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                ： </td>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 35%">

                                                <%# DataBinder.Eval(Container.DataItem,"COOName") %>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft" width="20%">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                ： </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"Type") %>
                                                                   
                                            </td>
                                        </tr>
                                        <tr>
                                            <<td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin%>"></asp:Label>
                                                ： </td>
                                            <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                <%# DataBinder.Eval(Container.DataItem,"ApplyReason") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <<td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>
                                                ：</td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"Amount") %>
                                                                   
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>
                                                ：</td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"CurrencyType") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <<td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>
                                                ： </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"ApplyTime") %>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,YaoQiuWanChengShiJian%>"></asp:Label>
                                                ： </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"FinishTime") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>
                                                ： </td>
                                            <td colspan ="3"  class="formItemBgStyleForAlignLeft">
                                                <%# DataBinder.Eval(Container.DataItem,"ApplicantCode") %> <%# DataBinder.Eval(Container.DataItem,"ApplicantName") %>
                                            </td>
                                           
                                        </tr>
                                      
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZhiZaoDanHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KeHuXingHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,TiaoMa%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label>PCS</strong>
                                                </td>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XiangShu%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,WanChengQingKang%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,JiaoHuDiDian%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ChuHuShiJian%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                </tr>
                            </table>
                            <asp:DataGrid runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                Height="30px" Width="100%" ID="DataGrid1">

                                <Columns>
                                    <asp:BoundColumn DataField="PDName" HeaderText="制造单">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CustomerModelNumber" HeaderText="客户型号">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="本厂型号">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="BarCode" HeaderText="条码">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="BoxNumber" HeaderText="装箱数">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Status">
    <ItemTemplate>
        <%# ShareClass.GetStatusHomeNameByOtherStatus(Eval("FinishStatus").ToString()) %>
    </ItemTemplate>
    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
</asp:TemplateColumn>
                                    <asp:BoundColumn DataField="DeliveryAddress" HeaderText="交货地点">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CheckOutTime" HeaderText="出货时间" DataFormatString="{0:yyyy/MM/dd}">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Comment" HeaderText="Remark">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>

                                </Columns>
                                <ItemStyle CssClass="itemStyle"></ItemStyle>
                                <PagerStyle Horizontalalign="center"></PagerStyle>
                            </asp:DataGrid>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table width="80%">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhiBiao%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiXing%>"></asp:Label>：</td>

                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PiZhun%>"></asp:Label>：</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--endprint1-->
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
