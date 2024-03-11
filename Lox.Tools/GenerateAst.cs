using System.IO;

namespace Lox.Tools;

public class GenerateAst {
    public static void DefineAst(string outputDir, string baseName, List<string> types) {
        var path = Path.Combine(outputDir, $"{baseName}.cs");
        using var writer = new StreamWriter(path);

        writer.WriteLine("// This code is generated by GenerateAst.cs. Do not modify it directly.");
        writer.WriteLine("namespace Lox;");
        writer.WriteLine();
        writer.WriteLine("public abstract class " + baseName + " {");

        DefineVisitor(writer, baseName, types);

        foreach (var type in types) {
            var className = type.Split(":")[0].Trim();
            var fields = type.Split(":")[1].Trim();
            DefineType(writer, baseName, className, fields);
        }

        writer.WriteLine("}");
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types) {
        writer.WriteLine("    public interface IVisitor<T> {");

        foreach (var type in types) {
            var typeName = type.Split(":")[0].Trim();
            writer.WriteLine($"        T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
        }

        writer.WriteLine("    }");
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList) {
        writer.WriteLine($"    public class {className} : {baseName} {{");

        var fields = fieldList.Split(", ").ToList();
        foreach (var field in fields) {
            var type = field.Split(" ")[0];
            var name = field.Split(" ")[1];
            writer.WriteLine($"        public {type} {UpperFirst(name)} {{ get; private set; }}");
        }

        writer.WriteLine();
        writer.WriteLine($"        public {className}({fieldList}) {{");
        foreach (var field in fields) {
            var name = field.Split(" ")[1];
            writer.WriteLine($"            {UpperFirst(name)} = {name};");
        }
        writer.WriteLine("        }");

        writer.WriteLine("    }");
    }

    private static string UpperFirst(string s) {
        return char.ToUpper(s[0]) + s[1..];
    }
}
