<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProRelatedRisk.aspx.cs"
    Inherits="TTProRelatedRisk" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                                                        <asp:Label ID="LB_tiProjectRisk" runat="server" Text="<%$ Resources:lang,RelatedRisk%>"></asp:Label>
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
                            <td align="left">
                                <table style="width: 100%; margin-top: 5px">
                                    <tr>
                                        <td rowspan="3" width="220px" style="border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px" valign="top" align="left">
                                            <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                ShowLines="True" Width="220px" NodeWrap="True">
                                                <RootNodeStyle CssClass="rootNode" />
                                                <NodeStyle CssClass="treeNode" />
                                                <LeafNodeStyle CssClass="leafNode" />
                                                <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                            </asp:TreeView>
                                        </td>
                                        <td width="100%">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                <tr>
                                                    <td align="right" style="padding-right: 5px;">
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
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                </td>
                                                                <td width="5%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                </td>

                                                                <td width="7%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgRiskID" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="8%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgRiskLevel" runat="server" Text="<%$ Resources:lang,Level%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="25%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgRiskName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="13%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgPredictEffectTime" runat="server" Text="<%$ Resources:lang,PredictEffectTime%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="13%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DiscoveryTime%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="10%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgToTask" runat="server" Text="<%$ Resources:lang,ToTask%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="6%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgStatus" runat="server" Text="<%$ Resources:lang,Status%>"></asp:Label></strong>
                                                                </td>
                                                                <td align="left" width="5%">
                                                                    <strong></strong>
                                                                </td>

                                                                <%--   <td align="left">
                                                                    <strong>&nbsp;
                                                                    </strong>
                                                                </td>--%>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="6" align="right">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowHeader="false" Height="1px" OnItemCommand="DataGrid1_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                PageSize="8" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                                                    <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="7%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RiskLevel" HeaderText="Level">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Risk" HeaderText="Risk">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="EffectDate" HeaderText="预计发生时间" DataFormatString="{0:yyyy/MM/dd}">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="13%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="EffectDate" HeaderText="发现时间" DataFormatString="{0:yyyy/MM/dd}">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="13%" />
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <a href='TTRiskToTask.aspx?RiskID=<%#DataBinder.Eval (Container .DataItem ,"ID") %>&ProjectID=<%#DataBinder.Eval (Container .DataItem ,"ProjectID") %>'>
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZhuanChengRenWu%>"></asp:Label>
                                                            </a>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Status">
                                                        <ItemTemplate>
                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ID", "TTRiskRelatedDoc.aspx?RelatedID={0}") %>'
                                                                Target="_blank"><img src ="ImagesSkin/Doc.gif" class="noBorder" /></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                                    </asp:TemplateColumn>

                                                    <%--                                                      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Other" Text="- - -">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                    </asp:ButtonColumn>--%>
                                                </Columns>
                                            </asp:DataGrid>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 100%; text-align: left;">
                                            <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                            <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                            <asp:Label ID="LB_UserCode" runat="server"
                                                Visible="False"></asp:Label>
                                            <asp:Label ID="LB_UserName" runat="server"
                                                Visible="False"></asp:Label>
                                        </td>
                                    </tr>

                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label10" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <table style="width: 95%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                <tr>
                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_RiskLevel" runat="server" Text="<%$ Resources:lang,Level%>"></asp:Label>
                                        :</td>
                                    <td style="width: 20%;" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_RiskLevel" runat="server">
                                            <asp:ListItem Value="Medium" Text="<%$ Resources:lang,Zhong%>" />
                                            <asp:ListItem Value="High" Text="<%$ Resources:lang,Gao%>" />
                                            <asp:ListItem Value="Low" Text="<%$ Resources:lang,Di2%>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbStatus" runat="server" Text="<%$ Resources:lang,Status%>"></asp:Label>:
                                    </td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_Status" runat="server">
                                            <asp:ListItem Value="Potential" Text="<%$ Resources:lang,QianZai%>" />
                                            <asp:ListItem Value="Exposed" Text="<%$ Resources:lang,BaoLu%>" />
                                            <asp:ListItem Value="Occurred" Text="<%$ Resources:lang,FaSheng%>" />
                                            <asp:ListItem Value="Resolved" Text="<%$ Resources:lang,JieChu%>" />
                                            <asp:ListItem Value="ToTask" Text="<%$ Resources:lang,ZhuanRen%>" />
                                        </asp:DropDownList>
                                        <asp:Label ID="LB_ID" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbRiskName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label>:
                                    </td>
                                    <td colspan="5" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_RiskName" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 61px;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbDescription" runat="server" Text="<%$ Resources:lang,Description%>"></asp:Label>:
                                    </td>
                                    <td colspan="5" style="height: 61px;" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_RiskDetail" runat="server" Height="50px" TextMode="MultiLine"
                                            Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbPredictEffectTime" runat="server" Text="<%$ Resources:lang,PredictEffectTime%>"></asp:Label>:
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:TextBox ID="DLC_EffectDate" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_EffectDate">
                                        </ajaxToolkit:CalendarExtender>

                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbFindTime" runat="server" Text="<%$ Resources:lang,DiscoveryTime%>"></asp:Label>:
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:TextBox ID="DLC_FindDate" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_FindDate">
                                        </ajaxToolkit:CalendarExtender>

                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="2"></td>
                                </tr>

                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popOtherWindow" name="noConfirm"
                        style="z-index: 9999; width: 800px; height: 100px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title111">
                            <asp:Label ID="Label2" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content112" class="layui-layer-content" style="overflow: auto; text-align: left; padding: 0px 5px 0px 5px;">

                            <table cellpadding="3" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td align="left">
                                        <table style="width: 100%;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td colspan="5" style="height: 4px;" class="formItemBgStyleForAlignLeft">

                                                    <asp:HyperLink ID="HL_RiskToTask" runat="server" Enabled="false"
                                                        Target="_blank" Text="<%$ Resources:lang,ToTask%>"></asp:HyperLink>


                                                    &nbsp;

                                                    <asp:HyperLink ID="HL_RiskReviewWF" runat="server" Enabled="false"
                                                        Target="_blank" Text="<%$ Resources:lang,hlRiskReview%>"></asp:HyperLink>
                                                    &nbsp;
                                                        <asp:HyperLink ID="HL_ActorGroup" runat="server" Enabled="False" NavigateUrl="~/TTProjectRelatedActorGroup.aspx" Target="_blank" Text="<%$ Resources:lang,ActorGroup%>"></asp:HyperLink>

                                                    &nbsp;
                                                    <asp:HyperLink ID="HL_RunRiskByWF" runat="server" Enabled="false" Visible="false"
                                                        Target="_blank" Text="<%$ Resources:lang,hlRunRiskByWF%>"></asp:HyperLink>
                                                    &nbsp;
                                                         <asp:HyperLink ID="HL_RelatedWorkFlowTemplate" runat="server" Enabled="false" NavigateUrl="TTProRelatedWFTemplate.aspx" Target="_blank" Text="<%$ Resources:lang,RelatedWFTemplate%>" Visible="false"></asp:HyperLink>
                                                    &nbsp;
                                                          <asp:HyperLink ID="HL_WLTem" runat="server" Enabled="False" NavigateUrl="~/TTWorkFlowTemplate.aspx" Target="_blank" Text="<%$ Resources:lang,TemplateMaintain%>" Visible="false"></asp:HyperLink>

                                                    &nbsp;   
                                                    <asp:HyperLink ID="HL_RiskRelatedDoc" runat="server" Visible="false" Enabled="false"
                                                        Target="_blank" Text="<%$ Resources:lang,RelatedDoc%>"></asp:HyperLink>



                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer111" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
