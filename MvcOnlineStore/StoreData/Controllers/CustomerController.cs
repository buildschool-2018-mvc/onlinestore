﻿using StoreData.Models;
using StoreData.Services;
using StoreData.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StoreData.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : Controller
    {
        private CustomerService customerService = new CustomerService();
        private LoginService loginservice = new LoginService();
        private OrdersService ordersService = new OrdersService();
        private OrderDetailService orderDetailService = new OrderDetailService();
        private MessageService messageService = new MessageService();
        private Customers customermodel = new Customers();
        private ProductService productService = new ProductService();
        private DeliveryService deliveryService = new DeliveryService();
        private PayService payService = new PayService();

        [Route("CustomerLogin")]
        public ActionResult CustomerLogin()
        {
            return PartialView();
        }
        [Route("CustomerLogin")]
        [HttpPost]
        public ActionResult CustomerLogin(CheckAccount model)
        {
            var Validatestr = loginservice.CustomerLoginCheck(model.CustomerID, model.CustomerPassword);
            if (String.IsNullOrEmpty(Validatestr))
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.CustomerID, DateTime.Now, DateTime.Now.AddMinutes(30), false, "Customer", FormsAuthentication.FormsCookiePath);
                var enTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, enTicket));
                TempData["Message"] = "登入成功";
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", Validatestr);
                TempData["Message"] = "帳號密碼錯誤";
                return RedirectToAction("Index", "Home");
            }
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }
        [Route("SelectCustomer")]
        public ActionResult SelectCustomer()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string CustomerID = ticket.Name;
                return View(customerService.GetAccountByCustomers(CustomerID));
            }
            else
            {
                TempData["Message"] = "尚未登入會員";
                return RedirectToAction("Index", "Home");
            }
        }

        //修改客戶資料
        [HttpPost]
        public ActionResult UpdateCustomer(CustomerView model)
        {
            customerService.UpdateCustomer(model.CustomerID, model.CustomerPassword, model.CustomerName, model.Telephone, model.Address, model.CustomerMail);
            TempData["Message"] = "修改成功";
            return RedirectToAction("Index", "Home");
        }
        //修改密碼
        [Route("UpdatePassword")]
        public ActionResult ChangePassword()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string CustomerID = ticket.Name;
                return View(customerService.GetAccountByCustomers(CustomerID));
            }
            else
            {
                TempData["Message"] = "尚未登入會員";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult ChangeCustomerPassword(CustomerView model)
        {
            customerService.UpdatePassword(model.CustomerID, model.newpassword);
            TempData["Message"] = "修改成功";
            return RedirectToAction("Index", "Home");
        }

        //判斷註冊帳號是否已被註冊過Action
        public JsonResult AccountCheck(CustomerRegisterView RegisterMember)
        {
            return Json(customerService.AccountCheck(RegisterMember.newCustomer.CustomerID), JsonRequestBehavior.AllowGet);
        }
        //註冊一開始顯示頁面
        [Route("Register")]
        public ActionResult Register()
        {
            return PartialView();
        }
        [Route("Register")]
        [HttpPost]
        public ActionResult Register(CustomerRegisterView RegisterMember)
        {
            if (ModelState.IsValid)
            {
                RegisterMember.newCustomer.CustomerPassword = RegisterMember.CustomerPassword;
                customerService.CreateCustomer(RegisterMember.newCustomer);
                TempData["Message"] = "註冊成功";
                return RedirectToAction("Index", "Home");
            }
            RegisterMember.CustomerPassword = null;
            RegisterMember.PasswordCheck = null;
            return RedirectToAction("Index", "Home");
        }
        //購物車列表
        [Route("Chart")]
        public ActionResult Chart()
        {
            var customerId = Get_CustomerId();

            var Data = new CartView();
            Data.DataList = productService.GetCartsList(customerId);

            var deliveries = deliveryService.deliveryGetAll();
            List<SelectListItem> deliveryitems = new List<SelectListItem>();
            foreach (var delivery in deliveries)
            {
                deliveryitems.Add(new SelectListItem()
                {
                    Text = delivery.DeliveryMethod,
                    Value = delivery.DeliveryMethodID.ToString()
                });
            }
            ViewBag.DeliveryMethod = deliveryitems;

            var payments = payService.paymentGetAll();
            List<SelectListItem> paymentitems = new List<SelectListItem>();
            foreach (var payment in payments)
            {
                paymentitems.Add(new SelectListItem()
                {
                    Text = payment.PaymentMethod,
                    Value = payment.PaymentMethodID.ToString()
                });
            }
            ViewBag.PaymenyMethod = paymentitems;

            return View(Data);
        }
        [Route("Chart")]
        [HttpPost]
        public ActionResult Chart(int DeliveryMethodID,int PaymentMethodID)
        {
            var customerId = Get_CustomerId();
            var model = new CreateOrderView();
            model.DeliveryMethodID = DeliveryMethodID;
            model.PaymentMethodID = PaymentMethodID;
            var str = ordersService.Create(customerId, model);
            if (str == "訂單成立")
            {
                TempData["Message"] = str;
                return RedirectToAction("OrderList", "Customer");
            }
            else
            {
                TempData["Message"] = str;
                return RedirectToAction("Chart", "Customer");
            }
        }
        //移除購物車產品
        [Route("RemoveCart")]
        [HttpPost]
        public ActionResult RemoveCart(string productId)
        {
            var customerId = Get_CustomerId();
            productService.RemoveCart(customerId, productId);
            TempData["Message"] = "成功從購物車刪除";
            return RedirectToAction("Chart","Customer");
        }
        //購物車產品數量
        [Route("ProductCount")]
        public ActionResult ProductCount()
        {
            var customerId = Get_CustomerId();
            var count = productService.GetCartsList(customerId).Count();
            ViewBag.count = count;
            return PartialView();
        }
        //取得目前使用者帳號
        public string Get_CustomerId()
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            return ticket.Name;
        }
        [Route("OrderList")]
        public ActionResult OrderList(string orderId, int Page = 1)
        {
            var customerId = Get_CustomerId();
            var data = new OrderView()
            {
                orderId = orderId,
                Paging = new ForPaging(Page)
            };
            data.DataList = ordersService.GetCustomerOrderList(customerId, data.orderId, data.Paging);

            return View(data);
        }
        [Route("OrderDetailList/{orderId}")]
        public ActionResult OrderDetailList(string orderId, int amount)
        {
            var data = new OrderDetailView()
            {
                CustomerId = Get_CustomerId(),
                OrderId = orderId,
                Amount = amount,
            };
            data.OrderDetailDataList = orderDetailService.GetAdminOrders(data.OrderId);
            data.MessageDataList = messageService.GetAdminMessage(data.OrderId);
            return View(data);
        }
        [HttpPost]
        public ActionResult CreateMessage(string orderId, int amount, string Message)
        {
            messageService.Create(orderId, Message);
            return RedirectToAction("OrderDetailList", "Customer", new { orderId, amount });
        }
    }
}