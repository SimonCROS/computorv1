use num_traits::Zero;
use std::{
    env::{self, Args},
    process::exit,
};

use crate::parser::{node::Node, Parser};
use crate::utils::degree;
use crate::{
    lexer::Lexer,
    utils::{dbg_equation, print_equation},
};

mod lexer;
mod parser;
mod utils;

fn simplify(node: &mut Node) {
    match node {
        Node::Equal(l, r) => {
            simplify(l);
            simplify(r);
        }
        Node::Add(l, r) => {
            simplify(l);
            simplify(r);
            match (l.as_mut(), r.as_mut()) {
                (Node::Number(l), Node::Number(r)) => *node = Node::Number(*l + *r),
                _ => (),
            }
        }
        Node::Sub(l, r) => {
            simplify(l);
            simplify(r);
            match (l.as_mut(), r.as_mut()) {
                (Node::Number(l), Node::Number(r)) => *node = Node::Number(*l - *r),
                _ => (),
            }
        }
        Node::Mul(l, r) => {
            simplify(l);
            simplify(r);
            match (l.as_mut(), r.as_mut()) {
                (Node::Number(l), Node::Number(r)) => *node = Node::Number(*l * *r),
                _ => (),
            }
        }
        Node::Div(l, r) => {
            simplify(l);
            simplify(r);
            match (l.as_mut(), r.as_mut()) {
                (_, Node::Number(r)) if r.is_zero() => {
                    eprintln!("Cannot compute `{}`: division by zero !", node);
                    exit(1);
                }
                (Node::Number(l), Node::Number(r)) => *node = Node::Number(*l / *r),
                _ => (),
            }
        }
        Node::Pow(l, r) => {
            simplify(l);
            simplify(r);
            match (l.as_mut(), r.as_mut()) {
                (Node::Number(l), Node::Number(r)) => *node = Node::Number((*l).powf(*r)),
                _ => (),
            }
        }
        Node::Negate(v) => {
            simplify(v);
            v.negate();
            *node = (**v).clone();
        }
        _ => (),
    }
}

fn sort_polynominal(node: &mut Node) {
    match node {
        Node::Add(l, r) | Node::Sub(l, r) => {
            sort_polynominal(l);
            sort_polynominal(r);

            if degree(r) > degree(l) {
                node.rotate();
            }
        }
        Node::Pow(l, r) | Node::Mul(l, r) | Node::Div(l, r) => {
            sort_polynominal(l);
            sort_polynominal(r);
        }
        // eprintln!(
        //     "`{}` is not a valid polynomial expression: the variable is in the exponent.",
        //     node
        // );
        // exit(1);

        // if rdeg < 0f32 {
        //     eprintln!("`{}` is not a polynomial expression: the variable has a negative exponent.", node);
        //     exit(1);
        // }

        // eprintln!(
        //     "`{}` is not a polynomial expression: the variable is in the denominator.",
        //     node
        // );
        // exit(1);
        Node::Negate(v) => sort_polynominal(v),
        _ => (),
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
