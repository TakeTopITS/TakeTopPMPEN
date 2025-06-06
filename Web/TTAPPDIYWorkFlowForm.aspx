<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPDIYWorkFlowForm.aspx.cs" Inherits="TTAPPDIYWorkFlowForm" %>


<%@ Register Assembly="TakeTopForm.JS" Namespace="TakeTopInfoPathSoft.JS" TagPrefix="cc1" %>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>自定义表单工作流</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/calendar.js"></script>
    <script type="text/javascript" src="js/calendar-en.js"></script>
    <script type="text/javascript" src="js/calendar-setup.js"></script>
    <%--    <script type="text/javascript" src="js/takeTopInfoPathSoft.js"></script>
    <script type="text/javascript" src="js/xmlOperation.js"></script>
    --%>

    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/dialog/lhgdialog.min.js"></script>

    <style type="text/css">
        .DemoTable {
            font-size: 10pt;
            font-family: 'Verdana';
        }

        /*  modal popup */
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: #EFF3FB;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }


        /*-----------------------------短按钮----------------------------*/
        .inpu {
            border-style: none;
            background-image: url('ImagesSkin/butbj.jpg');
            width: 71px;
            text-align: center;
            font-size: 12px;
            font-family: "microsoft yahei";
            margin: -2px 0;
            cursor: pointer;
            height: 25px;
            border-radius: 2px;
            color: #ffffff;
            background: #2e8ded; /*皮肤*/
        }

            .inpu:disabled {
                background: url('ImagesSkin/butbj.jpg');
                color: #cccccc;
                cursor: default;
            }

            .inpu[disabled] {
                background: url('ImagesSkin/butbj.jpg');
                color: #cccccc;
                cursor: default;
                height: 25px;
            }

            .inpu:disabled:hover {
                background: url('ImagesSkin/butbj.jpg');
                color: #cccccc;
            }

            .inpu[disabled]:hover {
                background: url('ImagesSkin/butbj.jpg');
                color: #cccccc;
            }

        .ToolBar {
            position: fixed !important;
            bottom: 0px;
            left: 0px;
            margin: 0 auto;
            position: absolute;
            top: expression(offsetParent.scrollTop+0);
            border: 1px solid #dddddd;
            border-bottom: 1px solid #dddddd;
            background-color: ButtonFace;
            width: 100%;
            height: 100px;
        }
    </style>

    <script type="text/javascript">

        var bdhtml;
        function preview() {

            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint-->";
            eprnstr = "<!--endprint-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
        }

        function setValue(m_strValue) {
            if (m_strValue != null && m_strValue != undefined) {
                txtReturnValue.value = m_strValue;
            }
        }
    </script>

    <script type="text/javascript">

        var isIE = (document.all) ? true : false;

        var $ = function (id) {
            return "string" == typeof id ? document.getElementById(id) : id;
        };

        var Class = {
            create: function () {
                return function () { this.initialize.apply(this, arguments); }
            }
        }

        var Extend = function (destination, source) {
            for (var property in source) {
                destination[property] = source[property];
            }
        }

        var Bind = function (object, fun) {
            return function () {
                return fun.apply(object, arguments);
            }
        }

        var BindAsEventListener = function (object, fun) {
            return function (event) {
                return fun.call(object, (event || window.event));
            }
        }

        function addEventHandler(oTarget, sEventType, fnHandler) {
            if (oTarget.addEventListener) {
                oTarget.addEventListener(sEventType, fnHandler, false);
            } else if (oTarget.attachEvent) {
                oTarget.attachEvent("on" + sEventType, fnHandler);
            } else {
                oTarget["on" + sEventType] = fnHandler;
            }
        };

        function removeEventHandler(oTarget, sEventType, fnHandler) {
            if (oTarget.removeEventListener) {
                oTarget.removeEventListener(sEventType, fnHandler, false);
            } else if (oTarget.detachEvent) {
                oTarget.detachEvent("on" + sEventType, fnHandler);
            } else {
                oTarget["on" + sEventType] = null;
            }
        };

        //拖放程序
        var SimpleDrag = Class.create();
        SimpleDrag.prototype = {
            //拖放对象,触发对象
            initialize: function (drag) {
                this.Drag = $(drag);
                this._x = this._y = 0;
                this._fM = BindAsEventListener(this, this.Move);
                this._fS = Bind(this, this.Stop);
                this.Drag.style.position = "absolute";
                addEventHandler(this.Drag, "mousedown", BindAsEventListener(this, this.Start));
            },
            //准备拖动
            Start: function (oEvent) {
                this._x = oEvent.clientX - this.Drag.offsetLeft;
                this._y = oEvent.clientY - this.Drag.offsetTop;
                addEventHandler(document, "mousemove", this._fM);
                addEventHandler(document, "mouseup", this._fS);
            },
            //拖动
            Move: function (oEvent) {
                this.Drag.style.left = oEvent.clientX - this._x + "px";
                this.Drag.style.top = oEvent.clientY - this._y + "px";
            },
            //停止拖动
            Stop: function () {
                removeEventHandler(document, "mousemove", this._fM);
                removeEventHandler(document, "mouseup", this._fS);
            }
        };

        jQuery.noConflict();
        jQuery(function () {

            jQuery("table:eq(0)").width(jQuery(document).width());

            jQuery("#btnNotWorkFlow").click(function () {
                jQuery("#tbName tr:gt(1)").remove();
                var strTemName = jQuery("#LLB_WorkFlowTemplate").val();
                if (strTemName != null && strTemName != "") {
                    jQuery.ajax({
                        type: "POST",
                        dataType: "json",
                        url: "Handler/GetNotToFormListHandler.ashx",
                        data: "strTemName=" + escape(strTemName),
                        success: function (json) {

                            var tbody = "";
                            jQuery.each(json, function (i, n) {
                                var trs = "";
                                trs += "<tr><td><label onclick='ClickCreateXMLToForm(" + n.TriggerID + ");' style=\"cursor:pointer\">" + n.Name + "</label></td></tr>";
                                tbody += trs;
                            });

                            if (tbody != null && tbody != "") {
                                jQuery("#idDrag").show();
                                jQuery("#tbName").append(tbody);
                                jQuery("#tbName tr:gt(1):odd").attr("class", "trodd");
                                jQuery("#tbName tr:gt(1):even").attr("class", "treven");
                            } else {
                                jQuery("#idDrag").hide();
                                alert("暂没有未处理的记录！");
                            }
                        },
                        error: function () {
                            alert("加载失败");
                        }
                    });
                } else {
                    alert("请先选择工作流模板！");
                }
            });
        });

        function ClickClosed() {
            jQuery("#idDrag").hide();
        }

        //产生XML，附加到表单
        function ClickCreateXMLToForm(objID) {
            document.getElementById("HF_TriggerID").value = objID;
            document.getElementById("BT_AppendXML").click();
        }

        //设置日期选择框的宽度，使之不折叠
        function changeDateBoxWidth() {
            var cNodes = document.getElementsByClassName("xdDTText xdBehavior_FormattingNoBUI");

            for (var i = 0; i < cNodes.length; i++) {

                cNodes[i].style.width = "110px";
            }
        }

        //逐步放大元素大小
        function toIncreaseSize(objID, largestSize) {

            document.getElementById(objID).style.width = largestSize + "px";
        }

        //一步还原元素大小
        function toRestoreOriginalSize(objID, oldSize) {

            document.getElementById(objID).style.width = oldSize + "px";

        }


    </script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

             /*  if (top.location != self.location) { } else { CloseWebPage(); }*/

        });

        //隐藏或显示工作流标题栏
        function hideOrDisplayWFMain() {

            if (document.getElementById('td_WFMain').style.display == 'none') {
                document.getElementById('td_WFMain').style.display = 'block';
            }
            else {
                document.getElementById('td_WFMain').style.display = 'none';
            }
        }



        //隐藏工作流标题栏
        function hideWFMain() {

            /*  document.getElementById('td_WFMain').style.display = 'none';*/
        }



    </script>

    <style type="text/css">
        .trodd {
            background: #fafafa;
        }

        .treven {
            background: #f4f4f4;
        }
    </style>
</head>
<%--<body onload="TakeTopInfoPath_Init();" oncontextmenu="return false">--%>
<body onload="TakeTopInfoPath_Init();changeDateBoxWidth();" onmousemove="changeDateBoxWidth();" onclick="aHandlerForWorkflowDIYFormDataPopWindow(arguments[0])">
    <script type="text/javascript">
        function autoClickActiveButton() {

            var aHref = document.getElementById("HL_WFChartView").href;
            if (aHref == "") {

                alert("请先选择工作流模板或工作流(Please select workflow template or workflow)!")
                return;
            }

            var autoActive = '<%= Session["AutoActiveWorkflow"].ToString() %>'.trim();
            if (autoActive == 'YES') {

                if (document.getElementById("BT_ActiveYes").disabled != "true")

                    document.getElementById("BT_ActiveYes").click();
            }
            else {

                document.getElementById("BT_ActiveYes").click();
                /* document.getElementById('bgLayer').style.display = 'block';*/

            }
        }
    </script>


    <center>
        <object id="factory" viewastext style="display: none" classid="clsid:1663ed61-23eb-11d2-b92f-008048fdd814"
            codebase="smsx.cab#Version=6,3,434,26">
        </object>
        <form id="form1" runat="server">
            <div style="z-index: -2;">
                <cc1:JSInfoPathRefer ID="JSInfoPathRefer1" runat="server" />



                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table class="page_topbj" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="text-align: right; padding-left: 20px; width: 50px;">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MuBan%>"></asp:Label>：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DL_WorkFlowTemplate" Width="99%" DataTextField="TemName" DataValueField="TemName" runat="server" OnSelectedIndexChanged="DL_WorkFlowTemplate_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; padding-left: 20px;">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,GongZuo%>"></asp:Label>：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DL_WorkFlow" Width="99%" DataTextField="WLName" DataValueField="WLID" runat="server" OnSelectedIndexChanged="DL_WorkFlow_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="LB_WFType" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" style="padding: 5px 5px 30px 5px;" onmouseover="hideWFMain()">
                                        <table width="100%" class="DemoTable">
                                            <tr>
                                                <td>
                                                    <!--startprint-->
                                                    <asp:Label ID="xdoc" runat="server" EnableViewState="False" Text="<%$ Resources:lang,YSHDBDZZLXS%>"></asp:Label>
                                                    <!--endprint-->
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <div id="divToolBar" name="divTooBar" class="ToolBar" style="width: 100%; text-align: left; padding-top: 10px; padding-bottom: 10px; background-color: aliceblue;">

                    <table width="100%">
                        <tr>
                            <td class="tdLeft" align="right" width="50px">
                                <span style="font-size: 10pt;">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：</span>
                            </td>
                            <td class="tdRight" align="left">
                                <asp:TextBox ID="TB_WorkFlowName" runat="server" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLeft" align="right">
                                <span style="font-size: 10pt;">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,MiaoShu%>"></asp:Label>： </span>
                            </td>
                            <td class="tdRight" align="left">
                                <asp:TextBox ID="TB_WLDescription" runat="server" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                             <td colspan="2" class="tdRight" align="left" style="padding-top: 4px; padding-bottom: 4px;">
                                <asp:Label ID="LB_SubmitCheck" runat="server" Text="<%$ Resources:lang,TJZGZLQCL%>" CssClass="submitButton" onclick="autoClickActiveButton();"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Label ID="LB_XSNFile" runat="server" Visible="false"></asp:Label>
                <asp:Literal ID="LIT_AttachUserJSCode" runat="server"></asp:Literal>
                <asp:Literal ID="LIT_AttachUserWFStepJSCode" runat="server"></asp:Literal>
                <input id="txtReturnValue" type="text" style="display: none;" />
                <asp:Label ID="LB_OldWFXMLFile" runat="server"></asp:Label>
                <asp:Label ID="LB_PublishUrl" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LB_Uri" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LB_ToBeHandledBusinessForm" runat="server" Text="<%$ Resources:lang,DaiChuLiYeWu%>" Visible="false"></asp:Label>
                <span style="cursor: pointer; visibility: hidden;">
                    <img src="Images/NotWorkflow.png" id="btnNotWorkFlow" />
                </span>
            </div>
            <div id="bgLayer" style="z-index: 9998; position: absolute; filter: alpha(style=0,opacity=100); width: 520px; display: none; background: #cccccc; height: 100px; left: 30%; top: 40%">
                <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Width="520px" Height="100px">
                    <table>
                        <tr>
                            <td style="width: 520px; padding: 5px 5px 5px 20px;" valign="top" align="left">
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 520px; padding: 5px 25px 5px 5px;" valign="top" align="left">
                                <asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                    ID="BT_ActiveNo" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Fou%>" OnClick="BT_ActiveNo_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="idDrag" style="background: #C4E3FD; width: 150px; display: none; position: absolute; top: 100px; right: 50px;">
                <table id="tbName" width="100%">
                    <tr>
                        <td align="right">
                            <input type="button" value="X" onclick="ClickClosed();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,WCLCXMC%>"></asp:Label></td>
                    </tr>
                </table>
                <asp:HiddenField ID="HF_TriggerID" runat="server"></asp:HiddenField>
                <asp:Button ID="BT_AppendXML" runat="server" Text="" Style="display: none" OnClick="BT_AppendXML_Click"></asp:Button>
                <script>
                    new SimpleDrag("idDrag");
                </script>
            </div>

            <table style="display: none;">
                <tr>
                    <td id="td_WFMain" style="padding: 5px 5px 5px 5px; width: 370px; vertical-align: top;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="40%" align="left" style="font-size: smaller;">
                                                            <a onclick="toIncreaseSize('LLB_WorkFlowTemplate',300)" style="font-size: large;">++</a> <strong>
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongZuoLiuMuBan%>"></asp:Label></strong>
                                                            <a onclick="toRestoreOriginalSize('LLB_WorkFlowTemplate',140)" style="font-size: large;">--</a>
                                                        </td>
                                                        <td align="left" style="font-size: smaller;">
                                                            <a onclick="toIncreaseSize('LLB_WokFlow',400)" style="font-size: large;">++</a> <strong>
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>
                                                            <a onclick="toRestoreOriginalSize('LLB_WokFlow',220)" style="font-size: large;">--</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLeft" align="left" width="40%" style="padding: 1px 1px 1px 1px;">&nbsp;</td>
                                <td class="tdRight" align="left" width="60%" style="padding: 1px 1px 1px 1px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLeft" colspan="2" style="padding: 1px 1px 1px 1px; border-bottom: solid 1px #D8D8D8; border-right: solid 1px #D8D8D8;">
                                    <table style="width: 370px;" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td class="tdLeft" align="right" width="50px"></td>
                                            <td class="tdLeft" width="110">
                                                <asp:Label ID="LB_WLID" runat="server" Font-Size="10pt"></asp:Label>
                                            </td>
                                            <td class="tdRight" align="left" width="210">
                                                <asp:HyperLink ID="HL_WFChartView" runat="server" Text="<%$ Resources:lang,LiuChengTu %>" Target="top.frames[0].frames[2].parent.frames['rightTabFrame']">  </asp:HyperLink>
                                            </td>

                                            <td class="tdLeft" align="right" width="50"></td>
                                            <td colspan="2" class="tdRight" align="left">
                                                <span style="font-size: 10pt;">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenPiJinDuFanKui%>"></asp:Label>：</span><asp:CheckBox
                                                        ID="CB_SMS" runat="server" Font-Size="10pt" Text="<%$ Resources:lang,DuanXin%>" Checked="true" /><asp:CheckBox ID="CB_Mail"
                                                            runat="server" Font-Size="10pt" Text="<%$ Resources:lang,YouJian%>" Checked="true" />
                                            </td>
                                        </tr>

                                        <tr style="display: none;">
                                            <td class="tdLeft" align="left" colspan="3">


                                                <asp:Button ID="BT_Download" runat="server" OnClick="BT_Download_Click" Text="<%$ Resources:lang,XiaZai%>" Enabled="False" />

                                                <asp:Button ID="BT_SaveXMLFile" runat="server" Enabled="False" CssClass="inpu" OnClick="BT_SaveXMLFile_Click" Text="<%$ Resources:lang,BaoCunGongZuoLiuGengGai%>" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="tdLeft" align="right">XML:
                                            </td>
                                            <td colspan="2" class="tdRight" align="left">
                                                <input id="XMLFile" runat="server" name="XMLFile" type="file" style="width: 220px;" />
                                                <asp:Button ID="BT_Upload" runat="server" OnClick="BT_Upload_Click" Text="<%$ Resources:lang,ShangZai%>" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td colspan="3" class="tdRight" style="text-align: center; padding: 2px 0px 2px 5px; border-bottom: solid 1px  #D8D8D8; width: 370px;">
                                                <asp:HyperLink ID="HL_WFXSNFile" runat="server"
                                                    Target="_blank" Font-Names="宋体" Font-Size="10pt">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,XGGZLBGMBXZ%>"></asp:Label>
                                                </asp:HyperLink>
                                                &nbsp;<input id="btPrintPreview" type="button" class="inpuPrint" name="print" onclick="preview()" />
                                                <asp:Button ID="BT_BackupSaveXMLFile" runat="server" OnClick="BT_BackupSaveXMLFile_Click"
                                                    Style="display: none;" />

                                                <asp:HyperLink ID="HL_OldWFXMLFile" runat="server"
                                                    Target="_blank" Font-Names="宋体" Font-Size="10pt" Text="<%$ Resources:lang,ShuJuWenJian%>"></asp:HyperLink>
                                                <asp:HyperLink ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                                    Target="_blank" Font-Names="宋体" Font-Size="10pt">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="HL_RedirectToMyWFDetail" runat="server" Target="_blank" Text="<%$ Resources:lang,ZhiXingJinDu%>" Visible="false"></asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="tdRight" align="left">
                                                <asp:HyperLink ID="HL_RelatedDoc" runat="server" Enabled="False" Font-Size="10pt" Text="<%$ Resources:lang,XiangGuanWenDang%>" Target="top.frames[0].frames[2].parent.frames['rightTabFrame']"></asp:HyperLink>

                                            </td>
                                            <td class="tdRight" align="left" style="padding-left: 20px;">
                                                <asp:Button ID="BT_ExportFormToExcel" runat="server" Text="<%$ Resources:lang,DaoChuDaoExcel%>" CssClass="inpu" Enabled="False" Style="height: 26px" OnClick="BT_ExportFormToExcel_Click" />
                                                <asp:HyperLink ID="HL_ExportFileURL" runat="server" Target="_blank"></asp:HyperLink>
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </td>
                    <td style="width: 20px; vertical-align: top;" onclick="hideOrDisplayWFMain()">
                        <img src="ImagesSkin/LeftRightArrow.jpg" />
                        <asp:DropDownList ID="DL_ToBeHandledBusinessForm" DataValueField="ID" DataTextField="Name" runat="server" Visible="false" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="DL_ToBeHandledBusinessForm_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>

                </tr>
            </table>

        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'diywfui.css';</script>
</html>
