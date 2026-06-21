using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreOnline.Models;

namespace BookStoreOnline.Models
{
    public class CartItem
    {
        NhaSachEntities3 db = new NhaSachEntities3();
        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public int? VolumeID { get; set; }
        public string VolumeName { get; set; }


        public decimal FinalPrice()
        {
            return Number * Price;
        }

        public CartItem(int ProductID, int? volumeID = null)
        {
            this.ProductID = ProductID;
            var productDB = db.SANPHAMs.Single(s => s.MaSanPham == this.ProductID);
            this.NamePro = productDB.TenSanPham;
            this.ImagePro = productDB.Anh;
            this.Price = (decimal)productDB.Gia;
            this.Number = 1;
            this.VolumeID = volumeID;

            if (volumeID.HasValue)
            {
                // NOTE: Will only compile after updating EF edmx
                // var volumeDB = db.TAP_SANPHAM.SingleOrDefault(t => t.MaTap == volumeID.Value);
                // if (volumeDB != null) this.VolumeName = volumeDB.TenTap;
                
                // Temp dynamic workaround until edmx is updated:
                var tap = db.Database.SqlQuery<string>("SELECT TenTap FROM TAP_SANPHAM WHERE MaTap = @p0", volumeID.Value).FirstOrDefault();
                if (tap != null) {
                    this.VolumeName = tap;
                }
            }
        }
    }
}