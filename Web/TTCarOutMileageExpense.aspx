<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCarOutMileageExpense.aspx.cs"
    Inherits="TTCarOutMileageExpense" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <%-- <style type="text/css">
        #AboveDiv
        {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>--%>
    <script type="text/javascript">

        var disPostion = 0;

        function SaveScroll() {
            disPostion = CarAssignListDivID.scrollTop;
        }

        function RestoreScroll() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            CarAssignListDivID.scrollTop = disPostion;
        }
    </script>
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
                                    <table width="98%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChuCheLiChengFeiYong%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label ID="LB_UserCode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LB_UserName" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LB_ID" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" width="100%">
                                        <tr>
                                            <td valign="top" style="padding: 5px 5px 5px 5px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="400px" style="border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px;">
                                                            <div id="CarAssignListDivID" style="width: 100%; height: 550px; overflow: auto;">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                    <tr>
                                                                        <td width="7">
                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="25%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="30%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChePaiHaoMa%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="30%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,SiJi%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="15%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
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
                                                                    Height="1px" OnItemCommand="DataGrid1_ItemCommand" ShowHeader="False" PageSize="15"
                                                                    OnPageIndexChanged="DataGrid1_PageIndexChanged" Width="100%" CellPadding="4"
                                                                    ForeColor="#333333" GridLines="None">

                                                                    <Columns>
                                                                        <asp:TemplateColumn HeaderText="Number">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="25%" />
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="CarCode" HeaderText="车牌号">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="30%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:HyperLinkColumn DataNavigateUrlField="DriverCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                            DataTextField="DriverName" HeaderText="Name" Target="_blank">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="30%" />
                                                                        </asp:HyperLinkColumn>
                                                                        <asp:TemplateColumn HeaderText="Status">
                                                                            <ItemTemplate>
                                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>
                                                        <td align="left">
                                                            <table border="1" style="width: 98%; border-collapse: collapse; margin: 0px auto;"
                                                                cellpadding="4" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                                        <b>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LiChengFeiYong%>"></asp:Label></b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                                        <table>
                                                                            <tr>
                                                                                <td class="formItemBgStyleForAlignLeft" >
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZongLiCheng%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Mileage" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,GuoLuFei%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_RoadToll" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,TingCheFei%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ParkingCharge" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,JiaYou%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_OilVolume" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,JiaYouFei%>"></asp:Label>：
                                                                                </td>
                                                                                <td>
                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_OilCharge" runat="server" Width="80px">0.00</NickLee:NumberBox>
                                                                                </td>
                                                                                <td></td>
                                                                                <td>
                                                                                    <asp:Button ID="BT_UpdateDetail" runat="server" CssClass="inpu" OnClick="BT_UpdateDetail_Click"
                                                                                        Text="<%$ Resources:lang,BaoCun%>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <asp:DataList ID="DataList1" runat="server" Height="1px" Width="98%" CellPadding="0"
                                                                CellSpacing="0">
                                                                <ItemTemplate>
                                                                    <table border="1" style="width: 100%; border-collapse: collapse; margin: 0px auto;"
                                                                        cellpadding="4" cellspacing="0">
                                                                        <tr>
                                                                            <td colspan="4"  class="formItemBgStyleForAlignLeft">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" style="height: 30px; text-align: left">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td width="8%" align="right">
                                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZongLiCheng%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "Mileage")%>
                                                                                        </td>
                                                                                        <td width="10%" align="right">
                                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,DangCiLiCheng%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "CurrentMileage")%>
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GuoLuFei%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "RoadToll")%>
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,TingCheFei%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "ParkingCharge")%>
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiaYou%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "OilVolume")%>
                                                                                        </td>
                                                                                        <td width="8%" align="left">
                                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,JiaYouFei%>"></asp:Label>：
                                                                                        </td>
                                                                                        <td width="10%" align="left">
                                                                                            <%#DataBinder.Eval(Container.DataItem, "OilCharge")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" height="40px"  class="formItemBgStyleForAlignLeft"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 15%; text-align: right">
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="width: 35%; text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "ID")%>
                                                                            </td>
                                                                            <td style="width: 15%; text-align: right">
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ChePaiHaoMa%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "CarCode")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShenQingBuMen%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "DepartName")%>
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "ApplicantName")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,SiJi%>"></asp:Label>：
                                                                            </td>
                                                                            <td colspan="3" style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "DriverName")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YongCheShiJian%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "DepartTime", "{0:yyyy/MM/dd HH:MM}")%>
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,HuanCheShiJian%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "BackTime", "{0:yyyy/MM/dd HH:MM}")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShangCheDiDian%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "BoardingSite")%>
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,MuDiDi%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "Destination")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,SuiCheRen%>"></asp:Label>：
                                                                            </td>
                                                                            <td colspan="3" style="height: 18px; text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "Attendant")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="height: 60px; text-align: right">
                                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ChuCheYuanYin%>"></asp:Label>：
                                                                            </td>
                                                                            <td colspan="3" style="height: 60px; text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "Comment")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：
                                                                            </td>
                                                                            <td colspan="3" style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "Status")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: center" colspan="4">
                                                                                <b>出入时间</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="15%" style="text-align: right">
                                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ChuFaShiJian%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="width: 35%; text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "RealDepartTime")%>
                                                                            </td>
                                                                            <td width="15%" style="text-align: right">
                                                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShouCheShiJian%>"></asp:Label>：
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <%#DataBinder.Eval(Container.DataItem, "RealBackTime")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" style="height: 40px; text-align: left"></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
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
