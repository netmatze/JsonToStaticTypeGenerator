using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators.JsonObjects
{
    public static class JsonObjectCreator
    {
        public static IEnumerable<JsonObject> Create()
        {
            for (int i = 0; i < 10; i++)
            {
                JsonObject jsonObject = new JsonObject();
                JsonNumber jsonNumber = new JsonNumber(i.ToString());
                JsonProperty jsonProperty = new JsonProperty(string.Format("number{0}", i), jsonNumber, jsonNumber);
                jsonObject.Properties.Add(jsonProperty);
                
                yield return jsonObject;
            }
            JsonObject jsonObjectArray = new JsonObject();
            JsonString jsonString = new JsonString("array value");
            JsonArray jsonArray = new JsonArray(jsonString, jsonString);
            JsonProperty jsonPropertyArray = new JsonProperty("Array", jsonArray, jsonArray);
            jsonObjectArray.Properties.Add(jsonPropertyArray);
            yield return jsonObjectArray;
        }        
    }
}
