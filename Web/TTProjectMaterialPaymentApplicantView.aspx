<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectMaterialPaymentApplicantView.aspx.cs" Inherits="TTProjectMaterialPaymentApplicantView" %>

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
            font-family: ΢���ź�,����;
            font-size: 1em;
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
                                <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
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
                                        <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,XiangMuWZFYFKSQ%>"></asp:Label>
                                        <br />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; width: 980px;">
                            <asp:DataList ID="DataList1" runat="server" Width="980px" CellPadding="0" CellSpacing="0">
                                <ItemTemplate>

                                    <table width="98%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft"  width="15%">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>��
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 35%; ">
                                                <%#DataBinder.Eval(Container.DataItem, "AOName")%>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                                <asp:Label ID="Label8" runat="server" Text="Applicant"></asp:Label>��
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%#DataBinder.Eval(Container.DataItem, "UserName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft"  width="15%">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>��
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 35%; ">
                                                <%#DataBinder.Eval(Container.DataItem, "ProjectID")%>
                                                <%#DataBinder.Eval(Container.DataItem, "ProjectName")%>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>��
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">

                                                <%#DataBinder.Eval(Container.DataItem, "CreateTime", "{0:yyyy/MM/dd}")%>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>��
                                            </td>
                                            <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                                <%#DataBinder.Eval(Container.DataItem, "ProjectID")%>

                                                <%#DataBinder.Eval(Container.DataItem, "ProjectName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,HeTongQianYueFang%>"></asp:Label>�� </td>
                                            <td class="formItemBgStyleForAlignLeft" >
                                                <%#DataBinder.Eval(Container.DataItem, "PartA")%>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,LianXiFangShi%>"></asp:Label>�� </td>
                                            <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                <%#DataBinder.Eval(Container.DataItem, "PartAContactInformation")%>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ZhiFuFangShi%>"></asp:Label>��</td>
                                            <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                <%#DataBinder.Eval(Container.DataItem, "PaymentMethod")%>

                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,LeiJiYuJiaoFaPiao%>"></asp:Label>��

                                                 <%#DataBinder.Eval(Container.DataItem, "AleadyTotalInvoice")%>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,LeiJiYingJiaoFaPiao%>"></asp:Label>��
                                                   <%#DataBinder.Eval(Container.DataItem, "ShouldTotalInvoice")%>

                                                                                               &nbsp;&nbsp;
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>��

                                                    <%#DataBinder.Eval(Container.DataItem, "CurrencyType")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label103" runat="server" Text="<%$ Resources:lang,KaiHuYingHang%>"></asp:Label>��</td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <%#DataBinder.Eval(Container.DataItem, "BankName")%></td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label111" runat="server" Text="<%$ Resources:lang,YinHangZhangHao%>"></asp:Label>��</td>
                                            <td class="formItemBgStyleForAlignLeft" >
                                                <%#DataBinder.Eval(Container.DataItem, "BankCode")%></td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label1112212" runat="server" ext="<%$ Resources:lang,HeTongFuKuanTiaoJianHeYuJiMiaoShu%>"></asp:Label>��</td>
                                            <td colspan="3" class="formItemBgStyleForAlignLeft" >

                                                <%#DataBinder.Eval(Container.DataItem, "ContractPayCondition")%></td>
                                        </tr>

                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,YingJiaoFuJian%>"></asp:Label>�� </td>
                                            <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                <%#DataBinder.Eval(Container.DataItem, "Attachment")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 70%;">

                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                width="100%">
                                <tr>
                                    <td width="7">
                                        <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="6%"><strong>
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>
                                                <td align="left" width="12%"><strong>
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>
                                                <td align="left" width="8%"><strong>
                                                    <asp:Label ID="Label113" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong></td>
                                                <%--  <td align="left" width="8%"><strong>
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>--%>
                                                <td align="left" width="15%"><strong>
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>
                                                <td align="left" width="8%"><strong>
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong></td>
                                                <td align="left" width="6%"><strong>
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label></strong></td>
                                                <td align="left" width="6%"><strong>
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="6">
                                        <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                ShowHeader="False"
                                Width="100%">
                                <Columns>

                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Manufacture" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                    <%--   <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>--%>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                </Columns>
                                <EditItemStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <ItemStyle CssClass="itemStyle" />

                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataGrid>
                        </td>
                    </tr>

                    <tr>
                        <td class="formItemBgStyleForAlignLeft">
                            <asp:Label ID="Label1331" runat="server" Text="<%$ Resources:lang,BenChiQingKuanJinE %>"></asp:Label>��
                            <asp:Label ID="LB_CurrentTotalPaymentAmount" runat="server"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label8882" runat="server" Text="<%$ Resources:lang,DaXie %>"></asp:Label>��
                            <asp:Label ID="LB_CurrentTotalPaymentAmountLarge" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <!--endprint1-->
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
