using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class AdminManageStudentViewModel: BaseViewModel
    {
        SinhVienDAO svDAO = new SinhVienDAO();
        private ObservableCollection<SinhVien> _ListStudent;
        public ObservableCollection<SinhVien> ListStudent { get => _ListStudent; set { _ListStudent = value;/* OnPropertyChanged();*/ } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchStudentsCommand { get; set; }

        public ObservableCollection<SinhVien> Students { get; set; }
        public ICommand LoadStudentsCommand { get; set; }
        public AdminManageStudentViewModel()
        {
            Students = new ObservableCollection<SinhVien>();
            var studentsData = svDAO.LoadListSinhVienKhongDuocDuyet();
            foreach (DataRow row in studentsData.Rows)
            {
                string giangVienId = row["sinhVienId"].ToString();
                string hoTen = row["hoTen"].ToString();
                string tenKhoa = row["tenKhoa"].ToString();

                Students.Add(new SinhVien(giangVienId, hoTen, tenKhoa));
            }
            ListStudent = Students;
            ListTK = new ObservableCollection<string>() { "Mã Số", "Họ Tên", "Khoa" };
            SearchStudentsCommand = new RelayCommand<AdminManageStudentView>((p) => { return p == null ? false : true; }, (p) => _SearchStudentsCommand(p));
            LoadStudentsCommand = new RelayCommand<AdminManageStudentView>((p) => true, (p) => _LoadStudentsCommand(p));
        }

        void _LoadStudentsCommand(AdminManageStudentView studentsView)
        {
            studentsView.ListStudentView.ItemsSource = listStudent();
            studentsView.cbxChon.SelectedIndex = 0;
        }
        ObservableCollection<SinhVien> listStudent()
        {
            Students = new ObservableCollection<SinhVien>();
            var studentsData = svDAO.LoadListSinhVienKhongDuocDuyet();
            foreach (DataRow row in studentsData.Rows)
            {
                string giangVienId = row["sinhVienId"].ToString();
                string hoTen = row["hoTen"].ToString();
                string tenKhoa = row["tenKhoa"].ToString();

                Students.Add(new SinhVien(giangVienId, hoTen, tenKhoa));
            }
            return Students;
        }


        void _SearchStudentsCommand(AdminManageStudentView studentsView)
        {
            ObservableCollection<SinhVien> temp = new ObservableCollection<SinhVien>();
            if (studentsView.cbxChon.Text != "")
            {
                switch (studentsView.cbxChon.SelectedItem.ToString())
                {
                    case "Mã Số":
                        {
                            foreach (SinhVien s in ListStudent)
                            {
                                if (s.SinhVienId.ToLower().Contains(studentsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "Khoa":
                        {
                            foreach (SinhVien s in ListStudent)
                            {
                                if (s.TenKhoa.ToLower().Contains(studentsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (SinhVien s in ListStudent)
                            {
                                if (s.HoTen.ToLower().Contains(studentsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                }
                studentsView.ListStudentView.ItemsSource = temp;
            }
            else
                studentsView.ListStudentView.ItemsSource = ListStudent;
        }

    }
}
