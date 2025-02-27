using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//���ݲ���ص���Ŀ
using TakeTopGantt.models;
using Devart.Data.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;


namespace TakeTopGantt.handler
{
    public partial class TaskController : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.dispachAction();
        }

        readonly extganttDataContext _db = new extganttDataContext();

        protected void dispachAction()
        {
            String action = this.Request["action"];
            String ret = null;
            String data = null;
            task[] jsonData = null;

            //���½�����Ŀ�У� ����id=������գ� 
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            //�жϵ�ǰ�û���û���޸��û��ƻ���Ȩ��
            int pid = Convert.ToInt32(Request["pid"]);
            if (GanttShareClass.CheckUserCanUpdatePlan(pid.ToString()) == false || GanttShareClass.CheckIsCanUpdatePlanByProjectStatus(pid.ToString()) == false)
            {
                if (action != "read")
                {
                    action = "";
                }
            }
         

            switch (action)
            {
                case "read":
                    ret = JsonConvert.SerializeObject(this.Get());
                    break;
                case "create":
                    data = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                    jsonData = JsonConvert.DeserializeObject<task[]>(data, settings);
                    ret = JsonConvert.SerializeObject(this.Create(jsonData));
                    break;
                case "update":
                    data = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                    jsonData = JsonConvert.DeserializeObject<task[]>(data, settings);
                    ret = JsonConvert.SerializeObject(this.Update(jsonData));
                    break;
                case "destroy":
                    data = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                    jsonData = JsonConvert.DeserializeObject<task[]>(data, settings);
                    ret = JsonConvert.SerializeObject(this.Delete(jsonData));
                    break;

            }
            this.Response.Write(ret);
            this.Response.End();
        }

        /*********************************************************
         * ���·ֱ�����ɾ�Ĳ�ľ���ʵ��
         * *******************************************************/
        //��ȡ�ƻ��б�
        public object Get()
        {
            extganttDataContext _db = new extganttDataContext();
            //������Ŀ��id
            int pid = Convert.ToInt32(Request["pid"]);

            //ȡֵ��parent_id = null��
            var rootTasks = _db.task.Where(b => b.parent_id == 0 && b.pid == pid);
            List<NestedTaskModel> roots = new List<NestedTaskModel>();

            foreach (task cd in rootTasks)
            {
                NestedTaskModel n = new NestedTaskModel(cd);
                roots.Add(n);
                this.SetNodeChildren(n);
            }

            return roots;
        }


        //�½��ƻ��� ע��ƻ���pidӦ�ñ���
        public Object Create(task[] jsonData)
        {
            //������Ŀ��id
            int pid = Convert.ToInt32(Request["pid"]);

            extganttDataContext _db = new extganttDataContext();

            //ǿ������pid�� ��������Ŀ����
            foreach (task t in jsonData)
            {
                t.pid = pid;
                //bryntum �Ὣroot�ڵ��parentid����Ϊnull�� �����������0���жϵ�
                if (t.parent_id == null)
                {
                    t.parent_id = 0;
                }
            }

            _db.task.InsertAllOnSubmit(jsonData);
            _db.SubmitChanges(ConflictMode.ContinueOnConflict);
            List<NestedTaskModel> roots = new List<NestedTaskModel>();

            foreach (task cd in jsonData)
            {
                NestedTaskModel n = new NestedTaskModel(cd);
                roots.Add(n);
                this.SetNodeChildren(n);
            }
            return roots;
        }

        //�ƻ��ĸ��²���
        public Object Update(task[] jsonData)
        {
            extganttDataContext _db = new extganttDataContext();
            foreach (task vals in jsonData)
            {
                task t = _db.task.SingleOrDefault(b => b.id == vals.id);

                if (t != null)
                {
                    t.name = vals.name;
                    t.parent_id = vals.parent_id;
                    t.duration = vals.duration;
                    t.duration_unit = vals.duration_unit;

                    t.percent_done = vals.percent_done;
                    t.start_date = vals.start_date;
                    t.end_date = vals.end_date;

                    t.priority = vals.priority;

                    t.baseline_start_date = vals.baseline_start_date;
                    t.baseline_end_date = vals.baseline_end_date;
                    t.baseline_percent_done = vals.baseline_percent_done;

                    t.other_field = vals.other_field;
                    t.pid = vals.pid;
                    t.index = vals.index;

                    try
                    {
                        t.leader = GanttShareClass.GetUserName(vals.leader.Trim());
                        t.leadercode = vals.leader;
                    }
                    catch
                    {
                        try
                        {
                            t.leadercode = GanttShareClass.GetUserCodeByProjectMemberName(vals.leader.Trim());
                            t.leader = vals.leader;
                        }
                        catch
                        {

                        }
                    }

                    

                    t.workhour = vals.workhour;
                    t.actualhour = vals.actualhour;

                    t.requirenumber = vals.requirenumber;
                    t.finishednumber = vals.finishednumber;
                    t.price = vals.price;
                    t.unitname = vals.unitname;

                    t.budget = vals.budget;
                    t.expense = vals.expense;

                    t.remark = vals.remark;
                    t.taskcolor = vals.taskcolor;
                    //bryntum �Ὣroot�ڵ��parentid����Ϊnull�� �����������0���жϵ�
                    if (t.parent_id == null)
                    {
                        t.parent_id = 0;
                    }
                }
            }
            _db.SubmitChanges(ConflictMode.ContinueOnConflict);

            List<NestedTaskModel> roots = new List<NestedTaskModel>();

            foreach (task cd in jsonData)
            {
                NestedTaskModel n = new NestedTaskModel(cd);
                roots.Add(n);
                this.SetNodeChildren(n);
            }
            return roots;
            //return jsonData;
        }

        public Object Delete(task[] jsonData)
        {
            extganttDataContext _db = new extganttDataContext();
            foreach (task t in jsonData)
            {
                task taskInDb = _db.task.SingleOrDefault(b => b.id == t.id);

                //��ɾ���ƻ��Ĺ�����ϵ����ɾ���ƻ�
                if (taskInDb != null)
                {
                    var deps = _db.dependency.Where(b => (b.to_id == t.id || b.from_id == t.id));
                    _db.dependency.DeleteAllOnSubmit(deps);
                    _db.task.DeleteOnSubmit(taskInDb);
                }
            }
            _db.SubmitChanges();
            return new { success = true };
        }

        //����Task���ӽڵ�
        protected void SetNodeChildren(NestedTaskModel node)
        {
            extganttDataContext _db = new extganttDataContext();
            //����node�Ķ��ӽڵ�
            var children = _db.task.Where(b => b.parent_id == node.id);

            if (children.Count<task>() > 0)
            {
                node.children = new List<NestedTaskModel>();

                //
                foreach (task t in children)
                {
                    NestedTaskModel n = new NestedTaskModel(t);
                    node.children.Add(n);
                    this.SetNodeChildren(n);
                }

                // Last step, sort children on the 'index' field
                //���һ��������node��index
                node.children = node.children.OrderBy(a => a.index).ToList();
            }
            node.leaf = (node.children == null);
            node.expanded = !node.leaf;
        }
    }
}