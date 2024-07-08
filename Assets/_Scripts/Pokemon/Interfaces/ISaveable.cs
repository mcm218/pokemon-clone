using UnityEngine;

namespace _Scripts.Pokemon {
    public interface ISaveable<T>
    {
        Awaitable Save();
        static Awaitable<T> Load(string name)
        {
            return default;
        }

        static Awaitable Delete(string name) {
            return default;
        }
    }
}