<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPGetUserPositionForIOSAndroid.aspx.cs" Inherits="TTAPPGetUserPositionForIOSAndroid" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<meta http-equiv="Content-Type" content="textml; charset=UTF-8" />

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>

    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

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
            min-height: 640px;
            height: expression (document.body.clientHeight <= 640 "640px" : "auto" ));
        }

        #container {
            height: auto !important;
            height: 530px;
            min-height: 530px;
        }

        .round {
            border: 1px solid #dedede;
            border-radius: 10%;
        }
    </style>

    <script type="text/javascript" src="https://lib.sinaapp.com/js/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" src="https://api.map.baidu.com/api?v=2.0&ak=Mesj2KjbrDAqsfcUrFBY7DNrQ4GZAUS0"></script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <link href="js/layer/mobile/need/layer.css" rel="stylesheet" />
    <script src="js/layer/mobile/layer.js"></script>
    <script src="js/jweixin-1.0.0.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {



        });

    </script>

</head>
<body >
    <form id="form1" runat="server">
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>



        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
            <tr>
                <td  height="31" class="page_topbj">
                    <table width="99%">
                        <tr>
                            <td style="width: 50%; text-align: left;">
                                <a href="TakeTopAPPMain.aspx" target="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';">
                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="30" align="right">
                                                <img src="ImagesSkin/Return.png" alt="" />
                                            </td>
                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titleziAPP">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,Back%>" />
                                            </td>

                                            <td class="titleziAPP" style="width: 100px; text-align: center; padding-top: 5px;">
                                                <img id="IMG_Waiting" src="Images/Processing.gif" alt="���Ժ򣬴�����..." style="display: none;" />
                                            </td>
                                        </tr>
                                    </table>
                                </a>
                            </td>

                            <td style="vertical-align: middle; text-align: center;" class="round">
                                <asp:Button ID="BT_SavePosition" runat="server" CssClass="inpuLong" Height="30px" OnClick="BT_SavePosition_Click" Text="<%$ Resources:lang,DingWeiBingFanHuiZhuYe%>" />

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
           <tr>
                <td>
                    <table width="99%">
                        <tr>
                            <td style="width: 50%; text-align: left; vertical-align: middle;">
                                <asp:RadioButtonList ID="RBL_ShiftType" runat="server" TextAlign="Left" CellPadding="10" CellSpacing="10">
                                </asp:RadioButtonList></td>
                          <td style="vertical-align: middle; text-align: center;" class="round">
                                <br />
                                <br />
                                <asp:Button ID="BT_Attendance" runat="server" CssClass="inpuLong" Style="border: 1px solid #dedede; border-radius: 10%;" Width="80%" Height="30px" OnClick="BT_Attendance_Click" Text="<%$ Resources:lang,DaKaBingFanHuiZhuYe%>" />
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>

                </td>
            <tr>
                <td>

                    <table width="99%">
                        <tr>
                            <td alight="right">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,Jing%>"></asp:Label>��</td>
                            <td>
                                <input type="text" id="LNG_value" runat="server" style="width: 100px;" readonly></input>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,Wei%>"></asp:Label>��</td>
                            <td>
                                <input type="text" id="LAT_value" runat="server" style="width: 100px;" readonly></input>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="status" style="text-align: center">
                        <a href="javascript:window.history.go(-1)" target="_top" onclick="javascript:document.getElementById('IMG_Waiting').style.display = 'block';"></a>
                    </div>
                    <div id="container" style="width: 99%; border: 1px solid gray; margin: 5px auto"></div>
                </td>
            </tr>
        </table>
        <asp:Label ID="LB_Sql" runat="server"></asp:Label>

        <%--     </ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 50%; top: 50%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="Images/Processing.gif" alt="Loading,please wait..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>--%>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>


<script type="text/javascript">

    var isWxConfigReady = false; //config�Ƿ���֤ͨ��
    var loadingIndex;
    var mk, map;
    function wxApi() {


        var loadingIndex = layer.open({
            type: 2
            , content: 'ImagesSkin/Processing.gif'
        });
        wx.config({
            debug: false, // ��������ģʽ,���õ�����api�ķ���ֵ���ڿͻ���alert��������Ҫ�鿴����Ĳ�����������pc�˴򿪣�������Ϣ��ͨ��log���������pc��ʱ�Ż��ӡ��
            appId: '<%=signModel.appId %>', // ������ںŵ�Ψһ��ʶ
            timestamp: '<%=signModel.time %>', // �������ǩ����ʱ���(�����д)
            nonceStr: '<%=signModel.randstr %>', // �������ǩ���������(�����д)
            signature: '<%=signModel.signstr %>', // ���ǩ��������¼1

            jsApiList: [
                'getLocation',
                'openLocation'
            ] // �����Ҫʹ�õ�JS�ӿ��б�����JS�ӿ��б����¼2
        });


        wx.ready(function () {
            layer.close(loadingIndex);
            // config��Ϣ��֤���ִ��ready���������нӿڵ��ö�������config�ӿڻ�ý��֮��config��һ���ͻ��˵��첽���������������Ҫ��ҳ�����ʱ�͵�����ؽӿڣ��������ؽӿڷ���ready�����е�����ȷ����ȷִ�С������û�����ʱ�ŵ��õĽӿڣ������ֱ�ӵ��ã�����Ҫ����ready�����С�
            isWxConfigReady = true;

            wx.getLocation({
                type: 'wgs84', // Ĭ��Ϊwgs84��gps���꣬���Ҫ����ֱ�Ӹ�openLocation�õĻ������꣬�ɴ���'gcj02'
                success: function (res) {
                    var latitude = res.latitude; // γ�ȣ�����������ΧΪ90 ~ -90
                    var longitude = res.longitude; // ���ȣ�����������ΧΪ180 ~ -180��

                    var lng = '';	//�ٶȾ���
                    var lat = '';	//�ٶ�γ��
                    var convertor = new BMap.Convertor();
                    var ggPoint = new BMap.Point(longitude, latitude);
                    var pointArr = [];
                    pointArr.push(ggPoint);
                    convertor.translate(pointArr, 1, 5, function (data) {
                        if (data.status === 0) {
                            var point = data.points[0];
                            lng = point.lng;
                            lat = point.lat;

                            document.getElementById("LNG_value").value = lng;
                            document.getElementById("LAT_value").value = lat;

                            mk = new BMap.Marker(point);
                            map = new BMap.Map("container");
                            map.addOverlay(mk);
                            map.centerAndZoom(point, 18);

                        }
                        else {
                            alert('����ת��ʧ��');
                        }
                    });




                    //wx.openLocation({
                    //latitude: latitude, // γ�ȣ�����������ΧΪ90 ~ -90
                    //longitude: longitude, // ���ȣ�����������ΧΪ180 ~ -180��
                    //name: '�ҵ�λ��', // λ����
                    //address: '', // ��ַ����˵��
                    //scale: 10, // ��ͼ���ż���,����ֵ,��Χ��1~28��Ĭ��Ϊ���
                    //infoUrl: '' // �ڲ鿴λ�ý���ײ���ʾ�ĳ�����,�ɵ����ת
                    //});
                }
            });
        });
        wx.error(function (res) {
            layer.close(loadingIndex);
            alert('failed' + JSON.stringify(res));
            // config��Ϣ��֤ʧ�ܻ�ִ��error��������ǩ�����ڵ�����֤ʧ�ܣ����������Ϣ���Դ�config��debugģʽ�鿴��Ҳ�����ڷ��ص�res�����в鿴������SPA�������������ǩ����
        });
    }

    window.onload = function () {


        wxApi();
    };

</script>
