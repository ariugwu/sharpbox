namespace sharpbox.Bootstrap.Models
{
    using System;

    [Serializable]
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