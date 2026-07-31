using Xunit.Sdk;
using Xunit.v3;

[assembly: AssemblyFixture(typeof(MagFlow.Tests._0_Setup.TestDatabaseFixture))]
[assembly: TestCollectionOrderer(typeof(MagFlow.Tests._0_Setup.AlphabeticalCollectionOrderer))]
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]
[assembly: CaptureConsole]
[assembly: CaptureTrace]
namespace MagFlow.Tests._0_Setup
{
    public class AlphabeticalCollectionOrderer : ITestCollectionOrderer
    {
        public IReadOnlyCollection<TTestCollection> OrderTestCollections<TTestCollection>(IReadOnlyCollection<TTestCollection> testCollections) where TTestCollection : ITestCollection
        {
            return testCollections.OrderBy(c => c.TestCollectionDisplayName).ToList();
        }
    }
}
