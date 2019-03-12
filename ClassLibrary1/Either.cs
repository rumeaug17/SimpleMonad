using System;

namespace SimpleMonad
{
    /// <summary>
    /// Standard almost functional way to handle two return value from function or method.
    /// One for error and one for value.
    /// Left hand side is normally the error one and Right hand side the value one.
    /// </summary>
    /// <typeparam name="TLeft">The Left part, often the error</typeparam>
    /// <typeparam name="TRight">The Right part, often the normal value</typeparam>
    public class Either<TLeft, TRight>
    {
        public readonly TLeft Left;
        public readonly TRight Right;
        public readonly bool IsLeft;

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);

        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);

        public Either(TLeft left)
        {
            this.Left = left;
            this.IsLeft = true;
        }

        public Either(TRight right)
        {
            this.Right = right;
            this.IsLeft = false;
        }

        public Either(Either<TLeft, TRight> other)
        {
            this.IsLeft = other.IsLeft;
            this.Left = other.Left;
            this.Right = other.Right;
        }

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
            => this.IsLeft ? leftFunc(this.Left) : rightFunc(this.Right);

        public Either<TLeft, TRightOther> RightMap<TRightOther>(Func<TRight, TRightOther> rightFunc)
            => this.IsLeft ? new Either<TLeft, TRightOther>(this.Left) : rightFunc(this.Right);

        public Either<TLeftOther, TRight> LeftMap<TLeftOther>(Func<TLeft, TLeftOther> leftFunc)
            => this.IsLeft ? leftFunc(this.Left) : new Either<TLeftOther, TRight>(this.Right);

        // default is right map
        public Either<TLeft, TRightOther> Map<TRightOther>(Func<TRight, TRightOther> rightFunc)
            => this.RightMap(rightFunc);

        public Either<TLeft, TRightOther> Fmap<TRightOther>(Func<TRight, Either<TLeft, TRightOther>> rightFunc)
            => this.IsLeft ? this.Left : rightFunc(this.Right);
    }
}
