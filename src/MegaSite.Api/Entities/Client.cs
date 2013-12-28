﻿using System;
using System.Collections.Generic;
using Dongle.Serialization;
using NHibernate.Validator.Constraints;

namespace MegaSite.Api.Entities
{
    public class Client : IHaveId, IHaveDataJson
    {
        public virtual int Id { get; set; }

        public virtual string Email { get; set; }
        /*public virtual string Password { get; set; }*/
        public virtual string FullName { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        /*public virtual string UserName { get; set; }*/
        public virtual string Code { get; set; }

        [Length(1024)]
        public virtual string Memo { get; set; }

        #region PhotoSelector

        [Length(150000)]
        public virtual string DataJson { get; set; }

        #endregion

        public virtual IEnumerable<ClientSubItem> SubItems { get; set; }
    }
}