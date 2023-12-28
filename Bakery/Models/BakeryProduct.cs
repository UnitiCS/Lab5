using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class BakeryProduct
    {
        public BakeryProduct()
        {
            Orders = new HashSet<Order>();
            BreadRecipes = new HashSet<BreadRecipe>();
        }

        public int BakeryProductId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле 'Name' может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле 'Name' не должно превышать 20 символов.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле 'Type' может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле 'Type' не должно превышать 20 символов.")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле 'Description' может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(100, ErrorMessage = "Поле не должно превышать 100 символов.")]
        public string? Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<BreadRecipe> BreadRecipes { get; set; }
    }
}
