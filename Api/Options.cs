using System;
using System.Collections.Generic;
using System.Configuration;
using Meganium.Api.Entities;
using Meganium.Api.Repositories;
using Meganium.Api.Trash;

namespace Meganium.Api
{
    public class Options : IOptions
    {
        private static readonly Dictionary<int, IOptions> Instances = new Dictionary<int, IOptions>();

        private Dictionary<string, object> _cachedOptions;
        private readonly int _licenseId;

        private Options(int licenseId)
        {
            _licenseId = licenseId;
        }

        public static IOptions GlobalOptions
        {
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

        private void Init()
        {
            if (_cachedOptions == null)
            {
                if (_licenseId == 0)
                {
                    _cachedOptions = InternalJsonSerializer
                        .Deserialize<Dictionary<string, object>>(ConfigurationManager.AppSettings["GlobalOptions"]);

                }
                else
                {

                    using (var db = CreateDatabaseConnection())
                    {
                        var repo = new Repository<License>(db);
                        var client = repo.GetById(_licenseId);
                        if (client != null)
                        {
                            _cachedOptions = InternalJsonSerializer
                                .Deserialize<Dictionary<string, object>>(client.OptionsJson);
                        }

                    }
                }
                if (_cachedOptions == null)
                {
                    _cachedOptions = new Dictionary<string, object>();
                }
            }
        }


        public object Get(string name)
        {
            Init();
            if (_cachedOptions.ContainsKey(name.ToLower()))
            {
                return _cachedOptions[name.ToLower()];
            }
            return null;
        }

        public string GetString(string name)
        {
            var value = Get(name);
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }

        public int GetInt(string name)
        {
            var config = GetString(name);
            int ret;
            Int32.TryParse(config, out ret);
            return ret;
        }

        public long GetLong(string name)
        {
            var config = GetString(name);
            long ret;
            Int64.TryParse(config, out ret);
            return ret;
        }

        public int Get(string name, int defaultValue)
        {
            var value = GetString(name);
            int intValue;
            return Int32.TryParse(value, out intValue) ? intValue : defaultValue;
        }

        public bool Get(string name, bool defaultValue)
        {
            var value = GetString(name);
            bool b;
            return Boolean.TryParse(value, out b) ? b : defaultValue;
        }

        public void Set(string name, object value)
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