<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectTaskReviewWL.aspx.cs"
    Inherits="TTProjectTaskReviewWL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
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
            <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table cellpadding="0" cellspacing="0" width="100%" class="bian">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,RenWuPingShen%>"></asp:Label>
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
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td style="padding-top: 5px">
                                            <asp:DataList ID="DataList1" runat="server" Height="1px" Width="98%" CellPadding="0">
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <ItemTemplate>
                                                    <table width="100%" class="bian">
                                                        <tr>
                                                            <td style="width: 10%; text-align: right;">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label>:
                                                            </td>
                                                            <td style="width: 15%; text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"TaskID") %>
                                                            </td>
                                                            <td style="width: 10%; text-align: right;">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChuangJianZhe%>"></asp:Label>��
                                                            </td>
                                                            <td style="width: 15%; text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"MakeManName") %>
                                                            </td>
                                                            <td style="width: 15%; text-align: right;">
                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XiangGuanXiangMu%>"></asp:Label>��
                                                            </td>
                                                            <td style="width: 30%; text-align: left;">
                                                                <a href='<%#"TTProjectDetailView.aspx?ProjectID="+DataBinder.Eval(Container.DataItem,"ProjectID")%>'
                                                                    target="_blank">
                                                                    <%# DataBinder.Eval(Container.DataItem,"ProjectID") %></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,RenWu%>"></asp:Label>:
                                                            </td>
                                                            <td colspan="5" style="text-align: left">
                                                                <%# DataBinder.Eval(Container.DataItem,"Task") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"BeginDate","{0:yyyy/MM/dd}") %>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"EndDate","{0:yyyy/MM/dd}") %>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"Budget") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 18px; text-align: right;">
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,FeiYong%>"></asp:Label>:
                                                            </td>
                                                            <td style="height: 18px" align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"Expense") %>
                                                            </td>
                                                            <td style="height: 18px; text-align: right;">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YuSuanRenLi%>"></asp:Label>:
                                                            </td>
                                                            <td style="height: 18px" align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"ManHour") %>
                                                            </td>
                                                            <td style="height: 18px; text-align: right;">
                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ShiJiRenLi%>"></asp:Label>:
                                                            </td>
                                                            <td style="height: 18px" align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"RealManHour") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,YouXianJi%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"Priority") %>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"FinishPercent") %>
                                                            %
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>:
                                                            </td>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container.DataItem,"Status") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>

                                                <ItemStyle CssClass="itemStyle" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <table cellpadding="2" cellspacing="0" class="formBgStyle" style="width: 98%">
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <b>
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XMRWPSGZL%>"></asp:Label>:</b>
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 10pt">
                                                    <td style="width: 100%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>��<asp:TextBox ID="TB_WLName" runat="server" Width="270px"></asp:TextBox>&nbsp;
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��
                                                        <asp:DropDownList ID="DL_WFType" runat="server">
                                                            <asp:ListItem Value="TaskReview" Text="<%$ Resources:lang,RenWuPingSheng%>" />
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;<asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GongZuoLiuMuBan%>"></asp:Label>��<asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName"
                                                            DataValueField="TemName" Width="144px">
                                                        </asp:DropDownList>
                                                        &nbsp; &nbsp;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ChaXunGuanJianZi%>"></asp:Label>��
                                                    <asp:TextBox ID="TB_KeyWord" runat="server" Width="80px"></asp:TextBox>
                                                        <asp:Button ID="BT_Refrash" runat="server" Text="<%$ Resources:lang,ShuaXin%>" OnClick="BT_Refrash_Click" CssClass="inpu" />
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 10pt">
                                                    <td style="width: 100%; height: 51px;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShuoMing%>"></asp:Label>��<asp:TextBox ID="TB_Description" runat="server" TextMode="MultiLine" Width="441px"
                                                            Height="48px"></asp:TextBox>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 10pt">
                                                    <td style="width: 100%;" class="formItemBgStyleForAlignLeft">
                                                        <span style="font-size: 10pt">��<asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXinXi%>"></asp:Label>��<asp:CheckBox ID="CB_Mail" runat="server"
                                                            Text="<%$ Resources:lang,YouJian%>" />
                                                            <asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,DuanXin%>" />�� </span>
                                                        <asp:Button ID="BT_SubmitApply" runat="server" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" />
                                                        <cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True"
                                                            TargetControlID="BT_SubmitApply" PopupControlID="Panel1" BackgroundCssClass="modalBackground" Y="150"
                                                            DynamicServicePath="">
                                                        </cc1:ModalPopupExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <b style="text-align: left">
                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>��</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <table width="90%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td width="7">
                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                    </td>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="6%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="40%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="12%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="6%" align="left">
                                                                    <strong></strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="6" align="right">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px"
                                                ShowHeader="false" AllowPaging="true" PageSize="5" Width="90%" CellPadding="4"
                                                ForeColor="#333333" GridLines="None">
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditItemStyle BackColor="#2461BF" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                <ItemStyle CssClass="itemStyle" />
                                                <Columns>
                                                    <asp:BoundColumn DataField="WLID" HeaderText="Number">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTMyWorkDetailMain.aspx?WLID={0}"
                                                        DataTextField="WLName" HeaderText="Workflow" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="50%" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn DataField="CreateTime" HeaderText="����ʱ��">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="25%" />
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Status">
                                                        <ItemTemplate>
                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=Review&WLID={0}") %>'
                                                                Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder"/></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;"
                        Width="500px">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                            ID="BT_ActiveNo" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Fou%>" OnClick="BT_ActiveNo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
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
