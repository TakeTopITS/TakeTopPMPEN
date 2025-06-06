<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsTypeSet.aspx.cs" Inherits="TTGoodsTypeSet" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>会计科目设置</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
            color: #CC0000;
        }
        </style>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }
            
        });
    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,LeiXingSheZhi %>"></asp:Label>
                                                    </td>
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
                            <td>
                                <table style="width: 98%; margin-top:5px">
                                    <tr>
                                        <td valign="top" align="left" style="width: 25%; text-align: left; padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">
                                            <asp:TreeView ID="TreeView1" runat="server" Font-Bold="False" Font-Names="宋体" Font-Size="10pt" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" ShowLines="True" Width="100%">
                                                <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                            </asp:TreeView>
                                        </td>
                                        <td>
                                            <table style="width:99%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,FuLei%>"></asp:Label>：<asp:TextBox ID="TB_GoodsParentType" runat="server" CssClass="shuru" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：<asp:TextBox ID="TB_GoodsTypeName" runat="server" Width="200px" CssClass="shuru"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,TeZhengMa%>"></asp:Label>：<asp:TextBox ID="TB_GoodsTypeChar" runat="server" Width="200px" CssClass="shuru"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,PaiXu%>"></asp:Label>：<asp:TextBox ID="TB_SortNumber" runat="server" Width="200px" CssClass="shuru"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Button CssClass="inpu" ID="BT_GoodsTypeAdd" runat="server"  Text="<%$ Resources:lang,XinZeng%>" OnClick="BT_GoodsTypeAdd_Click" />
                                                        &nbsp;<asp:Button CssClass="inpu" ID="BT_GoodsTypeUpdate" runat="server" Text="<%$ Resources:lang,BaoCun%>" OnClick="BT_GoodsTypeUpdate_Click" />
                                                        &nbsp;<asp:Button CssClass="inpu" ID="BT_GoodsTypeDelete" runat="server" Text="<%$ Resources:lang,ShanChu%>" OnClick="BT_GoodsTypeDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" />
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
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
