<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDWelderStatusReport.aspx.cs" Inherits="TTGDWelderStatusReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����״������</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { 

            

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
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,HanGongZhuangKuangBaoGao%>"></asp:Label>
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
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,HanGongZhuangTaiBaoGaoDaDi%>"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <font style="color: black;">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,JDDHGDHJJTHZB%>"></asp:Label></font>
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
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DaDiHanGong%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JiBie%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,HanJieRiQi%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">.<asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,DanXianTuHao%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JieTouShuLiang%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ChiCun%>"></asp:Label></th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">S/F</th>
                                                            <th style="text-align: center" class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,GaiMianHanGong%>"></asp:Label></th>
                                                        </tr>
                                                        <tr>
                                                            <th style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="9">
                                                                <hr />
                                                            </th>
                                                        </tr>
                                                        <asp:Repeater ID="rptList" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <font style="color: black;"><%# Container.ItemIndex+1 %></font>&nbsp;<%# Eval("Welders")%>
                                                                    </td>
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
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Mold")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Size")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("SF")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("CoveringWelder1")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td  class="formItemBgStyleForAlignLeft" colspan="9">
                                                                <hr />
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
