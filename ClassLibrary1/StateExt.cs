using System;

namespace SimpleMonad
{
    public static class StateExt
    {
        public static State<TOut, TState> Select<TIn, TOut, TState>(
            this State<TIn, TState> first,
            Func<TIn, TOut> selector)
            => first.Map(selector);

        public static State<TOut, TState> SelectMany<TIn, TOut, TState>(
            this State<TIn, TState> first,
            Func<TIn, State<TOut, TState>> selector)
            => first.Fmap(selector);

        public static State<TOut, TState> SelectMany<TIn, TBind, TOut, TState>(
            this State<TIn, TState> first,
            Func<TIn, State<TBind, TState>> collectionSelector,
            Func<TIn, TBind, TOut> resultSelector)
            => first.Fmap(value => collectionSelector(value)
                                   .Fmap(secondValue => State<TOut, TState>.Init((resultSelector(value, secondValue)))));

        // TODO useless ???
        private static State<T, TState> Where<T, TState>(
            this State<T, TState> source, Func<T, bool> predicate)
            => source.Map(
                val => predicate(val) ? val : default(T));
    }
}
