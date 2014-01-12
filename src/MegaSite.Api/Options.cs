using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;
using MegaSite.Api.Trash;
using NHibernate.Linq;

namespace MegaSite.Api
{
    public class Options : IOptions
    {
        private static readonly Dictionary<int, IOptions> Instances = new Dictionary<int, IOptions>();

        private Dictionary<string, string> _cachedOptions;
        private readonly int _licenseId;

        private Options(int licenseId)
        {
            _licenseId = licenseId;
        }

        public static IOptions GlobalOptions
        {
            //todo: arrumar o zero
            get { return Instances[0] ?? (Instances[0] = new Options(0)); }
        }

        public static IOptions GetOptions(int licenseId)
        {
            if (Instances.ContainsKey(licenseId))
            {
                return Instances[licenseId];
            }
            return Instances[licenseId] = new Options(licenseId);
        }

        public string Get(string name)
        {
            Init();
            if (_cachedOptions.ContainsKey(name.ToLower()))
            {
                return _cachedOptions[name.ToLower()];
            }
            return null;
        }

        private void Init()
        {
            if (_cachedOptions == null)
            {
                using (var db = CreateDatabaseConnection())
                {
                    var repo = new Repository<License>(db);
                    var client = repo.GetById(_licenseId);
                    if (client != null)
                    {
                        _cachedOptions = InternalJsonSerializer.Deserialize<Dictionary<string, string>>(client.OptionsJson) ?? new Dictionary<string, string>();
                    }
                    else
                    {
                        _cachedOptions = new Dictionary<string, string>();
                    }
                }
            }
        }

        public int GetInt(string name)
        {
            var config = Get(name);
            int ret;
            Int32.TryParse(config, out ret);
            return ret;
        }

        public long GetLong(string name)
        {
            var config = Get(name);
            long ret;
            Int64.TryParse(config, out ret);
            return ret;
        }

        public int Get(string name, int defaultValue)
        {
            var value = Get(name);
            int intValue;
            return Int32.TryParse(value, out intValue) ? intValue : defaultValue;
        }

        public bool Get(string name, bool defaultValue)
        {
            var value = Get(name);
            bool b;
            return Boolean.TryParse(value, out b) ? b : defaultValue;
        }

        public void Set(string name, int? value)
        {
            Set(name, value.ToString());
        }

        public void Set(string name, long? value)
        {
            Set(name, value.ToString());
        }

        public void Set(string name, bool? value)
        {
            Set(name, value.ToString());
        }

        public void Set(string name, string value)
        {
            Init();
            using (var db = CreateDatabaseConnection())
            {
                var repo = new Repository<License>(db);
                _cachedOptions[name.ToLowerInvariant()] = value;
                var optionsJson = InternalJsonSerializer.Serialize(_cachedOptions);
                var client = repo.GetById(_licenseId);
                if (client != null)
                {
                    client.OptionsJson = optionsJson;
                    repo.Edit(client);
                }
                else
                {
                    client = new License
                    {
                        OptionsJson = optionsJson
                    };
                    repo.Add(client);
                }
                db.Commit();
            }
            Init();
        }

        private static Database CreateDatabaseConnection()
        {
            return new Database(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
    }
}