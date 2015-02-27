﻿using System;
namespace sharpbox.Localization.Model
{
    [Serializable]
    public class Resource
    {
        public int ResourceId { get; set; }
        public ResourceNames Name { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Taken from CultureInfo (Thread.CurrentThread.CurrentCulture.Name)
        /// @SEE: https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.name(v=vs.110).aspx
        /// </summary>
        public string CultureCode { get; set; }
    }
}