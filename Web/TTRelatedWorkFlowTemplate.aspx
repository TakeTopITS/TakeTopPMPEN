<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRelatedWorkFlowTemplate.aspx.cs"
    Inherits="TTRelatedWorkFlowTemplate" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1240px;
            width: expression (document.body.clientWidth <= 1240? "1240px" : "auto" ));
        }

        .ButtonCss {
            height: 26px;
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
                        <table id="Table1" cellpadding="0" width="100%" cellspacing="0" class="bian">

                            <tr>
                                <td>
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
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongZuoLiuMuBan%>"></asp:Label>
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
                                                <table style="width: 100%; margin-top: 5px" cellpadding="2" cellspacing="0">
                                                    <tr style="font-size: 12pt">
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <span style="font-size: 11pt">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,QingShuRuGongZuoLiuMuBanMing%>"></asp:Label></span>：<asp:TextBox ID="TB_WFTemName" runat="server"
                                                                    Font-Size="10pt" Width="250px"></asp:TextBox>
                                                            <span style="font-size: 11pt">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：</span><asp:DropDownList ID="DL_WLType" runat="server"
                                                                    DataTextField="Type" DataValueField="Type" AutoPostBack="true"
                                                                    Height="20px" OnSelectedIndexChanged="DL_WLType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            <asp:Button ID="BT_CreateWorkFlowTemplate" runat="server" Font-Size="10pt" OnClick="BT_CreateWorkFlowTemplate_Click"
                                                                CssClass="inpuLong" Text="<%$ Resources:lang,ChuangJianGongZuoLiuMuBan%>" /><span style="font-size: 11pt"> </span>
                                                        </td>
                                                    </tr>
                                                    <tr style="font-weight: bold; font-size: 11pt">
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_WorkFlowName" runat="server" CssClass="inpuLongRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"TemName") %>' />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <table style="border: 1px dotted grey;">
                                                                <tr>
                                                                    <td align="left">
                                                                        <b>
                                                                            <asp:Label ID="LB_WFTemplate" runat="server" Font-Size="10pt"></asp:Label>
                                                                        </b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YXSPGCZBJSJ%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_EnableEdit" runat="server" AutoPostBack="True" Enabled="False"
                                                                                        OnSelectedIndexChanged="DL_EnableEdit_SelectedIndexChanged">
                                                                                        <asp:ListItem Value="YES" Text="<%$ Resources:lang,Shi%>" />
                                                                                        <asp:ListItem Value="NO" Text="<%$ Resources:lang,Fou%>" />
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShunXuHao%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox ID="NB_SortNumber" runat="server" MaxAmount="1000000000000" MinAmount="-100" Width="20px" Precision="0">00</NickLee:NumberBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="BT_SaveBelongDepartment" runat="server" CssClass="inpu" Enabled="false"
                                                                                        Text="<%$ Resources:lang,BaoCun%>" OnClick="BT_SaveBelongDepartment_Click" />
                                                                                </td>

                                                                                <td>
                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：<asp:Label ID="LB_WFType" runat="server"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ChuangJianZhe%>"></asp:Label>：</td>
                                                                                <td style="text-decoration: none;">
                                                                                    <asp:HyperLink ID="HL_Creator" runat="server" Target="_blank" Style="text-decoration: none;"></asp:HyperLink>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                    <br />
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:HyperLink ID="HL_WorkFlowDesigner" runat="server" Enabled="false">---&gt;<asp:Label ID="LB_DesignWorkflowTemplate" runat="server" Text="<%$ Resources:lang,SheJiChigongZuoLiuMoBan%>"></asp:Label></asp:HyperLink></td>
                                                                                            <td width="200px">&nbsp;</td>
                                                                                            <td>
                                                                                                <asp:Button ID="BT_DeleteWFTemplate" runat="server" CssClass="inpu" Enabled="false"
                                                                                                    OnClick="BT_DeleteWFTemplate_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChuMuBan%>" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <br />
                                                                                    <br />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="text-align: left">
                                                                                    <span>
                                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ZDYBDGZLSQSCINFOPATHBDZWBDNB%>"></asp:Label></span>
                                                                                    <br />
                                                                                    <asp:HyperLink ID="HL_XSNFile" Text="Template File" runat="server"></asp:HyperLink>
                                                                                </td>
                                                                                <td style="padding-left: 10px; text-align: left">
                                                                                    <span>
                                                                                        <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,CYGZLYMGL%>"></asp:Label>
                                                                                        ：</span></td>
                                                                            </tr>
                                                                            <tr style="font-weight: bold;">
                                                                                <td style="height: 14px; text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                        <Triggers>
                                                                                            <asp:PostBackTrigger ControlID="BT_UploadXSNFile" />
                                                                                        </Triggers>
                                                                                        <ContentTemplate>
                                                                                            <asp:FileUpload ID="FUP_File" runat="server" Width="250px" />
                                                                                            <asp:Button ID="BT_UploadXSNFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_UploadXSNFile_Click"
                                                                                                CssClass="inpu" Enabled="False" />
                                                                                            <asp:Button ID="BT_DeleteXSNFile" runat="server" OnClick="BT_DeleteXSNFile_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                                                                                CssClass="inpu" Enabled="False" Text="<%$ Resources:lang,ShanChu%>" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="padding-left: 10px; text-align: left">
                                                                                    <asp:DropDownList ID="DL_WFRelatedPage" runat="server" AutoPostBack="true" DataTextField="FormName" DataValueField="PageName" OnSelectedIndexChanged="DL_WFRelatedPage_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td align="left" colspan="2">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,FZMBXMBMC%>"></asp:Label>
                                                                                                ： </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="TB_NewTemName" runat="server" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Button ID="BT_CopyTem" runat="server" CssClass="inpu" Enabled="false" OnClick="BT_CopyTem_Click" Text="<%$ Resources:lang,FuZhi%>" />
                                                                                            </td>
                                                                                            <td style="width: 100px;">&nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,LeiXingBianGeng%>"></asp:Label>
                                                                                                ： </td>
                                                                                            <td>
                                                                                                <asp:DropDownList
                                                                                                    ID="DL_NewWLType" runat="server" DataTextField="HomeName" DataValueField="Type"
                                                                                                    AutoPostBack="true" Height="20px">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Button ID="BT_ChangeType" runat="server" CssClass="inpu" Enabled="false" Text="<%$ Resources:lang,BianGeng%>" OnClick="BT_ChangeType_Click" />
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
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <br />
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="text-align: left">

                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BuZhouLieBiao%>"></asp:Label>：
                                                                        <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserCode" runat="server"
                                                                            Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserName" runat="server"
                                                                            Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_MakeUserCode" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="86%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="6%" align="left">
                                                                                                <strong></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BuXu%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,MingChen%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZuiShaoRenShu%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShiJianXiaoShi%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,TiaoJian%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShiFouZiShen%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,BuMenXiangGuan%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,YongXuJianZhiShenPi%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XiangMuXiangGuan%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZiXuanShenPiRen%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShenQingRenZiDongShenPi%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YishenpiZheZiDongShenPi%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,XiaYiBuBiXu%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>

                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,XiaYiBu%>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="8%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,ZhanZhengTiJingDuYuGu %>"></asp:Label>
                                                                                                </strong>
                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px"
                                                                            ShowHeader="false" OnItemCommand="DataGrid2_ItemCommand"
                                                                            Width="86%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <Columns>
                                                                                <asp:TemplateColumn>
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_StepID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"StepID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="步序">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="StepName" HeaderText="Name">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="limitedOperator" HeaderText="最少人数">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LimitedTime" HeaderText="时间(小时)">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="StepID" DataNavigateUrlFormatString="TTWorkFlowStepConditionView.aspx?StepID={0}"
                                                                                    Target="_blank" Text="<%$ Resources:lang,TiaoJian%>">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="SelfReview" HeaderText="自审">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="DepartRelated" HeaderText="部门相关">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PartTimeReview" HeaderText="允许兼职审批">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProjectRelated" HeaderText="项目相关">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="OperatorSelect" HeaderText="自选审批者">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AllowSelfPass" HeaderText="申请人自批">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AllowPriorOperatorPass" HeaderText="已批者自批">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="NextStepMust" HeaderText="为下一步必须通过">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="NextSortNumber" HeaderText="下一步">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn>
                                                                                    <ItemTemplate>
                                                                                        <%# DataBinder.Eval(Container.DataItem,"FinishPercent") %> %
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                                                                </asp:TemplateColumn>

                                                                            </Columns>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                        </asp:DataGrid>

                                                                        <asp:Label ID="LB_SqlWL" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_RelatedUserCode" runat="server" Visible="False"></asp:Label>
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


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label63" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="text-align: left; overflow: auto; padding: 0px 5px 0px 5px;">

                            <table>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,BuXu%>"></asp:Label>
                                        <asp:Label ID="LB_SortNumber" runat="server"></asp:Label>
                                        <asp:Label ID="LB_StepName" runat="server"></asp:Label>
                                        <asp:Label ID="LB_StepID" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ChenYuanHeDongZuo%>"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax_tab_menu" Width="90%">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="步骤定义">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,BuZhouDingYi%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="2" cellspacing="0" class="formBgStyle" style="width: 100%;">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 20%;">
                                                    <asp:Label ID="LB_ID" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BuXu %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 30%; height: 15px;">
                                                    <asp:Label ID="LBL_SortNumber" runat="server"></asp:Label>

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 20%; height: 15px;">
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,MingChen %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 15px;">
                                                    <asp:Label ID="LBL_StepName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ZuiShaoRenShu %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_LimitedOperator" runat="server"></asp:Label>
                                                    <span class="color: #FF0066;">（<span class="font-size: small;">

                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XYHDYCYCBZDRSZH %>"></asp:Label>
                                                    </span>）</span>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ZuiChiShiJian %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_LimitedTime" runat="server"></asp:Label>
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,XiaoShi %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft"><span class="font-size: x-small;">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,XiaYiBuQueShen %>"></asp:Label>
                                                </span></td>
                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="LBL_NextSortNumber" runat="server"></asp:Label>

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft"><span class="font-size: x-small;">
                                                    <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,XiaYiBuBiXu %>"></asp:Label>
                                                </span></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_NextStepMust" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShiFouZiShen %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_SelfReview" runat="server"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BuMenXiangGuan %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LBL_DepartRelated" runat="server"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YongXuJianZhiShenPi %>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LBL_PartTimeReview" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,XiangMuXiangGuan %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="LBL_ProjectRelated" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ZiXuanShenPiZhe %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <table>
                                                        <tr>
                                                            <td>

                                                                <asp:Label ID="LBL_OperatorSelect" runat="server"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ShiFouQianBuZiXuan %>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LBL_IsPriorStepSelect" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,YXSQRZDPZ %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_AllowSelfPass" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,YXQBYPZZDPZ %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_AllowPriorOperatorPass" runat="server"></asp:Label>

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,TuiSongXiaoXi %>"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LBL_SendSMS" runat="server"></asp:Label>
                                                    <asp:Label ID="LBL_SendEMail" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="步骤成员">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,BuZhouChengYuan%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table align="left" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="7">
                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                            </td>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="left" width="15%"><strong>
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,CAnYuJueSeZhu %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="45%"><strong>
                                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,GongZuo %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="10%"><strong>
                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,ChenDanJueSe %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="10%"><strong>
                                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="10%"><strong>
                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ShiJianXiaoShi %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="10%"><strong></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="right" width="6">
                                                                <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="False" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand" ShowHeader="False" Width="100%">
                                                        <Columns>
                                                            <asp:HyperLinkColumn DataNavigateUrlField="IdentifyString" DataNavigateUrlFormatString="TTActorGroupDetail.aspx?IdentifyString={0}" DataTextField="ActorGroup" HeaderText="参与角色组" Target="_blank">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                            </asp:HyperLinkColumn>
                                                            <asp:BoundColumn DataField="WorkDetail" HeaderText="工作">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="45%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Actor" HeaderText="承担角色">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Requisite" HeaderText="必需">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LimitedTime" HeaderText="时间(小时)">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle CssClass="notTab" HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                        <ItemStyle CssClass="itemStyle" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="left" />
                                                    </asp:DataGrid>
                                                    <asp:Label ID="LB_DetailID" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindowStepOperator"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label68" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="text-align: left; overflow: auto; padding: 0px 5px 0px 5px;">

                            <br />
                            <table align="left" cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" width="20%">
                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,CanYuJueSeZhu %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" width="30%">
                                        <asp:Label ID="LBL_ActorGroup" runat="server"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 20%;">
                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LBL_Requisite" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ZhuYaoGongZuoNeiRong %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:Label ID="LBL_WorkDetail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,BuKeShiYu %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 4px;">
                                        <asp:Label ID="LBL_FieldList" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,KeBianJiYu %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:Label ID="LBL_EditFieldList" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,YaoQiuShiJian %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LBL_FinishedTime" runat="server"></asp:Label>
                                        小时 </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,JueSe %>"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 200px;">
                                        <asp:Label ID="LBL_Actor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
