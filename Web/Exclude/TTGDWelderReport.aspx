<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDWelderReport.aspx.cs" Inherits="TTGDWelderReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�����ǼǱ�</title>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HanGongDengJiBiao%>"></asp:Label>
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
                                                            <h2>WELDER SUMMARY LIST</h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <font style="color: black;">LIST OF QUALIFIED WELDERS</font>
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
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">WELDER No.</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">NAME</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">STATUS</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">WQT DATE</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">APPLICATION NO.</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">TYPE OF<br />
                                                                QUALIFICATION</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">WELDING<br />
                                                                PIPE</th>
                                                            <th style="text-align: center; width: 8%;" class="formItemBgStyleForAlignLeft">POSITIONS<br />
                                                                PLATE</th>
                                                            <th style="text-align: center" class="formItemBgStyleForAlignLeft">REMARKS</th>
                                                        </tr>
                                                        <tr>
                                                            <th style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="9">
                                                                <hr />
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th  class="formItemBgStyleForAlignLeft" colspan="9">��CNPC-SCC��   Total��<asp:Literal ID="LT_Count" runat="server"></asp:Literal></th>
                                                        </tr>
                                                        <asp:Repeater ID="rptList" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <font style="color: black;"><%# Container.ItemIndex+1 %></font>&nbsp;<%# Eval("Welders")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("WelderName")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Status")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%#Eval("PublicTime","{0:yyyy-MM-dd}")%><%--<%# Eval("PublicTime")%>--%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("RequestCode")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Qualification")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("WeldPosition1")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("WeldPosition2")%>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%# Eval("Remarks")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td  class="formItemBgStyleForAlignLeft" colspan="9">
                                                                <table class="formBgStyle" style="width: 100%;">
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft" rowspan="3">WCS</td>
                                                                        <td class="formItemBgStyleForAlignLeft">BTES</td>
                                                                        <td class="formItemBgStyleForAlignLeft" >Report Name:WelderNoReport</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">ProjectNo��MDI TRAIN LOT:04</td>
                                                                        <td class="formItemBgStyleForAlignLeft" >Page 1 of 2</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft">ProjectName NITRIC ACID</td>
                                                                        <td class="formItemBgStyleForAlignLeft" >
                                                                            <asp:Literal ID="LT_Time" runat="server"></asp:Literal></td>
                                                                    </tr>
                                                                </table>
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
