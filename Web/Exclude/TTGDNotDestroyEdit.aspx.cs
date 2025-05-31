using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDNotDestroyEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataGDProjectBinder();


            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindData(intID);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_GDProject.SelectedValue;
            string strIsom_no = DDL_Isom_no.SelectedValue;
            string strWeldCode = TXT_WeldCode.Text.Trim();
            string strWeldStatus = DDL_WeldStatus.SelectedValue;
            string strSize = TXT_Size.Text.Trim();
            string strMold = TXT_Mold.Text.Trim();
            string strSF = DDL_SF.SelectedValue;
            string strMedium = TXT_Medium.Text.Trim();
            string strRanderWelder1 = TXT_RanderWelder1.Text.Trim();
            string strRanderWelderName1 = HF_RanderWelder1.Value;
            string strRanderWelder2 = TXT_RanderWelder2.Text.Trim();
            string strRanderWelderName2 = HF_RanderWelder2.Value;
            string strRanderTime = TXT_RanderTime.Text.Trim();
            string strCoveringWelder1 = TXT_CoveringWelder1.Text.Trim();
            string strCoveringWelderName1 = HF_CoveringWelder1.Value;
            string strCoveringWelder2 = TXT_CoveringWelder2.Text.Trim();
            string strCoveringWelderName2 = HF_CoveringWelder2.Value;
            string strCoveringTime = TXT_CoveringTime.Text.Trim();
            string strReturnWelder = TXT_ReturnWelder.Text.Trim();
            string strReturnWelderName = HF_ReturnWelder.Value;
            string strReturnTime = TXT_ReturnTime.Text.Trim();
            string strPressurePackNo = TXT_PressurePackNo.Text.Trim();
            string strFRI1 = TXT_FRI1.Text.Trim();
            string strFRI2 = TXT_FRI2.Text.Trim();
            string strFRI3 = TXT_FRI3.Text.Trim();
            string strFRI4 = TXT_FRI4.Text.Trim();
            string strPackageTime = TXT_PackageTime.Text.Trim();
            string strPackage = TXT_Package.Text.Trim();
            string strOutsideTime = TXT_OutsideTime.Text.Trim();
            string strOutside = TXT_Outside.Text.Trim();
            string strRTTime = TXT_RTTime.Text.Trim();
            string strRT = TXT_RT.Text.Trim();
            string strPTTime = TXT_PTTime.Text.Trim();
            string strPT = TXT_PT.Text.Trim();
            string strPWHTTime = TXT_PWHTTime.Text.Trim();
            string strPWHT = TXT_PWHT.Text.Trim();
            string strPMITime = TXT_PMITime.Text.Trim();
            string strPMI = TXT_PMI.Text.Trim();
            string strMTTime = TXT_MTTime.Text.Trim();
            string strMT = TXT_MT.Text.Trim();
            string strOrificeTime = TXT_OrificeTime.Text.Trim();
            string strOrifice = TXT_Orifice.Text.Trim();
            string strAirPressTime = TXT_AirPressTime.Text.Trim();
            string strAirPress = TXT_AirPress.Text.Trim();
            string strTieInTime = TXT_TieInTime.Text.Trim();
            string strTieIn = TXT_TieIn.Text.Trim();
            string strRTDetail1 = TXT_RTDetail1.Text.Trim();
            string strRTDetail2 = TXT_RTDetail2.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMBNWKZ+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strIsom_no))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDXTHBNWKZ+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHKHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldStatus))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHKZTHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSize))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCCBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMold))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSF))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSFBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMedium))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJZBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRanderWelder1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDDHG1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRanderWelder2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDDHG2BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRanderTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDDRBNWFFZF+"')", true);
                return;
            }

            if (!ShareClass.CheckStringRight(strCoveringWelder1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGMHG1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCoveringWelder2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGMHG2BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCoveringTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGMRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strReturnWelder))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFGHGBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strReturnTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFGRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressurePackNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFRI1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFRI2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI2BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFRI3))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI3BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFRI4))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI4BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPackageTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPackage))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOutsideTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWGRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOutside))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWGBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRTRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRT))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPTTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPTRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPT))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPWHTTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPWHTRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPWHT))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPWHTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPMITime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPMIRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPMI))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPMIBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMTTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZMTRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMT))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZMTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOrificeTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZORIFICERBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOrifice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZORIFICEBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAirPressTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKSYRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAirPress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKSYBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strTieInTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTIEINRBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strTieIn))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTIEINBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTDetail1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRTXJ1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTDetail2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRTXJ2BNWFFZF+"')", true);
                return;
            }

            GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDNotDestroySql = "from GDNotDestroy as gDNotDestroy where id = " + intID;
                IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroySql);
                if (listGDNotDestroy != null && listGDNotDestroy.Count > 0)
                {
                    GDNotDestroy gDNotDestroy = (GDNotDestroy)listGDNotDestroy[0];

                    gDNotDestroy.ProjectCode = strProjectCode;
                    gDNotDestroy.Isom_no = strIsom_no;
                    gDNotDestroy.WeldCode = strWeldCode;
                    gDNotDestroy.WeldStatus = strWeldStatus;
                    gDNotDestroy.Size = strSize;
                    gDNotDestroy.Mold = strMold;
                    gDNotDestroy.SF = strSF;
                    gDNotDestroy.Medium = strMedium;
                    gDNotDestroy.RanderWelder1 = strRanderWelder1;
                    gDNotDestroy.RanderWelderName1 = strRanderWelderName1;
                    gDNotDestroy.RanderWelder2 = strRanderWelder2;
                    gDNotDestroy.RanderWelderName2 = strRanderWelderName2;
                    gDNotDestroy.RanderTime = strRanderTime;
                    gDNotDestroy.CoveringWelder1 = strCoveringWelder1;
                    gDNotDestroy.CoveringWelderName1 = strCoveringWelderName1;
                    gDNotDestroy.CoveringWelder2 = strCoveringWelder2;
                    gDNotDestroy.CoveringWelderName2 = strCoveringWelderName2;
                    gDNotDestroy.CoveringTime = strCoveringTime;
                    gDNotDestroy.ReturnWelder = strReturnWelder;
                    gDNotDestroy.ReturnWelderName = strReturnWelderName;
                    gDNotDestroy.ReturnTime = strReturnTime;
                    gDNotDestroy.PressurePackNo = strPressurePackNo;
                    gDNotDestroy.FRI1 = strFRI1;
                    gDNotDestroy.FRI2 = strFRI2;
                    gDNotDestroy.FRI3 = strFRI3;
                    gDNotDestroy.FRI4 = strFRI4;
                    gDNotDestroy.PackageTime = strPackageTime;
                    gDNotDestroy.Package = strPackage;
                    gDNotDestroy.OutsideTime = strOutsideTime;
                    gDNotDestroy.Outside = strOutside;
                    gDNotDestroy.RTTime = strRTTime;
                    gDNotDestroy.RT = strRT;
                    gDNotDestroy.PTTime = strPTTime;
                    gDNotDestroy.PT = strPT;
                    gDNotDestroy.PWHTTime = strPWHTTime;
                    gDNotDestroy.PWHT = strPWHT;
                    gDNotDestroy.PMITime = strPMITime;
                    gDNotDestroy.PMI = strPMI;
                    gDNotDestroy.MTTime = strMTTime;
                    gDNotDestroy.MT = strMT;
                    gDNotDestroy.OrificeTime = strOrificeTime;
                    gDNotDestroy.Orifice = strOrificeTime;
                    gDNotDestroy.AirPressTime = strAirPressTime;
                    gDNotDestroy.AirPress = strAirPress;
                    gDNotDestroy.TieInTime = strTieInTime;
                    gDNotDestroy.TieIn = strTieIn;
                    gDNotDestroy.RTDetail1 = strRTDetail1;
                    gDNotDestroy.RTDetail2 = strRTDetail2;

                    gDNotDestroyBLL.UpdateGDNotDestroy(gDNotDestroy, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDNotDestroy gDNotDestroy = new GDNotDestroy();
                gDNotDestroy.ProjectCode = strProjectCode;
                gDNotDestroy.Isom_no = strIsom_no;
                gDNotDestroy.WeldCode = strWeldCode;
                gDNotDestroy.WeldStatus = strWeldStatus;
                gDNotDestroy.Size = strSize;
                gDNotDestroy.Mold = strMold;
                gDNotDestroy.SF = strSF;
                gDNotDestroy.Medium = strMedium;
                gDNotDestroy.RanderWelder1 = strRanderWelder1;
                gDNotDestroy.RanderWelderName1 = strRanderWelderName1;
                gDNotDestroy.RanderWelder2 = strRanderWelder2;
                gDNotDestroy.RanderWelderName2 = strRanderWelderName2;
                gDNotDestroy.RanderTime = strRanderTime;
                gDNotDestroy.CoveringWelder1 = strCoveringWelder1;
                gDNotDestroy.CoveringWelderName1 = strCoveringWelderName1;
                gDNotDestroy.CoveringWelder2 = strCoveringWelder2;
                gDNotDestroy.CoveringWelderName2 = strCoveringWelderName2;
                gDNotDestroy.CoveringTime = strCoveringTime;
                gDNotDestroy.ReturnWelder = strReturnWelder;
                gDNotDestroy.ReturnWelderName = strReturnWelderName;
                gDNotDestroy.ReturnTime = strReturnTime;
                gDNotDestroy.PressurePackNo = strPressurePackNo;
                gDNotDestroy.FRI1 = strFRI1;
                gDNotDestroy.FRI2 = strFRI2;
                gDNotDestroy.FRI3 = strFRI3;
                gDNotDestroy.FRI4 = strFRI4;
                gDNotDestroy.PackageTime = strPackageTime;
                gDNotDestroy.Package = strPackage;
                gDNotDestroy.OutsideTime = strOutsideTime;
                gDNotDestroy.Outside = strOutside;
                gDNotDestroy.RTTime = strRTTime;
                gDNotDestroy.RT = strRT;
                gDNotDestroy.PTTime = strPTTime;
                gDNotDestroy.PT = strPT;
                gDNotDestroy.PWHTTime = strPWHTTime;
                gDNotDestroy.PWHT = strPWHT;
                gDNotDestroy.PMITime = strPMITime;
                gDNotDestroy.PMI = strPMI;
                gDNotDestroy.MTTime = strMTTime;
                gDNotDestroy.MT = strMT;
                gDNotDestroy.OrificeTime = strOrificeTime;
                gDNotDestroy.Orifice = strOrificeTime;
                gDNotDestroy.AirPressTime = strAirPressTime;
                gDNotDestroy.AirPress = strAirPress;
                gDNotDestroy.TieInTime = strTieInTime;
                gDNotDestroy.TieIn = strTieIn;
                gDNotDestroy.RTDetail1 = strRTDetail1;
                gDNotDestroy.RTDetail2 = strRTDetail2;

                gDNotDestroy.IsMark = 0;
                gDNotDestroy.UserCode = strUserCode;

                gDNotDestroyBLL.AddGDNotDestroy(gDNotDestroy);
            }

            Response.Redirect("TTGDNotDestroyList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();
        string strGDNotDestroySql = "from GDNotDestroy as gDNotDestroy where id = " + id;
        IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroySql);
        if (listGDNotDestroy != null && listGDNotDestroy.Count > 0)
        {
            GDNotDestroy gDNotDestroy = (GDNotDestroy)listGDNotDestroy[0];

            string strProjectCode = gDNotDestroy.ProjectCode;
            DDL_GDProject.SelectedValue = strProjectCode;

            GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
            string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strProjectCode + "'";
            IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

            DDL_Isom_no.DataSource = listGDLineWeld;
            DDL_Isom_no.DataTextField = "Isom_no";
            DDL_Isom_no.DataValueField = "Isom_no";
            DDL_Isom_no.DataBind();

            DDL_Isom_no.SelectedValue = gDNotDestroy.Isom_no;
            TXT_WeldCode.Text = gDNotDestroy.WeldCode;
            DDL_WeldStatus.SelectedValue = gDNotDestroy.WeldStatus;
            TXT_Size.Text = gDNotDestroy.Size;
            TXT_Mold.Text = gDNotDestroy.Mold;
            DDL_SF.SelectedValue = gDNotDestroy.SF;
            TXT_Medium.Text = gDNotDestroy.Medium;
            TXT_RanderWelder1.Text = gDNotDestroy.RanderWelder1;
            HF_RanderWelder1.Value = gDNotDestroy.RanderWelderName1;
            TXT_RanderWelder2.Text = gDNotDestroy.RanderWelder2;
            HF_RanderWelder2.Value = gDNotDestroy.RanderWelderName2;
            TXT_RanderTime.Text = gDNotDestroy.RanderTime;
            TXT_CoveringWelder1.Text = gDNotDestroy.CoveringWelder1;
            HF_CoveringWelder1.Value = gDNotDestroy.CoveringWelderName1;
            TXT_CoveringWelder2.Text = gDNotDestroy.CoveringWelder2;
            HF_CoveringWelder2.Value = gDNotDestroy.CoveringWelderName2;
            TXT_CoveringTime.Text = gDNotDestroy.CoveringTime;
            TXT_ReturnWelder.Text = gDNotDestroy.ReturnWelder;
            HF_ReturnWelder.Value = gDNotDestroy.ReturnWelderName;
            TXT_ReturnTime.Text = gDNotDestroy.ReturnTime;
            TXT_PressurePackNo.Text = gDNotDestroy.PressurePackNo;
            TXT_FRI1.Text = gDNotDestroy.FRI1;
            TXT_FRI2.Text = gDNotDestroy.FRI2;
            TXT_FRI3.Text = gDNotDestroy.FRI3;
            TXT_FRI4.Text = gDNotDestroy.FRI4;
            TXT_PackageTime.Text = gDNotDestroy.PackageTime;
            TXT_Package.Text = gDNotDestroy.Package;
            TXT_OutsideTime.Text = gDNotDestroy.OutsideTime;
            TXT_Outside.Text = gDNotDestroy.Outside;
            TXT_RTTime.Text = gDNotDestroy.RTTime;
            TXT_RT.Text = gDNotDestroy.RT;
            TXT_PTTime.Text = gDNotDestroy.PTTime;
            TXT_PT.Text = gDNotDestroy.PT;
            TXT_PWHTTime.Text = gDNotDestroy.PWHTTime;
            TXT_PWHT.Text = gDNotDestroy.PWHT;
            TXT_PMITime.Text = gDNotDestroy.PMITime;
            TXT_PMI.Text = gDNotDestroy.PMI;
            TXT_MTTime.Text = gDNotDestroy.MTTime;
            TXT_MT.Text = gDNotDestroy.MT;
            TXT_OrificeTime.Text = gDNotDestroy.OrificeTime;
            TXT_Orifice.Text = gDNotDestroy.Orifice;
            TXT_AirPressTime.Text = gDNotDestroy.AirPressTime;
            TXT_AirPress.Text = gDNotDestroy.AirPress;
            TXT_TieInTime.Text = gDNotDestroy.TieInTime;
            TXT_TieIn.Text = gDNotDestroy.TieIn;
            TXT_RTDetail1.Text = gDNotDestroy.RTDetail1;
            TXT_RTDetail2.Text = gDNotDestroy.RTDetail2;
        }
    }


    private void DataGDProjectBinder()
    {
        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = "from GDProject as gDProject";
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DDL_GDProject.DataSource = listGDProject;
        DDL_GDProject.DataTextField = "ProjectName";
        DDL_GDProject.DataValueField = "ProjectCode";
        DDL_GDProject.DataBind();

        DDL_GDProject.Items.Insert(0, new ListItem("", ""));
    }

    protected void DDL_GDProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSelectProject = DDL_GDProject.SelectedValue;

        GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
        string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strSelectProject + "'";
        IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

        DDL_Isom_no.DataSource = listGDLineWeld;
        DDL_Isom_no.DataTextField = "Isom_no";
        DDL_Isom_no.DataValueField = "Isom_no";
        DDL_Isom_no.DataBind();
    }
}