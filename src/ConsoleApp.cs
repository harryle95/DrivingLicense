namespace src;

class ConsoleApp(QuestionManager manager)
{
    private readonly QuestionManager manager = manager;
    public void SelectMode()
    {
        string? choice;
        while (true)
        {
            Console.WriteLine($"Select question mode: A - review with all questions, F - review with current focused questions, R - review incorrect questions, Q - exit the program");
            choice = Console.ReadLine();
            if (choice == "A" || choice == "a")
            {
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

    public void RunLoop()
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

    public static void Start(string filePath)
    {
        QuestionParser parser = new(filePath);
        QuestionManager manager = new(parser.Questions);
        ConsoleApp app = new(manager);
        while (true)
        {
            app.RunLoop();
            app.SelectMode();
        }
    }
}