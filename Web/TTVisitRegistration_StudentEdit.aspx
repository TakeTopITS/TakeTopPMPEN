<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTVisitRegistration_StudentEdit.aspx.cs" Inherits="TTVisitRegistration_StudentEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�ݷ�</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">

        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            


        });




        function LoadParentLit() {



            if (navigator.userAgent.indexOf("Firefox") >= 0) {

                alert("firefox");

                window.parent.LoadProjectList();

            }
            else {
                alert("other");

                window.parent.LoadProjectList();

            }

            CloseLayer();
        }



    </script>

    <script language=javascript>
        var PortOpened = 0;
        var CpuCardFound = 0;
        function Idcard() {
            var result;
            var photoname;
            var cardname;
            var cardsn;
            try {
                var ax = new ActiveXObject("IDRCONTROL.IdrControlCtrl.1");
                //    alert("�Ѱ�װ");  
            } catch (e) {
                alert("�ؼ�δ��װ");
            }
            //ע�⣺��һ������Ϊ��Ӧ���豸�˿ڣ�USB��Ϊ1001��������Ϊ1��16
            result = IdrControl1.ReadCard("1001", "d:\\test\\test.jpg");
            //	result=IdrControl1.ReadCard("1001","");
            if (result == 1) {
                

                $("#TXT_VisitName").val(IdrControl1.GetName());

                $("#LB_Name").text(IdrControl1.GetName());

                $("#LB_Han").text(IdrControl1.GetFolk());
                $("#LB_Sex").text(IdrControl1.GetSex());

                $("#DDL_VisitSex").val(IdrControl1.GetSex());

                $("#LB_Birth").text(IdrControl1.GetBirthYear() + "��" + IdrControl1.GetBirthMonth() + "��" + IdrControl1.GetBirthDay() + "��");
                cardsn = IdrControl1.GetCode();
                $("#TXT_VisitCardName").val(cardsn);

                $("#LB_Card").text(cardsn);

                $("#LB_Address").text(IdrControl1.GetAddress());
                $("#LB_SignZF").text(IdrControl1.GetAgency());
                $("#LB_EffectTime").text(IdrControl1.GetValid());
                
                photoname="d:\\test\\"+"Z"+cardsn+".jpg";
                cardname="d:\\test\\"+"Z"+cardsn+"card.jpg";

                
                //alert(photoname);
                result = IdrControl1.ExportPhoto.ExportPhoto(photoname, cardname);

                $("#cardImg").attr("src", photoname);
                //if (result!=1){
                //			alert(result);
                //	}

                document.all.notbook.src = "pic.htm";

                //alert(IdrControl1.GetSexN());
                //alert(IdrControl1.GetFolkN());

            } else {
                if (result == -1)
                    alert("�˿ڳ�ʼ��ʧ�ܣ�");
                if (result == -2)
                    alert("�����½���Ƭ�ŵ��������ϣ�");
                if (result == -3)
                    alert("��ȡ����ʧ�ܣ�");
                if (result == -4)
                    alert("������Ƭ�ļ�ʧ�ܣ������趨·���ʹ��̿ռ䣡");

            }
        }
        function ICcard() {
            var result;

            //ע�⣺����Ϊ��Ӧ���豸�˿ڣ�iDR210Ϊ8159��iDR200 USB��Ϊ1001��iDR200������Ϊ1��16
            result = IdrControl1.ReadICCard("8159");
            if (result == 1) {
                document.all.oTable.rows(13).cells(1).innerText = IdrControl1.GetICCardSN();
            }
            else {
                if (result == -1)
                    alert("�˿ڳ�ʼ��ʧ�ܣ�");
                if (result == -2)
                    alert("��IC��ʧ��");
            }
        }


        function Picflesh() {
            document.all.notbook.src = "pic.htm";
        }

        function FindCPUCard() {
            document.all.oTable.rows(14).cells(1).innerText = "";
            PortOpened = IdrControl1.InitComm(1001);
            CpuCardFound = 0;
            if (PortOpened != 1) {

                IdrControl1.CloseComm();
                alert("�˿ڳ�ʼ��ʧ�ܣ�");
                PortOpened = 0;
                return;
            }


            CpuCardFound = IdrControl1.FindICCard();
            document.all.oTable.rows(14).cells(1).innerText = "�ҵ������ͣ�" + CpuCardFound;

            if (CpuCardFound0 == 0) {
                IdrControl1.CloseComm();
                alert("δ�ҵ�����");
                CpuCardFound = 0;
                return;
            }
            if (CpuCardFound0 > 3) {
                IdrControl1.CloseComm();
                alert("�ҿ�ʧ�ܣ�");
                CpuCardFound = 0;
                return;
            }
        }

        

</script>
</head>

    
<body>
    <form id="form1" runat="server">

                <div>
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
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BaiFangDengJi%>"></asp:Label>
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
                                                    <tr>
                                                        <td style="width: 100%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft" valign="top">
                                                            <table class="formBgStyle" style="width: 80%;">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,LaiYuanShiJian%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_VisitStartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label> </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_VisitName" runat="server"></asp:TextBox>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DDL_VisitSex" runat="server">
                                                                            <asp:ListItem Text="<%$ Resources:lang,Nan%>" Value="Male"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,Nv%>" Value="Female"/>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LaiFangRenZhengJian%>"></asp:Label> </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_VisitCardName" runat="server" Width="216px"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LaiYuanYuanYin%>"></asp:Label> </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%--<asp:TextBox ID="TXT_VisitReason" runat="server" Width="413px"></asp:TextBox>--%>
                                                                        <asp:DropDownList ID="DDL_VisitReason" runat="server">
                                                                            <asp:ListItem Text="<%$ Resources:lang,CanGuan%>" Value="�ι�"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,QiaTan%>" Value="Ǣ̸"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,QiTa%>" Value="Other"/>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JieDaiRen%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <%--<asp:TextBox ID="TXT_ReceiverName" runat="server"></asp:TextBox>--%>
                                                                        <asp:DropDownList ID="DDL_ReceiverName" runat="server">
                                                                            <asp:ListItem Text="<%$ Resources:lang,HouQin%>" Value="����"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,JiaoXue%>" Value="��ѧ"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,YuanZhang%>" Value="԰��"/>
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,LiYuanShiJian%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_VisitEndTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="6" style="text-align: center;">
                                                                        <asp:Button ID="BT_Save" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="BT_Save_Click" />&nbsp;
                                                                        <%--<input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                            type="button" value="Closed" />--%>
                                                                        <input type="button" value="Back" class="inpuLong" onclick="window.location.href = 'TTVisitRegistration_StudentList.aspx'" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table id="oTable" width="800" border="1" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <input type="button" value="ReadCardID" onclick="Idcard();"></td>
                                                                </tr>
                                                                <tr style="display:none;">
                                                                    <td width="200">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhaoPian%>"></asp:Label> </td>
                                                                    <td width="500">
                                                                        <img id="cardImg" />
                                                                        &nbsp;</td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label> </td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Name" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,MingZhu%>"></asp:Label> </td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Han" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label> </td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Sex" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ChuSheng%>"></asp:Label> </td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Birth" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShengFengZhengHaoMa%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Card" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label> </td>
                                                                    <td>
                                                                        <asp:Label ID="LB_Address" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,QianFaJiGuan%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="LB_SignZF" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YouXiaoRiQi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="LB_EffectTime" runat="server" Text=""></asp:Label>
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
   
    </form>


    <object classid="clsid:5EB842AE-5C49-4FD8-8CE9-77D4AF9FD4FF" id="IdrControl1" width="100" height="100" >
</object>

</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
