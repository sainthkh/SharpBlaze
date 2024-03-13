namespace Lox;

public class Lox {
    public static bool HadError { get; private set; }
    public static bool HadRuntimeError { get; private set; }

    public static void Report(int line, string where, string message) {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        HadError = true;
    }

    public static void Report(int line, string message) {
        Report(line, "", message);
    }

    public static void Error(int line, string message) {
        Report(line, "", message);
    }

    public static void Error(Token token, string message) {
        if (token.Type == TokenType.EOF) {
            Report(token.Line, " at end", message);
        } else {
            Report(token.Line, $" at '{token.Lexeme}'", message);
        }
    }

    public static void RuntimeError(RuntimeError error) {
        Console.Error.WriteLine($"[line {error.Token.Line}] {error.Message}");
        HadRuntimeError = true;
    }
}
