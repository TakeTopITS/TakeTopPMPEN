<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectMemberClassList.aspx.cs" Inherits="TTProjectMemberClassList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�༶</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">

        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BanJi%>"></asp:Label>
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
                                <td style="padding: 0px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <table style="width: 80%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td style="width: 100%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft">

                                                            <table class="formBgStyle" width="80%">
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_ID" runat="server" ReadOnly="true"></asp:TextBox><font color="red">*<asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ZiDongShengCheng%>"></asp:Label></font>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,NianJi%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DDL_Grade" runat="server"></asp:DropDownList>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,BanJi%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_ClassName" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignRight" colspan="6">
                                                                        <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,XinJian%>" CssClass="inpu" OnClick="BT_Create_Click" />&nbsp;
                                                                        <asp:Button ID="BT_Edit" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="BT_Edit_Click" />&nbsp;
                                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:lang,QuXiao%>" CssClass="inpu" OnClick="btnCancel_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="6">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,BanJiJiLu%>"></asp:Label> <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,Gong%>"></asp:Label><asp:Label ID="LB_RecordCount" runat="server" Text=""></asp:Label><asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,Tiao%>"></asp:Label>&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <div style="width:60%;">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                    <tr>
                                                                        <td width="7">
                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="10%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="20%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="20%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,NianJi%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="50%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BanJi%>"></asp:Label></strong>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width="6" align="right">
                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:DataGrid ID="DG_List" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="20" ShowHeader="false"
                                                                    Width="100%" OnItemCommand="DG_List_ItemCommand" OnPageIndexChanged="DG_List_PageIndexChanged">
                                                                    <Columns>
                                                                        <asp:TemplateColumn HeaderText="">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>' CommandName="edit" CssClass="notTab">
                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BianJi%>"></asp:Label></asp:LinkButton>
                                                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>' CommandName="del" CssClass="notTab">
                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShanChu%>"></asp:Label></asp:LinkButton>
                                                                                
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="GradeName" HeaderText="�꼶">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ClassName" HeaderText="�༶">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="50%" />
                                                                        </asp:BoundColumn>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                                                </asp:DataGrid>
                                                                <asp:Label ID="LB_SQL" runat="server" Text="" Visible="false"></asp:Label>
                                                            </div>
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
                </div>
                <asp:HiddenField ID="HF_ID" runat="server" />
                 <asp:HiddenField ID="HiddenField1" runat="server" />
                 <asp:Label ID="LB_DepartString" runat="server" Visible ="false"></asp:Label>
                 <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" 
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + '<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css / ' + cssDirectory + ' / ' + 'bluelightmain.css';</script>;

</html>
