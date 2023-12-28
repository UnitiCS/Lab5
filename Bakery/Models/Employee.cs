using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class Employee
    {
        public Employee() 
        {
            Orders = new HashSet<Order>();
        }
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? Position { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public decimal Salary { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
