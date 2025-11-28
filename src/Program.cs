// See https://aka.ms/new-console-template for more information
using src;

public class Program()
{
    
    public static void Main(string[] arguments)
    {
        if (arguments.Length == 0)
            throw new ArgumentException("Question file argument required.");
        if (arguments.Length != 1)
            throw new ArgumentException($"The program expects only one argument - path of question file. {arguments.Length} arguments received");
        var filePath = arguments[0];
        QuestionParser parser = new(filePath);
        List<Question> questions = parser.Questions;
        foreach (var q in questions){
            Console.WriteLine(q);
        }
    }
}
