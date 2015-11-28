namespace sharpbox.WebLibrary.Core.Models
{
    using System;

    [Serializable]
    public class ExampleChild
    {
        public int ExampleChildId { get; set; }
        public string Title { get; set; }

        public bool IsValid { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ARandomNumber { get; set; }

        public double SomeDoubleNumber { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}