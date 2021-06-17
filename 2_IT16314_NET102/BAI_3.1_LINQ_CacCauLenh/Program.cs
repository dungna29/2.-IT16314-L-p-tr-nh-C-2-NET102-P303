using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAI_3._1_LINQ_CacCauLenh
{
    class Program
    {
        private static ServiceAll sa = new ServiceAll();
        private static List<NhanVien> _lstNhanViens;
        private static List<SanPham> _lstSanPhams;
        private static List<TheLoai> _lsttTheLoais;
        public Program()
        {
            _lstNhanViens = sa.GetListNhanViens();
            _lstSanPhams = sa.GetListSanPhams();
            _lsttTheLoais = sa.GetListTheLoais();
        }
        static void Main(string[] args)
        {
            //Gọi các ví dụ về lý thuyết lên để chạy
            Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
            Program program = new Program();
            ViDuGroupBy();
        }

        #region 1. Toán tử Where để lọc theo điều kiện trả về 1 danh sách hoặc 1 giá trị sau khi thỏa mãn điều kiện

        public static void ViduWhere()
        {
            //Lọc danh sách những người có đầu số sau
            var lst =
                from a in _lstNhanViens
                where a.Sdt.StartsWith("098") || a.Sdt.StartsWith("08")
                select a;
            var lst1 = _lstNhanViens.Where(a => a.Sdt.StartsWith("098") || a.Sdt.StartsWith("08")).Select(c => c.Sdt);//Trả ra 1 tập giá trị số điện thoại có kiểu đữ liệu của sdt.
            foreach (var x in lst)
            {
                x.InRaManHinh();
            }

            Console.WriteLine("==============");
            foreach (var x in lst1)
            {
                Console.WriteLine(x);
            }
        }


        #endregion

        #region  2. Toán tử OfType để lọc theo điều kiện lọc một phần tử trong tập hợp thành một kiểu được chỉ định

        public static void ViduOfType()
        {
            ArrayList arrTemp = new ArrayList();
            arrTemp.Add("Tám");
            arrTemp.Add(9);
            arrTemp.Add("Chín");
            arrTemp.Add(8);

            var lstTemp1 =
                from a in arrTemp.OfType<string>()
                select a;
            var lstTemp2 =
                from a in arrTemp.OfType<int>()
                select a;
            Console.WriteLine("arrTemp.OfType<string>()");
            foreach (var x in lstTemp1)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("arrTemp.OfType<int>()");
            foreach (var x in lstTemp2)
            {
                Console.WriteLine(x);
            }
        }


        #endregion

        #region 3. OrderBy sử dụng để sắp xếp danh sách theo một điều kiện cụ thể

        public static void ViDuOrderBy()
        {
            //Lấy ra 1 danh sách nhân viên được sắp xếp theo tên tăng dần
            var temp =
                from a in _lstNhanViens
                orderby a.TenNV  // ascending || descending
                select a;
            //Cách dùng lambda
            var temp2 = _lstNhanViens.OrderBy(c => c.TenNV);
        }
        //ThenBy và ThenByDescending đi với Orderby và nó là mở rộng để sắp xếp thêm nhiều trường hơn
        public static void ViDuThenBy()
        {
            //Lấy ra 1 danh sách nhân viên được sắp xếp theo tên tăng dần
            var temp2 = _lstNhanViens.OrderBy(c => c.TenNV).ThenBy(c => c.ThanhPho);
            var temp3 = _lstNhanViens.OrderBy(c => c.TenNV).ThenByDescending(c => c.ThanhPho);
        }
        #endregion

        #region 4. GroupBy nhóm các thành phần giống nhau

        public static void ViDuGroupBy()
        {
            //1.
            List<string> lstName = new List<string> { "Trang", "Trang", "Kiều", "Kiều", "A", "B", "C" };
            var temp1 = from a in lstName
                        group a by a
                into g
                        select g.Key;
            // foreach (var x in temp1)
            // {
            //     Console.Write(x + " ");
            // }

            Console.WriteLine();
            //2.Lấy ra danh sách thể loại trong bảng sản phẩm
            var temp2 = from a in _lstSanPhams
                        group a by a.IdTheLoai
                into g
                        select g.Key;
            // foreach (var x in temp2)
            // {
            //     Console.Write(x + " ");
            // }

            //3.
            var temp3 =
                from a in _lstSanPhams
                group a by new //Nhóm khi thỏa mãn 2 điều kiện dưới đây
                {
                    a.IdTheLoai,
                    a.TrangThai
                }
                into g
                select new
                {
                    IdTheLoai = g.Key.IdTheLoai,
                    TrangThai = g.Key.TrangThai,
                    SoLuongTrangThai = g.Count(c=>c.TrangThai == c.TrangThai),
                    SoLuongTheLoai = g.Count(c=>c.IdTheLoai==c.IdTheLoai)
                };
            var temp4 = _lstSanPhams.GroupBy(a => new {a.IdTheLoai, a.TrangThai}).Select(g => new
            {
                IdTheLoai = g.Key.IdTheLoai,
                TrangThai = g.Key.TrangThai,
                SoLuongTrangThai = g.Count(c => c.TrangThai == c.TrangThai),
                SoLuongTheLoai = g.Count(c => c.IdTheLoai == c.IdTheLoai)
            });//Sử dụng LAMBDA
            
            foreach (var x in temp3)
            {
                Console.WriteLine(x.IdTheLoai + " " + x.TrangThai + " SL Trạng Thái: " + x.SoLuongTrangThai + " SoLuongTheLoai = " + x.SoLuongTheLoai);
            }
            //Khi sử dụng Groupby khi cần nhóm các cột dữ liệu giống nhau tạo thành các bản ghi mới và thường đi với các hàm tổng hợp

            //Buổi sau code lại câu đếm số lượng nhân viên sống tại HN sử dụng Groupby
            //Tính tổng giá bán của các sản phẩm có cùng thể loại
        }


        #endregion
    }
}
