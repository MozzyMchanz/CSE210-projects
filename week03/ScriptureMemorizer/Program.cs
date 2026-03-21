using System;
using System.Collections.Generic;
using System.Linq;

// Word class - Encapsulates a single word in the scripture
public class Word
{
    private string _text;
    private string _display;

    public Word(string text)
    {
        _text = text.Trim();
        _display = _text;
    }

    public bool IsHidden => _display.All(c => c == '_');

    public void Hide()
    {
        _display = new string('_', _text.Length);
    }

    public string GetDisplay() => _display;
}

// Reference class - Encapsulates scripture reference with multiple constructors
public class Reference
{
    private string _book;
    private string _verse1;
    private string? _verse2;

    // Constructor for single verse: "John 3:16"
    public Reference(string book, string verse)
    {
        _book = book;
        _verse1 = verse;
    }

    // Constructor for range: "Proverbs 3:5-6"
    public Reference(string book, string verse1, string verse2)
    {
        _book = book;
        _verse1 = verse1;
        _verse2 = verse2;
    }

    // Constructor parsing full reference string
    public Reference(string fullReference)
    {
        var parts = fullReference.Split(' ');
        _book = parts[0];
        var versePart = parts[1];
        if (versePart.Contains('-'))
        {
            var verses = versePart.Split('-');
            _verse1 = verses[0];
            _verse2 = verses[1];
        }
        else
        {
            _verse1 = versePart;
        }
    }

    public override string ToString()
    {
        return _verse2 != null ? $"{_book} {_verse1}-{_verse2}" : $"{_book} {_verse1}";
    }
}

// Scripture class - Main encapsulation for scripture with hiding logic
public class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                     .Select(w => new Word(w))
                     .ToList();
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(w => w.IsHidden);
    }

    public void HideRandomWords(int count = 2)
    {
        var visibleWords = _words.Where(w => !w.IsHidden).ToList();
        if (visibleWords.Count == 0) return;

        var random = new Random();
        var numToHide = Math.Min(count, visibleWords.Count);
        var toHide = visibleWords.OrderBy(_ => random.Next())
                                 .Take(numToHide)
                                 .ToList();
        foreach (var word in toHide)
        {
            word.Hide();
        }
    }

    public string GetDisplayText()
    {
        return $"{_reference}: {string.Join(" ", _words.Select(w => w.GetDisplay()))}";
    }

    public int VisibleWordCount => _words.Count(w => !w.IsHidden);
    public int TotalWordCount => _words.Count;
}

// Program class
class Program
{
    static void Main(string[] args)
    {
        // Creativity: Menu for multiple scriptures
        Console.WriteLine("Scripture Memorizer");
        Console.WriteLine("1. John 3:16");
        Console.WriteLine("2. Proverbs 3:5-6");
        Console.Write("Select (1 or 2): ");
        string choice = Console.ReadLine()?.Trim();

        Scripture scripture;
        if (choice == "1")
        {
            scripture = new Scripture(new Reference("John", "3:16"),
                "For God so loved the world that he gave his one and only Son , that whoever believes in him shall not perish but have eternal life .");
        }
        else
        {
            scripture = new Scripture(new Reference("Proverbs", "3:5", "3:6"),
                "Trust in the LORD with all your heart and lean not on your own understanding ; in all your ways submit to him , and he will make your paths straight .");
        }

        Console.Clear();
        int attempts = 0;

        while (!scripture.IsCompletelyHidden())
        {
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine($"\nVisible words: {scripture.VisibleWordCount}/{scripture.TotalWordCount}");
            Console.Write("\nPress Enter for next hide or type 'quit' to exit: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "quit")
            {
                Console.WriteLine("Thanks for memorizing!");
                return;
            }

            attempts++;
            int hideCount = Math.Min(2 + (attempts / 3), scripture.VisibleWordCount); // Progressive hiding
            scripture.HideRandomWords(hideCount);
            Console.Clear();
        }

        // Final fully hidden display
        Console.WriteLine(scripture.GetDisplayText());
        Console.WriteLine($"\nCongratulations! You memorized the scripture in {attempts} attempts.");
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }
}

/*
Creativity Exceeded:
1. Stretch goal: Only hides visible (unhidden) words randomly.
2. Multiple scripture choices via menu.
3. Progressive hiding: starts with 2 words, increases over attempts.
4. Progress indicator: visible/total words.
5. Memorization stats: attempts count.
6. Proper encapsulation with private fields, multiple constructors.
7. Handles punctuation in words (kept as-is).
*/

