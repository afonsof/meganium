using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Dongle.Serialization;
using MegaSite.Api.Entities;

namespace MegaSite.Api
{
    public class Options : IOptions
    {
        private Dictionary<string, string> _cachedOptions;
        private static IOptions _instance;

        private Options()
        {
        }

        public static IOptions Instance
        {
            get { return _instance ?? (_instance = new Options()); }
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
                using (var uow = CreateUnitOfWork())
                {
                    var client = uow.ClientRepository.AsQueryable().FirstOrDefault();
                    if (client != null)
                    {
                        _cachedOptions =
                            JsonSimpleSerializer.UnserializeFromString<Dictionary<string, string>>(client.OptionsJson);
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

        public void Set(string name, string value)
        {
            Init();
            using (var uow = CreateUnitOfWork())
            {
                _cachedOptions[name.ToLowerInvariant()] = value;
                var optionsJson = JsonSimpleSerializer.SerializeToString(_cachedOptions);
                var client = uow.ClientRepository.AsQueryable().FirstOrDefault();
                if (client != null)
                {
                    client.OptionsJson = optionsJson;
                    uow.ClientRepository.Edit(client);
                }
                else
                {
                    client = new Client
                    {
                        OptionsJson = optionsJson
                    };
                    uow.ClientRepository.Add(client);
                }
                uow.Commit();
            }
            Init();
        }

        private static IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(null, ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
    }
}