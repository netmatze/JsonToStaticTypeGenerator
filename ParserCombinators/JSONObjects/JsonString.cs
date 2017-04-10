using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.JsonObjects
{
    public class JsonString : IValue
    {
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public JsonString(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
