<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTDWProMatchPurchase.aspx.cs" Inherits="TTDWProMatchPurchase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品原料成本表-采购部</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $(function () { 
            playEditor();
        });

        function playEditor() {

            var $allTitle = $(".playtitle");

            //点击编辑按钮-原标题及编辑按钮隐藏，input及保存按钮显示
            $(".playeditor").click(function () {
                $(this).parent().find(".playtx").val($.trim($(this).parent().find(".playtitle").html()));
                $(this).parent().find(".playtitle,.playeditor").hide();
                $(this).parent().find(".playtx,.playsave").show();
                $(".playtx").focus();
            });
            //点击保存按钮-input及保存按钮隐藏，原标题及编辑按钮显示
            $(".playsave").click(function () {
                var p = $(this).parent().find(".playtitle");
                var varPrice = $.trim($(this).parent().find(".playtx").val());
                var varProMath = $(this).attr("value");
                if (varPrice == "" || varPrice == null || varPrice == undefined) {
                    alert("产品配方价不能为空！");
                    return false;
                }
                var reg = /(^[-+]?[1-9]\d*(\.\d{1,2})?$)|(^[-+]?[0]{1}(\.\d{1,2})?$)/;
                if (!reg.test(varPrice) || varPrice < 0) {
                    alert("请输入正确的默认出价(正数，最多两位小数)！");
                    return false;
                }

                $.post("Handler/OperateProMatchHandler.ashx",
                 {
                     "proMatchID": varProMath,
                     "productPrice": varPrice
                 }, function (data) {
                     ;
                     if (data == "succ") {
                         //alert("修改成功！");
                         //p.html(varPrice);
                         document.location.reload();//重新加载页面
                         return true;
                     } else {
                         alert(data);
                         return true;
                     }


                 });
                $(this).parent().find(".playtitle,.playeditor").show();
                $(this).parent().find(".playtx,.playsave").hide();

            });

            //双击原标题-原标题及编辑按钮隐藏，input及保存按钮显示
            $(".playtitle").dblclick(function () {
                $(this).parent().find(".playtitle,.playeditor").hide();
                $(this).parent().find(".playtx,.playsave").show();
                //$(this).parent().find(".playtx").html($.trim($(this).html()));
                $(this).parent().find(".playtx").val($.trim($(this).html()));
                $(".playtx").focus();
            });
            //点击空白处-input及保存按钮隐藏，原标题级编辑按钮显示
            $(".playtx").blur(function () {
                var tempobj = $(this);
                setTimeout(function () {
                    $(tempobj).parent().find(".playtitle,.playeditor").show();
                    $(tempobj).parent().find(".playtx,.playsave").hide();
                }, 200);
            });
        }


        var pre_scrollTop = 0;//滚动条事件之前文档滚动高度 
        var pre_scrollLeft = 0;//滚动条事件之前文档滚动宽度 
        var obj_head;
        var obj_left;



        window.onload = function () {
            pre_scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
            pre_scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;
            obj_head = document.getElementById("tableHead");
            obj_left = document.getElementById("tableLeft");
            var widthValue = parseInt(document.getElementById("HF_ProductCount").value) * 180 + parseInt(document.getElementById("HF_ProductCount").value) * 4.2;
            //alert(widthValue);
            $("#tableHead").attr("width", widthValue);
        };
        window.onscroll = function () {
            if (pre_scrollTop != (document.documentElement.scrollTop || document.body.scrollTop)) {
                //滚动了数值滚动条 
                pre_scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
                if (obj_head) {
                    var topHeight = document.documentElement.scrollTop || document.body.scrollTop;
                    if (topHeight < 103) {
                        obj_head.style.top = 103 + topHeight + "px";
                    } else {
                        obj_head.style.top = topHeight + "px";
                    }

                }
            }
            else if (pre_scrollLeft != (document.documentElement.scrollLeft || document.body.scrollLeft)) {
                //滚动了水平滚动条 
                pre_scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;
                if (obj_left) {
                    var topLeft = document.documentElement.scrollLeft || document.body.scrollLeft;
                    if (topLeft < 8) {
                        obj_left.style.left = 8 + topLeft + "px";
                    } else {
                        obj_left.style.left = topLeft + "px";
                    }
                }
            }
        };
    </script>
    <style type="text/css">
        #tableHead td{
            width: 180px;
            white-space:normal;
            overflow:auto;
            background-color:yellow;
            font:bold;
        }

        #tableLeft td{
            width: 180px;
            white-space:normal;
            overflow:auto;
            background-color:azure;
            font:bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <div id="AboveDiv">
                <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                    <tr>
                        <td height="31" class="page_topbj">
                            <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="29">
                                                    <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                </td>
                                                <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChanPinYuanLiaoChengBenBiao%>"></asp:Label>
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
                                            <tr style="font-size: 12pt">
                                                <td class="formItemBgStyleForAlignLeft" width="100%">
                                                    <div style="width: 100%; height: 100%;">
                                                        <table class="formBgStyle">
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,YuanLaio%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TXT_MatchName" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChanPinLeiXing%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:DropDownList ID="DDL_Type" runat="server"
                                                                        DataTextField="ProductType" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ChanPinBianHao%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TXT_ProductCode" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ChanPinPaiHao%>"></asp:Label>：</td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <asp:TextBox ID="TXT_ProductName" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft" colspan="8">
                                                                    <asp:Button ID="Button1" runat="server" Text="<%$ Resources:lang,ChaXun%>" CssClass="inpu" OnClick="Button1_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table class="formBgStyle" style="width: 9999px;">
                                                            <tr>
                                                                <td rowspan="2" style="width: 360px;" class="formItemBgStyleForAlignLeft">
                                                                    <table class="formBgStyle" id="tableLeft" style="position: absolute; left: 8px; top: 100px; width: 180px;">
                                                                        <tr>
                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                <table class="formBgStyle" style="width: 360px;">
                                                                                    <tr>
                                                                                        <td width="180" class="formItemBgStyleForAlignLeft">
                                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YuanLiaoDaiHao%>"></asp:Label></td>
                                                                                        <td width="180" class="formItemBgStyleForAlignLeft">
                                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YuanLiaoJiaGe%>"></asp:Label></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table class="formBgStyle" style="width: 100%;">
                                                                                    <asp:Literal ID="LT_Left" runat="server"></asp:Literal>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <table id="tableHead" class="formBgStyle" style="position: absolute; height: 30px; left: 377px; top: 103px;">
                                                                        <tr>
                                                                            <asp:Literal ID="LT_Header" runat="server"></asp:Literal>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formItemBgStyleForAlignLeft">
                                                                    <table class="formBgStyle" style="margin-top: 25px;">
                                                                        <asp:Literal ID="LT_Content" runat="server"></asp:Literal>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
        <asp:HiddenField ID="HF_ProductCount" runat="server" Value="0" />
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
