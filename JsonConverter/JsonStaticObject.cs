using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.JsonObjects;

namespace JsonConverter
{
    public abstract class JsonStaticObject
    {
        protected string jsonString;

        protected JsonObject jsonObject;

        protected virtual JsonObject JsonObject_
        {
            get { return jsonObject; }
            set 
            {
                jsonObject = value;                
            } 
        }
    }
}
