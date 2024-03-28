using System;
using System.Collections.Generic;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class CustomDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}
