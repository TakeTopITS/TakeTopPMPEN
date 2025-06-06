<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCreatedProjectDetail.aspx.cs" Inherits="TTCreatedProjectDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>给成员建立和分派项目</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

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
                    <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
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
                                                                    <asp:Label ID="LB_RunProject" runat="server" Text="<%$ Resources:lang,ProjectMaintenance%>" />
                                                                </td>
                                                                <td width="5">
                                                                    <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                    <td align="center" style="padding-top: 5px;">
                                                        <asp:HyperLink ID="HL_BusinessForm" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:HyperLink>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="padding: 5px 5px 5px 5px;" valign="top">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 105px;" class="formItemBgStyleForAlignLeft"></td>
                                                    <td colspan="3" align="right" style="padding-right: 12px;" class="formItemBgStyleForAlignRight">
                                                        <asp:Button ID="BT_Upate" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu"
                                                            OnClick="BT_Update_Click" Enabled="False" />&nbsp;&nbsp;
                                                                    <asp:Button ID="BT_Delete" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                                                        Text="<%$ Resources:lang,Delete%>" />

                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JueSe%>"></asp:Label>：
                                                            
                                                                    <asp:DropDownList
                                                                        ID="DL_Aactor" runat="server">
                                                                        <asp:ListItem Value="ProjectManager" Text="<%$ Resources:lang,XiangMuJingLi%>" />
                                                                        <asp:ListItem Value="ProjectCreator" Text="<%$ Resources:lang,LiXiangZhe%>" />
                                                                    </asp:DropDownList>
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuanGei%>"></asp:Label>:
                                                                    <asp:DropDownList
                                                                        ID="DL_NewPMName" runat="server" DataTextField="UserName" DataValueField="UserCode"
                                                                        Width="95px">
                                                                    </asp:DropDownList>
                                                        <asp:Button ID="BT_DirectDepartment" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ZhiShuBuMen%>" />
                                                        <cc1:ModalPopupExtender ID="BT_DirectDepartment_ModalPopupExtender" runat="server"
                                                            Enabled="True" TargetControlID="BT_DirectDepartment" PopupControlID="Panel1"
                                                            CancelControlID="IMBT_Close" BackgroundCssClass="modalBackground" Y="150">
                                                        </cc1:ModalPopupExtender>

                                                        <asp:Button ID="BT_TransferProject" runat="server" CssClass="inpu" Text="<%$ Resources:lang,TransferPM%>"
                                                            OnClick="BT_TransferProject_Click" Enabled="False" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_tbProjectID" runat="server" Text="<%$ Resources:lang,ProjectID%>" />:
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="10%">
                                                                    <asp:Label ID="LB_ProjectID" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="10%" align="right">
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuDaiMa%>"></asp:Label>
                                                                    :
                                                                </td>
                                                                <td width="15%%">
                                                                    <asp:TextBox ID="TB_ProjectCode" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td width="10%" style="text-align: right;">
                                                                    <asp:Label ID="LB_TBProjectType" runat="server" Text="<%$ Resources:lang,ProjectType%>" />:
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <asp:DropDownList ID="DL_ProjectType" runat="server" DataTextField="Type"
                                                                        AutoPostBack="true" OnSelectedIndexChanged=" DL_ProjectType_SelectedIndexChanged"
                                                                        DataValueField="Type" CssClass="DDList" Width="178px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_ParentProjectID" runat="server" Font-Bold="False" Visible="false"></asp:Label>
                                                        <asp:Label ID="LB_TBParentProject" runat="server" Text="<%$ Resources:lang,ParentProject%>" />:
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <asp:TextBox ID="TB_ParentProject" Width="99%" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBProjectName" runat="server" Text="<%$ Resources:lang,ProjectName%>" />:
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <asp:TextBox ID="TB_ProjectName" runat="server" Width="99%"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBPM" runat="server" Text="<%$ Resources:lang,PM%>" />:
                                                    </td>
                                                    <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_PMCode" runat="server"></asp:Label><asp:Label ID="LB_PMName"
                                                            runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 8%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_CustomerPM" runat="server" Text="<%$ Resources:lang,CustomerPM%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:TextBox ID="TB_CustomerPMName" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBStartTime" runat="server" Text="<%$ Resources:lang,StartTime%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                                        </ajaxToolkit:CalendarExtender>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="TB_TBEndTime" runat="server" Text="<%$ Resources:lang,EndTime%>" />:
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
                                                        <asp:Label ID="LB_TBProjectAmount" runat="server" Text="<%$ Resources:lang,ProjectAmount%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ProjectAmount" runat="server" Width="100px">0.00</NickLee:NumberBox>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBBudget" runat="server" Text="<%$ Resources:lang,Budget%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Budget" runat="server" Width="100px">0.00</NickLee:NumberBox>
                                                        &nbsp;&nbsp;<asp:HyperLink ID="HL_ProjectBudget" runat="server" Target="_blank">
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YuSuanMingXi%>"></asp:Label>
                                                        </asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBManHour" runat="server" Text="<%$ Resources:lang,ManHour%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ManHour" runat="server" Width="53px">0.00</NickLee:NumberBox>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBManPower" runat="server" Text="<%$ Resources:lang,ManPower%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ManNubmer" runat="server" Width="53px">0.00</NickLee:NumberBox><span
                                                            style="font-size: 10pt">人</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JieSuanBiBie%>"></asp:Label>:
                                                    </td>
                                                    <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_CurrencyType" runat="server" ataTextField="Type" DataValueField="Type" Height="25px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBStatus" runat="server" Text="<%$ Resources:lang,Status%>" />:
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_Status" runat="server" DataTextField="HomeName"
                                                            OnSelectedIndexChanged="DL_Status_SelectedIndexChanged" AutoPostBack="true" DataValueField="Status"
                                                            CssClass="DDList">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;
                                                                <asp:Label ID="LB_StatusValue" runat="server" Text="<%$ Resources:lang,StatusValue%>" />:<asp:DropDownList
                                                                    ID="DL_StatusValue" OnSelectedIndexChanged=" DL_StatusValue_SelectedIndexChanged"
                                                                    AutoPostBack="true" runat="server">
                                                                    <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,JinXingZhong%>" />
                                                                    <asp:ListItem Value="Passed" Text="<%$ Resources:lang,TongGuo%>" />
                                                                </asp:DropDownList>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="HL_StatusReview" Enabled="false" runat="server"
                                                            Target="_blank" Text="<%$ Resources:lang,ProjectStatusReview%>"></asp:HyperLink>
                                                        <asp:Label ID="LB_Status" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBDescription" runat="server" Text="<%$ Resources:lang,Description%>" />:
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <CKEditor:CKEditorControl ID="HE_ProjectDetail" runat="server" Height="150px" Width="99%" Visible="false" />
                                                        <CKEditor:CKEditorControl runat="server" ID="HT_ProjectDetail" Width="99%" Height="100px" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBAcceptanceStandard" runat="server" Text="<%$ Resources:lang,AcceptanceStandard%>" />:
                                                    </td>
                                                    <td colspan="3" style="text-align: left; height: 47px;" class="formItemBgStyleForAlignLeft">
                                                        <CKEditor:CKEditorControl ID="HE_AcceptStandard" runat="server" Height="150" Width="99%" Visible="false" />
                                                        <CKEditor:CKEditorControl runat="server" ID="HT_AcceptStandard" Width="99%" Height="100px" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">

                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,SDYJDDJH%>"></asp:Label>:
                            
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_LockStartupedPlan" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,YINGXXMJLGXMZT%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_AllowPMChangeStatus" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>


                                                                <td style="text-align: right">
                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,JDHGQSHDBDXMJHYH%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_ProgressByDetailImpact" runat="server">
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>

                                                                <td style="text-align: right">
                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,RWJDYHJHJDYQR%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_PlanProgressNeedPlanerConfirm" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,RXZDFQLC%>"></asp:Label>:
                                                    </td>
                                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_AutoRunWFAfterMakeProject" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XMQDXYZJYHQR%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_ProjectStartupNeedSupperConfirm" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,XMJHYQD%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_ProjectPlanStartupSatus" runat="server">
                                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                        <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,XMJHTGGLGZLMB%>"></asp:Label>:
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DL_PlanStartupRelatedWorkflowTemplate" runat="server" DataTextField="TemName" DataValueField="TemName" Width="150px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                                    <td colspan="3" style="height: 1px;" class="formItemBgStyleForAlignLeft">


                                                        <asp:Repeater ID="Repeater1" runat="server">
                                                            <ItemTemplate>

                                                                <a href='<%# DataBinder.Eval(Container.DataItem,"ModulePage") %>' target="iframe">
                                                                    <%# DataBinder.Eval(Container.DataItem,"HomeModuleName") %> &nbsp;
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:Repeater>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                                    <td colspan="3" style="height: 1px;" class="formItemBgStyleForAlignLeft">
                                                        <div id="divProject2" runat="server">

                                                            <asp:Repeater ID="Repeater2" runat="server">
                                                                <ItemTemplate>

                                                                    <a href='<%# DataBinder.Eval(Container.DataItem,"ModulePage") %>' target="iframe">
                                                                        <%# DataBinder.Eval(Container.DataItem,"HomeModuleName") %> &nbsp;
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:Repeater>

                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                                    <td colspan="3" style="height: 1px;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="LB_TBNoticeProjectTeamMember" runat="server" Text="<%$ Resources:lang,NoticeProjectTeamMember%>" />:
                                                                <asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,SendSMS%>" />
                                                        <asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,SendEMail%>" />
                                                        <asp:TextBox ID="TB_Message" runat="server" Width="300px"></asp:TextBox>
                                                        <asp:Button ID="BT_Send" runat="server" Enabled="False" CssClass="inpu" OnClick="BT_Send_Click"
                                                            Text="<%$ Resources:lang,Send%>" />
                                                    </td>
                                                </tr>


                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 310px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">

                                        <asp:Button ID="BT_MyMember" runat="server" CssClass="inpuLongest" OnClick="BT_MyMember_Click"
                                            Text="<%$ Resources:lang,MyMember%>" />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
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
