using System;

namespace SimpleMonad
{
    public static class OptionExt
    {
        public static Option<T> None<T>() => new Option<T>();

        public static Option<T> Some<T>(T value) => value;

        public static Option<T> Where<T>(this Option<T> source, Func<T, bool> predicate)
            => source.HasValue && predicate(source.Value) ? source : new Option<T>();

        public static Option<U> Select<T, U>(this Option<T> first, Func<T, U> selector)
            => first.Map(selector);

        public static Option<U> SelectMany<T, U>(this Option<T> first, Func<T, Option<U>> selector)
            => first.Fmap(selector);

        public static Option<U> SelectMany<T, B, U>(
            this Option<T> first,
            Func<T, Option<B>> collectionSelector,
            Func<T, B, U> resultSelector)
            => first.Fmap(value => collectionSelector(value)
                                   .Map(secondValue => resultSelector(value, secondValue)));

        public static T FirstOrDefault<T>(this Option<T> first, T defaultValue)
            => first.Reduce(defaultValue);

        public static T FirstOrDefault<T>(this Option<T> first)
            => first.FirstOrDefault(default(T));
    }
}
