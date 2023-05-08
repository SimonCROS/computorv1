package fr.simoncros.computorv1.lexer;

import java.text.NumberFormat;
import java.util.Locale;

public class Token {
    public TokenType type;
    public String identifier = null;
    public float number = 0;

    public Token(TokenType type) {
        this.type = type;
    }

    public static Token newIdentifier(String value) {
        Token token = new Token(TokenType.IDENTIFIER);
        token.identifier = value;
        return token;
    }

    public static Token newNumber(float value) {
        Token token = new Token(TokenType.NUMBER);
        token.number = value;
        return token;
    }

    @Override
    public String toString() {
        return switch (this.type) {
            case ADD -> "+";
            case SUB -> "-";
            case MUL -> "*";
            case DIV -> "/";
            case POW -> "^";
            case EQUAL -> "=";
            case LPARENT -> "(";
            case RPARENT -> ")";
            case SPACE -> " ";
            case IDENTIFIER -> this.identifier;
            case NUMBER -> NumberFormat.getNumberInstance(Locale.ENGLISH).format(number);
            case EOF -> "";
        };
    }
}
