package fr.simoncros.computorv1;

import java.util.List;
import java.util.stream.Collectors;

import fr.simoncros.computorv1.lexer.Lexer;
import fr.simoncros.computorv1.lexer.Token;
import fr.simoncros.computorv1.utils.Result;

public class App {
    public static void main(String[] args) throws Exception {
        if (args.length != 1) {
            System.err.println("Usage: ./computorv1 <formula>");
            System.exit(1);
        }
        Result<List<Token>, String> tokens = new Lexer(args[0]).tokenize();
        if (tokens.isOk()) {
            System.out.println(tokens.ok().get().stream().map(Token::toString).collect(Collectors.joining("")));
        } else {
            System.err.println(tokens.err().get());
        }
    }
}
