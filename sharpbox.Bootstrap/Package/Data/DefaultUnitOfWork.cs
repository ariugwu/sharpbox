using System;

namespace sharpbox.WebLibrary.Data
{
  using Common.Data;

  public class DefaultUnitOfWork<T> : IUnitOfWork<T>
      where T : new()
  {
    public string ConnectionStringName { get; set; }
    private string _fileName { get; set; }

    public DefaultUnitOfWork(Io.Client file)
    {
      this.File = file;
      this._fileName = string.Format("{0}.dat", typeof(T).Name);
    }

    public Io.Client File { get; set; }

    public T Add(T instance)
    {
      this.File.Write(this._fileName, instance);

      return instance;
    }

    public T Update(T instance)
    {
      return Add(instance);
    }

    public T Remove(T instance)
    {
      throw new NotImplementedException();
    }
  }
}
