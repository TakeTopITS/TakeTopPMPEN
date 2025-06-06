<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTUserInfor_StudentAttendance.aspx.cs" Inherits="TTUserInfor_StudentAttendance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>学生考勤</title>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XueShengKaoQing%>"></asp:Label>
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
                                                    <tr style="font-size: 12pt">
                                                        <td class="formItemBgStyleForAlignLeft" width="100%">
                                                            <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                                <tr style="font-size: 12pt">
                                                                    <td  style="width: 45%; padding: 5px 5px 5px 5px; font-size: 12px;" class="formItemBgStyleForAlignLeft" valign="top">

                                                                        <table class="formBgStyle" width="100%">

                                                                            <tr>
                                                                                <td  class="formItemBgStyleForAlignLeft" colspan="2">
                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,Nian%>"></asp:Label><asp:DropDownList ID="DDL_Year" runat="server"></asp:DropDownList>&nbsp;
                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,Yue%>"></asp:Label><asp:DropDownList ID="DDL_Month" runat="server"></asp:DropDownList>&nbsp;
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td  class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XuanZheWenJian%>"></asp:Label></td>
                                                                                <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                                    <asp:FileUpload ID="fileExcel" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                                                                    <asp:Button ID="btnImport" runat="server" Text="<%$ Resources:lang,DaoRu%>" OnClick="btnImport_Click" CssClass="inpu" />&nbsp;
                                                                                    <asp:HyperLink ID="HyperLink1" NavigateUrl="Template/学生考勤数据导入表.xls" Text="<%$ Resources:lang,ShuJuDaoRuMuBan%>" runat="server">
                                                                                       
                                                                                    </asp:HyperLink>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-top: 5px;" class="formItemBgStyleForAlignLeft" colspan="4">
                                                                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft" width="80%">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,Nian%>"></asp:Label><asp:DropDownList ID="DDL_SYear" runat="server"></asp:DropDownList>&nbsp;
                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,Yue%>"></asp:Label><asp:DropDownList ID="DDL_SMonth" runat="server"></asp:DropDownList>&nbsp;
                                                                        <asp:Button ID="BT_Seach" runat="server" Text="<%$ Resources:lang,ChaXun%>" CssClass="inpu" OnClick="BT_Seach_Click" /><br />
                                                                        <asp:DataGrid ID="DG_ProjectMemberStudentAttendance" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="30" ShowHeader="True"
                                                                            Width="100%" OnItemCommand="DG_ProjectMemberStudentAttendance_ItemCommand" OnPageIndexChanged="DG_ProjectMemberStudentAttendance_PageIndexChanged">
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_ID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                                                                            CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>' CssClass="inpu" />

                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="StudentName" HeaderText="学生名称">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AttendanceTime" HeaderText="日期">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="IsStudy" HeaderText="是否上学">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_ProjectMemberStudentAttendanceSql" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
