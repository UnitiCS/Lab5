using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels.Users
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Display(Name = "Роль")]
        [RegularExpression("Admin|User|SuperAdmin", ErrorMessage = "Недопустимая роль")]
        public string UserRole { get; set; }

        public EditUserViewModel()
        {
            Id = string.Empty; 
            UserName = string.Empty; 
            Email = string.Empty; 
            UserRole = "User";
        }
    }
}
