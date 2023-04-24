use crate::{parser::node::Node, utils::degree};
use num_traits::Zero;
use std::process::exit;

pub fn simplify(node: &mut Node) {
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
