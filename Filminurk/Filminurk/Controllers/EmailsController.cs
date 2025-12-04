using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailsServices _emailsServices;
        public IActionResult Index()
        {
            return View();
        }
        public EmailsController(IEmailsServices emailsServices)
        {
            _emailsServices = emailsServices;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var dto = new EmailDTO()
            {
                SendTo = vm.SendTo,
                EmailSubject = vm.EmailSubject,
                EmailContent = vm.EmailContent,
            };
            _emailsServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
