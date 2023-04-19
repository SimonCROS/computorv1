use std::{
    env::{self, Args},
    process::exit,
};

use lexer::Lexer;
use parser::{node::Node, Parser};

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
                if *l == 0f32 {
                    **node = Node::Number(0f32);
                } else if let Node::Number(r) = r.as_mut() {
                    **node = Node::Number(*l * *r);
                }
            } else if let Node::Number(r) = r.as_mut() {
                if *r == 0f32 {
                    **node = Node::Number(0f32);
                }
            }
        }
        Node::Div(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(l) = l.as_mut() {
                if let Node::Number(r) = r.as_mut() {
                    if *r == 0f32 {
                        eprintln!("Cannot compute `{}`: division by zero !", node);
                        exit(1);
                    }
                    **node = Node::Number(*l / *r);
                } else if *l == 0f32 {
                    **node = Node::Number(0f32);
                }
            } else if let Node::Number(r) = r.as_mut() {
                if *r == 0f32 {
                    eprintln!("Cannot compute `{}`: division by zero !", node);
                    exit(1);
                }
            }
        }
        Node::Pow(l, r) => {
            reduce(l);
            reduce(r);
            if let Node::Number(r) = r.as_mut() {
                if let Node::Number(l) = l.as_mut() {
                    **node = Node::Number((*l).powf(*r));
                } else if *r == 0f32 {
                    **node = Node::Number(1f32);
                } else if *r == 1f32 {
                    **node = (**l).clone();
                }
            }
        }
        Node::Negate(v) => {
            reduce(v);
            if let Node::Number(v) = v.as_mut() {
                **node = Node::Number(-(*v));
            }
        }
        _ => (),
    }
}

fn remove_substractions(node: &mut Box<Node>) {
    match node.as_mut() {
        Node::Sub(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
            **node = Node::Add(l.clone(), Box::new(Node::Negate(r.clone())));
        }
        Node::Add(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
        }
        Node::Mul(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
        }
        Node::Div(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
        }
        Node::Pow(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
        }
        Node::Equal(l, r) => {
            remove_substractions(l);
            remove_substractions(r);
        }
        Node::Negate(v) => remove_substractions(v),
        _ => (),
    }
}

fn sort_polynominal(node: &mut Box<Node>, count: bool) -> f32 {
    match node.as_mut() {
        Node::Add(l, r) => {
            let ldeg = sort_polynominal(l, count);
            let rdeg = sort_polynominal(r, count);
            if rdeg > ldeg {
                std::mem::swap(l, r);
                return rdeg;
            }
            ldeg
        }
        Node::Number(v) => {
            if count {
                *v
            } else {
                0f32
            }
        }
        Node::Identifier(_) => 1f32,
        Node::Pow(l, r) => {
            let ldeg = sort_polynominal(l, count);
            let rdeg = sort_polynominal(r, count);
            if rdeg != 0f32 {
                eprintln!(
                    "`{}` is not a valid polynomial expression: the variable is in the exponent.",
                    node
                );
                exit(1);
            }
            if ldeg != 0f32 {
                let rdeg = sort_polynominal(r, true);
                if rdeg < 0f32 {
                    eprintln!("`{}` is not a polynomial expression: the variable has a negative exponent.", node);
                    exit(1);
                }
                return ldeg * rdeg;
            }
            0f32
        }
        Node::Sub(l, r) => sort_polynominal(l, count).max(sort_polynominal(r, count)),
        Node::Mul(l, r) => sort_polynominal(l, count) + sort_polynominal(r, count),
        Node::Div(l, r) => {
            if sort_polynominal(r, count) != 0f32 {
                eprintln!(
                    "`{}` is not a polynomial expression: the variable is in the denominator.",
                    node
                );
                exit(1);
            }
            sort_polynominal(l, count)
        }
        Node::Negate(v) => sort_polynominal(v, count),
        _ => 0f32,
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
                Ok((mut lhs, mut rhs)) => {
                    println!(
                        "Full\t- \x1b[33;1m{}\x1b[0m \x1b[31;1m=\x1b[0m \x1b[32;1m{}\x1b[0m",
                        lhs, rhs
                    );
                    remove_substractions(&mut lhs);
                    println!(
                        "No sub\t- \x1b[33;1m{}\x1b[0m \x1b[31;1m=\x1b[0m \x1b[32;1m{}\x1b[0m",
                        lhs, rhs
                    );
                    reduce(&mut lhs);
                    reduce(&mut rhs);
                    println!(
                        "Reduced\t- \x1b[33;1m{}\x1b[0m \x1b[31;1m=\x1b[0m \x1b[32;1m{}\x1b[0m",
                        lhs, rhs
                    );
                    let degree = sort_polynominal(&mut lhs, false);
                    println!(
                        "Sorted\t- \x1b[33;1m{}\x1b[0m \x1b[31;1m=\x1b[0m \x1b[32;1m{}\x1b[0m",
                        lhs, rhs
                    );
                    println!("Degree: {}", degree);
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
