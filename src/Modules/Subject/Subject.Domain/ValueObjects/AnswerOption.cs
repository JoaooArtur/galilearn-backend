
namespace Subject.Domain.ValueObjects
{
    public record AnswerOption(Guid OptionId, string Text, bool RightAnswer = false)
    {
        public static AnswerOption Create(Dto.AnswerOption dto)
            => new(dto.Id, dto.Text, dto.RightAnswer);

        public static implicit operator Dto.AnswerOption(AnswerOption answer)
            => new(answer.OptionId, answer.Text, answer.RightAnswer);

        public static bool operator ==(AnswerOption answer, Dto.AnswerOption dto)
            => dto == (Dto.AnswerOption)answer;

        public static bool operator !=(AnswerOption friend, Dto.AnswerOption dto)
            => dto != (Dto.AnswerOption)friend;

        public static AnswerOption Undefined
            => new(Guid.Empty, string.Empty);

        public static implicit operator AnswerOption(Dto.AnswerOption answer)
            => new(answer.Id, answer.Text, answer.RightAnswer);
    }
}
