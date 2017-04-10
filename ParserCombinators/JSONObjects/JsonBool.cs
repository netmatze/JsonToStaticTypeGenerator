using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.JsonObjects
{
    public class JsonBool : IValue
    {
        private bool value;

        public bool Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public JsonBool(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
