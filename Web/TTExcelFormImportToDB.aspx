<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTExcelFormImportToDB.aspx.cs" Inherits="TTExcelFormImportToDB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1800px;
            width: expression (document.body.clientWidth <= 1800? "1800px" : "auto" ));
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
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" width="175px">
                                                <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,LiuChengShuJiChaXun%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="������"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_FormType" runat="server" Width="100px"></asp:TextBox> 
                                                            <asp:DropDownList ID="DL_FormType" Width="80px" DataValueField="FormType" DataTextField="FormType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_FormType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                          
                                                            <asp:Label ID="Label17" runat="server" Text="����"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_FormName" runat="server" Width="80px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="����"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_FormCode" runat="server" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="BT_Delete" runat="server" CssClass="inpu" OnClick="BT_Delete_Click" Text="<%$ Resources:lang,ShanChu%>" />

                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="�д�"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_RowCode" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="����"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_FieldName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" Text="��ֵ"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_FieldValue" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" Text="�ϴ���"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_OperatorName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>

                                                        <td>
                                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td align="right">
                                                            <asp:Label ID="Label22" runat="server" Text="���ϴ�"></asp:Label>��

                                                        </td>
                                                        <td align="left" colspan="9">
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div>
                                                                        <Upload:InputFile ID="FileUpload_Training" runat="server" Width="400px" />

                                                                        <asp:Button ID="btn_ExcelToDB" runat="server" CssClass="inpu" OnClick="btn_ExcelToDB_Click" Text="<%$ Resources:lang,DaoRuShuJu%>" />
                                                                        <a href="Template/����Դ����ʽ.xls"><strong style="font-size:smaller;color:white;"><asp:Label ID="Label23" runat="server" Text="����ʽ����"></asp:Label></strong></a>
                                                                        <div id="ProgressBar">
                                                                            <Upload:ProgressBar ID="ProgressBar1" runat="server" Height="100px" Width="500px">
                                                                            </Upload:ProgressBar>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btn_ExcelToDB" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <asp:Label ID="LB_ErrorText" runat="server" ForeColor="white"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td></td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0" align="center">
                                        <tr>
                                            <td style="width: 100%; padding: 5px 5px 5px 10px; text-align: left;" valign="top">
                                                <b>
                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ChaXunJeGuo%>"></asp:Label>��</b>
                                                <asp:Label ID="LB_ResultCount" runat="server"></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center; padding: 5px 5px 5px 5px;" valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" alt="" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="���(id)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="������(formtype)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="����(formcode)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="����(formname)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label12" runat="server" Text="�б�ʶ��(rowcode)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label13" runat="server" Text="����(fieldname)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="13%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label14" runat="server" Text="��ֵ(fieldvalue)"></asp:Label></strong>
                                                                    </td>

                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label16" runat="server" Text="�ϴ���(operatorcode)"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label18" runat="server" Text="�ϴ�ʱ��(operatetime)"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                    PageSize="200" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FormType" HeaderText="������">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FormCode" HeaderText="����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FormName" HeaderText="����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RowCode" HeaderText="�к�">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FieldName" HeaderText="����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FieldValue" HeaderText="��ֵ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="13%" />
                                                        </asp:BoundColumn>

                                                        <asp:BoundColumn DataField="OperatorName" HeaderText="�ϴ�������">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="center" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="OperateTime" HeaderText="�ϴ�ʱ��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                    </Columns>

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
