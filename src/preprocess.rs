use crate::{parser::node::Node, utils::degree};
use num_traits::Zero;
use std::{process::exit, cell::RefCell};

fn find_cumulable_number<'a>(parent: RefCell<&'a mut Node>, node: RefCell<&'a mut Node>) -> Option<(RefCell<&'a mut Node>, RefCell<&'a mut Node>)> {
    match *node.borrow() {
        Node::Add(l, r) => find_cumulable_number(node, RefCell::new(l.as_mut())).or_else(|| find_cumulable_number(node, RefCell::new(r.as_mut()))),
        Node::Number(_) => Some((parent, node)),
        _ => None
    }
}

pub fn simplify(node: &mut Node) {
    match node {
        Node::Equal(l, r) => {
            simplify(l);
            simplify(r);
        }
        Node::Add(l, r) => {
            simplify(l);
            simplify(r);
            if let Some((_, lrc)) = find_cumulable_number(RefCell::new(node), RefCell::new(l.as_mut())) {
                if let Some((parent, rrc)) = find_cumulable_number(RefCell::new(node), RefCell::new(r.as_mut())) {
                    if let Node::Number(ln) = *lrc.borrow_mut() {
                        if let Node::Number(rn) = *rrc.borrow() {
                            *ln += *rn;
                        }
                    }
                }
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
