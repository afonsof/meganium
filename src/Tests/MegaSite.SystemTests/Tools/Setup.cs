using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace MegaSite.SystemTests.Tools
{
    [SetUpFixture]
    [ExcludeFromCodeCoverage]
    public class Setup
    {
        [TearDown]
        public static void Cleanup()
        {
            TestToolkit.Dispose();
        }
    }
}
