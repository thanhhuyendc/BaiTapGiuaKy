using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class AdminManageTeacherViewModel: BaseViewModel
    {
        GiangVienDAO gvDAO = new GiangVienDAO();
        private ObservableCollection<GiangVien> _ListTeacher;
        public ObservableCollection<GiangVien> ListTeacher { get => _ListTeacher; set { _ListTeacher = value;/* OnPropertyChanged();*/ } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTeachersCommand { get; set; }
        
        public ObservableCollection<GiangVien> Teachers { get; set; }
        public ICommand LoadTeachersCommand { get; set; }
        public AdminManageTeacherViewModel()
        {
            Teachers = new ObservableCollection<GiangVien>();
            var teachersData = gvDAO.LoadListGiangVienMax();
            foreach (DataRow row in teachersData.Rows)
            {
                string giangVienId = row["giangVienId"].ToString();
                string hoTen = row["hoTen"].ToString();
                
                int soLuongNhom = Convert.ToInt32(row["SoLuongNhom"]);
                
                Teachers.Add(new GiangVien(giangVienId, hoTen, soLuongNhom));
            }
            ListTeacher = Teachers;
            ListTK = new ObservableCollection<string>() { "Mã Số", "Họ Tên"};
            SearchTeachersCommand = new RelayCommand<AdminManageTeacherView>((p) => { return p == null ? false : true; }, (p) => _SearchTeachersCommand(p));
            LoadTeachersCommand = new RelayCommand<AdminManageTeacherView>((p) => true, (p) => _LoadTeachersCommand(p));
        }
        
        void _LoadTeachersCommand(AdminManageTeacherView teachersView)
        {
            teachersView.ListTeacherView.ItemsSource = listTeacher();
            teachersView.cbxChon.SelectedIndex = 0;
        }
        ObservableCollection<GiangVien> listTeacher()
        {
            Teachers = new ObservableCollection<GiangVien>();
            var teachersData = gvDAO.LoadListGiangVienMax();
            foreach (DataRow row in teachersData.Rows)
            {
                string giangVienId = row["giangVienId"].ToString();
                string hoTen = row["hoTen"].ToString();

                int soLuongNhom = Convert.ToInt32(row["SoLuongNhom"]);

                Teachers.Add(new GiangVien(giangVienId, hoTen, soLuongNhom));
            }
            return Teachers;
        }
        

        void _SearchTeachersCommand(AdminManageTeacherView teachersView)
        {
            ObservableCollection<GiangVien> temp = new ObservableCollection<GiangVien>();
            if (teachersView.cbxChon.Text != "")
            {
                switch (teachersView.cbxChon.SelectedItem.ToString())
                {
                    case "Mã Số":
                        {
                            foreach (GiangVien s in ListTeacher)
                            {
                                if (s.GiangVienId.ToLower().Contains(teachersView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (GiangVien s in ListTeacher)
                            {
                                if (s.HoTen.ToLower().Contains(teachersView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                }
                teachersView.ListTeacherView.ItemsSource = temp;
            }
            else
                teachersView.ListTeacherView.ItemsSource = ListTeacher;
        }


    }
}
