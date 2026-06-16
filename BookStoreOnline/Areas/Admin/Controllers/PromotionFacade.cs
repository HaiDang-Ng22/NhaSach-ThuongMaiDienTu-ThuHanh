using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BookStoreOnline.Models;

namespace BookStoreOnline.Services
{
    public class PromotionFacade
    {
        private readonly NhaSachEntities3 _db;

        public PromotionFacade()
        {
            _db = new NhaSachEntities3();
        }

        public List<KHUYENMAI> GetAllPromotions()
        {
            return _db.KHUYENMAIs.ToList();
        }

        public KHUYENMAI GetPromotionById(int id)
        {
            return _db.KHUYENMAIs.Find(id);
        }

        public void AddPromotion(KHUYENMAI promotion)
        {
            _db.KHUYENMAIs.Add(promotion);
            _db.SaveChanges();
        }

        public void UpdatePromotion(KHUYENMAI promotion)
        {
            _db.Entry(promotion).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePromotion(int id)
        {
            var promotion = _db.KHUYENMAIs.Find(id);
            if (promotion != null)
            {
                _db.KHUYENMAIs.Remove(promotion);
                _db.SaveChanges();
            }
        }

        public void TogglePromotionActivation(int id, bool isActive)
        {
            var promotion = _db.KHUYENMAIs.Find(id);
            if (promotion != null)
            {
                promotion.KichHoat = isActive;
                _db.SaveChanges();
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
