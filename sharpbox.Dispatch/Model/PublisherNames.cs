using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    public class PublisherNames
    {
        #region Field(s)

        protected string _value;

        #endregion

        public static readonly PublisherNames ExamplePublication = new PublisherNames("ExamplePublication");


        public PublisherNames(string value)
        {
            _value = value;
        }

        public PublisherNames() { }


        public override string ToString()
        {
            return _value;
        }

    }
}
