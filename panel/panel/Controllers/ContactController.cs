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
    public class ContactController : BaseController
    {
        private readonly IContactRepo _contactRepo;
        public ContactController(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }
        [Route(template:"addContact", Name ="İletişim Ekle")]
        public async Task<IActionResult> AddContact()
        {
            var contact = await _contactRepo.Get(StaticDetail.StaticDetails.getContact);
            if (contact == null)
            {
                return View();
            }

            return RedirectToAction("UpdateContact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact([FromForm] Contact contact)
        {
            string token = GetToken();
            var res = await _contactRepo.Create(StaticDetail.StaticDetails.createContact, contact, token);
            return RedirectToAction("AddContact");
        }

        //[Route(template:"getAllContacts", Name ="İletişim")]
        //public async Task<IActionResult> GetAllContacts()
        //{
        //    string token = GetToken();
        //    var contact = await _contactRepo.GetList(StaticDetail.StaticDetails.getAllContacts, token);
        //    return View(contact);
        //}

        //[Route(template:"getContact", Name ="İletişim")]
        //public async Task<IActionResult> GetContact()
        //{
        //    var contact = await _contactRepo.Get(StaticDetail.StaticDetails.getContact);
        //    return View(contact);
        //}

        [HttpGet("updateContact")]
        public async Task<IActionResult> UpdateContact()
        {
            var contact = await _contactRepo.Get(StaticDetail.StaticDetails.getContact);
            return View(contact);
        }

        [HttpPost("updateContactContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactContent([FromForm] Contact contact)
        {
            string token = GetToken();
            var res = await _contactRepo.Update(StaticDetail.StaticDetails.updateContact, contact, token);
            return RedirectToAction("AddContact");
        }

        [Route("deleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            string token = GetToken();
            bool result = await _contactRepo.Delete(StaticDetail.StaticDetails.deleteContact + Id, token);
            if (result)
            {
                TempData["success"] = "İletişim bilgileri silindi.  ";
            }
            else
            {
                TempData["fail"] = "İletişim bilgileri silinirken bir hata oluştu";
            }

            return RedirectToAction("AddContact");
        }
    }
}
