using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Lib
{

    using Newtonsoft.Json.Linq;
    using System.Text;

    namespace Json2Class
    {
        public class ClassGenerator
        {
            private string ToTitle(string input)
            {
                return input.Substring(0, 1).ToUpper() + input.Substring(1);
            }

            private bool IsDictionary(List<string> names)
            {
                return true;
            }

            private IEnumerable<string> CreateClass(string className, JObject jsonObject)
            {

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("public class " + className + " {");


                foreach (KeyValuePair<string, JToken> item in jsonObject)
                {
                    string name = ToTitle(item.Key);
                    //if (!UnicodeCharacterUtilities.IsValidIdentifier(name))
                    //{
                    //    continue;
                    //}

                    JToken value = item.Value;
                    JObject options = value as JObject;
                    if (options != null)
                    {
                        JProperty firstElement2 = options.Properties().First();
                        List<string> allNames = (from x in options.Properties()
                                                 select x.Name).ToList();
                        JToken firstValue = firstElement2.Value;
                        if (!IsDictionary(allNames))
                        {
                            string newType2 = name;
                            IEnumerable<string> allClass3 = CreateClass(newType2, options);
                            stringBuilder.AppendLine("----public " + newType2 + " " + name + " { set; get; }");
                            foreach (string item2 in allClass3)
                            {
                                yield return item2;
                            }

                            continue;
                        }

                        JArray arrayValue2 = firstValue as JArray;
                        if (arrayValue2 != null)
                        {
                            continue;
                        }

                        JObject objectDict = firstValue as JObject;
                        if (objectDict != null)
                        {
                            string newType = name;
                            IEnumerable<string> allClass2 = CreateClass(newType, objectDict);
                            stringBuilder.AppendLine("----public Dictionary<String, " + newType + "> " + name + " { set; get; }");
                            foreach (string item3 in allClass2)
                            {
                                yield return item3;
                            }
                        }
                        else
                        {
                            JTokenType elementType2 = firstElement2.Value.Type;
                            stringBuilder.AppendLine($"----public Dictionary<String, {elementType2}> {name} {{ set; get; }}");
                        }

                        continue;
                    }

                    value = item.Value;
                    JArray array = value as JArray;
                    if (array != null)
                    {
                        if (array.HasValues)
                        {
                            JToken firstElement = array[0];
                            JObject arrayValue = firstElement as JObject;
                            if (arrayValue != null)
                            {
                                string newName = name;
                                IEnumerable<string> allClass = CreateClass(newName, arrayValue);
                                stringBuilder.AppendLine("----public " + newName + "[] " + name + " { set; get; }");
                                foreach (string item4 in allClass)
                                {
                                    yield return item4;
                                }
                            }
                            else
                            {
                                JTokenType elementType = firstElement.Type;
                                stringBuilder.AppendLine($"----public {elementType}[] {name} {{ set; get; }}");
                            }
                        }
                        else
                        {
                            stringBuilder.AppendLine("----public Object[] " + name + " { set; get; }");
                        }
                    }
                    else
                    {
                        JTokenType type = item.Value.Type;
                        stringBuilder.AppendLine($"----public {type} {name} {{ set; get; }}");
                    }
                }

                stringBuilder.AppendLine("}");
                yield return stringBuilder.ToString();
            }

            public string JsonToClasses(string jsonString)
            {
                JObject jsonObject = JObject.Parse(jsonString);
                List<string> list = CreateClass("root", jsonObject).ToList();
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string item in list)
                {
                    IEnumerable<string> values = from x in item.Split(new char[1] { '\n' })
                                                 select "    " + x;
                    stringBuilder.AppendLine(string.Join("\n", values).TrimEnd(Array.Empty<char>()));
                }

                stringBuilder.AppendLine("}");
                return stringBuilder.ToString().Trim().Replace("----", "    ");
            }
        }
    }
}
