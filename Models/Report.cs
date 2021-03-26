using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeApp1.Models
{
    public class Report
    {
        [Required(ErrorMessage = "Пожалуйста, введите имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите адес электроной почты")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пожалуйста, выберите причину обращения")]
        public int ReportReason { get; set; }
        public Report()
        {

        }
        public Report(string un, string ema)
        {
            this.Email = ema;
            this.UserName = un;
        }
    }
}