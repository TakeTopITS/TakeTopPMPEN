<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMailLeftTree.aspx.cs" Inherits="LeftTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">$(function () {if (top.location != self.location) { } else { CloseWebPage(); }});</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="z-index: 101; left: 5px; width: 170px; position: absolute; top: 1px; height: 1px; border-width: 0px;">
                    <table style="border-collapse: collapse; width: 170px;" cellpadding="2">
                        <tr>
                            <td style="width: 170px; height: 22px; padding-top: 5px;" align="left">
                                <asp:Button ID="BT_SendRecive" runat="server" OnClick="BT_SendRecive_Click" Text="<%$ Resources:lang,FaSongHeJieShouYouJian%>"
                                    CssClass="inpuLong" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px; text-align: left">
                                <asp:TreeView ID="OperationView" Width="100%" runat="server" ImageSet="BulletedList3"
                                    ShowLines="True" BorderColor="Blue" AutoGenerateDataBindings="False">
                                    <ParentNodeStyle Font-Bold="False" />
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                        VerticalPadding="0px" />
                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                        NodeSpacing="0px" VerticalPadding="0px" />
                                    <Nodes>
                                        <asp:TreeNode Target="Main" Text="<%$ Resources:lang,GongNengLieBiao%>" Value="-1">
                                            <asp:TreeNode NavigateUrl="~/TTMailDesktop.aspx" Text="<%$ Resources:lang,YouJianWenJianJia%>" Value="0" Target="Desktop"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/TTMailSender.aspx" Text="<%$ Resources:lang,XinYouJian%>" Value="6" Target="Desktop"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/TTMailNewFolder.aspx" Target="Desktop" Text="<%$ Resources:lang,XinJianWenJianJia%>"
                                                Value="7"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="TTContactList.aspx?RelatedType=Other&RelatedID=0" Target="Desktop"  Text="<%$ Resources:lang,TongXunBu%>" Value="9"></asp:TreeNode>
                                             
                                            <asp:TreeNode NavigateUrl="~/TTMailSystemProfile.aspx" Target="Desktop" Text="<%$ Resources:lang,YouJianXiTongPeiZhi%>"  Value="8"></asp:TreeNode>
                                              
                                            <asp:TreeNode NavigateUrl="~/TTMailSignInfo.aspx" Target="Desktop" Text="<%$ Resources:lang,QianMingSheZhi%>" Value="10"></asp:TreeNode>
                                        </asp:TreeNode>
                                    </Nodes>
                                    <RootNodeStyle Font-Bold="True" ForeColor="Maroon" />
                                </asp:TreeView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 5; top: 10%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="Images/MailSendReceive.gif" alt="���Ժ򣬴�����..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
