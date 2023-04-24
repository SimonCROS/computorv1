use crate::utils::degree;
use crate::{
    lexer::Lexer,
    utils::{dbg_equation, print_equation},
};
use crate::{
    parser::{node::Node, Parser},
    preprocess::{simplify, sort_polynominal},
};
use std::{
    env::{self, Args},
    process::exit,
};

mod lexer;
mod parser;
mod preprocess;
mod utils;

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
                Ok((mut lhs, mut rhs)) => {
                    print_equation("Full", &lhs, &rhs);
                    *lhs = Node::Sub(lhs.clone(), rhs);
                    rhs = Box::new(Node::Number(0f32));
                    print_equation("Eq zero", &lhs, &rhs);

                    lhs.clean();
                    simplify(&mut lhs);
                    sort_polynominal(&mut lhs);
                    lhs.clean();
                    simplify(&mut lhs);

                    print_equation("Sorted", &lhs, &rhs);
                    println!("Degree: {}", degree(&lhs));
                    dbg_equation("dbg", &lhs, &rhs);
                }
                Err(v) => {
                    eprintln!("{}", v);
                    exit(1);
                }
            }
        }
        Err(v) => {
            eprintln!("{}", v);
            exit(1);
        }
    }
}
