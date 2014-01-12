﻿using System.Collections.Generic;
using System.Linq;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Plugins;
using MegaSite.Api.Repositories;
using MegaSite.Api.Resources;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;

namespace MegaSite.Api.Managers
{
    public class PostTypeManager
    {
        private readonly IRepositories _uow;
        private readonly License _license;

        public PostTypeManager(IRepositories uow, License license)
        {
            _uow = uow;
            _license = license;
        }

        public PostType GetById(int? id = null)
        {
            PostType postType = null;
            if (id != null)
            {
                postType = _uow.PostTypeRepository.GetById(id.Value);
            }
            return postType ?? _uow.PostTypeRepository.GetById(_license.Options.GetInt("DefaultPostTypeId"));
        }

        public IEnumerable<PostType> GetAll()
        {
            return _uow.PostTypeRepository
                .AsQueryable()
                .OrderBy(pt => pt.PluralName);
        }

        public void CreateAndSave(PostTypeCreateEditVm vm, PostType postType)
        {
            FillData(vm, postType);
            _uow.PostTypeRepository.Add(postType);
            _uow.Commit();
        }

        public void Change(PostTypeCreateEditVm vm, PostType postType)
        {
            FillData(vm, postType);
            _uow.PostTypeRepository.Edit(postType);
            _uow.Commit();
        }

        public Message Delete(int id)
        {
            var postType = _uow.PostTypeRepository.GetById(id);

            if (!postType.Posts.Any())
            {
                _uow.PostTypeRepository.Remove(id);
                _uow.Commit();
                return new Message(Resource.ItemSuccessfullyDeleted, MessageType.Success);
            }
            return new Message(Resource.CantDeleteBecauseThisTypeContainsObjects, MessageType.Error);
        }

        public IEnumerable<PostType> GetWhatAllowsCategories()
        {
            return _uow.PostTypeRepository
                .AsQueryable()
                .Where(pt => pt.BehaviorStr.Contains("AllowCategories"))
                .OrderBy(pt => pt.SingularName);
        }

        public PostType GetBySingularName(string singularName)
        {
            return _uow.PostTypeRepository
                .AsQueryable()
                .FirstOrDefault(pt => pt.SingularName.ToLowerInvariant() == singularName.ToLowerInvariant());
        }

        private static void FillData(PostTypeCreateEditVm vm, PostType postType)
        {
            if(vm == null) return;
            if (vm.BehaviorItems != null)
            {
                postType.BehaviorStr = vm.BehaviorItems.ToCsv();
            }
            postType.FieldsJson = GetFieldsJson(vm.item_Name, vm.item_Type, vm.item_SelectList);
        }

        private static string GetFieldsJson(string[] itemName, int[] itemType, string[] itemSelectList)
        {
            var fields = new List<Field>();
            for (var i = 0; i < itemName.Length; i++)
            {
                if (string.IsNullOrEmpty(itemName[i]))
                {
                    continue;
                }
                var selectList = itemSelectList[i];
                fields.Add(new Field
                {
                    Name = itemName[i],
                    Type = (FieldType)itemType[i],
                    SelectList = string.IsNullOrEmpty(selectList) ? null : selectList.Split(',').Select(o => o.Trim()).ToList()
                });
            }
            return InternalJsonSerializer.Serialize(fields);
        }

        public PostType GetByImportPluginType(ImportPluginType importPluginType)
        {
            int? id = null;

            if (importPluginType == ImportPluginType.Album)
            {
                id = _license.Options.GetInt("DefaultAlbumImportingPostTypeId");
            }
            else if (importPluginType == ImportPluginType.Video)
            {
                id = _license.Options.GetInt("DefaultVideoImportingPostTypeId");
            }
            if (id.HasValue)
            {
                return GetById(id.Value);
            }
            return null;
        }
    }
}