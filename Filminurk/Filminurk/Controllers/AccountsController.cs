using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Filminurk.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Filminurk.Models.Accounts;

namespace Filminurk.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FilminurkTARpe24Context _context;
        private readonly IEmailsServices _emailsServices;

        public AccountsController
            (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            FilminurkTARpe24Context context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.DisplayName,
                    Email = model.Email,
                    ProfileType = model.ProfileType,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new {userID = user.Id, token=token}, Request.Scheme);
                    //homework task: koosta email kasutajalt kasutajalt pärineva aadressile saatmiseks, kasutaja saab oma postkastist kätte emaili, kinnituslingiga
                    // mille jaoks kasutatakse tokenit, siin tuleb välja kutsuda vastav, uus, emaili saatmise meetod, mis saadab õige sisuga kirja
                }

                //

                return RedirectToAction("Index", "Home");
            }

            return BadRequest();
        }
    }
}
