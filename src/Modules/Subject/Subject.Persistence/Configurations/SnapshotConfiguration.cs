using Core.Persistence.Configurations;

namespace Subject.Persistence.Configurations
{
    using Subject.Domain.Aggregates;

    public class SnapshotConfiguration
    {
        public class StudentSnapshotConfiguration : SnapshotConfiguration<Subject>;
    }
}
