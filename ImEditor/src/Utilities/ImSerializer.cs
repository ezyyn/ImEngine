using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Utilities
{
    public static class ImSerializer
    {
        public static void ToFileXml<T>(T instance, string path)
        {
            try
            {
                var fs = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs, instance);
                fs.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static T FromFileXml<T>(string path)
        {
            try
            {
                var fs = new FileStream(path, FileMode.Open);
                var serializer = new DataContractSerializer(typeof(T));
                T result = (T)serializer.ReadObject(fs);
                fs.Close();

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
        }

       /* public static void ToFileJson<T>(T instance, string path)
        {
            try
            {
                var tw = new StreamWriter(path);

                var serializer = JsonSerializer.Create(new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                serializer.Serialize(tw, instance);

                tw.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static T FromFileJson<T>(string path)
        {
            try
            {
                var tr = new StreamReader(path);

                var serializer = JsonSerializer.CreateDefault();
                T result = (T)serializer.Deserialize(tr, typeof(T));
                tr.Close();
                return result; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
        }*/
    }
}
