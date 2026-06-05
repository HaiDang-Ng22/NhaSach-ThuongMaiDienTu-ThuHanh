using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BookStoreOnline.Models;

namespace BookStoreOnline.Factories
{
    public class ReviewFactory
    {
        // [MỚI] Tạo danh sách đánh giá
        public static List<DANHGIA> CreateReviews(NhaSachEntities3 db)
        {
            return db.DANHGIAs.Include("KHACHHANG").Include("SANPHAM").ToList();
        }

        // [MỚI] Lấy đánh giá theo ID
        public static DANHGIA CreateReviewById(NhaSachEntities3 db, int id)
        {
            return db.DANHGIAs.Find(id);
        }
    }
}
