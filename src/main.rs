use std::env::{self, Args};

fn main() {
    let args: Args = env::args();
    if args.len() != 2 {
        eprintln!("Usage: ./computorv1 <formula>");
        std::process::exit(1);
    }

    let arg: String = args.last().unwrap();
}
