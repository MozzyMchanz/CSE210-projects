using System;

class Program
{
    static void Main(string[] args)
    {
        // Creativity: this solution uses an abstract Activity base class
        // and derived classes for Running, Cycling, and Swimming.
        // It creates concrete activity objects directly and prints summaries
        // without requiring any user interaction.
        List<Activity> activities = new List<Activity>
        {
            new Running("03 Nov 2022", 30, 3.0),
            new Cycling("03 Nov 2022", 45, 14.0),
            new Swimming("03 Nov 2022", 30, 20)
        };

        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}