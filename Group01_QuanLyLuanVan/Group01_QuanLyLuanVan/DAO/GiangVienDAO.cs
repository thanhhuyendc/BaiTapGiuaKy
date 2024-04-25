using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Group01_QuanLyLuanVan.DAO
{

    public class GiangVienDAO
    {
        DBConnection conn = new DBConnection();
        public GiangVien FindOneByUsername(string username)
        {
            string sqlStr = string.Format("select * from GiangVien where username = '{0}'", username);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                GiangVien giangVien = new GiangVien(dr["giangVienId"].ToString(), dr["hoTen"].ToString(), DateTime.Parse(dr["ngaySinh"].ToString()), dr["gioiTinh"].ToString(),
                dr["diaChi"].ToString(), dr["email"].ToString(), dr["sdt"].ToString(), dr["khoaId"].ToString(), dr["username"].ToString());
                return giangVien;
            }
            else
            {
                return null;
            }
        }

        public GiangVien FindOneById(string giangVienId)
        {
            string sqlStr = string.Format("select * from GiangVien where giangVienId = '{0}'", giangVienId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                GiangVien giangVien = new GiangVien(dr["giangVienId"].ToString(), dr["hoTen"].ToString(), DateTime.Parse(dr["ngaySinh"].ToString()), dr["gioiTinh"].ToString(),
                dr["diaChi"].ToString(), dr["email"].ToString(), dr["sdt"].ToString(), dr["khoaId"].ToString(), dr["username"].ToString());
                return giangVien;
            }
            else
            {
                return null;
            }
        }

        public void UpdateGiangVien(GiangVien gv)
        {
            string sqlStr = string.Format("update GiangVien set khoaId='{0}', hoTen=N'{1}', gioiTinh=N'{2}',ngaySinh='{3}', sdt='{4}', email='{5}', diaChi= N'{6}' where giangVienId='{7}'",gv.KhoaId, gv.HoTen,gv.GioiTinh, gv.NgaySinh, gv.SDT, gv.Email, gv.DiaChi, gv.GiangVienId);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }
        public DataTable LoadListGiangVien()
        {
            DataTable dt = new DataTable();
            String sqlStr = string.Format("select * from GiangVien WHERE khoaId = '{0}'", Const.sinhVien.KhoaId);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }

        public DataTable LoadListGiangVienMax()
        {
            DataTable dt = new DataTable();

            String sqlStr = string.Format("SELECT gv.giangVienId, gv.hoTen, COUNT(dt.nhomId) AS SoLuongNhom FROM GiangVien gv LEFT JOIN DeTai dt ON gv.giangVienId = dt.giangVienId AND dt.trangThai = 1 GROUP BY gv.giangVienId, gv.hoTen HAVING COUNT(dt.nhomId) > 2");

            //String sqlStr = string.Format("SELECT gv.giangVienId, gv.hoTen, COUNT(dt.nhomId) AS SoLuongNhom FROM GiangVien gv LEFT JOIN DeTai dt ON gv.giangVienId = dt.giangVienId GROUP BY gv.giangVienId, gv.hoTen");
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
    }
}
