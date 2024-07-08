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
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public UserProfile()
        {
            InitializeComponent();
        }
        Employee em;
        public UserProfile(Employee employee)
        {
            InitializeComponent();
            em = employee;
            radioActive.IsEnabled = false;
            radioNonactive.IsEnabled = false;
            LoadData();
            LoadDataAccount();
        }

        private void LoadDataAccount()
        {
            var account = ProjectPrn212Context.INSTANCE.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
            if (account != null)
            {
                txtUsername.Text = account.Username.ToString();
            }
        }

        private void LoadData()
        {
            var emData = ProjectPrn212Context.INSTANCE.Employees.SingleOrDefault(e => e.Id == em.Id);
            if (emData != null)
            {
                txtID.Text = emData.Id.ToString();
                txtEmail.Text = emData.Email.ToString();
                txtFirstname.Text = emData.FirstName.ToString();
                txtPhone.Text = emData.Phone.ToString();
                txtLastname.Text = emData.LastName.ToString();
                txtSalary.Text = FormatNumber((double)emData.Salary);
                radioFemale.IsChecked = emData.Gender.HasValue && !emData.Gender.Value;
                radioMale.IsChecked = emData.Gender.HasValue && emData.Gender.Value;

                txtBirthdate.Text = emData.DateOfBirth.ToString();
                txtAddress.Text = emData.Address.ToString();
                LoadSubData();

                if (em.DeletedById != null)
                {
                    var selectedDepartment = ProjectPrn212Context.INSTANCE.Departments.FirstOrDefault(d => d.Id == em.DepartmentId);
                    if (selectedDepartment != null)
                    {
                        cbbDepartment.SelectedItem = selectedDepartment;
                    }
                }
                if (em.PositionId != null)
                {
                    var selectedPosition = ProjectPrn212Context.INSTANCE.Positions.FirstOrDefault(p => p.Id == em.PositionId);
                    if (selectedPosition != null)
                    {
                        cbbPosition.SelectedItem = selectedPosition;
                    }
                }
                if (em.ManagerId != null)
                {
                    var selectedManager = ProjectPrn212Context.INSTANCE.Employees.Select(m => new
                    {
                        ID = m.Id,
                        Fullname = m.FirstName + " " + m.LastName,
                    }).FirstOrDefault(m => m.ID == em.ManagerId);
                    if (selectedManager != null)
                    {
                        cbbManager.SelectedItem = selectedManager;
                    }
                }

                txtCreatedAt.Text = emData.CreatedAt.ToString();

                radioActive.IsChecked = emData.IsDelete.HasValue && !emData.IsDelete.Value;
                radioNonactive.IsChecked = emData.IsDelete.HasValue && emData.IsDelete.Value;
            }
        }

        private void LoadSubData()
        {
            var employ = ProjectPrn212Context.INSTANCE.Employees.SingleOrDefault(e => e.Id == em.Id);
            if (employ != null)
            {
                if (employ.RoleId == 1)
                {
                    cbbDepartment.ItemsSource = ProjectPrn212Context.INSTANCE.Departments.ToList();
                    cbbPosition.ItemsSource = ProjectPrn212Context.INSTANCE.Positions.ToList();
                    cbbManager.ItemsSource = ProjectPrn212Context.INSTANCE.Employees
                        .Where(m => m.RoleId == 3)
                        .Select(m => new { ID = m.Id, Fullname = m.FirstName + " " + m.LastName })
                        .ToList();
                }
                else
                {
                    cbbDepartment.ItemsSource = ProjectPrn212Context.INSTANCE.Departments
                        .Where(d => d.Id == em.DepartmentId)
                        .ToList();
                    cbbPosition.ItemsSource = ProjectPrn212Context.INSTANCE.Positions
                        .Where(p => p.Id == em.PositionId)
                        .ToList();
                    cbbManager.ItemsSource = ProjectPrn212Context.INSTANCE.Employees
                        .Where(m => m.Id == em.ManagerId)
                        .Select(m => new { ID = m.Id, Fullname = m.FirstName + " " + m.LastName })
                        .ToList();
                }

                cbbDepartment.DisplayMemberPath = "Name";
                cbbDepartment.SelectedValuePath = "ID";

                cbbPosition.DisplayMemberPath = "Name";
                cbbPosition.SelectedValuePath = "ID";

                cbbManager.DisplayMemberPath = "Fullname";
                cbbManager.SelectedValuePath = "ID";
            }
        }

        public string FormatNumber(double number)
        {
            return number.ToString("#,###,###");
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Changepassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int departmentId = 0;
            var department = cbbDepartment.SelectedItem as Department;
            if (department != null)
            {
                departmentId = department.Id;
                var employeee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == departmentId && e.RoleId == 3).Select(e => new
                {
                    ID = e.Id,
                    Fullname = e.FirstName + " " + e.LastName,
                }).FirstOrDefault();
                if (employeee != null)
                {
                    cbbManager.SelectedItem = employeee;
                }
            }
        }

        private void cbbManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int managerId = 0;
            var manager = cbbManager.SelectedItem as dynamic;
            if (manager != null)
            {
                managerId = manager.ID;
                var employee = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(emp => emp.Id == managerId);
                if (employee != null)
                {
                    var department = ProjectPrn212Context.INSTANCE.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
                    if (department != null)
                    {
                        cbbDepartment.SelectedItem = department;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy phòng ban!", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi tìm quản lý!", "Thông báo");
                }
            }
        }
    }
}
