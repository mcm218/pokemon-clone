namespace _Scripts.Pokemon {
    public interface ISaveable<T>
    {
        void Save();
        static T Load()
        {
            return default;
        }
    }
}