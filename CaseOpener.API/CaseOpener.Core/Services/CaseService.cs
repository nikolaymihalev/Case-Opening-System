using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Models.Item;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CaseOpener.Core.Services
{
    public class CaseService : ICaseService
    {
        private readonly IRepository repository;

        public CaseService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<string> AddCaseAsync(CaseFormModel model)
        {
            var caseModel = new Case()
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
            };

            var itemsList = new List<Item>();

            for(int i = 0; i < model.Items.Length - 1; i++)
            {
                var itemEntity = await repository.GetByIdAsync<Item>(model.Items[i]);

                if(itemEntity != null)
                    itemsList.Add(itemEntity);
            }

            caseModel.Items = JsonSerializer.Serialize(itemsList);

            await repository.AddAsync(caseModel);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "case");
        }

        public async Task<string> DeleteCaseAsync(int id)
        {
            var caseEntity = await repository.GetByIdAsync<Case>(id);

            if(caseEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            await repository.DeleteAsync<Case>(id);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyDeleted, "case");
        }

        public async Task<bool> DoesUserHaveCase(string userId, int caseId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                return false;

            var caseM = await repository.GetByIdAsync<Case>(caseId);

            if (caseM is null)
                return false;

            var userInventoryItems = await repository.AllReadonly<InventoryItem>()
                .Include(x=>x.Item)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return userInventoryItems.Any(x => x.Item.Name == caseM.Name);
        }

        public async Task<string> EditCaseAsync(CaseFormModel model)
        {
            var caseEntity = await repository.GetByIdAsync<Case>(model.Id);

            if (caseEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            caseEntity.Name = model.Name;
            caseEntity.Price = model.Price;
            caseEntity.ImageUrl = model.ImageUrl;

            var itemsList = new List<Item>();

            for (int i = 0; i < model.Items.Length - 1; i++)
            {
                var itemEntity = await repository.GetByIdAsync<Item>(model.Items[i]);

                if(itemEntity != null)
                    itemsList.Add(itemEntity);
            }

            caseEntity.Items = JsonSerializer.Serialize(itemsList);

            return string.Format(ReturnMessages.SuccessfullyEdited, "case");
        }

        public async Task<IEnumerable<CaseModel>> GetAllCasesAsync()
        {
            var cases = await repository.AllReadonly<Case>().ToListAsync();

            return cases.Select(x => new CaseModel()
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                Items = JsonSerializer.Deserialize<List<ItemModel>>(x.Items) ?? new List<ItemModel>()
            });
        }

        public async Task<CaseModel> GetCaseByIdAsync(int id)
        {
            var caseM = await repository.GetByIdAsync<Case>(id);

            if (caseM is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            return new CaseModel()
            {
                Id = caseM.Id,
                Name = caseM.Name,
                ImageUrl = caseM.ImageUrl,
                Price = caseM.Price,
                Items = JsonSerializer.Deserialize<List<ItemModel>>(caseM.Items) ?? new List<ItemModel>()
            };
        }

        public async Task<ItemModel> OpenCaseAsync(int caseId, string userId)
        {
            var caseM = await repository.GetByIdAsync<Case>(caseId);

            if (caseM is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            var items = JsonSerializer.Deserialize<List<ItemModel>>(caseM.Items) ?? new List<ItemModel>();

            var item = GetRandomItem(items);

            var caseOpening = new CaseOpeningModel()
            {
                UserId = userId,
                ItemId = item.Id,
                CaseId = caseId,
                DateOpened = DateTime.UtcNow,
            };

            await repository.AddAsync(caseOpening);

            await repository.SaveChangesAsync();

            return item;
        }

        public async Task<ItemModel> OpenDailyRewardAsync(string userId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user != null)
            {
                var dailyReward = await repository.All<DailyReward>().FirstOrDefaultAsync(x => x.UserId == userId);

                if (dailyReward != null && (DateTime.UtcNow - dailyReward.LastClaimedDate).TotalHours >= 24)
                {
                    var caseM = await repository.AllReadonly<Case>().FirstOrDefaultAsync(x => x.Name == "Daily Reward");

                    if (caseM != null)
                    {
                        var item = await OpenCaseAsync(caseM.Id, userId);
                        dailyReward.LastClaimedDate = DateTime.UtcNow;

                        await repository.SaveChangesAsync();

                        return item;
                    }
                }

                throw new ArgumentException(ReturnMessages.CannotClaimDailyReward);
            }
            else
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));
        }

        public async Task SubscribeUserToDailyRewardAsync(string userId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user != null)
            {
                var entity = await repository.AllReadonly<DailyReward>().FirstOrDefaultAsync(x => x.UserId == userId);

                if (entity is null)
                {
                    var caseM = await repository.AllReadonly<Case>().FirstOrDefaultAsync(x => x.Name == "Daily Reward");

                    if (caseM != null)
                    {
                        var model = new DailyRewardModel()
                        {
                            UserId = userId,
                            CaseId = caseM.Id,
                            LastClaimedDate = new DateTime()
                        };

                        await repository.AddAsync(model);
                        await repository.SaveChangesAsync();
                    }
                }
            }           
        }

        private ItemModel GetRandomItem(List<ItemModel> items)
        {
            var cumulativeProbability = 0.0;
            var weightedItems = items
                .Select(item => new
                {
                    Item = item,
                    CumulativeProbability = cumulativeProbability += item.Probability
                })
                .ToList();

            var random = new Random();
            var randomValue = random.NextDouble() * cumulativeProbability;

            return weightedItems.First(w => randomValue <= w.CumulativeProbability).Item;
        }
    }
}
