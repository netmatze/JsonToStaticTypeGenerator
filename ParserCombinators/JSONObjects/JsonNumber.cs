using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.JsonObjects
{
    public class JsonNumber : IValue
    {
        private decimal value;

        public decimal Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public JsonNumber(string value)
        {
            if (value.Contains('E') || value.Contains('e'))
            {               
                double doubleValue = double.Parse(value);
                this.value = Convert.ToDecimal(doubleValue);
            }
            else
            {
                this.value = decimal.Parse(value);
            }
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
