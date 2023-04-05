use std::fmt::Debug;

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
