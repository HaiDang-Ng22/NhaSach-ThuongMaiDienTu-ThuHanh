using BookStoreOnline.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BookStoreOnline.Models
{
    public class ProductProxy
    {
        private readonly NhaSachEntities3 _db;
        private SANPHAM _cachedProduct;
        private List<DANHGIA> _cachedReviews;
        private List<SANPHAM> _cachedRelatedProducts;

        public ProductProxy(NhaSachEntities3 db)
        {
            _db = db;
        }

        public SANPHAM GetProduct(int id)
        {
            return _cachedProduct ?? (_cachedProduct = _db.SANPHAMs
                .Include(p => p.LOAI)
                .FirstOrDefault(p => p.MaSanPham == id));
        }

        public List<DANHGIA> GetReviews(int productId)
        {
            return _cachedReviews ?? (_cachedReviews = _db.DANHGIAs
                .Include(d => d.KHACHHANG)
                .Where(d => d.MaSanPham == productId)
                .ToList());
        }

        public List<SANPHAM> GetRelatedProducts(int productId, int count)
        {
            if (_cachedRelatedProducts == null)
            {
                var mainProduct = GetProduct(productId);
                _cachedRelatedProducts = _db.SANPHAMs
                    .Where(p => p.MaLoai == mainProduct.MaLoai && p.MaSanPham != productId)
                    .Take(count)
                    .ToList();
            }
            return _cachedRelatedProducts;
        }
    }
}