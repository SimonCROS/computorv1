package fr.simoncros.computorv1.lexer;

public enum Token {
    ADD,
    SUB,
    DIV,
    MUL,
    POW,
    EQUAL,
    LPARENT,
    RPARENT,
    IDENTIFIER,
    NUMBER,
    EOF;

	public String identifier = null;
	public float number = 0;

	public static Token newIdentifier(String value) {
        Token token = Token.IDENTIFIER;
        token.identifier = value;
        return token;
	}

	public static Token newNumber(float value) {
        Token token = Token.IDENTIFIER;
        token.number = value;
        return token;
	}
}
