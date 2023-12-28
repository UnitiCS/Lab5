using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class Supply
    {
        public int SupplyId { get; set; }
        public int? IngredientId { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? Supplier { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? SupplyDate { get; set; }

        public virtual Ingredient? Ingredient { get; set; }
    }
}
