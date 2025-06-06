<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGetUserPosition.aspx.cs" Inherits="TTGetUserPosition" %>


<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.5; minimum-scale=0.1; user-scalable=1" />
<meta http-equiv="Content-Type" content="textml; charset=UTF-8" />

<!DOCTYPE html>
<html>
<head>
    <title>TakeTop GPS</title>

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
            height: 980px;
            min-height: 980px;
        }
    </style>

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=wqBXfIN3HkpM1AHKWujjCdsi"></script>
    <script type="text/javascript" src="http://developer.baidu.com/map/jsdemo/demo/convertor.js"></script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            

        });

    </script>
</head>
<body style="margin: 1px 1px;">
    <form id="form1" runat="server">
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
                                        <td width="480" align="left">

                                            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="GPS" />
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

                                        <td align="right" style="padding-top: 1px; padding-bottom: 1px; padding-right: 8px;">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div id="status" style="text-align: center">
                                    <table>
                                        <tr>
                                            <td alight="right">
                                                <asp:Label ID="Label7781" runat="server" Text="<%$ Resources:lang,Jing%>"></asp:Label>：</td>
                                            <td>
                                                <input type="text" id="LNG_value" runat="server"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,Wei%>"></asp:Label>：</td>
                                            <td>
                                                <input type="text" id="LAT_value" runat="server"/>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div id="container" style="border: 1px solid gray; margin: 5px auto"></div>
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
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>


<script type="text/javascript">

    window.onload = function () {
        if (navigator.geolocation) {
            //document.getElementById("status").innerHTML = "HTML5 Geolocation is supported in your browser.";
            // 百度地图API功能
            //var map = new BMap.Map("container");
            //var point = new BMap.Point(113.373456, 23.14153);
            //map.centerAndZoom(point, 12);

            var geolocation = new BMap.Geolocation();
            geolocation.getCurrentPosition(function (r) {
                if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                    var mk = new BMap.Marker(r.point);
                    var map = new BMap.Map("container");
                    map.addOverlay(mk);
                    var point = new BMap.Point(r.point.lng, r.point.lat);
                    map.centerAndZoom(point, 18);

                    //map.panTo(r.point);
                    //map.centerAndZoom(r.point, 12)

                    document.getElementById("LNG_value").value = r.point.lng;
                    document.getElementById("LAT_value").value = r.point.lat;

                    //this.document.getElementById("BT_SavePosition").click();
                }
                else {
                    alert('failed' + this.getStatus());
                }
            }, { enableHighAccuracy: true })
        }
    };

</script>
