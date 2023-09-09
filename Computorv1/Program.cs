using System.Globalization;
using Computorv1;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

if (args.Length != 1)
{
    Console.WriteLine("Usage: dotnet run <equation>");
    return 1;
}

return Solver.Solve(args[0]);
