using System;

namespace SimpleMonad
{
    public static class EitherExt
    {
        public static Either<TLeft, TRight> Where<TLeft, TRight>(
            this Either<TLeft, TRight> source,
            Func<TRight, bool> predicate) 
            => source.Match(
                left => source,
                right => predicate(right) ? source : default(TLeft));

        public static Either<TLeft, TRightOut> Select<TLeft, TRight, TRightOut>(
            this Either<TLeft, TRight> first, 
            Func<TRight, TRightOut> selector)
            => first.Map(selector);

        public static Either<TLeft, TRightOut> SelectMany<TLeft, TRight, TRightOut>(
            this Either<TLeft, TRight> first, 
            Func<TRight, Either<TLeft, TRightOut>> selector)
            => first.Fmap(selector);

        public static Either<TLeft, TRightOut> SelectMany<TLeft, TRight, TRightBind, TRightOut>(
           this Either<TLeft, TRight> first,
           Func<TRight, Either<TLeft, TRightBind>> collectionSelector,
           Func<TRight, TRightBind, TRightOut> resultSelector)
           => first.Fmap(value => collectionSelector(value)
                                  .Fmap(secondValue => new Either<TLeft, TRightOut>(resultSelector(value, secondValue))));
    }
}
