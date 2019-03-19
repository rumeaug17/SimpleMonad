using System;

namespace SimpleMonad
{
    /* Reader monad is a kind of DI (Ioc)
     * For transporting configuration for example
     * It is a special case of State Monad
     * Reader can read the enbv but not change it.
     * State can read and change it
     * */
    public class Reader<TEnv, T>
    {
        public Func<TEnv, T> Run { get; }
       
        public Reader(Func<TEnv, T> func)
        {
            this.Run = func;
        }

        public Reader<TEnv, TOut> Map<TOut>(Func<T, TOut> f) 
            => new Reader<TEnv, TOut>(env => f(this.Run(env)));

        public Reader<TEnv, TOut> Fmap<TOut>(Func<T, Reader<TEnv, TOut>> f) 
            => new Reader<TEnv, TOut>(
                env =>
                {
                    var a = this.Run(env);
                    return f(a).Run(env);
                });
    }
}
