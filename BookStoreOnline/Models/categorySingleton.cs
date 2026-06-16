using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreOnline.Singleton
{
    public sealed class CategorySingleton
    {
        private static CategorySingleton _instance;
        private static readonly object _lock = new object();
        private NhaSachEntities3 db = new NhaSachEntities3(); // DbContext

        private CategorySingleton() { } // Ngăn tạo instance từ bên ngoài

        public static CategorySingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock) // Đảm bảo thread-safety
                    {
                        if (_instance == null)
                        {
                            _instance = new CategorySingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        // Lấy danh sách danh mục
        public List<LOAI> GetAllCategories()
        {
            return db.LOAIs.ToList();
        }

        // Lấy danh mục theo ID
        public LOAI GetCategoryById(int id)
        {
            return db.LOAIs.AsNoTracking().FirstOrDefault(l => l.Maloai == id);
            
        }

        // Thêm danh mục
        public void AddCategory(LOAI loai)
        {
            db.LOAIs.Add(loai);
            db.SaveChanges();
        }

        // Cập nhật danh mục
        public void UpdateCategory(LOAI loai)
        {
            var existingCategory = db.LOAIs.Find(loai.Maloai);
            if (existingCategory != null)
            {
                db.Entry(existingCategory).CurrentValues.SetValues(loai);
                db.SaveChanges();
            }
        }

        // Xóa danh mục
        public void RemoveCategory(int id)
        {
            var loai = db.LOAIs.Find(id);
            if (loai == null)
            {
                throw new Exception("Không tìm thấy danh mục.");
            }

            // Kiểm tra xem có sản phẩm nào đang dùng danh mục này không
            bool hasProducts = db.SANPHAMs.Any(p => p.MaLoai == id);
            if (hasProducts)
            {
                throw new Exception("Không thể xóa danh mục này vì vẫn còn sản phẩm thuộc về danh mục.");
            }

            db.LOAIs.Remove(loai);
            db.SaveChanges();
        }
    }
}
