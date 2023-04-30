use crate::{
    parser::node::{Node, NodeType},
    utils::degree,
};
use std::{cell::RefCell, process::exit, rc::Rc};

pub fn simplify(node: Rc<RefCell<Node>>) {
    simplify(node.borrow_mut().left);
    simplify(node.borrow_mut().right);

    match node.borrow().get_type() {
        NodeType::Add => match (
            node.borrow().left.borrow().get_type(),
            node.borrow().right.borrow().get_type(),
        ) {
            (NodeType::Number, NodeType::Number) => {
                node.borrow_mut().number =
                    node.borrow().left.borrow().number + node.borrow().right.borrow().number
            }
            _ => (),
        },
        NodeType::Sub => match (
            node.borrow().left.borrow().get_type(),
            node.borrow().right.borrow().get_type(),
        ) {
            (NodeType::Number, NodeType::Number) => {
                node.borrow_mut().number =
                    node.borrow().left.borrow().number - node.borrow().right.borrow().number
            }
            _ => (),
        },
        NodeType::Mul => match (
            node.borrow().left.borrow().get_type(),
            node.borrow().right.borrow().get_type(),
        ) {
            (NodeType::Number, NodeType::Number) => {
                node.borrow_mut().number =
                    node.borrow().left.borrow().number * node.borrow().right.borrow().number
            }
            _ => (),
        },
        NodeType::Div => {
            if node.borrow().right.borrow().is_zero() {
                eprintln!(
                    "Cannot compute `{}`: division by zero !",
                    node.borrow().right.borrow()
                );
                exit(1);
            }
            match (
                node.borrow().left.borrow().get_type(),
                node.borrow().right.borrow().get_type(),
            ) {
                (NodeType::Number, NodeType::Number) => {
                    node.borrow_mut().number =
                        node.borrow().left.borrow().number / node.borrow().right.borrow().number
                }
                _ => (),
            }
        }
        NodeType::Pow => match (
            node.borrow().left.borrow().get_type(),
            node.borrow().right.borrow().get_type(),
        ) {
            (NodeType::Number, NodeType::Number) => {
                node.borrow_mut().number = node
                    .borrow()
                    .left
                    .borrow()
                    .number
                    .powf(node.borrow().right.borrow().number)
            }
            _ => (),
        },
        NodeType::Negate => {
            // v.negate();
            // *node = (**v).clone();
        }
        _ => (),
    }
}

pub fn sort_polynominal(node: &mut Node) {
    match node {
        Node::Add(l, r) | Node::Sub(l, r) => {
            sort_polynominal(l);
            sort_polynominal(r);

            if degree(r) > degree(l) {
                node.rotate();
            }
        }
        Node::Equal(l, r) | Node::Pow(l, r) | Node::Mul(l, r) | Node::Div(l, r) => {
            sort_polynominal(l);
            sort_polynominal(r);
        }
        Node::Negate(v) => sort_polynominal(v),
        _ => (),
    }
}
