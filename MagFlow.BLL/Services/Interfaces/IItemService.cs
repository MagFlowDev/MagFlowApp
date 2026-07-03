using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IItemService
    {
        Task<ItemDTO?> GetItem(int id);

        Task<QueryResponse<ItemDTO>> GetItems(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ItemDTO>> GetArchive(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);

        Task<Enums.Result> AddItem(ItemFormModel model);

        Task<Enums.Result> BlockItem(ItemDTO itemDTO, bool unblock = false);
        Task<Enums.Result> BlockItems(List<ItemDTO> itemsDTO, bool unblock = false);
        Task<Enums.Result> DeleteItem(ItemDTO itemDTO);
        Task<Enums.Result> DeleteItems(List<ItemDTO> itemsDTOs);

        Task<Enums.Result> RestoreItem(ItemDTO itemDTO);
        Task<Enums.Result> RestoreItems(List<ItemDTO> itemsDTOs);
    }
}
