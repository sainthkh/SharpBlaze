namespace Lox;

public class Lox {
    private static bool HadError = false;

    public static void Report(int line, string where, string message) {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        HadError = true;
    }

    public static void Error(int line, string message) {
        Report(line, "", message);
    }
}
