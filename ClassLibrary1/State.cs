using System;

namespace SimpleMonad
{

    public class State<T, TState>
    {
        public Func<TState, Tuple<T, TState>> Run { get; set; }

        public Tuple<T, TState> Apply(TState state) => this.Run(state);

        public T Eval(TState state) => this.Apply(state).Item1;

        public State<TOut, TState> Map<TOut>(Func<T, TOut> f)
        {
            return new State<TOut, TState>
            {
                Run = s =>
                {
                    var r = this.Run(s);
                    return Tuple.Create(f(r.Item1), r.Item2);
                }
            };
        }

        public State<TOut, TState> Fmap<TOut>(Func<T, State<TOut, TState>> f)
        {
            return new State<TOut, TState>
            {
                Run = s =>
                {
                    var r = this.Run(s);
                    return f(r.Item1).Apply(r.Item2);
                }
            };
        }

        public static State<T, TState> Init(T v)
        {
            return new State<T, TState>
            {
                Run = s => Tuple.Create(v, s)
            };
        }

        public static State<T, TState> Update(Func<TState, TState> f)
        {
            return new State<T, TState>
            {
                Run = s => Tuple.Create(default(T), f(s))
            };
        }

        public static State<T, TState> GetS(Func<TState, T> f)
        {
            return new State<T, TState>
            {
                Run = s => Tuple.Create(f(s), s)
            };
        }
    }
}
