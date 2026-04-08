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
    private bool _isCompleteState = false;

    public SimpleGoal(string description, int points, bool isCompleteState = false) : base(description, points)
    {
        _isCompleteState = isCompleteState;
    }

    public override string GetDisplayString()
    {
        return _isCompleteState ? $"[X] {Description}" : $"[ ] {Description} (worth {Points} points)";
    }

    public override bool IsCompleted()
    {
        return _isCompleteState;
    }

    public override int RecordEvent()
    {
        if (!_isCompleteState)
        {
            _isCompleteState = true;
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
    private int _targetCount;
    private int _completedCount;
    private int _bonusPoints;
    private bool _paidBonus;

    public int TargetCount => _targetCount;
    public int CompletedCount => _completedCount;
    public int BonusPoints => _bonusPoints;

    public ChecklistGoal(string description, int points, int target = 0, int bonus = 0, int completed = 0, bool paidBonus = false) : base(description, points)
    {
        _targetCount = target;
        _bonusPoints = bonus;
        _completedCount = completed;
        _paidBonus = completed >= target || paidBonus;
    }

    public override string GetDisplayString()
    {
        return $"[ ] {Description} ({_completedCount}/{_targetCount} | worth {Points} points + {_bonusPoints} bonus)";
    }

    public override bool IsCompleted()
    {
        return _completedCount >= _targetCount;
    }

    public override int RecordEvent()
    {
        _completedCount++;
        int earned = Points;
        if (_completedCount == _targetCount && !_paidBonus)
        {
            earned += _bonusPoints;
            _paidBonus = true;
        }
        return earned;
    }
}
