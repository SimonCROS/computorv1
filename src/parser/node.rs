use num_traits::{Signed, Zero};
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

impl Node {
    fn should_isolate(&self, parent: &Self) -> bool {
        match self {
            Self::Equal(_, _) => false,
            Self::Add(_, _) | Self::Sub(_, _) => {
                matches!(parent, Self::Mul(_, _) | Self::Div(_, _) | Self::Negate(_))
            }
            Self::Mul(_, _) | Self::Div(_, _) => {
                matches!(parent, Self::Pow(_, _) | Self::Negate(_))
            }
            Self::Pow(_, _) => matches!(parent, Self::Negate(_)),
            Self::Negate(_) | Self::Identifier(_) | Self::Number(_) => false,
        }
    }

    pub fn is_zero(&self) -> bool {
        match self {
            Self::Number(v) => v.is_zero(),
            Self::Pow(l, r) if l.is_zero() && !r.is_zero() => true,
            Self::Mul(v, _) | Self::Mul(_, v) | Self::Div(v, _) if v.is_zero() => true,
            Self::Negate(v) => v.is_zero(),
            _ => false,
        }
    }

    pub fn is_negative(&self) -> bool {
        match self {
            Self::Negate(_) => true,
            Self::Number(v) if v.is_negative() => true,
            Self::Mul(l, _) | Self::Div(l, _) if l.is_negative() => true,
            _ => false,
        }
    }

    pub fn negate(&mut self) {
        match self {
            Self::Negate(v) => *self = (**v).clone(),
            Self::Number(v) => *v = -*v,
            Self::Mul(l, _) | Self::Div(l, _) => l.negate(),
            _ => *self = Node::Negate(Box::new(self.clone())),
        }
    }

    pub fn rotate(&mut self) {
        match self {
            Self::Equal(l, r) | Self::Add(l, r) | Self::Mul(l, r) => {
                std::mem::swap(l.as_mut(), r.as_mut())
            }
            Self::Sub(l, r) => {
                l.negate();
                r.negate();
                std::mem::swap(l.as_mut(), r.as_mut())
            }
            _ => (),
        }
    }

    pub fn clean(&mut self) {
        match self {
            Self::Equal(l, r)
            | Self::Add(l, r)
            | Self::Sub(l, r)
            | Self::Mul(l, r)
            | Self::Div(l, r)
            | Self::Pow(l, r) => {
                l.clean();
                r.clean();
            }
            Self::Negate(v) => v.clean(),
            _ => (),
        }

        if self.is_zero() {
            *self = Node::Number(0f32);
            return;
        }

        match self {
            Self::Add(l, r) | Self::Add(r, l) if r.is_zero() => *self = l.as_ref().clone(),
            Self::Add(l, r) | Self::Add(r, l) if r.is_negative() => {
                r.negate();
                *self = Node::Sub(l.clone(), r.clone())
            }
            Self::Sub(l, r) if r.is_negative() => {
                r.negate();
                *self = Node::Add(l.clone(), r.clone())
            }
            Self::Sub(l, r) if r.is_zero() => *self = l.as_ref().clone(),
            Self::Sub(l, r) if l.is_zero() => {
                r.negate();
                *self = r.as_ref().clone();
            }
            Self::Mul(l, r) | Self::Div(l, r) => {
                if l.is_negative() && r.is_negative() {
                    l.negate();
                    r.negate();
                }
            }
            Self::Pow(l, r) if matches!(r.as_ref(), Node::Number(n) if *n == 1f32) => {
                *self = l.as_ref().clone()
            }
            Self::Pow(_, r) if r.is_zero() => *self = Node::Number(1f32),
            _ => (),
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
