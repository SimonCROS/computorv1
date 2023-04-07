use std::fmt::{Debug, Display};

#[derive(Clone)]
pub enum Node {
    Equal(Box<Node>, Box<Node>),
    Add(Box<Node>, Box<Node>),
    Sub(Box<Node>, Box<Node>),
    Div(Box<Node>, Box<Node>),
    Mul(Box<Node>, Box<Node>),
    Pow(Box<Node>, Box<Node>),
    Negate(Box<Node>),
    Identifier(String),
    Number(f32),
}

impl Display for Node {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Equal(l, r) => write!(f, "{} = {}", l, r),
            Self::Add(l, r) => {
                write!(f, "{} + {}", l, r)
            },
            Self::Sub(l, r) => {
                write!(f, "{} - {}", l, r)
            },
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
                }
                let lres;
                let rres;
                if matches!(l.as_ref(), Node::Add(_, _)) || matches!(l.as_ref(), Node::Sub(_, _)) {
                    lres = format!("({})", l);
                } else {
                    lres = format!("{}", l);
                }
                if matches!(r.as_ref(), Node::Add(_, _)) || matches!(r.as_ref(), Node::Sub(_, _)) {
                    rres = format!("({})", r);
                } else {
                    rres = format!("{}", r);
                }
                write!(f, "{} * {}", lres, rres)
            },
            Self::Div(l, r) => {
                let lres;
                let rres;
                if matches!(l.as_ref(), Node::Add(_, _)) || matches!(l.as_ref(), Node::Sub(_, _)) {
                    lres = format!("({})", l);
                } else {
                    lres = format!("{}", l);
                }
                if matches!(r.as_ref(), Node::Add(_, _)) || matches!(r.as_ref(), Node::Sub(_, _)) {
                    rres = format!("({})", r);
                } else {
                    rres = format!("{}", r);
                }
                write!(f, "{} / {}", lres, rres)
            },
            Self::Pow(l, r) => {
                if let Node::Number(r) = r.as_ref() {
                    if *r == 2f32 {
                        return write!(f, "{}²", l);
                    }
                    if *r == 3f32 {
                        return write!(f, "{}³", l);
                    }
                }
                write!(f, "({} ^ {})", l, r)
            },
            Self::Negate(v) => write!(f, "-({})", v),
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
