 using System;

public class Entry
{
    public string _date;
    public string _promptText;
    public string _entryText;
    public string _mood; // NEW FEATURE

    public void Display()
    {
        Console.WriteLine($"Date: {_date}");
        Console.WriteLine($"Mood: {_mood}");
        Console.WriteLine($"Prompt: {_promptText}");
        Console.WriteLine($"Response: {_entryText}");
        Console.WriteLine("-----------------------------------");
    }

    public string ToFileString()
    {
        return $"{_date}|{_mood}|{_promptText}|{_entryText}";
    }

    public static Entry FromFileString(string line)
    {
        string[] parts = line.Split("|");

        return new Entry
        {
            _date = parts[0],
            _mood = parts[1],
            _promptText = parts[2],
            _entryText = parts[3]
        };
    }
}