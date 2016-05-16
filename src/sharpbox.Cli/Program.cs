using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.App;
using sharpbox.Cli.Helper;
using sharpbox.Cli.Model;

namespace sharpbox.Cli
{
    using Dispatch.Model;

    using Io.Model;

    class Program
    {
        static void Main(string[] args)
        {
            var context = new App.AppContext();
                context.AppWiring.WireDefaultRoutes<ExampleModel>(context.Dispatch);
                context.DataPath = "";
                context.File.DataPath = "";

            var api = new Api.Client<ExampleModel>(context);

            
            var list = new List<ExampleModel>();
            list.Add(new ExampleModel() { ExampleModelId = 9, Age = 1, BirthDate = DateTime.Now.AddDays(1), FirstName = "Sally", LastName = "Ranch", Value = "A", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 8, Age = 2, BirthDate = DateTime.Now.AddDays(2), FirstName = "Mark", LastName = "Resiling", Value = "B", ExampleChildId = 3 });
            list.Add(new ExampleModel() { ExampleModelId = 7, Age = 3, BirthDate = DateTime.Now.AddDays(3), FirstName = "Jason", LastName = "Brooks", Value = "C", ExampleChildId = 2 });
            list.Add(new ExampleModel() { ExampleModelId = 6, Age = 4, BirthDate = DateTime.Now.AddDays(4), FirstName = "Alex", LastName = "Tinsley", Value = "D", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 5, Age = 5, BirthDate = DateTime.Now.AddDays(5), FirstName = "Brian", LastName = "Walker", Value = "E", ExampleChildId = 4 });
            list.Add(new ExampleModel() { ExampleModelId = 4, Age = 6, BirthDate = DateTime.Now.AddDays(6), FirstName = "Steven", LastName = "Stokes", Value = "F", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 3, Age = 7, BirthDate = DateTime.Now.AddDays(7), FirstName = "Mike", LastName = "Jackson", Value = "G", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 2, Age = 8, BirthDate = DateTime.Now.AddDays(8), FirstName = "Nick", LastName = "Lancaster", Value = "H", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 1, Age = 9, BirthDate = DateTime.Now.AddDays(9), FirstName = "Josh", LastName = "Holmes", Value = "I", ExampleChildId = 1 });

            //var response = api.Execute(DefaultContext.UpdateAll, new object[] { list });
            //Console.WriteLine(response.Message);

            var items = api.Query(App.AppContext.Get);
            var itemList = items.ToList();
            var foo = itemList.ToJson();

            Console.WriteLine(foo);

            //var items = context.Dispatch.Fetch<ExampleModel>(DefaultContext.Get, null);
            Console.WriteLine(itemList.Count);
            Console.ReadLine();
        }
    }
}
