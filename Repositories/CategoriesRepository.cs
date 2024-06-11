﻿using DomainModel;

namespace Repositories
{
    public interface ICategoriesRepository
    {
        void InsertCategory(Category c);
        void UpdateCategory(Category c);
        void DeleteCategory(int cid);
        List<Category> GetCategories();
        List<Category> GetCategoryByCategoryID(int CategoryID);
    }
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly StackOverflowDbContext db;

        public CategoriesRepository(StackOverflowDbContext _db)
        {
            db = _db;
        }

        public void InsertCategory(Category c)
        {
            db.Categories?.Add(c);
            db.SaveChanges();
        }

        public void UpdateCategory(Category c)
        {
            Category? ct = db.Categories?.Where(temp => temp.CategoryID == c.CategoryID).FirstOrDefault();
            if (ct != null)
            {
                ct.CategoryName = c.CategoryName;
                db.SaveChanges();
            }
        }
        public void DeleteCategory(int cid)
        {
            Category? ct = db.Categories?.Where(temp => temp.CategoryID == cid).FirstOrDefault();
            if (ct != null)
            {
                db.Categories?.Remove(ct);
                db.SaveChanges();
            }
        }
        public List<Category> GetCategories()
        {
            List<Category>? ct = db.Categories?.ToList();
            return ct;
        }
        public List<Category> GetCategoryByCategoryID(int CategoryID)
        {
            List<Category>? ct = db.Categories?.Where(temp => temp.CategoryID == CategoryID).ToList();
            return ct;
        }
    }
}