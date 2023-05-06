use crate::parser::node::{Node, NodeType, Literal};
use num_traits::{Signed, Zero};
use std::{process::exit, cell::Ref};

fn _degree(node: Ref<Node>, count: bool) -> f32 {
    match node.node_type {
        NodeType::Literal => match node.value {
            Literal::Number(v) if count => v,
            Literal::Identifier(_) if count => {
                eprintln!(
                    "`{}` is not a valid polynomial expression: the variable is in the exponent.",
                    node
                );
                exit(1)
            }
            Literal::Identifier(_) => 1f32,
            Literal::Number(_) | Literal::None => 0f32,
        },
        NodeType::Pow => {
            let ld = _degree(node.left.borrow(), count);
            let rd = _degree(node.right.borrow(), true);
            if !ld.is_zero() && rd.is_negative() {
                eprintln!(
                    "`{}` is not a polynomial expression: the variable has a negative exponent.",
                    node
                );
                exit(1);
            }
            ld * rd
        }
        NodeType::Add | NodeType::Sub => _degree(node.left.borrow(), count).max(_degree(node.right.borrow(), count)),
        NodeType::Mul => _degree(node.left.borrow(), count) + _degree(node.right.borrow(), count),
        NodeType::Div => {
            if !_degree(node.right.borrow(), count).is_zero() {
                eprintln!(
                    "`{}` is not a polynomial expression: the variable is in the denominator.",
                    node
                );
                exit(1);
            }
            _degree(node.left.borrow(), count)
        }
        NodeType::Negate => _degree(node.left.borrow(), count),
        _ => 0f32,
    }
}

pub fn degree(node: Ref<Node>) -> f32 {
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
