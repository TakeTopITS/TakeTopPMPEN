<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTUserInfoExtend.aspx.cs" Inherits="TTUserInfoExtend" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
    <script type="text/javascript" src="js/Silverlight.js"></script>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
    <script src="js/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript">
</script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="HF_UserCode" />
                <asp:HiddenField runat="server" ID="HF_JoinDate" />
                <asp:Label ID="LB_UserCode" runat="server" Visible="false"></asp:Label>

                <div id="AboveDiv">
                    <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                        <tr>
                            <td height="31" class="page_topbj">
                                <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" width="380">
                                            <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="29">
                                                        <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                    </td>
                                                    <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                        <asp:Label ID="LB_SelectUserCode" runat="server"></asp:Label>
                                                        &nbsp;&nbsp; 
                                                        <asp:Label ID="LB_SelectUserName" runat="server"></asp:Label>&nbsp;
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,YuanGongDanAnSeZhi%>"></asp:Label>��2��
                                                    </td>
                                                    <td width="5">
                                                        <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="text-align: center; padding-top: 5px;">
                                            <asp:Button runat="server" ID="BT_Save" CssClass="inpuYello " Visible="true" Text="����" OnClick="BT_Save_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="text-align: center; padding-left: 20px;">

                                <table class="formBgStyle" style="width: 90%; text-align: left;" class="formBgStyle"
                                    cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td style="width: 15%; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label4" runat="server" Text="1������"></asp:Label></td>
                                        <td style="width: 35%; text-align: left;" class="formItemBgStyle">
                                            <asp:TextBox ID="TB_TopDepartCode" runat="server"></asp:TextBox>
                                            <asp:Label ID="LB_TopDepartName" runat="server"></asp:Label>
                                            <cc1:ModalPopupExtender ID="ModalPopupExtender5"
                                                runat="server" Enabled="True" TargetControlID="TB_TopDepartCode" PopupControlID="PanelExtend3"
                                                CancelControlID="IMB_CloseDepartment" BackgroundCssClass="modalBackground" Y="150">
                                            </cc1:ModalPopupExtender>
                                        </td>
                                        <td style="width: 15%; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label5" runat="server" Text="˾��"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="TB_EntryTotalYearMonth" Width="100" runat="server" ReadOnly></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label10" runat="server" Text="�칫�ص�"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_OfficeAddress" runat="server">
                                                <asp:ListItem Value="����" Text="����" />
                                                <asp:ListItem Value="����" Text="����" />
                                                <asp:ListItem Value="�ո�" Text="�ո�" />
                                                <asp:ListItem Value="����" Text="����" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 28px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label16" runat="server" Text="Ա������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_UserTypeExtend" runat="server">
                                                <asp:ListItem Value="NoType" Text="NoType" />
                                                <asp:ListItem Value="Full-time" Text="Full-time" />
                                                <asp:ListItem Value="Part-time" Text="Part-time" />
                                                <asp:ListItem Value="Internship" Text="Internship" />
                                                <asp:ListItem Value="LaborDispatch" Text="LaborDispatch" />
                                                <asp:ListItem Value="RetirementRehire" Text="RetirementRehire" />
                                                <asp:ListItem Value="LaborOutsourcing" Text="LaborOutsourcing" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label1" runat="server" Text="Ա��״̬"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_UserState" runat="server">
                                                <asp:ListItem Value="����" Text="����" />
                                                <asp:ListItem Value="��ʽ" Text="��ʽ" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 28px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label2" runat="server" Text="������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_ProbationPeriod" runat="server">
                                                <asp:ListItem Value="��������" Text="��������" />
                                                <asp:ListItem Value="1����" Text="1����" />
                                                <asp:ListItem Value="2����" Text="2����" />
                                                <asp:ListItem Value="3����" Text="3����" />
                                                <asp:ListItem Value="4����" Text="4����" />
                                                <asp:ListItem Value="5����" Text="5����" />
                                                <asp:ListItem Value="6����" Text="6����" />
                                                <asp:ListItem Value="Other" Text="Other" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label3" runat="server" Text="ʵ��ת������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_TurnOfficialDate" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_TurnOfficialDate" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td style="height: 28px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label6" runat="server" Text="��������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_HouseRegisterType" runat="server">
                                                <asp:ListItem Value="LocalUrban" Text="LocalUrban" />
                                                <asp:ListItem Value="LocalRural" Text="LocalRural" />
                                                <asp:ListItem Value="Non-localUrban(OutofProvince)" Text="Non-localUrban(OutofProvince)" />
                                                <asp:ListItem Value="Non-localRural(OutofProvince)" Text="Non-localRural(OutofProvince)" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label7" runat="server" Text="������ò"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_PoliticalOutlook" runat="server">
                                                <asp:ListItem Value="��Ա" Text="��Ա" />
                                                <asp:ListItem Value="��Ա" Text="��Ա" />
                                                <asp:ListItem Value="Ⱥ��" Text="Ⱥ��" />
                                                <asp:ListItem Value="Other" Text="Other" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label20" runat="server" Text="��ϵ�˹�ϵ"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_UrgencyRelation" runat="server">
                                                <asp:ListItem Value="��ĸ" Text="��ĸ" />
                                                <asp:ListItem Value="��ż" Text="��ż" />
                                                <asp:ListItem Value="��Ů" Text="��Ů" />
                                                <asp:ListItem Value="Other" Text="Other" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label9" runat="server" Text="��ͬ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_ContractType" runat="server">
                                                <asp:ListItem Value="LaborContract" Text="LaborContract" />
                                                <asp:ListItem Value="InternshipAgreement" Text="InternshipAgreement" />
                                                <asp:ListItem Value="RehireAgreement" Text="RehireAgreement" />
                                                <asp:ListItem Value="NoContractorAgreement" Text="NoContractorAgreement" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label8" runat="server" Text="��ͬ��˾"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="TB_ContractCompany" Width="90%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label11" runat="server" Text="�״κ�ͬ��ʼ��"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_FirstContractStartTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_FirstContractStartTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label12" runat="server" Text="�״κ�ͬ������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_FirstContractEndTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4" runat="server" TargetControlID="DLC_FirstContractEndTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label26" runat="server" Text="�״κ�ͬ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_FirstContractYears" runat="server">
                                                <asp:ListItem Value="1��" Text="1��" />
                                                <asp:ListItem Value="2��" Text="2��" />
                                                <asp:ListItem Value="3��" Text="3��" />
                                                <asp:ListItem Value="�޹̶���" Text="�޹̶���" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label13" runat="server" Text="�ڶ��κ�ͬ��ʼ��"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_SecondContractStartTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_SecondContractStartTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label14" runat="server" Text="�ڶ��κ�ͬ������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_SecondContractEndTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender5" runat="server" TargetControlID="DLC_SecondContractEndTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label27" runat="server" Text="�ڶ��κ�ͬ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_SecondContractYears" runat="server">
                                                <asp:ListItem Value="1��" Text="1��" />
                                                <asp:ListItem Value="2��" Text="2��" />
                                                <asp:ListItem Value="3��" Text="3��" />
                                                <asp:ListItem Value="�޹̶���" Text="�޹̶���" />
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label15" runat="server" Text="�����κ�ͬ��ʼ��"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_ThirdContractStartTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender6" runat="server" TargetControlID="DLC_ThirdContractStartTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label17" runat="server" Text="�����κ�ͬ������"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_ThirdContractEndTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender7" runat="server" TargetControlID="DLC_ThirdContractEndTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label28" runat="server" Text="�����κ�ͬ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_ThirdContractYears" runat="server">
                                                <asp:ListItem Value="1��" Text="1��" />
                                                <asp:ListItem Value="2��" Text="2��" />
                                                <asp:ListItem Value="3��" Text="3��" />
                                                <asp:ListItem Value="�޹̶���" Text="�޹̶���" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label18" runat="server" Text="��ǩ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="TB_SignContractCount" Width="90%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label19" runat="server" Text="�ֺ�ͬ��ʼ��"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:TextBox ID="DLC_ContractStartTime" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender8" runat="server" TargetControlID="DLC_ContractStartTime" Enabled="True"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label29" runat="server" Text="�ֺ�ͬ����"></asp:Label></td>
                                        <td class="formItemBgStyle">
                                            <asp:DropDownList ID="DL_ContractYears" runat="server">
                                                <asp:ListItem Value="1��" Text="1��" />
                                                <asp:ListItem Value="2��" Text="2��" />
                                                <asp:ListItem Value="3��" Text="3��" />
                                                <asp:ListItem Value="�޹̶���" Text="�޹̶���" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label21" runat="server" Text="���֤(������)"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyle">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:FileUpload ID="FUL_IdcardFrontFile" runat="server" Width="150px" />&nbsp;
                            <asp:Button ID="BT_IdcardFrontFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_IdcardFrontFile_Click" CssClass="inpu" Enabled="true" />
                                                    <asp:Literal ID="LT_IdcardFrontFileText" runat="server"></asp:Literal>
                                                    <br />
                                                    <asp:Image runat="server" ID="IMG_IdcardFrontFile" Width="370" Height="260" Visible="false" />
                                                    <asp:HiddenField ID="HF_IdcardFrontFileName" runat="server" />
                                                    <asp:HiddenField ID="HF_IdcardFrontFileUrl" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BT_IdcardFrontFile"></asp:PostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label22" runat="server" Text="���֤(������)"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyle">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:FileUpload ID="FUL_IdcardBackFile" runat="server" Width="150px" />&nbsp;
                            <asp:Button ID="BT_IdcardBackFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_IdcardBackFile_Click" CssClass="inpu" Enabled="true" />
                                                    <asp:Literal ID="LT_IdcardBackFileText" runat="server"></asp:Literal>
                                                    <br />
                                                    <asp:Image runat="server" ID="IMG_IdcardBackFile" Width="370" Height="260" Visible="false" />
                                                    <asp:HiddenField ID="HF_IdcardBackFileName" runat="server" />
                                                    <asp:HiddenField ID="HF_IdcardBackFileUrl" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BT_IdcardBackFile"></asp:PostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label23" runat="server" Text="ѧ��֤��"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyle">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:FileUpload ID="FUL_AcademicFile" runat="server" Width="150px" />&nbsp;
                                    <asp:Button ID="BT_AcademicFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_AcademicFile_Click" CssClass="inpu" Enabled="true" />
                                                    <asp:Literal ID="LT_AcademicFileText" runat="server"></asp:Literal>
                                                    <asp:HiddenField ID="HF_AcademicFileName" runat="server" />
                                                    <asp:HiddenField ID="HF_AcademicFileUrl" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BT_AcademicFile"></asp:PostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label24" runat="server" Text="ѧλ֤��"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyle">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:FileUpload ID="FUL_DegreeFile" runat="server" Width="150px" />&nbsp;
                                    <asp:Button ID="BT_DegreeFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_DegreeFile_Click" CssClass="inpu" Enabled="true" />
                                                    <asp:Literal ID="LT_DegreeFileText" runat="server"></asp:Literal>
                                                    <asp:HiddenField ID="HF_DegreeFileName" runat="server" />
                                                    <asp:HiddenField ID="HF_DegreeFileUrl" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BT_DegreeFile"></asp:PostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; text-align: left;" class="formItemBgStyle">
                                            <asp:Label ID="Label25" runat="server" Text="ǰ��˾��ְ֤��"></asp:Label>
                                        </td>
                                        <td colspan="3" class="formItemBgStyle">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:FileUpload ID="FUL_PreviousLeaveFile" runat="server" Width="150px" />&nbsp;
                    <asp:Button ID="BT_PreviousLeaveFile" runat="server" Text="<%$ Resources:lang,ShangChuan%>" OnClick="BT_PreviousLeaveFile_Click" CssClass="inpu" Enabled="true" />
                                                    <asp:Literal ID="LT_PreviousLeaveFileText" runat="server"></asp:Literal>
                                                    <asp:HiddenField ID="HF_PreviousLeaveFileName" runat="server" />
                                                    <asp:HiddenField ID="HF_PreviousLeaveFileUrl" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BT_PreviousLeaveFile"></asp:PostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>

                                </table>

                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Panel ID="PanelExtend3" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                        <table>
                            <tr>
                                <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                    <asp:TreeView ID="TreeView3" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView3_SelectedNodeChanged"
                                        ShowLines="True" Width="99%">
                                        <RootNodeStyle CssClass="rootNode" />
                                        <NodeStyle CssClass="treeNode" />
                                        <LeafNodeStyle CssClass="leafNode" />
                                        <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                    </asp:TreeView>
                                </td>
                                <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="center">
                                    <asp:ImageButton ID="IMB_CloseDepartment" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>


            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>

</html>
