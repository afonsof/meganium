using System;
using System.Linq;
using System.Text;
using Dongle.Algorithms;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Repositories;
using MegaSite.Api.Resources;
using MegaSite.Api.ViewModels;

namespace MegaSite.Api.Managers
{
    public class ClientManager
    {
        private readonly IRepositories _repos;

        public ClientManager(IRepositories repos)
        {
            _repos = repos;
        }

        public object GetAll()
        {
            return _repos.ClientRepository.AsQueryable().OrderBy(u => u.FullName);
        }

        public Message Change(ClientEditVm vm)
        {
            var client = _repos.ClientRepository.GetById(vm.Id);
            client.FullName = vm.FullName;
            client.Enabled = vm.Enabled;
            client.AvailableMediaFilesJson = vm.AvailableMediaFilesJson;
            client.Memo = vm.Memo;
            client.PhotoCount = vm.PhotoCount;
            _repos.ClientRepository.Edit(client);
            _repos.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Message Remove(int id)
        {
            _repos.ClientRepository.Remove(id);
            _repos.Commit();
            return new Message(Resource.ItemSuccessfullyDeleted, MessageType.Success);
        }

        public Client GetById(int id)
        {
            return _repos.ClientRepository.GetById(id);
        }

        private bool Exists(string userNameOrEmail)
        {
            return _repos.ClientRepository.AsQueryable().Any(u => u.Email == userNameOrEmail);
        }

        public Message CreateAndSave(Client client)
        {
            if (Exists(client.Email))
            {
                return new Message(Resource.CantSaveBecauseAItemWithSameEmailAlreadyExists, MessageType.Error);
            }

            client.Code = HumanReadableHash.Compute(new Random().Next().ToString(), Encoding.ASCII);
            client.CreatedAt = DateTime.Now;
            _repos.ClientRepository.Add(client);
            _repos.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Client GetByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return null;
            }
            return _repos.ClientRepository.AsQueryable().FirstOrDefault(p=>p.Code.ToLowerInvariant() == hash.ToLowerInvariant());
        }

        public Client GetHavingHashInObject(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return null;
            }
            return _repos.ClientRepository.AsQueryable().FirstOrDefault(c => c.DataJson.ToLowerInvariant().Contains(hash.ToLowerInvariant()));
        }
    }
}