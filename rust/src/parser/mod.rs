use self::node::Node;
use crate::lexer::token::Token;
use std::{iter::Peekable, vec::IntoIter};

pub mod node;

pub struct Parser {
    tokens: Peekable<IntoIter<Token>>,
}

impl Parser {
    pub fn new(tokens: Vec<Token>) -> Self {
        Self {
            tokens: tokens.into_iter().peekable(),
        }
    }

    pub fn parse(mut self) -> Result<(Box<Node>, Box<Node>), String> {
        let root = self.assignation();

        if self.tokens.peek().is_some() {
            return Err(format!(
                "Unexpected token : {:?}",
                self.tokens.peek().unwrap()
            ));
        }
        if root.is_none() {
            return Err(format!("Expected token at end"));
        }
        if let Node::Equal(lhs, rhs) = *root.unwrap() {
            return Ok((lhs, rhs));
        }
        panic!("This should never happen.");
    }

    fn assignation(&mut self) -> Option<Box<Node>> {
        let lhs = self.expr();
        if lhs.is_none() {
            return None;
        }

        // Don't do loop so there is at most one assignation
        let token = self.tokens.peek();
        match token {
            Some(Token::Equal(_)) => {
                self.tokens.next();
                match self.expr() {
                    Some(rhs) => return Some(Box::new(Node::Equal(lhs.unwrap(), rhs))),
                    None => return None,
                }
            }
            _ => return None, // Force at least one assignation
        }
    }

    fn expr(&mut self) -> Option<Box<Node>> {
        let lhs = self.factor();
        if lhs.is_none() {
            return None;
        }

        let mut result: Box<Node> = lhs.unwrap();
        while let Some(token) = self.tokens.peek() {
            match token {
                Token::Add(_) => {
                    self.tokens.next();
                    match self.factor() {
                        Some(rhs) => result = Box::new(Node::Add(result, rhs)),
                        None => return None,
                    }
                }
                Token::Sub(_) => {
                    self.tokens.next();
                    match self.factor() {
                        Some(rhs) => result = Box::new(Node::Sub(result, rhs)),
                        None => return None,
                    }
                }
                _ => break,
            }
        }
        return Some(result);
    }

    fn factor(&mut self) -> Option<Box<Node>> {
        let lhs = self.pow();
        if lhs.is_none() {
            return None;
        }

        let mut result: Box<Node> = lhs.unwrap();
        while let Some(token) = self.tokens.peek() {
            match token {
                Token::Mul(_) => {
                    self.tokens.next();
                    match self.pow() {
                        Some(rhs) => result = Box::new(Node::Mul(result, rhs)),
                        None => return None,
                    }
                }
                Token::Div(_) => {
                    self.tokens.next();
                    match self.pow() {
                        Some(rhs) => result = Box::new(Node::Div(result, rhs)),
                        None => return None,
                    }
                }
                _ => break,
            }
        }
        return Some(result);
    }

    fn pow(&mut self) -> Option<Box<Node>> {
        let lhs = self.term();
        if lhs.is_none() {
            return None;
        }

        let mut result: Box<Node> = lhs.unwrap();
        while let Some(token) = self.tokens.peek() {
            match token {
                Token::Pow(_) => {
                    self.tokens.next();
                    match self.term() {
                        Some(rhs) => result = Box::new(Node::Pow(result, rhs)),
                        None => return None,
                    }
                }
                _ => break,
            }
        }
        return Some(result);
    }

    fn term(&mut self) -> Option<Box<Node>> {
        let mut result: Option<Box<Node>>;

        let token = self.tokens.peek();
        match token {
            Some(Token::Sub(_)) => {
                self.tokens.next();
                result = self.term();
                match result {
                    Some(next) => result = Some(Box::new(Node::Negate(next))),
                    _ => return None,
                }
            }
            Some(Token::Number(n)) => {
                result = Some(Box::new(Node::Number(*n)));
                self.tokens.next();
            }
            Some(Token::Identifier(n)) => {
                result = Some(Box::new(Node::Identifier(n.clone())));
                self.tokens.next();
            }
            Some(Token::LParent(_)) => {
                self.tokens.next();
                result = self.expr();
                if !matches!(self.tokens.peek(), Some(Token::RParent(_))) {
                    return None;
                }
                self.tokens.next();
            }
            _ => return None,
        }
        result
    }
}
