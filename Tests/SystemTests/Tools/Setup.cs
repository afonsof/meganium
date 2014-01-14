using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Meganium.SystemTests.Tools
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
