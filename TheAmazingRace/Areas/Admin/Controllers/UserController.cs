using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheAmazingRace.Areas.Admin.Models;
using TheAmazingRace.BLL;
using TheAmazingRace.Controllers;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;
using TheAmazingRace.Utilities;

namespace TheAmazingRace.Areas.Admin.Controllers
{
    public partial class UserController : AccountPartialController
    {
        private TheAmazingRaceDbContext context = TheAmazingRaceDbContext.Create();
        private UserService userService = new UserService();

        public string RoleName { get; set; }

        [Authorize(Roles = ("Administrator,Staff"))]
        public virtual ActionResult Manage()
        {
            var currentUserId = User.Identity.GetUserId();
            if (RoleName != "")
            {
                var models = userService.GetAllByRoleName(RoleName);
                return View(models);
            }
            return View();
        }

        [Authorize(Roles = ("Administrator,Staff"))]
        public virtual ActionResult Details(string id)
        {
            if (id != null)
            {
                var user = userService.GetUserById(id);
                ViewData["GenderOptions"] = GenderOptions;
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [Authorize(Roles = ("Administrator,Staff"))]
        public virtual ActionResult Create()
        {
            ViewData["GenderOptions"] = GenderOptions;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Administrator,Staff"))]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var filename = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);

                    if (System.Configuration.ConfigurationManager.AppSettings["StorageAccountName"] != null)
                    {
                        var azureUploadLocation = await StorageHelper.UploadFileToStorage(file.InputStream, filename);

                        if (azureUploadLocation != null)
                        {
                            model.AppUser.PhotoUrl = azureUploadLocation;
                        }
                    }
                    else
                    {
                        var path = Path.Combine(Server.MapPath("~/UploadedImages"), filename);
                        file.SaveAs(path);

                        model.AppUser.PhotoUrl = "/UploadedImages/" + filename;
                    }
                }

                model.AppUser.UserName = model.Email;
                model.AppUser.Email = model.Email;
                model.AppUser.CreatedById = User.Identity.GetUserId();
                model.AppUser.CreatedOn = DateTime.Now;

                var password = GenerateRandomPassword.Generate(12);

                var result = await base.UserManager.CreateAsync(model.AppUser, password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(model.AppUser.Id, RoleName);

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(model.AppUser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = model.AppUser.Id, code = code }, protocol: Request.Url.Scheme);
                    var message = String.Format(@"<html>

<head>
    <meta charset='UTF-8'>
    <meta content='width=device-width, initial-scale=1' name='viewport'>
    <meta content='telephone=no' name='format-detection'>
    <title></title>
</head>

<body>
    <div class='es-wrapper-color'>
        <table class='es-wrapper' width='100%' cellspacing='0' cellpadding='0'>
            <tbody>
                <tr>
                    <td class='esd-email-paddings' valign='top'>
                        <table class='es-header' cellspacing='0' cellpadding='0' align='center'>
                            <tbody>
                                <tr>
                                    <td class='es-adaptive esd-stripe' align='center'>
                                        <table class='es-header-body' width='600' cellspacing='0' cellpadding='0' align='center'>
                                            <tbody>
                                                <tr>
                                                    <td class='esd-structure es-p20t es-p20b es-p40r es-p40l' esd-general-paddings-checked='true' align='left'>
                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td class='esd-container-frame' width='520' valign='top' align='center'>
                                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class='esd-block-image es-m-p0l' align='center'>
                                                                                        <img src='https://ojp8zqasz32qat8n13om56p4-wpengine.netdna-ssl.com/wp-content/uploads/2015/11/TheAmazingRaceLogo.png' width='250'>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table class='es-content' cellspacing='0' cellpadding='0' align='center'>
                            <tbody>
                                <tr>
                                    <td class='esd-stripe' esd-custom-block-id='3109' align='center'>
                                        <table class='es-content-body' style='background-color: rgb(255, 255, 255);' width='600' cellspacing='0' cellpadding='0' bgcolor='#ffffff' align='center'>
                                            <tbody>
                                                <tr>
                                                    <td class='esd-structure es-p20t es-p20b es-p40r es-p40l' esd-general-paddings-checked='true' align='left'>
                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td class='esd-container-frame' width='520' valign='top' align='center'>
                                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class='esd-block-text' align='left'>
                                                                                        <h1 style='color: rgb(74, 126, 176);'>Welcome to The Race</h1>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class='esd-block-spacer es-p5t es-p20b' align='left'>
                                                                                        <table width='5%' height='100%' cellspacing='0' cellpadding='0' border='0'>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style='border-bottom: 2px solid rgb(153, 153, 153); background: rgba(0, 0, 0, 0) none repeat scroll 0% 0%; height: 1px; width: 100%; margin: 0px;'></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class='esd-block-text es-p10b' align='left'>
                                                                                        <br />
                                                                                        <p><span style='font-size: 16px; line-height: 150%;'>Hi {0},</span></p>
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class='esd-block-text' align='left'>
                                                                                        <p>You have been successfully registered to The Amazing Race. You can find your credential below:</p>
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class='esd-block-text' align='left'>
                                                                                        <p>
                                                                                            <b>Role:</b> {1}<br />
                                                                                            <b>Email/Username:</b> {2}<br />
                                                                                            <b>Password:</b> {3}<br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class='esd-block-text' align='left'>
                                                                                        <br />
                                                                                        <p>
                                                                                            Best Regards,<br />
                                                                                            The Amazing Race Team
                                                                                        </p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class='esd-structure es-p20t es-p20b es-p40r es-p40l' esd-general-paddings-checked='true' align='left'>
                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td class='esd-container-frame' width='520' valign='top' align='center'>
                                                                        <table width='100%' cellspacing='0' cellpadding='0'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class='esd-block-spacer es-p20t es-p20b es-p5r' align='center'>
                                                                                        <table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0'>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style='border-bottom: 1px solid rgb(255, 255, 255); background: rgba(0, 0, 0, 0) none repeat scroll 0% 0%; height: 1px; width: 100%; margin: 0px;'></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</body>

</html>", model.AppUser.Name, RoleName, model.Email, password);

                    await UserManager.SendEmailAsync(model.AppUser.Id, "Welcome to The Amazing Race Singapore", message);

                    TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = RoleName + " is successfully created." };
                    return RedirectToAction("Create");
                }
                AddErrors(result);
            }

            ViewData["GenderOptions"] = GenderOptions;
            return View(model);
        }

        [Authorize(Roles = ("Administrator,Staff"))]
        public virtual ActionResult Edit(string id)
        {
            if (id != null)
            {
                var user = userService.GetUserById(id, RoleName);
                ViewData["GenderOptions"] = GenderOptions;

                if(user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize(Roles = ("Administrator,Staff"))]
        public async Task<ActionResult> Edit(User user)
        {
            try
            {
                var newUser = userService.GetUserById(user.Id);

                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.DOB = user.DOB;
                newUser.Gender = user.Gender;
                newUser.UpdatedOn = DateTime.Now;
                newUser.UpdatedById = User.Identity.GetUserId();

                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var filename = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);

                    if (System.Configuration.ConfigurationManager.AppSettings["StorageAccountName"] != null)
                    {
                        var azureUploadLocation = await StorageHelper.UploadFileToStorage(file.InputStream, filename);

                        if (azureUploadLocation != null)
                        {
                            newUser.PhotoUrl = azureUploadLocation;
                        }
                    }
                    else
                    {
                        var path = Path.Combine(Server.MapPath("~/UploadedImages"), filename);
                        file.SaveAs(path);

                        newUser.PhotoUrl = "/UploadedImages/" + filename;
                    }
                }

                userService.Update(newUser);
                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = RoleName + " is successfully updated." };
                return RedirectToAction("Edit", new { id = user.Id });
            }
            catch(Exception ex)
            {
                ViewData["GenderOptions"] = GenderOptions;
                return View(user);
            }
        }

        [Authorize(Roles = ("Administrator,Staff"))]
        public virtual ActionResult Delete(string id)
        {
            if (id != null)
            {
                var user = userService.GetUserById(id, RoleName);
                ViewData["GenderOptions"] = GenderOptions;

                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize(Roles = ("Administrator,Staff"))]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO
                var user = userService.GetUserById(id);
                userService.Delete(user);
                //var deletedUser = UserService.getUserByAppUserId(appUser.User.Id);
                //UserService.Delete(deletedUser);

                return RedirectToAction("Manage");
            }
            catch
            {
                return RedirectToAction("Manage");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public IEnumerable<SelectListItem> GenderOptions
        {
            get
            {
                List<SelectListItem> ddl = new List<SelectListItem>();
                ddl.Add(new SelectListItem { Text = "Male", Value = "Male" });
                ddl.Add(new SelectListItem { Text = "Female", Value = "Female" });
                return ddl;
            }
        }

        [Authorize(Roles = ("Administrator,Staff,Participant"))]
        public virtual ActionResult ManageAccount()
        {
            var user = userService.GetUserById(User.Identity.GetUserId());
            ViewData["GenderOptions"] = GenderOptions;

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize(Roles = ("Administrator,Staff,Participant"))]
        public async Task<ActionResult> ManageAccount(User user)
        {
            ViewData["GenderOptions"] = GenderOptions;

            try
            {
                var newUser = userService.GetUserById(User.Identity.GetUserId());

                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.DOB = user.DOB;
                newUser.Gender = user.Gender;
                newUser.UpdatedOn = DateTime.Now;
                newUser.UpdatedById = User.Identity.GetUserId();

                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var filename = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);

                    if (System.Configuration.ConfigurationManager.AppSettings["StorageAccountName"] != null)
                    {
                        var azureUploadLocation = await StorageHelper.UploadFileToStorage(file.InputStream, filename);

                        if (azureUploadLocation != null)
                        {
                            newUser.PhotoUrl = azureUploadLocation;
                        }
                    }
                    else
                    {
                        var path = Path.Combine(Server.MapPath("~/UploadedImages"), filename);
                        file.SaveAs(path);

                        newUser.PhotoUrl = "/UploadedImages/" + filename;
                    }
                }

                userService.Update(newUser);

                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                // create a new identity from the old one
                var identity = new ClaimsIdentity(User.Identity);
                // update claim value
                identity.RemoveClaim(identity.FindFirst(CustomClaimTypes.UserEntityPhoto));
                identity.AddClaim(new Claim(CustomClaimTypes.UserEntityPhoto, newUser.PhotoUrl));

                authenticationManager.AuthenticationResponseGrant =
                    new AuthenticationResponseGrant(
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties { IsPersistent = true }
                    );

                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = "Profile is successfully updated." };
                return RedirectToAction("ManageAccount");
            }
            catch
            {
                return View(user);
            }
        }

        [Authorize(Roles = ("Administrator,Staff,Participant"))]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Administrator,Staff,Participant"))]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = "Password is successfully updated." };
                return RedirectToAction("ManageAccount");
            }
            AddErrors(result);
            return View(model);
        }
    }
}