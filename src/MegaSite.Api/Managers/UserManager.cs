using System;
using System.Collections.Generic;
using System.Linq;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Repositories;
using MegaSite.Api.Resources;
using MegaSite.Api.ViewModels;
using NHibernate.Linq;

namespace MegaSite.Api.Managers
{
    public class UserManager
    {
        private readonly IRepositories _repositories;

        public UserManager(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public User GetById(int id)
        {
            return _repositories.UserRepository.GetById(id);
        }

        public User GetFromUsernameAndPassword(string username, string password)
        {
            var md5Password = password.ToMd5();
            var login = username;

            return _repositories.Db
                .Session
                .Query<User>()
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
            _repositories.UserRepository.Add(user);
            _repositories.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public User GetByUserNameOrEmail(string userNameOrEmail)
        {
            return _repositories.UserRepository.AsQueryable().FirstOrDefault(u => u.Email == userNameOrEmail || u.UserName == userNameOrEmail);
        }

        private bool Exists(string userNameOrEmail)
        {
            return _repositories.UserRepository.AsQueryable().Any(u => u.Email == userNameOrEmail || u.UserName == userNameOrEmail);
        }

        public IEnumerable<User> GetAll()
        {
            return _repositories.UserRepository.AsQueryable().OrderBy(u => u.FullName);
        }

        public Message Change(UserEditVm vm)
        {
            var user = _repositories.UserRepository.GetById(vm.Id);
            user.FullName = vm.FullName;
            user.Enabled = vm.Enabled;
            _repositories.UserRepository.Edit(user);
            _repositories.Commit();

            return new Message(Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        public Message Remove(int id)
        {
            var user = _repositories.UserRepository.GetById(id);

            if (!_repositories.PostRepository.AsQueryable().Any(p => p.CreatedBy == user || p.UpdatedBy == user))
            {
                _repositories.UserRepository.Remove(id);
                _repositories.Commit();
                return new Message(Resource.ItemSuccessfullyDeleted, MessageType.Success);
            }
            return new Message(Resource.CantRemoveBecauseThisUserHavePosts, MessageType.Error);
        }
    }
}