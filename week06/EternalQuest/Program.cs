using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    private static List<Goal> _goals = new();
    private static int _score = 0;
    private static readonly string _saveFile = "goals.json";

    static void Main(string[] args)
    {
        LoadGoals();
        while (true)
        {
            Console.Clear();
            DisplayGoals();
            Console.WriteLine($"\nYour Eternal Quest Score: {_score} points");
            int level = _score / 1000 + 1;
            Console.WriteLine($"Level: {level}");
            Console.WriteLine("\nMenu Options:");
            Console.WriteLine(" 1) Create New Goal");
            Console.WriteLine(" 2) Record Goal Event");
            Console.WriteLine(" 3) Save Goals");
            Console.WriteLine(" 4) Load Goals");
            Console.WriteLine(" 5) Quit");
            Console.Write("Select option (1-5): ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    RecordEvent();
                    break;
                case "3":
                    SaveGoals();
                    break;
                case "4":
                    LoadGoals();
                    break;
                case "5":
                    SaveGoals();
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DisplayGoals()
    {
        Console.WriteLine("Goals:");
        if (_goals.Count == 0)
        {
            Console.WriteLine("  No goals yet. Create some!");
            return;
        }
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDisplayString()}");
        }
    }

    private static void CreateGoal()
    {
        Console.Write("What is the goal description? ");
        string desc = Console.ReadLine()?.Trim();
        Console.Write("How many points for completing? ");
        int points = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("\nGoal Types:");
        Console.WriteLine(" 1) Simple Goal (one-time)");
        Console.WriteLine(" 2) Eternal Goal (repeatable)");
        Console.WriteLine(" 3) Checklist Goal (limited repeats)");
        Console.Write("Select goal type (1-3): ");
        string typeChoice = Console.ReadLine()?.Trim();

        Goal? goal = null;
        switch (typeChoice)
        {
            case "1":
                goal = new SimpleGoal(desc ?? "", points);
                break;
            case "2":
                goal = new EternalGoal(desc ?? "", points);
                break;
            case "3":
                Console.Write("How many times needed? ");
                int target = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Bonus points when complete? ");
                int bonus = int.Parse(Console.ReadLine() ?? "0");
                goal = new ChecklistGoal(desc ?? "", points, target, bonus);
                break;
        }

        if (goal != null)
        {
            _goals.Add(goal);
            Console.WriteLine("Goal created! Press any key...");
        }
        else
        {
            Console.WriteLine("Invalid type. Press any key...");
        }
        Console.ReadKey();
    }

    private static void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals. Create some first! Press any key...");
            Console.ReadKey();
            return;
        }

        DisplayGoals();
        Console.Write("Which goal did you accomplish? (number): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= _goals.Count)
        {
            int pts = _goals[index - 1].RecordEvent();
            _score += pts;
            if (pts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Great job! You earned {pts} points!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No points awarded (already completed).");
            }
        }
        else
        {
            Console.WriteLine("Invalid goal number.");
        }
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }

    private static void SaveGoals()
    {
        try
        {
            var data = new
            {
                Score = _score,
                Goals = _goals.Select(g => g switch
                {
                    SimpleGoal sg => new { Type = "Simple", sg.Description, sg.Points, IsCompleted = sg.IsCompleted() },
                    EternalGoal eg => new { Type = "Eternal", eg.Description, eg.Points },
                    ChecklistGoal cg => new { Type = "Checklist", cg.Description, cg.Points, cg.TargetCount, cg.CompletedCount, cg.BonusPoints },
                    _ => null
                }).Where(d => d != null).ToList<object>()
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_saveFile, json);
            Console.WriteLine("Goals saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving: {ex.Message}");
        }
        Console.ReadKey();
    }

    private static void LoadGoals()
    {
        if (!File.Exists(_saveFile))
        {
            Console.WriteLine("No save file found.");
            Console.ReadKey();
            return;
        }

        try
        {
            string json = File.ReadAllText(_saveFile);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

            _score = (int)(double)data["Score"];

            _goals.Clear();
            var goalsData = ((JsonElement)data["Goals"]).EnumerateArray().ToList();

            foreach (var goalData in goalsData)
            {
                var dict = goalData.EnumerateObject().ToDictionary(p => p.Name, p => (object)p.Value);
                string type = (string)dict["Type"];

                Goal? goal = type switch
                {
                    "Simple" => new SimpleGoal((string)dict["Description"], (int)(double)dict["Points"])
                    {
                        _isCompleted = (bool)dict["IsCompleted"] // Note: need public setter or reflection, wait adjust class?
                    },
                    "Eternal" => new EternalGoal((string)dict["Description"], (int)(double)dict["Points"]),
                    "Checklist" => new ChecklistGoal((string)dict["Description"], (int)(double)dict["Points"],
                        (int)(double)dict["TargetCount"], (int)(double)dict["BonusPoints"])
                    {
                        CompletedCount = (int)(double)dict["CompletedCount"] // adjust
                    },
                    _ => null
                };

                if (goal != null)
                {
                    _goals.Add(goal);
                }
            }

            Console.WriteLine("Goals loaded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading: {ex.Message}");
        }
        Console.ReadKey();
    }
}
