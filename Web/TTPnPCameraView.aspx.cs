using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using System.Collections;
using System.Data;

public partial class TTPnPCameraView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "������", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindPnPCameraData();
        }
    }

    private void BindPnPCameraData()
    {

        string strCameraInfoHQL = string.Format(@"select (case when i.CameraType = '1' then 'OrganizationalStructureDepartment'
			when i.CameraType = '2' then 'ProjectDepartment'
			end ) TypeName
            ,(case when i.CameraType = '1' then d.DepartName
			            when i.CameraType = '2' then p.ProjectName
			            end ) DPName
            ,i.CameraName,i.ServerIP,i.CameraUserName,
            i.CameraPass,i.CreatorName,i.CreateTime,i.ID,i.CameraType from T_CameraInfo i 
            left join T_Department d on i.ForeignID = d.DepartCode and i.CameraType = '1'
            left join T_Project p on i.ForeignID = cast(p.ProjectID as varchar(50)) and i.CameraType = '2'
            order by i.CreateTime desc");   
        DataTable dtCameraInfo = ShareClass.GetDataSetFromSql(strCameraInfoHQL, "strCameraInfoHQL").Tables[0];

        DG_CameraList.DataSource = dtCameraInfo;
        DG_CameraList.DataBind();
    }
}
