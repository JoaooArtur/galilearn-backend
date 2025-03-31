using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Student.Domain.Enumerations
{
    [method: JsonConstructor]
    public class LessonStatus(string name, int value) : SmartEnum<LessonStatus>(name, value)
    {
        public static readonly LessonStatus Pending = new PendingStatus();
        public static readonly LessonStatus InProgress = new InProgressStatus();
        public static readonly LessonStatus Finished = new FinishedStatus();

        public static implicit operator LessonStatus(string name)
            => FromName(name);

        public static implicit operator LessonStatus(int value)
            => FromValue(value);

        public static implicit operator string(LessonStatus status)
            => status.Name;

        public static implicit operator int(LessonStatus status)
            => status.Value;

        public class PendingStatus() : LessonStatus(nameof(Pending), 0) { }
        public class InProgressStatus() : LessonStatus(nameof(InProgress), 1) { }
        public class FinishedStatus() : LessonStatus(nameof(Finished), 2) { }
    }
}
