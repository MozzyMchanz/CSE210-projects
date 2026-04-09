class Cycling : Activity
{
    private double _speedMph;

    public Cycling(string date, int lengthMinutes, double speedMph)
        : base(date, lengthMinutes)
    {
        _speedMph = speedMph;
    }

    public override double GetDistance()
    {
        return _speedMph * GetLengthMinutes() / 60.0;
    }

    public override double GetSpeed()
    {
        return _speedMph;
    }

    public override double GetPace()
    {
        return 60.0 / GetSpeed();
    }
}
