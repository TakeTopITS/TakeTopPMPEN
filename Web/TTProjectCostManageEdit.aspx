<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectCostManageEdit.aspx.cs" Inherits="TTProjectCostManageEdit" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });
    </script>
    <style type="text/css">
        .auto-style1 {
            background-color: #F6FAFD;
            background-repeat: no-repeat;
            width: 285px;
        }

        .auto-style2 {
            background: #f37c00;
            width: 71px;
            margin: 8px 0;
            border-radius: 5px;
            text-align: center;
            color: #FDFCFC;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuChengBenYuSuanGuanLi%>"></asp:Label></td>
                                                    <td width="5">
                                                        <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
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
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                    <tr>
                                        <td align="left">
                                            <table cellpadding="0" cellspacing="0" class="formBgStyle">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ChengBenXinXi%>"></asp:Label>：
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TB_RemarkQuery" runat="server" Width="250px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                    <tr>
                                        <td   class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XMCBLB%>"></asp:Label>：
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                 
                                                                <td width="5%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,ID%>" /></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BianMa%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="20%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ChengBenLeiXing%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GongChengLiang%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZongJia%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ChuangJianRen%>"></asp:Label></strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px"
                                                OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ForeColor="#333333"
                                                GridLines="None" ShowHeader="false" AllowPaging="true" PageSize="25" OnPageIndexChanged="DataGrid2_PageIndexChanged">

                                                <ItemStyle CssClass="itemStyle" />
                                                <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
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
                                                    <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Code" HeaderText="编码">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Name" HeaderText="定额项目">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Quantities" HeaderText="工程量">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Total" HeaderText="总价">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="创建人">
                                                        <ItemTemplate>
                                                            <%#GetUserName(Eval("Creater").ToString()) %>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditItemStyle BackColor="#2461BF" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius:10px;">
                        <div class="layui-layer-title"  style="background:#e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label19" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding :0px 5px 0px 5px;">
                            <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                <tr style="display:none;">
                                    <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>：
                                    </td>
                                    <td  colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianMa%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Code" runat="server" Width="180px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChengBenLeiXing%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Name" runat="server" CssClass="shuru" Width="180px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GongChengLiang%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="1">
                                        <NickLee:NumberBox ID="NB_Quantities" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" Precision="6">0.000000</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Unit" runat="server" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="1">
                                        <NickLee:NumberBox ID="NB_Price" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000">0.00</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZongJia%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <NickLee:NumberBox ID="NB_Total" runat="server" Enabled="False" MaxAmount="1000000000000" MinAmount="-1000000000000">0.00</NickLee:NumberBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BeiZhuShuoMing%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:TextBox ID="TB_Remark" runat="server" CssClass="shuru" Width="90%" Height="37px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width: 150px;" class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="2"></td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>

                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>


                    <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>

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
