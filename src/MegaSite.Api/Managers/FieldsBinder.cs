using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dongle.Serialization;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Managers
{
    public class FieldManager
    {
        public List<Field> Bind(string fieldsJson, string fieldValuesJson = null)
        {
            var fields = JsonSimpleSerializer.UnserializeFromString<List<Field>>(fieldsJson) ?? new List<Field>();

            if (!string.IsNullOrEmpty(fieldValuesJson))
            {
                var valueList = JsonSimpleSerializer.UnserializeFromString<Dictionary<string, string>>(fieldValuesJson);
                foreach (var field in fields)
                {
                    if (valueList.ContainsKey(field.Name))
                    {
                        field.Value = valueList[field.Name];
                    }
                }
            }
            return fields.Select(item => (Field)item.Clone()).ToList();
        }

        public Dictionary<string, string> FillDictionary(FormCollection form, List<Field> fields)
        {
            var fieldValues = new Dictionary<string, string>();
            foreach (var key in form.AllKeys)
            {
                var field = fields.FirstOrDefault(ef => ef.Name == key);
                if (field != null)
                {
                    if (field.Type == FieldType.Boolean)
                    {
                        fieldValues.Add(key, (string.IsNullOrEmpty(form[key]) == false && form[key] != "false").ToString());
                        continue;
                    }
                    fieldValues.Add(key, form[key]);
                }
            }
            return fieldValues;
        }
    }
}