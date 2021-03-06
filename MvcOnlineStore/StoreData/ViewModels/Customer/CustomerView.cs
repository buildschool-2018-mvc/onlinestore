﻿using StoreData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreData.ViewModels.Customer
{
    public class CustomerView
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string CustomerID { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string CustomerPassword { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        public string CustomerName { get; set; }

        [DisplayName("電話號碼")]
        [Required(ErrorMessage = "請輸入電話號碼")]
        public string Telephone { get; set; }

        [DisplayName("地址")]
        [Required(ErrorMessage = "請輸入地址")]
        public string Address { get; set; }

        [DisplayName("電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件")]
        public string CustomerMail { get; set; }

        public Customers modelpassword { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string newpassword { get; set; }

        [DisplayName("確認密碼")]
        [Compare("newpassword", ErrorMessage = "兩次密碼輸入不一致")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        public string checknewpassword { get; set; }
    }
}