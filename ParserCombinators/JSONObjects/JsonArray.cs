using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators.JsonObjects
{
    public class JsonArray : IValue
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<IValue> values = new List<IValue>();

        public List<IValue> Values
        {
            get 
            {                
                return values; 
            }
            set 
            { 
                values = value; 
            }
        }

        public JsonArray(params IValue[] arrayValues)
        {            
            foreach (var value in arrayValues)
            {
                values.Add(value);
            }
        }

        public JsonArray(string name, params IValue[] arrayValues) : this(arrayValues)
        {
            this.name = name;            
        }
    }
}
