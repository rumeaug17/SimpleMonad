using System;

namespace SimpleMonad
{
    public class Option<T>
    {
        public T Value { get; }

        public bool HasValue { get; }

        public Option(T value) : this()
        {
            if (value != null)
            {
                this.Value = value;
                this.HasValue = true;
            }
        }

        public Option()
        {
            this.Value = default(T);
            this.HasValue = false;
        }

        public Option<U> Map<U>(Func<T, U> func) 
            => this.HasValue ? func(this.Value) : new Option<U>();

        public Option<U> Fmap<U>(Func<T, Option<U>> func) 
            => this.HasValue ? func(this.Value) : new Option<U>();

        public T Reduce(T defaultValue) 
            => this.HasValue ? this.Value : defaultValue;

        public U Match<U>(Func<T, U> someFunc, Func<U> noneFunc) 
            => this.HasValue ? someFunc(this.Value) : noneFunc();

        public static implicit operator Option<T>(T value) 
            => new Option<T>(value);
    }

}
