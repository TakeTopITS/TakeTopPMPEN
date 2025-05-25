<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTImplePlanView.aspx.cs"
    Inherits="TTImplePlanView" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>��Ŀ�����ƻ�</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            

        });

    </script>

</head>
<body>
  
        <form id="form1" runat="server">
          <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" >
            </asp:ScriptManager>        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <table cellpadding="0" cellspacing="0" border="0" width="100%" align="left" class="bian">
                    <tr>
                        <td height="31" class="page_topbj">
                            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="29">
                                                    <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuJiHua%>"></asp:Label>
                                                </td>
                                                <td width="5">
                                                    <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td rowspan="4" valign="top" width="230" style="padding: 5px 5px 0px 5px; text-align: left; border-right: solid 1px #D8D8D8">
                                        <table style="width: 230px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuanZeBanBen%>"></asp:Label>��
                                                <asp:DropDownList ID="DL_VersionID" runat="server" DataTextField="VerID" DataValueField="ID"
                                                    OnSelectedIndexChanged="DL_Version_SelectedIndexChanged" Width="40px" Height="21px"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��<asp:DropDownList ID="DL_ChangeVersionType" runat="server" AutoPostBack="True">
                                                        <asp:ListItem Value="InUse" Text="<%$ Resources:lang,ZaiYong%>"/>
                                                        <asp:ListItem Value="Backup" Text="<%$ Resources:lang,BeiYong%>"/>
                                                        <asp:ListItem Value="Baseline" Text="<%$ Resources:lang,JiZhun%>"/>
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                        Width="220px" ShowLines="True" Font-Size="10pt" Font-Bold="False" Font-Names="����">
                                                        <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                                    </asp:TreeView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table align="left" style="width: 98%; margin-top: 10px" cellpadding="3" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td width="20%"  class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��
                                                </td>
                                                <td style="width: 30%; "  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_PlanType" runat="server" CssClass="DDList" >
                                                        <asp:ListItem Value="Plan" Text="<%$ Resources:lang,JiHua%>"/>
                                                        <asp:ListItem Value="Milestone" Text="<%$ Resources:lang,LiChengBei%>"/>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="20%"  class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,QianJiHua%>"></asp:Label>��
                                                </td>
                                                <td width="30%"  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="TB_PriorID" runat="server" MinAmount="0" Precision="0" Width="50px">
                                                 0
                                                    </NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JiHuaNeiRong%>"></asp:Label>��
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_PlanDetail" runat="server" Height="19px" Width="570px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_EndDate" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                        runat="server" TargetControlID="DLC_EndDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZiYuan%>"></asp:Label>��
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Resource" runat="server" Width="442px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox   ID="TB_PlanBudget" runat="server" Width="79px" MinAmount="0">
                                                 0 0.00
                                                    </NickLee:NumberBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,JianLiShiJian%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_MakeTime" runat="server" Font-Size="9pt" Width="220px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BiaoZhunJinDu%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox   ID="NB_DefaultSchedule" runat="server" MinAmount="0" Width="79px">
                                                    0 0.00
                                                    </NickLee:NumberBox>
                                                    %
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,BiaoZhunChengBen%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000"  ID="NB_DefaultCost" runat="server" MinAmount="0" Width="79px">
                                                 0 0.00
                                                    </NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" CssClass="DDList" DataTextField="Status"
                                                        DataValueField="Status" Width="89px" class="formItemBgStyleForAlignLeft">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,SuoDingZhuangTai%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_LockStatus" runat="server" Enabled="False" AutoPostBack="true">
                                                        <asp:ListItem>No</asp:ListItem>
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                              
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height: 25px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_BackupID" runat="server"  Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_ProjectID" runat="server"  Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_ParentID" runat="server"  Visible="False"></asp:Label>
                                                    <asp:HyperLink ID="HL_RelatedDoc" runat="server" Height="16px" NavigateUrl="TTProPlanRelatedDocView.aspx"
                                                        Target="_blank" Enabled="False">---&gt;<asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,XiangGuanWenDang%>"></asp:Label></asp:HyperLink>
                                                    &nbsp;<asp:HyperLink ID="HL_WorkPlanView" runat="server" Height="16px" NavigateUrl="TTWorkPlanView.aspx"
                                                        Target="_blank">---&gt;<asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,LieBiaoShiLiuLan%>"></asp:Label></asp:HyperLink>
                                                    &nbsp;<asp:HyperLink ID="HL_ProPlanGanttRight" runat="server" Height="16px" NavigateUrl="TTWorkPlanGanttForProject.aspx"
                                                        Target="right">---&gt;<asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GanTeTuYou%>"></asp:Label></asp:HyperLink>
                                                    &nbsp;<asp:HyperLink ID="HL_ProPlanGanttNew" runat="server" Height="16px" NavigateUrl="TTWorkPlanGanttForProject.aspx"
                                                        Target="_blank">---&gt;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,GanTeTuXin%>"></asp:Label></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table align="left" cellpadding="0" cellspacing="0" width="98%">
                                            <tr>
                                                <td align="left" style="padding-left: 20px; font-weight: bold; height: 24px; color: #394f66; background-image: url('ImagesSkin/titleBG.jpg')">
                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,JiHua%>"></asp:Label>��<asp:Label ID="LB_PlanID" runat="server" Font-Size="10pt"></asp:Label>
                                                    <asp:Label ID="LB_PlanDetail" runat="server" Font-Size="10pt"></asp:Label>
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,DeCanYuRenYuan%>"></asp:Label>��
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; vertical-align: top;">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                    <tr>
                                                                        <td width="7">
                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="8%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="12%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="30%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhuYaoGongZuo%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="20%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,KaiShiGongZuoShiJian%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="20%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,JieShuGongZuoShiJian%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="10%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label></strong>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width="6" align="right">
                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                                    Height="30px" PageSize="8" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="MainWork" HeaderText="��Ҫ����">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="StartWorkDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��ʼ����ʱ��">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EndWorkDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������ʱ��">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Budget" HeaderText="Budget">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; padding-top: 10px">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
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
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
