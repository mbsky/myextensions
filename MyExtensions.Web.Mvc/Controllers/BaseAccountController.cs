using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Net.Mail;
using System.Web.Mvc.Models;
using System.IO;
using System.Drawing;

namespace System.Web.Mvc.Controllers
{

    [HandleError]
    public class BaseAccountController : ControllerEx
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        protected virtual string LogOnRedirectToUrl
        {
            get
            {
                return Url.Action("Index", "Home", new { area = string.Empty });
            }
        }

        public ActionResult AjaxLogOn(string returnUrl, string redirectUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            ViewData["redirectUrl"] = redirectUrl;
            return PartialView();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl, bool? isAjaxLogon)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);

                    if (isAjaxLogon.HasValue && isAjaxLogon.Value)
                    {
                        return Json(new { Success = true });
                    }

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect(LogOnRedirectToUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form

            if (isAjaxLogon.HasValue && isAjaxLogon.Value)
            {
                return Json(new { Success = false });
            }

            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        protected virtual string LogOffRedirectToUrl
        {
            get
            {
                return Url.Action("Index", "Home", new { area = string.Empty });
            }
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return Redirect(LogOffRedirectToUrl);
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult CheckUserEmail(string email)
        {
            email = email.SafeTrim();

            var username = Membership.GetUserNameByEmail(email);

            if (username.IsNullOrEmpty())
            {
                return Auto("该邮件地址已经被注册,请重新填写");
            }

            return Auto();
        }

        public ActionResult Register()
        {
            ViewData["UserNameLength"] = MembershipService.MinUserNameLength;
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        public virtual string RegisterSuccessRedirectToUrl
        {
            get
            {
                return Url.Action("Index", "Home", new { area = string.Empty });
            }
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipCreateStatus.ProviderError;
                var user = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.Question, model.Answer, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return Redirect(RegisterSuccessRedirectToUrl);
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["UserNameLength"] = MembershipService.MinUserNameLength;
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public virtual ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        // **************************************
        // URL: /Account/PasswordRecovery
        // **************************************

        private MembershipUser CheckPasswordRecoveryEmail(string email)
        {
            email = email.ToLower();

            if (email.IsNullOrEmpty())
            {
                ModelState.AddModelError("email", "请输入您的email ");

                return null;
            }

            if (email.IsEmailAddress() == false)
            {
                ModelState.AddModelError("email", "email地址格式不正确");

                return null;
            }

            string username = Membership.GetUserNameByEmail(email);

            if (username.IsNullOrEmpty())
            {
                ModelState.AddModelError("email", string.Format("不存在用户{0}", email));

                return null;
            }

            return Membership.GetUser(username);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordRecovery()
        {
            return View();
        }

        /// <summary>
        /// 找回密码——验证用户emailPOST
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PasswordRecovery(string email)
        {
            email = email.ToLower();

            var user = CheckPasswordRecoveryEmail(email);

            if (null == user)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                ViewData["question"] = user.PasswordQuestion;
                ViewData["email"] = email;

                return View("PasswordRecoveryQuestion");
            }

            return View();
        }

        /// <summary>
        /// 找回密码——回答密码问题
        /// </summary>
        /// <param name="email"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public ActionResult PasswordRecoveryQuestion(string email, string question)
        {
            return View();
        }

        /// <summary>
        /// 找回密码——回答密码问题 POST
        /// </summary>
        /// <param name="email"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PasswordRecoveryQuestion(string email, string question, string answer, FormCollection fm)
        {
            email = email.ToLower();

            var user = CheckPasswordRecoveryEmail(email);

            if (null == user)
            {
                return RedirectToAction("PasswordRecovery");
            }

            string newPassword = null;

            try
            {
                newPassword = user.ResetPassword(answer);
            }
            catch (MembershipPasswordException)
            {
                ModelState.AddModelError("answer", "答案不正确");
            }

            if (ModelState.IsValid)
            {

                try
                {

                    string ToAddress = user.Email;

                    MailMessage mm = new MailMessage();

                    mm.To.Add(new MailAddress(ToAddress));

                    mm.Subject = "找回密码成功 ";

                    mm.Body = string.Format("您好,{0}，已经为您生成了新密码，请记录下您的这个新密码：", user.UserName) + newPassword;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Send(mm);

                    ViewData["Message"] = string.Format("发送密码邮件到{0}成功，请查收您的邮件！", user.Email);

                    return View("PasswordRecoverySuccess");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("_Form", "发生密码邮件失败，详细错误信息：" + ex.Message);
                    log.Error("Send Password PasswordRecovery Email Failed", ex);

                    return View("PasswordRecoverySuccess");
                }
            }

            ViewData["email"] = email;
            ViewData["question"] = question;

            return View();
        }

        /// <summary>
        /// 找回密码成功
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordRecoverySuccess()
        {
            return View();
        }

        protected virtual void CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 11.5);
            Bitmap image = new Bitmap(iwidth, 20);
            Graphics g = Graphics.FromImage(image);
            Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            Brush b = new System.Drawing.SolidBrush(Color.White);
            //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            g.Clear(Color.Blue);
            g.DrawString(checkCode, f, b, 3, 3);

            Pen blackPen = new Pen(Color.Black, 0);
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                int y = rand.Next(image.Height);
                g.DrawLine(blackPen, 0, y, image.Width, y);
            }

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }

        public ActionResult CreateRandomCode(int codeCount)
        {
            //string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                //int t = rand.Next(35);
                int t = rand.Next(9);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }

            return Content(randomCode);
        }
    }
}
