using System.Globalization;
using computorv1;
using computorv1.Tokens;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

if (args.Length != 1)
{
    Console.WriteLine("Usage: dotnet run <equation>");
    return 1;
}

if (new Lexer(args[0]).Tokenize(out List<Token> tokens))
{
    foreach (Token token in tokens)
    {
        Console.WriteLine(token);
    }
}
else
{
    return 1;
}

return 0;
