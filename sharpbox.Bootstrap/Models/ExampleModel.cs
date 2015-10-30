namespace sharpbox.Bootstrap.Models
{
    using System;

    [Serializable]
    public class ExampleModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public string Value { get; set; }

        public static ExampleModel TestTargetMethod(ExampleModel exampleModel)
        {
            exampleModel.Value = exampleModel.Value + "...I changed this";
            return exampleModel;
        }
    }
}