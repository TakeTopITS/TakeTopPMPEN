using System;
using System.Data;

/// <summary>
/// Summary description for DataGenerator
/// </summary>
public class DataGeneratorGantt
{
    public static DataTable GetData()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("text", typeof(string));
        dt.Columns.Add("parent", typeof(string));
        dt.Columns.Add("info", typeof(string));
        dt.Columns.Add("milestone", typeof(bool));
        dt.Columns.Add("complete", typeof(int));

        DataRow dr;

        dr = dt.NewRow();
        dr["id"] = 101;
        dr["start"] = Convert.ToDateTime("15:00");
        dr["end"] = Convert.ToDateTime("15:00").AddDays(1);
        dr["text"] = "Group 1";
        dr["info"] = "Additional info";
        dr["parent"] = null;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = Convert.ToDateTime("15:00");
        dr["end"] = Convert.ToDateTime("15:00").AddDays(1);
        dr["text"] = "Task 1";
        dr["complete"] = 50;
        dr["parent"] = 101;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = Convert.ToDateTime("15:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("15:00").AddDays(2);
        dr["text"] = "Task 2";
        dr["complete"] = 25;
        dr["parent"] = 101;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = Convert.ToDateTime("15:00").AddDays(1);
        dr["text"] = "Milestone 1";
        dr["milestone"] = true;
        dr["parent"] = 101;
        dt.Rows.Add(dr);

        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;

    }
    public static DataTable GetDataLarge()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("text", typeof(string));
        dt.Columns.Add("parent", typeof(string));

        DataRow dr;

        for (int i = 0; i < 10000; i++)
        {
            dr = dt.NewRow();
            dr["id"] = i;
            dr["start"] = Convert.ToDateTime("15:00");
            dr["end"] = Convert.ToDateTime("15:00").AddDays(1);
            dr["text"] = "Task " + i;
            dr["parent"] = null;
            dt.Rows.Add(dr);
        }

        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;

    }


}
