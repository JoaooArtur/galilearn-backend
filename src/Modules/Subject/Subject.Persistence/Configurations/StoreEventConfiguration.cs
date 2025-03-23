namespace Subject.Persistence.Configurations
{
    using Core.Persistence.Configurations;
    using Subject.Domain.Aggregates;

    public class StoreEventConfiguration
    {
        public class StudentStoreEventConfiguration : StoreEventConfiguration<Subject>;
    }
}
