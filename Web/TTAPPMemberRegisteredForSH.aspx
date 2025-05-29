<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPMemberRegisteredForSH.aspx.cs" Inherits="TTAPPMemberRegisteredForSH" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />
<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--    <link href="css/zhuce.css" rel="stylesheet" type="text/css">--%>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            background-repeat: repeat-x;
            font: normal 100% Helvetica, Arial, sans-serif;
        }

        #AboveDiv {
            max-width: 1024px;
            width: expression (document.body.clientWidth >= 1024? "1024px" : "auto" ));
            min-width: 277px;
            width: expression (document.body.clientWidth <= 277? "277px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
        });

        //检测设备是否是移动端
        function detectmob() {
            if (navigator.userAgent.match(/Android/i)
                || navigator.userAgent.match(/webOS/i)
                || navigator.userAgent.match(/iPhone/i)
                || navigator.userAgent.match(/iPad/i)
                || navigator.userAgent.match(/iPod/i)
                || navigator.userAgent.match(/BlackBerry/i)
                || navigator.userAgent.match(/Windows Phone/i)
            ) {
                return true;
            }
            else {
                return false;
            }
        }


        function redirectToTarget() {

            if (detectmob()) {

                window.open("Logo/indexSAAS.html");
            }
            else {

                window.open("Logo/indexSAAS.html", "_parent");
            }
        }

    </script>

</head>
<body bgcolor="#FFFFFF">
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="AboveDiv" style="padding-left: 25px;">
                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="ImagesSkin/ZhuCeLogo.png">  </img>
                                            </td>
                                            <td style="vertical-align: middle; ">
                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,ShangHuiChengYuanDengJi%>"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft" >
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShouJiHao%>"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_UserCode" runat="server" Height="25px" Width="90%"></asp:TextBox>
                                    <span style="color: #ff0000">*</span>
                                    <br />
                                    <asp:Label ID="Label49" runat="server" Font-Size="X-Small" Text="<%$ Resources:lang,QSRZSSJHZWDLZHY%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft"  style="vertical-align: bottom;">
                                    <asp:Label ID="Label4" runat="server" Text="Name"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_UserName" runat="server" Height="25px" Width="90%"></asp:TextBox>
                                    <span style="color: #ff0000">*</span></td>
                            </tr>

                            <tr>
                                <td class="formItemBgStyleForAlignLeft"  style="vertical-align: bottom;">
                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem Value="Male" Text="<%$ Resources:lang,Nan%>" />
                                        <asp:ListItem Value="Female" Text="<%$ Resources:lang,Nv%>" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft"  style="vertical-align: bottom;">
                                    <asp:Label ID="Label7" runat="server" Text="Company"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_ChildDepartment" runat="server" Width="90%"></asp:TextBox>
                                    <span style="color: #ff0000">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft" >E_Mail</td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_EMail" runat="server" Height="25px" Width="90%"></asp:TextBox>
                                    <span style="color: #ff0000">*</span>
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Font-Size="X-Small" Text="<%$ Resources:lang,QSRZYDEMAILJSXXYHQHMMY%>"></asp:Label>
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td class="formItemBgStyleForAlignLeft"  style="vertical-align: bottom;">

                                    <asp:DropDownList ID="ddlLangSwitcher" runat="server" AutoPostBack="True" DataTextField="Language" DataValueField="LangCode" Width="90%" Height="30px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TB_CheckCode" runat="server" ForeColor="#cdcccc" Style="width: 100px; height: 32px;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Image ID="IM_CheckCode" runat="server" src="TTCheckCode.aspx" Style="width: 150px; height: 32px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <br />
                                    <asp:Button ID="BT_Add" runat="server" Height="30px" CssClass="inpuLongest" Width="90%" OnClick="BT_Add_Click" Text="<%$ Resources:lang, BaoCun%>" />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="BT_BackLoginPage" runat="server" Height="30px" CssClass="inpuLongest" Width="90%" OnClientClick="redirectToTarget()" Text="<%$ Resources:lang,Back%>" Visible="false" />

                                </td>
                            </tr>

                        </table>

                    </div>


                    <div style="display: none;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhiWu%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Duty" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft"><span style="color: #ff0000">*</span>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Image ID="IM_MemberPhoto" runat="server" AlternateText="Member'sPhoto" />
                                                <br />
                                                <br />
                                                <asp:Button ID="BT_TakePhoto" runat="server" CssClass="inpu"
                                                    Enabled="False" OnClick="BT_TakePhoto_Click" Text="<%$ Resources:lang,PaiZhao%>" />
                                                <cc1:ModalPopupExtender ID="BT_TakePhoto_ModalPopupExtender" runat="server"
                                                    BackgroundCssClass="modalBackground" Y="150" CancelControlID="IMBT_Close"
                                                    DynamicServicePath="" Enabled="True" PopupControlID="Panel2"
                                                    TargetControlID="BT_TakePhoto">
                                                </cc1:ModalPopupExtender>
                                                &nbsp;
                                                <asp:Button ID="BT_DeletePhoto" runat="server" CssClass="inpu"
                                                    Enabled="False" OnClick="BT_DeletePhoto_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td colspan="3"  class="formItemBgStyleForAlignLeft"><span style="color: red; font-size: 8pt;">
                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,DWBTXXGSMMWKJBGBYLMM%>"></asp:Label>
                                </span></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td colspan="3"  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                    ： </td>
                                <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DL_Status" runat="server">
                                        <asp:ListItem Value="Employed" Text="<%$ Resources:lang,ZaiZhi%>" />
                                        <asp:ListItem Value="Resign" Text="<%$ Resources:lang,LiZhi%>" />
                                        <asp:ListItem Value="Stop" Text="<%$ Resources:lang,ZhongZhi%>" />
                                    </asp:DropDownList>
                                    <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                    <asp:Button ID="BT_Update" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Update_Click" Text="<%$ Resources:lang,BaoCun%>" Visible="false" /></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Button ID="BT_Delete" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" Visible="false" /></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhaoPianShangChuan%>"></asp:Label>
                                    ： </td>
                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="FUP_File" runat="server" />
                                            <asp:Button ID="BT_UploadPhoto" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_UploadPhoto_Click"
                                                CssClass="inpu" Enabled="False" />
                                            <asp:HyperLink ID="HL_MemberPhoto" runat="server" Visible="False">[HL_MemberPhoto]</asp:HyperLink>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="BT_UploadPhoto"></asp:PostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label>
                                    ： </td>
                                <td  class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DL_Gender" runat="server" AutoPostBack="True">
                                        <asp:ListItem Selected="True" Value="Male" Text="<%$ Resources:lang,Nan%>" />
                                        <asp:ListItem Value="Female" Text="<%$ Resources:lang,Nv%>" />
                                    </asp:DropDownList></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,NianLing%>"></asp:Label>
                                    ： </td>
                                <td  class="formItemBgStyleForAlignLeft">
                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_Age" runat="server" Precision="0" OnBlur=""
                                        OnFocus="" OnKeyPress="" PositiveColor="">
                                        0</NickLee:NumberBox></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BuMen%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DL_Department" runat="server" DataTextField="DepartName"
                                        DataValueField="DepartCode" CssClass="DDList">
                                    </asp:DropDownList></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"&nbsp;></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ZiBuMen%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BanGongDianHua%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_OfficePhone" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td style="height: 28px;" class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShouJi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_MobilePhone" runat="server"></asp:TextBox></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"<span style="color: #ff0000">*</span></td>
                                <td style="height: 28px; "  class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td style="height: 23px;" class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,GongZuoFanWei%>"></asp:Label>
                                    ： </td>
                                <td colspan="3" class="auto-style1">
                                    <asp:TextBox ID="TB_WorkScope" runat="server" Width="99%"></asp:TextBox></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                    <asp:Label ID="Label134" runat="server" Text="<%$ Resources:lang,UnitSalary%>"></asp:Label>
                                    ：</td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <NickLee:NumberBox ID="NB_HourlySalary" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox></td>
                                <td class="formItemBgStyleForAlignLeft"  width="15%">
                                    <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,YueXin %>"></asp:Label>
                                    ：</td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <NickLee:NumberBox ID="NB_MonthlySalary" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,YongGongLeiXing%>"></asp:Label>
                                    ：</td>
                                <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DL_WorkType" runat="server" DataTextField="TypeName" DataValueField="TypeName"></asp:DropDownList></td>
                                <td style="height: 12px; "  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                            </tr>
                            <tr style="display: none;">
                                <td style="height: 12px; " class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiaRuRiQi%>"></asp:Label>
                                    ： </td>
                                <td colspan="2" style="height: 12px; "  class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="DLC_JoinDate" ReadOnly="false" runat="server"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                        runat="server" TargetControlID="DLC_JoinDate">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td style="height: 12px; "  class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,XingZhi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px;">
                                    <asp:DropDownList ID="DL_UserType" Enabled="false" runat="server">
                                        <asp:ListItem Value="INNER" Text="<%$ Resources:lang,NeiBu%>" />
                                        <asp:ListItem Value="OUTER" Text="<%$ Resources:lang,WaiBu%>" />
                                    </asp:DropDownList></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">&nbsp;</td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuJieMianFengGe%>"></asp:Label>
                                    ：</td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <asp:DropDownList ID="DL_SystemMDIStyle" runat="server" DataTextField="MDIStyle" DataValueField="MDIStyle"></asp:DropDownList></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; "></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,KeDengLuSheBei%>"></asp:Label>
                                    ：</td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <asp:DropDownList ID="DL_AllowDevice" runat="server" Enabled="false">
                                        <asp:ListItem Value="ALL" Text="<%$ Resources:lang,QuanBu%>" />
                                        <asp:ListItem Value="PC" Text="<%$ Resources:lang,DianNao%>" />
                                        <asp:ListItem Value="MOBILE" Text="<%$ Resources:lang,YiDongSheBei%>" />
                                    </asp:DropDownList></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; "></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,CanKaoGongHao%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <asp:TextBox ID="TB_RefUserCode" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; "></td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,RTXZhangHu%>"></asp:Label>
                                    ：</td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <asp:TextBox ID="TB_UserRTXCode" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">&nbsp;</td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">
                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShunXuHao%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_SortNumber" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                        PositiveColor="" Precision="0">
                                        0</NickLee:NumberBox></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; "></td>
                            </tr>
                            <tr style="display: none;">
                                <td style="height: 12px; " class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <table border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td colspan="2" align="left"><strong>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BuMenChengYuan%>"></asp:Label>
                                                        </strong></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" CellPadding="2" Font-Bold="true" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid2_ItemCommand" ShowHeader="false">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle CssClass="notTab" Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                        <ItemStyle CssClass="itemStyle" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Resources.lang.ZZXMZY">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                <ItemTemplate>
                                                    <asp:Button ID="BT_UserCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>' />
                                                    <asp:Button ID="BT_UserName" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">&nbsp;</td>
                            </tr>
                            <tr style="display: none;">
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">&nbsp;</td>
                                <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 12px; ">
                                    <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" ShowLines="True">
                                        <RootNodeStyle CssClass="rootNode" />
                                        <NodeStyle CssClass="treeNode" />
                                        <LeafNodeStyle CssClass="leafNode" />
                                        <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                    </asp:TreeView>
                                    <asp:Label ID="LB_DepartCode" runat="server" Visible="False"></asp:Label></td>
                                <td class="formItemBgStyleForAlignLeft" style="height: 12px; ">&nbsp;</td>
                            </tr>
                        </table>
                        <table class="formBgStyle" style="text-align: left; visibility: hidden;"
                            class="formBgStyle" cellpadding="3" cellspacing="0">
                            <tr>
                                <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,YingWenMing%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_EnglishName" runat="server"></asp:TextBox></td>
                                <td  width="15%" class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,MinZu%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Nationality" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,ChuShengDi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_NativePlace" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,HuKouSuoZaiDi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_HuKou" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,JuZhuDi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Residency" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label>
                                    ： </td>
                                <td colspan="3" class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Address" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShengRi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="DLC_BirthDay" ReadOnly="false" runat="server"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2"
                                        runat="server" TargetControlID="DLC_BirthDay">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,HunYinZhuangTai%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:DropDownList ID="DL_MaritalStatus" runat="server">
                                        <asp:ListItem Value="Unmarried" Text="<%$ Resources:lang,WeiHun%>" />
                                        <asp:ListItem Value="Married" Text="<%$ Resources:lang,YiHun%>" />
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,XueLi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Degree" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ZhuanYe%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Major" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,BiYeXueXiao%>"></asp:Label>
                                    ： </td>
                                <td colspan="3" class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_GraduateSchool" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_IDCard" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,XueXing%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_BloodType" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ShenGao%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <NickLee:NumberBox ID="NB_Height" runat="server" OnBlur="" OnFocus=""
                                        OnKeyPress="" PositiveColor="" MinAmount="0" Precision="0">
                                        0</NickLee:NumberBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,JinJiLianXiRen%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_UrgencyPerson" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,LianXiDianHua%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_UrgencyCall" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,JieShaoRen%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Introducer" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,JieShaoRenBuMen%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_IntroducerDepartment" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,YuJieShaoRenGuanXi%>"></asp:Label>
                                    ： </td>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_IntroducerRelation" runat="server"></asp:TextBox></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                                <td class="formItemBgStyleForAlignLeft"></td>
                            </tr>
                            <tr>
                                <td class="formItemBgStyleForAlignLeft">
                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>
                                    ： </td>
                                <td colspan="3" class="formItemBgStyleForAlignLeft">
                                    <asp:TextBox ID="TB_Comment" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                        </table>
                        <asp:TextBox ID="TB_PhotoString1" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_PhotoString2" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_PhotoString3" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:TextBox ID="TB_PhotoString4" runat="server" Style="visibility: hidden"></asp:TextBox>
                        <asp:Button ID="BT_SavePhoto" runat="server" CssClass="inpuLong" OnClick="BT_SavePhoto_Click"
                            Style="visibility: hidden" />
                        <asp:Label ID="LB_DepartString" runat="server" Visible="false"></asp:Label>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="dibupic"></div>
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
