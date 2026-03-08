using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1, 101);
        int guess = -1;
        int guessCount = 0;

        // Loop until the user guesses the magic number
        do
        {
            // Ask for user guess
            Console.Write("What is your guess? ");
            guess = int.Parse(Console.ReadLine());
            guessCount++;

            // Determine if the user needs to guess higher or lower
            if (guess < magicNumber)
            {
                Console.WriteLine("Higher");
            }
            else if (guess > magicNumber)
            {
                Console.WriteLine("Lower");
            }
            else
            {
                Console.WriteLine("You guessed it!");
            }
        } while (guess != magicNumber);

        // Display the number of guesses
        Console.WriteLine($"It took you {guessCount} guesses to find the magic number!");
    }
}
