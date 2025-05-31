<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZSupplierRegister.aspx.cs" Inherits="TTWZSupplierRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����ע��</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">

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
                <div style="width: 100%;">
                   
                       
                                    <table class="formBgStyle" width="100%">
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_SupplierName" runat="server" Width="450px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KaiHuHang%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_OpeningBank" runat="server" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,FaRenDaiBiao%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_PersonDelegate" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhangHao%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_AccountNumber" runat="server" Width="450px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DanWeiDianHua%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_UnitPhone" runat="server"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShuiHao%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_RateNumber" runat="server" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DanWeiDiZhi%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_UnitAddress" runat="server" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YouBian%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_ZipCode" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,RuWangWenJian%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:FileUpload ID="FUP_InDocument" runat="server" />
                                                <asp:Button ID="BT_InDocument" runat="server" Text="<%$ Resources:lang,ShangChuan%>" CssClass="inpu" OnClick="BT_InDocument_Click" /><br />
                                                <asp:Literal ID="LT_InDocument" runat="server"></asp:Literal>
                                                <asp:HiddenField ID="HF_InDocument" runat="server" />
                                                <asp:HiddenField ID="HF_InDocumentURL" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,WeiTuoDaiLiRen%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_DelegateAgent" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,GuDingDianHua%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_ContactPhone" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShouJi%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_Mobile" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">QQ��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_QQ" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">E-mail��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_Email" runat="server" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,TuiJianRen%>"></asp:Label>��</td>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TXT_PushPerson" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="2">
                                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                                                        <input type="button" runat ="server"  Text="<%$ Resources:lang,FanHui%>" id="BT_Return" class="inpu" onclick="window.location.href = 'TTWZSupplierList.aspx'" />

                                            </td>
                                        </tr>
                                    </table>

                                    <%--<td class="formItemBgStyleForAlignLeft" style="width:20%; display:none;">
                                                            <div style="width: 500px;">
                                                                <asp:DataGrid ID="DG_SupplierTemplateFile" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="false"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="TemplateFileName" HeaderText="ģ������">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="80%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:TemplateColumn>
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="20%" />
                                                                            <HeaderTemplate>
                                                                                ����
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>

                                                                                <a href='<%# Eval("TemplateFileURL") %>' class="notTab" target="_blank">����</a>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>--%>
                                             
                  
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
                <asp:PostBackTrigger ControlID="BT_InDocument" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
