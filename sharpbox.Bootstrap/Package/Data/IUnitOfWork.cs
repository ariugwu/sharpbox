namespace sharpbox.WebLibrary.Data
{
    public interface IUnitOfWork<T>
    {
        T Insert(T instance);

        T Update(T instance);

        T Delete(T instance);
    }
}