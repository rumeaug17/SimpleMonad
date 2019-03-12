using System;

namespace SimpleMonad
{
    public class Try : Try<bool>
    {
        public Try(Exception ex) : base(ex)
        {
        }

        public Try() : base(true)
        {
        }

        private new bool Value => IsValue;
    }
}
