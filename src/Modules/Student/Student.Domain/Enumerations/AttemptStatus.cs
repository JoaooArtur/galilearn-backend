using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Student.Domain.Enumerations
{
    [method: JsonConstructor]
    public class AttemptStatus(string name, int value) : SmartEnum<AttemptStatus>(name, value)
    {
        public static readonly AttemptStatus Pending = new PendingStatus();
        public static readonly AttemptStatus Finished = new FinishedStatus();

        public static implicit operator AttemptStatus(string name)
            => FromName(name);

        public static implicit operator AttemptStatus(int value)
            => FromValue(value);

        public static implicit operator string(AttemptStatus status)
            => status.Name;

        public static implicit operator int(AttemptStatus status)
            => status.Value;

        public class PendingStatus() : AttemptStatus(nameof(Pending), 0) { }
        public class FinishedStatus() : AttemptStatus(nameof(Finished), 1) { }
    }
}
