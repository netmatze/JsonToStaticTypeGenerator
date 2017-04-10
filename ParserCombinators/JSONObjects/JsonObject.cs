using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators.JsonObjects
{
    public class JsonObject : IValue
    {
        //private Dictionary<string, object> properties = new Dictionary<string, object>();

        private List<JsonProperty> properties = new List<JsonProperty>();

        public List<JsonProperty> Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        private Dictionary<string, JsonObject> objects = new Dictionary<string, JsonObject>();

        public Dictionary<string, JsonObject> Objects
        {
            get { return objects; }
            set { objects = value; }
        }

        private List<JsonArray> arrays = new List<JsonArray>();

        public List<JsonArray> Arrays
        {
            get { return arrays; }
            set { arrays = value; }
        }

        private JsonObject parentJsonObject;

        public JsonObject ParentJsonObject
        {
            get { return parentJsonObject; }
            set { parentJsonObject = value; }
        }

        //public Dictionary<string, object> Properties
        //{
        //    get { return properties; }
        //    set { properties = value; }
        //}
    }
}
