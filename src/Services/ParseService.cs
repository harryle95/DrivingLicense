using src.Models;
namespace src.Services;

public class ValidationException : Exception
{
    public ValidationException() { }
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}

public class ParseException : Exception
{
    public ParseException() { }
    public ParseException(string message) : base(message) { }
    public ParseException(string message, Exception innerException) : base(message, innerException) { }
}

public class QuestionBuilder
{
    private string _header = string.Empty;
    private readonly List<string> _body = [];
    private readonly List<string> _answer = [];

    public void Reset()
    {
        _header = string.Empty;
        _body.Clear();
        _answer.Clear();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(_header))
            throw new ValidationException("Empty header");
        if (_answer.Count == 0)
            throw new ValidationException("Empty answer");
    }

    public Question Build()
    {
        Validate();
        string body = _body.Count != 0 ? string.Join("\n", _body) : string.Empty;
        string answer = string.Join("\n", _answer);
        return new Question(_header, body, answer);
    }

    public void SetHeader(string header)
    {
        if (!header.StartsWith("###"))
            throw new ValidationException("Question header must begin with ###: " + header);
        _header = header[3..].Trim();
    }

    public void AddBody(string body)
    {
        _body.Add(body.Trim());
    }

    public void AddAnswer(string answer)
    {
        _answer.Add(answer.Trim());
    }
}

public class ParseService
{
    private readonly int _numParsed = 0;
    public List<Question> Questions { get; } = [];
    private readonly int _lineNumber = 0;
    private readonly string _filePath = string.Empty;
    private string GetErrorMessage()
    {
        return $"{_filePath}:{_lineNumber}";
    }
    public ParseService(string filePath)
    {
        _lineNumber = -1;
        _filePath = filePath;
        try
        {
            using StreamReader sr = new(_filePath);
            string? line;

            bool parsingAnswer = false;
            QuestionBuilder builder = new();
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                _lineNumber++;
                if (string.IsNullOrEmpty(line))
                    continue;
                else if (line.StartsWith("---") || line.StartsWith("___") || line.StartsWith("***"))
                    parsingAnswer = true;
                else if (line.StartsWith("###"))
                {
                    parsingAnswer = false;
                    if (_numParsed != 0)
                        Questions.Add(builder.Build());
                    builder.Reset();
                    builder.SetHeader(line);
                    _numParsed++;
                }
                else
                {
                    if (parsingAnswer)
                        builder.AddAnswer(line);
                    else
                        builder.AddBody(line);
                }

            }
            if (_numParsed != 0)
            {
                Questions.Add(builder.Build());
                _numParsed++;
            }
        }
        catch (ValidationException valException)
        {
            throw new ParseException(GetErrorMessage(), valException);
        }
    }
}