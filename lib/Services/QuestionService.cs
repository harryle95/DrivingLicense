using lib.Models;
namespace lib.Services;

public static class QuestionService
{
    public static List<Question> Questions = [];
    private static List<int> FocusIndices = [];
    private static readonly HashSet<int> ReviewIndices = [];

    public static void ImportMarkdown(string filePath)
    {
        ParseService parser = new(filePath);
        UpdateQuestions(parser.Questions);
    }

    public static void UpdateQuestions(IEnumerable<Question> questions)
    {
        Questions = [..questions];
        RestartAllQuestions();
    }

    public static void RestartCurrentSession()
    {
        Reset();
    }

    public static void RestartReviewSession()
    {
        FocusIndices = [.. ReviewIndices];
        Reset();
    }

    public static void RestartAllQuestions()
    {
        FocusIndices = [.. Enumerable.Range(0, Questions.Count)];
        Reset();
    }

    public static void Reset()
    {
        ReviewIndices.Clear();
        Shuffle();
    }

    public static void Shuffle()
    {
        Random rng = new();
        int n = FocusIndices.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (FocusIndices[n], FocusIndices[k]) = (FocusIndices[k], FocusIndices[n]);
        }
    }

    public static IEnumerable<QuestionGetDTO> GetQuestions()
    {
        return FocusIndices.Select(index =>
        {
            Question question = Questions[index];
            return new QuestionGetDTO(index, question.Header, question.Body, question.Answer);
        });
    }

    public static void MarkForReview(IEnumerable<int> indices)
    {
        foreach (var index in indices)
            ReviewIndices.Add(index);
    }
}
