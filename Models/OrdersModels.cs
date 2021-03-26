using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WeApp1.Models
{
    public class OrdersModels
    {
        public class CheckoutOrder
        {
            
            [Required(ErrorMessage ="Пожалуйста, введите имя пользователя")]
            public string UserName { get; set; }
            [Required(ErrorMessage ="Количестов товаров")]
            public int ProductCount { get; set; }
            [Required(ErrorMessage ="Стоимость")]
            public double Cost { get; set; }
            [Required(ErrorMessage ="Способ доставки")]
            public string DeliveryMethodid { get; set; }
            [Required(ErrorMessage ="Способ отправки")]
            public string PayMethodId { get; set; }
            [Required(ErrorMessage ="Адрес")]
            public string Address { get; set; }
            [Required(ErrorMessage ="Пожалуйста, введите номер телефона")]
            [Phone]
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage ="Пожалуйста, введите адрес электорнной почты")]
            [EmailAddress]
            public string Email { get; set; }
            public string Comment { get; set; }
        }
    }
}