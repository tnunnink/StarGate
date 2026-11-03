using System;
using NUnit.Framework;

namespace StarGate.Galaxy.Tests;

public static class DefaultDomain
{
    public static void Require()
    {
        if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
            Assert.Ignore("Requires the default AppDomain. Run the test exe with --where \"cat == DefaultDomain\".");
    }
}