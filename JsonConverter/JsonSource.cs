using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.JsonObjects;

namespace JsonConverter
{
    public static class JsonSource
    {
        private static JsonObject jsonObject_;

        public static JsonObject JsonObject_
        {
            get { return jsonObject_; }
            set { jsonObject_ = value; }
        }

        public static T1 Find<T1>(string objectname, string name)
        {
            JsonObject localJsonObject = Find(jsonObject_, objectname);
            if (localJsonObject == null)
            {
                localJsonObject = jsonObject_;
            }
            foreach (var property in localJsonObject.Properties)
            {
                if (property.Name == name)
                {
                    return (T1)property.Value;
                }
            }            
            return default(T1);
        }

        public static JsonObject Find(JsonObject jsonObject, string objectname)
        {
            foreach (var localjsonObject in jsonObject_.Objects)
            {
                if (localjsonObject.Key == objectname)
                {
                    return localjsonObject.Value;
                }
                foreach (var innerJsonObject in localjsonObject.Value.Objects)
                {
                    return Find(innerJsonObject.Value, objectname);
                }
            }
            return null;
        }

        public static T1 FindObject<T1>(string objectname, string name)
        {
            foreach (var obj in jsonObject_.Objects)
            {
                if (obj.Key == name)
                {                    
                    object innerObj = Activator.CreateInstance(typeof(T1));                   
                    return (T1)innerObj;
                }
            }
            return default(T1);
        }

        public static string[] FindArrayString(string objectname, string name)
        {
            JsonObject localJsonObject = Find(jsonObject_, objectname);
            if (localJsonObject == null)
            {
                localJsonObject = jsonObject_;
            }
            foreach (JsonArray array in localJsonObject.Arrays)
            {
                if (array.Name == name)
                {
                    List<string> list = new List<string>();
                    foreach (IValue arrayItem in array.Values)
                    {                       
                        list.Add(((JsonString)arrayItem).Value);                     
                    }
                    return list.ToArray();
                }
            }
            return default(string[]);
        }

        public static double[] FindArrayNumber(string objectname, string name)
        {
            JsonObject localJsonObject = Find(jsonObject_, objectname);
            if (localJsonObject == null)
            {
                localJsonObject = jsonObject_;
            }
            foreach (JsonArray array in localJsonObject.Arrays)
            {
                if (array.Name == name)
                {
                    List<double> list = new List<double>();
                    foreach (IValue arrayItem in array.Values)
                    {
                        list.Add((double)((JsonNumber)arrayItem).Value);
                    }
                    return list.ToArray();
                }
            }
            return default(double[]);
        }

        public static bool[] FindArrayBool(string objectname, string name)
        {
            JsonObject localJsonObject = Find(jsonObject_, objectname);
            if (localJsonObject == null)
            {
                localJsonObject = jsonObject_;
            }
            foreach (JsonArray array in localJsonObject.Arrays)
            {
                if (array.Name == name)
                {
                    List<bool> list = new List<bool>();
                    foreach (IValue arrayItem in array.Values)
                    {
                        list.Add(((JsonBool)arrayItem).Value);
                    }
                    return list.ToArray();
                }
            }
            return default(bool[]);
        }
    }
}
