using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class BreadRecipe
    {
        public int BreadRecipeId { get; set; }
        public int? IngredientId { get; set; }
        public int? BakeryProductId { get; set; }


        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле 'Name' может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле 'Name' не должно превышать 20 символов.")]
        public string? IngredientName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int? QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public decimal? Price { get; set; }

        public virtual Ingredient? Ingredient { get; set; }
        public virtual BakeryProduct? BakeryProduct { get; set; }
    }
}
