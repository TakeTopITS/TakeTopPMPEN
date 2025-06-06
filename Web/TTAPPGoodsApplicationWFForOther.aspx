<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPGoodsApplicationWFForOther.aspx.cs" Inherits="TTAPPGoodsApplicationWFForOther" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=1" />

<!DOCTYPE html>

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
            /*margin-top: 5px;*/
            /*background-image: url(Images/login_bj.jpg);*/
            background-repeat: repeat-x;
            font: normal 100% Helvetica, Arial, sans-serif;
        }
    </style>

    <style type="text/css">
        #AboveDiv {
            max-width: 1024px;
            width: expression (document.body.clientWidth >= 1024? "1024px" : "auto" ));
            min-width: 277px;
            width: expression (document.body.clientWidth <= 277? "277px" : "auto" ));
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            /* /*  if (top.location != self.location) { } else { CloseWebPage(); }*/*/

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
                                                <%--<a href="javascript:window.history.go(-1)" target ="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';">--%>
                                                <a href="javascript:window.history.go(-1)" target="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';">
                                                    <table width="245" border="0" align="left" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29">
                                                                <img src="ImagesSkin/return.png" alt="" width="29" height="31" /></td>
                                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titleziAPP">
                                                                <asp:Label runat="server" Text="<%$ Resources:lang,Back%>" />
                                                            </td>
                                                            <td width="5">
                                                                <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                                        </tr>
                                                    </table>
                                                    <img id="IMG_Waiting" src="Images/Processing.gif" alt="请稍候，处理中..." style="display: none;" />
                                                </a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding: 5px 5px 0px 5px;">
                                    <asp:Button ID="BT_CreateMain" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_CreateMain_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px" valign="top" align="left">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                        </td>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,FaQiShengQing%>" /></strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong></strong>
                                                        </td>
                                                        <td align="left" width="35%"><strong>
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                        </strong></td>
                                                        <td align="left" width="20%">
                                                            <strong>
                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="20%"><strong>
                                                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                        </strong></td>
                                                        <td align="left">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right" width="6">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid1_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="25" ShowHeader="false" Width="100%">
                                        <Columns>
                                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:ButtonColumn>
                                            <asp:TemplateColumn HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:TemplateColumn>
                                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Assign" Text="&lt;div&gt;&lt;img src=ImagesSkin/Assign.png border=0 alt='Deleted' /&gt;&lt;/div&gt;">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:ButtonColumn>
                                            <asp:BoundColumn DataField="AAID" HeaderText="AAID">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="GAAName" HeaderText="申请名称">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="35%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ApplyTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="申请时间">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Status">
                                                <ItemTemplate>
                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="打印">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                <ItemTemplate>
                                                    <a href='TTGoodsApplicationOrderView.aspx?AAID=<%# DataBinder.Eval(Container.DataItem,"AAID") %>' target="_blank">
                                                        <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle CssClass="notTab" Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />

                                        <ItemStyle CssClass="itemStyle" />
                                    </asp:DataGrid>
                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow" name="fixedDiv"
                        style="z-index: 9999; width: 98%; height: 500px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label111" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 60%; vertical-align: middle;">
                                        <table class="formBgStyle" cellpadding="3" cellspacing="0" style="width: 98%; margin-top: 5px"
                                            align="left">
                                            <tr>
                                                <td style="width: 25%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_AAID" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_GAAName" runat="server" Width="99%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ApplyReason" runat="server" Width="99%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ApplicantCode" runat="server" Width="96px"></asp:TextBox>
                                                    <asp:Label ID="LB_ApplicantName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="NB_TotalAmount" runat="server" Enabled="false" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Width="100px" Precision="3">0.000</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_CurrencyType" runat="server" DataTextField="Type" DataValueField="Type">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_ApplyTime" runat="server" ReadOnly="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_ApplyTime">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YaoQiuWanChengShiJian%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_FinishTime" runat="server" ReadOnly="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_FinishTime">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" OnSelectedIndexChanged="DL_Status_SelectedIndexChanged">
                                                        <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                                        <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                                        <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                                        <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <table style="display: none;">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,LaiYuan%>"></asp:Label>：
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DL_SourceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_SourceType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                                </asp:DropDownList></td>
                                                            <td>ID:
                                                            </td>
                                                            <td>
                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_SourceID" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                                    PositiveColor="" Precision="0" Width="35px">0</NickLee:NumberBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="padding-bottom: 5px;">
                                        <asp:Button ID="BT_CreateDetail" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_CreateDetail_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="110%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                            <tr>

                                                <td width="7">

                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                <td>

                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                        <tr>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                            </td>



                                                            <td align="left" width="5%"><strong></td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,LiaoPinDaiMa %>"></asp:Label></strong> </td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,LiaoPinMingCheng %>"></asp:Label></strong> </td>
                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong> </td>

                                                            <td width="15%" align="left"><strong>

                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong> </td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong> </td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label></strong> </td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YiChuKu %>"></asp:Label></strong> </td>

                                                            <td width="10%" align="left"><strong>

                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong> </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td width="6" align="right">

                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="30px"
                                            OnItemCommand="DataGrid2_ItemCommand" Width="110%" CellPadding="4" ShowHeader="False"
                                            ForeColor="#333333" GridLines="None">
                                            <Columns>

                                                <asp:ButtonColumn CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:ButtonColumn>
                                                <asp:TemplateColumn HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="GoodsCode" HeaderText="物料代码">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="GoodsName" HeaderText="物料名">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">

                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="Amount" HeaderText="Amount">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="CheckOutNumber" HeaderText="出库">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                            </Columns>

                                            <EditItemStyle BackColor="#2461BF" />

                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                            <ItemStyle CssClass="itemStyle" />

                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        </asp:DataGrid>

                                        <asp:Label ID="LB_ID" runat="server" Visible="False"></asp:Label>
                                    </td>

                                </tr>
                            </table>
                        </div>
                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewMain" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewMain_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label112" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popDetailWindow" name="fixedDiv"
                        style="z-index: 9999; width: 98%; height: 500px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2">
                            <asp:Label ID="Label113" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content2" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 60%; border-right: solid 1px #D8D8D8;" valign="top">
                                        <table width="98%" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" colspan="2" style="height: 6px">
                                                    <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                                        <cc1:TabPanel ID="TabPanel1" runat="server">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShenQingDanMingXi%>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table class="formBgStyle" style="width: 100%;" cellpadding="3" cellspacing="0">
                                                                                <tr style="display: none;">
                                                                                    <td style="display: none;" class="formItemBgStyleForAlignLeft">
                                                                                        <table style="display: none;">
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:Label ID="LB_DetailID" runat="server"></asp:Label></td>
                                                                                                <td style="width: 100px; text-align: right">
                                                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,LaiYuan %>"></asp:Label>：</td>
                                                                                                <td align="left">
                                                                                                    <asp:DropDownList ID="DL_RecordSourceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RecordSourceType_SelectedIndexChanged">
                                                                                                          <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                                                                    </asp:DropDownList></td>
                                                                                                <td align="right">ID:</td>
                                                                                                <td align="left">
                                                                                                    <NickLee:NumberBox ID="NB_RecordSourceID" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="0" Width="30px">0</NickLee:NumberBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Label ID="LB_SourceRelatedID" runat="server" Visible="False" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 25%; " class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>
                                                                                    </td>
                                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                                        <asp:DropDownList ID="DL_GoodsType" runat="server" DataTextField="Type" DataValueField="Type"></asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,LiaoPinDaiMa %>"></asp:Label>

                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                                                                        <asp:TextBox ID="TB_GoodsCode" runat="server" Width="99%"></asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LiaoPinMingCheng %>"></asp:Label>

                                                                                    </td>
                                                                                    <td style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                                                                        <asp:TextBox ID="TB_GoodsName" runat="server" Width="99%"></asp:TextBox>

                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>

                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:TextBox ID="TB_ModelNumber" runat="server" Width="99%"></asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>

                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:TextBox ID="TB_Spec" runat="server" Height="50px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:TextBox ID="TB_Brand" runat="server" Width="99%"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft" colspan="2">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="BT_Clear" runat="server" CssClass="inpu" Width="50px" OnClick="BT_Clear_Click" Text="<%$ Resources:lang,QingKong %>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" Width="50px" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun %>" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>

                                                                                    </td>
                                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" Width="85px" OnBlur="" OnFocus=""
                                                                                            OnKeyPress="" PositiveColor="" Precision="3">0.000</NickLee:NumberBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>

                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <NickLee:NumberBox ID="NB_Price" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Width="85px" Precision="3">0.000</NickLee:NumberBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>

                                                                                    </td>
                                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                                        <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
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
                                    <td style="width: 40%; border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px;"
                                        valign="top" align="left">
                                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoLingYongDeLiaoPin%>"></asp:Label>：<asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_GoodsOwner" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="0"
                                            Width="100%">
                                            <cc1:TabPanel ID="TabPanel8" runat="server">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,LiaoPinKuCunLieBiao%>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoXiaoShouDeLiaoPin %>"></asp:Label>
                                                    ：<asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="150%">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                </td>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="left" width="12%"><strong>
                                                                                <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="16%"><strong>
                                                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="10%"><strong>
                                                                                <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="24%"><strong>
                                                                                <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                                            </strong></td>

                                                                            <td align="left" width="10%"><strong>
                                                                                <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="6%"><strong>
                                                                                <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="6%"><strong>
                                                                                <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="6%"><strong>
                                                                                <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>
                                                                            </strong></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="right" width="6">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid3_ItemCommand" ShowHeader="False" Width="150%">
                                                            <Columns>

                                                                <asp:TemplateColumn HeaderText="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="24%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="TotalNumber" HeaderText="Quantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <EditItemStyle BackColor="#2461BF" />
                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <PagerStyle CssClass="notTab" Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                        </asp:DataGrid>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel6" runat="server" TabIndex="1">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,LPCXLB%>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoXRuKuDeLiaoPin %>"></asp:Label>：
                                                    <div id="Div2" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="150%">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                </td>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="left" width="15%"><strong>
                                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="20%"><strong>
                                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left" width="10%"><strong>
                                                                                <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="15%" align="left"><strong>
                                                                                <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                            <td align="left" width="35%"><strong>
                                                                                <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                                            </strong></td>

                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="right" width="6">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid9" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid9_ItemCommand" ShowHeader="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="BT_ItemCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ItemCode").ToString().Trim() %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                </asp:TemplateColumn>
                                                                <asp:HyperLinkColumn DataNavigateUrlField="ItemCode" DataNavigateUrlFormatString="TTItemInforView.aspx?ItemCode={0}" DataTextField="ItemName" HeaderText="Name" Target="_blank">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                </asp:HyperLinkColumn>
                                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Specification" HeaderText="Specification">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="35%" />
                                                                </asp:BoundColumn>

                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <EditItemStyle BackColor="#2461BF" />
                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <PagerStyle CssClass="notTab" Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                        </asp:DataGrid>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer1" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewDetail" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewDetail_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label114" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>
                    <div class="layui-layer layui-layer-iframe" id="popAssignWindow" name="noConfirm"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label103" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,GongZuoLiuDingYi%>"></asp:Label>

                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 100px; height: 6px; text-align: right;">
                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,LiuChengMuBan%>"></asp:Label>： </td>
                                    <td align="left" style="width: 550px; height: 6px">
                                        <asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                            Width="194px">
                                        </asp:DropDownList><asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：<asp:DropDownList ID="DL_WFType" runat="server">
                                            <asp:ListItem Value="MaterialWithdrawal" Text="<%$ Resources:lang,LiaoPingLingYong%>" />
                                        </asp:DropDownList><asp:HyperLink ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                            Target="_blank">
                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,MoBanWeiHu%>"></asp:Label>
                                        </asp:HyperLink><asp:Button ID="BT_Reflash" runat="server" OnClick="BT_Reflash_Click" Text="<%$ Resources:lang,ShuaXin%>" CssClass="inpu" /></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 6px; text-align: right;">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" style="width: 550px; height: 27px"><span style="font-size: 10pt">（<asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXingXi%>"></asp:Label>：</span><asp:CheckBox ID="CB_SMS" runat="server"
                                                    Font-Size="10pt" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt">) </span>
                                                    <asp:Button ID="BT_SubmitApply" runat="server" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" /><cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True"
                                                        TargetControlID="BT_SubmitApply" PopupControlID="Panel1" BackgroundCssClass="modalBackground" Y="150"
                                                        DynamicServicePath="">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label><asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 22px; text-align: left;">
                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>： </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                        <td width="50%" align="left"><strong>
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong> </td>
                                                                        <td width="20%" align="left"><strong>
                                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px"
                                                        PageSize="5" Width="98%" CellPadding="4" ShowHeader="False" ForeColor="#333333"
                                                        GridLines="None">
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
                                                            <asp:BoundColumn DataField="CreateTime" HeaderText="申请时间">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                                        Target="_blank"><img  class="noBorder" src="ImagesSkin/Doc.gif"/></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                            </table>


                        </div>

                        <div id="popwindow_footer11" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label115" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>



                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;"
                        Width="500px">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
