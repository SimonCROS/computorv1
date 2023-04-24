use crate::parser::node::Node;
use num_traits::{Signed, Zero};
use std::process::exit;

fn _degree(node: &Node, count: bool) -> f32 {
    match node {
        Node::Number(v) if count => *v,
        Node::Identifier(_) if count => {
            eprintln!(
                "`{}` is not a valid polynomial expression: the variable is in the exponent.",
                node
            );
            exit(1)
        }
        Node::Identifier(_) => 1f32,
        Node::Pow(l, r) => {
            let ld = _degree(l, count);
            let rd = _degree(r, true);
            if !ld.is_zero() && rd.is_negative() {
                eprintln!(
                    "`{}` is not a polynomial expression: the variable has a negative exponent.",
                    node
                );
                exit(1);
            }
            ld * rd
        }
        Node::Add(l, r) | Node::Sub(l, r) => _degree(l, count).max(_degree(r, count)),
        Node::Mul(l, r) => _degree(l, count) + _degree(r, count),
        Node::Div(l, r) => {
            if !_degree(r, count).is_zero() {
                eprintln!(
                    "`{}` is not a polynomial expression: the variable is in the denominator.",
                    node
                );
                exit(1);
            }
            _degree(l, count)
        }
        Node::Negate(v) => _degree(v, count),
        _ => 0f32,
    }
}

pub fn degree(node: &Node) -> f32 {
    _degree(node, false)
}

pub fn dbg_equation(message: &str, lhs: &Node, rhs: &Node) {
    println!("\x1b[37;1m{}\t- {:?} = {:?}\x1b[0m", message, lhs, rhs);
}

pub fn print_equation(message: &str, lhs: &Node, rhs: &Node) {
    println!(
        "{}\t- \x1b[33;1m{}\x1b[0m \x1b[31;1m=\x1b[0m \x1b[32;1m{}\x1b[0m",
        message, lhs, rhs
    );
}
