// See https://aka.ms/new-console-template for more information
using src;

public class Program()
{
    public static void SelectMode(QuestionManager manager)
    {
        string? choice;
        while (true)
        {
            Console.WriteLine($"Select question mode: A - review with all questions, F - review with current focused questions, R - review incorrect questions, Q - exit the program");
            choice = Console.ReadLine();
            if (choice=="A" || choice == "a"){
                manager.RestartAllQuestions();
                break;
            }
            if (choice == "F" || choice == "f")
            {
                manager.RestartCurrentSession();
                break;
            }
            if (choice == "R" || choice == "r")
            {
                manager.RestartReviewSession();
                break;
            }
            if (choice == "Q" || choice == "q")
            {
                Environment.Exit(0);
            }
            Console.WriteLine("Invalid selection choice!");
        }
    }

    public static void RunQuestionLoop(QuestionManager manager)
    {
        while (manager.HasNext())
        {
            Console.Clear();
            Question question = manager.GetQuestion();
            Console.WriteLine($"Question: {question.Header}");
            Console.WriteLine(question.Body);
            Console.ReadLine();
            Console.WriteLine(question.Answer);
            Console.WriteLine("\nPress r to add to review list or any key to move to the next question!");
            string? key = Console.ReadLine();
            if (key == "r" || key == "R")
            {
                manager.MarkForReview();
                continue;
            }
            if (key == "q" || key == "Q")
                Environment.Exit(0);
                
        }
    }
    public static void Main(string[] arguments)
    {
        if (arguments.Length == 0)
            throw new ArgumentException("Question file argument required.");
        if (arguments.Length != 1)
            throw new ArgumentException($"The program expects only one argument - path of question file. {arguments.Length} arguments received");
        var filePath = arguments[0];
        QuestionParser parser = new(filePath);
        QuestionManager manager = new(parser.Questions);
        while (true)
        {
            RunQuestionLoop(manager);
            SelectMode(manager);
        }

    }
}
