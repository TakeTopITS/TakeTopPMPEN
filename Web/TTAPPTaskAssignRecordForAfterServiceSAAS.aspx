<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPTaskAssignRecordForAfterServiceSAAS.aspx.cs" Inherits="TTAPPTaskAssignRecordForAfterServiceSAAS" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=1" />

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link id="flxappCss" href="css/flxapp.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        body {
            /*margin-top: 5px;*/
            /*background-image: url(Images/login_bj.jpg);*/
            background-repeat: repeat-x;
            font: normal 100% Helvetica, Arial, sans-serif;
        }
    </style>

    <style type="text/css">
        #AboveDiv {
            max-width: 1024px;
            width: expression (document.body.clientWidth >= 1024? "1024px" : "auto" ));
            min-width: 277px;
            width: expression (document.body.clientWidth <= 277? "277px" : "auto" ));
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>

    <link href="js/layer/mobile/need/layer.css" rel="stylesheet" />
    <script src="js/layer/mobile/layer.js"></script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
               /*  if (top.location != self.location) { } else { CloseWebPage(); }*/
        });


        function aHandler() {

            $("a").not(".notTab").each(function () {

                var title = $(this).html().replace('---&gt;', '').replace('--&gt;', '');

                var url = $(this).attr("href");
                var click = $(this).attr("onclick");


                //�ж��Ƿ���tree�����߷�ҳ
                if (click != "" && click != null && click != undefined) {
                    if (click.toLowerCase().indexOf("treeview") == -1 && url.toLowerCase().indexOf("lbt_delete") == -1) {
                        $(this).click(function () {

                            if (url.indexOf("TakeTopAPPMain") == -1 && url.indexOf("TTAppTask") == -1) {

                                popShowByURL(url, 800, 600,window.location);
                                return false;
                            }

                            //top.frames[0].frames[2].parent.frames["rightTabFrame"].popShowByURL(url, 800, 600,window.location);


                        });
                    }
                }
                else if (title != ">" && title != "<" && (title.toLowerCase().indexOf("img") == -1 || url.toLowerCase().indexOf("treeview") == -1 || url.indexOf("TTDocumentTreeView") != -1 || url.indexOf("TakeTopAPPMain") == -1 || url.toLowerCase().indexOf("lbt_delete") == -1) && title != null && title != "" && title != "&gt;" && title != "&lt;") {
                    $(this).click(function () {
                        if (title.toLowerCase().indexOf("icon_del") == -1 && url.toLowerCase().indexOf("javascript") == -1) {

                            if (url.indexOf("TakeTopAPPMain") == -1 && url.indexOf("TTAppTask") == -1) {

                                popShowByURL(url, 800, 600,window.location);
                                return false;
                            }

                            //top.frames[0].frames[2].parent.frames["rightTabFrame"].popShowByURL(url, 800, 600,window.location);


                        }
                    });
                }

            });
        }

        function preview1() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint1-->";
            eprnstr = "<!--endprint1-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            document.body.innerHTML = bdhtml;
            return false;
        }

    </script>

</head>
<body>
    <script type="text/javascript" language="javascript">

        var txtQrCode = '#<%=TB_QrCode.ClientID%>';
        var btnFind = '#<%=BT_Find.ClientID%>';


        var loadingIndex; //��ʾ��index
        var isWxConfigReady = false; //config�Ƿ���֤ͨ��
        $(function () {

            try {

                alert(signModel);

                if ('<%=signModel %>' == null) {
                    return;
                }

                if ('<%=signModel.appId %>' == '') {
                    return;
                }

                var ids = "," + "@Model.MenuIds" + ",";
                $("a[id^='my_a_']").each(function (i, item) {
                    var val = $(this).attr("id").replace("my_a_", "");
                    if (ids.indexOf("," + val + ",") == -1) {
                        $(this).hide();
                    }
                });
                wxApi();

                //ɾ�����ⵯ����
                if (isWxConfigReady == false) {
                    var m = document.getElementById("layui-layer1");
                    m.parentNode.removeChild(m);

                    var k = document.getElementById("layui-layer-shade1");
                    k.parentNode.removeChild(k);
                }
            }
            catch
            {
            }
        });


        function wxApi() {
            var loadingIndex = layer.open({
                type: 2
                // , content: 'ImagesSkin/Processing.gif'
            });
            wx.config({
                debug: false, // ��������ģʽ,���õ�����api�ķ���ֵ���ڿͻ���alert��������Ҫ�鿴����Ĳ�����������pc�˴򿪣�������Ϣ��ͨ��log���������pc��ʱ�Ż��ӡ��
                appId: '<%=signModel.appId %>', // ������ںŵ�Ψһ��ʶ
                timestamp: '<%=signModel.time %>', // �������ǩ����ʱ���(�����д)
                nonceStr: '<%=signModel.randstr %>', // �������ǩ���������(�����д)
                signature: '<%=signModel.signstr %>', // ���ǩ��������¼1

                jsApiList: [
                    'checkJsApi',
                    'onMenuShareTimeline',
                    'onMenuShareAppMessage',
                    'onMenuShareQQ',
                    'onMenuShareWeibo',
                    'hideMenuItems',
                    'showMenuItems',
                    'hideAllNonBaseMenuItem',
                    'showAllNonBaseMenuItem',
                    'translateVoice',
                    'startRecord',
                    'stopRecord',
                    'onRecordEnd',
                    'playVoice',
                    'pauseVoice',
                    'stopVoice',
                    'uploadVoice',
                    'downloadVoice',
                    'chooseImage',
                    'previewImage',
                    'uploadImage',
                    'downloadImage',
                    'getNetworkType',
                    'openLocation',
                    'getLocation',
                    'hideOptionMenu',
                    'showOptionMenu',
                    'closeWindow',
                    'scanQRCode',
                    'chooseWXPay',
                    'openProductSpecificView',
                    'addCard',
                    'chooseCard',
                    'openCard'
                    //,


                    //'openEnterpriseChat',
                    //'openEnterpriseContact',
                    //'onMenuShareQZone',
                    //'onVoiceRecordEnd',
                    //'onVoicePlayEnd',
                    //'translateVoice',


                ] // �����Ҫʹ�õ�JS�ӿ��б�����JS�ӿ��б����¼2
            });


            wx.ready(function () {
                layer.close(loadingIndex);
                // config��Ϣ��֤���ִ��ready���������нӿڵ��ö�������config�ӿڻ�ý��֮��config��һ���ͻ��˵��첽���������������Ҫ��ҳ�����ʱ�͵�����ؽӿڣ��������ؽӿڷ���ready�����е�����ȷ����ȷִ�С������û�����ʱ�ŵ��õĽӿڣ������ֱ�ӵ��ã�����Ҫ����ready�����С�
                isWxConfigReady = true;
            });
            wx.error(function (res) {
                layer.close(loadingIndex);
                alert(JSON.stringify(res));
                // config��Ϣ��֤ʧ�ܻ�ִ��error��������ǩ�����ڵ�����֤ʧ�ܣ����������Ϣ���Դ�config��debugģʽ�鿴��Ҳ�����ڷ��ص�res�����в鿴������SPA�������������ǩ����
            });
        }

        function qrcode() {
            wx.scanQRCode({
                needResult: 1, // Ĭ��Ϊ0��ɨ������΢�Ŵ���1��ֱ�ӷ���ɨ������
                scanType: ["qrCode", "barCode"], // ����ָ��ɨ��ά�뻹��һά�룬Ĭ�϶��߶���
                success: function (res) {
                    var result = res.resultStr; // ��needResult Ϊ 1 ʱ��ɨ�뷵�صĽ��
                    if (typeof (result) != "undefined") {

                        result = result.substring(result.indexOf(',') + 1, result.length);

                        //�ı���ֵ	
                        $(txtQrCode).val(result);
                        //�����ѯ��ť
                        $(btnFind).click();
                    }
                }
            });

        }

    </script>

    <form id="form1" runat="server">
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                    <tr>
                        <td height="31" class="page_topbj">
                            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <a href="TakeTopAPPMain.aspx" target="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';">
                                            <table width="245" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="29">
                                                        <img src="ImagesSkin/return.png" alt="" />
                                                    </td>
                                                    <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titleziAPP">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,Back%>" />
                                                    </td>
                                                    <td width="5">
                                                        <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <img id="IMG_Waiting" src="Images/Processing.gif" alt="���Ժ򣬴�����..." style="display: none;" />
                                        </a>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="padding: 5px 2px  0px 5px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,fenpaiRen%>" />��
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_AssignManName" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="padding-left: 10px;">
                                        <asp:Button ID="BT_FindAll" runat="server" CssClass="inpuQuery" OnClick="BT_FindAll_Click" />

                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="padding-left: 10px;">

                                        <asp:Button ID="BT_Qrcode" runat="server" CssClass="inpuQrCode" OnClientClick="qrcode()" />
                                        <asp:TextBox ID="TB_QrCode" runat="server" Style="display: none;"></asp:TextBox>
                                        <asp:Button ID="BT_Find" runat="server" Style="display: none;" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Find_Click" />

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--startprint1-->
                            <table style="width: 98%" align="left">
                                <tr>
                                    <td align="left">
                                        <asp:Image ID="IMG_QrCode" runat="server" />
                                        <asp:DataList ID="DataList2" runat="server" Width="100%" OnItemCommand="DataList2_ItemCommand"
                                            DataKeyField="ID" CellPadding="0" ForeColor="#333333">
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <ItemTemplate>

                                                <div class="npb npbs">
                                                    <table width="100%" cellpadding="4" cellspacing="0" class="bian">
                                                        <tr>
                                                            <td style="width: 30%; text-align: right;">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JiLuBianHao%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"ID") %>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                            </td>
                                                            <td style="font-size: 10pt; text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"Status") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,GongZuoYaoQiu%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left">
                                                                <b>
                                                                    <%# DataBinder.Eval(Container.DataItem,"Operation") %>
                                                                </b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <td colspan="2" style="text-align: center;">
                                                                <%--<asp:Button ID="BT_StartupBusinessForm" runat="server" CssClass ="inpulong" Text="<%$ Resources:lang,XiangGuanYeWuDan%>" />--%>

                                                                <a onclick="popShowByURL('TTRelatedDIYBusinessForm.aspx?RelatedType=TaskRecord&RelatedID=<%# DataBinder.Eval(Container.DataItem,"ID") %>&IdentifyString=<%# ShareClass.GetWLTemplateIdentifyString( ShareClass.getBusinessFormTemName("TaskRecord",Eval("ID").ToString ()))%>'>, 800, 600,window.location);"
                                                                    href='TTRelatedDIYBusinessForm.aspx?RelatedType=TaskRecord&RelatedID=<%# DataBinder.Eval(Container.DataItem,"ID") %>&IdentifyString=<%# ShareClass .GetWLTemplateIdentifyString( ShareClass. getBusinessFormTemName("TaskRecord",Eval("ID").ToString ()))%>'>
                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:Label>
                                                                </a>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"BeginDate","{0:yyyy/MM/dd}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:yyyy/MM/dd}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,FenPaiRen%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left; font-size: 10pt">
                                                                <%# DataBinder.Eval(Container.DataItem,"AssignManName") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShouLiRen%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"OperatorCode") %>
                                                                <%# DataBinder.Eval(Container.DataItem,"OperatorName") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShouLiRenShiJian%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <%# DataBinder.Eval(Container.DataItem,"OperationTime","{0:yyyy/MM/dd}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,QianJiLu%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left; font-size: 10pt">
                                                                <%# DataBinder.Eval(Container.DataItem,"PriorID") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,FeiYong%>"></asp:Label>��</td>
                                                            <td style="text-align: left;"><%# DataBinder.Eval(Container.DataItem,"Expense") %></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label>��</td>
                                                            <td style="text-align: left;"><%# DataBinder.Eval(Container.DataItem,"finishPercent") %>%</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">&nbsp;</td>
                                                            <td style="text-align: left; font-size: 10pt">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GongZuoRiZhi%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left">
                                                                <b><%# DataBinder.Eval(Container.DataItem,"OperatorContent") %> </b>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="height: 13px; text-align: right"></td>
                                                        </tr>
                                                    </table>
                                                </div>

                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemStyle" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                            <!--endprint1-->
                            <br />
                        </td>
                    </tr>
                </table>
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
