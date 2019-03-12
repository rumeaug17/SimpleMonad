using System;

namespace SimpleMonad
{
    public class Try<T> : Either<Exception, T>
    {
        public Try(Exception ex) : base(ex)
        {
        }

        public Try(T value) : base(value)
        {
        }

        public Try(Either<Exception, T> either) : base(either)
        {
        }

        public T Value
        {
            get
            {
                if (this.IsLeft)
                {
                    throw this.Left;
                }

                return this.Right;
            }
        }

        public Exception ExceptionValue => this.Left;

        public bool IsValue => !this.IsLeft;

        public bool IsException => this.IsLeft;

        public Try<TOut> AndThen<TOut>(Func<T, TOut> rightFunc) => new Try<TOut>(this.Map(rightFunc));
    }
}
