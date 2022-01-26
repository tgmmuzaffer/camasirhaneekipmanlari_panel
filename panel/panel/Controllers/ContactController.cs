using Microsoft.AspNetCore.Mvc;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepo _contactRepo;
        public ContactController(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }
        [Route(template:"addContact", Name ="İletişim Ekle")]
        public IActionResult AddContact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact([FromForm] Contact contact)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _contactRepo.Create(StaticDetail.StaticDetails.createContact, contact, token);

            return RedirectToAction("GetAllContacts");
        }

        [Route(template:"getAllContacts", Name ="İletişim")]
        public async Task<IActionResult> GetAllContacts()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var contact = await _contactRepo.GetList(StaticDetail.StaticDetails.getAllContacts, token);

            return View(contact);
        }

        [HttpGet("updateContact/{Id}")]
        public async Task<IActionResult> UpdateContact(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var contact = await _contactRepo.Get(StaticDetail.StaticDetails.getContact + Id, token);
           
            return View(contact);
        }

        [HttpPost("updateContactContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactContent([FromForm] Contact contact)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _contactRepo.Update(StaticDetail.StaticDetails.updateContact, contact, token);

            return RedirectToAction("GetAllContacts");
        }

        [Route("deleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _contactRepo.Delete(StaticDetail.StaticDetails.deleteContact + Id, token);
            if (result)
            {
                TempData["success"] = "İletişim bilgileri silindi.  ";
            }
            else
            {
                TempData["fail"] = "İletişim bilgileri silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllContacts");
        }
    }
}
