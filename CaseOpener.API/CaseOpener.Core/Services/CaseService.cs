using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Models.Item;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

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
                CategoryId = model.CategoryId,
            };

            await repository.AddAsync(caseModel);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "case");
        }

        public async Task<string> AddItemToCaseAsync(int caseId, int itemId, double probability)
        {
            var caseEntity = await repository.GetByIdAsync<Case>(caseId);

            if (caseEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            var itemEntity = await repository.GetByIdAsync<Item>(itemId);

            if (itemEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

            var caseItem = new CaseItem()
            {
                CaseId = caseId,
                ItemId = itemId,
                Probability = probability
            };

            await repository.AddAsync(caseItem);

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "item to case");
        }

        public async Task<string> BuyCaseAsync(int caseId, string userId, int quantity)
        {
            var caseEntity = await repository.GetByIdAsync<Case>(caseId);

            if (caseEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            var userEntity = await repository.GetByIdAsync<User>(userId);

            if (userEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            var model = await repository.All<CaseUser>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CaseId == caseId);

            if (model is null)
            {
                var entity = new CaseUser()
                {
                    CaseId = caseId,
                    UserId = userId,
                    Quantity = quantity
                };

                await repository.AddAsync(entity);
            }
            else
            {
                model.Quantity += quantity;
            }

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "case to user");
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

        public async Task<int> DoesUserHaveCaseAsync(string userId, int caseId)
        {
            var model = await repository.AllReadonly<CaseUser>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CaseId == caseId);

            if (model is null)
                return 0;

            return model.Quantity;
        }

        public async Task<string> EditCaseAsync(CaseFormModel model)
        {
            var caseEntity = await repository.GetByIdAsync<Case>(model.Id);

            if (caseEntity is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            caseEntity.Name = model.Name;
            caseEntity.Price = model.Price;
            caseEntity.ImageUrl = model.ImageUrl;
            caseEntity.CategoryId = model.CategoryId;

            return string.Format(ReturnMessages.SuccessfullyEdited, "case");
        }

        public async Task<IEnumerable<CasePageModel>> GetAllCasesAsync(string? name = null)
        {
            List<Case> cases = new List<Case>();

            if (name != null) 
            {
                cases = await repository.AllReadonly<Case>()
                    .Include(x=>x.Category)
                    .Where(x=>x.Name.ToLower().Contains(name.ToLower()))
                    .OrderByDescending(x => x.Id)
                    .ThenBy(x => x.CategoryId)
                    .ToListAsync();
            }
            else
            {
                cases = await repository.AllReadonly<Case>()
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.Id)
                    .ThenBy(x => x.CategoryId)
                    .ToListAsync();
            }

            return cases.Select(x => new CasePageModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                CategoryName = x.Category.Name
            });
        }

        public async Task<CaseModel> GetCaseByIdAsync(int id)
        {
            var caseE = await repository.GetByIdAsync<Case>(id);
            var items = await repository.AllReadonly<CaseItem>()
                .Include(x=>x.Item)
                .Where(x => x.CaseId == id)
                .Select(x=>new ItemModel()
                {
                    Id = x.ItemId,
                    Amount = x.Item.Amount,
                    Name = x.Item.Name,
                    ImageUrl = x.Item.ImageUrl,
                    Type = x.Item.Type,
                    Rarity = x.Item.Rarity,
                })
                .ToListAsync();

            if (caseE is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            var category = await repository.GetByIdAsync<Category>(caseE.CategoryId);

            if (category is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Category"));

            var caseM = new CasePageModel()
            {
                Id = caseE.Id,
                Name = caseE.Name,
                ImageUrl = caseE.ImageUrl,
                Price = caseE.Price,
                CategoryName = category.Name
            };

            return new CaseModel()
            {
                Case = caseM,
                Items = items,
            };
        }

        public async Task<IEnumerable<CaseOpeningModel>> GetUserOpenedCasesAsync(string userId)
        {
            var caseOpenings = await repository.AllReadonly<CaseOpening>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var list = new List<CaseOpeningModel>();

            foreach(var item in caseOpenings)
            {
                var caseM = await repository.GetByIdAsync<Case>(item.CaseId);

                if(caseM == null)
                    throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

                var categoryM = await repository.GetByIdAsync<Category>(caseM.CategoryId);

                if (categoryM == null)
                    throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Category"));

                var listCase = new CasePageModel()
                {
                    Id = caseM.Id,
                    Name = caseM.Name,
                    ImageUrl = caseM.ImageUrl,
                    Price = caseM.Price,
                    CategoryName = categoryM.Name
                };

                var itemM = await repository.GetByIdAsync<Item>(item.ItemId);

                if (itemM == null)
                    throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

                var listItem = new ItemModel()
                {
                    Id = itemM.Id,
                    Name = itemM.Name,
                    ImageUrl = itemM.ImageUrl,
                    Amount = itemM.Amount,
                    Rarity = itemM.Rarity,
                    Type = itemM.Type,
                };

                var listCaseOpening = new CaseOpeningModel()
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Case = listCase,
                    Item = listItem,
                    DateOpened = item.DateOpened
                };

                list.Add(listCaseOpening);
            }

            return list;
        }

        public async Task<IEnumerable<CaseUserModel>> GetUsersCasesAsync(string userId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            return await repository.AllReadonly<CaseUser>()
                .Include(x => x.Case)
                .Where(x => x.UserId == userId)
                .Select(x => new CaseUserModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CaseId = x.CaseId,
                    Case = new CasePageModel()
                    {
                        Id = x.Case.Id,
                        Name = x.Case.Name,
                        ImageUrl = x.Case.ImageUrl,
                        Price = x.Case.Price,
                    }
                })
                .ToListAsync();
        }

        public async Task<ItemModel> OpenCaseAsync(int caseId, string userId)
        {
            var caseM = await repository.GetByIdAsync<Case>(caseId);

            if (caseM is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            var caseUserModel = await repository.All<CaseUser>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CaseId == caseId);

            if(caseUserModel == null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

            await repository.DeleteAsync<CaseUser>(caseUserModel.Id);

            var items = await repository.AllReadonly<CaseItem>()
                .Where(x => x.CaseId == caseId)
                .Select(x => new CaseItemModel()
                {
                    Id = x.Id,
                    CaseId = x.CaseId,
                    ItemId = x.ItemId,
                    Probability = x.Probability,
                })
                .ToListAsync();

            var itemId = GetRandomItem(items);

            var caseOpening = new CaseOpening()
            {
                UserId = userId,
                ItemId = itemId,
                CaseId = caseId,
                DateOpened = DateTime.UtcNow,
            };

            await repository.AddAsync(caseOpening);

            await repository.SaveChangesAsync();

            return new ItemModel();
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

        private int GetRandomItem(List<CaseItemModel> items)
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

            return weightedItems.First(w => randomValue <= w.CumulativeProbability).Item.ItemId;
        }
    }
}
