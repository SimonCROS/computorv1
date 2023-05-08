package fr.simoncros.computorv1.lexer;

import static fr.simoncros.computorv1.utils.Result.err;
import static fr.simoncros.computorv1.utils.Result.ok;

import java.text.CharacterIterator;
import java.text.StringCharacterIterator;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.function.Predicate;

import fr.simoncros.computorv1.utils.Result;

public class Lexer {
	CharacterIterator chars;

	public Lexer(String expression) {
		this.chars = new StringCharacterIterator(expression);
	}

	public Result<List<Token>, String> tokenize() {
		List<Token> tokens = new ArrayList<Token>();

		while (true) {
			Result<Token, String> token = this.next_token();
			if (token.isOk()) {
				if (token.ok().get().type == TokenType.EOF)
					break;
				else
					tokens.add(token.ok().get());
			} else {
				return err(token.err().get());
			}
		}
		return ok(tokens);
	}

	public Result<Token, String> next_token() {
		TokenType tokenType;

		char current = this.chars.current();
		switch (current) {
			case '+' -> tokenType = TokenType.ADD;
			case '-' -> tokenType = TokenType.SUB;
			case '*' -> tokenType = TokenType.MUL;
			case '/' -> tokenType = TokenType.DIV;
			case '^' -> tokenType = TokenType.POW;
			case '=' -> tokenType = TokenType.EQUAL;
			case '(' -> tokenType = TokenType.LPARENT;
			case ')' -> tokenType = TokenType.RPARENT;
			case CharacterIterator.DONE -> tokenType = TokenType.EOF;
			default -> {
				if (this.readSpaces()) {
					return ok(new Token(TokenType.SPACE));
				} else {
					Optional<String> identifier = this.readIdentifier();
					if (identifier.isPresent()) {
						return ok(Token.newIdentifier(identifier.get()));
					} else {
						Optional<Float> number = this.readNumber();
						if (number.isPresent()) {
							return ok(Token.newNumber(number.get()));
						} else {
							return err(String.format("Unknown token %c", current));
						}
					}
				}
			}
		}
		this.chars.next();
		return ok(new Token(tokenType));
	}

	public boolean readSpaces() {
		char current = this.chars.current();

		if (!Character.isSpaceChar(current)) {
			return false;
		}
		while (Character.isSpaceChar(current)) {
			current = this.chars.next();
		}
		return true;
	}

	public Optional<String> readIdentifier() {
		String result = new String();

		char current = this.chars.current();
		while (true) {
			if (Character.isLetter(current) || current == '_') {
				result += current;
				current = this.chars.next();
			} else {
				break;
			}
		}
		return Optional.of(result).filter(Predicate.not(String::isEmpty));
	}

	public Optional<Float> readNumber() {
		String result = new String();

		char current = this.chars.current();
		while (true) {
			if (Character.isDigit(current) || current == '.') {
				result += current;
				current = this.chars.next();
			} else {
				break;
			}
		}
		try {
			return Optional.of(Float.parseFloat(result));
		} catch (Exception ex) {
			return Optional.empty();
		}
	}
}
