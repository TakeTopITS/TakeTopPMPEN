<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTDeletedDocumentView.aspx.cs"
    Inherits="TTDeletedDocumentView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1024px;
            width: expression (document.body.clientWidth <= 1024? "1024px" : "auto" ));
        }
    </style>
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
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,YiShanChuWenJianGuanLi%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="margin-top: 5px; width: 98%;">
                                        <tr>
                                            <td style="background: #f0f0f0; width: 100%;" align="left">
                                                <table>
                                                    <tr>
                                                        <td width="100px" align="right">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ZongHeChaXun%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:Label ID="Label1234" runat="server" Text="<%$ Resources:lang,WenJianMing%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_HazyFind" runat="server" Width="150px"></asp:TextBox></td>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ShangChuanZhe%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_UploaderName" runat="server" Width="150px"></asp:TextBox></td>
                                                        <td>&nbsp;
                                                            <asp:Button ID="BT_UploaderFind" runat="server" OnClick="BT_UploaderFind_Click" Text="<%$ Resources:lang,ChaXun%>"
                                                                CssClass="inpu" /></td>
                                                        <td align ="right">
                                                                 <asp:Label ID="LB_Count" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 1px; text-align: center;">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>

                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DaLei%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,FuLei%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="25%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,GongSi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShangChuanZhe%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShangChuanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                    OnItemCommand="DataGrid1_ItemCommand" Width="100%" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="DocID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RelatedType" HeaderText="MajorCategory">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="父类">
                                                            <ItemTemplate>
                                                                <%# ShareClass . getDocParentTypeByID(Eval("DocTypeID").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="DocType" HeaderText="Type">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="Address" DataNavigateUrlFormatString="{0}"
                                                            DataTextField="DocName" HeaderText="文件名" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="UploadManCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                            DataTextField="UploadManName" HeaderText="上传者" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="UploadTime" HeaderText="上传时间">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Address" Visible="False"></asp:BoundColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_Recover" CssClass="inpu" runat="server" Text="<%$ Resources:lang,QuXiaoShanChu%>" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                           
                                             
                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
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
