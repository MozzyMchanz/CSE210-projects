using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter your grade percentage");
        int grade = int.Parse(Console.ReadLine());
        string letter = "";
        if (grade>=90)
        {letter = "A";}
        else if (grade>=80)
        {letter = "B";}
        else if (grade>=70)
        {letter = "C";}
        else if (grade>=60)
        {letter = "D";}
        else
        {letter = "F";}
        Console.WriteLine ($"Your grade is {letter}");
        if (grade >= 70)
        {Console.WriteLine("Congratulations! you passed the course.");}
        else
        {Console.WriteLine("Keep trying! You can do ByteConverter next time"); }
    }
}