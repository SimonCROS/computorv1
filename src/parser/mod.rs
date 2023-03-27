use std::{iter::Peekable, vec::IntoIter};

use crate::lexer::token::Token;

use self::node::Node;

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

    pub fn parse(mut self) -> Result<Node<'static>, String> {
        let mut tokens: Vec<Token> = Vec::<Token>::new();
        
		let root = self.expr();
		if self.tokens.peek().is_some() {
			return Err(format!("Unexpected token : {:?}", self.tokens.peek().unwrap()));
		}

		Ok(root)
    }

	fn expr(&self) -> Node<'static> {
		let result = self.term();

		todo!()
	}

	fn term(&self) -> Node<'static> {
		let result = self.factor();
		
		todo!()
	}

	fn factor(&self) -> Node<'static> {
		todo!()
	}
}
