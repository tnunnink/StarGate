using System;
using NUnitLite;

namespace StarGate.Galaxy.Tests;

public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        return new AutoRun().Execute(args);
    }
}