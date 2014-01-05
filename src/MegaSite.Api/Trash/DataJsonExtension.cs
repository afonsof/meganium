using System;
using Dongle.Serialization;
using MegaSite.Api.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MegaSite.Api.Trash
{
    public static class DataJsonExtension
    {
        public static T GetData<T>(this IHaveDataJson obj) where T : class
        {
            var ret = InternalJsonSerializer.Deserialize<T>(obj.DataJson);
            if (ret == null)
            {
                return Activator.CreateInstance<T>();
            }
            return ret;
        }

        public static void SetData<T>(this IHaveDataJson obj, T data)
        {
            var obj1 = JObject.Parse(InternalJsonSerializer.Serialize(data));
            var obj2 = JObject.Parse(obj.DataJson);
            obj2.MergeInto(obj1);
            obj.DataJson = obj2.ToString(Formatting.None);
        }

        private static void MergeInto(this JContainer left, JToken right)
        {
            foreach (var rightChild in right.Children<JProperty>())
            {
                var rightChildProperty = rightChild;
                var leftProperty = left.SelectToken(rightChildProperty.Name);

                if (leftProperty == null)
                {
                    // no matching property, just add 
                    left.Add(rightChild);
                }
                else
                {
                    var leftObject = leftProperty as JObject;
                    if (leftObject == null)
                    {
                        // replace value
                        var leftParent = (JProperty) leftProperty.Parent;
                        leftParent.Value = rightChildProperty.Value;
                    }

                    else
                        // recurse object
                        MergeInto(leftObject, rightChildProperty.Value);
                }
            }
        }
    }
}