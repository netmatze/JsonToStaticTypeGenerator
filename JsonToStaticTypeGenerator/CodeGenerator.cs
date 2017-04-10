using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.JsonObjects;

namespace JsonToStaticTypeGenerator
{
    //http://jsonclassgenerator.codeplex.com/
    public class CodeGenerator
    {
        public string Generate(JsonObject jsonObject, string namespace_, string objectname)
        {
            StringBuilder stringBuilder = new StringBuilder();
            GenerateStartPart(stringBuilder, namespace_, objectname);
            GenerateProperties(stringBuilder, jsonObject, objectname);
            GenerateArrays(stringBuilder, jsonObject, objectname);
            GenerateObjects(stringBuilder, jsonObject, namespace_);
            GenerateEndPart(stringBuilder);
            return stringBuilder.ToString();
        }

        public void GenerateObjects(StringBuilder stringBuilder, JsonObject jsonObject, string namespace_)
        {
            foreach (var object_ in jsonObject.Objects)
            {
                GenerateObject(stringBuilder, object_.Value, namespace_, object_.Key);
            }
        }

        public void GenerateObject(StringBuilder stringBuilder, JsonObject jsonObject, string namespace_, string objectname)
        {
            stringBuilder.AppendLine("public " + objectname + " " + objectname + 
                "_ { get { return JsonSource.FindObject<" + objectname + ">(\"" + objectname + "\", \"" + objectname + "\"); } }");
            GenerateObjectStartPart(stringBuilder, objectname);
            GenerateProperties(stringBuilder, jsonObject, objectname);
            GenerateObjectEndPart(stringBuilder, objectname);
        }

        public void GenerateProperties(StringBuilder stringBuilder, JsonObject jsonObject, string objectname)
        {
            foreach (var property in jsonObject.Properties)
            {
                if (property.Type_ is JsonString)
                {
                    stringBuilder.AppendLine(
                        "public string " + property.Name +
                        " { get { return JsonSource.Find<string>(\"" + objectname + 
                        "\", \"" + property.Name + "\"); } }");     
                }
                if (property.Type_ is JsonNumber)
                {
                    stringBuilder.AppendLine(
                        "public double " + property.Name +
                        " { get { return JsonSource.Find<double>(\"" + objectname +
                        "\", \"" + property.Name + "\"); } }");     
                }
                if (property.Type_ is JsonBool)
                {
                    stringBuilder.AppendLine(
                        "public bool " + property.Name +
                        " { get { return JsonSource.Find<bool>(\"" + objectname +
                        "\", \"" + property.Name + "\"); } }");    
                }
            }
        }

        public void GenerateArrays(StringBuilder stringBuilder, JsonObject jsonObject, string objectname)
        {
            foreach (JsonArray property in jsonObject.Arrays)
            {
                if (property.Values[0] is JsonString)
                {
                    stringBuilder.AppendLine(
                        "public string[] " + property.Name +
                        " { get { return JsonSource.FindArrayString(\"" + objectname +
                        "\", \"" + property.Name + "\"); } }");
                }
                if (property.Values[0] is JsonNumber)
                {
                    stringBuilder.AppendLine(
                        "public double[] " + property.Name +
                        " { get { return JsonSource.FindArrayNumber(\"" + objectname +
                        "\", \"" + property.Name + "\"); } }");
                }
                if (property.Values[0] is JsonBool)
                {
                    stringBuilder.AppendLine(
                        "public bool[] " + property.Name +
                        " { get { return JsonSource.FindArrayBool(\"" + objectname +
                        "\", \"" + property.Name + "\"); } }");
                }                                            
            }
        }

        public void GenerateStartPart(StringBuilder stringBuilder, string namespace_, string objectname)
        {
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using System.Linq;");
            stringBuilder.AppendLine("using System.Text;");
            stringBuilder.AppendLine("using JsonConverter;");
            stringBuilder.AppendLine(string.Format("namespace {0}", namespace_));
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(string.Format("public class {0} : JsonStaticObject", objectname));
            stringBuilder.AppendLine("{");
        }

        public void GenerateObjectStartPart(StringBuilder stringBuilder, string objectname)
        {            
            stringBuilder.AppendLine(string.Format("public class {0} : JsonStaticObject", objectname));
            stringBuilder.AppendLine("{");
        }

        public void GenerateObjectEndPart(StringBuilder stringBuilder, string objectname)
        {           
            stringBuilder.AppendLine("}");
        }

        public void GenerateEndPart(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}");
        }
    }
}

