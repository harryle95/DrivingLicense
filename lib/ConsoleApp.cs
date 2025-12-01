using lib.Models;
using lib.Services;

namespace lib;

class ConsoleApp
{
    public static void SelectMode()
    {
        string? choice;
        while (true)
        {
            Console.WriteLine($"Select question mode: A - review with all questions, F - review with current focused questions, R - review incorrect questions, Q - exit the program");
            choice = Console.ReadLine();
            if (choice == "A" || choice == "a")
            {
                QuestionService.RestartAllQuestions();
                break;
            }
            if (choice == "F" || choice == "f")
            {
                QuestionService.RestartCurrentSession();
                break;
            }
            if (choice == "R" || choice == "r")
            {
                QuestionService.RestartReviewSession();
                break;
            }
            if (choice == "Q" || choice == "q")
            {
                Environment.Exit(0);
            }
            Console.WriteLine("Invalid selection choice!");
        }
    }

    public static void RunLoop()
    {
        List<QuestionGetDTO> questions = [.. QuestionService.GetQuestions()];
        List<int> reviewIndices = [];
        foreach (var question in questions)
        {
            Console.Clear();
            Console.WriteLine($"Question: {question.Header}");
            Console.WriteLine(question.Body);
            Console.ReadLine();
            Console.WriteLine(question.Answer);
            Console.WriteLine("\nPress r to add to review list or any key to move to the next question!");
            string? key = Console.ReadLine();
            if (key == "r" || key == "R")
            {
                reviewIndices.Add(question.Id);
                continue;
            }
            if (key == "q" || key == "Q")
                Environment.Exit(0);
        }
        QuestionService.MarkForReview([.. reviewIndices]);
    }

    public static void Start(string filePath)
    {
        ParseService parser = new(filePath);
        QuestionService.UpdateQuestions(parser.Questions);
        Console.WriteLine("Finished updating questions");
        while (true)
        {
            ConsoleApp.RunLoop();
            ConsoleApp.SelectMode();
        }
    }
}