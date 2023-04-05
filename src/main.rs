use std::{
    env::{self, Args},
    process::exit,
};

use lexer::Lexer;
use parser::{Parser, node::Node};

mod lexer;
mod parser;

fn reduce(node: &mut Box<Node>) {
    match node.as_mut() {
        Node::Equal(l, r) => {
            reduce(l);
            reduce(r);
        }
        Node::Add(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number(*l + *r);
                }
            }
        }
        Node::Sub(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number(*l - *r);
                }
            }
        }
        Node::Mul(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number(*l * *r);
                }
            }
        }
        Node::Div(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number(*l / *r);
                }
            }
        }
        Node::Pow(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number((*l).powf(*r));
                }
            }
        }
        Node::Negate(v) => {
            reduce(v);
            if let Node::Number(v) = v.as_mut() {
                **node = Node::Number(-(*v));
            }
        }
        _ => ()
    }
}

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
                Ok(mut root) => {
                    println!("Before:");
                    println!("{:?}", root);
                    reduce(&mut root);
                    println!("After:");
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
