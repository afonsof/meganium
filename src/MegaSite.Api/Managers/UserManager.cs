using System;
using System.Collections.Generic;
using System.Linq;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Repositories;
using MegaSite.Api.Resources;
using MegaSite.Api.ViewModels;

namespace MegaSite.Api.Managers
{
    public class UserManager
    {
        private readonly IRepositories _uow;

        public UserManager(IRepositories uow)
        {
            _uow = uow;
        }

        public User GetById(int id)
        {
            return _uow.UserRepository.GetById(id);
        }

        public User GetFromUsernameAndPassword(string username, string password)
        {
            var md5Password = password.ToMd5();
            var login = username;

            return _uow.UserRepository
                .AsQueryable()
                .FirstOrDefault(u => u.Email == login && u.Password == md5Password);
        }

        public Message CreateAndSave(User user)
        {
            if (Exists(user.Email))
            {
                return new Message(Resource.CantSaveBecauseAItemWithSameEmailAlreadyExists, MessageType.Error);
            }
            if (Exists(user.UserName))
            {
                return new Message(Resource.CantSaveBecauseAItemWithSameNameAlreadyExists, MessageType.Error);
            }

            user.Password = user.Password.ToMd5();
            user.CreatedAt = DateTime.Now;
            _uow.UserRepository.Add(user);
            _uow.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public User GetByUserNameOrEmail(string userNameOrEmail)
        {
            return _uow.UserRepository.AsQueryable().FirstOrDefault(u => u.Email == userNameOrEmail || u.UserName == userNameOrEmail);
        }

        private bool Exists(string userNameOrEmail)
        {
            return _uow.UserRepository.AsQueryable().Any(u => u.Email == userNameOrEmail || u.UserName == userNameOrEmail);
        }

        public IEnumerable<User> GetAll()
        {
            return _uow.UserRepository.AsQueryable().OrderBy(u => u.FullName);
        }

        public Message Change(UserEditVm vm)
        {
            var user = _uow.UserRepository.GetById(vm.Id);
            user.FullName = vm.FullName;
            user.Enabled = vm.Enabled;
            _uow.UserRepository.Edit(user);
            _uow.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Message Remove(int id)
        {
            var user = _uow.UserRepository.GetById(id);

            if (!_uow.PostRepository.AsQueryable().Any(p => p.CreatedBy == user || p.UpdatedBy == user))
            {
                _uow.UserRepository.Remove(id);
                _uow.Commit();
                return new Message(Resource.ItemSuccessfullyDeleted, MessageType.Success);
            }
            return new Message(Resource.CantRemoveBecauseThisUserHavePosts, MessageType.Error);
        }
    }
}