<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDWeekWorkloadReport.aspx.cs" Inherits="TTGDWeekWorkloadReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
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
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZhouGongZuoLiangBaoGao%>"></asp:Label>
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
                                            <td>
                                                <table style="width: 80%;" cellpadding="0" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <h2>
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,MZGZLBGAQY%>"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <font style="color: black;">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,SJHJJTQYBG%>"></asp:Label></font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Form<asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,Cong%>"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;To<asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,Dao%>"></asp:Label>��
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <div style="width: 1300px;">
                                                    <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                        <tr>
                                                            <th style="text-align: center; width: 20%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 20%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,HanJieXingShi%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 20%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,HanJieDB%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 20%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,HanJieJieTou%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 20%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,GuanXianChangDu%>"></asp:Label></th>
                                                        </tr>
                                                        <tr>
                                                            <th style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="5">
                                                                <hr />
                                                            </th>
                                                        </tr>
                                                        <asp:Repeater ID="rptList" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <font style="color: black;"><%# Container.ItemIndex+1 %></font>&nbsp;<%# Eval("Welders")%></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Status")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("RanderTime")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Isom_no")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("JointNo")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,CunKouHeJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,HanKouGeShu%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YanChangXian%>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZongJi%>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
