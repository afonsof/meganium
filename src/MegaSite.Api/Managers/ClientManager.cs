using System;
using System.Linq;
using System.Text;
using Dongle.Algorithms;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Resources;
using MegaSite.Api.ViewModels;

namespace MegaSite.Api.Managers
{
    public class ClientManager
    {
        private readonly IUnitOfWork _uow;

        public ClientManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public object GetAll()
        {
            return _uow.ClientRepository.AsQueryable().OrderBy(u => u.FullName);
        }

        public Message Change(ClientEditVm vm)
        {
            var client = _uow.ClientRepository.GetById(vm.Id);
            client.FullName = vm.FullName;
            client.Enabled = vm.Enabled;
            client.AvailableMediaFilesJson = vm.AvailableMediaFilesJson;
            client.Memo = vm.Memo;
            client.PhotoCount = vm.PhotoCount;
            _uow.ClientRepository.Edit(client);
            _uow.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Message Remove(int id)
        {
            _uow.ClientRepository.Remove(id);
            _uow.Commit();
            return new Message(Resource.ItemSuccessfullyDeleted, MessageType.Success);
        }

        public Client GetById(int id)
        {
            return _uow.ClientRepository.GetById(id);
        }

        private bool Exists(string userNameOrEmail)
        {
            return _uow.ClientRepository.AsQueryable().Any(u => u.Email == userNameOrEmail);
        }

        public Message CreateAndSave(Client client)
        {
            if (Exists(client.Email))
            {
                return new Message(Resource.CantSaveBecauseAItemWithSameEmailAlreadyExists, MessageType.Error);
            }

            client.Hash = HumanReadableHash.Compute(new Random().Next().ToString(), Encoding.ASCII);
            client.CreatedAt = DateTime.Now;
            _uow.ClientRepository.Add(client);
            _uow.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Client GetByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return null;
            }
            return _uow.ClientRepository.AsQueryable().FirstOrDefault(p=>p.Hash.ToLowerInvariant() == hash.ToLowerInvariant());
        }
    }
}