﻿using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.Dispatch
{
    /// <summary>
    /// This will be the list that all calls within the application will use to register their dispatch requests. The Dispatch module has the built action defined. This is for extension.
    /// </summary>
    public class PublicationNamesExtension : PublisherNames
    {
        public static readonly PublisherNames ExampleExtendedPublisher = new PublisherNames("ExampleExtendedPublisher");


        #region Field(s)

        private static List<PublisherNames> _extendedPublisherList; 

        #endregion
        #region Properties

        public static List<PublisherNames> ExtendedPubList
        {
            get
            {
                if (_extendedPublisherList != null) return _extendedPublisherList;
                // Grab the default list
                var pubList = DefaultPubList();

                // Create a list of our app specific publishers.
                var extensionList = new List<PublisherNames>()
                {
                    ExampleExtendedPublisher
                };

                // Add them to the default list
                pubList.AddRange(extensionList);

                // return
                return (_extendedPublisherList = pubList);
            }
        }

        #endregion
    }
}