using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class UserVM
    {
        public IEnumerable<UserDto> Users { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}
