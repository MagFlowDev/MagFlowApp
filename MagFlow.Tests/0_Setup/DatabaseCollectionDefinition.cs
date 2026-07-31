using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Tests._0_Setup
{
    [CollectionDefinition("DatabaseSequentialTests", DisableParallelization = true)]
    public class DatabaseCollectionDefinition : ICollectionFixture<TestDatabaseFixture>
    {
    }
}
