using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            DataGDProjectBinder();

            DataGDPressureBinder();
        }
    }

    private void DataBinder()
    {
        string strGDPressureHQL = @"select t.PressurePackNo,
COALESCE(a.BWPlan,0) as BWPlan,
0 as BWComp,
COALESCE(b.NotBWPlan,0) as NotBWPlan,
0 as NotBWComp,
COALESCE(c.RTPlan,0) as RTPlan,
0 as RTComp,
COALESCE(d.NotRTPlan,0) as NotRTPlan,
0 as NotRTComp,
COALESCE(e.PTPlan,0) as PTPlan,
COALESCE(f.PTComp,0) as PTComp,
COALESCE(g.MTPlan,0) as MTPlan,
COALESCE(h.MTComp,0) as MTComp,
COALESCE(i.PWHTPlan,0) as PWHTPlan,
COALESCE(j.PWHTComp,0) as PWHTComp,
COALESCE(k.PMIPlan,0) as PMIPlan,
COALESCE(l.PMIComp,0) as PMIComp
from
(
select PressurePackNo from T_GDIsomJoint
group by PressurePackNo
) t
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as BWPlan from T_GDIsomJoint
where Mold = 'BW'
group by PressurePackNo
) a on t.PressurePackNo = a.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as NotBWPlan from T_GDIsomJoint
where Mold != 'BW'
group by PressurePackNo
) b on t.PressurePackNo = b.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as RTPlan from T_GDIsomJoint
where COALESCE(RT,null) != null
group by PressurePackNo
) c on t.PressurePackNo = c.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as NotRTPlan from T_GDIsomJoint
where RT is null
group by PressurePackNo
) d on t.PressurePackNo = d.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PTPlan from T_GDIsomJoint
where COALESCE(PT,null) != null
group by PressurePackNo
) e on t.PressurePackNo = e.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PTComp from T_GDIsomJoint
where PT is null
group by PressurePackNo
) f on t.PressurePackNo = f.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as MTPlan from T_GDIsomJoint
where COALESCE(MT,null) != null
group by PressurePackNo
) g on t.PressurePackNo = g.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as MTComp from T_GDIsomJoint
where MT is null
group by PressurePackNo
) h on t.PressurePackNo = h.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PWHTPlan from T_GDIsomJoint
where COALESCE(PWHT,null) != null
group by PressurePackNo
) i on t.PressurePackNo = i.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PWHTComp from T_GDIsomJoint
where PWHT is null
group by PressurePackNo
) j on t.PressurePackNo = j.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PMIPlan from T_GDIsomJoint
where COALESCE(PMI,null) != null
group by PressurePackNo
) k on t.PressurePackNo = k.PressurePackNo
left join
(
select PressurePackNo,SUM(cast(COALESCE(Size, '0') as decimal(18,2))	) as PMIComp from T_GDIsomJoint
where PMI is null
group by PressurePackNo
) l on t.PressurePackNo = l.PressurePackNo";
        //string strProjectCode = DDL_GDProject.SelectedValue;
        //if (!string.IsNullOrEmpty(strProjectCode))
        //{
        //    strGDPressureHQL += " where gDPressure.ProjectCode = '" + strProjectCode + "'";
        //}
        string strPressureCode = DDL_Pressure.SelectedValue;
        if (!string.IsNullOrEmpty(strPressureCode))
        {
            strGDPressureHQL += " where t.PressureCode = '" + strPressureCode + "'";
        }
        DataTable dtGDPressure = ShareClass.GetDataSetFromSql(strGDPressureHQL, "GDPressure").Tables[0];

        RT_List.DataSource = dtGDPressure;
        RT_List.DataBind();

    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDPressureEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDPressureBLL gDPressureBLL = new GDPressureBLL();
            string strGDPressureSql = "from GDPressure as gDPressure where PressureCode = '" + cmdArges + "'";
            IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureSql);
            if (listGDPressure != null && listGDPressure.Count == 1)
            {
                GDPressure gDPressure = (GDPressure)listGDPressure[0];
                gDPressureBLL.DeleteGDPressure(gDPressure);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }





    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
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


    private void DataGDPressureBinder()
    {
        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureHQL = "from GDPressure as gDPressure";
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureHQL);

        DDL_Pressure.DataSource = listGDPressure;
        DDL_Pressure.DataTextField = "PressureCode";
        DDL_Pressure.DataValueField = "PressureCode";
        DDL_Pressure.DataBind();

        DDL_Pressure.Items.Insert(0, new ListItem("", ""));
    }

    protected void DDL_GDProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }


    protected void DDL_Pressure_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }



    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();
    }

}