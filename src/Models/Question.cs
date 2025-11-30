namespace src.Models;

using System;


public record Question(string Header, string Body, string Answer);

public record QuestionGetDTO(int Id, string Header, string Body, string Answer) : Question(Header, Body, Answer);
