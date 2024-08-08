using BookLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookLib
{
    /// <summary>
    /// The Repository class provides functionality for saving and loading data to and from XML files using serialization.
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Saves the specified data object to an XML file using serialization.
        /// </summary>
        /// <typeparam name="T">The type of the data object.</typeparam>
        /// <param name="data">The data object to be saved.</param>
        /// <param name="fileName">The path to the XML file where the data will be saved.</param>
        public void SaveData<T>(T data, string fileName)
        {
            var xml = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(fileName))
                xml.Serialize(sw, data);
        }

        /// <summary>
        /// Loads data from the specified XML file using deserialization.
        /// </summary>
        /// <typeparam name="T">The type of the data object.</typeparam>
        /// <param name="fileName">The path to the XML file from which the data will be loaded.</param>
        /// <returns>T</returns>
        public T LoadData<T>(string fileName)
        {
            var xml = new XmlSerializer(typeof(T));
            using (var sr = new StreamReader(fileName))
                return (T) xml.Deserialize(sr);
        }
    }
}
