<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPCustomerInfoView.aspx.cs" Inherits="TTAPPCustomerInfoView" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1280px;
            width: expression (document.body.clientWidth <= 1280? "1280px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
         /*    /*  if (top.location != self.location) { } else { CloseWebPage(); }*/*/



        });
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
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" >
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td height="31" class="page_topbj">
                                                <a href="TTAppCustomerManagement.aspx" target="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';">
                                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                                <table width="300" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="29">
                                                                            <img src="ImagesSkin/return.png" alt="" />
                                                                        </td>
                                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,Back%>"></asp:Label>
                                                                        </td>
                                                                        <td width="5">
                                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 5px 5px 5px 5px;">
                                                <cc2:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                    Width="100%">
                                                    <cc2:TabPanel ID="TabPanel1" runat="server" HeaderText="基本信息">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JiBenXinXi%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <table cellpadding="0" cellspacing="0" width="90%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:DataList ID="DataList2" runat="server" Height="1px" Width="100%" CellPadding="0"
                                                                            ForeColor="#333333">
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <ItemTemplate>
                                                                                <br />

                                                                                <table cellpadding="4" cellspacing="0" style="width: 100%;" class="bianTable">
                                                                                    <tr>
                                                                                        <td style="width: 10%; text-align: right;">
                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,KeHuDaiMa%>"></asp:Label>： </td>
                                                                                        <td style="width: 20%" align="left"><%#DataBinder .Eval (Container .DataItem ,"CustomerCode") %> </td>
                                                                                        <td style="width: 10%; text-align: right;">
                                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,KeHuMingCheng%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"CustomerName") %></td>
                                                                                        <td style="width: 10%; text-align: right;">
                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,GuiShuBuMen%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"BelongDepartCode") %>&nbsp;<%#DataBinder .Eval (Container .DataItem ,"BelongDepartName") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; text-align: right;">
                                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,YingWenMing%>"></asp:Label>： </td>
                                                                                        <td colspan="5" style="text-align: left"><%#DataBinder .Eval (Container .DataItem ,"CustomerEnglishName") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; text-align: right;">
                                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,HangYeLeiXing%>"></asp:Label>： </td>
                                                                                        <td style="width: 100px" align="left"><%#DataBinder .Eval (Container .DataItem ,"Type") %></td>
                                                                                        <td style="width: 100px; text-align: right;">
                                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhuYaoLianXiRen%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"ContactName") %></td>
                                                                                        <td style="text-align: right;">
                                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,YeWuYuan%>"></asp:Label>： </td>
                                                                                        <td style="width: 135px; text-align: left;"><%#DataBinder .Eval (Container .DataItem ,"SalesPerson") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; text-align: right;">
                                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,FaPiaoDiZhi%>"></asp:Label>： </td>
                                                                                        <td colspan="3" style="text-align: left"><%#DataBinder .Eval (Container .DataItem ,"InvoiceAddress") %></td>
                                                                                        <td style="text-align: right">
                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"Currency") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right">
                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YinHangZhangHao%>"></asp:Label>： </td>
                                                                                        <td colspan="3" style="text-align: left"><%#DataBinder .Eval (Container .DataItem ,"BankAccount") %></td>
                                                                                        <td style="text-align: right">
                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ZheKouLv%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"Discount") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; text-align: right">
                                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,KaiHuYinHang%>"></asp:Label>： </td>
                                                                                        <td colspan="3" style="text-align: left"><%#DataBinder .Eval (Container .DataItem ,"Bank") %></td>
                                                                                        <td style="text-align: right">
                                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XinYongDengJi%>"></asp:Label>： </td>
                                                                                        <td align="left"><%#DataBinder .Eval (Container .DataItem ,"CreditRate") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,DianHuaYi%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left">
                                                                                            <a href='tel:<%#DataBinder .Eval (Container .DataItem ,"Tel1") %>'><%#DataBinder .Eval (Container .DataItem ,"Tel1") %> </a>

                                                                                        </td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,DianHuaEr%>"></asp:Label>： </td>
                                                                                        <td style="text-align: left" class="style3">
                                                                                            <a href='tel:<%#DataBinder .Eval (Container .DataItem ,"Tel2") %>'><%#DataBinder .Eval (Container .DataItem ,"Tel2") %> </a>

                                                                                        </td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ChuanZhen%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"Fax") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; text-align: right">E_Mail： </td>
                                                                                        <td colspan="3" style="text-align: left"><%#DataBinder .Eval (Container .DataItem ,"EmailAddress") %></td>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,WangZhi%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"WebSite") %></td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YouZhengBianMa%>"></asp:Label>： </td>
                                                                                        <td style="text-align: left" class="style3"><%#DataBinder .Eval (Container .DataItem ,"ZP") %></td>
                                                                                        <td style="height: 20px; text-align: right"></td>
                                                                                        <td style="height: 20px; text-align: left"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,GuoJia%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"Country") %></td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShengFen%>"></asp:Label>： </td>
                                                                                        <td style="text-align: left" class="style3"><%#DataBinder .Eval (Container .DataItem ,"State") %></td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ChengShi%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"City") %></td>
                                                                                    </tr>
                                                                                    <tr>

                                                                                        <td style="width: 100px; height: 20px; text-align: right">

                                                                                            <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label>： </td>

                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"AreaAddress") %></td>

                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,XiangXiDiZhiZhong%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left" colspan="3"><%#DataBinder .Eval (Container .DataItem ,"RegistrationAddressCN") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,XiangXiDiZhiYing%>"></asp:Label>： </td>
                                                                                        <td colspan="5" style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"RegistrationAddressEN") %></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>： </td>
                                                                                        <td colspan="2" style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"Comment") %></td>
                                                                                        <td style="height: 20px; text-align: right">
                                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,JianLiRiQi%>"></asp:Label>： </td>
                                                                                        <td style="height: 20px; text-align: left"><%#DataBinder .Eval (Container .DataItem ,"CreateDate") %></td>
                                                                                        <td>

                                                                                            <a href='TTContactList.aspx?RelatedType=Customer&RelatedID=<%#DataBinder.Eval(Container.DataItem, "CustomerCode")%>'
                                                                                                target="DetailArea">
                                                                                                <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,XiangGuanLianXiRen%>"></asp:Label></a>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                        </asp:DataList>

                                                                        <asp:HyperLink ID="HL_RelatedContactInfor" runat="server" Enabled="false" Visible="false" Target="_blank" Text="<%$ Resources:lang,XiangGuanLianXiRen%>"></asp:HyperLink></td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel2" runat="server" HeaderText="客服记录">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,KeFuJiLu%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="5%" align="left"><strong>
                                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                <td width="10%" align="left"><strong>
                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>
                                                                                <td width="40%" align="left"><strong>
                                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,WenTi%>"></asp:Label></strong> </td>
                                                                                <td width="10%" align="left"><strong>
                                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                <td width="10%" align="left"><strong>
                                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ShouLiRen%>"></asp:Label></strong> </td>
                                                                                <td width="10%" align="left"><strong>
                                                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                <td width="10%" align="left"><strong>
                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,JiLuRen%>"></asp:Label></strong> </td>
                                                                                <td width="5%" align="left"><strong></strong></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                Height="1px" Width="100%"
                                                                CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTCustomerQuestionHandleRecordList.aspx?ID={0}"
                                                                        DataTextField="Question" HeaderText="问题" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="40%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                        DataTextField="OperatorName" HeaderText="受理人" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
    <ItemTemplate>
        <%# ShareClass.GetStatusHomeNameByOtherStatus(Eval("OperatorStatus").ToString()) %>
    </ItemTemplate>
    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
</asp:TemplateColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="RecorderCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                        DataTextField="RecorderCode" HeaderText="记录人" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ID", "TTCustomerQuestionRelatedDoc.aspx?RelatedID={0}") %>'
                                                                                Target="_blank"><img src="ImagesSkin/Doc.gif"  class="noBorder" /></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            </asp:DataGrid>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel4" runat="server" HeaderText="RelatedProject">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,GuanLianXiangMu%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <table width="99%" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="5%" align="left"><strong>
                                                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                            <td width="25%" align="left"><strong>
                                                                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,LiXiangRiQi%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label></strong> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                            GridLines="None" Width="99%">

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ProjectID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProjectName" HeaderText="项目名称">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="开始日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="结束日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="立项日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="完成程度">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="LB_FinishPercent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinishPercent")%> '></asp:Label>%
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <HeaderStyle BackColor="#507CD1" BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px"
                                                                                Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid></td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel7" runat="server" HeaderText="关联合同">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,GuanLianHeTong%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table align="left" cellpadding="0" cellspacing="0" width="98%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong> </td>
                                                                                            <td width="17%" align="left"><strong>
                                                                                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong> </td>
                                                                                            <td width="5%" align="left"><strong>
                                                                                                <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>
                                                                                            <td width="5%" align="left"><strong>
                                                                                                <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                            <td width="8%" align="left"><strong>
                                                                                                <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong> </td>
                                                                                            <td width="5%" align="left"><strong>
                                                                                                <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,YiFangDanWei%>"></asp:Label></strong> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid6" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                            Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ConstractCode" HeaderText="ContractCode">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                                    DataTextField="ConstractName" HeaderText="ContractName">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="17%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="SignDate" HeaderText="SigningDate" DataFormatString="{0:yyyy/MM/dd}">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Currency" HeaderText="Currency">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PartA" HeaderText="PartyAUnit">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PartB" HeaderText="乙方单位">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid><asp:Label ID="Label4" runat="server" Visible="False"></asp:Label><asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                            Visible="False" Width="57px"></asp:Label><asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                                Visible="False" Width="57px"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel3" runat="server" HeaderText="关联物料销售订单">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,GuanLianShangPinXiaoShouDingDan%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                width="100%">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="25%"><strong>
                                                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="15%"><strong>
                                                                                    <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="20%"><strong>
                                                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,XiaoShouShiJian%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,YeWuYuan%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="right" width="6">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid5" runat="server" AutoGenerateColumns="False"
                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                ShowHeader="False"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="SOID" HeaderText="Number">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="SOID" DataNavigateUrlFormatString="TTGoodsSaleOrderView.aspx?SOID={0}"
                                                                        DataTextField="SOName" HeaderText="Name" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:BoundColumn DataField="Amount" HeaderText="总金额">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SaleTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="销售时间">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="SalesCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                        DataTextField="SalesName" HeaderText="Salesperson" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            </asp:DataGrid>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel5" runat="server" HeaderText="关联物料退货单">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,GuanLianShangPinTuiHuoDan%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="15%" align="left"><strong>
                                                                                    <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>
                                                                                <td width="40%" align="left"><strong>
                                                                                    <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>
                                                                                <td width="15%" align="left"><strong>
                                                                                    <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong> </td>
                                                                                <td width="15%" align="left"><strong>
                                                                                    <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label></strong> </td>
                                                                                <td width="15%" align="left"><strong>
                                                                                    <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong> </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="False" Height="1px"
                                                                Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                <Columns>
                                                                    <asp:BoundColumn DataField="ROID" HeaderText="Number">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="ROID" DataNavigateUrlFormatString="TTGoodsReturnOrderView.aspx?ROID={0}"
                                                                        DataTextField="ReturnName" HeaderText="Name" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="40%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="CurrencyType" HeaderText="Currency">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Applicant" HeaderText="Applicant">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <ItemStyle CssClass="itemStyle" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            </asp:DataGrid>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel6" runat="server" HeaderText="报价单">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,XiangGuanBaoJiaDan%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                width="100%">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="40%"><strong>
                                                                                    <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label75" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label76" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="20%"><strong>
                                                                                    <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,BaoJiaShiJian%>"></asp:Label></strong> </td>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label78" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="right" width="6">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid7" runat="server" AutoGenerateColumns="False"
                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                ShowHeader="False"
                                                                Width="100%">

                                                                <Columns>
                                                                    <asp:BoundColumn DataField="QOID" HeaderText="Number">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="QOID" DataNavigateUrlFormatString="TTGoodsSaleQuotationOrderView.aspx?QOID={0}"
                                                                        DataTextField="QOName" HeaderText="Name" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="40%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:BoundColumn DataField="Amount" HeaderText="总金额">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="CurrencyType" HeaderText="Currency">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="QuotationTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="报价时间">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <ItemStyle CssClass="itemStyle" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            </asp:DataGrid>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel8" runat="server" HeaderText=" 物料保修">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label79" runat="server" Text="<%$ Resources:lang,ShangPinBaoXiu%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" align="left" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,XuLieHao%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label81" runat="server" Text="<%$ Resources:lang,ZhongDuanKeHu%>"></asp:Label></strong> </td>
                                                                                            <td width="8%" align="left"><strong>
                                                                                                <asp:Label ID="Label82" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>
                                                                                            <td width="8%" align="left"><strong>
                                                                                                <asp:Label ID="Label83" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong> </td>
                                                                                            <td width="8%" align="left"><strong>
                                                                                                <asp:Label ID="Label84" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong> </td>
                                                                                            <td width="6%" align="left"><strong>
                                                                                                <asp:Label ID="Label85" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong> </td>
                                                                                            <td width="6%" align="left"><strong>
                                                                                                <asp:Label ID="Label86" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label87" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label></strong> </td>
                                                                                            <td width="6%" align="left"><strong>
                                                                                                <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label89" runat="server" Text="<%$ Resources:lang,ChuKuKeHu%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label90" runat="server" Text="<%$ Resources:lang,ChuKuShiJian%>"></asp:Label></strong> </td>
                                                                                            <td width="7%" align="left"><strong>
                                                                                                <asp:Label ID="Label91" runat="server" Text="<%$ Resources:lang,BaoXiuQi%>"></asp:Label></strong> </td>
                                                                                            <td width="8%" align="left"><strong>
                                                                                                <asp:Label ID="Label92" runat="server" Text="<%$ Resources:lang,DaoQiShiJian%>"></asp:Label></strong> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid9" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid9_ItemCommand"
                                                                            ShowHeader="False" Height="1px"
                                                                            Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="系列号">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_GoodsSN" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SN") %>'
                                                                                            class="inpuLong" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="FinalCustomerCode" DataNavigateUrlFormatString="TTCustomerInfoView.aspx?CustomerCode={0}"
                                                                                    DataTextField="FinalCustomerName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="MaterialName">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ShipmentNO" HeaderText="出库单号">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="CustomerCode" DataNavigateUrlFormatString="TTCustomerInfoView.aspx?CustomerCode={0}"
                                                                                    DataTextField="CustomerName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="ShipTime" HeaderText="出库时间" DataFormatString="{0:yyyy/MM/dd}">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="WarrantyPeriod" HeaderText="保修期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="WarrantyEndTime" HeaderText="EndTime" DataFormatString="{0:yyyy/MM/dd}">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label93" runat="server" Text="<%$ Resources:lang,ShouHouRenWu%>"></asp:Label>： </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="9%"><strong>
                                                                                                <asp:Label ID="Label94" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="8%"><strong>
                                                                                                <asp:Label ID="Label95" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="12%"><strong>
                                                                                                <asp:Label ID="Label96" runat="server" Text="<%$ Resources:lang,RenWu%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="8%"><strong>
                                                                                                <asp:Label ID="Label97" runat="server" Text="<%$ Resources:lang,YouXianJi%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="8%"><strong>
                                                                                                <asp:Label ID="Label98" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="10%"><strong>
                                                                                                <asp:Label ID="Label99" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="10%"><strong>
                                                                                                <asp:Label ID="Label100" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="7%"><strong>
                                                                                                <asp:Label ID="Label101" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="7%"><strong>
                                                                                                <asp:Label ID="Label102" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="7%"><strong>
                                                                                                <asp:Label ID="Label103" runat="server" Text="<%$ Resources:lang,FeiYong%>"></asp:Label></strong> </td>
                                                                                            <td align="left" width="7%"><strong>
                                                                                                <asp:Label ID="Label104" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False"
                                                                            ShowHeader="False" OnItemCommand="DataGrid3_ItemCommand"
                                                                            Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Number">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_TaskID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TaskID") %>'
                                                                                            CssClass="inpu" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Task" HeaderText="Task">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Priority" HeaderText="优先级">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Budget" HeaderText="Budget">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="FinishPercent" HeaderText="完成程度">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Expense" HeaderText="Expense">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="7%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label105" runat="server" Text="<%$ Resources:lang,LingYongPeiJian%>"></asp:Label>： </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label106" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                            <td width="20%" align="left"><strong>
                                                                                                <asp:Label ID="Label107" runat="server" Text="<%$ Resources:lang,ShangPinMing%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label108" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong> </td>
                                                                                            <td width="20%" align="left"><strong>
                                                                                                <asp:Label ID="Label109" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label110" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label111" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label112" runat="server" Text="<%$ Resources:lang,YiChuKu%>"></asp:Label></strong> </td>
                                                                                            <td width="10%" align="left"><strong>
                                                                                                <asp:Label ID="Label113" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label></strong> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                            Height="30px" Width="100%" ID="DataGrid8">

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="物料名">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CheckOutNumber" HeaderText="已出库">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <ItemStyle CssClass="itemStyle"></ItemStyle>
                                                                            <PagerStyle Horizontalalign="center"></PagerStyle>
                                                                        </asp:DataGrid><asp:Label ID="LB_UserCode" runat="server"
                                                                            Visible="False"></asp:Label><asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label><asp:Label ID="LB_UserName" runat="server"
                                                                                Visible="False"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>

                                                    <cc2:TabPanel ID="TabPanel10" runat="server" HeaderText="关联物料">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GuanLianLiaoPin%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <br />
                                                            <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                                                <tr>

                                                                    <td align="left">

                                                                        <asp:Label ID="LB_GoodsOwner" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                                                </tr>

                                                                <tr>

                                                                    <td>

                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">

                                                                            <tr>

                                                                                <td width="7">

                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>

                                                                                <td>

                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                        <tr>

                                                                                            <td align="left" width="8%"><strong>

                                                                                                <asp:Label ID="Label114" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="10%"><strong>

                                                                                                <asp:Label ID="Label134" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="20%"><strong>

                                                                                                <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="15%"><strong>

                                                                                                <asp:Label ID="Label139" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="13%"><strong>

                                                                                                <asp:Label ID="Label136" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="8%"><strong>

                                                                                                <asp:Label ID="Label137" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>

                                                                                            <td align="left" width="10%"><strong>

                                                                                                <asp:Label ID="Label138" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td align="right" width="6">

                                                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                                                            </tr>
                                                                        </table>

                                                                        <asp:DataGrid ID="DataGrid12" runat="server" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                            ShowHeader="False"
                                                                            Width="100%">

                                                                            <Columns>

                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="13%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>


                                                                            </Columns>

                                                                            <EditItemStyle BackColor="#2461BF" />

                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <ItemStyle CssClass="itemStyle" />

                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid></td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                </cc2:TabContainer>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
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
