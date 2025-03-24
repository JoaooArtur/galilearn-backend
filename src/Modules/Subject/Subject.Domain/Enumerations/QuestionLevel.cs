using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Subject.Domain.Enumerations
{
    [method: JsonConstructor]
    public class QuestionLevel(string name, int value) : SmartEnum<QuestionLevel>(name, value)
    {
        public static readonly QuestionLevel Introduction = new IntroductionLevel();
        public static readonly QuestionLevel Easy = new EasyLevel();
        public static readonly QuestionLevel Medium = new MediumLevel();
        public static readonly QuestionLevel Hard = new HardLevel();
        public static readonly QuestionLevel Advanced = new AdvancedLevel();

        public static implicit operator QuestionLevel(string name)
            => FromName(name);

        public static implicit operator QuestionLevel(int value)
            => FromValue(value);

        public static implicit operator string(QuestionLevel level)
            => level.Name;

        public static implicit operator int(QuestionLevel level)
            => level.Value;

        public class IntroductionLevel() : QuestionLevel(nameof(Introduction), 1) { }
        public class EasyLevel() : QuestionLevel(nameof(Easy), 2) { }
        public class MediumLevel() : QuestionLevel(nameof(Medium), 3) { }
        public class HardLevel() : QuestionLevel(nameof(Hard), 4) { }
        public class AdvancedLevel() : QuestionLevel(nameof(Advanced), 5) { }
    }
}
