using System;
using System.Collections.Generic;

namespace MegaSite.Api.Entities
{
    public class Field : ICloneable
    {
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public string Value;
        public List<string> SelectList { get; set; }
        public int Order { get; set; }


        //TODO: Isto faz sentido?
        public object Clone()
        {
            return new Field
            {
                Type = Type,
                Name = Name,
                Value = Value,
                SelectList = SelectList,
                Order = Order
            };
        }

    }
}