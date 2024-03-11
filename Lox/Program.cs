using Lox;

// See https://aka.ms/new-console-template for more information
void RunFile(string path) {
    var source = File.ReadAllText(path);
    Run(source);
}

void RunPrompt() {
    while (true) {
        Console.Write("> ");
        var line = Console.ReadLine();
        Run(line ?? "");
    }
}

void Run(string source) {
    var scanner = new Scanner(source);
    var tokens = scanner.ScanTokens();
    
    foreach (var token in tokens) {
        Console.WriteLine(token);
    }
}

Console.WriteLine("Welcome to Lox!");

if (args.Length > 1) {
    Console.WriteLine("Usage: lox [script]");
    Environment.Exit(64);
} else if (args.Length == 1) {
    RunFile(args[0]);
} else {
    RunPrompt();
}
