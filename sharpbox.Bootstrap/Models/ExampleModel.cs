﻿namespace sharpbox.Bootstrap.Models
{
    using System;

    [Serializable]
    public class ExampleModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        //public bool IsAlive { get; set; } //TODO: Need to fix form to send "true" instead of "on", otherwise it fails the model binding validation.
        public string Value { get; set; }

        public static ExampleModel TestTargetMethod(ExampleModel exampleModel)
        {
            exampleModel.Value = exampleModel.Value + "...I changed this";
            return exampleModel;
        }
    }
}