using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using StackOverFlowProject.CustomFilters;
using ViewModel;

namespace StackOverFlowProject.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionsService qs;
        private readonly IAnswersService asr;
        private readonly ICategoriesService cs;

        public QuestionsController(IQuestionsService _qs, IAnswersService _asr, ICategoriesService _cs)
        {
            qs = _qs;
            asr = _asr;
            cs = _cs;
        }

        
        [Route("questions/view/{id}")]
        public IActionResult View(int id)
        {
            qs.UpdateQuestionViewsCount(id, 1);
            int uid = Convert.ToInt32(HttpContext?.Session.GetInt32("CurrentUserID"));
            QuestionViewModel qvm = qs.GetQuestionByQuestionID(id, uid);
            return View(qvm);
        }

        [HttpPost]
        [Route("questions/addanswer")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        public IActionResult AddAnswer(NewAnswerViewModel navm)
        {
            navm.UserID = Convert.ToInt32(HttpContext?.Session.GetInt32("CurrentUserID"));
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            if (ModelState.IsValid)
            {
                this.asr.InsertAnswer(navm);
                return RedirectToAction("View", "Questions", new { id = navm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(navm.QuestionID, navm.UserID);
                return View("View", qvm);
            }
        }

        [HttpPost]
        [Route("questions/editanswer")]
        public IActionResult EditAnswer(EditAnswerViewModel avm)
        {
            if (ModelState.IsValid)
            {
                avm.UserID = Convert.ToInt32(HttpContext?.Session.GetInt32("CurrentUserID"));
                this.asr.UpdateAnswer(avm);
                return RedirectToAction("View", new { id = avm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return RedirectToAction("View", new { id = avm.QuestionID });
            }
        }

        [Route("questions/create")]
        public IActionResult Create()
        {
            List<CategoryViewModel> categories = cs.GetCategories();
            ViewBag.categories = categories;
            return View();
        }


        [HttpPost]
        [Route("questions/create")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        public IActionResult Create(NewQuestionViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                qvm.AnswersCount = 0;
                qvm.ViewsCount = 0;
                qvm.VotesCount = 0;
                qvm.QuestionDateAndTime = DateTime.Now;
                qvm.UserID = Convert.ToInt32(HttpContext?.Session.GetInt32("CurrentUserID"));
                this.qs.InsertQuestion(qvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }
    }
}
