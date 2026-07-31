using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

namespace MagFlow.Tests._0_Setup
{
    [CollectionDefinition("DatabaseSequentialTests", DisableParallelization = true)]
    public abstract class BaseMagFlowTest
    {
        protected readonly TestDatabaseFixture _fixture;

        protected ILogger Log { get; }

        protected BaseMagFlowTest(TestDatabaseFixture fixture)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
                    .AddXUnit(TestContext.Current.TestOutputHelper!);
            });

            Log = loggerFactory.CreateLogger(GetType());
            _fixture = fixture;

            if (!_fixture.IsSetupSuccessful)
            {
                Assert.Skip("Skipped: All tests withing 0_Setup must finish successfully.");
            }
        }

        protected void LogStep(string message)
        {
            var formattedMessage = $"[{DateTime.Now:HH:mm:ss.fff}] -> {message}";
            Log.LogInformation(formattedMessage);
        }
    }
}
