using System;
using System.Collections.Generic;

namespace SimpleMonad
{
    /*
     * Writer Monad usefull for havings logs without side effects
     * or for writing to a database, or event sourcing...
     * 
     * TVal for compute value and TLog for the event source
     * */
    public class Writer<TVal, TLog>
    {
        public TVal Value { get; }

        public IEnumerable<TLog> Log { get; }

        public Writer(TVal value, TLog logItem) : this(value, new[] { logItem })
        {

        }

        public Writer(TVal value, IEnumerable<TLog> log)
        {
            this.Value = value;
            this.Log = log;
        }

        public Writer(TVal value)
        {
            this.Value = value;
            this.Log = new List<TLog>();
        }

        public Writer<R, TLog> Map<R>(Func<TVal, R> func)
        {
            return func(this.Value);
        }

        public Writer<R, TLog> Fmap<R>(Func<TVal, Writer<R, TLog>> func)
        {
            var newWriter = func(this.Value);
            var newLog = new List<TLog>(this.Log);
            newLog.AddRange(newWriter.Log);

            return new Writer<R, TLog>(newWriter.Value, newLog);
        }

        public static Writer<TVal, TLog> Init(TVal v)
        {
            return new Writer<TVal, TLog>(v);
        }


        public static implicit operator Writer<TVal, TLog>(TVal value)
            => new Writer<TVal, TLog>(value);

    }
}
