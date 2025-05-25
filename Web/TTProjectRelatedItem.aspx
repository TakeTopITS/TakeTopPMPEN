<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectRelatedItem.aspx.cs" Inherits="TTProjectRelatedItem" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload"
    TagPrefix="Upload" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品资料设置</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1840px;
            width: expression (document.body.clientWidth <= 1840? "1840px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            if (top.location != self.location) { } else { CloseWebPage(); }

        });

        function autoheight() {?//函数：获取尺寸
            //获取浏览器窗口高度
            var winHeight = 0;
            if (window.innerHeight)
                winHeight = window.innerHeight;
            else if ((document.body) && (document.body.clientHeight))
                winHeight = document.body.clientHeight;

            if (document.documentElement && document.documentElement.clientHeight)
                winHeight = document.documentElement.clientHeight;

            document.getElementById("LB_ItemCode").style.height = (winHeight - 120) + "px";
        }

        window.onresize = autoheight;?//浏览器窗口发生变化时同时变化DIV高度
    </script>

    <script type="text/javascript">

        var disPostion = 0;


        function SaveScroll() {
            disPostion = LB_ItemCode.scrollTop;
        }

        function RestoreScroll() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(LB_ItemCodeEndRequestHandler);
        }

        function LB_ItemCodeEndRequestHandler(sender, args) {
            LB_ItemCode.scrollTop = disPostion;
        }

        var disPostion1 = 0;

        function GoodsListIDSaveScroll() {
            disPostion1 = GoodsListDivID.scrollTop;
        }

        function GoodsListIDRestoreScroll() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(GoodsListDivIDEndRequestHandler);
        }


        function GoodsListDivIDEndRequestHandler(sender, args) {
            GoodsListDivID.scrollTop = disPostion1;
        }

        function displayWaitingImg() {

            this.document.getElementById('img_processing').style.display = 'block';
        }

    </script>
</head>
<body onload="autoheight();">
    <form id="form1" runat="server">
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="AboveDiv">
                    <table id="AboveTable" cellpadding="0" cellspacing="0" class="bian" width="100%">
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td height="31" class="page_topbj">
                                            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="350" align="left">
                                                        <table border="0" width="345" align="left" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="29" height="31">
                                                                    <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                                </td>
                                                                <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,WuZiYuShuanBianZhi%>"></asp:Label>
                                                                </td>
                                                                <td width="5">
                                                                    <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="left" width="300px" height="31">
                                                        <asp:HyperLink ID="HL_RelatedItemAndBom" runat="server" NavigateUrl="TTItemDataAndBomSet.aspx" Visible="false"
                                                            Target="_blank" Text="<%$ Resources:lang,WBSJieGouDaiMaYuanShuDingYi%>"></asp:HyperLink>
                                                    </td>
                                                    <td align="left" width="300px" height="31"></td>
                                                    <td align="left">
                                                        <asp:HyperLink ID="HL_ProjectItemBomToPlan" runat="server" NavigateUrl="TTProjectItemBomToPlan.aspx"
                                                            Target="_blank" class="titlezi"  Text="<%$ Resources:lang,WBSJieGouGuiHuaZhuanJiHua%>"></asp:HyperLink>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="300px" valign="top" style="text-align: left; border-right: solid 1px #D8D8D8">
                                                        <table width="300px" class="formBgStyle">
                                                            <tr>
                                                                <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label>
                                                                </td>
                                                                <td style=" width: 30%;" class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_ProjectItemCode" runat="server" Width="91%"></asp:TextBox>
                                                                </td>

                                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%;">
                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_ProjectItemName" runat="server" Width="92%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_ProjectModelNumber" runat="server" Width="91%"></asp:TextBox>
                                                                </td>

                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_ProjectBrand" Width="92%" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,Specification%>"></asp:Label>
                                                                </td>
                                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                                    <asp:TextBox ID="TB_ProjectItemSpec" runat="server" Width="97%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XiangGuan%>"></asp:Label>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:DropDownList ID="DL_RelatedType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RelatedType_SelectedIndexChanged">
                                                                        <asp:ListItem Value="SYSTEM" Text="<%$ Resources:lang,XiTong%>" />
                                                                        <asp:ListItem Value="PROJECT" Text="<%$ Resources:lang,XiangMu%>" />
                                                                        <asp:ListItem Value="OTHER" Text="<%$ Resources:lang,QiTa%>" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbType" runat="server" Text="<%$ Resources:lang,Type%>"></asp:Label>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:DropDownList ID="DL_ProjectItemType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_ProjectItemType_SelectedIndexChanged">
                                                                        <asp:ListItem Value="PurchParts" Text="<%$ Resources:lang,CaiGouJian%>" />
                                                                        <asp:ListItem Value="MadeParts" Text="<%$ Resources:lang,ZiZhiJian%>" />
                                                                        <asp:ListItem Value="OutParts" Text="<%$ Resources:lang,WaiXieJian%>" />
                                                                        <asp:ListItem Value="DelivParts" Text="<%$ Resources:lang,JiaoFuJian%>" />
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="BT_FindItemName" runat="server" CssClass="inpu" Width="40px" OnClick="BT_FindItemName_Click"
                                                                        Text="<%$ Resources:lang,ChaXun%>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:ListBox ID="LB_ItemCode" runat="server" Width="100%" Height="800px" OnSelectedIndexChanged="LB_ItemCode_SelectedIndexChanged"
                                                            DataTextField="ProjectItemName" DataValueField="ItemCode" AutoPostBack="True"></asp:ListBox>
                                                    </td>
                                                    <td style="padding: 5px 5px 0px 5px; text-align: left; vertical-align: top; border-right: solid 1px #D8D8D8">
                                                        <table style="width: 500px;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                            <tr>
                                                                <td style="width: 150px; " class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbCode" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label>:
                                                                </td>
                                                                <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                    <asp:TextBox ID="TB_ItemCode" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbItemName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                    <asp:TextBox ID="TB_ItemName" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                    <asp:TextBox ID="TB_SmallType" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbSpecification" runat="server" Text="<%$ Resources:lang,Specification%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                    <asp:TextBox ID="TB_Specification" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label>：
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                    <asp:TextBox ID="TB_Brand" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbItemType" runat="server" Text="<%$ Resources:lang,DaLei%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                    <asp:DropDownList ID="DL_ItemType" runat="server" CssClass="DDList">
                                                                        <asp:ListItem Value="MadeParts" Text="<%$ Resources:lang,ZiZhiJian%>" />
                                                                        <asp:ListItem Value="PurchParts" Text="<%$ Resources:lang,CaiGouJian%>" />
                                                                        <asp:ListItem Value="OutParts" Text="<%$ Resources:lang,WaiXieJian%>" />
                                                                        <asp:ListItem Value="DelivParts" Text="<%$ Resources:lang,JiaoFuJian%>" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbItemUnit" runat="server" Text="<%$ Resources:lang,Unit%>"></asp:Label>
                                                                    :
                                                                </td>
                                                                <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName" Height="25px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 100px; ">
                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,CaiGouDanJia%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox id="NB_PurchasePrice" runat="server" maxamount="1000000000000" minamount="0" width="79px" precision="3">
                                                                        0 0.00
                                                                            0.000</nicklee:numberbox>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 100px; ">
                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,XiaoShouDanJia%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox id="NB_SalePrice" runat="server" maxamount="1000000000000" minamount="0" width="79px" precision="3">
                                                                        0 0.00
                                                                            0.000</nicklee:numberbox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbPurchaseLeadTime" runat="server" Text="<%$ Resources:lang,PurchaseLeadTime%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox id="NB_PULeadTime" runat="server" maxamount="1000000000000" minamount="0" width="79px">
                                                                        0 0.00
                                                                    </nicklee:numberbox>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; ">
                                                                    <asp:Label ID="LB_tbManufactureLeadTime" runat="server" Text="<%$ Resources:lang,ManufactureLeadTime%>"></asp:Label>
                                                                    : </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox id="NB_MFLeadTime" runat="server" maxamount="1000000000000" minamount="0" width="79px">
                                                                        0 0.00
                                                                    </nicklee:numberbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbLaborCost" runat="server" Text="<%$ Resources:lang,LaborCost%>"></asp:Label>:
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_HRCost" runat="server" minamount="0" width="79px">
                                                                        0 0.00
                                                                    </nicklee:numberbox>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 100px; ">
                                                                    <asp:Label ID="LB_MaterialCost" runat="server" Text="<%$ Resources:lang,MaterialCost%>"></asp:Label>:
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" style="width: 230px; ">
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_MTCost" runat="server" minamount="0" width="79px">
                                                                        0 0.00
                                                                    </nicklee:numberbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbManufactureCost" runat="server" Text="<%$ Resources:lang,ManufactureCost%>"></asp:Label>:
                                                                </td>
                                                                <td style="width: 230px; "  class="formItemBgStyleForAlignLeft">
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_MFCost" runat="server" minamount="0" width="79px">
                                                                        0 0.00
                                                                    </nicklee:numberbox>
                                                                </td>
                                                                <td style="width: 100px; " class="formItemBgStyleForAlignLeft">&nbsp;
                                                                </td>
                                                                <td style="width: 230px; "  class="formItemBgStyleForAlignLeft">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="LB_tbDefaultRouting" runat="server" Text="<%$ Resources:lang,DefaultRouting%>"></asp:Label>:
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                    <asp:TextBox ID="TB_DefaultProcess" runat="server" Width="99%"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td colspan="4" class="formItemBgStyleForAlignLeft" >&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="text-align: left;">&nbsp;
                                                                    <asp:Label ID="Label8" runat="server" Text="BOM"></asp:Label>:
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" class="formItemBgStyleForAlignLeft" >
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="LB_tbTopItemName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>:
                                                                            </td>
                                                                            <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="LB_TopItemCode" runat="server"></asp:Label>
                                                                                &nbsp;
                                                                                 <asp:Label ID="LB_TopItemName" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="LB_tbItemBomCost" runat="server" Text="<%$ Resources:lang,Cost%>"></asp:Label>:
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="LB_ItemBomCost" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="LB_tbTopItemVersion" runat="server" Text="<%$ Resources:lang,Version%>"></asp:Label>:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="DL_VersionID" runat="server" AutoPostBack="True" DataTextField="VerID"
                                                                                    DataValueField="ID" Height="21px" OnSelectedIndexChanged="DL_VersionID_SelectedIndexChanged"
                                                                                    Width="40px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="LB_tbItemBomType" runat="server" Text="<%$ Resources:lang,Type%>"></asp:Label>:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="DL_ChangeVersionType" runat="server" AutoPostBack="True"
                                                                                    Width="50px">
                                                                                    <asp:ListItem Value="InUse" Text="<%$ Resources:lang,ZaiYong%>" />
                                                                                    <asp:ListItem Value="Backup" Text="<%$ Resources:lang,BeiYong%>" />
                                                                                    <asp:ListItem Value="Baseline" Text="<%$ Resources:lang,JiZhun%>" />
                                                                                </asp:DropDownList>
                                                                                <asp:Label ID="LB_TreeDepth" runat="server" Text="-1" Visible="false"></asp:Label>
                                                                            </td>
                                                                            <td colspan="2" style="text-align: center;">
                                                                                <asp:HyperLink ID="HL_ItemRelatedDoc" runat="server" NavigateUrl="TTItemRelatedDoc.aspx" Text="<%$ Resources:lang,XiangGuanWenDang%>"
                                                                                    Target="_blank" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <hr />
                                                                    <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" ShowLines="True" Font-Size="10pt"
                                                                        Font-Bold="False" Font-Names="宋体" Style="width: 99%; height: 100%;">
                                                                        <RootNodeStyle CssClass="rootNode" />
                                                                        <NodeStyle CssClass="treeNode" />
                                                                        <LeafNodeStyle CssClass="leafNode" />
                                                                        <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                                    </asp:TreeView>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="4">

                                                                    <asp:Image ID="IM_ItemPhoto" runat="server" Height="180px" Width="200px" AlternateText="元素照片" />
                                                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                    <td style="width: 600px; padding: 0px 1px 0px 1px; text-align: left; vertical-align: top; border-right: solid 1px #D8D8D8;">
                                                        <table width="600px">
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 1px;">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,YiJiMuLu%>"></asp:Label>
                                                                                ：</td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="TB_FirstDirectory" runat="server" Width="99%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ErJiMuLu%>"></asp:Label>
                                                                                ：</td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="TB_SecondDirectory" runat="server" Width="99%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,SanJiMuLu%>"></asp:Label>
                                                                                ：</td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="TB_ThirdDirectory" runat="server" Width="99%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,SiJiMuLu1%>"></asp:Label>
                                                                                ：</td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="TB_FourthDirectory" runat="server" Width="99%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="LB_tbChildItemName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>
                                                                                ： </td>
                                                                            <td colspan="3">
                                                                                <asp:Label ID="LB_ProjectItemID" runat="server"></asp:Label>
                                                                                <asp:Label ID="LB_ChildItemCode" runat="server"></asp:Label>
                                                                                <asp:Label ID="LB_ChildItemName" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="left" width="10%">
                                                                                <asp:Label ID="LB_tbChildItemBOMVersion" runat="server" Text="<%$ Resources:lang,Version%>"></asp:Label>
                                                                                ： </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="DL_ChildVersionID" runat="server" AutoPostBack="True" DataTextField="VerID" DataValueField="ID" OnSelectedIndexChanged="DL_ChildVersionID_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="LB_tbChildItemNumber" runat="server" Text="<%$ Resources:lang,Number%>"></asp:Label>：
                                                                            </td>
                                                                            <td>
                                                                                <nicklee:numberbox maxamount="1000000000000" id="NB_ChildItemNumberToBom" runat="server" amount="1" minamount="0"
                                                                                    width="70px">
                                                                                    0 0.00
                                                                                    1.00</nicklee:numberbox>
                                                                            </td>
                                                                            <td align="left" width="10%">
                                                                                <asp:Label ID="LB_tbChildItemReservedNumber" runat="server" Text="<%$ Resources:lang,ReservedNumber%>"></asp:Label>:
                                                                            </td>
                                                                            <td>
                                                                                <nicklee:numberbox maxamount="1000000000000" id="NB_ChildItemReservedNumberToBom" runat="server"
                                                                                    minamount="0" width="70px">
                                                                                    0 0.00
                                                                                1.000.00</nicklee:numberbox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="LB_ChildItemUnit" runat="server" Text="<%$ Resources:lang,Unit%>"></asp:Label>：
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="DL_ChildItemUnitToBom" runat="server" DataTextField="UnitName" DataValueField="UnitName">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="LB_tbChildItemProcess" runat="server" Text="<%$ Resources:lang,Process%>"></asp:Label>：
                                                                            </td>
                                                                            <td colspan="5">
                                                                                <asp:TextBox ID="TB_ChildDefaultProcess" runat="server" Width="70%"></asp:TextBox>
                                                                                <asp:DropDownList ID="DL_ProductProcess2" runat="server" AutoPostBack="True" DataTextField="ProcessName"
                                                                                    DataValueField="ProcessName" OnSelectedIndexChanged="DL_ProductProcess2_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" colspan="2" style="padding-top: 6px;">
                                                                                <asp:Button ID="BT_AddToProject" runat="server" CssClass="inpuLong" Enabled="false" OnClick="BT_AddToProject_Click"
                                                                                    Text="<%$ Resources:lang,AddToProject%>" />
                                                                            </td>
                                                                            <td align="left" style="padding-top: 6px;">
                                                                                <asp:Button ID="BT_DeleteToProject" runat="server" CssClass="inpuLong" Enabled="false" OnClick="BT_DeleteToProject_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,Delete%>" />
                                                                            </td>
                                                                            <td align="left" colspan="2" style="padding-top: 6px;">
                                                                                <asp:Button ID="BT_TransferProjectBom" runat="server" CssClass="inpuLong" OnClick="BT_TransferProjectBom_Click"
                                                                                    Text="<%$ Resources:lang,SolidifyToProject%>" />
                                                                            </td>
                                                                            <td align="left" style="padding-top: 5px;">
                                                                                <asp:HyperLink ID="HL_ProjectRelatedItemReport" runat="server" NavigateUrl="TTProjectRelatedItemReport.aspx" Target="_blank" Text="<%$ Resources:lang,BaoBiao%>"> </asp:HyperLink>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"  class="formItemBgStyleForAlignLeft">

                                                                    <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                                        <tr>
                                                                            <td colspan="6" align="left">
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XiangMuGuanLianLiaoPing%>"></asp:Label>
                                                                                （<asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label>:
                                                                                <asp:Label ID="LB_ProjectItemAmount" runat="server"></asp:Label>）

                                                                            </td>
                                                                            <td colspan="5" align="left" style="padding-right: 5px;">

                                                                                <asp:Button ID="BT_CopyFromParentProject" runat="server" CssClass="inpuLong" OnClick="BT_CopyFromParentProject_Click" OnClientClick="window.scrollTo(0, 0);" Text="<%$ Resources:lang,ChongFuXiangMuFuZhi%>" />

                                                                                &nbsp;&nbsp;
                                                                                <asp:Button ID="BT_ExportToExcel" runat="server" CssClass="inpuLong" Text="<%$ Resources:lang,DaoChuExcel%>" OnClick="BT_ExportToExcel_Click" />

                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label>:
                                                                            </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:TextBox ID="TB_FindItemCode" runat="server" Width="80px"></asp:TextBox>
                                                                            </td>

                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>
                                                                                : </td>
                                                                            <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                                <asp:TextBox ID="TB_FindItemName" runat="server" Width="80px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,Specification%>"></asp:Label>
                                                                                : </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:TextBox ID="TB_FindItemSpec" runat="server" Width="80px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>
                                                                                : </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:TextBox ID="TB_FindModelNumber" runat="server" Width="80px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <asp:Button ID="BT_MaterialBudgetFind" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_MaterialBudgetFind_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <div id="GoodsListDivID" style="width: 100%; height: 450px; overflow: auto;">
                                                                        <table width="180%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>&nbsp;
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemID" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="4%"><strong>
                                                                                                <asp:Label ID="Label323" runat="server" Text="<%$ Resources:lang,YiJiMuLu%>"></asp:Label></strong></td>
                                                                                            <td align="left" width="4%"><strong>
                                                                                                <asp:Label ID="Label325" runat="server"  Text="<%$ Resources:lang,ErJiMuLu%>"></asp:Label></strong></td>
                                                                                            <td align="left" width="4%"><strong>
                                                                                                <asp:Label ID="Label328" runat="server"  Text="<%$ Resources:lang,SanJiMuLu%>"></asp:Label></strong></td>
                                                                                            <td align="left" width="4%"><strong>
                                                                                                <asp:Label ID="Label329" runat="server"  Text="<%$ Resources:lang,SiJiMuLu%>"></asp:Label></strong></td>


                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemCode" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="12%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left"><strong>
                                                                                                <asp:Label ID="Label333" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong></td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildBrand" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemNumber" runat="server" Text="<%$ Resources:lang,Number%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YuRuKuLiang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,YiFaHuo%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YuChuKuLiang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,YiXiaoShouLiang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemProcess" runat="server" Text="<%$ Resources:lang,KuChengLiang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="LB_dgChildItemUnit" runat="server" Text="<%$ Resources:lang,Unit%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="7%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <%--  <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="销售单价"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label14" runat="server" Text="销售总额"></asp:Label></strong>
                                                                                            </td>--%>


                                                                                            <td align="left" width="8%"><strong>
                                                                                                <asp:Label ID="Label330" runat="server" Text="<%$ Resources:lang,BeiZhu %>"></asp:Label></strong></td>


                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="left">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid2_ItemCommand"
                                                                            Width="180%" ShowHeader="false">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <ItemStyle CssClass="itemStyle" Horizontalalign="left" />
                                                                            <Columns>
                                                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:ButtonColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="FirstDirectory" HeaderText="FirstLevelDirectory">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SecondDirectory" HeaderText="SecondLevelDirectory">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ThirdDirectory" HeaderText="ThirdLevelDirectory">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="FourthDirectory" HeaderText="FourthLevelDirectory">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ItemCode" HeaderText="Code">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ItemName" HeaderText="Name">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="物料类型">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetItemType(Eval("ItemCode").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <%--    <asp:ButtonColumn CommandName="BOM" Text="&lt;div&gt;&lt;img src=ImagesSkin/BOM.png border=0 width=24 height=24 alt='BOM' /&gt;&lt;/div&gt;">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:ButtonColumn>--%>
                                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadyPurchased" HeaderText="PurchasedQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadyCheckIn" HeaderText="StockedQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadyPick" HeaderText="MaterialIssuedQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AleadySale" HeaderText="SoldQuantity">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="库存量">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="UnitPrice">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Amount">
                                                                                    <ItemTemplate>
                                                                                        <%# decimal.Parse( decimal.Parse( Eval("PurchasePrice").ToString()).ToString("F2")) * decimal.Parse( decimal.Parse(Eval("Number").ToString()).ToString("F2")) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:TemplateColumn>
                                                                                <%-- <asp:BoundColumn DataField="SalePrice" HeaderText="销售单价">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="销售总额">
                                                                                    <ItemTemplate>
                                                                                        <%# decimal.Parse(decimal.Parse( Eval("SalePrice").ToString()).ToString("F2")) * decimal.Parse( decimal.Parse(Eval("Number").ToString()).ToString("F2")) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                </asp:TemplateColumn>--%>


                                                                                <asp:BoundColumn DataField="Comment" HeaderText="备注 ">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                        </asp:DataGrid>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td  class="formItemBgStyleForAlignLeft">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <div>
                                                                                            <upload:inputfile id="FileUpload_Training" runat="server" width="200px" />
                                                                                            &nbsp;<asp:Button ID="btn_ExcelToDataTraining" runat="server" CssClass="inpu" OnClientClick="javascript:displayWaitingImg();" OnClick="btn_ExcelToDataTraining_Click" Text="<%$ Resources:lang,DaoRuShuJu%>" />

                                                                                            <img id="img_processing" src="Images/Processing.gif" alt="Loading,please wait..." style="display: none;" />

                                                                                            <br />
                                                                                            <asp:Label ID="Label70" runat="server"></asp:Label>
                                                                                            <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,SJDRMBGS%>"></asp:Label>
                                                                                            ：<a href="Template/物料定义和项目或合同关联物料数据导入表.xls"><asp:Label ID="Label72" runat="server" Text="项目关联物资导入模板"></asp:Label>
                                                                                            </a>
                                                                                            <div id="ProgressBar">
                                                                                                <upload:progressbar id="ProgressBar1" runat="server" height="100px" width="500px">
                                                                                                </upload:progressbar>
                                                                                            </div>
                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:PostBackTrigger ControlID="btn_ExcelToDataTraining" />
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>

                                                                                <asp:Label ID="Label46" runat="server" Text="注：请确保EXCEL表的标签是否为 Sheet1 " ForeColor="Red"></asp:Label>
                                                                                <br />
                                                                                <asp:Label ID="LB_ErrorText" runat="server" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                            <td align="left">

                                                                                <asp:Button ID="BT_DeleteAllProjectItem" runat="server" CssClass="inpuLong" OnClick="BT_DeleteAllProjectItem_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="删除全部" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 5px 5px 0px 5px; vertical-align: top; border-right: solid 1px #D8D8D8;" class="formItemBgStyleForAlignLeft">
                                                        <table width="500px" cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LB_tbAddVersion" runat="server" Text="<%$ Resources:lang,AddVersion%>"></asp:Label>:
                                                                </td>
                                                                <td align="left">
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_NewProjectItemBomVerID" runat="server" amount="1" minamount="1"
                                                                        precision="0" width="30px">
                                                                        1</nicklee:numberbox>
                                                                </td>
                                                                <td colspan="4" align="left">
                                                                    <asp:Button ID="BT_NewVersion" runat="server" CssClass="inpu" OnClick="BT_NewVersion_Click"
                                                                        Text="<%$ Resources:lang,New%>" />
                                                                    &nbsp;
                                                                <asp:Button ID="BT_DeleteVersion" runat="server" CssClass="inpu"
                                                                    OnClick="BT_DeleteVersion_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,Delete%>" />
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LB_tbItemBomVersion" runat="server" Text="<%$ Resources:lang,Version%>"></asp:Label>:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="DL_ChangeProjectItemBomVersionID" runat="server" AutoPostBack="True"
                                                                        DataTextField="VerID" DataValueField="ID" OnSelectedIndexChanged="DL_ChangeProjecrItemBomVersionID_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LB_tbProjectItemBomType" runat="server" Text="<%$ Resources:lang,Type%>"></asp:Label>:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="DL_ChangeProjectItemBomVersionType" runat="server" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="DL_ChangeProjectItemBomVersionType_SelectedIndexChanged">
                                                                        <asp:ListItem Value="InUse" Text="<%$ Resources:lang,ZaiYong%>" />
                                                                        <asp:ListItem Value="Backup" Text="<%$ Resources:lang,BeiYong%>" />
                                                                        <asp:ListItem Value="Baseline" Text="<%$ Resources:lang,JiZhun%>" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="LB_tbProjectItemBomCost" runat="server" Text="<%$ Resources:lang,Cost%>"></asp:Label>:
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="LB_ProRelatedItemBomCost" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <hr />
                                                        <asp:TreeView ID="TreeView2" runat="server" Font-Bold="False" Font-Names="宋体" Font-Size="10pt"
                                                            NodeWrap="True" ShowLines="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged">
                                                            <RootNodeStyle CssClass="rootNode" />
                                                            <NodeStyle CssClass="treeNode" />
                                                            <LeafNodeStyle CssClass="leafNode" />
                                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                        </asp:TreeView>
                                                    </td>

                                                    <td style="padding: 5px 5px 0px 5px; vertical-align: top;" class="formItemBgStyleForAlignLeft">
                                                        <table style="width: 300px;" cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 25%;">
                                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LB_ItemBomID" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LB_SelectItemCode" runat="server"></asp:Label>
                                                                    <asp:Label ID="LB_SelectItemName" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LB_SelectType" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_SelectModelNumber" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>：
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TB_SelectSpecification" runat="server" Width="99%" Height="80px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_SelectBOMNumber" runat="server" minamount="0" width="79px" precision="5">
                                                                        0 0.00
                                                        0.00000</nicklee:numberbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,YuLiuLiang%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <nicklee:numberbox maxamount="1000000000000" id="NB_SelectBOMReservedNumber" runat="server" minamount="0" width="79px" precision="5">
                                                                        0 0.00
                                                        0.00000</nicklee:numberbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_SelectBOMUnit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                                        Height="25px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,GongYi%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TB_BomDefaultProcess" runat="server" Width="99%" Height="40px"
                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_ProductProcess3" runat="server" AutoPostBack="True" DataTextField="ProcessName"
                                                                        DataValueField="ProcessName" OnSelectedIndexChanged="DL_ProductProcess3_SelectedIndexChanged"
                                                                        Width="150px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>：
                                                                </td>
                                                                <td>
                                                                    <nicklee:numberbox id="NB_SortNumber" runat="server" maxamount="1000000000000" minamount="0" width="79px" precision="0">
                                                                        0 0.00
                                                                        00.000
                                                                    </nicklee:numberbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td style="text-align: left;">
                                                                    <asp:HyperLink ID="HL_ItemBomRelatedDoc" runat="server" NavigateUrl="TTItemRelatedDoc.aspx" Text="<%$ Resources:lang,XiangGuanWenDang%>"
                                                                        Target="_blank" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="LB_SelectGuid" runat="server" Visible="false"></asp:Label>
                                                                    <asp:Label ID="LB_SelectParentGuid" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">&nbsp;<asp:Button ID="BT_UpdateFormBom" runat="server" CssClass="inpu" OnClick="BT_UpdateFormBom_Click"
                                                                    Text="<%$ Resources:lang,BaoCun%>" />
                                                                    &nbsp;<asp:Button ID="BT_DeleteFormBom" runat="server" CssClass="inpu" OnClientClick="return confirm(getDeleteMsgByLangCode())" OnClick="BT_DeleteFormBom_Click"
                                                                        Text="<%$ Resources:lang,ShanChu%>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="layui-layer layui-layer-iframe" id="popParentProjectItemWindow"
                    style="z-index: 9999; width: 980px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                    <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                        <asp:Label ID="Label13" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                    </div>
                    <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                        <div style="width: 100%; text-align: right;">
                            <asp:Button ID="BT_CopyFromParentItem" CssClass="inpu" runat="server" Text="<%$ Resources:lang,FuZhi%>" OnClick="BT_CopyFromParentItem_Click" />
                        </div>
                        <div id="ParentItemListDivID" style="width: 100%; overflow: auto;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                    </td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="4%" align="left">
                                                    <asp:CheckBox ID="CB_SelectAll" runat="server" Text="全选" AutoPostBack="true" OnCheckedChanged="CB_SelectAll_CheckedChanged" />
                                                </td>
                                                <td width="4%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label></strong>
                                                </td>
                                                <td width="14%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                </td>

                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,Number%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,YuRuKuLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label54" runat="server" Text="MaterialIssuedQuantity"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,YuChuKuLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label></strong>
                                                </td>

                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,KuChengLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="5%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,Unit%>"></asp:Label></strong>
                                                </td>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,CaiGouDanJia%>"></asp:Label></strong>
                                                </td>
                                                <td width="7%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label42" runat="server" Text="采购总额"></asp:Label></strong>
                                                </td>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label43" runat="server" Text="销售单价"></asp:Label></strong>
                                                </td>
                                                <td align="left">
                                                    <strong>
                                                        <asp:Label ID="Label44" runat="server" Text="销售总额"></asp:Label></strong>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="left">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                Width="100%" ShowHeader="false">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditItemStyle BackColor="#2461BF" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                <ItemStyle CssClass="itemStyle" Horizontalalign="left" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CB_Select" Checked="false" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ItemCode" HeaderText="Code">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ItemName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="14%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TB_Number" runat="server" Width="95%" Text='<%# Eval("Number").ToString() %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="AleadyPurchased" HeaderText="PurchasedQuantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AleadyCheckIn" HeaderText="StockedQuantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AleadyPick" HeaderText="MaterialIssuedQuantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="库存量">
                                        <ItemTemplate>
                                            <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="PurchasePrice" HeaderText="采购单价">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="采购总额">
                                        <ItemTemplate>
                                            <%# decimal.Parse( decimal.Parse( Eval("PurchasePrice").ToString()).ToString("F2")) * decimal.Parse( decimal.Parse(Eval("Number").ToString()).ToString("F2")) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="SalePrice" HeaderText="销售单价">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="销售总额">
                                        <ItemTemplate>
                                            <%# decimal.Parse(decimal.Parse( Eval("SalePrice").ToString()).ToString("F2")) * decimal.Parse( decimal.Parse(Eval("Number").ToString()).ToString("F2")) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>

                    </div>

                    <div id="popwindow_footer11" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                        <a class="layui-layer-btn notTab" onclick="return popClose();">
                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                    </div>
                    <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                </div>

                <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BT_ExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 50%; top: 50%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="Images/Processing.gif" alt="Loading,please wait..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
