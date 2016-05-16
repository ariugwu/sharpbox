using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.Cli.Model
{
    [Serializable]
    public class ExampleModel
    {
        public int ExampleModelId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        //public bool IsAlive { get; set; } //TODO: Need to fix form to send "true" instead of "on", otherwise it fails the model binding validation.
        public string Value { get; set; }

        public int ExampleChildId { get; set; }

        public static ExampleModel TestTargetMethod(ExampleModel exampleModel)
        {
            exampleModel.Value = exampleModel.Value + "...I changed this";
            return exampleModel;
        }

        public override string ToString()
        {
            return $"{this.LastName},{this.FirstName}";
        }
    }
}