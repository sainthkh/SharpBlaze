namespace Lox;

public class Interpreter: Expr.IVisitor<object?>, Stmt.IVisitor<object?> {
    private Env _env = new();

    public void Interpret(List<Stmt?> statements) {
        try {
            foreach (var statement in statements) {
                Execute(statement);
            }
        } catch (RuntimeError error) {
            Lox.RuntimeError(error);
        }
    }

    private object? Evaluate(Expr expr) {
        return expr.Accept(this);
    }

    public object? VisitAssignExpr(Expr.Assign expr) {
        var value = Evaluate(expr.Value);
        _env.Assign(expr.Name, value);
        return value;
    }

    public object? VisitBinaryExpr(Expr.Binary expr) {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

#pragma warning disable CS8604 // Possible null reference argument.
        switch (expr.Op.Type) {
            case TokenType.MINUS:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left - (double)right;
            case TokenType.SLASH:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left / (double)right;
            case TokenType.STAR:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left * (double)right;
            case TokenType.PLUS:
                if (left is double && right is double) {
                    return (double)left + (double)right;
                }
                if (left is string && right is string) {
                    return (string)left + (string)right;
                }
                throw new RuntimeError(expr.Op, "Operands must be two numbers or two strings.");
            case TokenType.GREATER:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left >= (double)right;
            case TokenType.LESS:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left < (double)right;
            case TokenType.LESS_EQUAL:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left <= (double)right;
            case TokenType.BANG_EQUAL: return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
        }
#pragma warning restore CS8604 // Possible null reference argument.

        return null;
    }

    public object? VisitGroupingExpr(Expr.Grouping expr) {
        return Evaluate(expr.Expression);
    }

    public object? VisitLiteralExpr(Expr.Literal expr) {
        return expr.Value;
    }

    public object? VisitUnaryExpr(Expr.Unary expr) {
        var right = Evaluate(expr.Right);

        switch (expr.Op.Type) {
            case TokenType.MINUS:
                CheckNumberOperand(expr.Op, right);
#pragma warning disable CS8605 // Unboxing a possibly null value.
                return -(double)right;
#pragma warning restore CS8605 // Unboxing a possibly null value.
            case TokenType.BANG:
                return !IsTruthy(right);
        }

        return null;
    }

    public object? VisitVariableExpr(Expr.Variable expr) {
        return _env.Get(expr.Name);
    }

    private void CheckNumberOperand(Token op, object? operand) {
        if (operand is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    private void CheckNumberOperands(Token op, object left, object right) {
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operands must be numbers.");
    }

    private bool IsEqual(object? a, object? b) {
        if (a == null && b == null) return true;
        if (a == null) return false;
        return a.Equals(b);
    }

    private bool IsTruthy(object? obj) {
        if (obj == null) return false;
        if (obj is bool) return (bool)obj;
        return true;
    }

    private string Stringify(object? obj) {
        if (obj == null) return "nil";
        if (obj is double) {
            var text = obj.ToString();
            if (text != null && text.EndsWith(".0")) {
                text = text.Substring(0, text.Length - 2);
            }
            return text ?? "";
        }
        return obj.ToString() ?? "";
    }

    public object? VisitBlockStmt(Stmt.Block stmt) {
        ExecuteBlock(stmt.Statements, new Env(_env));
        return null;
    }

    public object? VisitExpressionStmt(Stmt.Expression stmt) {
        Evaluate(stmt.Expr);
        return null;
    }

    public object? VisitPrintStmt(Stmt.Print stmt) {
        var value = Evaluate(stmt.Expr);
        Console.WriteLine(Stringify(value));
        return null;
    }

    public object? VisitVarStmt(Stmt.Var stmt) {
        object? value = null;
        if (stmt.Initializer != null) {
            value = Evaluate(stmt.Initializer);
        }

        _env.Define(stmt.Name.Lexeme, value);
        return null;
    }

    private void ExecuteBlock(List<Stmt?> statements, Env environment) {
        var previous = _env;
        try {
            _env = environment;
            foreach (var statement in statements) {
                Execute(statement);
            }
        } finally {
            _env = previous;
        }
    }

    private void Execute(Stmt? stmt) {
        if (stmt != null) stmt.Accept(this);
    }
}

public class RuntimeError: Exception {
    public Token Token { get; }
    public override string Message { get; }

    public RuntimeError(Token token, string message) {
        Token = token;
        Message = message;
    }
}
