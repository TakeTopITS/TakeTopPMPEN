using System;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for DataGenerator
/// </summary>
public static class DataGeneratorCalendar
{
    public static DataTable GetData()
    {
        DataTable dt;
        dt = new DataTable();


        string strHQL;
        string strUserCode = HttpContext.Current.Session["UserCode"].ToString();

        strHQL = string.Format(@"Select id,start,""end"",""name"",allday,color,""column"",color from t_schedule where usercode = '{0}'", strUserCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Schedule");

        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("column", typeof(string));
        dt.Columns.Add("allday", typeof(bool));
        dt.Columns.Add("color", typeof(string));

        DataRow dr;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            dr = dt.NewRow();
            dr["id"] = ds.Tables[0].Rows[i]["id"].ToString().Trim();
            dr["start"] = Convert.ToDateTime(ds.Tables[0].Rows[i]["start"].ToString().Trim());
            dr["end"] = Convert.ToDateTime(ds.Tables[0].Rows[i]["end"].ToString().Trim());
            dr["name"] = ds.Tables[0].Rows[i]["name"].ToString().Trim();
            dr["column"] = ds.Tables[0].Rows[i]["column"].ToString().Trim();
            dr["color"] = ds.Tables[0].Rows[i]["color"].ToString().Trim();
            dt.Rows.Add(dr);
        }


        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;
    }
}
