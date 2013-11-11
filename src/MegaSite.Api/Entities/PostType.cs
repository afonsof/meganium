using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Constraints;

namespace MegaSite.Api.Entities
{
    public class PostType : IHaveId
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual string SingularName { get; set; }

        [Required]
        public virtual string PluralName { get; set; }

        [Required]
        public virtual string IconId { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual string BehaviorStr { get; set; }

        [Length(1024)]
        public virtual string FieldsJson { get; set; }

        #region Behavior

        public virtual void SetBehavior(BehaviorFlags behavior)
        {
            BehaviorStr = behavior.ToString("F");
        }

        private BehaviorFlags? _behavior;
        public virtual BehaviorFlags Behavior
        {
            get
            {
                if (_behavior == null && !string.IsNullOrEmpty(BehaviorStr))
                {
                    _behavior = (BehaviorFlags)System.Enum.Parse(typeof(BehaviorFlags), BehaviorStr, true);
                }
                if (_behavior == null)
                {
                    return BehaviorFlags.AllowDescription;
                }
                return _behavior.Value;
            }
        }

        [Flags]
        public enum BehaviorFlags
        {
            AllowMarkAsFeatured = 1,
            AllowLocation = 2,
            AllowTimeBox = 4,
            AllowExternalId = 8,
            AllowComments = 16,
            AllowCategories = 32,
            AllowPhotos = 64,
            AllowVideo = 128,
            AllowTree = 256,
            AllowDescription = 512,
            AllowHash = 1024,
        }

        #endregion
    }
}