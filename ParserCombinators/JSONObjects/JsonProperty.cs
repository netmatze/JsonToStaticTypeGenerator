using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators.JsonObjects
{
    public class JsonProperty
    {
        private IValue type;

        public IValue Type_
        {
            get { return type; }
            set { type = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private object value;

        public object Value
        {
            get 
            {
                if (value is JsonNumber)
                {
                    return ((JsonNumber)value).Value;
                }
                if (value is JsonString)
                {
                    return ((JsonString)value).Value;
                }
                if (value is JsonBool)
                {
                    return ((JsonBool)value).Value;
                }
                if (value is JsonArray)
                {
                    return ((JsonArray)value).Values;                    
                }
                return this.value; 
            }
            set 
            { 
                this.value = value; 
            }
        }

        public JsonProperty(string name, object value, IValue type)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }
}
