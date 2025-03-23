using Core.Persistence.Configurations;

namespace Student.Persistence.Configurations
{
    using Student.Domain.Aggregates;

    public class SnapshotConfiguration
    {
        public class StudentSnapshotConfiguration : SnapshotConfiguration<Student>;
    }
}
