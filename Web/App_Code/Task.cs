using System;

/// <summary>
/// Task ��ժҪ˵��
/// </summary>
public class Task
{
    public Task()
    {
        //
        // TODO: �ڴ˴���ӹ��캯���߼�
        //
    }

    private int planDays;
    private DateTime planStartDate;
    private DateTime truckStartDate;
    private int truckDates;
    private int trackDays;

    public int PlanDays
    {
        set { planDays = value; }
        get { return planDays; }
    }

    public DateTime PlanStartDate
    {
        set { planStartDate = value; }
        get { return planStartDate; }
    }

    public DateTime TrackStartDate
    {
        set { truckStartDate = value; }
        get { return truckStartDate; }
    }

    public int TruckDates
    {
        set { truckDates = value; }
        get { return truckDates; }
    }

    public int TrackDays
    {
        set { trackDays = value; }
        get { return trackDays; }
    }

    internal static void Run(Action value)
    {
        throw new NotImplementedException();
    }
}