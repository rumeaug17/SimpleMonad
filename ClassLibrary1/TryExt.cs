using System;

namespace SimpleMonad
{
    public static class TryExt
    {
        public static Try<T> Try<T>(this T value)
        {
            return new Try<T>(value);
        }

        public static Try<T> Try<T>(this Exception exc)
        {
            return new Try<T>(exc);
        }

        public static Try Try(this Exception exc)
        {
            return new Try(exc);
        }

        public static Try<T> Try<T>(this Either<Exception, T> e)
        {
            return new Try<T>(e);
        }

        public static Try<T> Try<T>(this Func<T> func)
        {
            try
            {
                var value = func();
                return value.Try();
            }
            catch (Exception ex)
            {
                return ex.Try<T>();
            }
        }

        public static Try Try(this Action action)
        {
            try
            {
                action();
                return new Try();
            }
            catch (Exception ex)
            {
                return ex.Try();
            }
        }
    }
}
