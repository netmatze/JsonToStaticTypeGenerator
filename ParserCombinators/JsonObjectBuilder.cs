using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.JsonObjects;

namespace ParserCombinators
{
    public class JsonObjectBuilder
    {
        public JsonObjectBuilder()
        {
            this.mainjsonObject = new JsonObject();
        }

        private JsonObject mainjsonObject;

        public JsonObject MainJsonObject
        {
            get { return mainjsonObject; }
            set { mainjsonObject = value; }
        }

        private JsonObject actualObject;

        public void Create(JsonObject jsonObject)
        {
            if (actualObject == null)
            {
                this.actualObject = this.mainjsonObject;    
            }
            else
            {
                jsonObject.ParentJsonObject = actualObject;
                this.actualObject.Objects.Add(keyValuePairName, jsonObject);
                this.actualObject = jsonObject;
            }
        }

        //public void CreateSubObject(JsonObject jsonObject)
        //{
        //    this.actualObject.Objects.Add(keyValuePairName, jsonObject);
        //    this.actualObject = jsonObject;            
        //}

        //public void EndSubObject()
        //{
        //    //this.actualObject = parentObject;
        //}

        public void EndObject()
        {
            this.actualObject = this.actualObject.ParentJsonObject;
        }

        private string keyValuePairName = String.Empty;

        public void AddName(string name)
        {
            this.keyValuePairName = name;
        }

        //private string keyValuePairNameSubObject = String.Empty;

        //public void AddNameSubObject(string name)
        //{
        //    this.keyValuePairNameSubObject = name;
        //}

        public void AddValue<T>(T value, IValue type)
        {            
            JsonProperty jsonProperty = new JsonProperty(keyValuePairName, value, type);
            actualObject.Properties.Add(jsonProperty);         
        }

        //public void AddValueSubObject<T>(T value)
        //{
        //    JsonProperty jsonProperty = new JsonProperty(keyValuePairNameSubObject, value);
        //    actualObject.Properties.Add(jsonProperty);
        //}

        public void AddArray()
        {
            this.actualObject.Arrays.Add(new JsonArray(this.keyValuePairName));            
        }

        //public void AddArraySubObject()
        //{
        //    this.actualObject.Arrays.Add(new JsonArray());
        //}
        
        public void AddArrayValue(string value)
        {
            JsonString jsonString = new JsonString(value);
            var array = actualObject.Arrays.ToArray();
            array[array.Length - 1].Values.Add(jsonString);
        }

        public void AddArrayValue(double value)
        {
            JsonNumber jsonNumber = new JsonNumber(value.ToString());
            var array = actualObject.Arrays.ToArray();
            array[array.Length - 1].Values.Add(jsonNumber);
        }

        public void AddArrayValue(bool value)
        {
            JsonBool jsonBool = new JsonBool(value);
            var array = actualObject.Arrays.ToArray();
            array[array.Length - 1].Values.Add(jsonBool);
        }

        //public void AddArrayValueSubObject(string value)
        //{
        //    JsonString jsonString = new JsonString(value);
        //    var array = actualObject.Arrays.ToArray();
        //    array[array.Length - 1].Values.Add(jsonString);
        //}
    }
}
