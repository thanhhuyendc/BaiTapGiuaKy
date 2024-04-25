﻿using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class MessageTaskDAO
    {
        DBConnection conn = new DBConnection();

        public DataTable LoadListMessageTask(int yeuCauId)
        {
            string sqlStr = string.Format("SELECT * From TinNhanYeuCau WHERE yeuCauId = '{0}'", yeuCauId);
            DataTable tb = conn.Sql_Select(sqlStr);
            return tb;
        }
        public List<YeuCau> ListTask(string deTaiId)
        {
            List<YeuCau> dsYC = new List<YeuCau>();
            string sqlStr = string.Format("SELECT * From YeuCau WHERE deTaiId = '{0}'", deTaiId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dsYC.Add(new YeuCau(int.Parse(tb.Rows[i]["yeuCauId"].ToString()), tb.Rows[i]["noiDung"].ToString(), int.Parse(tb.Rows[i]["trangThai"].ToString()), tb.Rows[i]["deTaiId"].ToString()));
                }
                return dsYC;
            }
            else
            {
                return null;
            }
        }
        public void AddMessage(string noiDung, int trangThai, string deTaiId)
        {
            string sqlStr = string.Format("Insert into YeuCau(noiDung, trangThai, deTaiId) values(N'{0}', '{1}', '{2}')", noiDung, trangThai, deTaiId);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }
        public DataTable ListYeuCauByDeTaiId(string deTaiId)
        {
            DataTable dt = new DataTable();
            string sqlStr = string.Format("SELECT yeuCauId, noiDung, trangThai, deTaiId FROM YeuCau where deTaiId = '{0}'", deTaiId);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
    }
}
