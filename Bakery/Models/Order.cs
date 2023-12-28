using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public int? BakeryProductId { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(35, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Полеобязательно для заполнения")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]*$", ErrorMessage = "Поле может содержать только буквы (русские и латинские), цифры и пробелы.")]
        [MaxLength(20, ErrorMessage = "Поле не должно превышать 20 символов.")]
        public string? ProductType { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public decimal? Price { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsDamaged { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? DeliveryDate { get; set; }
        
        public virtual Employee? Employee { get; set; }
        public virtual BakeryProduct? BakeryProduct { get; set; }
    }
}
