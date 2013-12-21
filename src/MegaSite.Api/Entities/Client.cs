using System;
using System.Collections.Generic;
using Dongle.Serialization;
using NHibernate.Validator.Constraints;

namespace MegaSite.Api.Entities
{
    public class Client : IHaveId
    {
        public virtual int Id { get; set; }

        public virtual string Email { get; set; }
        /*public virtual string Password { get; set; }*/
        public virtual string FullName { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        /*public virtual string UserName { get; set; }*/
        public virtual string Hash { get; set; }

        [Length(1024)]
        public virtual string Memo { get; set; }

        #region PhotoSelector

        public virtual int PhotoCount { get; set; }

        [Length(1024)]
        public virtual string AvailableMediaFilesJson { get; set; }
        public virtual List<MediaFile> AvailableMediaFiles
        {
            get
            {
                var value = JsonSimpleSerializer.UnserializeFromString<List<MediaFile>>(AvailableMediaFilesJson);
                if (value == null)
                {
                    return new List<MediaFile>();
                }
                return value;
            }
        }

        [Length(1024)]
        public virtual string SelectedMediaFilesJson { get; set; }
        public virtual List<MediaFile> SelectedMediaFiles
        {
            get
            {
                var value = JsonSimpleSerializer.UnserializeFromString<List<MediaFile>>(SelectedMediaFilesJson);
                if (value == null)
                {
                    return new List<MediaFile>();
                }
                return value;
            }
        }

        public string DataJson { get; set; }

        #endregion
    }
}