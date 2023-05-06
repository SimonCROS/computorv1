package fr.simoncros.computorv1.lexer;

import java.text.CharacterIterator;
import java.text.StringCharacterIterator;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.concurrent.atomic.AtomicReference;

import fr.simoncros.computorv1.utils.Result;
import static fr.simoncros.computorv1.utils.Result.ok;
import static fr.simoncros.computorv1.utils.Result.err;

public class Lexer {
	CharacterIterator chars;

	public Lexer(String expression) {
		this.chars = new StringCharacterIterator(expression);
	}

	public List<Token> tokenize() {
		List<Token> tokens = new ArrayList<Token>();

		while (true) {
			Token token = this.next_token();
			if (token == Token.EOF)
				break;
			else
				tokens.add(token);
		}
		return tokens;
	}

	public Result<Token, String> next_token() {
		Token token;

		while (Character.isSpaceChar(this.chars.current())) {
			this.chars.next();
		}
		char current = this.chars.current();
		switch (current) {
			case '+' -> token = Token.ADD;
			case '-' -> token = Token.SUB;
			case '*' -> token = Token.MUL;
			case '/' -> token = Token.DIV;
			case '^' -> token = Token.POW;
			case '=' -> token = Token.EQUAL;
			case '(' -> token = Token.LPARENT;
			case ')' -> token = Token.RPARENT;
			case '\0' -> token = Token.EOF;
			default:
				Optional<String> identifier = this.readIdentifier();
				if (identifier.isPresent()) {
					token = Token.newIdentifier(identifier.get());
				} else {
					Optional<Float> number = this.readNumber();
					if (number.isPresent()) {
						token = Token.newNumber(number.get());
					} else {
						return err(String.format("Unknown token %c", current));
					}
				}
				break;
		}
		this.chars.next();
		return ok(token);
	}

	public 

//     fn read_identifier(&mut self) -> Option<String> {
//         let mut ret: String = String::new();
//         loop {
//             match self.chars.next_if(|&c| c.is_alphabetic() || c == '_') {
//                 Some(c) => ret.push(c),
//                 None => break,
//             }
//         }
//         Some(ret).filter(|s| !s.is_empty())
//     }

//     fn read_number(&mut self) -> Option<Result<f32, String>> {
//         let mut ret: String = String::new();
//         loop {
//             match self.chars.next_if(|&c| c.is_ascii_digit() || c == '.') {
//                 Some(c) => ret.push(c),
//                 None => break,
//             }
//         }
//         let tmp = ret.clone();
//         Some(ret)
//             .filter(|s| !s.is_empty())
//             .map(|s| s.parse::<f32>().map_err(|_| tmp))
//     }
}
