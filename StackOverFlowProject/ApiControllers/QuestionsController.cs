using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace StackOverFlowProject.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
       private readonly IAnswersService asr;

        public QuestionsController(IAnswersService _asr)
        {
            asr = _asr;
        }

        public void Post(int AnswerID, int UserID, int value)
        {
            asr.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}
