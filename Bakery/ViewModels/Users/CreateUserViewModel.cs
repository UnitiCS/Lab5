using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels.Users
{
    public class CreateUserViewModel
    {
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Роль")]
        public string UserRole { get; set; }
        public CreateUserViewModel()
        {
            UserRole = "User";
            
        }

    }
}
