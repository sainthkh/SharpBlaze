using System.Data;

public enum TokenType {
    // Single-character tokens.
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

    // One or two character tokens.
    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,

    // Literals.
    IDENTIFIER, STRING, NUMBER,

    // Keywords.
    AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
    PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

    EOF
}

public class Token {
    private readonly TokenType type;
    private readonly string lexeme;
    private readonly object? literal;
    private readonly int line;

    public TokenType Type { get => type; }
    public string Lexeme { get => lexeme; }
    public object? Literal { get => literal; }
    public int Line { get => line; }

    public Token(TokenType type, string lexeme, object? literal, int line) {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }

    public override string ToString() {
        return $"{type} {lexeme} {literal}";
    }

}

public class Scanner {
    private readonly string source;
    private List<Token> tokens = new List<Token>();
    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
        { "and", TokenType.AND },
        { "class", TokenType.CLASS },
        { "else", TokenType.ELSE },
        { "false", TokenType.FALSE },
        { "for", TokenType.FOR },
        { "fun", TokenType.FUN },
        { "if", TokenType.IF },
        { "nil", TokenType.NIL },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "super", TokenType.SUPER },
        { "this", TokenType.THIS },
        { "true", TokenType.TRUE },
        { "var", TokenType.VAR },
        { "while", TokenType.WHILE }
    };
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source) {
        this.source = source;
    }

    public List<Token> ScanTokens() {
        while (!IsAtEnd()) {
            start = current;
            ScanToken();
            // tokens.Add();
        }
        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private void ScanToken() {
        char c = Advance();
        switch (c) {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/':
                if (Match('/')) {
                    while (Peek() != '\n' && !IsAtEnd()) {
                        Advance();
                    }
                } else {
                    AddToken(TokenType.SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                line++;
                break;
            case '"': String(); break;
            default:
                if (IsDigit(c)) {
                    Number();
                } else if (IsAlpha(c)) {
                    Identifier();
                } else {
                    Lox.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    private bool IsAtEnd() {
        return current >= source.Length;
    }

    private char Advance() {
        current++;
        return source[current - 1];
    }

    private void AddToken(TokenType type) {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object? literal) {
        string text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line));
    }

    private bool Match(char expected) {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private char Peek() {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private void String() {
        while (Peek() != '"' && !IsAtEnd()) {
            if (Peek() == '\n') line++;
            Advance();
        }

        if (IsAtEnd()) {
            Lox.Error(line, "Unterminated string.");
            return;
        }

        Advance();

        string value = source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.STRING, value);
    }

    private bool IsDigit(char c) {
        return c >= '0' && c <= '9';
    }

    private void Number() {
        while (IsDigit(Peek())) {
            Advance();
        }

        if (Peek() == '.' && IsDigit(PeekNext())) {
            Advance();
            while (IsDigit(Peek())) {
                Advance();
            }
        }

        AddToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start)));
    }

    private char PeekNext() {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private bool IsAlpha(char c) {
        return (c >= 'a' && c <= 'z') ||
               (c >= 'A' && c <= 'Z') ||
               c == '_';
    }

    private void Identifier() {
        while (IsAlphaNumeric(Peek())) {
            Advance();
        }

        string text = source.Substring(start, current - start);
        TokenType type;
        if (!keywords.TryGetValue(text, out type)) {
            type = TokenType.IDENTIFIER;
        }
        AddToken(type);
    }

    private bool IsAlphaNumeric(char c) {
        return IsAlpha(c) || IsDigit(c);
    }
}
