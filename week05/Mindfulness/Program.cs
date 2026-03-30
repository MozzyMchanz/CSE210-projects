using System;
using System.Collections.Generic;
using System.Threading;

/*
Mindfulness App - CSE210 Project (Single File Version)
All logic (classes, animations, menu) consolidated into Program.cs per request.
Implements full spec: Breathing/Reflection/Listing with menu, start/end messages, animations, timing.
Exceeds reqs: DateTime timing, input handling, varied animations.
*/

public enum AnimationType { Spinner, Countdown, Dots }

public abstract class Activity
{
    private string _name;
    private string _description;
    protected int _duration;

    protected Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void StartActivity()
    {
        DisplayStartingMessage();
        Thread.Sleep(2000);
        Console.WriteLine("Get ready...");
        Pause(3000, AnimationType.Spinner);
        Run();
    }

    public void EndActivity()
    {
        Pause(2000, AnimationType.Dots);
        DisplayEndingMessage();
        Pause(3000, AnimationType.Spinner);
    }

    protected virtual void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Starting {_name}");
        Console.WriteLine(_description);
        Console.Write("How long, in seconds, would you like for your session? ");
        while (!int.TryParse(Console.ReadLine(), out _duration) || _duration <= 0)
        {
            Console.Write("Please enter a valid positive number: ");
        }
        Console.Clear();
        Console.WriteLine("Prepare to begin in ");
    }

    protected virtual void DisplayEndingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("You have done well!");
        Console.WriteLine();
        Console.WriteLine($"You have completed the {_name} activity for {_duration} seconds.");
        Console.WriteLine();
    }

    protected void Pause(int milliseconds, AnimationType type)
    {
        switch (type)
        {
            case AnimationType.Spinner:
                Spinner(milliseconds / 200);
                break;
            case AnimationType.Countdown:
                Countdown(milliseconds / 1000);
                break;
            case AnimationType.Dots:
                Dots(milliseconds / 1000);
                break;
        }
    }

    private void Spinner(int iterations)
    {
        string[] spinner = { "|", "/", "-", "\\" };
        for (int i = 0; i < iterations; i++)
        {
            foreach (string s in spinner)
            {
                Console.Write(s);
                Thread.Sleep(200);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }
        Console.WriteLine();
    }

    private void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i}");
            if (i > 1) Console.Write("...");
            Thread.Sleep(1000);
            if (i > 1)
            {
                int back = (i == 2 ? 2 : 4);
                Console.SetCursorPosition(Console.CursorLeft - back, Console.CursorTop);
            }
            else
            {
                Console.WriteLine(" Ready!");
            }
        }
        Console.WriteLine();
    }

    private void Dots(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    public abstract void Run();
}

public class BreathingActivity : Activity
{
    public BreathingActivity() : base("breathing", "This breathing activity will help you relax by walking you through slowly breathing in and out. Clear your mind and focus on your breathing.") { }

    public override void Run()
    {
        DateTime endTime = DateTime.UtcNow.AddSeconds(_duration);
        bool breatheIn = true;
        while (DateTime.UtcNow < endTime)
        {
            string message = breatheIn ? "Breathe in . . . " : "Breathe out . . . ";
            Console.Write(message);
            Pause(4500, AnimationType.Countdown);
            Console.SetCursorPosition(0, Console.CursorTop);
            breatheIn = !breatheIn;
        }
        Console.WriteLine();
    }
}

public class ReflectionActivity : Activity
{
    private readonly List<string> _prompts = new()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private readonly List<string> _questions = new()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base("reflecting", "This activity will help you reflect on times of strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.") { }

    public override void Run()
    {
        var rand = new Random();
        string prompt = _prompts[rand.Next(_prompts.Count)];
        Console.WriteLine(prompt);
        Pause(8000, AnimationType.Spinner);

        DateTime endTime = DateTime.UtcNow.AddSeconds(_duration);
        while (DateTime.UtcNow < endTime)
        {
            string question = _questions[rand.Next(_questions.Count)];
            Console.WriteLine($"\n--- {question} ---");
            Pause(8000, AnimationType.Spinner);
        }
    }
}

public class ListingActivity : Activity
{
    private readonly List<string> _prompts = new()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() : base("listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.") { }

    public override void Run()
    {
        var rand = new Random();
        string prompt = _prompts[rand.Next(_prompts.Count)];
        Console.WriteLine(prompt);
        Console.WriteLine("Get ready...");
        Pause(5000, AnimationType.Countdown);

        var items = new List<string>();
        DateTime endTime = DateTime.UtcNow.AddSeconds(_duration);
        Console.WriteLine("You may begin listing items (or press enter without text to skip):");

        while (DateTime.UtcNow < endTime)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input == "-") continue;
            items.Add(input.Trim());
            Pause(500, AnimationType.Dots);
        }

        Console.WriteLine($"\nYou listed {items.Count} items!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Mindfulness Program!");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu Options: ");
            Console.WriteLine(" 1 - Breathing Activity");
            Console.WriteLine(" 2 - Reflection Activity");
            Console.WriteLine(" 3 - Listing Activity");
            Console.WriteLine(" 4 - Quit");
            Console.Write("Select choice: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    var breathing = new BreathingActivity();
                    breathing.StartActivity();
                    breathing.EndActivity();
                    break;
                case "2":
                    var reflection = new ReflectionActivity();
                    reflection.StartActivity();
                    reflection.EndActivity();
                    break;
                case "3":
                    var listing = new ListingActivity();
                    listing.StartActivity();
                    listing.EndActivity();
                    break;
                case "4":
                    Console.WriteLine("Thanks for using Mindfulness!");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Thread.Sleep(2000);
                    break;
            }

            Console.Write("Press Enter to continue to menu...");
            Console.ReadLine();
        }
    }
}

