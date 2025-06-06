<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTQuestionToCustomer.aspx.cs" Inherits="TTQuestionToCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1100px;
            width: expression (document.body.clientWidth <= 1100? "1100px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GLHZCKHDA%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td align="left">（<asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_CustCode" runat="server" Width="120px"></asp:TextBox>
                                                        </td>

                                                        <td align="left">
                                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_CustName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>

                                                        <td align="left">
                                                            <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_IndustryTypeFind" runat="server" Width="120px"></asp:TextBox>
                                                            <asp:DropDownList ID="DL_IndustryTypeFind" runat="server" AutoPostBack="True" DataTextField="Type"
                                                                DataValueField="Type" OnSelectedIndexChanged="DL_IndustryTypeFind_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_ContactPerson" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,FenGuanDaiLiShang%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_AgencyName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_AreaAddress" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,ShengFen%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,ChengShi%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBox2" runat="server" Width="120px"></asp:TextBox>
                                                        </td>

                                                        <td>
                                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Find_Click" />）
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
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 250px; border-right: solid 1px #D8D8D8; padding: 10px 5px 5px 5px"
                                                valign="top" align="left">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,YiGuanLianKeHu%>"></asp:Label>
                                                <table width="250" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="40%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="60%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="false" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                                    Width="250px" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_CustomerCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerCode") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="CustomerName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="60%" />
                                                        </asp:BoundColumn>
                                                    </Columns>

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                </asp:DataGrid>

                                                <br />

                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,WeiGuanLianKeHu%>"></asp:Label>
                                                <table width="250" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="40%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="60%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    ShowHeader="false" Height="1px" OnItemCommand="DataGrid2_ItemCommand" PageSize="25"
                                                    OnPageIndexChanged="DataGrid2_PageIndexChanged" Width="250px" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_CustomerCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerCode") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="CustomerName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="60%" />
                                                        </asp:BoundColumn>
                                                    </Columns>

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                </asp:DataGrid>

                                            </td>
                                            <td rowspan="2" style="padding: 10px 5px 5px 5px;" valign="top">
                                                <table style="width: 99%; text-align: center;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td  width="10%" class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,FuWuMiaoShu%>"></asp:Label>：</td>
                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_QuestionID" runat="server"></asp:Label>
                                                            <asp:Label ID="LB_QuestionName" runat="server"></asp:Label>
                                                        </td>
                                                        <td colspan="2" class="formItemBgStyleForAlignLeft" >
                                                            <asp:Button ID="BT_RelatedCustomer" runat="server" CssClass="inpuLong" Enabled="false" OnClick="BT_RelatedCustomer_Click" Text="<%$ Resources:lang,GuanLianCiKeHu%>" />
                                                            &nbsp;
                                                                <asp:Button ID="BT_CancelRelatedCustomer" runat="server" CssClass="inpuLong" Enabled="false" OnClick="BT_CancelRelatedCustomer_Click" Text="<%$ Resources:lang,QuXiaoGuanLianCiKeHu%>" />
                                                        </td>


                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,KeHuDaiMa%>"></asp:Label>： </td>
                                                        <td class="formItemBgStyleForAlignLeft" width="35%">
                                                            <asp:TextBox ID="TB_CustomerCode" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft"  width="10%">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>： </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_CustomerName" runat="server" Style="margin-left: 0px" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>


                                                        <td  width="15%" class="formItemBgStyleForAlignLeft">


                                                            <asp:Label ID="Label190" runat="server" Text="<%$ Resources:lang,JianCheng %>"></asp:Label>： 

                                                        </td>


                                                        <td  width="30%" class="formItemBgStyleForAlignLeft">


                                                            <asp:TextBox ID="TB_SimpleName" runat="server" Width="200px"></asp:TextBox>

                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YingWenMing%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_CustomerEnglishName" runat="server" Height="20px" Width="95%"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,HangYeLeiXing%>"></asp:Label>：
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Type" runat="server" Width="120px"></asp:TextBox>
                                                            <asp:DropDownList ID="DL_IndustryType" runat="server" AutoPostBack="True"
                                                                DataTextField="Type" DataValueField="Type" OnSelectedIndexChanged="DL_IndustryType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,KaiHuYinHang%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Bank" runat="server" Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,YinHangZhangHao%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_BankAccount" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,FaPiaoDiZhi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_InvoiceAddress" TextMode="MultiLine" Height="60" runat="server"
                                                                Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhuYaoLianXiRen%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_ContactName" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,DianHua1%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Tel1" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,DianHua2%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Tel2" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">EMail：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_EMailAddress" runat="server" Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,YouZhengBianMa%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_ZP" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,WangZhi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_WebSite" runat="server" Width="99%"></asp:TextBox>
                                                        </td>

                                                        <td class="formItemBgStyleForAlignLeft">


                                                            <asp:Label ID="Label192" runat="server" Text="<%$ Resources:lang,BanGongWangZi %>"></asp:Label>： 

                                                        </td>


                                                        <td class="formItemBgStyleForAlignLeft">


                                                            <asp:TextBox ID="TB_WorkSiteURL" runat="server" Width="99%"></asp:TextBox>


                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ChuanZhen%>"></asp:Label>：
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Fax" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,YeWuYuan%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_SalePerson" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_Currency" runat="server">
                                                                <asp:ListItem Value="Renminbi" Text="<%$ Resources:lang,RenMinBi%>" />
                                                                <asp:ListItem Value="UsDollar" Text="<%$ Resources:lang,MeiYuan%>" />
                                                                <asp:ListItem Value="Euro" Text="<%$ Resources:lang,OuYuan%>" />
                                                                <asp:ListItem Value="HongKongDollar" Text="<%$ Resources:lang,GangBi%>" />
                                                                <asp:ListItem Value="NewTaiwanDollar" Text="<%$ Resources:lang,TaiBi%>" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZheKouLv%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Discount" runat="server" Height="23px" Width="44px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;0.00
                                                            </NickLee:NumberBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,XinYongDengJi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_CreditRate" runat="server" Height="22px" Width="44px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.00&nbsp;&nbsp;&nbsp;&nbsp;0.00
                                                            </NickLee:NumberBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,GuoJia%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Country" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShengFen%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_State" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ChengShi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_City" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,JianLiShiJian%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_CreateDate" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,XiangXiDiZhi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_AddressCN" Height="60px" TextMode="MultiLine" runat="server"
                                                                Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,YingWenDiZhi%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_AddressEN" runat="server" Height="60" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,GuiShuBuMen %>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft" >
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LB_BelongDepartCode" runat="server"></asp:Label>
                                                                        <asp:Label ID="LB_BelongDepartName" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BT_BelongDepartment" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe %>" />
                                                                        <cc1:ModalPopupExtender ID="BT_BelongDepartment_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="IMBT_CloseTree3" DynamicServicePath="" Enabled="True" PopupControlID="Panel3" TargetControlID="BT_BelongDepartment" Y="150">
                                                                        </cc1:ModalPopupExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label154" runat="server" Text="<%$ Resources:lang,FenGuanDaiLiShang %>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" >
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LB_BelongAgencyCode" runat="server"></asp:Label>
                                                                        <asp:Label ID="LB_BelongAgencyName" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BT_BelongAgency" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe %>" OnClick="BT_BelongAgency_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>：
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_Comment" runat="server" Height="60px" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:Button ID="BT_Add" runat="server" CssClass="inpu" OnClick="BT_Add_Click"
                                                                Text="<%$ Resources:lang,XinZeng%>" />
                                                            &nbsp;
                                                        <asp:Button ID="BT_Update" runat="server" CssClass="inpu" OnClick="BT_Update_Click"
                                                            Text="<%$ Resources:lang,BaoCun%>" Enabled="False" />
                                                            &nbsp;
                                                        <asp:Button ID="BT_Delete" runat="server" CssClass="inpu" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                                            Text="<%$ Resources:lang,ShanChu%>" Enabled="False" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:HyperLink ID="HL_RelatedContactInfor"
                                                                runat="server" Enabled="false" Target="_blank">
                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,XiangGuanLianXiRen%>"></asp:Label>
                                                            </asp:HyperLink>
                                                            &nbsp;&nbsp;<asp:HyperLink ID="HL_TransferProject" runat="server" Enabled="false" Target="_blank">---&gt;<asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ZhuanChengXiangMu%>"></asp:Label></asp:HyperLink>
                                                            &nbsp;
                                                            <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table width="99%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="相关项目" TabIndex="0">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,XiangGuanXiangMu%>"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,XiangMuHao %>"></asp:Label>：
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="TB_ProjectID" runat="server" Width="150px"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Button ID="BT_RelatedProject" runat="server" CssClass="inpu" OnClick="BT_RelatedProject_Click"
                                                                                                    Text="<%$ Resources:lang,GuanLian %>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
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
                                                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="20%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="15%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,KaiShiRiQi %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="15%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,JieShuRiQi %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="15%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,LiXiangRiQi %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="9%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,WanChengChengDu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="8%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,ShanChu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                            ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid4_ItemCommand"
                                                                            ShowHeader="False" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ProjectID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProjectName" HeaderText="项目名称">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="开始日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="结束日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="立项日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="完成程度">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="LB_FinishPercent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinishPercent")%> '></asp:Label>
                                                                                        %
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:ButtonColumn CommandName="Delete" Text="&lt;div&gt;&lt;img src=ImagesSkin/icon_del.gif border=0 alt='Deleted' /&gt;&lt;/div&gt;">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:ButtonColumn>
                                                                            </Columns>

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="VisiblePersonnel" TabIndex="0">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,KeShiRenYuan%>"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <table width="900px" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="width: 220px; border-right: solid 1px #D8D8D8; padding: 5px 0px 0px 5px"
                                                                                    valign="top" align="left">
                                                                                    <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                                                                        ShowLines="True" Width="220px">
                                                                                        <RootNodeStyle CssClass="rootNode" />
                                                                                        <NodeStyle CssClass="treeNode" />
                                                                                        <LeafNodeStyle CssClass="leafNode" />
                                                                                        <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                                                    </asp:TreeView>
                                                                                </td>
                                                                                <td width="165px" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">
                                                                                    <table style="width: 165px; height: 53px">
                                                                                        <tr>
                                                                                            <td style="width: 165; text-align: center;" valign="top">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                                    <tr>
                                                                                                        <td width="7">
                                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,BuMenRenYuan%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td width="6" align="right">
                                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid3_ItemCommand"
                                                                                                    Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                                                    ShowHeader="False">
                                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                                    <ItemStyle CssClass="itemStyle" />
                                                                                                    <Columns>
                                                                                                        <asp:TemplateColumn HeaderText="部门人员：">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Button ID="BT_UserCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>'
                                                                                                                    CssClass="inpu" />
                                                                                                                <asp:Button ID="BT_UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                                                                    CssClass="inpu" />
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                                                                                Font-Strikeout="False" Font-Underline="False" />
                                                                                                        </asp:TemplateColumn>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="500px" align="left" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">
                                                                                    <asp:Repeater ID="RP_CustomerMember" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                                                        <ItemTemplate>
                                                                                            <asp:Button ID="BT_UserName" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                                                Width="70px" />
                                                                                        </ItemTemplate>
                                                                                    </asp:Repeater>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                            </cc1:TabContainer>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="LB_DepartString" runat="server" Visible="false"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="layui-layer layui-layer-iframe" id="popFindAgencyWindow" name="fixedDiv"
                style="z-index: 9999; width: 720px; height: 630px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                <div class="layui-layer-title" style="background: #e7e7e8; text-align: center;" id="popwindow_title">
                    <asp:Label ID="Label175" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                </div>
                <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                    <table>
                        <tr>
                            <td style="width: 900px; padding: 5px 5px 5px 5px;" valign="top" align="left">

                                <table>
                                    <tr>
                                        <td align="left">（<asp:Label ID="Label178" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_FindAgencyCode" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td align="left">
                                            <asp:Label ID="Label179" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_FindAgencyName" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td align="left">
                                            <asp:Label ID="Label180" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_FindAgencyIndustryType" runat="server" Width="120px"></asp:TextBox>
                                            <asp:DropDownList ID="DL_FindAgencyIndustryType" runat="server" AutoPostBack="True" DataTextField="Type"
                                                DataValueField="Type" OnSelectedIndexChanged="DL_FindAgencyIndustryType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label181" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TB_AgencyContactPerson" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="BT_FindAgency" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_FindAgency_Click" />）
                                        </td>
                                    </tr>
                                </table>

                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                    <tr>
                                        <td width="7">
                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="8%" align="left"><strong></strong></td>
                                                    <td width="10%" align="left"><strong>
                                                        <asp:Label ID="Label163" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                    </strong></td>
                                                    <td width="18%" align="left"><strong>
                                                        <asp:Label ID="Label171" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                    </strong></td>
                                                    <td width="6%" align="left"><strong>
                                                        <asp:Label ID="Label172" runat="server" Text="<%$ Resources:lang,ChuangJianRen %>"></asp:Label>
                                                    </strong></td>
                                                    <td width="8%" align="left"><strong>
                                                        <asp:Label ID="Label173" runat="server" Text="<%$ Resources:lang,LianXiRen %>"></asp:Label>
                                                    </strong></td>
                                                    <td width="10%" align="left"><strong>
                                                        <asp:Label ID="Label174" runat="server" Text="<%$ Resources:lang,DianHua %>"></asp:Label>
                                                    </strong></td>
                                                    <td width="10%" align="left"><strong>EMail</strong> </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td width="6" align="right">
                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                    </tr>
                                </table>
                                <asp:DataGrid ID="DataGrid15" runat="server" AutoGenerateColumns="False" Height="1px" AllowPaging="True" PageSize="30" OnItemCommand="DataGrid15_ItemCommand" OnPageIndexChanged="DataGrid15_PageIndexChanged"
                                    ShowHeader="False" Width="100%" GridLines="None" CellPadding="4" ForeColor="#333333">
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button ID="BT_Select" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZe%>" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemBorder" Width="8%" />
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="CustomerCode" HeaderText="Code">
                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="CustomerCode" DataNavigateUrlFormatString="TTCustomerInfoView.aspx?CustomerCode={0}"
                                            DataTextField="CustomerName" HeaderText="CustomerName" Target="_blank">
                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                        </asp:HyperLinkColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforView.aspx?UserCode={0}"
                                            DataTextField="CreatorName" HeaderText="创建人" Target="_blank">
                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                        </asp:HyperLinkColumn>
                                        <asp:BoundColumn DataField="ContactName" HeaderText="联系人">
                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Tel1" HeaderText="Telephone">
                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EmailAddress" HeaderText="EMail">
                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundColumn>

                                    </Columns>

                                    <EditItemStyle BackColor="#2461BF" />

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <ItemStyle CssClass="itemStyle" />

                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                </asp:DataGrid>
                            </td>
                            <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                <asp:ImageButton ID="IMBT_CloseTree4" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="LB_Sql15" runat="server" Visible="false"></asp:Label>

                </div>

                <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                    <a class="layui-layer-btn notTab" onclick="return popCloseNoPromt();">
                        <asp:Label ID="Label176" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                </div>
                <span class="layui-layer-setwin"><a onclick="return popCloseNoPromt();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
            </div>

            <%--            <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>--%>

            <asp:Label ID="LB_Sql5" runat="server" Visible="false"></asp:Label>



            <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none;">
                <div class="modalPopup-text" style="width: 420px; height: 400px; overflow: auto;">
                    <table>
                        <tr>
                            <td style="width: 360px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                <asp:TreeView ID="TreeView3" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView3_SelectedNodeChanged"
                                    ShowLines="True" Width="220px">
                                    <RootNodeStyle CssClass="rootNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                </asp:TreeView>
                            </td>
                            <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                <asp:ImageButton ID="IMBT_CloseTree3" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

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
