<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMakeProjectForEngineeringIndustry.aspx.cs" Inherits="TTMakeProjectForEngineeringIndustry" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>给成员建立和分派项目</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*#AboveDiv {
            min-width: 1580px;
            width: expression (document.body.clientWidth <= 1580? "1580px" : "auto" ));
        }*/

        .auto-style1 {
            position: absolute;
            left: 35%;
            top: 35%;
            height: 34px;
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
    <script type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }
        });

        //设置IFRMAE的高度
        function setBusinessFormIFrameHeight() {

            var winHeight = 0;

            winHeight = document.getElementById("popwindow").style.height;

            document.getElementById("TabContainer1_TabPanel2_IFrame_RelatedInformation").height = (winHeight.toString().replace("px", "") - 160);
        }
        window.onmousemove = setBusinessFormIFrameHeight;

        //document.addEventListener('paste', function (event) {
        //    // 阻止默认粘贴操作
        //    event.preventDefault();

        //    // 获取粘贴的内容
        //    var clipboardData = event.clipboardData || window.clipboardData;
        //    var pastedText = clipboardData.getData('text');

        //    // 处理粘贴的内容
        //    const arr = pastedText.split(",");

        //    alert(topframe.contentWindow.document.getElementById("TabContainer1_TabPanel1_TB_Longitude").value);

        //    this.topframe.contentWindow.document.getElementById("TabContainer1_TabPanel1_TB_Longitude").value = arr[0];
        //    this.topframe.contentWindow.document.getElementById("TabContainer1_TabPanel1_TB_Latitude").value = arr[1];
        //});



    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
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
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,SetUpProject%>" />
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
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="100%" align="left" style="padding: 5px 5px 5px 5px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="80%" align="left">
                                                            <table width="750px">
                                                                <tr>
                                                                    <td width="20%" align="right">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XiangMuMing%>"></asp:Label>： </td>
                                                                    <td width="25%" align="left">
                                                                        <asp:TextBox ID="TB_FindProjectName" widh="99%" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td width="20%" align="right">
                                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,XiangMuJingLi%>"></asp:Label>：</td>
                                                                    <td width="25%" align="left">
                                                                        <asp:TextBox ID="TB_FindPMName" widh="99%" runat="server"></asp:TextBox></td>
                                                                    <td align="left">
                                                                        <asp:Button ID="BT_Find" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Find_Click" />

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>

                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,ProjectID%>" /></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Lb_DGProjectCode" runat="server" Text="<%$ Resources:lang,ProjectCode%>" /></strong>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGProjectName" runat="server" Text="<%$ Resources:lang,ProjectName%>" /></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGPM" runat="server" Text="<%$ Resources:lang,PM%>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGStatus" runat="server" Text="<%$ Resources:lang,Status%>" /></strong>
                                                                    </td>
                                                                    <td colspan="2" width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,Budget%>" />
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <%-- <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>--%>
                                                                    <td width="5%" align="left">
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
                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                    OnItemCommand="DataGrid2_ItemCommand" OnPageIndexChanged="DataGrid2_PageIndexChanged"
                                                    AllowCustomPaging="false" AllowPaging="true" PageSize="25" ShowHeader="False"
                                                    Width="100%">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <Columns>
                                                        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:ButtonColumn>
                                                        <asp:TemplateColumn HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="ProjectID" HeaderText="项目ID">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProjectCode" HeaderText="ProjectCode">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTProjectDetail.aspx?ProjectID={0}"
                                                            DataTextField="ProjectName" HeaderText="项目名称" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="PMName" HeaderText="ProjectManager">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByProjectStatus(Eval("Status").ToString(),Eval("ProjectType").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTMakeProjectBudget.aspx?ProjectID={0}"
                                                            Text="<%$ Resources:lang,zijin%>" HeaderText="资金" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTProjectRelatedItem.aspx?ProjectID={0}"
                                                            Text="<%$ Resources:lang,WuZhi%>" HeaderText="物资" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTProjectPrimaveraTask.aspx?ProjectID={0}"
                                                            Text="<%$ Resources:lang,XiangMuZuoYeRenWu%>" HeaderText="Task" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTProjectCostManageEdit.aspx?ProjectID={0}"
                                                            Text="<%$ Resources:lang,XiangMuChengBenKongZhi%>" HeaderText="Cost" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTMakeConstractFromOther.aspx?RelatedType=PROJECT&RelatedID={0}"
                                                            Text="<%$ Resources:lang,ZhuanHeTong%>" HeaderText="Contract" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <%--  <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTMakeCollaboration.aspx?RelatedType=PROJECT&RelatedID={0}"
                                                            Text="<%$ Resources:lang,XieZuo%>" HeaderText="Collaboration" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>--%>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTProjectRelatedDoc.aspx?ProjectID={0}"
                                                            Text="<%$ Resources:lang,WenDang%>" HeaderText="文档" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>

                                                        <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTRelatedDIYWorkflowForm.aspx?RelatedType=Project&RelatedID={0}"
                                                            Text="<%$ Resources:lang,RunByWF%>" HeaderText="发起流程" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <div>
                                                                    <asp:HyperLink ID="HL_ProjectReport" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ProjectID", "TTProjectReportMain.aspx?ProjectID={0}") %>'
                                                                        Target="_blank"><img src="ImagesSkin/dian.gif" class="noBorder" /></asp:HyperLink>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_UserCode" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_UserName" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_Status" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 420px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td colspan="2" style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">

                                        <asp:Button ID="BT_MyMember" runat="server" CssClass="inpuLongest" OnClick="BT_MyMember_Click"
                                            Text="<%$ Resources:lang,MyMember%>" />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 360px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                            ShowLines="True" Width="350px">
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
                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 420px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 360px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                            ShowLines="True" Width="350px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_CloseTree" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
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

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="LB_PopWindowTitle" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="text-align: left; overflow: auto; padding: 0px 5px 0px 5px;">

                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" Width="100%" runat="server" ActiveTabIndex="0">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="基本信息" TabIndex="0">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,JiBenXinXi%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" style="padding: 5px 5px 5px 5px; border-right: solid 1px #D8D8D8;">
                                                    <table width="98%" cellpadding="0" cellspacing="0" align="left">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                                    <tr>
                                                                        <td width="10%" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,WuZiXiangMuShuXing%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_ProjectAttribute" runat="server">
                                                                            </asp:DropDownList>(<asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,WuZiXiangMuCaiXuanZe%>"></asp:Label>)
                                                                        </td>
                                                                        <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,FuJian%>"></asp:Label>：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_AttachFile" runat="server" Width="95%"></asp:TextBox>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,WuZiXiangMuXingZhi%>"></asp:Label>：</td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_ProjectNature" runat="server">
                                                                            </asp:DropDownList>(<asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,WuZiXiangMuCaiXuanZe%>"></asp:Label>)
                                                                            
                                                                            <asp:Button ID="BT_CreateObjectCode" CssClass="inpuLong" runat="server" Text="<%$ Resources:lang,SCWZXMBM%>" OnClick="BT_CreateObjectCode_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DanWeiLeiXing%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_UnitType" runat="server">
                                                                                <asp:ListItem Text="" Value="" />
                                                                                <asp:ListItem Text="<%$ Resources:lang,XingZhengDanWei%>" Value="行政单位" />
                                                                                <asp:ListItem Text="<%$ Resources:lang,XiangMuBu%>" Value="ProjectDepartment" />
                                                                            </asp:DropDownList>(<asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,WuZiXiangMuCaiXuanZe%>"></asp:Label>)
                                                                                

                                                                            <table style="display: none;">
                                                                                <tr>

                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="LB_tbPirority" runat="server" Text="<%$ Resources:lang,Pirority %>"></asp:Label>:</td>
                                                                                    <td class="formItemBgStyleForAlignLeft">

                                                                                        <asp:DropDownList ID="DL_Priority" runat="server" DataTextField="Priority" DataValueField="Priority">
                                                                                            <asp:ListItem Value="COMMON" Text="<%$ Resources:lang,YiBan%>" />
                                                                                            <asp:ListItem Value="2-H" Text="<%$ Resources:lang,ErGao%>" />
                                                                                            <asp:ListItem Value="1-H" Text="<%$ Resources:lang,YiGao%>" />
                                                                                            <asp:ListItem Value="2-M" Text="<%$ Resources:lang,ErZhong%>" />
                                                                                            <asp:ListItem Value="1-M" Text="<%$ Resources:lang,YiZhong%>" />
                                                                                            <asp:ListItem Value="1-L" Text="<%$ Resources:lang,YiDi%>" />
                                                                                            <asp:ListItem Value="2-L" Text="<%$ Resources:lang,ErDi%>" />
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,GuiShuBuMen %>"></asp:Label>:</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="LB_BelongDepartCode" runat="server"></asp:Label><asp:Label runat="server" ID="LB_BelongDepartName"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Button ID="BT_BelongDepartment" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe%>" /></td>
                                                                                    <cc1:ModalPopupExtender ID="BT_BelongDepartment_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="IMBT_CloseTree3" DynamicServicePath="" Enabled="True" PopupControlID="Panel3" TargetControlID="BT_BelongDepartment" Y="150"></cc1:ModalPopupExtender>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBParentProject" runat="server" Text="<%$ Resources:lang,ParentProject%>" />：
                                                                        </td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_ParentProject" Width="98%" runat="server"></asp:TextBox>
                                                                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                                                                Enabled="True" TargetControlID="TB_ParentProject" PopupControlID="Panel2"
                                                                                CancelControlID="IMBT_CloseTree" BackgroundCssClass="modalBackground" Y="150">
                                                                            </cc1:ModalPopupExtender>
                                                                            <asp:Label ID="LB_ParentProjectID" runat="server" Font-Bold="False" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label77771" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>"></asp:Label>：
                                                                        </td>
                                                                        <td style="width: 25%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_ProjectCode" runat="server" Width="95%"></asp:TextBox>
                                                                            <asp:Label ID="LB_ProjectID" runat="server" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBProjectName" runat="server" Text="<%$ Resources:lang,ProjectName%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_ProjectName" runat="server" Width="95%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBPM" runat="server" Text="<%$ Resources:lang,PM%>" />：
                                                                        </td>
                                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DL_PM" runat="server" DataTextField="UserName" DataValueField="UserCode">
                                                                            </asp:DropDownList>
                                                                            <asp:Button ID="BT_DirectDepartment" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ZhiShuBuMen%>" />
                                                                            <cc1:ModalPopupExtender ID="BT_DirectDepartment_ModalPopupExtender" runat="server"
                                                                                Enabled="True" TargetControlID="BT_DirectDepartment" PopupControlID="Panel1"
                                                                                CancelControlID="IMBT_Close" BackgroundCssClass="modalBackground" Y="150">
                                                                            </cc1:ModalPopupExtender>
                                                                        </td>
                                                                        <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_CustomerPM" runat="server" Text="<%$ Resources:lang,CustomerPM%>" />：
                                                                        </td>
                                                                        <td style="width: 40%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_CustomerPMName" runat="server"></asp:TextBox>
                                                                            <asp:HyperLink ID="HL_CustomerInfo" runat="server" Enabled="False" NavigateUrl="~/TTCustomerInfo.aspx"
                                                                                Target="_blank" Text="<%$ Resources:lang,Customer%>"></asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBProjectType" runat="server" Text="<%$ Resources:lang,ProjectType%>" />：
                                                                        </td>
                                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DL_ProjectType" runat="server" DataTextField="Type"
                                                                                AutoPostBack="true" OnSelectedIndexChanged=" DL_ProjectType_SelectedIndexChanged"
                                                                                DataValueField="Type" CssClass="DDList">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,XiangMuFangXiang%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_ProjectDirection" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShouQuanCaiGou%>"></asp:Label>：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DL_AuthorizedProcurement" runat="server">
                                                                                <asp:ListItem />
                                                                                <asp:ListItem Value="YES" Text="<%$ Resources:lang,You%>" />
                                                                                <asp:ListItem Value="NO" Text="<%$ Resources:lang,Wu%>" />
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ZiJunLaiYuan%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DL_FundingSource" runat="server" DataTextField="FundingSource" DataValueField="FundingSource" AutoPostBack="false">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBStartTime" runat="server" Text="<%$ Resources:lang,StartTime%>" />：
                                                                        </td>
                                                                        <td style="width: 35%; height: 35px;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_BeginDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                                            <%--<asp:TextBox ID="DLC_BeginDate" ReadOnly="false" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate">
                                                                        </ajaxToolkit:CalendarExtender>--%>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="TB_TBEndTime" runat="server" Text="<%$ Resources:lang,EndTime%>" />：
                                                                        </td>
                                                                        <td style="height: 35px;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_EndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                                            <%--<asp:TextBox ID="DLC_EndDate" ReadOnly="false" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                                            runat="server" TargetControlID="DLC_EndDate">
                                                                        </ajaxToolkit:CalendarExtender>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBProjectAmount" runat="server" Text="<%$ Resources:lang,ProjectAmount%>" />：
                                                                        </td>
                                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ProjectAmount" runat="server" Width="150px">0.00</NickLee:NumberBox>
                                                                        </td>
                                                                        <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBBudget" runat="server" Text="<%$ Resources:lang,Budget%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Budget" runat="server" Width="150px">0.00</NickLee:NumberBox>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBManHour" runat="server" Text="<%$ Resources:lang,ManHour%>" />：
                                                                        </td>
                                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="0" ID="NB_ManHour" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                            <asp:Label ID="Label888882" runat="server" Text="<%$ Resources:lang,Tian %>"></asp:Label>

                                                                        </td>
                                                                        <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBManPower" runat="server" Text="<%$ Resources:lang,ManPower%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
                                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="0" ID="NB_ManNubmer" runat="server" Width="80px">0.00</NickLee:NumberBox><span
                                                                                style="font-size: 10pt">人</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label>：
                                                                        </td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 80%">
                                                                                        <asp:TextBox ID="TB_ProjectLocation" runat="server" Width="95%"></asp:TextBox>
                                                                                    </td>
                                                                                    <td onclick="popShowByURL('TTMakeProjectBaiDuMap.aspx','BaiDuMap', 600, 500)">
                                                                                        <img src="ImagesSkin/GPS.jpg" alt="取经纬度" width="20" height="20" style="border: 0px;">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,Longitude%>" />：
                                                                        </td>
                                                                        <td style="width: 35%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_Longitude" runat="server" Width="80%"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,Latitude%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_Latitude" runat="server" Width="80%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,JieSuanBiBie%>"></asp:Label>：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">

                                                                            <asp:DropDownList ID="DL_CurrencyType" runat="server" DataTextField="Type" DataValueField="Type"></asp:DropDownList>

                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,XiangMuYiShuanKeMu%>" />：
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_BudgetAccount" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="LB_TBStatus" runat="server" Text="<%$ Resources:lang,Status%>" />：
                                                                        </td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DL_Status" runat="server" DataTextField="HomeName"
                                                                                OnSelectedIndexChanged="DL_Status_SelectedIndexChanged" AutoPostBack="true" DataValueField="Status"
                                                                                CssClass="DDList">
                                                                            </asp:DropDownList>
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="LB_StatusValue" runat="server" Text="<%$ Resources:lang,StatusValue%>" />
                                                                            ：<asp:DropDownList ID="DL_StatusValue" OnSelectedIndexChanged=" DL_StatusValue_SelectedIndexChanged"
                                                                                AutoPostBack="true" runat="server" Width="80px">
                                                                                <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,JinXingZhong%>" />
                                                                                <asp:ListItem Value="Passed" Text="<%$ Resources:lang,TongGuo%>" />
                                                                            </asp:DropDownList>

                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JiaGongYuSuan%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="NB_ABudgetFor" runat="server"></asp:TextBox>
                                                                            <%--<NickLee:NumberBox MaxAmount="1000000000000" MinAmount="0" ID="NB_ABudgetFor" runat="server" Width="120px">0.00</NickLee:NumberBox>--%>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZiGouYuSuan%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="NB_SincePurchaseBudget" runat="server"></asp:TextBox>
                                                                            <%--<NickLee:NumberBox MaxAmount="1000000000000" MinAmount="0" ID="NB_SincePurchaseBudget" runat="server" Width="120px">0.00</NickLee:NumberBox>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,JianSheDanWei%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_ConstructionUnit" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,JianLiDanWei%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TB_SupervisionUnit" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ZhuGuanLingDao%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:HiddenField ID="HF_Leader" runat="server" />
                                                                            <asp:TextBox ID="TXT_Leader" runat="server"></asp:TextBox>&nbsp;
                                                                        <input type="button" id="btnLeader" class="inpu" runat="server" value="Select" onclick="SelectEmployee('TTWZSelectorMember.aspx', 'TabContainer1_TabPanel1_HF_Leader', 'TabContainer1_TabPanel1_TXT_Leader')" />
                                                                            <%--<asp:DropDownList ID="DDL_Leader" runat="server" DataTextField="UserName" DataValueField="UserCode"
                                                                            Width="120px" >
                                                                        </asp:DropDownList>
                                                                        <asp:Button ID="BT_Leader" runat="server" Text="<%$ Resources:lang,ZhiShuBuMen%>" />
                                                                        <cc1:ModalPopupExtender ID="BT_ModalPopupLeader" runat="server"
                                                                            Enabled="True" TargetControlID="BT_Leader" PopupControlID="PanelLeader"
                                                                            CancelControlID="IMBT_Leader_Close" BackgroundCssClass="modalBackground" Y="150">
                                                                        </cc1:ModalPopupExtender>--%>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,FeiKongZhuGuan%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:HiddenField ID="HF_FeeManage" runat="server" />
                                                                            <asp:TextBox ID="TXT_FeeManage" runat="server"></asp:TextBox>&nbsp;
                                                                        <input type="button" id="BT_FeeManage" class="inpu" runat="server" value="Select" onclick="SelectEmployee('TTWZSelectorMember.aspx', 'TabContainer1_TabPanel1_HF_FeeManage', 'TabContainer1_TabPanel1_TXT_FeeManage')" />
                                                                            <%--<asp:DropDownList ID="DDL_FeeManage" runat="server" DataTextField="UserName" DataValueField="UserCode"
                                                                            Width="120px" >
                                                                        </asp:DropDownList>
                                                                        <asp:Button ID="BT_FeeManage" runat="server" Text="<%$ Resources:lang,ZhiShuBuMen%>" />
                                                                        <cc1:ModalPopupExtender ID="BT_ModalPopupExtenderFeeManage" runat="server"
                                                                            Enabled="True" TargetControlID="BT_DirectDepartment" PopupControlID="PanelFeeManage"
                                                                            CancelControlID="IMBT_FeeManage_Close" BackgroundCssClass="modalBackground" Y="150">
                                                                        </cc1:ModalPopupExtender>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,CaiLiaoYuan%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:HiddenField ID="HF_MaterialPerson" runat="server" />
                                                                            <asp:TextBox ID="TXT_MaterialPerson" runat="server"></asp:TextBox>&nbsp;
                                                                        <input type="button" id="BT_MaterialPerson" class="inpu" runat="server" value="Select" onclick="SelectEmployee('TTWZSelectorMember.aspx', 'TabContainer1_TabPanel1_HF_MaterialPerson', 'TabContainer1_TabPanel1_TXT_MaterialPerson')" />
                                                                            <%--<asp:DropDownList ID="DDL_MaterialPerson" runat="server" DataTextField="UserName" DataValueField="UserCode"
                                                                            Width="120px" >
                                                                        </asp:DropDownList>
                                                                        <asp:Button ID="BT_MaterialPerson" runat="server" Text="<%$ Resources:lang,ZhiShuBuMen%>" />
                                                                        <cc1:ModalPopupExtender ID="BT_ModalPopupExtenderMaterialPerson" runat="server"
                                                                            Enabled="True" TargetControlID="BT_MaterialPerson" PopupControlID="PanelMaterialPerson"
                                                                            CancelControlID="IMBT_MaterialPerson_Close" BackgroundCssClass="modalBackground" Y="150">
                                                                        </cc1:ModalPopupExtender>--%>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ShiYongBiaoJi%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_IsMark" runat="server" Enabled="false">
                                                                                <asp:ListItem Text="<%$ Resources:lang,Shi%>" Value="-1" />
                                                                                <asp:ListItem Text="<%$ Resources:lang,Fou%>" Value="0" Selected="True" />
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,KaiGongRiQi%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_StartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,WanGongRiQi%>"></asp:Label>：</td>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_EndTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                                            <asp:Label ID="LB_TBDescription" runat="server" Text="<%$ Resources:lang,Description%>" />：
                                                                        </td>
                                                                        <td colspan="3" align="left" style="width: 100%;" class="formItemBgStyleForAlignLeft">
                                                                            <CKEditor:CKEditorControl ID="HE_ProjectDetail" runat="server" Height="150px" Width="90%" Visible="false" />
                                                                            <CKEditor:CKEditorControl runat="server" ID="HT_ProjectDetail" Width="90%" Height="150px" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                                            <asp:Label ID="LB_TBAcceptanceStandard" runat="server" Text="<%$ Resources:lang,AcceptanceStandard%>" />：
                                                                        </td>
                                                                        <td colspan="3" style="width: 100%;" class="formItemBgStyleForAlignLeft">
                                                                            <CKEditor:CKEditorControl ID="HE_AcceptStandard" runat="server" Height="150" Width="90%" Visible="false" />
                                                                            <CKEditor:CKEditorControl runat="server" ID="HT_AcceptStandard" Width="90%" Height="150px" Visible="false" />
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">

                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,SDYJDDJH%>"></asp:Label>:
                            
                                                                        </td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">

                                                                            <asp:DropDownList ID="DL_LockStartupedPlan" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>


                                                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,YINGXXMJLGXMZT%>"></asp:Label>:
                 
                                                                            <asp:DropDownList ID="DL_AllowPMChangeStatus" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,JDHGQSHDBDXMJHYH%>"></asp:Label>:
              
                                                                            <asp:DropDownList ID="DL_ProgressByDetailImpact" runat="server">
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,RWJDYHJHJDYQR%>"></asp:Label>:
              
                                                                            <asp:DropDownList ID="DL_PlanProgressNeedPlanerConfirm" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,RXZDFQLC%>"></asp:Label>:
                                                                        </td>
                                                                        <td colspan="3" class="formItemBgStyleForAlignLeft">

                                                                            <asp:DropDownList ID="DL_AutoRunWFAfterMakeProject" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,XMQDXYZJYHQR%>"></asp:Label>:
                 
                                                                            <asp:DropDownList ID="DL_ProjectStartupNeedSupperConfirm" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,XMJHYQD%>"></asp:Label>:
              
                                                                            <asp:DropDownList ID="DL_ProjectPlanStartupSatus" runat="server">
                                                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                                                                <asp:ListItem Value="YES">YES</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,XMJHTGGLGZLMB%>"></asp:Label>:
                
                                                                            <asp:DropDownList ID="DL_PlanStartupRelatedWorkflowTemplate" runat="server" DataTextField="TemName" DataValueField="TemName" Width="150px">
                                                                            </asp:DropDownList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4" style="width: 100%; padding-left: 10px" class="formItemBgStyleForAlignLeft">
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

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="关联信息" TabIndex="2">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel_RelatedBusiness" runat="server" Visible="false">
                                            <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                <%--  <tr>
                                                    <td colspan="8" align="left">
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan %>"></asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label3333" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>: </td>
                                                    <td style="width: 20%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_WLType" runat="server" DataTextField="HomeName" DataValueField="Type"
                                                            AutoPostBack="true" OnSelectedIndexChanged="DL_WLType_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label7333" runat="server" Text="<%$ Resources:lang,BiaoDanGuanLianDeLiuChengMoBan%>"></asp:Label>：
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_WFTemplate" runat="server" DataTextField="TemName" DataValueField="TemName">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%;" class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,YunXuHouXuXiuGe%>"></asp:Label>：
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_AllowUpdate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_AllowUpdate_SelectedIndexChanged">
                                                            <asp:ListItem Value="YES" Text="YES"></asp:ListItem>
                                                            <asp:ListItem Value="NO" Text="NO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">

                                                        <asp:Button ID="BT_StartupBusinessForm" runat="server" CssClass="inpu" Text="<%$ Resources:lang,DaKai %>" OnClick="BT_StartupBusinessForm_Click" />
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <iframe id="IFrame_RelatedInformation" name="IFrame_RelatedInformation" src="TTRelatedDIYBusinessForm.aspx" runat="server" style="width: 100%; overflow: auto;"></iframe>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClientClick="window.frames['IFrame_RelatedInformation'].document.getElementById('BT_SaveXMLFile').click()" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 35%; top: 35%;">
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
