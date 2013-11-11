using System.Collections.Generic;
using System.Linq;
using Dongle.Serialization;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Resources;
using MegaSite.Api.ViewModels;

namespace MegaSite.Api.Managers
{
    public class PostTypeManager
    {
        private readonly IUnitOfWork _uow;

        public PostTypeManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public PostType GetById(int? id)
        {
            id = id ?? Options.Instance.GetInt("DefaultPostTypeId");
            return _uow.PostTypeRepository.GetById(id.Value);
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
            return JsonSimpleSerializer.SerializeToString(fields);
        }
    }
}