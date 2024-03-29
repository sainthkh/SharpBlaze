using System.Text;

namespace Lox;

public class AstPrinter: Expr.IVisitor<string> {
    public string Print(Expr? expr) {
        if (expr == null) return "parse result is null.";

        return expr.Accept(this);
    }

    public string VisitAssignExpr(Expr.Assign expr) {
        return Parenthesize2("=", expr.Name.Lexeme, expr.Value);
    }

    public string VisitBinaryExpr(Expr.Binary expr) {
        return Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr) {
        return Parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr) {
        if (expr.Value == null) return "nil";
        return expr.Value.ToString() ?? "";
    }

    public string VisitLogicalExpr(Expr.Logical expr) {
        return Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
    }

    public string VisitUnaryExpr(Expr.Unary expr) {
        return Parenthesize(expr.Op.Lexeme, expr.Right);
    }

    public string VisitVariableExpr(Expr.Variable expr) {
        return expr.Name.Lexeme;
    }

    private string Parenthesize(string name, params Expr[] exprs) {
        var builder = new StringBuilder();
        
        builder.Append("(").Append(name);
        
        foreach (var expr in exprs) {
            builder.Append(" ");
            builder.Append(expr.Accept(this));
        }
        
        builder.Append(")");
        
        return builder.ToString();
    }

    private string Parenthesize2(string name, string left, Expr right) {
        var builder = new StringBuilder();
        
        builder.Append("(").Append(name).Append(" ");
        builder.Append(left).Append(" ");
        builder.Append(right.Accept(this));
        builder.Append(")");
        
        return builder.ToString();
    }
}
