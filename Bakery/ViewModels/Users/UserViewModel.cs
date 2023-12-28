using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Display(Name = "Роль")]
        public string RoleName { get; set; }

        public UserViewModel()
        {
            Id = string.Empty; 
            UserName = string.Empty; 
            RoleName = string.Empty; 
            Email = string.Empty; 
        }
    }
}
