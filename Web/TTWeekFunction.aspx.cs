using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;

public partial class TTWeekFunction : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
        strUserName = ShareClass.GetUserName(strUserCode);

         if (!IsPostBack)
        {
            LoadWeekTree();
        }
    }

    private void LoadWeekTree()
    {
        string strCarCheckWeekHQL = string.Empty;
        strCarCheckWeekHQL = "from CarCheckWeek as carCheckWeek where CustomerCode = '" + strUserCode + "' order by CreateTime desc";
        CarCheckWeekBLL carCheckWeekBLL = new CarCheckWeekBLL();
        IList listCarCheckWeek = carCheckWeekBLL.GetAllCarCheckWeeks(strCarCheckWeekHQL);
        TV_Weeks.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = LanguageHandle.GetWord("QuanBu");
        Node.Value = "all|all";
        if (listCarCheckWeek != null && listCarCheckWeek.Count > 0)
        {
            for (int i = 0; i < listCarCheckWeek.Count; i++)
            {
                CarCheckWeek carCheckWeek = (CarCheckWeek)listCarCheckWeek[i];

                TreeNode NodeChild = new TreeNode();
                NodeChild.Text = carCheckWeek.WeekName;
                string strWeekCode = carCheckWeek.WeekCode;
                NodeChild.Value = "carCheckWeekName|" + strWeekCode;
                //周检计划表
                TreeNode NodeWeekPlan = new TreeNode();
                NodeWeekPlan.Text = LanguageHandle.GetWord("ZhouJianJiHuaBiao");
                NodeWeekPlan.Value = "carCheckWeekPlan|" + strWeekCode;

                CustomModuleBLL customModuleBLL1 = new CustomModuleBLL();
                string strCustomModuleSql1 = "from CustomModule as customModule where CustomType = 'carCheckWeekPlan'";
                IList customModuleList1 = customModuleBLL1.GetAllCustomModules(strCustomModuleSql1);
                if (customModuleList1 != null && customModuleList1.Count > 0)
                {
                    if (customModuleList1.Count > 1)
                    {
                        for (int j = 0; j < customModuleList1.Count; j++)
                        {
                            CustomModule customModule = (CustomModule)customModuleList1[j];

                            TreeNode NodeProductChild = new TreeNode();
                            NodeProductChild.Text = customModule.TemName;
                            NodeProductChild.Value = "child|" + strWeekCode + "|" + customModule.IdentifyString;
                            NodeWeekPlan.ChildNodes.Add(NodeProductChild);
                        }

                    }
                }
                NodeChild.ChildNodes.Add(NodeWeekPlan);
                //周检问题表
                TreeNode NodeWeekProblem = new TreeNode();
                NodeWeekProblem.Text = LanguageHandle.GetWord("ZhouJianWenTiBiao");
                NodeWeekProblem.Value = "carCheckWeekProblem|" + strWeekCode;

                CustomModuleBLL customModuleBLL2 = new CustomModuleBLL();
                string strCustomModuleSql2 = "from CustomModule as customModule where CustomType = 'carCheckWeekProblem'";
                IList customModuleList2 = customModuleBLL2.GetAllCustomModules(strCustomModuleSql2);
                if (customModuleList2 != null && customModuleList2.Count > 0)
                {
                    if (customModuleList2.Count > 1)
                    {
                        for (int j = 0; j < customModuleList2.Count; j++)
                        {
                            CustomModule customModule = (CustomModule)customModuleList2[j];

                            TreeNode NodeProductChild = new TreeNode();
                            NodeProductChild.Text = customModule.TemName;
                            NodeProductChild.Value = "child|" + strWeekCode + "|" + customModule.IdentifyString;
                            NodeWeekProblem.ChildNodes.Add(NodeProductChild);
                        }

                    }
                }
                NodeChild.ChildNodes.Add(NodeWeekProblem);
                //会议讲评记录
                TreeNode NodeWeekRecord = new TreeNode();
                NodeWeekRecord.Text = LanguageHandle.GetWord("HuiYiJiangPingJiLu");
                NodeWeekRecord.Value = "carCheckWeekRecord|" + strWeekCode;

                CustomModuleBLL customModuleBLL3 = new CustomModuleBLL();
                string strCustomModuleSql3 = "from CustomModule as customModule where CustomType = 'carCheckWeekRecord'";
                IList customModuleList3 = customModuleBLL3.GetAllCustomModules(strCustomModuleSql3);
                if (customModuleList3 != null && customModuleList3.Count > 0)
                {
                    if (customModuleList3.Count > 1)
                    {
                        for (int j = 0; j < customModuleList3.Count; j++)
                        {
                            CustomModule customModule = (CustomModule)customModuleList3[j];

                            TreeNode NodeProductChild = new TreeNode();
                            NodeProductChild.Text = customModule.TemName;
                            NodeProductChild.Value = "child|" + strWeekCode + "|" + customModule.IdentifyString;
                            NodeWeekRecord.ChildNodes.Add(NodeProductChild);
                        }

                    }
                }
                NodeChild.ChildNodes.Add(NodeWeekRecord);
                //问题整改通知单
                TreeNode NodeWeekNotice = new TreeNode();
                NodeWeekNotice.Text = LanguageHandle.GetWord("WenTiZhengGaiTongZhiChan");
                NodeWeekNotice.Value = "carCheckWeekNotice|" + strWeekCode;

                CustomModuleBLL customModuleBLL4 = new CustomModuleBLL();
                string strCustomModuleSql4 = "from CustomModule as customModule where CustomType = 'carCheckWeekRecord'";
                IList customModuleList4 = customModuleBLL4.GetAllCustomModules(strCustomModuleSql4);
                if (customModuleList4 != null && customModuleList4.Count > 0)
                {
                    if (customModuleList4.Count > 1)
                    {
                        for (int j = 0; j < customModuleList4.Count; j++)
                        {
                            CustomModule customModule = (CustomModule)customModuleList4[j];

                            TreeNode NodeProductChild = new TreeNode();
                            NodeProductChild.Text = customModule.TemName;
                            NodeProductChild.Value = "child|" + strWeekCode + "|" + customModule.IdentifyString;
                            NodeWeekNotice.ChildNodes.Add(NodeProductChild);
                        }

                    }
                }
                NodeChild.ChildNodes.Add(NodeWeekNotice);
                //问题汇总
                TreeNode NodeWeekTotal = new TreeNode();
                NodeWeekTotal.Text = LanguageHandle.GetWord("WenTiHuiZong");
                NodeWeekTotal.Value = "carCheckWeekTotal|" + strWeekCode;

                CustomModuleBLL customModuleBLL5 = new CustomModuleBLL();
                string strCustomModuleSql5 = "from CustomModule as customModule where CustomType = 'carCheckWeekTotal'";
                IList customModuleList5 = customModuleBLL5.GetAllCustomModules(strCustomModuleSql5);
                if (customModuleList5 != null && customModuleList5.Count > 0)
                {
                    if (customModuleList5.Count > 1)
                    {
                        for (int j = 0; j < customModuleList5.Count; j++)
                        {
                            CustomModule customModule = (CustomModule)customModuleList5[j];

                            TreeNode NodeProductChild = new TreeNode();
                            NodeProductChild.Text = customModule.TemName;
                            NodeProductChild.Value = "child|" + strWeekCode + "|" + customModule.IdentifyString;
                            NodeWeekTotal.ChildNodes.Add(NodeProductChild);
                        }

                    }
                }
                NodeChild.ChildNodes.Add(NodeWeekTotal);
                Node.ChildNodes.Add(NodeChild);
            }
        }
        Node.ExpandAll();
        TV_Weeks.Nodes.Add(Node);
    }



    private void ShowCarCheckWeekInfo(string strWeekCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from CarCheckWeek as carCheckWeek where carCheckWeek.WeekCode = '" + strWeekCode + "'";
        CarCheckWeekBLL carCheckWeekBLL = new CarCheckWeekBLL();
        lst = carCheckWeekBLL.GetAllCarCheckWeeks(strHQL);
        if (lst != null && lst.Count > 0)
        {
            CarCheckWeek carCheckWeek = (CarCheckWeek)lst[0];

            TXT_WeekCode.Text = carCheckWeek.WeekCode.Trim();
            TXT_WeekName.Text = carCheckWeek.WeekName.Trim();
            TXT_Ext1.Text = carCheckWeek.Ext1.Trim();
            TXT_Ext2.Text = carCheckWeek.Ext2.Trim();
            TXT_Ext3.Text = carCheckWeek.Ext3.Trim();
            TXT_Ext4.Text = carCheckWeek.Ext4.Trim();
            TXT_Ext5.Text = carCheckWeek.Ext5.Trim();
            TXT_Remark.Text = carCheckWeek.Remark.Trim();

            TXT_WeekCode.ReadOnly = true;
        }
        else
        {
            TXT_WeekCode.Text = "";
            TXT_WeekName.Text = "";
            TXT_Ext1.Text = "";
            TXT_Ext2.Text = "";
            TXT_Ext3.Text = "";
            TXT_Ext4.Text = "";
            TXT_Ext5.Text = "";
            TXT_Remark.Text = "";

            TXT_WeekCode.ReadOnly = false;
        }
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strWeekCode, strWeekName, strExt1, strExt2, strExt3, strExt4, strExt5, strRemark;

        strWeekCode = TXT_WeekCode.Text.Trim();
        strWeekName = TXT_WeekName.Text.Trim();
        strExt1 = TXT_Ext1.Text.Trim();
        strExt2 = TXT_Ext2.Text.Trim();
        strExt3 = TXT_Ext3.Text.Trim();
        strExt4 = TXT_Ext4.Text.Trim();
        strExt5 = TXT_Ext5.Text.Trim();
        strRemark = TXT_Remark.Text.Trim();

        if (strWeekCode != "" | strWeekName != "")
        {
            CarCheckWeekBLL carCheckWeekBLL = new CarCheckWeekBLL();
            CarCheckWeek carCheckWeek = new CarCheckWeek();

            carCheckWeek.WeekCode = strWeekCode;
            carCheckWeek.WeekName = strWeekName;
            carCheckWeek.Ext1 = strExt1;
            carCheckWeek.Ext2 = strExt2;
            carCheckWeek.Ext3 = strExt3;
            carCheckWeek.Ext4 = strExt4;
            carCheckWeek.Ext5 = strExt5;
            carCheckWeek.Remark = strRemark;
            carCheckWeek.CreateTime = DateTime.Now;
            carCheckWeek.CustomerCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim(); ;
            carCheckWeek.CustomerName = Session["UserName"] == null ? "" : Session["UserName"].ToString();

            try
            {
                carCheckWeekBLL.AddCarCheckWeek(carCheckWeek);


                LoadWeekTree();
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>ShowDiv();</script>");
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertXinZengChuCuoQingJi"));
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertKeHuDaiMaHeMingChen"));
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strWeekCode, strWeekName, strExt1, strExt2, strExt3, strExt4, strExt5, strRemark;

        strWeekCode = TXT_WeekCode.Text.Trim();
        strWeekName = TXT_WeekName.Text.Trim();
        strExt1 = TXT_Ext1.Text.Trim();
        strExt2 = TXT_Ext2.Text.Trim();
        strExt3 = TXT_Ext3.Text.Trim();
        strExt4 = TXT_Ext4.Text.Trim();
        strExt5 = TXT_Ext5.Text.Trim();
        strRemark = TXT_Remark.Text.Trim();

        if (strWeekCode != "" | strWeekName != "")
        {
            strHQL = "from CarCheckWeek as carCheckWeek where carCheckWeek.WeekCode=" + "'" + strWeekCode + "'";
            CarCheckWeekBLL carCheckWeekBLL = new CarCheckWeekBLL();
            lst = carCheckWeekBLL.GetAllCarCheckWeeks(strHQL);
            if (lst != null && lst.Count > 0)
            {
                CarCheckWeek carCheckWeek = (CarCheckWeek)lst[0];

                carCheckWeek.WeekCode = strWeekCode;
                carCheckWeek.WeekName = strWeekName;
                carCheckWeek.Ext1 = strExt1;
                carCheckWeek.Ext2 = strExt2;
                carCheckWeek.Ext3 = strExt3;
                carCheckWeek.Ext4 = strExt4;
                carCheckWeek.Ext5 = strExt5;
                carCheckWeek.Remark = strRemark;

                try
                {
                    carCheckWeekBLL.UpdateCarCheckWeek(carCheckWeek, strWeekCode);

                    LoadWeekTree();
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertBaoCunChengGongShow"));
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertXiuGaiChuCuoQingJia"));
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertBuNengXiuGaiKeHuDai"));
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertGongYingShangDaiMaH"));
        }
    }


    protected void TV_Weeks_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (TV_Weeks.SelectedNode != null)
        {
            string strSelectNodeValue = TV_Weeks.SelectedNode.Value;
            string[] arrSelectNodeValue = strSelectNodeValue.Split('|');
            if (arrSelectNodeValue[0] == "all")
            {
                mainFrame.Attributes.Add("src", "");
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = false;
                BT_Add.Enabled = true;
                ShowCarCheckWeekInfo("");
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekName")
            {
                mainFrame.Attributes.Add("src", "");
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekPlan")
            {
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("carCheckWeekPlan", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strWeekCode;
                        mainFrame.Attributes.Add("src", "");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekProblem")
            {
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("carCheckWeekProblem", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strWeekCode;
                        mainFrame.Attributes.Add("src", "");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekRecord")
            {
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("carCheckWeekRecord", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strWeekCode;
                        mainFrame.Attributes.Add("src", "");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekNotice")
            {
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("carCheckWeekNotice", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strWeekCode;
                        mainFrame.Attributes.Add("src", "");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "carCheckWeekTotal")
            {
                string strWeekCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCarCheckWeekInfo(strWeekCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("carCheckWeekTotal", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strWeekCode;
                        mainFrame.Attributes.Add("src", "");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "child")
            {
                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strWeekCode = arrSelectNodeValue[1];
                strTemName = TV_Weeks.SelectedNode.Text;
                strIdentifyString = arrSelectNodeValue[2];

                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString))
                {
                    mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strWeekCode);
                }
            }
        }
    }


    private void ReadCustomerModule(string strCustomerType, out string strTemName, out string strIdentifyString, out string strDuo)
    {
        CustomModuleBLL customModuleBLL = new CustomModuleBLL();
        string strCustomModuleSql = "from CustomModule as customModule where CustomType = '" + strCustomerType + "'";
        IList customModuleList = customModuleBLL.GetAllCustomModules(strCustomModuleSql);
        if (customModuleList != null && customModuleList.Count > 0)
        {
            if (customModuleList.Count == 1)
            {
                CustomModule customModule = (CustomModule)customModuleList[0];
                strTemName = customModule.TemName;
                strIdentifyString = customModule.IdentifyString;
                strDuo = "1";
            }
            else
            {
                strTemName = "2";
                strIdentifyString = "2";
                strDuo = "2";
            }
        }
        else
        {
            strTemName = "";
            strIdentifyString = "";
            strDuo = "";
        }
    }
}