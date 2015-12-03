using System;
using System.Collections.Generic;
using sharpbox.App;

namespace sharpbox.WebLibrary.Core.Wiring
{
    using System.Linq;

    public interface IAppPersistence
    {
        AppContext AppContext { get; set; }

        T Add<T>(T instance) where T : new();

        T Update<T>(T instance) where T : new();

        List<T> UpdateAll<T>(List<T> items) where T : new();

        T Remove<T>(T instance) where T : new();

        IQueryable<T> Get<T>(object args) where T : new();

        T GetById<T>(string id) where T : new();

    }
}