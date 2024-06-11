using DomainModel;

namespace Repositories
{
    public interface IAnswersRepository
    {
        void InsertAnswer(Answer a);
        void UpdateAnswer(Answer a);
        void UpdateAnswerVotesCount(int aid, int uid, int value);
        void DeleteAnswer(int aid);
        List<Answer> GetAnswersByQuestionID(int qid);
        List<Answer> GetAnswersByAnswerID(int AnswerID);
    }
    public class AnswersRepository : IAnswersRepository
    {
        
        private readonly StackOverflowDbContext db;
        private readonly IQuestionRepository qr;
        private readonly IVotesRepository vr;
        public AnswersRepository(StackOverflowDbContext _db)
        {
            db = _db;
            qr = new QuestionsRepository(_db);
            vr = new VotesRepository(_db);
        }


        public void InsertAnswer(Answer a)
        {
            db.Answers.Add(a);
            db.SaveChanges();
            qr.UpdateQuestionAnswersCount(a.QuestionID, 1);
        }

        public void UpdateAnswer(Answer a)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerID == a.AnswerID).FirstOrDefault();
            if (ans != null)
            {
                ans.AnswerText = a.AnswerText;
                db.SaveChanges();
            }
        }

        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            Answer ans = db?.Answers?.Where(temp => temp.AnswerID == aid)?.FirstOrDefault();
            if (ans != null)
            {
                ans.VotesCount += value;
                db?.SaveChanges();
                qr.UpdateQuestionVotesCount(ans.QuestionID, value);
                vr.UpdateVote(aid, uid, value);
            }
        }

        public void DeleteAnswer(int aid)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerID == aid).First();
            if (ans != null)
            {
                db.Answers.Remove(ans);
                db.SaveChanges();
                qr.UpdateQuestionAnswersCount(ans.QuestionID, -1);
            }
        }
        public List<Answer> GetAnswersByQuestionID(int qid)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.QuestionID == qid).OrderByDescending(temp => temp.AnswerDateAndTime).ToList();
            return ans;
        }
        public List<Answer> GetAnswersByAnswerID(int aid)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.AnswerID == aid).ToList();
            return ans;
        }
    }
}
