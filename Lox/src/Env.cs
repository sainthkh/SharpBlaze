namespace Lox;

public class Env {
    public Env? Enclosing { get; set; }
    private readonly Dictionary<string, object?> values = new();

    public Env() {
        Enclosing = null;
    }

    public Env(Env enclosing) {
        Enclosing = enclosing;
    }

    public void Define(string name, object? value) {
        values[name] = value;
    }

    public object? Get(Token name) {
        if (values.TryGetValue(name.Lexeme, out var value)) {
            return value;
        }

        if (Enclosing != null) return Enclosing.Get(name);

        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Assign(Token name, object? value) {
        if (values.ContainsKey(name.Lexeme)) {
            values[name.Lexeme] = value;
            return;
        }

        if (Enclosing != null) {
            Enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }
}