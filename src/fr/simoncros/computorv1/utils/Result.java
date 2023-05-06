package fr.simoncros.computorv1.utils;

import java.util.Optional;

public class Result<T, E> {
	private T okValue;
	private E errValue;

	private Result(T ok, E err) {
		this.okValue = ok;
		this.errValue = err;
	}

	public boolean isOk() {
		return this.okValue != null;
	}

	public boolean isErr() {
		return this.errValue != null;
	}

	public Optional<T> ok() {
		return Optional.ofNullable(this.okValue);
	}

	public Optional<E> err() {
		return Optional.ofNullable(this.errValue);
	}

	public static <T, E> Result<T, E> ok(T value) {
		return new Result<>(value, null);
	}

	public static <T, E> Result<T, E> err(E value) {
		return new Result<>(null, value);
	}
}
