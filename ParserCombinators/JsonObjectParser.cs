using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using ParserCombinators.JsonObjects;

namespace ParserCombinators
{
    public static class JsonObjectParser
    {
        public static Parse<string> ParserObject(JsonObjectBuilder jsonObjectBuilder)
        {
            var keyStringValuePairObject =
                Parser.StringValue().CreateName(jsonObjectBuilder).Then_(_ => Parser.Literal(":")).
                Then_(_ => Parser.StringValue().CreateValue(jsonObjectBuilder));

            var innerValuePropertyObject =                
                Parser.StringValue().CreateArrayValue(jsonObjectBuilder).
                Then_(_ => Parser.Literal(",").Then_(__ => Parser.StringValue().CreateArrayValue(jsonObjectBuilder)));

            var keyStringArrayValueObject = 
                Parser.StringValue().CreateName(jsonObjectBuilder).Then_(_ => Parser.Literal(":")).Then_(___ =>
                Parser.Literal("[").CreateArray(jsonObjectBuilder).
                And_(_ => innerValuePropertyObject).And_(_ => Parser.Literal("]")));

            return Parser.StringValue().CreateName(jsonObjectBuilder).
                    Then_(_ => Parser.Literal(":")).
                    Then_(_ => Parser.Literal("{").CreateJsonObject(jsonObjectBuilder)).
                    Then_(__ => keyStringValuePairObject.Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder))).
                    Repeat(Parser.Literal(",").
                    And_(_ => keyStringValuePairObject.Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder)))).
                    Then_(___ => Parser.Literal("}").EndJsonObject(jsonObjectBuilder));
        }

        public static Parse<string> ParserObjectWithDoubleHighComma(JsonObjectBuilder jsonObjectBuilder)
        {
            var stringParseName = Parser.Literal("\"").
                And_(_ => Parser.StringValue().CreateName(jsonObjectBuilder).
                And_(__ => Parser.Literal("\"")));

            var parseName = Parser.StringValue().CreateName(jsonObjectBuilder);

            var stringParseValueOrParseName = stringParseName.Or(parseName);

            var stringParseValue = Parser.Literal("\"").
               And_(_ => Parser.StringValue().CreateValue(jsonObjectBuilder).
               And_(__ => Parser.Literal("\"")));

            var parseValue = Parser.StringValue().CreateValue(jsonObjectBuilder);

            var stringParseValueOrParseValue = stringParseValue.Or(parseValue);

            var keyStringValuePairObject =
                stringParseValueOrParseName.Then_(_ => Parser.Literal(":")).
                Then_(_ => stringParseValueOrParseValue);

            var innerValuePropertyObject = Parser.StringValue().CreateArrayValue(jsonObjectBuilder).
                Then_(_ => Parser.Literal(",").Then_(__ => Parser.StringValue().CreateArrayValue(jsonObjectBuilder)));

            var keyStringArrayValueObject =
                Parser.Literal("[").CreateArray(jsonObjectBuilder).
                And_(_ => innerValuePropertyObject).And_(_ => Parser.Literal("]"));

            return stringParseValueOrParseName.
                Then_(_ => Parser.Literal(":")).
                Then_(_ => Parser.Literal("{").CreateJsonObject(jsonObjectBuilder)).
                Then_(__ => keyStringValuePairObject.Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder))).
                Repeat(Parser.Literal(",").
                And_(_ => keyStringValuePairObject.Or(keyStringArrayValueObject).Or(ParserObject(jsonObjectBuilder)))).
                Then_(___ => Parser.Literal("}").EndJsonObject(jsonObjectBuilder));
        }

        public static Parse<T> CreateJsonObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
                {
                    ParseResult<T> result = parse(value);
                    if (result.Succeeded)
                    {
                        jsonObjectBuilder.Create(new JsonObject());
                    }
                    return result;
                };
        }

        //public static Parse<T> CreateJsonSubObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //        {
        //            ParseResult<T> result = parse(value);
        //            if (result.Succeeded)
        //            {
        //                jsonObjectBuilder.CreateSubObject(new JsonObject());
        //            }
        //            return result;
        //        };
        //}

        public static Parse<T> EndJsonObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    jsonObjectBuilder.EndObject();
                }
                return result;
            };
        }

        //public static Parse<T> EndJsonSubObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //    {
        //        ParseResult<T> result = parse(value);
        //        if (result.Succeeded)
        //        {
        //            jsonObjectBuilder.EndSubObject();
        //        }
        //        return result;
        //    };
        //}

        public static Parse<string> CreateName(this Parse<string> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
                {
                    ParseResult<string> result = parse(value);
                    if (result.Succeeded)
                    {
                        jsonObjectBuilder.AddName(result.Result);
                    }
                    return result;
                };
        }

        //public static Parse<string> CreateNameSubObject(this Parse<string> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //        {
        //            ParseResult<string> result = parse(value);
        //            if (result.Succeeded)
        //            {
        //                jsonObjectBuilder.AddNameSubObject(result.Result);
        //            }
        //            return result;
        //        };
        //}

        public static Parse<T> CreateValue<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
                {
                    ParseResult<T> result = parse(value);
                    if (result.Succeeded)
                    {
                        double decValue;
                        var culture = new CultureInfo("en-gb");
                        bool boolValue;
                        if (double.TryParse(result.Result.ToString(), NumberStyles.Number, culture, out decValue))
                        {
                            jsonObjectBuilder.AddValue(decValue, new JsonNumber(decValue.ToString()));
                        }
                        else if (bool.TryParse(result.Result.ToString(), out boolValue))
                        {
                            jsonObjectBuilder.AddValue(boolValue, new JsonBool(boolValue));
                        }
                        else
                        {
                            var str =
                                result.Result.ToString().Replace("'", "").ToString();
                            jsonObjectBuilder.AddValue(str, new JsonString(str));
                        }
                    }
                    return result;
                };
        }

        //public static Parse<string> CreateValueSubObject(this Parse<string> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //        {
        //            ParseResult<string> result = parse(value);
        //            if (result.Succeeded)
        //            {
        //                double decValue;
        //                var culture = new CultureInfo("en-gb");
        //                bool boolValue;
        //                if (double.TryParse(result.Result.ToString(), NumberStyles.Number, culture, out decValue))
        //                {
        //                    jsonObjectBuilder.AddValueSubObject(decValue);
        //                }
        //                else if (bool.TryParse(result.Result.ToString(), out boolValue))
        //                {
        //                    jsonObjectBuilder.AddValueSubObject(boolValue);
        //                }
        //                else
        //                {
        //                    jsonObjectBuilder.AddValueSubObject(result.Result);
        //                }
        //            }                
        //            return result;
        //        };
        //}

        public static Parse<T> CreateArray<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
                {
                    ParseResult<T> result = parse(value);
                    if (result.Succeeded)
                    {
                        jsonObjectBuilder.AddArray();
                    }
                    return result;
                };
        }

        public static Parse<T> CreateArrayValue<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        {
            return value =>
                {
                    ParseResult<T> result = parse(value);
                    if (result.Succeeded)
                    {
                        double decValue;
                        var culture = new CultureInfo("de-de");
                        bool boolValue;
                        if (double.TryParse(result.Result.ToString(), NumberStyles.Number, culture, out decValue))
                        {
                            jsonObjectBuilder.AddArrayValue(decValue);
                        }
                        else if (bool.TryParse(result.Result.ToString(), out boolValue))
                        {
                            jsonObjectBuilder.AddArrayValue(boolValue);
                        }
                        else
                        {
                            jsonObjectBuilder.AddArrayValue(result.Result.ToString());
                        }
                    }
                    return result;
                };
        }

        //public static Parse<T> CreateArraySubObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //        {
        //            ParseResult<T> result = parse(value);
        //            if (result.Succeeded)
        //            {
        //                jsonObjectBuilder.AddArraySubObject();
        //            }
        //            return result;
        //        };
        //}

        //public static Parse<T> CreateArrayValueSubObject<T>(this Parse<T> parse, JsonObjectBuilder jsonObjectBuilder)
        //{
        //    return value =>
        //        {
        //            ParseResult<T> result = parse(value);
        //            if (result.Succeeded)
        //            {
        //                jsonObjectBuilder.AddArrayValueSubObject(result.Result.ToString());
        //            }
        //            return result;
        //        };
        //}
    }
}
