using System.Collections.Generic;

namespace sharpbox.Common.App
{
    using System.Linq;

    public interface ICrud
    {
        T Add<T>(T instance) where T : new();

        T UpdateById<T>(T instance) where T : new();

        IList<T> Update<T>(List<T> items) where T : new();

        T Remove<T>(T instance) where T : new();

        IList<T> Get<T>() where T : new();

        T GetById<T>(string id) where T : new();
    }
}