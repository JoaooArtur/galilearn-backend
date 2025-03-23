
namespace Student.Persistence.Configurations
{
    using Core.Persistence.Configurations;
    using Student.Domain.Aggregates;

    public class StoreEventConfiguration
    {
        public class StudentStoreEventConfiguration : StoreEventConfiguration<Student>;
    }
}
