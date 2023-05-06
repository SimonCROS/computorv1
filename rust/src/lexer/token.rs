use std::fmt::Debug;

pub enum Token {
    Add(char),
    Sub(char),
    Div(char),
    Mul(char),
    Pow(char),
    Equal(char),
    LParent(char),
    RParent(char),
    Identifier(String),
    Number(f32),
    EOF,
}

impl Debug for Token {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Add(v) | Self::Sub(v) | Self::Mul(v) | Self::Div(v) | Self::Pow(v) | Self::Equal(v) | Self::LParent(v) | Self::RParent(v) => write!(f, "{:?}", v),
            Self::Identifier(v) => write!(f, "{:?}", v),
            Self::Number(v) => write!(f, "{:?}", v),
            Self::EOF => write!(f, ""),
        }
    }
}
