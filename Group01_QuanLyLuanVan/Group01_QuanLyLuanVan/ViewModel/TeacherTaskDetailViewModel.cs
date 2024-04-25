﻿using Group01_QuanLyLuanVan.DAO;
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
using System.Windows;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherTaskDetailViewModel : BaseViewModel
    {
        YeuCauDAO ycDAO = new YeuCauDAO();
        MessageTaskDAO messageTaskDAO = new MessageTaskDAO();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddTask { get; set; }
        public ICommand back { get; set; }

        private ObservableCollection<YeuCau> _ListTask;
        public ObservableCollection<YeuCau> ListTask
        {
            get { return _ListTask ?? (_ListTask = new ObservableCollection<YeuCau>()); }
            set { _ListTask = value; }
        }
        private ObservableCollection<YeuCau> _ListMessage;
        public ObservableCollection<YeuCau> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<YeuCau>()); }
            set { _ListMessage = value; }
        }

        public ICommand MessageTaskCommand { get; set; }

        public ObservableCollection<MessageTask> MessageTasks { get; set; }

        public TeacherTaskDetailViewModel()
        {
            back = new RelayCommand<TeacherTaskDetailView>((p) => true, p => _back(p));
            AddTask = new RelayCommand<TeacherTaskDetailView>((p) => true, (p) => _AddTask(p));
            MessageTaskCommand = new RelayCommand<TeacherTaskDetailView>((p) => { return p.ListTaskView.SelectedItem == null ? false : true; }, (p) => _MessageTaskCommand(p));
            var tasksdata = ycDAO.LoadListTask(Const.deTaiId);
            foreach (DataRow row in tasksdata.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);
                ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
        }

        void _MessageTaskCommand(TeacherTaskDetailView teacherTaskDetailView)
        {
            TeacherTaskMessageView messageView = new TeacherTaskMessageView();
            YeuCau temp = (YeuCau)teacherTaskDetailView.ListTaskView.SelectedItem;
            //messageView.TenDeTai.Text = teacherTaskDetailView.TenDeTai.Text;
            messageView.TenTask.Text = temp.NoiDung.ToString();
            MessageTasks = new ObservableCollection<MessageTask>();
            var messages = messageTaskDAO.LoadListMessageTask(temp.YeuCauId);
            foreach (DataRow row in messages.Rows)
            {
                int tinNhanId = int.Parse(row["tinNhanId"].ToString());
                string tinNhan = row["tinNhan"].ToString();
                string nguoiGuiId = row["nguoiGuiId"].ToString();
                string nguoiNhanId = row["nguoiNhanId"].ToString();
                DateTime thoiGian = DateTime.Parse(row["thoiGian"].ToString());
                int yeuCauId = Convert.ToInt32(row["yeuCauId"]);
                MessageTasks.Add(new MessageTask(tinNhanId, tinNhan, nguoiGuiId, nguoiNhanId, thoiGian, yeuCauId));
            }
            //messageView.ListTaskView.ItemsSource = MessageTasks;
           // messageView.ListTaskView.SelectedItem = null;
            TeacherMainViewModel.MainFrame.Content = messageView;
        }
        void _AddTask(TeacherTaskDetailView p)
        {
            if (p.TaskName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung cho task.");
                return;
            }
            else
            {
                ycDAO.AddTask(p.TaskName.Text, 0, Const.deTaiId);
                DataTable dataTable = ycDAO.ListYeuCauByDeTaiId(Const.deTaiId);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                    int trangThai = 0;
                    if (lastRow["TrangThai"].ToString() == "1")
                    {
                        trangThai = 1;
                    }
                    string noiDung = lastRow["noiDung"].ToString();
                    string deTaiId = lastRow["deTaiId"].ToString();
                    int yeuCauId = int.Parse(lastRow["yeuCauId"].ToString());

                    ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
                }
                p.TaskName.Text = "";

                TeacherTaskDetailView topicsView = new TeacherTaskDetailView();
                topicsView.ListTaskView.ItemsSource = listTask();
                topicsView.ListTaskView.Items.Refresh();
                TeacherMainViewModel.MainFrame.Content = topicsView;
            }

        }
        ObservableCollection<YeuCau> listTask()
        {
            ListTask = new ObservableCollection<YeuCau>();
            DataTable dataTable = ycDAO.ListYeuCauByDeTaiId(Const.deTaiId);
            foreach (DataRow row in dataTable.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);
                ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            return ListTask;
        }
        void _back(TeacherTaskDetailView paramater)
        {
            TeacherTaskView tasksView = new TeacherTaskView();
            TeacherMainViewModel.MainFrame.Content = tasksView;
        }
    }
}
