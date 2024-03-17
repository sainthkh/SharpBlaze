// See https://aka.ms/new-console-template for more information
using Lox.Tools;

if (args.Length != 1) {
    Console.Error.WriteLine("Usage: generate_ast <output directory>");
    Environment.Exit(1);
}
var outputDir = args[0];

GenerateAst.DefineAst(outputDir, "Expr", new List<string> {
    "Assign   : Token name, Expr value",
    "Binary   : Expr left, Token op, Expr right",
    "Grouping : Expr expression",
    "Literal  : object? value",
    "Unary    : Token op, Expr right",
    "Variable : Token name",
});

GenerateAst.DefineAst(outputDir, "Stmt", new List<string> {
    "Block      : List<Stmt?> statements",
    "Expression : Expr expr",
    "Print      : Expr expr",
    "Var        : Token name, Expr? initializer",
});
