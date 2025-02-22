<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTPnPCameraView.aspx.cs" Inherits="TTPnPCameraView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����ͷ�б�</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">

        function OnClickHandler() {
            if ($("#RD_Department").attr("checked")) {
                //alert("��֯���ܱ�");
                window.location.href = "TTPnPCameraAdd.aspx";
            } else {
                //alert("��Ŀ��");
                window.location.href = "TTPnPCameraProjectAdd.aspx";
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="AboveDiv">
            <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <table width="300" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="29">
                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                            </td>
                                            <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,SheXiangTouLieBiao%>"></asp:Label>
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
                                    <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                        <tr style="font-size: 12pt;">
                                            <td align="right" class="formItemBgStyle">
                                                <span>
                                                    <input type="radio" value="Department" id="RD_Department" name="add" checked="checked" /><asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZuZhiJiaGou%>"></asp:Label></span>
                                                <span>
                                                    <input type="radio" value="Project" id="RD_Project" name="add" /><asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMuBu%>"></asp:Label></span>&nbsp;&nbsp;<input type="button" class="inpuLong" value="��������ͷ" onclick="OnClickHandler();" />
                                            </td>
                                        </tr>
                                        <tr style="font-size: 12pt">
                                            <td align="left" style="width: 10%;" class="formItemBgStyle">
                                                <asp:DataGrid ID="DG_CameraList" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="True"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="TypeName" HeaderText="����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DPName" HeaderText="��֯�ܹ�����Ŀ��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CameraName" HeaderText="����ͷ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ServerIP" HeaderText="IP">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CameraUserName" HeaderText="����ͷ�û���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CameraPass" HeaderText="����ͷ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CreatorName" HeaderText="������">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CreateTime" HeaderText="����ʱ��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <a href='<%# Eval("CameraType").ToString() =="1" ? "TTPnPCameraAdd.aspx" : "TTPnPCameraProjectAdd.aspx" %>?id=<%# DataBinder.Eval(Container.DataItem,"ID") %>'>
                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,BianJi%>"></asp:Label></a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                </asp:DataGrid>
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
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
