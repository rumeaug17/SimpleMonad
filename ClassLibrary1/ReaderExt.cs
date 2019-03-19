using System;

namespace SimpleMonad
{
    public static class ReaderExt
    {
        public static Reader<TEnv, TOut> Select<TEnv, TIn, TOut>(
            this Reader<TEnv, TIn> first,
            Func<TIn, TOut> selector)
            => first.Map(selector);

        public static Reader<TEnv, TOut> SelectMany<TEnv, TIn, TOut>(
           this Reader<TEnv, TIn> first,
           Func<TIn, Reader<TEnv, TOut>> selector)
           => first.Fmap(selector);

        public static Reader<TEnv, TOut> SelectMany<TEnv, TIn, TBind, TOut>(
            this Reader<TEnv, TIn> first,
            Func<TIn, Reader<TEnv, TBind>> collectionSelector,
            Func<TIn, TBind, TOut> resultSelector)
            => first.Fmap(value => collectionSelector(value)
                                   .Map(secondValue => resultSelector(value, secondValue)));

    }
}
