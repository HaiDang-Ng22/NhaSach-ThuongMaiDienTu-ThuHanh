using System;
using System.Collections.Generic;

namespace BookStoreOnline.Models
{
    public partial class SANPHAM : System.ICloneable
    {
        public object Clone()
        {
            // Tạo bản sao nông (shallow copy)
            var clone = (SANPHAM)this.MemberwiseClone();

            // Xử lý các trường đặc biệt
            clone.TenSanPham = "Bản sao của " + this.TenSanPham;
            clone.SoLuongBan = 0;     // Reset số lượng bán
            clone.MaSanPham = 0;      // ID mới sẽ được DB tự sinh
            clone.MaNVTao = null;     // Bỏ nhân viên tạo

            // Giữ nguyên tất cả thông tin quan trọng
            clone.TacGia = this.TacGia;
            clone.Gia = this.Gia;
            clone.MoTa = this.MoTa;
            clone.Anh = this.Anh;
            clone.MaLoai = this.MaLoai;
            clone.SoLuong = this.SoLuong;

            // Xử lý các quan hệ (nếu cần)
            clone.CHITIETDONHANGs = new HashSet<CHITIETDONHANG>();
            clone.DANHGIAs = new HashSet<DANHGIA>();
            clone.LOAI = null;
            clone.NHANVIEN = null;

            return clone;
        }

        // Phiên bản clone có kiểm soát
        public SANPHAM CloneWithCustomName(string newName, int? maNVTao = null)
        {
            var clone = (SANPHAM)this.Clone();
            clone.TenSanPham = newName;
            clone.MaNVTao = maNVTao; // Cho phép gán nhân viên nếu cần
            return clone;
        }
    }
}