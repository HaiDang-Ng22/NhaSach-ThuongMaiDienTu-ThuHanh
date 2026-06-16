using System;
using System.Collections.Generic;
using System.Linq;
using BookStoreOnline.Models;

namespace BookStoreOnline.Core
{
    public interface Iterator
    {
        NHANVIEN First();
        NHANVIEN Next();
        bool IsDone { get; }
        NHANVIEN CurrentItem { get; }

    }

    public class AdminAccountsIterator : Iterator
    {
         List<NHANVIEN> _nhanVienList;
         int _current = 0;
        int step = 1;

        public AdminAccountsIterator(List<NHANVIEN> nhanViens)
        {
            _nhanVienList = nhanViens;
        }
        public bool IsDone { get { return _current >= _nhanVienList.Count; } }

        public NHANVIEN CurrentItem =>  _nhanVienList[_current];

        public NHANVIEN First()
        {
            _current = 0;
            if (_nhanVienList.Count > 0)
            {
                return _nhanVienList[_current];
                           }
            return null;
        }

        public NHANVIEN Next()
        {
           _current += step;
            if (!IsDone)
            {
                return _nhanVienList[_current];
            }
            return null;
        }

        

    }
}
