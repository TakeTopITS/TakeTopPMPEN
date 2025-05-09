using System;
using System.Net;
using System.Web.UI;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Threading;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web;
using System.ServiceModel.Channels;
using System.Activities.Statements;
using Npgsql;
using mshtml;

public partial class TakeTopSystemOtherCodeRunPage : System.Web.UI.Page
{
    public string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        if (Page.IsPostBack == false)
        {
            Session["SystemName"] = System.Configuration.ConfigurationManager.AppSettings["SystemName"];

            AsyncWork();
        }
    }

    protected void AsyncWork()
    {
        if (ShareClass.SystemLatestLoginUser == "")
        {
            ShareClass.SystemLatestLoginUser = "Timer";

            // ִ���������
            RunSpecialCode(strUserCode, strUserName);

            //����¼�û�
            ShareClass.SystemLatestLoginUser = "";
        }

        //���Ӻ������ͼ���û�
        AddHChartToUser(strUserCode);
    }

    //ִ���������
    public static void RunSpecialCode(string strUserCode, string strUserName)
    {
        int intUserNumber, intRunMarkInDB;

        intUserNumber = getUserNumber();
        intRunMarkInDB = GetNormalOtherCodeRunMark();

        //�������ֵ�����Ծ����Ƿ�ִ������Ĵ���
        int intRunMark = 3;

        if (intRunMarkInDB < intRunMark)
        {
            //����Ԥ����������
            AddEarlyWarningOrder("�������ȱ��"); 

            //����ϵͳ����ͼ����
            AddSystemAnalystChart("��ִ����Ŀ״̬"); 
            AddSystemAnalystChart("��Ŀ��Ȼؿ�״̬"); 
            AddSystemAnalystChart("������Ŀ״̬"); 
            AddSystemAnalystChart("�����Ŀ��ʱ״̬"); 
            AddSystemAnalystChart("��ִ������״̬"); 


            //����ϵͳ����ͼSqlCode����
            UpdateSystemAnalystChart("��ִ����Ŀ״̬");
            UpdateSystemAnalystChart("��Ŀ��Ȼؿ�״̬");
            UpdateSystemAnalystChart("������Ŀ״̬");
            UpdateSystemAnalystChart("�����Ŀ��ʱ״̬");
            UpdateSystemAnalystChart("��ִ������״̬");


            //����Ԥ������
            UpdateEaryWarningOrder("�����������"); 
            UpdateEaryWarningOrder("�����������"); 
            UpdateEaryWarningOrder("Ҫ�����Ͷ��"); 
            UpdateEaryWarningOrder("Ҫ��˵�����"); 
            UpdateEaryWarningOrder("���ڵ���Ŀ�ƻ�"); 
            UpdateEaryWarningOrder("��ͬԤ��Ԥ��"); 
            UpdateEaryWarningOrder("Ҫ�μӵĻ���"); 
            UpdateEaryWarningOrder("δд���ܼƻ�"); 
            UpdateEaryWarningOrder("Ҫ��˵�����"); 

            //��ʼ��ģ�����������·�߶���
            UpdateModuleFlowDefinition();

            //���û�����ϵͳ����ͼ
            InitialSystemAnalystChart(strUserCode, "ADMIN");
            //���ӷ���ͼ���û�
            AddChartToUserFromADMIN(strUserCode);

            //���û�����ϵͳ����ͼ
            UpdateSystemAnalystChartForUser();

            //�������ݿ�ֻ���û���ֻ�����룬һ���ڱ��������
            SetDBUserIDPasswordForDBOnlyReadUser();

            //�����������б��
            SetNormalOtherCodeMark(intRunMark);

      


            ////�ж�����ϵͳ�Ƿ��Ѿ���ʹ�ã���ʽʹ������ִ���������
            //if (intUserNumber > 2)
            //{
            //    //����ʵʩ�׶εĻ�������ɾ������
            //    UpdateIsCanClearBaseData(strUserCode, strUserName);
            //}
        }
    }

    //ȡ��ͨ����������б��
    public static int GetNormalOtherCodeRunMark()
    {
        string strHQL;
        int intMark = 0;
        strHQL = "Select NormalCodeRunMark From T_OtherCodeRunMark";
        DataSet dataSet = ShareClass.GetDataSetFromSql(strHQL, "T_OtherCodeRunMark");
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            intMark = Convert.ToInt32(dataSet.Tables[0].Rows[0]["NormalCodeRunMark"].ToString());
        }
        else
        {
            intMark = 0;
        }

        return intMark;
    }

    //�����������б��
    public static void SetNormalOtherCodeMark(int intMark)
    {
        string strHQL;
        strHQL = "Update T_OtherCodeRunMark Set NormalCodeRunMark = " + intMark;
        ShareClass.RunSqlCommand(strHQL);
    }

    //����ϵͳ����ͼ����
    protected static void AddSystemAnalystChart(string strChartName)
    {
        string strHQL;
        string strChartType, strSqlCode;

        strHQL = "From SystemAnalystChartManagement as systemAnalystChartManagement Where systemAnalystChartManagement.ChartName = '" + strChartName + "'";
        SystemAnalystChartManagementBLL systemAnalystChartManagementBLL = new SystemAnalystChartManagementBLL();
        SystemAnalystChartManagement systemAnalystChartManagement = new SystemAnalystChartManagement();
        IList lst = systemAnalystChartManagementBLL.GetAllSystemAnalystChartManagements(strHQL);
        if (lst.Count > 0)
        {
            return;
        }

        if (strChartName == "��ִ����Ŀ״̬") 
        {
            strChartType = "HRuningProjectStatus";

            strSqlCode = @"WITH ProjectData AS (
    SELECT 
        Status,
        EXTRACT(YEAR FROM begindate) AS BeginYear
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
      AND Status IN ('InProgress', 'Acceptance', 'CaseClosed')
)
SELECT 
    COUNT(*) AS XName,
    (SUM(CASE WHEN Status = 'InProgress' AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE) THEN 1 ELSE 0 END) || ',' ||
     SUM(CASE WHEN Status IN ('Acceptance', 'CaseClosed') AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE) THEN 1 ELSE 0 END)) AS YNumber
FROM ProjectData
WHERE Status = 'InProgress';";

            systemAnalystChartManagement.ChartType = strChartType;
            systemAnalystChartManagement.ChartName = strChartName;
            systemAnalystChartManagement.SqlCode = strSqlCode;
            systemAnalystChartManagement.LinkURL = "";

            systemAnalystChartManagement.Status = "YES";

            try
            {
                systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strChartName == "��Ŀ��Ȼؿ�״̬") 
        {
            strChartType = "HAnnualPaymentStatus";

            strSqlCode = @"WITH ProjectIDs AS (
    SELECT ProjectID
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
),
CurrentYear AS (
    SELECT EXTRACT(YEAR FROM CURRENT_DATE) AS CurrentYear
)
SELECT 
    A.XName AS XName,
    (B.YNumber || ',' || C.ZNumber) AS YNumber
FROM (
    SELECT COALESCE(SUM(receiveraccount), 0) AS XName
    FROM public.t_constractreceivables
    WHERE RelatedType = 'Project'
      AND RelatedID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM receivertime) = (SELECT CurrentYear FROM CurrentYear)
) AS A,
(
    SELECT COALESCE(SUM(RealCharge), 0) AS YNumber
    FROM v_procurrentyearrealcharge
    WHERE ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM effectdate) = (SELECT CurrentYear FROM CurrentYear)
) AS B,
(
    SELECT COUNT(*) AS ZNumber
    FROM V_ProRealCharge A
    JOIN T_Project B ON A.ProjectID = B.ProjectID
    WHERE A.ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND A.RealCharge > B.Budget
) AS C;";

            systemAnalystChartManagement.ChartType = strChartType;
            systemAnalystChartManagement.ChartName = strChartName;
            systemAnalystChartManagement.SqlCode = strSqlCode;
            systemAnalystChartManagement.LinkURL = "";

            systemAnalystChartManagement.Status = "YES";

            try
            {
                systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strChartName == "������Ŀ״̬") 
        {
            strChartType = "HDelayProjectStatus";

            strSqlCode = @"WITH ProjectData AS (
    SELECT
        FinishPercent,
        EndDate
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
)
SELECT
    (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent < 100 AND now() > (EndDate + interval '30 days')) AS XName,
    (
        (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent = 100 AND now() > EndDate) || ',' ||
        (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent < 100 AND now() < (EndDate + interval '10 days') AND now() > EndDate)
    ) AS YNumber;";

            systemAnalystChartManagement.ChartType = strChartType;
            systemAnalystChartManagement.ChartName = strChartName;
            systemAnalystChartManagement.SqlCode = strSqlCode;
            systemAnalystChartManagement.LinkURL = "";

            systemAnalystChartManagement.Status = "YES";

            try
            {
                systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strChartName == "�����Ŀ��ʱ״̬") 
        {
            strChartType = "HAnnualWorkHourStatus";

            strSqlCode = @"WITH ProjectIDs AS (
    SELECT ProjectID
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
),
FilteredDailyWork AS (
    SELECT
        ManHour,
        UserCode,
        Confirmbonus
    FROM public.T_DailyWork
    WHERE ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
)
SELECT
    COALESCE(SUM(ManHour), 0) AS XName,
    (COUNT(DISTINCT UserCode) || ',' || COALESCE(SUM(Confirmbonus), 0)) AS YNumber
FROM FilteredDailyWork;";

            systemAnalystChartManagement.ChartType = strChartType;
            systemAnalystChartManagement.ChartName = strChartName;
            systemAnalystChartManagement.SqlCode = strSqlCode;
            systemAnalystChartManagement.LinkURL = "";

            systemAnalystChartManagement.Status = "YES";

            try
            {
                systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strChartName == "��ִ������״̬") 
        {
            strChartType = "HRuningTaskStatus";

            strSqlCode = @"WITH TaskData AS (
    SELECT
        Status,
        finishpercent,
        EXTRACT(YEAR FROM begindate) AS BeginYear
    FROM T_ProjectTask
    WHERE makemancode = '[TAKETOPUSERCODE]'
      AND (
          (finishpercent < 100 AND Status = 'InProgress') OR
          (finishpercent = 100 AND Status IN ('Completed', 'Closed'))
      )
)
SELECT
    (SELECT COUNT(*) FROM TaskData WHERE finishpercent < 100 AND Status = 'InProgress') AS XName,
    (
        (SELECT COUNT(*) FROM TaskData WHERE finishpercent < 100 AND Status = 'InProgress' AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE)) || ',' ||
        (SELECT COUNT(*) FROM TaskData WHERE finishpercent = 100 AND Status IN ('Completed', 'Closed') AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE))
    ) AS YNumber;";

            systemAnalystChartManagement.ChartType = strChartType;
            systemAnalystChartManagement.ChartName = strChartName;
            systemAnalystChartManagement.SqlCode = strSqlCode;
            systemAnalystChartManagement.LinkURL = "";

            systemAnalystChartManagement.Status = "YES";

            try
            {
                systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    //����ϵͳ����ͼSqlCode����
    public static void UpdateSystemAnalystChart(string strChartName)
    {
        string strHQL;
        int intID = 0;

        strHQL = "From SystemAnalystChartManagement as systemAnalystChartManagement Where systemAnalystChartManagement.ChartName = '" + strChartName + "'";
        SystemAnalystChartManagementBLL systemAnalystChartManagementBLL = new SystemAnalystChartManagementBLL();
        IList lst = systemAnalystChartManagementBLL.GetAllSystemAnalystChartManagements(strHQL);
        if (lst.Count > 0)
        {
            SystemAnalystChartManagement systemAnalystChartManagement = (SystemAnalystChartManagement)lst[0];

            intID = systemAnalystChartManagement.ID;

            if (strChartName == "��ִ����Ŀ״̬") 
            {
                systemAnalystChartManagement.SqlCode = @"WITH ProjectData AS (
    SELECT 
        Status,
        EXTRACT(YEAR FROM begindate) AS BeginYear
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
      AND Status IN ('InProgress', 'Acceptance', 'CaseClosed')
)
SELECT 
    COUNT(*) AS XName,
    (SUM(CASE WHEN Status = 'InProgress' AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE) THEN 1 ELSE 0 END) || ',' ||
     SUM(CASE WHEN Status IN ('Acceptance', 'CaseClosed') AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE) THEN 1 ELSE 0 END)) AS YNumber
FROM ProjectData
WHERE Status = 'InProgress';";


                try
                {
                    systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, intID);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }

            if (strChartName == "������Ŀ״̬")
            {
                systemAnalystChartManagement.SqlCode = @"WITH ProjectData AS (
    SELECT
        FinishPercent,
        EndDate
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
)
SELECT
    (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent < 100 AND now() > (EndDate + interval '30 days')) AS XName,
    (
        (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent = 100 AND now() > EndDate) || ',' ||
        (SELECT COUNT(*) FROM ProjectData WHERE FinishPercent < 100 AND now() < (EndDate + interval '10 days') AND now() > EndDate)
    ) AS YNumber;";


                try
                {
                    systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, intID);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }

            if (strChartName == "�����Ŀ��ʱ״̬")
            {
                systemAnalystChartManagement.SqlCode = @"WITH ProjectIDs AS (
    SELECT ProjectID
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
),
FilteredDailyWork AS (
    SELECT
        ManHour,
        UserCode,
        Confirmbonus
    FROM public.T_DailyWork
    WHERE ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
)
SELECT
    COALESCE(SUM(ManHour), 0) AS XName,
    (COUNT(DISTINCT UserCode) || ',' || COALESCE(SUM(Confirmbonus), 0)) AS YNumber
FROM FilteredDailyWork;";


                try
                {
                    systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, intID);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }

            if (strChartName == "��Ŀ��Ȼؿ�״̬")
            {
                systemAnalystChartManagement.SqlCode = @"WITH ProjectIDs AS (
    SELECT ProjectID
    FROM T_Project
    WHERE PMCode = '[TAKETOPUSERCODE]'
),
CurrentYear AS (
    SELECT EXTRACT(YEAR FROM CURRENT_DATE) AS CurrentYear
)
SELECT 
    A.XName AS XName,
    (B.YNumber || ',' || C.ZNumber) AS YNumber
FROM (
    SELECT COALESCE(SUM(receiveraccount), 0) AS XName
    FROM public.t_constractreceivables
    WHERE RelatedType = 'Project'
      AND RelatedID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM receivertime) = (SELECT CurrentYear FROM CurrentYear)
) AS A,
(
    SELECT COALESCE(SUM(RealCharge), 0) AS YNumber
    FROM v_procurrentyearrealcharge
    WHERE ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND EXTRACT(YEAR FROM effectdate) = (SELECT CurrentYear FROM CurrentYear)
) AS B,
(
    SELECT COUNT(*) AS ZNumber
    FROM V_ProRealCharge A
    JOIN T_Project B ON A.ProjectID = B.ProjectID
    WHERE A.ProjectID IN (SELECT ProjectID FROM ProjectIDs)
      AND A.RealCharge > B.Budget
) AS C;";


                try
                {
                    systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, intID);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }

            if (strChartName == "��ִ������״̬")
            {
                systemAnalystChartManagement.SqlCode = @"WITH TaskData AS (
    SELECT
        Status,
        finishpercent,
        EXTRACT(YEAR FROM begindate) AS BeginYear
    FROM T_ProjectTask
    WHERE makemancode = '[TAKETOPUSERCODE]'
      AND (
          (finishpercent < 100 AND Status = 'InProgress') OR
          (finishpercent = 100 AND Status IN ('Completed', 'Closed'))
      )
)
SELECT
    (SELECT COUNT(*) FROM TaskData WHERE finishpercent < 100 AND Status = 'InProgress') AS XName,
    (
        (SELECT COUNT(*) FROM TaskData WHERE finishpercent < 100 AND Status = 'InProgress' AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE)) || ',' ||
        (SELECT COUNT(*) FROM TaskData WHERE finishpercent = 100 AND Status IN ('Completed', 'Closed') AND BeginYear = EXTRACT(YEAR FROM CURRENT_DATE))
    ) AS YNumber;";


                try
                {
                    systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, intID);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }
        }
    }

    //���ӷ���ͼ���û�
    public static void InitialSystemAnalystChart(string strToUserCode, string strFromUserCode)
    {
        string strHQL;

        try
        {
            strHQL = string.Format(@"Insert Into T_SystemAnalystChartRelatedUser(UserCode,ChartName,FormType,SortNumber)
                 Select '{0}',ChartName,FormType,SortNumber From T_SystemAnalystChartRelatedUser
                 Where UserCode = '{1}' 
	             and ChartName Not in (Select ChartName From T_SystemAnalystChartRelatedUser Where UserCode = '{0}')", strToUserCode, strFromUserCode);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = @"Delete From T_SystemAnalystChartRelatedUser Where ID Not In 
                  (Select Max(ID) From T_SystemAnalystChartRelatedUser Group By UserCode, ChartName, FormType)";
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //���Ӻ������ͼ���û�
    public static void AddHChartToUser(string strUserCode)
    {
        string strHQL;

        try
        {
            strHQL = string.Format(@"Insert Into t_systemanalystchartrelateduser(UserCode,ChartName,FormType,SortNumber) Select '{0}',ChartName,'PersonalSpacePage',1
                    From t_systemanalystchartmanagement Where ChartType Like 'H%'
                    and ChartName Not In (Select ChartName From t_systemanalystchartrelateduser Where UserCode = '{0}')", strUserCode);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update public.t_systemanalystchartrelateduser Set SortNumber = 1 Where ChartName = '��ִ����Ŀ״̬';
                    Update public.t_systemanalystchartrelateduser Set SortNumber = 2 Where ChartName = '������Ŀ״̬';
                    Update public.t_systemanalystchartrelateduser Set SortNumber = 3 Where ChartName = '�����Ŀ��ʱ״̬';
                    Update public.t_systemanalystchartrelateduser Set SortNumber = 4 Where ChartName = '��Ŀ��Ȼؿ�״̬';
                    Update public.t_systemanalystchartrelateduser Set SortNumber = 5 Where ChartName = '��ִ������״̬';
                    Update public.t_systemanalystchartrelateduser Set SortNumber = 10 Where ChartName Not In ('��ִ����Ŀ״̬','������Ŀ״̬','�����Ŀ��ʱ״̬','��Ŀ��Ȼؿ�״̬','��ִ������״̬');
                    ");
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }


    //Copy ����ԱADMIN�û��ķ���ͼ�������û�
    public static void AddChartToUserFromADMIN(string strUserCode)
    {
        string strHQL;

        if (GetUserChartNumber(strUserCode) == 0)
        {
            strHQL = string.Format(@"Insert Into t_systemanalystchartrelateduser(UserCode,ChartName,FormType,SortNumber) 
              Select '{0}',ChartName,FormType,SortNumber From t_systemanalystchartrelateduser 
                Where UserCode = 'ADMIN'", strUserCode);

            ShareClass.RunSqlCommand(strHQL);
        }
    }


    //����Ԥ������
    public static void AddEarlyWarningOrder(string strFunName)
    {
        string strHQL, strUpdateHQL;
        IList lst;

        strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
        FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
        lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
        if (lst.Count > 0)
        {
            return;
        }

        if (strFunName == "�������ȱ��") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"select * from T_DefectAssignRecord as defectAssignRecordBySystem where defectAssignRecordBySystem.OperatorCode = '[TAKETOPUSERCODE]' 
                    and defectAssignRecordBySystem.Status in ('Plan','Accepted','ToHandle') and defectAssignRecordBySystem.ID not in (select defectAssignRecord.PriorID 
                    from T_DefectAssignRecord as defectAssignRecord) and defectAssignRecordBySystem.DefectID in 
                    (select defectment.DefectID from T_Defectment as defectment where defectment.Status not in ('Closed','Hided','Deleted','Archived'))
                    Order by defectAssignRecordBySystem.ID DESC";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "TTDefectHandlePage.aspx";
                funInforDialBox.MobileLinkAddress = "TTDefectHandlePage.aspx";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��ִ����Ŀ״̬") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
    SELECT count(*) AS XName  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  
      AND Status IN ('InProgress')
) AS A,
(
    SELECT count(*) AS YNumber  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  
      AND Status IN ('InProgress') and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS B,
(
    SELECT count(*) AS ZNumber  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
      AND Status IN ('Acceptance','CaseClosed')
) AS C;";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "";
                funInforDialBox.MobileLinkAddress = "";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��Ŀ��Ȼؿ�״̬") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT CoalEsce(Sum(receiveraccount),0) as XName FROM public.t_constractreceivables  where RelatedType = 'Project' and RelatedID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM receivertime) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS A,
(
    Select CoalEsce(Sum(RealCharge),0) As YNumber from v_procurrentyearrealcharge  where ProjectID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]') 
	 and EXTRACT(YEAR FROM effectdate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS B,
(
 Select count(*) as ZNumber from V_ProRealCharge A,T_Project B where A.ProjectID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]') and A.RealCharge > B.Budget
) AS C;";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "";
                funInforDialBox.MobileLinkAddress = "";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "������Ŀ״̬") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT count(*) AS XName  
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent <100
  AND now() > (EndDate + interval '30 days')
) AS A,
(
   
   SELECT count(*) AS YNumber
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent = 100
  AND now() > EndDate
) AS B,
(

   SELECT count(*) AS ZNumber
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent <100
  AND now() > (EndDate + interval '10 days')
) AS C;";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "";
                funInforDialBox.MobileLinkAddress = "";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }


        if (strFunName == "�����Ŀ��ʱ״̬") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT CoalEsce(Sum(ManHour),0) as XName FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS A,
(
   SELECT Count(Distinct UserCode) as YNumber FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
     
) AS B,
(
  SELECT CoalEsce(Sum(Confirmbonus),0) as ZNumber FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)

) AS C;";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "";
                funInforDialBox.MobileLinkAddress = "";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��ִ������״̬") 
        {
            try
            {

                FunInforDialBox funInforDialBox = new FunInforDialBox();

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
    SELECT count(*) AS XName  
    FROM T_ProjectTask 
    WHERE makemancode = '[TAKETOPUSERCODE]'  
      AND finishpercent <100 and Status IN ('InProgress')
) AS A,
(
    SELECT count(*) AS YNumber  
    FROM T_ProjectTask 
    WHERE makemancode = '[TAKETOPUSERCODE]'  
      AND finishpercent <100 and Status IN ('InProgress') and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS B,
(
    SELECT count(*) AS ZNumber  
    FROM T_ProjectTask 
    WHERE makemancode = '[TAKETOPUSERCODE]'  
	and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
      AND finishpercent = 100 AND Status IN ('Completed','Closed')
) AS C;";

                funInforDialBox.Status = "����"; 
                funInforDialBox.SQLCode = strUpdateHQL;
                funInforDialBox.InforName = strFunName;
                funInforDialBox.HomeName = strFunName;
                funInforDialBox.LangCode = HttpContext.Current.Session["LangCode"].ToString();
                funInforDialBox.CreateTime = DateTime.Now;
                funInforDialBox.BoxType = "SYS";
                funInforDialBox.UserType = "INNER";
                funInforDialBox.IsForceInfor = "NO";
                funInforDialBox.LinkAddress = "";
                funInforDialBox.MobileLinkAddress = "";
                funInforDialBox.IsSendMsg = "YES";
                funInforDialBox.IsSendEmail = "YES";

                funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }


    //����Ԥ������
    public static void UpdateEaryWarningOrder(string strFunName)
    {
        string strHQL, strUpdateHQL;
        IList lst;

        int intID;

        if (strFunName == "�����������") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"select * from T_TaskAssignRecord as taskAssignRecordBySystem where taskAssignRecordBySystem.OperatorCode = '[TAKETOPUSERCODE]' 
                          and taskAssignRecordBySystem.Status in ('Plan','Accepted','InProgress','ToHandle','InProgress') and taskAssignRecordBySystem.ID not in 
                          (select taskAssignRecord.PriorID from T_TaskAssignRecord as taskAssignRecord) 
                          and taskAssignRecordBySystem.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask 
                          where projectTask.Status <> 'Closed') and taskAssignRecordBySystem.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask 
                          where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from T_Project as project
                          where project.Status not in ('New','Hided','Deleted','Archived')))) Order by taskAssignRecordBySystem.ID DESC";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "�����������") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"select * from T_ReqAssignRecord as reqAssignRecordBySystem where reqAssignRecordBySystem.OperatorCode = '[TAKETOPUSERCODE]' 
                    and reqAssignRecordBySystem.Status in ('Plan','Accepted','ToHandle') and reqAssignRecordBySystem.ID not in (select reqAssignRecord.PriorID 
                    from T_ReqAssignRecord as reqAssignRecord) and reqAssignRecordBySystem.ReqID in 
                    (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))
                    Order by reqAssignRecordBySystem.ID DESC";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "Ҫ��˵�����") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;


                strUpdateHQL = string.Format(@"Select * From (Select A.ID,A.StepID,A.WorkDetail,B.CreatorCode,B.CreatorName,A.Requisite,A.Operation,A.CheckingTime,A.WLID,Rtrim(cast(A.WLID as char(20))) || '. ' || B.WLName as WLName,B.Status From T_WorkFlowStepDetail A,T_WorkFlow B 
                 Where A.WLID = B.WLID And A.Status In ('InProgress','Reviewing','Signing','ReReview') 
                 And B.Status Not In ('Updating','Closed','Passed','CaseClosed') And (trim(A.OperatorCode) = '{0}' Or A.OperatorCode in ( Select UserCode From T_MemberLevel Where UnderCode <> UserCode and UnderCode = '{0}' and AgencyStatus = 1))
																 And A.IsOperator = 'YES' ) C Order By C.StepID DESC", "[TAKETOPUSERCODE]");

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "Ҫ�����Ͷ��") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"Select *  From T_Tender_HYYQ Where  IsTender <> 0 and rtrim(TenderBuyTime) <= to_char(now()+(TenderBuyDay+1)* interval '1 day','yyyymmdd')  and (CreatorCode = '[TAKETOPUSERCODE]' or ID in (Select TenderID from T_TenderRelatedUser where UserCode = '[TAKETOPUSERCODE]')) and TenderCode like '%%'  and ProjectName like '%%'  
                        UNION
                        Select *  From T_Tender_HYYQ Where  IsMargin <> 0 and rtrim(MarginTime) <= to_char(now()+(MarginDay+1)* interval '1 day','yyyymmdd')  and (CreatorCode = '[TAKETOPUSERCODE]' or ID in (Select TenderID from T_TenderRelatedUser where UserCode = '[TAKETOPUSERCODE]')) and TenderCode like '%%'  and ProjectName like '%%'  
                        UNION
                        Select *  From T_Tender_HYYQ Where  IsReceiveMargin <> 0 and to_char(cast(ReceiveMarginTime as date),'yyyymmdd') <= to_char(now()+ReceiveMarginDay* interval '1 day','yyyymmdd')  and (CreatorCode = '[TAKETOPUSERCODE]' or ID in (Select TenderID from T_TenderRelatedUser where UserCode = '[TAKETOPUSERCODE]')) and TenderCode like '%%'  and ProjectName like '%%'  
                        UNION
                        Select *  From T_Tender_HYYQ Where  IsBidOpening <> 0 and rtrim(BidOpeningDate) <= to_char(now()+(BidOpeningDay+1)* interval '1 day','yyyymmdd')  and (CreatorCode = '[TAKETOPUSERCODE]' or ID in (Select TenderID from T_TenderRelatedUser where UserCode = '[TAKETOPUSERCODE]')) and TenderCode like '%%'  and ProjectName like '%%'  
                        UNION
                        Select *  From T_Tender_HYYQ Where  IsWinningFee <> 0 and rtrim(WinningFeeDate) <= to_char(now()+(WinningFeeDay+1)* interval '1 day','yyyymmdd')  and (CreatorCode = '[TAKETOPUSERCODE]' or ID in (Select TenderID from T_TenderRelatedUser where UserCode = '[TAKETOPUSERCODE]')) and TenderCode like '%%'  and ProjectName like '%%'";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "���ڵ���Ŀ�ƻ�") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"------------------------���˵���Ŀ�ƻ�------------------ 
                    select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,
	                    PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName  
	                    from V_ProjectPlanList
	                    where PMCode =  '[TAKETOPUSERCODE]'
	                    and Expireday > 1  --�����������ĳ�����Ҫ������ 
	
	                    and ParentIDGantt > 0
	                    and Percent_Done < 100
	                    and PlanID not In (Select ParentIDGantt From T_ImplePlan)
	
                     UNION
                     ------------------------���ܵ�ֱ�ӳ�Ա����Ŀ�ƻ�------------------ 
                     select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,
	                    PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName 
	                    from V_ProjectPlanList
	                    where PMCode in (Select UserCode From T_MemberLevel Where UserCode = '[TAKETOPUSERCODE]'  and ProjectVisible = 'YES' )  
                        and Expireday > 5   --�����������ĳ�����Ҫ������ 
	   
	                    and ParentIDGantt > 0
	                    and Percent_Done < 100
	                    and PlanID not In (Select ParentIDGantt From T_ImplePlan)
           
                     UNION
                     ------------------------���ܵ����г�Ա����Ŀ�ƻ�------------------ 
                    select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,
	                    PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName 
	                    from V_ProjectPlanList
	                    Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode in [TAKETOPSUPERDEPARTSTRING])
                        and Expireday > 5   --�����������ĳ�����Ҫ������ 
	   
	                    and ParentIDGantt > 0
	                    and Percent_Done < 100
	                    and PlanID not In (Select ParentIDGantt From T_ImplePlan)";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��ͬԤ��Ԥ��") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"select ID from T_ConstractReceivables as constractReceivablesBySystem 
                     where constractReceivablesBySystem.Status not in ('Completed','Cancel') 
                     and to_char(constractReceivablesBySystem.ReceivablesTime,'yyyymmdd') <=to_char(now()+interval '1 day','yyyymmdd') 
                     and constractReceivablesBySystem.ConstractCode in (Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = '[TAKETOPUSERCODE]') 
                     and constractReceivablesBySystem.ConstractCode not in (Select constract.ConstractCode from T_Constract as constract where constract.Status  in ('Archived','Cancel','Deleted')) 
                     union all select ID from T_ConstractPayable as constractPayableBySystem where constractPayableBySystem.Status not in ('Completed','Cancel') 
                     and to_char(constractPayableBySystem.PayableTime,'yyyymmdd') <= to_char(now()+PreDays*interval '1 day','yyyymmdd') 
                     and constractPayableBySystem.ConstractCode in (Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser 
                     where constractRelatedUser.UserCode= '[TAKETOPUSERCODE]') and constractPayableBySystem.ConstractCode 
                     not in (Select constract.ConstractCode from T_Constract as constract where constract.Status in ('Archived','Cancel','Deleted'))";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "Ҫ�μӵĻ���") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"select * from T_Meeting as meetingBySystem where meetingBySystem.ID in (select meetingAttendant.MeetingID from T_MeetingAttendant as meetingAttendant 
                  where meetingAttendant.UserCode = '[TAKETOPUSERCODE]') and meetingBySystem.EndTime > now() order by meetingBySystem.ID DESC";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "δд���ܼƻ�") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"Select * From T_ProjectMember  Where UserCode = '[TAKETOPUSERCODE]' and UserCode not in 
                       (Select CreatorCode From T_Plan Where to_char(StartTime,'yyyymmdd') >= to_char(now()-(extract(DOW FROM now())-2) * interval '1 day','yyyymmdd') 
                       and to_char(EndTime,'yyyymmdd')  <=  to_char(now()+(8-extract(DOW FROM now()))* interval '1 day','yyyymmdd'));";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��ִ����Ŀ״̬") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
    SELECT count(*) AS XName  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  
      AND Status IN ('InProgress')
) AS A,
(
    SELECT count(*) AS YNumber  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  
      AND Status IN ('InProgress') and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS B,
(
    SELECT count(*) AS ZNumber  
    FROM T_Project 
    WHERE PMCode = '[TAKETOPUSERCODE]'  and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
      AND Status IN ('Acceptance','CaseClosed')
) AS C;";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��Ŀ��Ȼؿ�״̬") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT CoalEsce(Sum(receiveraccount),0) as XName FROM public.t_constractreceivables  where RelatedType = 'Project' and RelatedID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM receivertime) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS A,
(
    Select CoalEsce(Sum(RealCharge),0) As YNumber from v_procurrentyearrealcharge  where ProjectID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]') 
	 and EXTRACT(YEAR FROM effectdate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS B,
(
 Select count(*) as ZNumber from V_ProRealCharge A,T_Project B where A.ProjectID 
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]') and A.RealCharge > B.Budget
) AS C";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "������Ŀ״̬") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT count(*) AS XName  
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent <100
  AND now() > (EndDate + interval '30 days')
) AS A,
(
   
   SELECT count(*) AS YNumber
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent = 100
  AND now() > EndDate
) AS B,
(

   SELECT count(*) AS ZNumber
FROM T_Project 
WHERE PMCode = '[TAKETOPUSERCODE]'  
  AND FinishPercent <100
  AND now() > (EndDate + interval '10 days')
) AS C;";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "�����Ŀ��ʱ״̬") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
FROM (
   SELECT CoalEsce(Sum(ManHour),0) as XName FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
) AS A,
(
   SELECT Count(Distinct UserCode) as YNumber FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)
     
) AS B,
(
  SELECT CoalEsce(Sum(Confirmbonus),0) as ZNumber FROM public.T_DailyWork where ProjectID
     In (Select ProjectID From T_Project Where PMCode = '[TAKETOPUSERCODE]')  and EXTRACT(YEAR FROM WorkDate) = EXTRACT(YEAR FROM CURRENT_DATE)

) AS C;";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        if (strFunName == "��ִ������״̬") 
        {
            try
            {
                strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + strFunName + "'";
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

                intID = funInforDialBox.ID;

                strUpdateHQL = @"SELECT A.XName as XName ,(B.YNumber ||','|| C.ZNumber) as YNumber
                                FROM (
                                    SELECT count(*) AS XName  
                                    FROM T_ProjectTask 
                                    WHERE makemancode = '[TAKETOPUSERCODE]'  
                                      AND finishpercent <100 and Status IN ('InProgress')
                                ) AS A,
                                (
                                    SELECT count(*) AS YNumber  
                                    FROM T_ProjectTask 
                                    WHERE makemancode = '[TAKETOPUSERCODE]'  
                                      AND finishpercent <100 and Status IN ('InProgress') and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
                                ) AS B,
                                (
                                    SELECT count(*) AS ZNumber  
                                    FROM T_ProjectTask 
                                    WHERE makemancode = '[TAKETOPUSERCODE]'  
	                                and EXTRACT(YEAR FROM begindate) = EXTRACT(YEAR FROM CURRENT_DATE)
                                      AND finishpercent = 100 AND Status IN ('Completed','Closed')
                                ) AS C;";

                if (funInforDialBox.SQLCode != strUpdateHQL)
                {
                    funInforDialBox.SQLCode = strUpdateHQL;

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }


    //�������ݿ�ֻ���û���ֻ�����룬һ���ڱ��������
    protected static void SetDBUserIDPasswordForDBOnlyReadUser()
    {
        string strHQL1, strHQL2;
        string strDBReadOnlyUserID, strDBReadOnlyUserPassword;

        strDBReadOnlyUserID = ShareClass.getDBReadOnlyUserID();
        strDBReadOnlyUserPassword = ShareClass.genernalPassword();

        try
        {
            strHQL1 = "Select Password From T_DBReadOnlyUserInfor";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_DBReadOnlyUserInfor");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL2 = string.Format("Insert Into T_DBReadOnlyUserInfor(DBUserID,Password) values('{0}','{1}')", strDBReadOnlyUserID, strDBReadOnlyUserPassword);
                ShareClass.RunSqlCommand(strHQL2);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
        }
    }

    //��ʼ��ģ�����������·�߶���
    public static void UpdateModuleFlowDefinition()
    {
        string strMFXML;
        string strSystemProductType;

        try
        {
            strSystemProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];

            //��ʼ��ʵʩ������ʵʩ·�߶���
            strMFXML = @"{states:{rect1:{type:'start',text:{text:'��ʼ'}, attr:{ x:141, y:12, width:50, height:50}, props:{guid:{value:'4af6bc4b-7ed9-0b0b-e3a0-91c9d8fd92d1'},text:{value:'��ʼ'}}},rect2:{type:'task',text:{text:'��������(����)'}, attr:{ x:267, y:62, width:100, height:50}, props:{guid:{value:'330ea6c7-33f5-e8e4-882a-c2e7b5763e84'},text:{value:'��������(����)',url:'TTBaseDataOuter.aspx'}}},rect3:{type:'task',text:{text:'��������(����)'}, attr:{ x:496, y:62, width:100, height:50}, props:{guid:{value:'39ecc8b3-039f-f13b-7b56-b380c8eb2d3d'},text:{value:'��������(����)',url:'TTBaseDataInner.aspx'}}},rect4:{type:'task',text:{text:'��֯�ܹ�����'}, attr:{ x:691, y:62, width:100, height:50}, props:{guid:{value:'0a6fd6c1-c2a6-b674-6a38-b407ac819f76'},text:{value:'��֯�ܹ�����',url:'TTDepartment.aspx'}}},rect5:{type:'task',text:{text:'Ա����������'}, attr:{ x:886, y:63, width:100, height:50}, props:{guid:{value:'fcf442a4-3711-d09c-ffee-b8a6fcdcdac9'},text:{value:'Ա����������',url:'TTUserInfor.aspx'}}},rect6:{type:'task',text:{text:'Ա�����ϵ���'}, attr:{ x:271, y:154, width:100, height:50}, props:{guid:{value:'85f9b02d-84c6-b9d9-9b22-4831788ae50e'},text:{value:'Ա�����ϵ���',url:'TTUserInforImport.aspx'}}},rect7:{type:'task',text:{text:'�û�Ȩ�޹���'}, attr:{ x:270, y:250, width:100, height:50}, props:{guid:{value:'4ca043ad-3c84-c0af-4e7b-0c749ccb1c3d'},text:{value:'�û�Ȩ�޹���',url:'TTProModuleAuthority.aspx'}}},rect8:{type:'task',text:{text:'ϵͳ�û�����'}, attr:{ x:494, y:253, width:100, height:50}, props:{guid:{value:'01517ae8-e172-149d-359e-5ff6afc87603'},text:{value:'ϵͳ�û�����',url:'TTSystemActiveUserSet.aspx'}}},rect9:{type:'task',text:{text:'ֱ�ӳ�Ա�������'}, attr:{ x:692, y:253, width:100, height:50}, props:{guid:{value:'18faee71-5273-0cbd-7bd3-1fc624e2253f'},text:{value:'ֱ�ӳ�Ա�������',url:'TTMemberLevelSet.aspx'}}},rect10:{type:'task',text:{text:'����ģ������'}, attr:{ x:887, y:255, width:100, height:50}, props:{guid:{value:'b84f2f71-fd7b-5c3f-78aa-8e79021683ba'},text:{value:'����ģ������',url:'TTWorkFlowTemplate.aspx'}}},rect11:{type:'task',text:{text:'�ĵ���������'}, attr:{ x:272, y:332, width:100, height:50}, props:{guid:{value:'7291f992-1ad8-b3b6-1eff-be100785b975'},text:{value:'�ĵ���������',url:'TTDocumentTypeSet.aspx'}}},rect12:{type:'task',text:{text:'��ɫ������'}, attr:{ x:272, y:456, width:100, height:50}, props:{guid:{value:'6c73074a-58f7-f5c7-62ce-a5aba9142648'},text:{value:'��ɫ������',url:'TTActorGroup.aspx'}}},rect13:{type:'task',text:{text:'������������'}, attr:{ x:491, y:457, width:100, height:50}, props:{guid:{value:'f0d6623c-39ab-fdca-cf64-3c507371a429'},text:{value:'������������',url:'TTPersonalSpaceNewsTypeEdit.aspx'}}},rect14:{type:'end',text:{text:'����(End)'}, attr:{ x:698, y:456, width:50, height:50}, props:{guid:{value:'718652a3-5724-2781-786b-abe937bd574b'},text:{value:'����(End)'}}}},paths:{path15:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ����(Step)'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path16:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ����(Step)'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path17:{from:'rect3',to:'rect4', dots:[],text:{text:'TO ��֯�ܹ�����'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path18:{from:'rect4',to:'rect5', dots:[],text:{text:'TO Ա�����ϵ���'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path19:{from:'rect7',to:'rect8', dots:[],text:{text:'TO ϵͳ�û�����'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path20:{from:'rect8',to:'rect9', dots:[],text:{text:'TO ֱ�ӳ�Ա�������'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path21:{from:'rect12',to:'rect13', dots:[],text:{text:'TO ������������'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path22:{from:'rect9',to:'rect10', dots:[],text:{text:'TO ����ģ������'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path23:{from:'rect13',to:'rect14', dots:[],text:{text:'TO ����(End)'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path24:{from:'rect5',to:'rect6', dots:[{x:935,y:179}],text:{text:'TO Ա����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO Ա����������'}}},path25:{from:'rect6',to:'rect7', dots:[],text:{text:'TO �û�Ȩ�޹���'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path26:{from:'rect10',to:'rect11', dots:[{x:937,y:357}],text:{text:'TO �ĵ���������'},textPos:{x:-12,y:-10}, props:{text:{value:'TO �ĵ���������'}}},path27:{from:'rect11',to:'rect12', dots:[],text:{text:'TO ��ɫ������'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
            SaveModuleFlowDefinition("ʵʩ����", strMFXML, 1, "INNER"); 

            if ("ECMP,DEMO".IndexOf(strSystemProductType) > -1)
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:313, y:8, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'����������'}, attr:{ x:541, y:128, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect5:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:313, y:243, width:100, height:50}, props:{guid:{value:'54404d63-2600-e15e-81e4-d73382f0c4be'},text:{value:'��Ŀ�������',url:'TTProjectReqManageMain.aspx'}}},rect6:{type:'task',text:{text:'��Ӧ������'}, attr:{ x:733, y:130, width:100, height:50}, props:{guid:{value:'1c96be8a-74f3-4245-8410-1e644295853b'},text:{value:'��Ӧ������',url:'TTGoodsManage.aspx'}}},rect7:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1142, y:132, width:100, height:50}, props:{guid:{value:'5d7924f1-d30f-8e77-f9c2-bbff046fc474'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect8:{type:'task',text:{text:'�������'}, attr:{ x:940, y:131, width:100, height:50}, props:{guid:{value:'1d88fb65-7ead-3211-578a-ca3f51f6d5c4'},text:{value:'�������',url:'TTReceivablesPayableAlert.aspx'}}},rect9:{type:'task',text:{text:'��ⵥ'}, attr:{ x:733, y:7, width:100, height:50}, props:{guid:{value:'7f67e5bf-6b78-d79c-9ef3-b2858f7293b0'},text:{value:'��ⵥ',url:'TTMakeGoods.aspx'}}},rect10:{type:'task',text:{text:'���ⵥ'}, attr:{ x:733, y:244, width:100, height:50}, props:{guid:{value:'ca55c30d-c204-9183-baea-4629fb52ef33'},text:{value:'���ⵥ',url:'TTGoodsShipmentOrder.aspx'}}},rect11:{type:'task',text:{text:'�տ�'}, attr:{ x:940, y:6, width:100, height:50}, props:{guid:{value:'bb49d768-afb5-6148-bad3-a10a1f4e47db'},text:{value:'�տ�',url:'TTAccountReceivablesRecord.aspx'}}},rect12:{type:'task',text:{text:'����'}, attr:{ x:940, y:243, width:100, height:50}, props:{guid:{value:'40067ae4-3ada-306f-0e3a-33f00532c494'},text:{value:'����',url:'TTAccountPayableRecord.aspx'}}}},paths:{path13:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path14:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path15:{from:'rect2',to:'rect4', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path16:{from:'rect2',to:'rect5', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path17:{from:'rect4',to:'rect6', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path18:{from:'rect6',to:'rect8', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path19:{from:'rect8',to:'rect7', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}},path20:{from:'rect9',to:'rect6', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path21:{from:'rect10',to:'rect6', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path22:{from:'rect11',to:'rect8', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path23:{from:'rect12',to:'rect8', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "EDPMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:30, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'����������'}, attr:{ x:541, y:128, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect5:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1150, y:129, width:100, height:50}, props:{guid:{value:'cc37c28e-3f87-c548-408e-c5404815c2f6'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect6:{type:'task',text:{text:'Ͷ�����'}, attr:{ x:741, y:128, width:100, height:50}, props:{guid:{value:'7afdc1c7-86a7-86f0-f1f4-27080c5a5e12'},text:{value:'Ͷ�����',url:'TTTenderUNHandleList.aspx'}}},rect7:{type:'task',text:{text:'Ͷ��Ǽ�'}, attr:{ x:741, y:32, width:100, height:50}, props:{guid:{value:'a156bfcc-cc20-8067-f531-88166371239c'},text:{value:'Ͷ��Ǽ�',url:'TTTenderList.aspx'}}},rect8:{type:'task',text:{text:'Ͷ��ȷ��'}, attr:{ x:742, y:234, width:100, height:50}, props:{guid:{value:'ed594496-0b83-c6d3-5922-8efad984eee1'},text:{value:'Ͷ��ȷ��',url:'TTTenderFinanceList.aspx'}}},rect9:{type:'task',text:{text:'���г�Ա��Ŀ״̬'}, attr:{ x:935, y:130, width:120, height:50}, props:{guid:{value:'016de4e1-1aa6-e650-cd13-261b951d2066'},text:{value:'���г�Ա��Ŀ״̬',url:'TTAllProjectsRunStatus.aspx'}}},rect10:{type:'task',text:{text:'��Ŀ���չ���'}, attr:{ x:542, y:234, width:100, height:50}, props:{guid:{value:'273b9704-371f-e2aa-41ae-d3a94c29ae6c'},text:{value:'��Ŀ���չ���',url:'TTProjectRiskManageMain.aspx'}}}},paths:{path11:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path12:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path13:{from:'rect2',to:'rect4', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path14:{from:'rect4',to:'rect6', dots:[],text:{text:'TO Ͷ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO Ͷ�����'}}},path15:{from:'rect7',to:'rect6', dots:[],text:{text:'TO Ͷ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO Ͷ�����'}}},path16:{from:'rect8',to:'rect6', dots:[],text:{text:'TO Ͷ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO Ͷ�����'}}},path17:{from:'rect6',to:'rect9', dots:[],text:{text:'TO ���г�Ա��Ŀ״̬'},textPos:{x:0,y:-10}, props:{text:{value:'TO ���г�Ա��Ŀ״̬'}}},path18:{from:'rect9',to:'rect5', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}},path19:{from:'rect2',to:'rect10', dots:[],text:{text:'TO ��Ŀ���չ���'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "RDPMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:146, y:125, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:405, y:126, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:320, y:18, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'��Ӧ������'}, attr:{ x:795, y:127, width:100, height:50}, props:{guid:{value:'2c43e892-71ba-f826-f0df-5393bdd20173'},text:{value:'��Ӧ������',url:'TTGoodsManage.aspx'}}},rect5:{type:'task',text:{text:'���ϵ�'}, attr:{ x:795, y:11, width:100, height:50}, props:{guid:{value:'ee0e9c61-2ba9-f307-2ca5-45ca1e58345f'},text:{value:'���ϵ�',url:'TTGoodsApplicationOrder.aspx'}}},rect6:{type:'task',text:{text:'���ⵥ'}, attr:{ x:795, y:244, width:100, height:50}, props:{guid:{value:'d0018ba6-ef5d-b0ba-5e9c-97e001e698c5'},text:{value:'���ⵥ',url:'TTGoodsShipmentOrder.aspx'}}},rect7:{type:'task',text:{text:'����������'}, attr:{ x:607, y:127, width:100, height:50}, props:{guid:{value:'39e45ece-447c-72ff-eb5f-bd5d40217313'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect8:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:323, y:241, width:100, height:50}, props:{guid:{value:'77cdd84c-a60a-7777-2611-114303e65439'},text:{value:'��Ŀ�������',url:'TTProjectReqManageMain.aspx'}}},rect9:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1040, y:128, width:100, height:50}, props:{guid:{value:'ed1042f6-938a-dbc6-d876-5b0adbeffecc'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect10:{type:'task',text:{text:'��Ŀȱ�ݹ���'}, attr:{ x:498, y:243, width:100, height:50}, props:{guid:{value:'e7016fa9-7391-8ca2-4f7d-d528cf302d8d'},text:{value:'��Ŀȱ�ݹ���',url:'TTProjectDefectManageMain.aspx'}}},rect11:{type:'task',text:{text:'��Ŀ���չ���'}, attr:{ x:496, y:19, width:100, height:50}, props:{guid:{value:'960e6089-9bbb-4f27-a14f-acc8acc0fce4'},text:{value:'��Ŀ���չ���',url:'TTProjectRiskManageMain.aspx'}}}},paths:{path12:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path13:{from:'rect5',to:'rect4', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path14:{from:'rect6',to:'rect4', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path15:{from:'rect2',to:'rect7', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path16:{from:'rect7',to:'rect4', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path17:{from:'rect3',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path18:{from:'rect8',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path19:{from:'rect4',to:'rect9', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}},path20:{from:'rect10',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path21:{from:'rect11',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "SIPMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:32, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'�տ���ϸ���ܱ�'}, attr:{ x:972, y:32, width:100, height:50}, props:{guid:{value:'6f8c99c9-3fcd-297d-1fa8-19811a07312b'},text:{value:'�տ���ϸ���ܱ�',url:'TTAccountReceiveRecordSummary.aspx'}}},rect5:{type:'task',text:{text:'������ϸ���ܱ�'}, attr:{ x:971, y:233, width:100, height:50}, props:{guid:{value:'5a1f8b2b-c6f3-ce0b-7604-f7a986a65be7'},text:{value:'������ϸ���ܱ�',url:'TTAccountPayRecordSummary.aspx'}}},rect6:{type:'task',text:{text:'����������'}, attr:{ x:541, y:128, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect7:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:233, width:100, height:50}, props:{guid:{value:'cc37c28e-3f87-c548-408e-c5404815c2f6'},text:{value:'��Ŀ�������',url:'TTProjectReqManageMain.aspx'}}},rect8:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1192, y:127, width:100, height:50}, props:{guid:{value:'28fad06e-abdf-9e5c-5dcc-83cf8e31b868'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect9:{type:'task',text:{text:'��Ӧ������'}, attr:{ x:734, y:126, width:100, height:50}, props:{guid:{value:'97a9836e-9bfd-0785-186c-82b045d4c045'},text:{value:'��Ӧ������',url:'TTGoodsManage.aspx'}}},rect10:{type:'task',text:{text:'�ɹ�����'}, attr:{ x:681, y:35, width:100, height:50}, props:{guid:{value:'2885595e-3090-94cf-dcbb-eeec21743fd1'},text:{value:'�ɹ�����',url:'TTMakeGoodsPurchase.aspx'}}},rect11:{type:'task',text:{text:'��ⵥ'}, attr:{ x:831, y:33, width:100, height:50}, props:{guid:{value:'ff012f74-eb6a-ece7-07d0-802a93ac8630'},text:{value:'��ⵥ',url:'TTMakeGoods.aspx'}}},rect12:{type:'task',text:{text:'���ϵ�'}, attr:{ x:681, y:232, width:100, height:50}, props:{guid:{value:'d6a96f3c-4232-b23a-5d98-851660ee72e3'},text:{value:'���ϵ�',url:'TTGoodsApplicationOrder.aspx'}}},rect13:{type:'task',text:{text:'���ⵥ'}, attr:{ x:831, y:231, width:100, height:50}, props:{guid:{value:'8552f52f-2566-d548-b15d-b482a47f9c59'},text:{value:'���ⵥ',url:'TTGoodsShipmentOrder.aspx'}}},rect14:{type:'task',text:{text:'�������'}, attr:{ x:972, y:126, width:100, height:50}, props:{guid:{value:'fdfcecbb-2f46-87c3-73ba-3264fee27d42'},text:{value:'�������',url:'TTReceivablesPayableAlert.aspx'}}}},paths:{path15:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path16:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path17:{from:'rect2',to:'rect6', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path18:{from:'rect6',to:'rect9', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path19:{from:'rect9',to:'rect14', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path20:{from:'rect10',to:'rect9', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path21:{from:'rect11',to:'rect9', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path22:{from:'rect12',to:'rect9', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path23:{from:'rect13',to:'rect9', dots:[],text:{text:'TO ��Ӧ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ӧ������'}}},path24:{from:'rect4',to:'rect14', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path25:{from:'rect5',to:'rect14', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path26:{from:'rect2',to:'rect7', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path27:{from:'rect14',to:'rect8', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "SOPMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:30, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'�ո���Ԥ��'}, attr:{ x:752, y:131, width:100, height:50}, props:{guid:{value:'cf2796ca-6729-11cd-5270-5a691941fe8b'},text:{value:'�ո���Ԥ��',url:'TTReceivablesPayableAlert.aspx'}}},rect5:{type:'task',text:{text:'�տ���ϸ���ܱ�'}, attr:{ x:961, y:69, width:100, height:50}, props:{guid:{value:'6f8c99c9-3fcd-297d-1fa8-19811a07312b'},text:{value:'�տ���ϸ���ܱ�',url:'TTAccountReceiveRecordSummary.aspx'}}},rect6:{type:'task',text:{text:'������ϸ���ܱ�'}, attr:{ x:961, y:180, width:100, height:50}, props:{guid:{value:'5a1f8b2b-c6f3-ce0b-7604-f7a986a65be7'},text:{value:'������ϸ���ܱ�',url:'TTAccountPayRecordSummary.aspx'}}},rect7:{type:'task',text:{text:'����������'}, attr:{ x:541, y:128, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect8:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:540, y:236, width:100, height:50}, props:{guid:{value:'cc37c28e-3f87-c548-408e-c5404815c2f6'},text:{value:'��Ŀ�������',url:'TTProjectReqManageMain.aspx'}}},rect21:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1174, y:131, width:100, height:50}, props:{guid:{value:'3801c149-8a75-9414-4a66-2dd13c668cd1'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}}},paths:{path10:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path11:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path12:{from:'rect3',to:'rect4', dots:[],text:{text:'TO �ո���Ԥ��'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ո���Ԥ��'}}},path13:{from:'rect4',to:'rect5', dots:[],text:{text:'TO �տ���ϸ���ܱ�'},textPos:{x:0,y:-10}, props:{text:{value:'TO �տ���ϸ���ܱ�'}}},path14:{from:'rect4',to:'rect6', dots:[],text:{text:'TO ������ϸ���ܱ�'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path15:{from:'rect2',to:'rect7', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path16:{from:'rect7',to:'rect4', dots:[],text:{text:'TO �ո���Ԥ��'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ո���Ԥ��'}}},path18:{from:'rect8',to:'rect4', dots:[],text:{text:'TO �ո���Ԥ��'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path22:{from:'rect2',to:'rect8', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path23:{from:'rect4',to:'rect21', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "GAPMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:538, y:29, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'����������'}, attr:{ x:541, y:127, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect5:{type:'task',text:{text:'��Ա����'}, attr:{ x:740, y:130, width:100, height:50}, props:{guid:{value:'f1e0ed67-edc6-c121-6483-f8c659a50981'},text:{value:'��Ա����',url:'TTMyMemWorkLoad.aspx'}}},rect6:{type:'task',text:{text:'���г�Ա��Ŀ״̬'}, attr:{ x:938, y:131, width:129, height:50}, props:{guid:{value:'b38dbce8-3d0f-59c2-254c-eed1be874be4'},text:{value:'���г�Ա��Ŀ״̬',url:'TTAllProjectsRunStatus.aspx'}}},rect7:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:541, y:240, width:100, height:50}, props:{guid:{value:'05017a00-1673-af9b-376a-4910161fabdb'},text:{value:'��Ŀ�������',url:'TTProjectReqManageMain.aspx'}}},rect8:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1169, y:132, width:100, height:50}, props:{guid:{value:'aa579bd1-56e0-6b13-0f2f-0cc3f2bcba5c'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}}},paths:{path9:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path10:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path11:{from:'rect2',to:'rect4', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path12:{from:'rect2',to:'rect7', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path13:{from:'rect4',to:'rect5', dots:[],text:{text:'TO ��Ա����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ա����'}}},path14:{from:'rect5',to:'rect6', dots:[],text:{text:'TO ���г�Ա��Ŀ״̬'},textPos:{x:0,y:-10}, props:{text:{value:'TO ���г�Ա��Ŀ״̬'}}},path15:{from:'rect6',to:'rect8', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "ERP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:30, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'����������'}, attr:{ x:541, y:128, width:100, height:50}, props:{guid:{value:'7c6325be-8337-ec12-76ed-1a03b86afab4'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect5:{type:'task',text:{text:'������'}, attr:{ x:751, y:129, width:100, height:50}, props:{guid:{value:'82e392bd-eacd-e39b-6c04-20b6b557fb8d'},text:{value:'������',url:'TTGoodsManage.aspx'}}},rect6:{type:'task',text:{text:'��ⵥ'}, attr:{ x:806, y:32, width:100, height:50}, props:{guid:{value:'220f25e9-5536-da10-cf76-df87209bdd57'},text:{value:'��ⵥ',url:'TTMakeGoods.aspx'}}},rect7:{type:'task',text:{text:'���ⵥ'}, attr:{ x:750, y:232, width:100, height:50}, props:{guid:{value:'c35a73a7-7760-0515-9cb4-c819a205e156'},text:{value:'���ⵥ',url:'TTGoodsShipmentOrder.aspx'}}},rect8:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1140, y:130, width:100, height:50}, props:{guid:{value:'7493182a-8965-2821-127c-307cf283d679'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect9:{type:'task',text:{text:'��Ŀȱ�ݹ���'}, attr:{ x:541, y:232, width:100, height:50}, props:{guid:{value:'ee665fc9-3bf7-6e8f-6258-ac32f63aeb21'},text:{value:'��Ŀȱ�ݹ���',url:'TTProjectDefectManageMain.aspx'}}},rect10:{type:'task',text:{text:'��ҵ����'}, attr:{ x:941, y:130, width:100, height:50}, props:{guid:{value:'a1db5803-1780-e0c5-2c57-6a69a7ef579d'},text:{value:'��ҵ����',url:'TTGoodsProductionManagement.aspx'}}},rect11:{type:'task',text:{text:'������ҵ��'}, attr:{ x:940, y:32, width:100, height:50}, props:{guid:{value:'5df740b5-bc9c-d182-e1ef-dd6c2602838b'},text:{value:'������ҵ��',url:'TTGoodsProductionOrder.aspx'}}},rect12:{type:'task',text:{text:'��ҵ��������'}, attr:{ x:940, y:233, width:100, height:50}, props:{guid:{value:'5d11af4b-6483-4d3f-078b-21877023f95f'},text:{value:'��ҵ��������',url:'TTGoodsApplicationOrderForProduction.aspx'}}},rect13:{type:'task',text:{text:'�ɹ�����'}, attr:{ x:681, y:32, width:100, height:50}, props:{guid:{value:'101a03f5-091f-8d2d-e3a0-3a4053d40323'},text:{value:'�ɹ�����',url:'TTMakeGoodsPurchase.aspx'}}}},paths:{path14:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path15:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path16:{from:'rect2',to:'rect4', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path17:{from:'rect4',to:'rect5', dots:[],text:{text:'TO ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ������'}}},path18:{from:'rect6',to:'rect5', dots:[],text:{text:'TO ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ������'}}},path19:{from:'rect7',to:'rect5', dots:[],text:{text:'TO ������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ������'}}},path20:{from:'rect2',to:'rect9', dots:[],text:{text:'TO ��Ŀȱ�ݹ���'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀȱ�ݹ���'}}},path21:{from:'rect5',to:'rect10', dots:[],text:{text:'TO ��ҵ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ҵ����'}}},path22:{from:'rect10',to:'rect8', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}},path23:{from:'rect10',to:'rect11', dots:[],text:{text:'TO ������ҵ��'},textPos:{x:0,y:-10}, props:{text:{value:'TO ������ҵ��'}}},path24:{from:'rect10',to:'rect12', dots:[],text:{text:'TO ��ҵ��������'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path25:{from:'rect13',to:'rect5', dots:[],text:{text:'TO ������'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "CMP")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1160, y:122, width:100, height:50}, props:{guid:{value:'a7881a76-7a03-bbb7-acb5-a0b3c9798e5d'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}},rect2:{type:'task',text:{text:'����������'}, attr:{ x:336, y:118, width:115, height:50}, props:{guid:{value:'9070892c-92e7-a7ba-6a9e-eabbb579c872'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect3:{type:'task',text:{text:'���ù���������'}, attr:{ x:118, y:117, width:114, height:50}, props:{guid:{value:'e4ea0ce0-ef2b-704c-b200-7b8fa1f73673'},text:{value:'���ù���������',url:'TTRegularWLMain.aspx'}}},rect4:{type:'task',text:{text:'��ͬ����'}, attr:{ x:569, y:118, width:100, height:50}, props:{guid:{value:'cadbd16e-60c2-9a78-469b-303d94aada59'},text:{value:'��ͬ����',url:'TTConstractManagement.aspx'}}},rect5:{type:'task',text:{text:'�ʲ�����'}, attr:{ x:768, y:119, width:100, height:50}, props:{guid:{value:'ff18fe05-f7f6-a480-bf89-aee77f83738b'},text:{value:'�ʲ�����',url:'TTAssetManage.aspx'}}},rect6:{type:'task',text:{text:'�������'}, attr:{ x:959, y:121, width:100, height:50}, props:{guid:{value:'78df5ac9-4c70-d0fc-6b5d-c05c64e9eda0'},text:{value:'�������',url:'TTMeetingManage.aspx'}}},rect7:{type:'task',text:{text:'�ƶ���ͬ'}, attr:{ x:569, y:13, width:100, height:50}, props:{guid:{value:'2e4ba6ce-eaaf-707c-af98-1f6de5f4bbd2'},text:{value:'�ƶ���ͬ',url:'TTMakeConstract.aspx'}}},rect8:{type:'task',text:{text:'��ͬ�ո���Ԥ��'}, attr:{ x:570, y:244, width:100, height:50}, props:{guid:{value:'43c3f25a-f5b7-1240-bfd7-fe9a72565e62'},text:{value:'��ͬ�ո���Ԥ��',url:'TTConstractUnHandleReceivePay.aspx'}}},rect9:{type:'task',text:{text:'�ʲ��Ǽ����'}, attr:{ x:768, y:13, width:100, height:50}, props:{guid:{value:'f3a168f0-0754-c806-8335-6f43083cd1c3'},text:{value:'�ʲ��Ǽ����',url:'TTMakeAsset.aspx'}}},rect10:{type:'task',text:{text:'��Ӧ�̵���'}, attr:{ x:768, y:244, width:100, height:50}, props:{guid:{value:'e9763179-5c7f-b63e-3f28-27dc57403adc'},text:{value:'��Ӧ�̵���',url:'TTMakeVendor.aspx'}}}},paths:{path11:{from:'rect3',to:'rect2', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path12:{from:'rect2',to:'rect4', dots:[],text:{text:'TO ��ͬ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ͬ����'}}},path13:{from:'rect4',to:'rect5', dots:[],text:{text:'TO �ʲ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ʲ�����'}}},path14:{from:'rect5',to:'rect6', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:'TO �������'}}},path15:{from:'rect6',to:'rect1', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path16:{from:'rect7',to:'rect4', dots:[],text:{text:'TO ��ͬ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ͬ����'}}},path17:{from:'rect4',to:'rect8', dots:[],text:{text:'TO ��ͬ�ո���Ԥ��'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ͬ�ո���Ԥ��'}}},path19:{from:'rect10',to:'rect5', dots:[],text:{text:'TO �ʲ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ʲ�����'}}},path20:{from:'rect9',to:'rect5', dots:[],text:{text:'TO �ʲ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ʲ�����'}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType == "CRM")
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'����������'}, attr:{ x:1033, y:114, width:100, height:50}, props:{guid:{value:'a7881a76-7a03-bbb7-acb5-a0b3c9798e5d'},text:{value:'����������',url:'TTWLManage.aspx'}}},rect2:{type:'task',text:{text:'�ͻ�����'}, attr:{ x:60, y:109, width:100, height:50}, props:{guid:{value:'3952c5e4-0d25-add9-8021-ad5f7f334d98'},text:{value:'�ͻ�����',url:'TTMakeCustomer.aspx'}}},rect3:{type:'task',text:{text:'�ͻ�����'}, attr:{ x:229, y:110, width:100, height:50}, props:{guid:{value:'f7543276-c8df-37e4-5e85-e1b37a22529a'},text:{value:'�ͻ�����',url:'TTCustomerManagement.aspx'}}},rect4:{type:'task',text:{text:'��¼�ͻ�����'}, attr:{ x:394, y:111, width:100, height:50}, props:{guid:{value:'b72692cc-6050-cd8c-d6ad-151b700ce96a'},text:{value:'��¼�ͻ�����',url:'TTCustomerQuestionRecord.aspx'}}},rect5:{type:'task',text:{text:'�ͻ�����'}, attr:{ x:566, y:110, width:100, height:50}, props:{guid:{value:'3f24ae23-e9e7-747f-a131-3226f27a8650'},text:{value:'�ͻ�����',url:'TTCustomerQuestionManage.aspx'}}},rect6:{type:'task',text:{text:'ֱ�ӳ�Ա�ͻ�����'}, attr:{ x:719, y:20, width:100, height:50}, props:{guid:{value:'5be02d4d-4885-de29-ff36-af24bc36c22c'},text:{value:'ֱ�ӳ�Ա�ͻ�����',url:'TTMyMemberCustomerQuestions.aspx'}}},rect7:{type:'task',text:{text:'ֱ�ӳ�Ա�ͻ�'}, attr:{ x:720, y:216, width:100, height:50}, props:{guid:{value:'623ae48d-2495-f612-6a49-69c00ad4dd8f'},text:{value:'ֱ�ӳ�Ա�ͻ�',url:'TTMyMemberCustomers.aspx'}}},rect8:{type:'task',text:{text:'��ͬ����'}, attr:{ x:854, y:113, width:100, height:50}, props:{guid:{value:'d7b0d005-9fce-a047-8aec-b67aa39d69fd'},text:{value:'��ͬ����',url:'TTConstractManagement.aspx'}}},rect9:{type:'task',text:{text:'֪ʶ����'}, attr:{ x:1208, y:114, width:100, height:50}, props:{guid:{value:'0f72603f-0b40-f129-608a-168a9284654e'},text:{value:'֪ʶ����',url:'TTDocumentManage.aspx'}}}},paths:{path10:{from:'rect2',to:'rect3', dots:[],text:{text:'TO �ͻ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ͻ�����'}}},path11:{from:'rect3',to:'rect4', dots:[],text:{text:'TO ��¼�ͻ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��¼�ͻ�����'}}},path12:{from:'rect4',to:'rect5', dots:[],text:{text:'TO �ͻ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ͻ�����'}}},path13:{from:'rect5',to:'rect6', dots:[],text:{text:'TO ֱ�ӳ�Ա�ͻ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֱ�ӳ�Ա�ͻ�����'}}},path14:{from:'rect5',to:'rect7', dots:[],text:{text:'TO ֱ�ӳ�Ա�ͻ�'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֱ�ӳ�Ա�ͻ�'}}},path15:{from:'rect5',to:'rect8', dots:[],text:{text:'TO ��ͬ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ͬ����'}}},path16:{from:'rect8',to:'rect1', dots:[],text:{text:'TO ����������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ����������'}}},path17:{from:'rect1',to:'rect9', dots:[],text:{text:'TO ֪ʶ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ֪ʶ����'}}},path18:{from:'rect6',to:'rect8', dots:[],text:{text:'TO ��ͬ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��ͬ����'}}},path19:{from:'rect7',to:'rect8', dots:[],text:{text:'TO ��ͬ����'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            if (strSystemProductType.IndexOf("SAAS") > -1)
            {
                strMFXML = @"{states:{rect1:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:105, y:127, width:100, height:50}, props:{guid:{value:'02899d0f-472c-063f-f67e-c6b9d45c8d29'},text:{value:'��Ŀ����',url:'TTMakeProject.aspx'}}},rect2:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:313, y:127, width:100, height:50}, props:{guid:{value:'5fe585ee-cf14-5681-46eb-e0f32e23b369'},text:{value:'��Ŀ����',url:'TTProjectManageSAAS.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ�������'}, attr:{ x:539, y:27, width:100, height:50}, props:{guid:{value:'e2637d3e-2cdb-2369-c06d-febd974c8af5'},text:{value:'��Ŀ�������',url:'TTProjectTaskManageMain.aspx'}}},rect4:{type:'task',text:{text:'�տ���ϸ���ܱ�'}, attr:{ x:952, y:26, width:100, height:50}, props:{guid:{value:'6f8c99c9-3fcd-297d-1fa8-19811a07312b'},text:{value:'�տ���ϸ���ܱ�',url:'TTAccountReceiveRecordSummarySAAS.aspx'}}},rect5:{type:'task',text:{text:'������ϸ���ܱ�'}, attr:{ x:952, y:221, width:100, height:50}, props:{guid:{value:'5a1f8b2b-c6f3-ce0b-7604-f7a986a65be7'},text:{value:'������ϸ���ܱ�',url:'TTAccountPayRecordSummarySAAS.aspx'}}},rect6:{type:'task',text:{text:'�������'}, attr:{ x:711, y:130, width:100, height:50}, props:{guid:{value:'a5861d1c-845e-3475-5fb0-c194c3325bbb'},text:{value:'�������',url:'TTReceivablesPayableAlert.aspx'}}},rect7:{type:'task',text:{text:'���г�Ա����Ŀ'}, attr:{ x:1165, y:133, width:100, height:50}, props:{guid:{value:'1402c8e9-c44c-d59b-5f95-2448f73144ec'},text:{value:'���г�Ա����Ŀ',url:'TTAllProject.aspx'}}},rect8:{type:'task',text:{text:'��Ŀ�ĵ�����'}, attr:{ x:540, y:221, width:100, height:50}, props:{guid:{value:'4c9dcf45-06d3-a292-1516-57d02b2a61e0'},text:{value:'��Ŀ�ĵ�����',url:'TTProjectDocManageMain.aspx'}}}},paths:{path9:{from:'rect1',to:'rect2', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path10:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ�������'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�������'}}},path11:{from:'rect6',to:'rect4', dots:[],text:{text:'TO �տ���ϸ���ܱ�'},textPos:{x:0,y:-10}, props:{text:{value:'TO �տ���ϸ���ܱ�'}}},path12:{from:'rect6',to:'rect5', dots:[],text:{text:'TO ������ϸ���ܱ�'},textPos:{x:0,y:-10}, props:{text:{value:'TO ������ϸ���ܱ�'}}},path13:{from:'rect2',to:'rect6', dots:[],text:{text:'TO �������'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path14:{from:'rect2',to:'rect8', dots:[],text:{text:'TO ��Ŀ�ĵ�����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ�ĵ�����'}}},path15:{from:'rect6',to:'rect7', dots:[],text:{text:'TO ���г�Ա����Ŀ'},textPos:{x:0,y:-10}, props:{text:{value:''}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
                SaveModuleFlowDefinition("��������", strMFXML, 3, "INNER"); 
            }

            strMFXML = @"{states:{rect1:{type:'task',text:{text:'�ҵ�Э��'}, attr:{ x:89, y:114, width:100, height:50}, props:{guid:{value:'9cf167e5-eaa0-2a0a-d046-f9b95f20a18f'},text:{value:'�ҵ�Э��',url:'TTCollaborationManage.aspx'}}},rect2:{type:'task',text:{text:'�ҵ�����'}, attr:{ x:292, y:114, width:100, height:50}, props:{guid:{value:'d453b354-38bd-d4f2-5bd2-d052ce6757d0'},text:{value:'�ҵ�����',url:'TTWLManage.aspx'}}},rect3:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:486, y:116, width:100, height:50}, props:{guid:{value:'4d13368a-8662-46bb-2302-30f0ec9ab00b'},text:{value:'��Ŀ����',url:'TTProjectManageOuter.aspx'}}},rect4:{type:'task',text:{text:'�ҵĿͷ�'}, attr:{ x:677, y:116, width:100, height:50}, props:{guid:{value:'97c25bff-5352-fc37-8ae7-cb421fcdf7cc'},text:{value:'�ҵĿͷ�',url:'TTCustomerQuestionManage.aspx'}}},rect5:{type:'task',text:{text:'�ҵ�ȱ��'}, attr:{ x:386, y:225, width:100, height:50}, props:{guid:{value:'7bfb19b8-ba4b-9be7-9af6-a3037cd27a1c'},text:{value:'�ҵ�ȱ��',url:'TTDefectHandlePageThirdPart.aspx'}}},rect6:{type:'task',text:{text:'�ҵ�����'}, attr:{ x:583, y:223, width:100, height:50}, props:{guid:{value:'463cef82-59b2-a9ad-134c-3b446afe71a9'},text:{value:'�ҵ�����',url:'TTReqHandlePageThirdPart.aspx'}}},rect7:{type:'task',text:{text:'�ҵĿ���'}, attr:{ x:873, y:117, width:100, height:50}, props:{guid:{value:'c6ab08dc-af3e-84c8-b0d6-094f352580cc'},text:{value:'�ҵĿ���',url:'TTUserAttendanceRecordForMe.aspx'}}},rect8:{type:'task',text:{text:'��Ŀ����'}, attr:{ x:486, y:11, width:100, height:50}, props:{guid:{value:'907e7f31-cd94-309a-bdf6-95313362f7c1'},text:{value:'��Ŀ����',url:'TTProjectTaskManageMain.aspx'}}}},paths:{path9:{from:'rect2',to:'rect3', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path10:{from:'rect3',to:'rect4', dots:[],text:{text:'TO �ҵĿͷ�'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ҵĿͷ�'}}},path11:{from:'rect4',to:'rect7', dots:[],text:{text:'TO �ҵĿ���'},textPos:{x:0,y:-10}, props:{text:{value:'TO �ҵĿ���'}}},path12:{from:'rect6',to:'rect3', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path13:{from:'rect5',to:'rect3', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}},path14:{from:'rect1',to:'rect2', dots:[],text:{text:'TO �ҵ�����'},textPos:{x:0,y:-10}, props:{text:{value:''}}},path15:{from:'rect8',to:'rect3', dots:[],text:{text:'TO ��Ŀ����'},textPos:{x:0,y:-10}, props:{text:{value:'TO ��Ŀ����'}}}},props:{props:{name:{value:'�½�����'},key:{value:''},desc:{value:''}}}}"; 
            SaveModuleFlowDefinition("��������", strMFXML, 3, "OUTER"); 
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //����ʵʩ�׶εĻ�������ɾ������
    public static void UpdateIsCanClearBaseData(string strUserCode, string strUserName)
    {
        string strHQL1, strHQL2;

        try
        {
            strHQL1 = "Select * from T_SystemDataManageForBeginer Where OperationName = 'ClearData'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_SystemDataManageForBeginer");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL2 = string.Format(@"Insert Into T_SystemDataManageForBeginer(OperationName,IsForbit,OperatorCode,OperatorName,Operatetime,IsBackup)
                      Values('{0}','{1}','{2}','{3}',now(),'YES')", "ClearData", "YES", strUserCode, strUserName);

                ShareClass.RunSqlCommand(strHQL2);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //��ʼ��ģ��Ĳ����������̶���
    public static void SaveModuleFlowDefinition(string strModuleName, string strMFXML, int intUpdateMark, string strUserType)
    {
        string strHQL;
        IList lst;

        string strID;
        int i;

        try
        {
            if (GetProModuleUpdateMark(strModuleName, strUserType) != intUpdateMark)
            {
                ProModuleLevelBLL proModuleLevelBLL = new ProModuleLevelBLL();
                strHQL = string.Format(@"from ProModuleLevel as proModuleLevel where proModuleLevel.ModuleName = '{0}' and proModuleLevel.UserType ='{1}' and proModuleLevel.ModuleType = 'SYSTEM'", strModuleName, strUserType);
                lst = proModuleLevelBLL.GetAllProModuleLevels(strHQL);

                ProModuleLevel proModuleLevel;

                for (i = 0; i < lst.Count; i++)
                {
                    proModuleLevel = (ProModuleLevel)lst[i];

                    strID = proModuleLevel.ID.ToString();
                    proModuleLevel.ModuleDefinition = strMFXML;
                    proModuleLevelBLL.UpdateProModuleLevel(proModuleLevel, int.Parse(strID));
                }

                strHQL = string.Format(@"Update T_ProModuleLevel Set UpdateMark = {0} Where ModuleName = '{1}' and UserType ='{2}' and ModuleType = 'SYSTEM'", intUpdateMark, strModuleName, strUserType);
                ShareClass.RunSqlCommand(strHQL);


                //���û�����ı�־����ˢ��ҳ�滺��
                ChangePageCache();
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }


    //���û��������ͼ  
    private static void UpdateSystemAnalystChartForUser()
    {
        string strHQL;

        strHQL = @"Insert Into public.t_systemanalystchartrelateduser(UserCode,chartName,FormType,SortNumber)
               Select B.UserCode,A.chartName,'PersonalSpacePage',1 From t_systemanalystchartmanagement A,public.t_systemactiveuser B
                 Where A.ChartName 
	             Not In (Select ChartName From t_systemanalystchartrelateduser Where UserCode = B.UserCode and FormType = 'PersonalSpacePage' )
	             and A.ChartName in ('��ִ����Ŀ״̬','������Ŀ״̬','�����Ŀ��ʱ״̬','��ִ������״̬','��Ŀ��Ȼؿ�״̬');";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = @"Update t_systemanalystchartrelateduser Set SortNumber = 2 Where ChartName = '������Ŀ״̬';
                Update t_systemanalystchartrelateduser Set SortNumber = 3 Where ChartName = '�����Ŀ��ʱ״̬';
                Update t_systemanalystchartrelateduser Set SortNumber = 5 Where ChartName = '��Ŀ��Ȼؿ�״̬';
                Update t_systemanalystchartrelateduser Set SortNumber = 4 Where ChartName = '��ִ������״̬';";
             
        ShareClass.RunSqlCommand(strHQL);

    }

    //���û�����ı�־����ˢ��ҳ�滺��
    protected static void ChangePageCache()
    {
        //����ҳ�滺�棬ˢ��ҳ��
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceModuleFlowView.aspx", "");
        ShareClass.AddSpaceLineToFile("TTModuleFlowChartViewJS.aspx", "");
        ShareClass.AddSpaceLineToFile("WFDesigner/TTTakeTopMFChartViewJS.aspx", "");
    }


    //ȡ��ģ���и��±�־
    protected static int GetProModuleUpdateMark(string strModuleName, string strUserType)
    {
        string strHQL;

        strHQL = string.Format(@"Select UpdateMark From T_ProModuleLevel Where ModuleName = '{0}' and UserType = '{1}'", strModuleName, strUserType);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //ȡ��ϵͳԱ������
    public static int getUserNumber()
    {
        string strHQL1;

        strHQL1 = "Select * from T_ProjectMember limit 3";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_SystemDataManageForBeginer");

        return ds.Tables[0].Rows.Count;
    }


    //ȡ���û�����ͼ����
    public static int GetUserChartNumber(string strUserCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From t_systemanalystchartrelateduser
           Where UserCode = '{0}'", strUserCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "t_systemanalystchartrelateduser");

        return ds.Tables[0].Rows.Count;
    }
}
