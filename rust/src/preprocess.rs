use crate::{
    parser::node::{Literal, Node, NodeType},
    utils::degree,
};
use std::{cell::RefCell, process::exit, rc::Rc};

pub fn simplify(node: Rc<RefCell<Node>>) {
    simplify(node.borrow_mut().left);
    simplify(node.borrow_mut().right);

    match node.borrow().node_type {
        NodeType::Add => match (
            node.borrow().left.borrow().value,
            node.borrow().right.borrow().value,
        ) {
            (Literal::Number(l), Literal::Number(r)) => {
                node.borrow_mut().value = Literal::Number(l + r)
            }
            _ => (),
        },
        NodeType::Sub => match (
            node.borrow().left.borrow().value,
            node.borrow().right.borrow().value,
        ) {
            (Literal::Number(l), Literal::Number(r)) => {
                node.borrow_mut().value = Literal::Number(l - r)
            }
            _ => (),
        },
        NodeType::Mul => match (
            node.borrow().left.borrow().value,
            node.borrow().right.borrow().value,
        ) {
            (Literal::Number(l), Literal::Number(r)) => {
                node.borrow_mut().value = Literal::Number(l * r)
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
                node.borrow().left.borrow().value,
                node.borrow().right.borrow().value,
            ) {
                (Literal::Number(l), Literal::Number(r)) => {
                    node.borrow_mut().value = Literal::Number(l / r)
                }
                _ => (),
            }
        }
        NodeType::Pow => match (
            node.borrow().left.borrow().value,
            node.borrow().right.borrow().value,
        ) {
            (Literal::Number(l), Literal::Number(r)) => {
                node.borrow_mut().value = Literal::Number(l.powf(r));
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

pub fn sort_polynominal(node: Rc<RefCell<Node>>) {
    match node.borrow().node_type {
        NodeType::Add | NodeType::Sub => {
            sort_polynominal(node.borrow().left);
            sort_polynominal(node.borrow().right);

            if degree(node.borrow().right.borrow()) > degree(node.borrow().left.borrow()) {
                node.borrow().left.borrow().negate();
                node.borrow().right.borrow().negate();
                node.borrow().rotate();
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
