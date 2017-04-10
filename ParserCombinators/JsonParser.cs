using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.JsonObjects;

namespace ParserCombinators
{
    public class JsonParser
    {
        public JsonObject Deserialize(string jsonString)
        {
            JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();            
            var jsonObject = ParserObject(jsonObjectBuilder);
            var result = jsonObject(jsonString);
            return jsonObjectBuilder.MainJsonObject;
        }

        public string Serialize(JsonObject jsonObject)
        {
            JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
            StringBuilder stringBuilder = new StringBuilder();
            SerializeJsonObject(jsonObject, stringBuilder);
            return stringBuilder.ToString();
        }

        private void SerializeArray(JsonArray jsonArray, StringBuilder stringBuilder)
        {
            stringBuilder.Append("'");
            stringBuilder.Append(jsonArray.Name);
            stringBuilder.Append("'");
            stringBuilder.Append(" : ");
            stringBuilder.Append("[");
            foreach (var arrayItem in jsonArray.Values)
            {
                stringBuilder.Append(arrayItem);
                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append("]");
        }

        private void SerializeJsonObject(JsonObject jsonObject, StringBuilder stringBuilder)
        {            
            stringBuilder.Append("{");
            foreach (var property in jsonObject.Properties)
            {
                stringBuilder.Append("'");
                stringBuilder.Append(property.Name);
                stringBuilder.Append("'");
                stringBuilder.Append(" : ");
                stringBuilder.Append(property.Value);
                stringBuilder.Append(", ");
            }            
            foreach (var array in jsonObject.Arrays)
            {
                SerializeArray(array, stringBuilder);
            }
            if (jsonObject.Properties.Count > 0 && jsonObject.Arrays.Count == 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            SerializeObject(jsonObject.Objects, stringBuilder);            
            stringBuilder.Append("}");            
        }

        private void SerializeObject(Dictionary<string, JsonObject> jsonObjects, StringBuilder stringBuilder)
        {
            if (jsonObjects.Count > 0)
            {
                stringBuilder.Append(", ");
                foreach (var jsonObject in jsonObjects)
                {
                    stringBuilder.Append("'");
                    stringBuilder.Append(jsonObject.Key);
                    stringBuilder.Append("'");
                    stringBuilder.Append(" : ");
                    SerializeJsonObject(jsonObject.Value, stringBuilder);
                }
                //stringBuilder.Append("}");   
            }
        }

        private Parse<string> ParserObject(JsonObjectBuilder jsonObjectBuilder)
        {
            var start = Parser.Literal("{").CreateJsonObject(jsonObjectBuilder);

            var end = Parser.Literal("}").EndJsonObject(jsonObjectBuilder);

            var stringParseName = Parser.Literal("'").
                And_(_ => Parser.StringValue().CreateName(jsonObjectBuilder).
                And_(__ => Parser.Literal("'")));

            var parseName = Parser.StringValue().CreateName(jsonObjectBuilder);

            var stringParseValueOrParseName = stringParseName.Or(parseName);

            var doubleParserValue = Parser.DecimalString().CreateValue(jsonObjectBuilder);

            var boolParserValue = Parser.BoolString().CreateValue(jsonObjectBuilder);

            var stringParseValue = Parser.Literal("'").
               And_(_ => Parser.StringValue().CreateValue(jsonObjectBuilder).
               And_(__ => Parser.Literal("'")));
            
            var parseValue = Parser.StringValue().CreateValue(jsonObjectBuilder);

            var stringParseValueOrParseValue = stringParseValue.Or(parseValue).Or(doubleParserValue);
                //Or(boolParserValue)           

            var stringArrayParseValue = Parser.Literal("'").
               And_(_ => Parser.StringValue().CreateArrayValue(jsonObjectBuilder).
               And_(__ => Parser.Literal("'")));

            var parseArrayValue = Parser.StringValue().CreateArrayValue(jsonObjectBuilder);

            var doubleArrayParserValue = Parser.DecimalString().CreateArrayValue(jsonObjectBuilder);

            //var boolArrayParserValue = Parser.BoolString().CreateArrayValue(jsonObjectBuilder);

            var stringArrayParseValueOrParseValue = stringArrayParseValue.Or(parseArrayValue).Or(doubleArrayParserValue);
                //Or(boolArrayParserValue);

            var keyStringValuePairObject =
                stringParseValueOrParseName.Then_(_ => Parser.Literal(":").Then_(__ => stringParseValueOrParseValue));
                //Then_(_ => stringParseValueOrParseValue);

            var innerValuePropertyObject = stringParseValueOrParseValue.
                Then_(_ => Parser.Literal(","));

            var innerArrayValuePropertyObject = stringArrayParseValueOrParseValue.
               Then_(_ => Parser.Literal(","));

            var keyStringArrayValueObject = stringParseValueOrParseName.Then_(_ => Parser.Literal(":")).
                Then_(__ =>
                Parser.Literal("[").CreateArray(jsonObjectBuilder).
                Repeat(innerArrayValuePropertyObject).Then_(_ => Parser.Literal("]")));

            var keyStringPropertyOrKeyStringArray = keyStringArrayValueObject.Or(keyStringValuePairObject);

            return keyStringPropertyOrKeyStringArray.
                Then_(_ => Parser.Literal(":")).
                Then_(_ => start).
                Then_(__ => end.Or(keyStringValuePairObject).Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder))).
                Repeat(Parser.Literal(",").
                And_(_ => end.Or(keyStringValuePairObject).Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder)))).
                Then_(___ => end);
        }
    }
}
