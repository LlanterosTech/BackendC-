namespace plantita.ProjectPlantita.communityandsupport.Domain.Entities;

public class AnswerForum
{
    public Guid AnswerId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTime AnsweredAt { get; set; }
    public bool IsBestAnswer { get; set; }
}