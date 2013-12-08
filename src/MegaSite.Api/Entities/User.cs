﻿using System;

namespace MegaSite.Api.Entities
{
    public class User : IHaveId
    {
        public virtual int Id { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }

        public virtual int UserType { get; set; }

        public virtual string ExternalServiceUser { get; set; }
        public virtual string ExternalServiceName { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(FullName) == false)
                {
                    return FullName;
                }
                if (String.IsNullOrEmpty(UserName) == false)
                {
                    return UserName;
                }
                if (String.IsNullOrEmpty(ExternalServiceUser) == false)
                {
                    return ExternalServiceUser;
                }
                return Email;
            }
        }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string UserName { get; set; }
    }
}