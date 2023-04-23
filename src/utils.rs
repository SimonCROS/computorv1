use crate::parser::node::Node;

fn _degree(node: &Node, count: bool) -> f32 {
    match node {
        Node::Number(v) if count => *v,
        Node::Identifier(_) => 1f32,
        Node::Pow(l, r) => _degree(l, count) * _degree(r, true),
        Node::Add(l, r) | Node::Sub(l, r) => {
            _degree(l, count).max(_degree(r, count))
        }
        Node::Mul(l, r) => _degree(l, count) + _degree(r, count),
        Node::Div(_, _) => unimplemented!(),
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
