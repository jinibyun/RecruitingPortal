using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RecruitingPortal.BLL;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;

namespace RecruitingPortal.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // ref: https://stackoverflow.com/questions/22153921/prevent-login-when-emailconfirmed-is-false
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    //ModelState.AddModelError("", "You need to confirm your email.");
                    //return View(model);

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var body = WebUtil.PrepareMailBodyWith(
                                      "BodyTitle", "Confirm your account"
                                    , "BodyContent", string.Format("Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>")
                                      );

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);

                    return View("RegisterConfirmation");
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:                        
                        SetLoggedInUser(EnumLoginFrom.Internal);
                        return RedirectToLocal(returnUrl);

                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error in login " + ex.Message);
                throw ex;
            }
        }                

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string userType)
        {
            if (ModelState.IsValid)
            {
                EnumMemberRole enumMemberRole = (EnumMemberRole)Enum.Parse(typeof(EnumMemberRole), userType);
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // assign role to new user
                    var currentUser = UserManager.FindByName(user.UserName);
                    var roleresult = UserManager.AddToRole(currentUser.Id, userType);

                    // NOTE: do not sign-in yet until user confirmed it
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var body = WebUtil.PrepareMailBodyWith(
                                      "BodyTitle", "Confirm your account"
                                    , "BodyContent", string.Format("Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>")
                                      );


                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);

                    return View("RegisterConfirmation");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }        

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReconfirmEmail(ReconfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "The email is not registered one. Please try it again");
                    return View("RegisterConfirmation");
                }
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var body = WebUtil.PrepareMailBodyWith(
                                      "BodyTitle", "Reconfirm your account"
                                    , "BodyContent", string.Format("Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>")
                                      );


                    await UserManager.SendEmailAsync(user.Id, "ReConfirm your account", body);

                    return View("ReconfirmEmailResult");
                }                

            }
            return View("Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    // return View("ForgotPasswordConfirmation");
                    ViewData["ErrorMessage"] = "Email does not exist. Please correct your email";
                    return View("ForgotPassword");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var body = WebUtil.PrepareMailBodyWith(                                                
                                                "BodyTitle", "Forgot Password"
                                                , "BodyContent", string.Format("Recruiting Portal reset your password. Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>")                                   
                                                );

                await UserManager.SendEmailAsync(user.Id, "Reset Password", body);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation",new { isValid = false } );               
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation(bool isValid = true)
        {
            ViewData["ErrorMessage"] = !isValid ? "Email does not exist. Please correct your email" : "";
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl, string userType)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl, userType = userType }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string userType)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            ///////////////// post ////////////////////
            //if (loginInfo.Login.LoginProvider.Equals("facebook", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
            //    var accessToken = identity.FindFirstValue("FacebookAccessToken");
            //    var fb = new FacebookClient(accessToken);



            //    var args = new Dictionary<string, object>();
            //    args["message"] = "Test message";
            //    args["caption"] = "Caption";
            //    args["description"] = "Description";
            //    args["name"] = "Name";
            //    args["picture"] = "";
            //    args["link"] = "";


            //    //dynamic parameters = new ExpandoObject();
            //    //parameters.title = "test title";
            //    //parameters.message = "test message";

            //    try
            //    {
            //        // ref: https://stackoverflow.com/questions/10397917/facebook-error-some-of-the-aliases-you-requested-do-not-exist-access-token
            //        var pageId = "";
            //        var result2 = fb.Post(pageId + "/feed", args);
            //    }
            //    catch (FacebookApiException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

            //    dynamic myInfo = fb.Get("/me?fields=email");
            //    var email = myInfo.email;
            //}
            ///////////////////////////////////////////

            if (loginInfo.Login.LoginProvider.Equals("facebook", StringComparison.InvariantCultureIgnoreCase))
            {
                // ref: https://stackoverflow.com/questions/32059384/why-new-fb-api-2-4-returns-null-email-on-mvc-5-with-identity-and-oauth-2
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var accessToken = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(accessToken);
                dynamic myInfo = fb.Get("/me?fields=email,first_name,last_name,gender"); // specify the email field
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    SetLoggedInUser(EnumLoginFrom.External);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    // ViewBag.ReturnUrl = returnUrl;
                    // ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    // return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });

                    // instead of showing "associate" view, just add users ( and roles ) automatically   
                    // NOTE: 1. confirm email by force  2. assign role to new user   3. Sign in by force
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        return View("ExternalLoginFailure");
                    }

                    EnumMemberRole enumMemberRole = (EnumMemberRole)Enum.Parse(typeof(EnumMemberRole), userType);
                    var user = new ApplicationUser { UserName = loginInfo.Email, Email = loginInfo.Email };
                    var resultAssociate = await UserManager.CreateAsync(user);
                    if (resultAssociate.Succeeded)
                    {
                        // 1. confirm email by force
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var resultConfirm = await UserManager.ConfirmEmailAsync(user.Id, code);
                        if(!resultConfirm.Succeeded)
                            return View("Error");

                        // 2. assign role to new user
                        var roleresult = UserManager.AddToRole(user.Id, userType);

                        var loginResult = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (loginResult.Succeeded)
                        {
                            // 3. Sign in by force
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            SetLoggedInUser(EnumLoginFrom.External);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    return View("ExternalLoginFailure");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebUtil.ClearSession();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            // TODO: Sign out does not remove oAuth cookie. It should be
            // ref: https://stackoverflow.com/questions/28999318/owin-authentication-signout-doesnt-seem-to-remove-the-cookie
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            return RedirectToAction("Login");
        }

        // Get
        public ActionResult SignOut()
        {
            WebUtil.ClearSession();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // TODO: Sign out does not remove oAuth cookie. It should be
            // ref: https://stackoverflow.com/questions/28999318/owin-authentication-signout-doesnt-seem-to-remove-the-cookie
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void SetLoggedInUser(EnumLoginFrom loginFrom)
        {
            var id = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
            var userName = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserName();
            var role = EnumMemberRole.JobSeeker;
            var claimPrincipal = new ClaimsPrincipal(AuthenticationManager.AuthenticationResponseGrant.Identity);
            if (claimPrincipal.IsInRole(EnumMemberRole.Administrator.ToString()))
            {
                role = EnumMemberRole.Administrator;
            }
            else if (claimPrincipal.IsInRole(EnumMemberRole.Company.ToString()))
            {
                role = EnumMemberRole.Company;
            }
            else if (claimPrincipal.IsInRole(EnumMemberRole.Staff.ToString()))
            {
                role = EnumMemberRole.Staff;
            }

            WebUtil.SetSession<LoggedInUserViewModel>("LoggedInUserViewModel",
                                                        new LoggedInUserViewModel { Id = id, Role = role, UserName = userName, LoginFrom = loginFrom }
                                                     );
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}