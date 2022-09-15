namespace Ara;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length >= 1) 
            return Cli.Run(args[0]);
        
        Console.WriteLine("Usage: ara file");
        return 1;
    }
}