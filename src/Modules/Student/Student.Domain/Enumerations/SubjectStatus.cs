using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Student.Domain.Enumerations
{
    [method: JsonConstructor]
    public class SubjectStatus(string name, int value) : SmartEnum<SubjectStatus>(name, value)
    {
        public static readonly SubjectStatus Pending = new PendingStatus();
        public static readonly SubjectStatus InProgress = new InProgressStatus();
        public static readonly SubjectStatus Finished = new FinishedStatus();

        public static implicit operator SubjectStatus(string name)
            => FromName(name);

        public static implicit operator SubjectStatus(int value)
            => FromValue(value);

        public static implicit operator string(SubjectStatus status)
            => status.Name;

        public static implicit operator int(SubjectStatus status)
            => status.Value;

        public class PendingStatus() : SubjectStatus(nameof(Pending), 0) { }
        public class InProgressStatus() : SubjectStatus(nameof(InProgress), 1) { }
        public class FinishedStatus() : SubjectStatus(nameof(Finished), 2) { }
    }
}
