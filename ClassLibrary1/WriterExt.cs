using System;

namespace SimpleMonad
{
    public static class WriterExt
    {
        public static Writer<TOut, TLog> Select<TIn, TOut, TLog>(
            this Writer<TIn, TLog> first, 
            Func<TIn, TOut> selector)
            => first.Map(selector);

        public static Writer<TOut, TLog> SelectMany<TIn, TOut, TLog>(
            this Writer<TIn, TLog> first, 
            Func<TIn, Writer<TOut, TLog>> selector)
            => first.Fmap(selector);

        public static Writer<TOut, TLog> SelectMany<TIn, TBind, TOut, TLog>(
            this Writer<TIn, TLog> first,
            Func<TIn, Writer<TBind, TLog>> collectionSelector,
            Func<TIn, TBind, TOut> resultSelector)
            => first.Fmap(value => collectionSelector(value)
                                   .Fmap(secondValue => Writer<TOut, TLog>.Init((resultSelector(value, secondValue)))));

    }
}
