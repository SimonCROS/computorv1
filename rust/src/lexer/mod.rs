use self::token::Token;
use std::{iter::Peekable, str::Chars};
pub mod token;

pub struct Lexer<'a> {
    chars: Peekable<Chars<'a>>,
}

impl<'a> Lexer<'a> {
    pub fn new(input: &'a String) -> Self {
        Self {
            chars: input.chars().peekable(),
        }
    }

    pub fn tokenize(mut self) -> Result<Vec<Token>, String> {
        let mut tokens: Vec<Token> = Vec::<Token>::new();
        loop {
            match self.next_token() {
                Ok(token) => match token {
                    Token::EOF => break,
                    _ => tokens.push(token),
                },
                Err(v) => return Err(v),
            }
        }
        Ok(tokens)
    }

    fn next_token(&mut self) -> Result<Token, String> {
        let tok: Token;

        while self.chars.next_if(|c| c.is_ascii_whitespace()).is_some() {}
        let char = self.chars.peek().map_or('\0', |&f| f);
        match char {
            '+' => tok = Token::Add(char),
            '-' => tok = Token::Sub(char),
            '*' => tok = Token::Mul(char),
            '/' => tok = Token::Div(char),
            '^' => tok = Token::Pow(char),
            '=' => tok = Token::Equal(char),
            '(' => tok = Token::LParent(char),
            ')' => tok = Token::RParent(char),
            '\0' => tok = Token::EOF,
            _ => {
                match self.read_identifier() {
                    Some(v) => return Ok(Token::Identifier(v)),
                    _ => (),
                }
                match self.read_number() {
                    Some(Ok(v)) => return Ok(Token::Number(v)),
                    Some(Err(v)) => return Err(format!("Failed conversion to f32: {}", v)),
                    _ => (),
                }
                return Err(format!("Unknown token `{}`", char));
            }
        }
        self.chars.next();
        Ok(tok)
    }

    fn read_identifier(&mut self) -> Option<String> {
        let mut ret: String = String::new();
        loop {
            match self.chars.next_if(|&c| c.is_alphabetic() || c == '_') {
                Some(c) => ret.push(c),
                None => break,
            }
        }
        Some(ret).filter(|s| !s.is_empty())
    }

    fn read_number(&mut self) -> Option<Result<f32, String>> {
        let mut ret: String = String::new();
        loop {
            match self.chars.next_if(|&c| c.is_ascii_digit() || c == '.') {
                Some(c) => ret.push(c),
                None => break,
            }
        }
        let tmp = ret.clone();
        Some(ret)
            .filter(|s| !s.is_empty())
            .map(|s| s.parse::<f32>().map_err(|_| tmp))
    }
}
