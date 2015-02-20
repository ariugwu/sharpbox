namespace sharpbox.Io.Strategy
{
    public interface IStrategy
    {
        void Write<T>();
        T Read<T>();
        void Delete<T>();
    }
}
