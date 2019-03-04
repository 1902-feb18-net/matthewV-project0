using System;
using System.Collections.Generic;


 namespace Project0.Library.Models
{
    public static class DictionaryComparison
    {
        //assumes null = null is false
        static public bool DictionaryEquals<TKey, TValue>(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            // early-exit checks
            if (null == y || null == x) //assumes null = null is false
                return false;
            if (ReferenceEquals(x, y)) //if literally the same object
                return true;
            if (x.Count != y.Count) //different sizes means they can't be equal
                return false;

            // first is to check keys are the same, second is for value
            foreach (TKey k in x.Keys) {
                if (!y.ContainsKey(k) || !x[k].Equals(y[k])) {
                    return false;
                }
            }

            return true;
        }
    }
}
