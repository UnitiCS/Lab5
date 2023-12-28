using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            BreadRecipes = new HashSet<BreadRecipe>();
            Supplies = new HashSet<Supply>();
        }

        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int? Quantity { get; set; }

        public virtual ICollection<BreadRecipe> BreadRecipes { get; set; }
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
