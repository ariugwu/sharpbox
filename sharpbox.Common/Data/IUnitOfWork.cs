namespace sharpbox.Common.Data
{
  public interface IUnitOfWork<T>
  {
    string ConnectionStringName { get; set; }
    T Add(T instance);

    T Update(T instance);

    T Remove(T instance);
  }
}