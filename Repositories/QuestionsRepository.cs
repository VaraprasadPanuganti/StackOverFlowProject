using DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IQuestionRepository
    {
        void InsertQuestion(Question q);
        void UpdateQuestionDetails(Question q);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuestionAnswersCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<Question> GetQuestions();
        List<Question> GetQuestionByQuestionID(int QuestionID);
    }
    public class QuestionsRepository : IQuestionRepository
    {
        private readonly StackOverflowDbContext db;

        public QuestionsRepository(StackOverflowDbContext _db)
        {
            db = _db;
        }

        public void InsertQuestion(Question q)
        {
            db.Questions?.Add(q);
            db.SaveChanges();
        }

        public void UpdateQuestionDetails(Question q)
        {
            Question? qt = db.Questions?.Where(temp => temp.QuestionID == q.QuestionID).FirstOrDefault();
            if (qt != null)
            {
                qt.QuestionName = q.QuestionName;
                qt.QuestionDateAndTime = q.QuestionDateAndTime;
                qt.CategoryID = q.CategoryID;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionVotesCount(int qid, int value)
        {
            Question? qt = db.Questions?.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.VotesCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionAnswersCount(int qid, int value)
        {
            Question? qt = db.Questions?.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.AnswersCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionViewsCount(int qid, int value)
        {
            Question? qt = db.Questions?.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.ViewsCount += value;
                db.SaveChanges();
            }
        }

        public void DeleteQuestion(int qid)
        {
            Question? qt = db.Questions?.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                db.Questions?.Remove(qt);
                db.SaveChanges();
            }
        }

        public List<Question> GetQuestions()
        {
            List<Question>? qt = db.Questions?.Include(q => q.User).Include(q => q.Category).Include(q => q.Answers).OrderByDescending(temp => temp.QuestionDateAndTime).ToList();
            return qt;
        }

        public List<Question> GetQuestionByQuestionID(int QuestionID)
        {
            List<Question>? qt = db.Questions?.Include(q=>q.User).Include(q => q.Category).Include(q => q.Answers).Where(temp => temp.QuestionID == QuestionID).ToList();
            return qt;
        }
    }
}
