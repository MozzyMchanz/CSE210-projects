using System.Text.Json.Serialization;

public abstract class Goal
{
    public string Description { get; protected set; }
    public int Points { get; protected set; }

    protected Goal(string description, int points)
    {
        Description = description;
        Points = points;
    }

    public abstract string GetDisplayString();
    public abstract bool IsCompleted();
    public abstract int RecordEvent();
}

public class SimpleGoal : Goal
{
    public bool IsCompleted { get; set; } = false;

    public SimpleGoal(string description, int points) : base(description, points) { }

    public override string GetDisplayString()
    {
        return IsCompleted ? $"[X] {Description}" : $"[ ] {Description} (worth {Points} points)";
    }

    public override bool IsCompleted()
    {
        return IsCompleted;
    }

    public override int RecordEvent()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            return Points;
        }
        return 0;
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string description, int points) : base(description, points) { }

    public override string GetDisplayString()
    {
        return $"[ ] {Description} (Eternal Goal - {Points} points)";
    }

    public override bool IsCompleted()
    {
        return false;
    }

    public override int RecordEvent()
    {
        return Points;
    }
}

public class ChecklistGoal : Goal
{
    public int TargetCount { get; set; }
    public int CompletedCount { get; set; }
    public int BonusPoints { get; set; }
    public bool PaidBonus { get; set; } = false;

    public ChecklistGoal(string description, int points, int target, int bonus) : base(description, points)
    {
        TargetCount = target;
        BonusPoints = bonus;
    }

    public override string GetDisplayString()
    {
        return $"[ ] {Description} ({CompletedCount}/{TargetCount} | worth {Points} points + {BonusPoints} bonus)";
    }

    public override bool IsCompleted()
    {
        return CompletedCount >= TargetCount;
    }

    public override int RecordEvent()
    {
        CompletedCount++;
        int earned = Points;
        if (CompletedCount == TargetCount && !PaidBonus)
        {
            earned += BonusPoints;
            PaidBonus = true;
        }
        return earned;
    }
}
