using System.Collections.Generic;
using Sourceportal.Domain.Models.DB.Items;

namespace Sourceportal.DB.Items
{
    public interface IItemsIhsRepository
    {
        List<ItemIhs> GetItems(string searchString, int limit);
        List<ItemIhs> SearchItem(string partnumber);
    }
}