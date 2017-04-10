using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.JsonObjects;
using System.Reflection;

namespace JsonConverter
{
    public class JsonConverter<T> where T : JsonStaticObject, new()
    {
        public T Deserialize(string jsonString) 
        {
            JsonStaticObject t = (JsonStaticObject)new T();            
            JsonParser jsonParser = new JsonParser();
            JsonObject jsonObject = jsonParser.Deserialize(jsonString);
            var propertyInfo = t.GetType().GetField("jsonObject", 
                BindingFlags.Instance | BindingFlags.SetField | BindingFlags.NonPublic); 
            propertyInfo.SetValue(t, jsonObject);            
            JsonSource.JsonObject_ = jsonObject;
            return (T)t;
        }        
    }
}
