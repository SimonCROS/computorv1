use num_traits::{Signed, Zero};
use std::{
    cell::RefCell,
    fmt::{Debug, Display},
    rc::{Rc},
};

#[derive(Clone, PartialEq)]
pub struct Node {
    // parent: Option<Weak<RefCell<Node>>>,
    pub left: Rc<RefCell<Node>>,
    pub right: Rc<RefCell<Node>>,
    pub value: Literal,
    node_type: NodeType,
}

impl Node {
    pub fn get_type(&self) -> NodeType {
        self.node_type
    }
}

#[derive(Clone, PartialEq)]
pub enum Literal {
    Identifier(String),
    Number(f32),
    None,
}

#[derive(Clone, PartialEq)]
pub enum NodeType {
    Equal,
    Add,
    Sub,
    Div,
    Mul,
    Pow,
    Negate,
    Literal,
}

impl Node {
    pub fn new_negate(node: Node) -> Node {
        Node { left: Rc::new(RefCell::new(node)), right: (), value: Literal::None, node_type: NodeType::Negate }
    }

    pub fn new_literal(value: Literal) -> Node {
        Node { left: (), right: (), value: value, node_type: NodeType::Literal }
    }

    pub fn new_binary(left: Node, right: Node, node_type: NodeType) -> Node {
        Node { left: Rc::new(RefCell::new(left)), right: Rc::new(RefCell::new(right)), value: Literal::None, node_type: node_type }
    }

    pub fn is_zero(&self) -> bool {
        match self.get_type() {
            NodeType::Literal if matches!(self.value, Literal::Number(n) if n.is_zero()) => true,
            NodeType::Pow if self.left.borrow().is_zero() && !self.right.borrow().is_zero() => true,
            NodeType::Mul if self.left.borrow().is_zero() || self.right.borrow().is_zero() => true,
            NodeType::Div if self.left.borrow().is_zero() => true,
            NodeType::Negate => self.right.borrow().is_zero(),
            _ => false,
        }
    }

    pub fn is_negative(&self) -> bool {
        match self.get_type() {
            NodeType::Negate => true,
            NodeType::Literal if matches!(self.value, Literal::Number(n) if n.is_negative()) => true,
            NodeType::Mul | NodeType::Div if self.left.borrow().is_negative() => true,
            _ => false,
        }
    }

    pub fn negate(&mut self) {
        match self.get_type() {
            NodeType::Negate => *self = self.left.borrow().clone(),
            NodeType::Literal if matches!(self.value, Literal::Number(n)) => self.value.,
            NodeType::Mul | NodeType::Div => self.left.get_mut().negate(),
            _ => {
                *self = Self::new_negate(self.clone());
            },
        }
    }

    pub fn rotate(&mut self) {
        std::mem::swap(self.left.get_mut(), self.right.get_mut())
    }

    pub fn clean(&mut self) {
        self.left.borrow().clean();
        self.right.borrow().clean();

        if self.is_zero() {
            self.node_type = NodeType::Number;
            self.number = 0f32;
            return;
        }

        match self.get_type() {
            NodeType::Add if self.left.borrow().is_zero() => *self = self.right.borrow().clone(),
            NodeType::Add if self.right.borrow().is_zero() => *self = self.left.borrow().clone(),
            NodeType::Sub if self.right.borrow().is_zero() => *self = self.left.borrow().clone(),
            NodeType::Sub if self.left.borrow().is_zero() => {
                self.right.get_mut().negate();
                *self = self.right.borrow().clone();
            }
            NodeType::Sub => {
                self.right.get_mut().negate();
                self.node_type = NodeType::Add;
                self.rotate();
            }
            NodeType::Mul | NodeType::Div
                if self.left.borrow().is_negative() && self.right.borrow().is_negative() =>
            {
                self.left.borrow().negate();
                self.right.borrow().negate();
            }
            NodeType::Pow if self.right.borrow().get_type() == NodeType::Number && self.right.borrow().number == 1f32 => {
                *self = self.left.borrow().clone()
            }
            NodeType::Pow if self.right.borrow().is_zero() => {
                self.node_type = NodeType::Number;
                self.number = 0f32;
            }
            _ => (),
        }
    }
}

impl NodeType {
    fn should_isolate(&self, parent: &Self) -> bool {
        match self {
            Self::Equal => false,
            Self::Add | Self::Sub => {
                matches!(parent, Self::Mul | Self::Div | Self::Negate)
            }
            Self::Mul | Self::Div => {
                matches!(parent, Self::Pow | Self::Negate)
            }
            Self::Pow => matches!(parent, Self::Negate),
            Self::Negate | Self::Identifier | Self::Number => false,
        }
    }
}

impl Display for Node {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Equal(l, r) => write!(f, "{} = {}", l, r),
            Self::Add(l, r) => {
                let mut res = String::new();
                if l.should_isolate(self) {
                    res = format!("{}({})", res, l);
                } else {
                    res = format!("{}{}", res, l);
                }
                res += " + ";
                if r.should_isolate(self) {
                    res = format!("{}({})", res, r);
                } else {
                    res = format!("{}{}", res, r);
                }
                write!(f, "{}", res)
            }
            Self::Sub(l, r) => {
                let mut res = String::new();
                if l.should_isolate(self) {
                    res = format!("{}({})", res, l);
                } else {
                    res = format!("{}{}", res, l);
                }
                res += " - ";
                if r.should_isolate(self) {
                    res = format!("{}({})", res, r);
                } else {
                    res = format!("{}{}", res, r);
                }
                write!(f, "{}", res)
            }
            Self::Mul(l, r) => {
                if let Node::Identifier(l) = l.as_ref() {
                    if let Node::Number(r) = r.as_ref() {
                        return write!(f, "{}{}", r, l);
                    }
                }
                if let Node::Number(l) = l.as_ref() {
                    if let Node::Identifier(r) = r.as_ref() {
                        return write!(f, "{}{}", l, r);
                    }
                    if let Node::Pow(pl, _) = r.as_ref() {
                        if let Node::Identifier(_) = pl.as_ref() {
                            return write!(f, "{}{}", l, r);
                        }
                    }
                }
                let mut res = String::new();
                if l.should_isolate(self) {
                    res = format!("{}({})", res, l);
                } else {
                    res = format!("{}{}", res, l);
                }
                res += " * ";
                if r.should_isolate(self) {
                    res = format!("{}({})", res, r);
                } else {
                    res = format!("{}{}", res, r);
                }
                write!(f, "{}", res)
            }
            Self::Div(l, r) => {
                let mut res = String::new();
                if l.should_isolate(self) {
                    res = format!("{}({})", res, l);
                } else {
                    res = format!("{}{}", res, l);
                }
                res += " / ";
                if r.should_isolate(self) {
                    res = format!("{}({})", res, r);
                } else {
                    res = format!("{}{}", res, r);
                }
                write!(f, "{}", res)
            }
            Self::Pow(l, r) => {
                if let Node::Number(r) = r.as_ref() {
                    match *r {
                        x if x == 2f32 => return write!(f, "{}²", l),
                        x if x == 3f32 => return write!(f, "{}³", l),
                        x if x == 4f32 => return write!(f, "{}⁴", l),
                        x if x == 5f32 => return write!(f, "{}⁵", l),
                        x if x == 6f32 => return write!(f, "{}⁶", l),
                        x if x == 7f32 => return write!(f, "{}⁷", l),
                        x if x == 8f32 => return write!(f, "{}⁸", l),
                        x if x == 9f32 => return write!(f, "{}⁹", l),
                        _ => (),
                    }
                }
                let mut res = String::new();
                if l.should_isolate(self) {
                    res = format!("{}({})", res, l);
                } else {
                    res = format!("{}{}", res, l);
                }
                res += " ^ ";
                if r.should_isolate(self) {
                    res = format!("{}({})", res, r);
                } else {
                    res = format!("{}{}", res, r);
                }
                write!(f, "{}", res)
            }
            Self::Negate(v) => {
                if v.should_isolate(self) {
                    write!(f, "-({})", v)
                } else {
                    write!(f, "-{}", v)
                }
            }
            Self::Identifier(v) => write!(f, "{}", v),
            Self::Number(v) => write!(f, "{}", v),
        }
    }
}

impl Debug for Node {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Equal(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Add(l, r) => write!(f, "({:?} + {:?})", l, r),
            Self::Sub(l, r) => write!(f, "({:?} - {:?})", l, r),
            Self::Mul(l, r) => write!(f, "({:?} * {:?})", l, r),
            Self::Div(l, r) => write!(f, "({:?} / {:?})", l, r),
            Self::Pow(l, r) => write!(f, "({:?} ^ {:?})", l, r),
            Self::Negate(v) => write!(f, "-({:?})", v),
            Self::Identifier(v) => write!(f, "{:?}", v),
            Self::Number(v) => write!(f, "{:?}", v),
        }
    }
}
