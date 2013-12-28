using System;
using System.Collections.Generic;
using System.Linq;
using Dongle.Serialization;
using Dongle.System;
using MegaSite.Api.Resources;
using MegaSite.Api.Trash;
using NHibernate.Validator.Constraints;

namespace MegaSite.Api.Entities
{
    //refactor: rever metodos desta classe
    public class Post : IHaveId, IHaveSlug
    {
        public virtual int Id { get; set; }

        //Main Infomation
        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Hash { get; set; }

        [Length(1024)]
        public virtual string Content { get; set; }
        public virtual bool Published { get; set; }

        [Length(1024)]
        public virtual string FeaturedMediaFileJson { get; set; }
        public virtual MediaFile FeaturedMediaFile
        {
            get
            {
                return InternalJsonSerializer.Deserialize<MediaFile>(FeaturedMediaFileJson);
            }
        }

        [Length(150000)]
        public virtual string MediaFilesJson { get; set; }
        public virtual List<MediaFile> MediaFiles
        {
            get
            {
                var value = InternalJsonSerializer.Deserialize<List<MediaFile>>(MediaFilesJson);
                if (value == null)
                {
                    return new List<MediaFile>();
                }
                return value;
            }
        }

        //Users
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }

        //Dates
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? PublishedAt { get; set; }

        //Featured
        public virtual bool IsFeatured { get; set; }
        public virtual int FeaturedOrder { get; set; }

        //Tree
        public virtual Post Parent { get; set; }

        //Location
        public virtual Double Latitude { get; set; }
        public virtual Double Longitude { get; set; }
        public virtual string Location { get; set; }

        //TimeBox
        public virtual DateTime StartedAt { get; set; }
        public virtual DateTime EndedAt { get; set; }

        //Categories
        public virtual ICollection<Category> Categories { get; set; }

        //External identification
        public virtual string ExternalServiceId { get; set; }
        public virtual string ExternalServiceUser { get; set; }
        public virtual string ExternalServiceName { get; set; }

        //Custom Entity
        public virtual PostType PostType { get; set; }
        [Length(1024)]
        public virtual string FieldsValuesJson { get; set; }

        private Dictionary<string, string> _fieldsValues;

        public virtual Dictionary<string, string> FieldsValues
        {
            get
            {
                return _fieldsValues ??
                       (_fieldsValues = InternalJsonSerializer.Deserialize<Dictionary<string, string>>(
                           FieldsValuesJson) ?? new Dictionary<string, string>());
            }
        }

        public virtual string GetFieldValue(string name)
        {
            return FieldsValues.ContainsKey(name) ? FieldsValues[name] : null;
        }

        #region Generated Properties

        public virtual bool HasThumbnail
        {
            get
            {
                return FeaturedMediaFile != null;
            }
        }

        public virtual string PreviewContent
        {
            get
            {
                return ContentBeforeMore.Take(200) + "...";
            }
        }

        public virtual string ContentBeforeMore
        {
            get
            {
                if (Content == null)
                {
                    return "";
                }
                var morePos = Content.IndexOf("<!--more-->");
                return morePos == -1 ? Content.StripTags() : Content.Substring(0, morePos).StripTags();
            }
        }

        public virtual bool HasMore
        {
            get { return Content != null && (Content.IndexOf("<!--more-->") > -1); }
        }

        public virtual string UrlPath
        {
            get
            {
                return "~/" + Type.ToSlug() + "/" + (Slug ?? "");
            }
        }

        public virtual string Type
        {
            get { return PostType!=null?PostType.SingularName:""; }
        }

        public virtual string TitleWithType
        {
            get { return "[" + Type + "] " + Title; }
        }

        public virtual string CategoriesCsv
        {
            get
            {
                return Categories.Select(c => c.Title).ToCsv();
            }
        }

        public virtual string ParentName
        {
            get
            {
                if (Parent == null)
                {
                    return Resource.RootPage;
                }
                return Parent.Title;
            }
        }

        #endregion
    }
}