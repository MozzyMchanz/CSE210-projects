using System;

abstract class Activity
{
    private DateTime _date;
    private int _lengthMinutes;

    public Activity(string date, int lengthMinutes)
    {
        _date = DateTime.Parse(date);
        _lengthMinutes = lengthMinutes;
    }

    public string GetDate()
    {
        return _date.ToString("dd MMM yyyy");
    }

    public int GetLengthMinutes()
    {
        return _lengthMinutes;
    }

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    public virtual string GetSummary()
    {
        string activityName = GetType().Name;
        double distance = GetDistance();
        double speed = GetSpeed();
        double pace = GetPace();

        return $"{GetDate()} {activityName} ({GetLengthMinutes()} min): Distance {distance:F1} miles, Speed {speed:F1} mph, Pace: {pace:F2} min per mile";
    }
}
