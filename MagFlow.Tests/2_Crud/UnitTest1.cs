using MagFlow.Tests._0_Setup;

namespace MagFlow.Tests._2_Crud
{
    public class UnitTest1 : BaseMagFlowTest
    {
        public UnitTest1(TestDatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }
    }
}
