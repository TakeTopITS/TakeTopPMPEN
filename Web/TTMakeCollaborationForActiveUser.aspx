<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMakeCollaborationForActiveUser.aspx.cs"
    Inherits="TTMakeCollaborationForActiveUser" %>


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
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="bian">
                        <tr>
                            <td height="31" class="page_topbj">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="29">
                                                        <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                    </td>
                                                    <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,JianLiXieZuo%>"></asp:Label>
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
                                <table style="width: 100%;" align="left" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td align="right" style="padding: 5px 5px 0px 5px;">
                                            <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="padding: 5px 5px 5px 5px;">
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
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                  
                                                                <td width="5%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,BianHao%>" /></strong>
                                                                </td>
                                                                <td width="40%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,XieZuo%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="20%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ChuangJianShiJian%>"></asp:Label></strong>
                                                                </td>
                                                                <td align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                </td>
                                                                <td align="left">
                                                                    <strong>&nbsp</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="6" align="right">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="25"
                                                ShowHeader="false" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                OnItemCommand="DataGrid4_ItemCommand" OnPageIndexChanged="DataGrid4_PageIndexChanged"
                                                Width="100%">
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
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
                                                    <asp:BoundColumn DataField="CoID" HeaderText="ID">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="CoID" DataNavigateUrlFormatString="TTCollaborationDetailMain.aspx?CoID={0}"
                                                        DataTextField="CollaborationName" HeaderText="Collaboration" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="40%" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn DataField="CreateTime" HeaderText="CreationTime">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Status">
                                                        <ItemTemplate>
                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.CoID", "TTCollaborationRelatedDoc.aspx?RelatedID={0}") %>'
                                                                Target="_blank"><img src="ImagesSkin/Doc.gif" 
                                                                        class="noBorder" /></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" VerticalAlign="Middle" />
                                                    </asp:TemplateColumn>
                                                </Columns>

                                                <ItemStyle CssClass="itemStyle" />
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
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label6" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>

                                    <td style="width: 190px; padding: 0px 5px 0px 5px" valign="top" align="left">
                                        <asp:TreeView ID="TreeView1" runat="server" BorderColor="Transparent" NodeWrap="True"
                                            OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" ShowLines="True" Width="190px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>

                                    <td width="170px" style="border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px; text-align: center; vertical-align: top;">
                                        <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="BT_AllActiveUser" CssClass="inpuLong" runat="server" Text="<%$ Resources:lang,ChaKanSuoYouZaiXianYongHu%>"
                                                        OnClick="BT_AllActiveUser_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; padding: 5px 5px 5px 5px; vertical-align: top;">
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
                                                                                <asp:Label ID="LB_UserType" runat="server" Text="<%$ Resources:lang,ZaiXianYongHu%>"></asp:Label></strong>
                                                                        </td>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid1_ItemCommand"
                                                        Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false">

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" Horizontalalign="left" />

                                                        <ItemStyle CssClass="itemStyle" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_UserCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>'
                                                                        CssClass="inpu" />
                                                                    <asp:Button ID="BT_UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                        CssClass="inpu" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td valign="top" align="left" style="padding: 5px 5px 5px 5px; border-right: solid 1px #D8D8D8;">
                                        <table style="width: 100%;" align="left" cellpadding="2" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; height: 24px">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_CoID" runat="server" Visible="false"></asp:Label><asp:TextBox ID="TB_CollaborationName"
                                                        runat="server" Width="90%" Text="<%$ Resources:lang,LinShiXiaoXi%>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  style="width: 15%; height: 24px" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,CanYuRenYuan%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Repeater ID="RP_Attendant" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                        <ItemTemplate>
                                                            <asp:Button ID="BT_UserName" runat="server" CssClass="inpuRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>' />
                                                            <asp:Button ID="BT_UserCode" runat="server" CssClass="inpuRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  style="width: 15%; height: 24px" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,NeiRong%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <CKEditor:CKEditorControl ID="HTEditor1" runat="server" Toolbar="TextColor|BGColor|Bold|Font|FontSize|Image" Width="99%" Height="150px" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  style="width: 15%; height: 24px" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                </td>
                                                <td  style="height: 21px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_Status" runat="server" Text="<%$ Resources:lang,XinJian%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 21px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left" style="height: 24px;" class="formItemBgStyleForAlignLeft">

                                                    <asp:Button ID="BT_Close" runat="server" Enabled="False"
                                                        CssClass="inpuClose" ToolTip="<%$ Resources:lang,GuanBi%>" OnClick="BT_Close_Click" />
                                                    &nbsp;<asp:Button ID="BT_Active" runat="server" Enabled="False" CssClass="inpuActive" ToolTip="<%$ Resources:lang,JiHuo%>"
                                                        OnClick="BT_Active_Click" />
                                                    &nbsp;&nbsp;<asp:HyperLink ID="HL_RelatedDoc"
                                                        runat="server" Enabled="False" Visible="false" NavigateUrl="~/TTProjectRelatedDoc.aspx" Target="_blank">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XiangGuanWenJian%>"></asp:Label>
                                                    </asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 29px;" class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:CheckBox ID="CB_MSM" runat="server" Text="<%$ Resources:lang,FaXinXi%>" />
                                                    <asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,FaYouJian%>" />
                                                    &nbsp;
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,TongZhiXieZuoRenYuan%>"></asp:Label>
                                                    <asp:TextBox ID="TB_Message" runat="server" Width="35%"></asp:TextBox>
                                                    <asp:Button ID="BT_Send" runat="server" Text="<%$ Resources:lang,FaSong%>" Enabled="False" CssClass="inpu"
                                                        OnClick="BT_Send_Click" />
                                                </td>
                                            </tr>
                                        </table>

                                    </td>



                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
    <p>
        <asp:Label ID="LB_Sql1" runat="server" Visible="False"></asp:Label>
    </p>
    <p>
        <asp:Label ID="LB_Sql2" runat="server" Visible="False"></asp:Label>
    </p>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
