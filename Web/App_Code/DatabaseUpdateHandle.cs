using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Web.UI;

using System.Xml;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using TakeTopCore;

using TakeTopSecurity;
using System.Web;
using com.sun.source.tree;
using Npgsql;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Web.Administration;


/// <summary>
/// DatabaseHandle 的摘要说明
/// </summary>
public static class DatabaseUpdateHandle
{
    //运行更新字段值代码
    public static void RunUpdateColumnValueCode()
    {
        int intUpdateColumnRunMarkInDB = DatabaseUpdateHandle.GetUpdateColumnValueCodeRunmark();
        //设置这个值，可以决定是否执行下面的代码
        int intUpdateColumnRunMark = 1;

        if (intUpdateColumnRunMarkInDB < intUpdateColumnRunMark)
        {
            //如果存在升级语句，那么升级数据库
            if (TakeTopCore.CoreShareClass.UpgradeDataBase() == false)
            {
            }

            try
            {
                //更新带有Status字段Table的Status的值到英文核心
                DatabaseUpdateHandle.UpdateTableStatus();
                //更新带有Type字段Table的Type的值到英文核心
                DatabaseUpdateHandle.UpdateTableType();
                //更新带有GroupName字段Table的GroupName的值到英文核心
                DatabaseUpdateHandle.UpdateTableGroup();
                //更新带有Gender字段Table的Gender的值到英文核心
                DatabaseUpdateHandle.UpdateTableGender();

                //更新带有Authority字段Table的Authority的值到英文核心
                DatabaseUpdateHandle.UpdateTableAuthority();

                ////执行模组流程定义英文化的语句
                //UpdateModulesDefinition();

                //语句执行完毕，设置标记
                DatabaseUpdateHandle.SetUpdateColumnValueCodeRunmark(intUpdateColumnRunMark);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    //运行模组名称英文化代码
    public  static void RunUpdateModuleNameCode()
    {
        int intUpdateModuleRunMarkInDB = DatabaseUpdateHandle.GetUpdateModuleNameCodeRunMark();

        //设置这个值，可以决定是否执行下面的代码
        int intUpdateModuleRunMark = 1;

        if (intUpdateModuleRunMarkInDB < intUpdateModuleRunMark)
        {
            //如果存在升级语句，那么升级数据库
            if (TakeTopCore.CoreShareClass.UpgradeDataBase() == false)
            {
            }

            try
            {
                //执行模组栏英文化的语句
                DatabaseUpdateHandle.ExcuteUpdateProModuleNameToEnglish();

                //语句执行完毕，设置标记
                DatabaseUpdateHandle.SetUpdateModuleNameCodeRunMark(intUpdateModuleRunMark);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            }
        }
    }

    //取更新字段值额外代码运行标记
    public static int GetUpdateColumnValueCodeRunmark()
    {
        string strHQL;
        int intMark = 0;
        strHQL = "Select UpdateColumnValueCodeRunmark From T_OtherCodeRunMark";
        DataSet dataSet = ShareClass.GetDataSetFromSql(strHQL, "T_OtherCodeRunMark");
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            intMark = Convert.ToInt32(dataSet.Tables[0].Rows[0]["UpdateColumnValueCodeRunmark"].ToString());
        }
        else
        {
            intMark = 0;
        }

        return intMark;
    }

    //设更新字段值额外代码运行标记
    public static void SetUpdateColumnValueCodeRunmark(int intMark)
    {
        string strHQL;
        strHQL = "Update T_OtherCodeRunMark Set UpdateColumnValueCodeRunmark = " + intMark;
        ShareClass.RunSqlCommand(strHQL);
    }

    //取更新模组栏值额外代码运行标记
    public static int GetUpdateModuleNameCodeRunMark()
    {
        string strHQL;
        int intMark = 0;
        strHQL = "Select UpdateModuleNameCodeRunMark From T_OtherCodeRunMark";
        DataSet dataSet = ShareClass.GetDataSetFromSql(strHQL, "T_OtherCodeRunMark");
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            intMark = Convert.ToInt32(dataSet.Tables[0].Rows[0]["UpdateModuleNameCodeRunMark"].ToString());
        }
        else
        {
            intMark = 0;
        }

        return intMark;
    }

    //设更新模组栏值额外代码运行标记
    public static void SetUpdateModuleNameCodeRunMark(int intMark)
    {
        string strHQL;
        strHQL = "Update T_OtherCodeRunMark Set UpdateModuleNameCodeRunMark = " + intMark;
        ShareClass.RunSqlCommand(strHQL);
    }




    //更新带有Status字段Table的Status的值到英文核心
    public static void UpdateTableStatus()
    {
        // 查询所有包含'status'列的表
        var strHQL = @"
              SELECT c.table_name, c.column_name
        FROM information_schema.columns c
        JOIN information_schema.tables t
          ON c.table_schema = t.table_schema
          AND c.table_name = t.table_name
        WHERE c.column_name LIKE '%status%'
          AND c.table_schema = 'public'
          AND t.table_type = 'BASE TABLE'
          AND c.table_name NOT IN ('pbcatvld', 'pbcatedt','t_memberlevel','t_triggertabletofrom','t_sendtask','t_sentrecord') AND C.column_name not like '%id%';";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Table");

        try
        {
            strHQL = "DROP VIEW public.v_mymember_workload;";
            ShareClass.RunSqlCommand(strHQL);
            strHQL = "DROP VIEW public.v_projectmember_workload;";
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }


        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string tableName = ds.Tables[0].Rows[i][0].ToString();
            string columnName = ds.Tables[0].Rows[i][1].ToString();



            // 构建并执行更新语句
            string updateQuery = $@"
                UPDATE {tableName} 
                SET {columnName} = CASE 
                    WHEN {columnName} = '新建' THEN 'New'
                    WHEN {columnName} = '评审' THEN 'Review'
                    WHEN {columnName} = '批准' THEN 'Approved'
                    WHEN {columnName} = '受理' THEN 'Accepted'
                    WHEN {columnName} = '拒绝' THEN 'Rejected'
                    WHEN {columnName} = '处理中' THEN 'InProgress'
                    WHEN {columnName} = '进行中' THEN 'InProgress'
                    WHEN {columnName} = '测试' THEN 'Testing'
                    WHEN {columnName} = '完成' THEN 'Completed'
                    WHEN {columnName} = '转项' THEN 'ToProject'
                    WHEN {columnName} = '通过' THEN 'Passed'
                    WHEN {columnName} = '转任' THEN 'ToTask'
                    WHEN {columnName} = '审核中' THEN 'Reviewing'
                    WHEN {columnName} = '评审中' THEN 'Reviewing'
                    WHEN {columnName} = '验收' THEN 'Acceptance'
                    WHEN {columnName} = '会签中' THEN 'Signing'
                    WHEN {columnName} = '更改中' THEN 'Updating'
                    WHEN {columnName} = '延期' THEN 'Postponed'
                    WHEN {columnName} = '拖期' THEN 'Delayed'
                    WHEN {columnName} = '复核中' THEN 'ReReview'
                    WHEN {columnName} = '挂起' THEN 'Suspended'
                    WHEN {columnName} = '关闭' THEN 'Closed'
                    WHEN {columnName} = '结案' THEN 'CaseClosed'
                               
                    WHEN {columnName} = '删除' THEN 'Deleted'
                    WHEN {columnName} = '归档' THEN 'Archived'
                    WHEN {columnName} = '分派' THEN 'Assigned'
                    WHEN {columnName} = '隐藏' THEN 'Hided'
                    WHEN {columnName} = '计划' THEN 'Plan'
                    WHEN {columnName} = '取消' THEN 'Cancel'
                    WHEN {columnName} = '待处理' THEN 'ToHandle'
                    WHEN {columnName} = '暂停' THEN 'Pause'
                    WHEN {columnName} = '终止' THEN 'Stop'

                    WHEN {columnName} = '在职' THEN 'InPosition'
                    WHEN {columnName} = '离职' THEN 'Depart'

                    WHEN {columnName} = '未婚' THEN 'Stop'
                    WHEN {columnName} = '已婚' THEN 'Married'
                    WHEN {columnName} = '离异' THEN 'Divorced'
                    WHEN {columnName} = '丧偶' THEN 'Widowed'

                    WHEN {columnName} = '预警的' THEN 'Warning'
                    WHEN {columnName} = '已处理' THEN 'Processed'
                    WHEN {columnName} = '已删除' THEN 'Deleted'
                    WHEN {columnName} = '所有' THEN 'All'
                    WHEN {columnName} = '采购中' THEN 'Procuring'
                    WHEN {columnName} = '评审中' THEN 'Reviewing'
                    WHEN {columnName} = '合格' THEN 'Qualified'
                    WHEN {columnName} = '不合格' THEN 'Unqualified'
                    WHEN {columnName} = '出车' THEN 'Departure'
                    WHEN {columnName} = '收车' THEN 'Return'
                    WHEN {columnName} = '未发' THEN 'Unsent'
                    WHEN {columnName} = '已发' THEN 'Sent'
                    WHEN {columnName} = '启用' THEN 'Enabled'
                    WHEN {columnName} = '禁用' THEN 'Disabled'
                    WHEN {columnName} = '已记账' THEN 'Recorded'
                    WHEN {columnName} = '销售中' THEN 'Selling'
                    WHEN {columnName} = '已调查' THEN 'Investigated'
                    WHEN {columnName} = '已演练' THEN 'Drilled'
                    WHEN {columnName} = '正常' THEN 'Normal'
                    WHEN {columnName} = '异常' THEN 'Abnormal'
                    WHEN {columnName} = '紧急' THEN 'Emergency'
                    WHEN {columnName} = '整改中' THEN 'Rectifying'
                    WHEN {columnName} = '可外借' THEN 'Borrowable'
                    WHEN {columnName} = '不外借' THEN 'Not Borrowable'
                    WHEN {columnName} = '维修' THEN 'Maintenance'
                    WHEN {columnName} = '报废' THEN 'Scrapped'
                    WHEN {columnName} = '执行中' THEN 'Executing'
                    WHEN {columnName} = '待审核' THEN 'Pending Review'
                    WHEN {columnName} = '现行' THEN 'Current'
                    WHEN {columnName} = '作废' THEN 'Obsolete'
                    WHEN {columnName} = '潜在' THEN 'Potential'
                    WHEN {columnName} = '暴露' THEN 'Exposed'
                    WHEN {columnName} = '发生' THEN 'Occurred'
                    WHEN {columnName} = '解除' THEN 'Resolved'
                    WHEN {columnName} = '已报验' THEN 'Reported'
                    WHEN {columnName} = '已整改' THEN 'Rectified'
                    WHEN {columnName} = '已交' THEN 'Submitted'
                    WHEN {columnName} = '未交' THEN 'Unsubmitted'
                    WHEN {columnName} = '停用' THEN 'Deactivated'
                    WHEN {columnName} = '测试中' THEN 'Testing'
                    WHEN {columnName} = '失败' THEN 'Failed'
                    WHEN {columnName} = '成功' THEN 'Success'
                    WHEN {columnName} = '投标中' THEN 'Bidding'
                    WHEN {columnName} = '已立项' THEN 'Approved'
                    WHEN {columnName} = '未中标' THEN 'UnsuccessfulBid'
                    WHEN {columnName} = '在用' THEN 'InUse'

               

                               

                    ELSE {columnName} END
                WHERE {columnName} IN (
                    '新建', '评审', '批准', '受理', '拒绝', '处理中','进行中', '测试', '完成', '转项', '通过', '转任', 
                    '审核中', '评审中','验收', '会签中', '更改中', '延期', '拖期', '复核中', '挂起', '关闭', '结案', 
                    '删除', '归档', '分派', '隐藏', '计划', '取消', '待处理','暂停','终止','在职','离职','未婚','已婚','离异','丧偶',
                    '预警的', '预警的', '已处理', '已处理', '已删除', '已删除', '所有', '所有', '采购中', '采购中', '评审中', '评审中', 
                    '合格', '合格', '不合格', '不合格', '出车', '出车', '收车', '收车', '未发', '未发', '已发', '已发', '启用', '启用', '禁用',
                    '禁用', '已记账', '已记账', '销售中', '销售中', '已调查', '已调查', '已演练', '已演练', '正常', '正常', '异常', '异常', '紧急', '紧急',
                    '整改中', '整改中', '可外借', '可外借', '不外借', '不外借', '维修', '维修', '报废', '报废', '执行中', '执行中', '待审核', '待审核', '现行', '现行',
                    '作废', '作废', '潜在', '潜在', '暴露', '暴露', '发生', '发生', '解除', '解除', '已报验', '已报验', '已整改', '已整改', '已交', '已交', '未交', '未交',
                    '停用', '停用', '测试中', '测试中', '失败', '失败', '成功', '成功', '投标中', '投标中', '已立项', '已立项', '未中标','在用');";





            try
            {
                ShareClass.RunSqlCommand(updateQuery);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(updateQuery + " error:" + ex.Message.ToString());
            }

            strHQL = @"
                    CREATE OR REPLACE VIEW public.v_projectmember_workload
                     AS
                     SELECT '项目'::text AS type,
                        t_project.pmcode AS usercode,
                        t_project.pmname AS username,
                        t_project.projectid::character varying(30) AS projectid,
                        t_project.projectname,
                        t_project.begindate,
                        t_project.enddate,
                        t_project.status
                       FROM t_project
                      WHERE t_project.status <> ALL (ARRAY['关闭'::bpchar, '完成'::bpchar, '结案'::bpchar, '取消'::bpchar, '删除'::bpchar, '归档'::bpchar, '拒绝'::bpchar, '挂起'::bpchar])
                    UNION
                     SELECT '项目'::text AS type,
                        a.usercode,
                        a.username,
                        a.projectid::character varying(30) AS projectid,
                        a.projectname,
                        b.begindate,
                        b.enddate,
                        b.status
                       FROM t_relateduser a,
                        t_project b
                      WHERE a.projectid = b.projectid AND (a.projectid IN ( SELECT t_project.projectid
                               FROM t_project
                              WHERE t_project.status <> ALL (ARRAY['关闭'::bpchar, '完成'::bpchar, '结案'::bpchar, '取消'::bpchar, '删除'::bpchar, '归档'::bpchar, '拒绝'::bpchar, '挂起'::bpchar]))) AND (a.projectname::bpchar IN ( SELECT t_project.projectname
                               FROM t_project
                              WHERE t_project.status <> ALL (ARRAY['关闭'::bpchar, '完成'::bpchar, '结案'::bpchar, '取消'::bpchar, '删除'::bpchar, '归档'::bpchar, '拒绝'::bpchar, '挂起'::bpchar])))
                    UNION
                     SELECT '计划'::text AS type,
                        a.usercode,
                        a.username,
                        (('Project:'::text || b.projectid::character varying(30)::text) || ' Plan:'::text) || b.id::character varying(30)::text AS projectid,
                        b.name AS projectname,
                        b.start_date AS begindate,
                        b.end_date AS enddate,
                        b.status
                       FROM t_planmember a,
                        t_impleplan b
                      WHERE a.planid = b.id AND b.type = 'InUse'::bpchar
                    UNION
                     SELECT '计划'::text AS type,
                        t_plan.usercode,
                        t_plan.username,
                        t_plan.planid::character varying(30) AS projectid,
                        t_plan.planname AS projectname,
                        t_plan.starttime AS begindate,
                        t_plan.endtime AS enddate,
                        t_plan.status
                       FROM t_plan
                    UNION
                     SELECT '任务'::text AS type,
                        a.operatorcode AS usercode,
                        a.operatorname AS username,
                        (('Project:'::text || b.projectid::character varying(30)::text) || ' Task:'::text) || a.taskid::character varying(30)::text AS projectid,
                        a.task AS projectname,
                        a.begindate,
                        a.enddate,
                        b.status
                       FROM t_taskassignrecord a,
                        t_projecttask b
                      WHERE a.taskid = b.taskid
                    UNION
                     SELECT '需求'::text AS type,
                        t_reqassignrecord.operatorcode AS usercode,
                        t_reqassignrecord.operatorname AS username,
                        t_reqassignrecord.reqid::character varying(30) AS projectid,
                        t_reqassignrecord.reqname AS projectname,
                        t_reqassignrecord.begindate,
                        t_reqassignrecord.enddate,
                        t_reqassignrecord.status
                       FROM t_reqassignrecord
                    UNION
                     SELECT '会议'::text AS type,
                        a.usercode,
                        a.username,
                        a.meetingid::character varying(30) AS projectid,
                        b.name AS projectname,
                        b.begintime AS begindate,
                        b.endtime AS enddate,
                        b.status
                       FROM t_meetingattendant a,
                        t_meeting b
                      WHERE a.meetingid = b.id;

                    ALTER TABLE public.v_projectmember_workload
                        OWNER TO postgres;";
            try
            {
                ShareClass.RunSqlCommand(strHQL);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(strHQL + " error:" + ex.Message.ToString());
            }

            strHQL = @"
                        CREATE OR REPLACE VIEW public.v_mymember_workload
                         AS
                         SELECT DISTINCT a.type,
                            a.usercode,
                            a.username,
                            a.projectid,
                            a.projectname,
                            a.begindate,
                            a.enddate,
                            b.sortnumber,
                            a.status
                           FROM v_projectmember_workload a,
                            t_memberlevel b
                          WHERE a.usercode = b.usercode AND (a.status <> ALL (ARRAY['关闭'::bpchar, '完成'::bpchar, '结案'::bpchar, '取消'::bpchar, '删除'::bpchar, '归档'::bpchar, '拒绝'::bpchar, '挂起'::bpchar, '转项'::bpchar, '转任'::bpchar]));

                        ALTER TABLE public.v_mymember_workload
                            OWNER TO postgres;";
            try
            {
                ShareClass.RunSqlCommand(strHQL);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(strHQL + " error:" + ex.Message.ToString());
            }
        }
    }

    //更新带有Type字段Table的Type的值到英文核心
    public static void UpdateTableType()
    {
        // 查询所有包含'Type'列的表
        var strHQL = @"
              SELECT c.table_name, c.column_name
        FROM information_schema.columns c
        JOIN information_schema.tables t
          ON c.table_schema = t.table_schema
          AND c.table_name = t.table_name
        WHERE c.column_name LIKE '%type%'
          AND c.table_schema = 'public'
          AND t.table_type = 'BASE TABLE'
          AND c.table_name NOT IN ('pbcatvld', 'pbcatedt')
AND c.table_name NOT Like '%rcj%' AND C.column_name not like '%id%' and C.column_name <> 'warrantyperiod';";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Table");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string tableName = ds.Tables[0].Rows[i][0].ToString();
            string columnName = ds.Tables[0].Rows[i][1].ToString();



            // 构建并执行更新语句
            string updateQuery = $@"
                            UPDATE {tableName} 
                            SET {columnName} = CASE 

                                WHEN {columnName} = '在用' Then 'InUse'
                                WHEN {columnName} = '备用' THEN 'Backup'
                                WHEN {columnName} = '基准' THEN 'Baseline'

                                WHEN {columnName} = '采购件' Then 'PurchParts'
                                WHEN {columnName} = '自制件' THEN 'MadeParts'
                                WHEN {columnName} = '外协件' THEN 'OutParts'
                                WHEN {columnName} = '交付件' THEN 'DelivParts'

                                WHEN {columnName} = '房屋租凭' THEN 'HouseRental'
                                WHEN {columnName} = '市场营销' THEN 'Marketing'
                                WHEN {columnName} = '考勤管理' THEN 'AttendanceManagement'
                                WHEN {columnName} = '招标管理' THEN 'TenderManagement'
                                WHEN {columnName} = '运营管理' THEN 'OperationManagement'
                                WHEN {columnName} = '供应商管理' THEN 'SupplierManagement'
                                WHEN {columnName} = '分包申请' THEN 'SubcontractApplication'
                                WHEN {columnName} = '费用报销' THEN 'ExpenseReimbursement'
                                WHEN {columnName} = '设计生产' THEN 'DesignAndProduction'
                                WHEN {columnName} = '计划审批' THEN 'PlanApproval'
                                WHEN {columnName} = '招标计划' THEN 'TenderPlan'
                                WHEN {columnName} = '付款申请' THEN 'PaymentRequest'
                                WHEN {columnName} = '费用申请' THEN 'ExpenseRequest'
                                WHEN {columnName} = '签证管理' THEN 'VisaManagement'
                                WHEN {columnName} = '计划评审' THEN 'PlanReview'
                                WHEN {columnName} = '发票处理' THEN 'InvoiceProcessing'
                                WHEN {columnName} = '项目评审' THEN 'ProjectReview'
                                WHEN {columnName} = '状态评审' THEN 'StatusReview'
                                WHEN {columnName} = '需求评审' THEN 'RequirementReview'
                                WHEN {columnName} = '任务评审' THEN 'TaskReview'
                                WHEN {columnName} = '出差申请' THEN 'BusinessTripRequest'
                                WHEN {columnName} = '物料采购' THEN 'MaterialProcurement'
                                WHEN {columnName} = '物料领用' THEN 'MaterialWithdrawal'
                                WHEN {columnName} = '文件评审' THEN 'DocumentReview'
                                WHEN {columnName} = '资产采购' THEN 'AssetProcurement'
                                WHEN {columnName} = '物料销售' THEN 'MaterialSales'
                                WHEN {columnName} = '资产领用' THEN 'AssetWithdrawal'
                                WHEN {columnName} = '物料供货' THEN 'MaterialSupply'
                                WHEN {columnName} = '投标管理' THEN 'BidManagement'
                                WHEN {columnName} = '质检流程' THEN 'QualityInspectionProcess'
                                WHEN {columnName} = '物料生产' THEN 'MaterialProduction'
                                WHEN {columnName} = '合同评审' THEN 'ContractReview'
                                WHEN {columnName} = '学生管理' THEN 'StudentManagement'
                                WHEN {columnName} = '风险评估' THEN 'RiskAssessment'
                                WHEN {columnName} = '客服评审' THEN 'CustomerServiceReview'
                                WHEN {columnName} = '应聘申请' THEN 'JobApplication'
                                WHEN {columnName} = '车辆申请' THEN 'VehicleRequest'
                                WHEN {columnName} = '会议申请' THEN 'MeetingRequest'
                                WHEN {columnName} = '物料报价' THEN 'MaterialQuotation'
                                WHEN {columnName} = '预算管理' THEN 'BudgetManagement'
                                WHEN {columnName} = '公文审批' THEN 'OfficialDocumentApproval'
                                WHEN {columnName} = '人事管理' THEN 'HumanResourcesManagement'
                                WHEN {columnName} = '物料采购付款申请' THEN 'MaterialProcurementPaymentRequest'
                                WHEN {columnName} = '资产采购付款申请' THEN 'AssetProcurementPaymentRequest'
                                WHEN {columnName} = '物资采购付款申请' THEN 'MaterialProcurementPaymentRequest'
                                WHEN {columnName} = '出货申请' THEN 'ShippingRequest'
                                 
                                WHEN {columnName} = '基础' THEN 'Base'
                                WHEN {columnName} = '预算' THEN 'Budget'

                                WHEN {columnName} = '操作' THEN 'Operation'
                                WHEN {columnName} = '其它' THEN 'Other'
                               
                                WHEN {columnName} = '验收' THEN 'Acceptancet'
                                WHEN {columnName} = '分包' THEN 'Subcontract'

                                WHEN {columnName} = '项目' THEN 'Project'
                                WHEN {columnName} = '计划' THEN 'Plan'
                                WHEN {columnName} = '需求' THEN 'Requirement'
                                WHEN {columnName} = '任务' THEN 'Task'
                                WHEN {columnName} = '缺陷' THEN 'Defect'
                                WHEN {columnName} = '工作流' THEN 'Workflow'

                                WHEN {columnName} = '标准版' THEN 'StandardEdition'
                                WHEN {columnName} = '企业版' THEN 'EnterpriseEdition'
                                WHEN {columnName} = '集团版' THEN 'GroupEdition'
                                WHEN {columnName} = '资产' THEN 'Assets'
                                WHEN {columnName} = '负债' THEN 'Liabilities'
                                WHEN {columnName} = '成本' THEN 'Cost'
                                WHEN {columnName} = '权益' THEN 'Equity'
                                WHEN {columnName} = '损益' THEN 'ProfitandLoss'
                                WHEN {columnName} = '客户' THEN 'Customer'
                                WHEN {columnName} = '供应商' THEN 'Supplier'
                                WHEN {columnName} = '同事' THEN 'Colleague'
                                WHEN {columnName} = '朋友' THEN 'Friend'
                                WHEN {columnName} = '同学' THEN 'Classmate'
                                WHEN {columnName} = '亲属' THEN 'Relative'
                                WHEN {columnName} = '国税' THEN 'NationalTax'
                                WHEN {columnName} = '地税' THEN 'LocalTax'
                                WHEN {columnName} = '显示异常' THEN 'DisplayAbnormal'
                                WHEN {columnName} = '显示全部' THEN 'DisplayAll'
                                WHEN {columnName} = '澄清文件' THEN 'ClarificationDocument'
                                WHEN {columnName} = '答疑文件' THEN 'Q&ADocument'
                                WHEN {columnName} = '国企' THEN 'State-ownedEnterprise'
                                WHEN {columnName} = '事业单位' THEN 'PublicInstitution'
                                WHEN {columnName} = '私企' THEN 'PrivateEnterprise'
                                WHEN {columnName} = '外资(合资)' THEN 'Foreign(JointVenture)'
                                WHEN {columnName} = '股份制' THEN 'ShareholdingSystem'
                                WHEN {columnName} = '图纸' THEN 'Drawing'
                                WHEN {columnName} = '文件' THEN 'Document'
                                WHEN {columnName} = '缺陷评审' THEN 'DefectReview'
                                WHEN {columnName} = '生产领料' THEN 'ProductionPicking'
                                WHEN {columnName} = '生产补料' THEN 'ProductionReplenishment'
                                WHEN {columnName} = '物料借出' THEN 'MaterialBorrowing'
                                WHEN {columnName} = '借出归还' THEN 'BorrowingReturn'
                                WHEN {columnName} = '生产退料' THEN 'ProductionReturn'
                                WHEN {columnName} = '采购退货' THEN 'PurchaseReturn'
                                WHEN {columnName} = '销售退货' THEN 'SalesReturn'
                                WHEN {columnName} = '采购单' THEN 'PurchaseOrder'
                                WHEN {columnName} = '物料质检' THEN 'MaterialQualityInspection'
                                WHEN {columnName} = '内部' THEN 'Internal'
                                WHEN {columnName} = '外部' THEN 'External'
                                WHEN {columnName} = '尘肺' THEN 'Pneumoconiosis'
                                WHEN {columnName} = '放射病' THEN 'RadiationSickness'
                                WHEN {columnName} = '中毒' THEN 'Poisoning'
                                WHEN {columnName} = '物理因素' THEN 'PhysicalFactors'
                                WHEN {columnName} = '生物因素' THEN 'BiologicalFactors'
                                WHEN {columnName} = '皮肤病' THEN 'SkinDisease'
                                WHEN {columnName} = '眼病' THEN 'EyeDisease'
                                WHEN {columnName} = '耳鼻喉疾病' THEN 'ENTDisease'
                                WHEN {columnName} = '肿瘤' THEN 'Tumor'
                                WHEN {columnName} = '其他' THEN 'Other'
                                WHEN {columnName} = '年例会' THEN 'AnnualMeeting'
                                WHEN {columnName} = '季例会' THEN 'QuarterlyMeeting'
                                WHEN {columnName} = '月例会' THEN 'MonthlyMeeting'
                                WHEN {columnName} = '周例会' THEN 'WeeklyMeeting'
                                WHEN {columnName} = '人员备案' THEN 'PersonnelFiling'
                                WHEN {columnName} = '设备备案' THEN 'EquipmentFiling'
                                WHEN {columnName} = '入场培训' THEN 'OnboardingTraining'
                                WHEN {columnName} = '专项培训' THEN 'SpecialTraining'
                                WHEN {columnName} = '里程碑' THEN 'Milestone'
                                WHEN {columnName} = '甲方原因' THEN 'PartyAReason'
                                WHEN {columnName} = '乙方原因' THEN 'PartyBReason'
                                WHEN {columnName} = '双方原因' THEN 'BothPartiesReason'
                                WHEN {columnName} = '顺排' THEN 'ForwardScheduling'
                                WHEN {columnName} = '倒排' THEN 'BackwardScheduling'
                                WHEN {columnName} = '公告文件' THEN 'AnnouncementDocument'
                                WHEN {columnName} = '通知文件' THEN 'NotificationDocument'
                                WHEN {columnName} = '标书购买' THEN 'BidDocumentPurchase'
                                WHEN {columnName} = '交保证金' THEN 'PayDeposit'
                                WHEN {columnName} = '收保证金' THEN 'ReceiveDeposit'
                                WHEN {columnName} = '开标' THEN 'BidOpening'
                                WHEN {columnName} = '交中标费' THEN 'PayWinningBidFee'
                                WHEN {columnName} = '请选择' THEN 'PleaseSelect'
                                WHEN {columnName} = '员工培训' THEN 'EmployeeTraining'
                                WHEN {columnName} = '培训记录' THEN 'TrainingRecord'
                                WHEN {columnName} = '特种作业' THEN 'SpecialOperations'
                                WHEN {columnName} = '特种设备' THEN 'SpecialEquipment'
                                WHEN {columnName} = '焊接持证' THEN 'WeldingCertification'
                                WHEN {columnName} = '施工管理员证' THEN 'ConstructionManagerCertificate'
                                WHEN {columnName} = '上午上班时间' THEN 'MorningWorkStartTime'
                                WHEN {columnName} = '上午下班时间' THEN 'MorningWorkEndTime'
                                WHEN {columnName} = '下午上班时间' THEN 'AfternoonWorkStartTime'
                                WHEN {columnName} = '下午下班时间' THEN 'AfternoonWorkEndTime'
                                WHEN {columnName} = '晚班上班时间' THEN 'NightShiftStartTime'
                                WHEN {columnName} = '晚班下班时间' THEN 'NightShiftEndTime'
                                WHEN {columnName} = '午夜上班时间' THEN 'MidnightWorkStartTime'
                                WHEN {columnName} = '午夜下班时间' THEN 'MidnightWorkEndTime'
                                WHEN {columnName} = '无类型' THEN 'NoType'
                                WHEN {columnName} = '全职' THEN 'Full-time'
                                WHEN {columnName} = '兼职' THEN 'Part-time'
                                WHEN {columnName} = '实习' THEN 'Internship'
                                WHEN {columnName} = '劳务派遣' THEN 'LaborDispatch'
                                WHEN {columnName} = '退休返聘' THEN 'RetirementRehire'
                                WHEN {columnName} = '劳务外包' THEN 'LaborOutsourcing'
                                WHEN {columnName} = '本地城镇' THEN 'LocalUrban'
                                WHEN {columnName} = '本地农村' THEN 'LocalRural'
                                WHEN {columnName} = '外地城镇（省外）' THEN 'Non-localUrban(OutofProvince)'
                                WHEN {columnName} = '外地农村（省外）' THEN 'Non-localRural(OutofProvince)'
                                WHEN {columnName} = '劳动合同' THEN 'LaborContract'
                                WHEN {columnName} = '实习协议' THEN 'InternshipAgreement'
                                WHEN {columnName} = '返聘协议' THEN 'RehireAgreement'
                                WHEN {columnName} = '无合同无协议' THEN 'NoContractorAgreement'
                                WHEN {columnName} = '入职' THEN 'Onboarding'
                                WHEN {columnName} = '升职' THEN 'Promotion'
                                WHEN {columnName} = '调动' THEN 'Transfer'
                                WHEN {columnName} = '适用类别' THEN 'ApplicableCategory'
                                WHEN {columnName} = '焊接方法' THEN 'WeldingMethod'
                                WHEN {columnName} = '焊后热处理方法' THEN 'Post-WeldHeatTreatmentMethod'
                                WHEN {columnName} = '焊后热处理类别' THEN 'Post-WeldHeatTreatmentCategory'
                                WHEN {columnName} = '母材钢号' THEN 'BaseMaterialSteelGrade'
                                WHEN {columnName} = '母材规格' THEN 'BaseMaterialSpecification'
                                WHEN {columnName} = '母材类别' THEN 'BaseMaterialCategory'
                                WHEN {columnName} = '焊材类别' THEN 'WeldingMaterialCategory'
                                WHEN {columnName} = '焊丝型号' THEN 'WeldingWireModel'
                                WHEN {columnName} = '焊条型号' THEN 'WeldingRodModel'
                                WHEN {columnName} = '焊剂型号' THEN 'FluxModel'
                                WHEN {columnName} = '焊丝牌号' THEN 'WeldingWireBrand'
                                WHEN {columnName} = '焊条牌号' THEN 'WeldingRodBrand'
                                WHEN {columnName} = '焊剂牌号' THEN 'FluxBrand'
                                WHEN {columnName} = '焊丝规格' THEN 'WeldingWireSpecification'
                                WHEN {columnName} = '焊条规格' THEN 'WeldingRodSpecification'
                                WHEN {columnName} = '焊剂规格' THEN 'FluxSpecification'
                                WHEN {columnName} = '焊接方向' THEN 'WeldingDirection'
                                WHEN {columnName} = '冷却方法' THEN 'CoolingMethod'
                                WHEN {columnName} = '类别组号' THEN 'CategoryGroupNumber'
                                WHEN {columnName} = '加热方式' THEN 'HeatingMethod'
                                WHEN {columnName} = '焊材标准' THEN 'WeldingMaterialStandard'
                                WHEN {columnName} = '摆动焊' THEN 'OscillationWelding'
                                WHEN {columnName} = '不摆动焊' THEN 'Non-oscillationWelding'
                                WHEN {columnName} = '单道焊' THEN 'Single-passWelding'
                                WHEN {columnName} = '多道焊' THEN 'Multi-passWelding'
                                WHEN {columnName} = '单丝焊' THEN 'Single-wireWelding'
                                WHEN {columnName} = '多丝焊' THEN 'Multi-wireWelding'
                                WHEN {columnName} = '喷射弧' THEN 'SprayArc'
                                WHEN {columnName} = '短路弧' THEN 'ShortCircuitArc'
                                WHEN {columnName} = '物资招标专家' THEN 'MaterialBiddingExpert'
                                WHEN {columnName} = '工程招标专家' THEN 'EngineeringBiddingExpert'
                                WHEN {columnName} = '其他招标专家' THEN 'OtherBiddingExpert'

                                WHEN {columnName} = '所有' THEN 'All'
                                WHEN {columnName} = '部分' THEN 'Part'
                                WHEN {columnName} = '超级' THEN 'Super'

                                ELSE {columnName} END
                            WHERE {columnName} IN ('在用','备用','基准','采购件','自制件','外协件','交付件','房屋租凭','市场营销','考勤管理','招标管理','运营管理','供应商管理',
                                 '分包申请','费用报销','设计生产','计划审批','招标计划','付款申请','费用申请','签证管理','计划评审','发票处理','项目评审','状态评审','需求评审',
                                '任务评审','出差申请','物料采购','物料领用','文件评审','资产采购','物料销售','资产领用','物料供货','投标管理','质检流程','物料生产','合同评审',
                                '学生管理','风险评估','客服评审','应聘申请','车辆申请','物料报价','预算管理','公文审批','人事管理','物料采购付款申请','资产采购付款申请',
                                '物资采购付款申请','出货申请','基础','预算','操作','其它','会议申请','验收','分包','项目','计划','需求','任务','缺陷','工作流',
                                '标准版','标准版','企业版','企业版','集团版','集团版','资产','资产','负债','负债','成本','成本','权益','权益','损益','损益','客户','客户','供应商','供应商','同事','同事','朋友','朋友','同学','同学','亲属','亲属','国税','国税','地税','地税','显示异常','显示异常','显示全部','显示全部','澄清文件','澄清文件','答疑文件','答疑文件','国企','国企','事业单位','事业单位','私企','私企','外资(合资)','外资(合资)','股份制','股份制','图纸','图纸','文件','文件','缺陷评审','缺陷评审','生产领料','生产领料','生产补料','生产补料','物料借出','物料借出','借出归还','借出归还','生产退料','生产退料','采购退货','采购退货','销售退货','销售退货','采购单','采购单','物料质检','物料质检','内部','内部','外部',
                               '外部','尘肺','尘肺','放射病','放射病','中毒','中毒','物理因素','物理因素','生物因素','生物因素','皮肤病','皮肤病','眼病','眼病','耳鼻喉疾病','耳鼻喉疾病','肿瘤','肿瘤','其他','其他','年例会','年例会','季例会','季例会','月例会','月例会','周例会','周例会','人员备案','人员备案','设备备案','设备备案','入场培训','入场培训','专项培训','专项培训','里程碑','里程碑','甲方原因','甲方原因','乙方原因','乙方原因','双方原因','双方原因','顺排','顺排','倒排','倒排','公告文件','公告文件','通知文件','通知文件','标书购买','标书购买','交保证金','交保证金','收保证金','收保证金','开标','开标','交中标费','交中标费','请选择','请选择','员工培训','员工培训','培训记录','培训记录','特种作业','特种作业',
                               '特种设备','特种设备','焊接持证','焊接持证','施工管理员证','施工管理员证','上午上班时间','上午上班时间','上午下班时间','上午下班时间','下午上班时间','下午上班时间','下午下班时间','下午下班时间','晚班上班时间','晚班上班时间','晚班下班时间','晚班下班时间','午夜上班时间','午夜上班时间','午夜下班时间','午夜下班时间','无类型','无类型','全职','全职','兼职','兼职','实习','实习','劳务派遣','劳务派遣','退休返聘','退休返聘','劳务外包','劳务外包','本地城镇','本地城镇','本地农村','本地农村','外地城镇（省外）','外地城镇（省外）','外地农村（省外）','外地农村（省外）','劳动合同','劳动合同','实习协议','实习协议','返聘协议','返聘协议','无合同无协议','无合同无协议','入职','入职','升职','升职',
                               '调动','调动','适用类别','适用类别','焊接方法','焊接方法','焊后热处理方法','焊后热处理方法','焊后热处理类别','焊后热处理类别','母材钢号','母材钢号','母材规格','母材规格','母材类别','母材类别','焊材类别','焊材类别','焊丝型号','焊丝型号','焊条型号','焊条型号','焊剂型号','焊剂型号','焊丝牌号','焊丝牌号','焊条牌号','焊条牌号','焊剂牌号','焊剂牌号','焊丝规格','焊丝规格','焊条规格','焊条规格','焊剂规格','焊剂规格','焊接方向','焊接方向','冷却方法','冷却方法','类别组号','类别组号','加热方式','加热方式','焊材标准','焊材标准','摆动焊','摆动焊','不摆动焊','不摆动焊','单道焊','单道焊','多道焊','多道焊','单丝焊','单丝焊','多丝焊','多丝焊','喷射弧','喷射弧','短路弧','短路弧','物资招标专家','物资招标专家','工程招标专家','工程招标专家','其他招标专家','其他招标专家',
                                '所有','部分','超级' );";


            try
            {
                ShareClass.RunSqlCommand(updateQuery);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(updateQuery + " error:" + ex.Message.ToString());
            }
        }
    }


    //更新带有GroupName字段Table的GroupName的值到英文核心
    public static void UpdateTableGroup()
    {
        // 查询所有包含'status'列的表
        var strHQL = @"
              SELECT c.table_name, c.column_name
        FROM information_schema.columns c
        JOIN information_schema.tables t
          ON c.table_schema = t.table_schema
          AND c.table_name = t.table_name
        WHERE c.column_name LIKE '%group%'
          AND c.table_schema = 'public'
          AND t.table_type = 'BASE TABLE'
          AND c.table_name NOT IN ('pbcatvld', 'pbcatedt') AND C.column_name not like '%id%';";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Table");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string tableName = ds.Tables[0].Rows[i][0].ToString();
            string columnName = ds.Tables[0].Rows[i][1].ToString();

            // 构建并执行更新语句
            string updateQuery = $@"
                            UPDATE {tableName} 
                            SET {columnName} = CASE 
                                WHEN {columnName} = '个人' THEN 'Individual'
                                WHEN {columnName} = '所有' THEN 'All'
                                WHEN {columnName} = '部门' THEN 'Department'
                                WHEN {columnName} = '全体' THEN 'Entire'
                                WHEN {columnName} = '集团' THEN 'Group'
                                WHEN {columnName} = '公司' THEN 'Company'
                               

                                ELSE {columnName} END
                            WHERE {columnName} IN (
                                '个人', '所有', '部门', '全体', '集团', '公司');";

            try
            {
                ShareClass.RunSqlCommand(updateQuery);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(updateQuery + " error:" + ex.Message.ToString());
            }
        }
    }

    //更新带有Gender字段Table的Gender的值到英文核心
    public static void UpdateTableGender()
    {
        // 查询所有包含'status'列的表
        var strHQL = @"
              SELECT c.table_name, c.column_name
        FROM information_schema.columns c
        JOIN information_schema.tables t
          ON c.table_schema = t.table_schema
          AND c.table_name = t.table_name
        WHERE c.column_name LIKE '%gender%'
          AND c.table_schema = 'public'
          AND t.table_type = 'BASE TABLE'
          AND c.table_name NOT IN ('pbcatvld', 'pbcatedt') AND C.column_name not like '%id%';";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Table");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string tableName = ds.Tables[0].Rows[i][0].ToString();
            string columnName = ds.Tables[0].Rows[i][1].ToString();

            // 构建并执行更新语句
            string updateQuery = $@"
                            UPDATE {tableName} 
                            SET {columnName} = CASE 
                                WHEN {columnName} = '男' THEN 'Male'
                                WHEN {columnName} = '女' THEN 'Female'

                                ELSE {columnName} END
                            WHERE {columnName} IN (
                                '男', '女');";

            try
            {
                ShareClass.RunSqlCommand(updateQuery);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(updateQuery + " error:" + ex.Message.ToString());
            }
        }
    }

    //更新带有Authority字段Table的Authority的值到英文核心
    public static void UpdateTableAuthority()
    {
        // 查询所有包含'status'列的表
        var strHQL = @"
              SELECT c.table_name, c.column_name
        FROM information_schema.columns c
        JOIN information_schema.tables t
          ON c.table_schema = t.table_schema
          AND c.table_name = t.table_name
        WHERE c.column_name LIKE '%authority%'
          AND c.table_schema = 'public'
          AND t.table_type = 'BASE TABLE'
          AND c.table_name NOT IN ('pbcatvld', 'pbcatedt') AND C.column_name not like '%id%';";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Table");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string tableName = ds.Tables[0].Rows[i][0].ToString();
            string columnName = ds.Tables[0].Rows[i][1].ToString();

            // 构建并执行更新语句
            string updateQuery = $@"
                            UPDATE {tableName} 
                            SET {columnName} = CASE 
                                WHEN {columnName} = '所有' THEN 'All'
                                WHEN {columnName} = '部分' THEN 'Part'

                                ELSE {columnName} END
                            WHERE {columnName} IN (
                                '所有', '部分');";

            try
            {
                ShareClass.RunSqlCommand(updateQuery);
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(updateQuery + " error:" + ex.Message.ToString());
            }
        }
    }

    //执行模组英文化的语句
    public static void ExcuteUpdateProModuleNameToEnglish()
    {
        // 读取SQL语句文件
        string sqlFilePath = HttpContext.Current.Server.MapPath("UpdateCode/UpdateModule.txt");
        string sqlContent = File.ReadAllText(sqlFilePath);

        // 按分号分割SQL语句
        string[] sqlStatements = sqlContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);


        foreach (var sql in sqlStatements)
        {
            // 去除语句前后的空白字符
            string trimmedSql = sql.Trim();

            // 如果语句不为空，则执行
            if (!string.IsNullOrEmpty(trimmedSql))
            {
                try
                {
                    ShareClass.RunSqlCommand(sql);
                }
                catch (Exception ex)
                {
                    LogClass.WriteLogFile(sql + " " + ex.Message.ToString());
                }
            }

        }

    }

    //执行模组流程定义英文化的语句
    public static void UpdateModulesDefinition()
    {

        // 获取所有ModuleDefinition不为空的T_ProModule记录
        var getTProModuleCmd = "SELECT Id, ModuleDefinition FROM T_ProModule WHERE ModuleDefinition IS NOT NULL AND ModuleDefinition <> ''";
        DataSet ds = ShareClass.GetDataSetFromSql(getTProModuleCmd, "T_ProModule");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            int id = int.Parse(ds.Tables[0].Rows[i][0].ToString());
            string moduleDefinition = ds.Tables[0].Rows[i][1].ToString();

            // 提取ModuleDefinition中的所有中文词语
            var chineseWords = ExtractChineseWords(moduleDefinition);

            foreach (string chineseWord in chineseWords)
            {
                // 根据HomeModuleName查找对应的ModuleName
                var findModuleNameCmd = string.Format(@"SELECT ModuleName FROM T_ProModuleLevel WHERE HomeModuleName = '{0}' AND LangCode = 'zh-CN';", chineseWord);
                DataSet ds2 = ShareClass.GetDataSetFromSql(findModuleNameCmd, "T_ProModuleLevel");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    string moduleName = ds.Tables[0].Rows[0][0].ToString();
                    moduleDefinition = ReplaceFirstOccurrence(moduleDefinition, chineseWord, moduleName);
                }
            }

            // 更新T_ProModule表中的ModuleDefinition字段
            var updateCmd = string.Format(@"From ProModule as proModule WHERE proModule.ID = {0};", id);
            ProModuleBLL proModuleBLL = new ProModuleBLL();
            IList lst = proModuleBLL.GetAllProModules(updateCmd);
            ProModule proModule = (ProModule)lst[0];

            proModule.ModuleDefinition = moduleDefinition;

            proModuleBLL.UpdateProModule(proModule, id);
        }

    }

    // 从文本中提取所有中文词语
    private static List<string> ExtractChineseWords(string text)
    {
        var words = new List<string>();
        // 使用正则表达式匹配中文字符
        var matches = Regex.Matches(text, @"[\u4e00-\u9fff]+");
        foreach (Match match in matches)
        {
            words.Add(match.Value);
        }
        return words;
    }

    // 替换字符串中的第一个出现的指定子串
    private static string ReplaceFirstOccurrence(string original, string oldText, string newText)
    {
        int index = original.IndexOf(oldText);
        if (index < 0)
            return original;
        return original.Substring(0, index) + newText + original.Substring(index + oldText.Length);
    }
}