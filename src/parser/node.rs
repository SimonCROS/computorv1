use std::fmt::Debug;

pub enum Node<'a> {
    Add(&'a Node<'a>, &'a Node<'a>),
    Sub(&'a Node<'a>, &'a Node<'a>),
    Div(&'a Node<'a>, &'a Node<'a>),
    Mul(&'a Node<'a>, &'a Node<'a>),
    Pow(&'a Node<'a>, &'a Node<'a>),
    Equal(&'a Node<'a>, &'a Node<'a>),
    Identifier(String),
    Number(f32),
}

impl Debug for Node<'_> {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Add(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Sub(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Mul(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Div(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Pow(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Equal(l, r) => write!(f, "({:?} = {:?})", l, r),
            Self::Identifier(v) => write!(f, "{:?}", v),
            Self::Number(v) => write!(f, "{:?}", v),
        }
    }
}
