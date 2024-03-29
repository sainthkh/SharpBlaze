// This code is generated by GenerateAst.cs. Do not modify it directly.
namespace Lox;

public abstract class Expr {
    public interface IVisitor<T> {
        T VisitAssignExpr(Assign expr);
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitLogicalExpr(Logical expr);
        T VisitUnaryExpr(Unary expr);
        T VisitVariableExpr(Variable expr);
    }
    public class Assign : Expr {
        public Token Name { get; private set; }
        public Expr Value { get; private set; }

        public Assign(Token name, Expr value) {
            Name = name;
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitAssignExpr(this);
        }
    }
    public class Binary : Expr {
        public Expr Left { get; private set; }
        public Token Op { get; private set; }
        public Expr Right { get; private set; }

        public Binary(Expr left, Token op, Expr right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitBinaryExpr(this);
        }
    }
    public class Grouping : Expr {
        public Expr Expression { get; private set; }

        public Grouping(Expr expression) {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitGroupingExpr(this);
        }
    }
    public class Literal : Expr {
        public object? Value { get; private set; }

        public Literal(object? value) {
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLiteralExpr(this);
        }
    }
    public class Logical : Expr {
        public Expr Left { get; private set; }
        public Token Op { get; private set; }
        public Expr Right { get; private set; }

        public Logical(Expr left, Token op, Expr right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLogicalExpr(this);
        }
    }
    public class Unary : Expr {
        public Token Op { get; private set; }
        public Expr Right { get; private set; }

        public Unary(Token op, Expr right) {
            Op = op;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitUnaryExpr(this);
        }
    }
    public class Variable : Expr {
        public Token Name { get; private set; }

        public Variable(Token name) {
            Name = name;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitVariableExpr(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
