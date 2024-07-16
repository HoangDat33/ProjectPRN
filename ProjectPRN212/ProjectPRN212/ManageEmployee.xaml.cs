using ProjectPRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectPRN212
{
    /// <summary>
    /// Interaction logic for ManageEmployee.xaml
    /// </summary>
    public partial class ManageEmployee : Window
    {
        Employee em;
        public ManageEmployee()
        {
            InitializeComponent();
        }
        public ManageEmployee(Employee employee)
        {
            InitializeComponent();
            em = employee;
            LoadDataEmployee();
        }

        private void LoadDataEmployee()
        {
            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.IsDelete == false).Select(e => new
            {
                ID = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = (bool)e.Gender ? "Nam" : "Nữ",
                Address = e.Address,
                Salary = e.Salary,
                Department = e.Department.Name,
                Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                Position = e.Position.Name,
            }).ToList();
            dgEmployee.ItemsSource = employee.ToList();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Login login = new Login();
                this.Hide();
                login.ShowDialog();
                this.Close();
            }
        }

        private void txtDatasearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = txtDatasearch.Text.ToLower();
            var ems = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.FirstName.ToLower().Contains(filterText) || e.LastName.ToLower().Contains(filterText) && e.IsDelete == false).Select(e => new
            {
                ID = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = (bool)e.Gender ? "Nam" : "Nữ",
                Address = e.Address,
                Salary = e.Salary,
                Department = e.Department.Name,
                Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                Position = e.Position.Name,
            }).ToList();
            dgEmployee.ItemsSource = ems.ToList();
            cbAllEmployee.IsChecked = false;
        }

        private void cbAllEmployee_Checked(object sender, RoutedEventArgs e)
        {
            LoadDataEmployee();
        }

        private void cbbFilterStatusEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idStatus = cbbFilterEmployeeStatus.SelectedIndex;
            if (idStatus == 0)
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.IsDelete == false).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Count > 0)
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo");
                    return;
                }
            }
            if (idStatus == 1)
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.IsDelete == true).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Count > 0)
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo");
                    return;
                }
            }
            if (idStatus == 2)//Nam
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Gender == true).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Count > 0)
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo");
                    return;
                }
            }

            if (idStatus == 3)//Nữ
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Gender == false).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Count > 0)
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo");
                    return;
                }
            }

            if (idStatus == 4)//trưởng phòng
            {
                FilterPosition(1);
            }
            if (idStatus == 5)//nhân viên chính
            {
                FilterPosition(2);
            }
            if (idStatus == 6) //nhân viên partime
            {
                FilterPosition(3);
            }
            if (idStatus == 7)
            {
                FilterPosition(4);
            }
        }

        private void FilterPosition(int id)
        {
            dgEmployee.ItemsSource = string.Empty;
            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.PositionId == id).Select(e => new
            {
                ID = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = (bool)e.Gender ? "Nam" : "Nữ",
                Address = e.Address,
                Salary = e.Salary,
                Department = e.Department.Name,
                Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                Position = e.Position.Name,
            }).ToList();
            if (employee.Count > 0)
            {
                dgEmployee.ItemsSource = employee.ToList();
                cbAllEmployee.IsChecked = false;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên nào đã bị xóa!", "Thông báo");
                return;
            }
        }

        private void DetailEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtEmployeeID.Text, out int idEmployee))
            {
                MessageBox.Show("Vui lòng lựa chọn nhân viên muốn xem chi tiết!", "Thông báo");
                return;
            }
            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == idEmployee).SingleOrDefault();
            if (employee != null)
            {
                UserProfile userprofile = new UserProfile(em, employee);
                this.Hide();
                userprofile.ShowDialog();
                this.Close();
            }
        }

        private void AddNewEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
