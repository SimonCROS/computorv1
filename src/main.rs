use std::{
    env::{self, Args},
    process::exit,
};

use lexer::Lexer;
use parser::Parser;

mod lexer;
mod parser;

fn main() {
    let args: Args = env::args();
    if args.len() != 2 {
        eprintln!("Usage: ./computorv1 <formula>");
        exit(1);
    }
    let arg: String = args.last().unwrap();

    let lexer = Lexer::new(&arg);
    match lexer.tokenize() {
        Ok(tokens) => {
            let parser = Parser::new(tokens);
            match parser.parse() {
                Ok(root) => {
                    println!("{:?}", root);
                },
                Err(v) => {
                    eprintln!("{}", v);
                    exit(1);
                }
            }
        },
        Err(v) => {
            eprintln!("{}", v);
            exit(1);
        },
    }
}
