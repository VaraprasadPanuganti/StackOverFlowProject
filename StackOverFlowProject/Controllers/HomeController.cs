using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ViewModel;

namespace StackOverFlowProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionsService qs;
        private readonly ICategoriesService cts;

        public HomeController(IQuestionsService _qs, ICategoriesService _categories)
        {
            qs = _qs;
            cts = _categories;
        }

        [Route("/")]
        [Route("home/index")]
        public IActionResult Index()
        {
           List<QuestionViewModel> questions = qs.GetQuestions().Take(10).ToList();
            return View(questions);
        }

        [Route("home/about")]
        public IActionResult About()
        {
            
            return View();
        }

        [Route("home/contact")]
        public IActionResult Contact()
        {

            return View();
        }

        [Route("home/categories")]
        public IActionResult Categories()
        {
            List<CategoryViewModel> categories = cts.GetCategories();
            return View(categories);
        }

        [Route("home/allquestions")]
        public IActionResult Questions()
        {
            List<QuestionViewModel> questions = qs.GetQuestions();
            return View(questions);
        }

        [Route("home/search")]
        public IActionResult Search(string str)
        {
             List<QuestionViewModel> questions = qs.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower()) || temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            //List<QuestionViewModel> questions = qs.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }
    }
}
