using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.Bootstrap.Models
{
  public class ExampleModel
  {
    public string Value { get; set; }

    public static ExampleModel TestTargetMethod(ExampleModel exampleModel)
    {
      exampleModel.Value = exampleModel.Value + "...I changed this";
      return exampleModel;
    }
  }
}