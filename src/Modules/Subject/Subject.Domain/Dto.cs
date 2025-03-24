namespace Subject.Domain
{
    public static class Dto
    {
        public record AnswerOption(
            Guid Id,
            string Text,
            bool RightAnswer = false);
    }
}
