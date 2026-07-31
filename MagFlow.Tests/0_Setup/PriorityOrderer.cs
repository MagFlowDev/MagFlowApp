using Xunit.Sdk;
using Xunit.v3;

namespace MagFlow.Tests._0_Setup
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; }
        public PriorityAttribute(int priority) => Priority = priority;
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            return testCases.OrderBy(testCase =>
            {
                if (testCase is IXunitTestCase xunitTestCase)
                {
                    var priorityAttr = xunitTestCase.TestMethod.Method
                        .GetCustomAttributes(typeof(PriorityAttribute), inherit: true)
                        .FirstOrDefault();

                    if (priorityAttr is PriorityAttribute priorityAttribute)
                    {
                        return priorityAttribute.Priority;
                    }
                }
                return int.MaxValue;
            }).ToList();
        }
    }
}
