using Microsoft.AspNetCore.Mvc;
using panel.Models;
using panel.Repository.IRepository;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class FaqController : BaseController
    {
        private readonly IFaqRepo _faqRepo;
        public FaqController(IFaqRepo faqRepo)
        {
            _faqRepo = faqRepo;
        }

        [Route(template: "addFaq", Name = "S.S.S Ekle")]
        public async Task<IActionResult> AddFaq()
        {
            var res = await _faqRepo.GetList(StaticDetail.StaticDetails.getAllFaqs);
            if (res.Count!=0)
            {
                ViewBag.Faq = res;
                return View();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaq([FromForm] Faq faq)
        {
            string token = GetToken();
            var res = await _faqRepo.Create(StaticDetail.StaticDetails.createFaq, faq, token);
            return RedirectToAction("AddFaq");
        }

        [Route(template: "getAllFaqs", Name = "S.S.S'lar")]
        public async Task<IActionResult> GetAllFaqs()
        {
            string token = GetToken();
            var res = await _faqRepo.GetList(StaticDetail.StaticDetails.getAllFaqs, token);
            return View(res);
        }

        [Route(template: "getFaq", Name = "S.S.S.")]
        public async Task<IActionResult> GetFaq()
        {
            var res = await _faqRepo.Get(StaticDetail.StaticDetails.getFaq);
            return View(res);
        }

        [HttpGet("updateFaq/{Id}")]
        public async Task<IActionResult> UpdateFaq(int Id)
        {
            var res = await _faqRepo.Get(StaticDetail.StaticDetails.getFaq+ Id);
            return View(res);
        }

        [HttpPost("updateFaqContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFaqContent([FromForm] Faq faq)
        {
            string token = GetToken();
            var res = await _faqRepo.Update(StaticDetail.StaticDetails.updateFaq, faq, token);
            return RedirectToAction("AddFaq");
        }

        [Route("deleteFaq/{id}")]
        public async Task<IActionResult> DeleteFaq(int Id)
        {
            string token = GetToken();
            bool result = await _faqRepo.Delete(StaticDetail.StaticDetails.deleteFaq + Id, token);
            if (result)
            {
                TempData["success"] = "S.S.S. bilgileri silindi.  ";
            }
            else
            {
                TempData["fail"] = "S.S.S. bilgileri silinirken bir hata oluştu";
            }

            return RedirectToAction("AddFaq");
        }
    }
}
