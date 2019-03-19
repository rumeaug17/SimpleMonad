using System;
using System.Collections.Generic;

namespace SimpleMonad
{

    /*
     * A monad have to be construct (with a init or return value)
     * and compose by a bind operation (fmap or select)
     * A monad is almost always a functor with a map operation
     */

    public class Option<T> : IEquatable<Option<T>>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Option<T>);
        }

        public bool Equals(Option<T> other)
        {
            return other != null &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value) &&
                   HasValue == other.HasValue;
        }

        public override int GetHashCode()
        {
            var hashCode = 1816676634;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + HasValue.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            this.HasValue ? $"Some({this.Value})" : "None()";
 
        public static implicit operator Option<T>(T value) 
            => new Option<T>(value);
    }

}
