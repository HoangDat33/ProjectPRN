using Microsoft.IdentityModel.Tokens;
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
            if (em.RoleId == 1)
            {
                txtSalary.IsReadOnly = false;
            }
            LoadData(em.Id);
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

        private void LoadData(int emID)
        {
            var emData = ProjectPrn212Context.INSTANCE.Employees.SingleOrDefault(e => e.Id == emID);
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
            if (em != null)
            {
                Home home = new Home(em);
                this.Hide();
                home.Show();
                this.Close();
            }
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

        private void Changepassword_Click(object sender, RoutedEventArgs e)
        {
            string password = txtOldpassword.Password;
            string newpassword = txtNewpassword.Password;
            string repassword = txtRepassword.Password;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền mật khẩu!", "Thông báo");
                return;
            }
            if (string.IsNullOrEmpty(newpassword))
            {
                MessageBox.Show("Vui lòng điền mật khẩu mới!", "Thông báo");
                return;
            }
            if (string.IsNullOrEmpty(repassword))
            {
                MessageBox.Show("Vui lòng điền xác nhận mật khẩu!", "Thông báo");
                return;
            }

            var authentication = ProjectPrn212Context.INSTANCE.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);

            if (authentication != null)
            {
                if (!password.Equals(authentication.PassWord))
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác!", "Thông báo");
                    return;
                }

                if (!newpassword.Equals(repassword))
                {
                    MessageBox.Show("Vui lòng điền 'Xác nhận mật khẩu' giống với 'Mật khẩu mới'!", "Thông báo");
                    return;
                }

                if (MessageBox.Show("Bạn chắc chắn muốn đổi mật khẩu?", "Thông bảo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    authentication.PassWord = newpassword;
                    if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                    {
                        MessageBox.Show("Cập nhật mật khẩu thành công!", "Thông báo");
                        ClearChangePassWord();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật mật khẩu không thành công!", "Thông báo");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lỗi không tìm thấy tài khoản cho người dùng này, vui lòng thử lại sau!", "Thông báo");
            }
        }


        private void ClearChangePassWord()
        {
            txtOldpassword.Clear();
            txtNewpassword.Clear();
            txtRepassword.Clear();
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (em.RoleId == 1)
            {
                string email = txtEmail.Text;
                string fname = txtFirstname.Text;
                string phone = txtPhone.Text;
                string lname = txtLastname.Text;
                string address = txtAddress.Text;
                string birthday = txtBirthdate.Text;
                DateOnly dob = DateOnly.Parse(birthday);

                bool gender = true;
                if (radioMale.IsChecked == true)
                {
                    gender = true;
                }
                else
                {
                    gender = false;
                }
                bool status = false;
                if (radioActive.IsChecked == false)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }


                if (!double.TryParse(txtSalary.Text, out double salary))
                {
                    MessageBox.Show("Nhập lương nhân viên!", "Thông báo");
                    return;
                }

                var selectedDepart = cbbDepartment.SelectedItem as Department;
                if (selectedDepart == null)
                {
                    MessageBox.Show("Lựa chọn phòng ban!", "Thông báo");
                    return;
                }
                int idDepart = selectedDepart.Id;

                var selectedPosition = cbbPosition.SelectedItem as Position;
                if (selectedPosition == null)
                {
                    MessageBox.Show("Chọn vị trí công việc!", "Thông báo");
                    return;
                }
                int idPosition = selectedPosition.Id;
                if (idPosition == 1 && em.PositionId != 1 && em.ManagerId != em.Id)
                {
                    if (MessageBox.Show("Giao cho nhân viên " + em.FirstName + " " + em.LastName + " thành trưởng phòng?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var changeManager = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == em.ManagerId).SingleOrDefault();
                        List<Employee> chageAllManager = new List<Employee>();
                        if (changeManager != null)
                        {
                            chageAllManager = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.ManagerId == changeManager.Id).ToList();
                        }
                        else
                        {
                            chageAllManager = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == idDepart).ToList();
                        }
                        if (changeManager != null)
                        {
                            changeManager.PositionId = 2;
                            changeManager.ManagerId = em.Id;
                            changeManager.RoleId = 2;
                            ProjectPrn212Context.INSTANCE.Employees.Update(changeManager);
                            ProjectPrn212Context.INSTANCE.SaveChanges();
                        }
                        if (chageAllManager != null)
                        {
                            chageAllManager.ForEach(e => { e.ManagerId = em.Id; });
                        }
                    }
                    var employeeUpdate = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.Id == em.Id);
                    if (employeeUpdate != null)
                    {
                        employeeUpdate.FirstName = fname;
                        employeeUpdate.LastName = lname;
                        employeeUpdate.Email = email;
                        employeeUpdate.Phone = phone;
                        employeeUpdate.Address = address;
                        employeeUpdate.DateOfBirth = dob;
                        employeeUpdate.Gender = gender;
                        employeeUpdate.Salary = (decimal)salary;
                        employeeUpdate.PositionId = idPosition;
                        employeeUpdate.DepartmentId = idDepart;
                        employeeUpdate.ManagerId = em.Id;
                        employeeUpdate.RoleId = 3;
                        employeeUpdate.IsDelete = status;
                        employeeUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                        ProjectPrn212Context.INSTANCE.Employees.Update(employeeUpdate);
                        ProjectPrn212Context.INSTANCE.SaveChanges();
                        var authentication = ProjectPrn212Context.INSTANCE.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
                        authentication.IsDelete = status;
                        MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo");
                    }
                }
                else
                {
                    var selectedManager = cbbManager.SelectedItem as Employee;
                    int idManager = selectedManager.Id;
                    var employeeUpdate = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.Id == em.Id);
                    if (employeeUpdate != null)
                    {
                        employeeUpdate.FirstName = fname;
                        employeeUpdate.LastName = lname;
                        employeeUpdate.Email = email;
                        employeeUpdate.Phone = phone;
                        employeeUpdate.Address = address;
                        employeeUpdate.DateOfBirth = dob;
                        employeeUpdate.Gender = gender;
                        employeeUpdate.Salary = (decimal)salary;
                        employeeUpdate.PositionId = idPosition;
                        employeeUpdate.DepartmentId = idDepart;
                        employeeUpdate.ManagerId = em.Id;
                        employeeUpdate.RoleId = 3;
                        employeeUpdate.IsDelete = status;
                        employeeUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                        ProjectPrn212Context.INSTANCE.Employees.Update(employeeUpdate);
                        ProjectPrn212Context.INSTANCE.SaveChanges();
                        var authentication = ProjectPrn212Context.INSTANCE.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
                        authentication.IsDelete = status;
                        MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                    }
                }

            }
            else
            {
                string email = txtEmail.Text;
                string fname = txtFirstname.Text;
                string phone = txtPhone.Text;
                string lname = txtLastname.Text;
                string address = txtAddress.Text;
                string birthday = txtBirthdate.Text;
                DateOnly dob = DateOnly.Parse(birthday);

                bool gender = true;
                if (radioMale.IsChecked == true)
                {
                    gender = true;
                }
                else
                {
                    gender = false;
                }

                var emUpdate = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.Id == em.Id);
                if (emUpdate != null)
                {
                    emUpdate.FirstName = fname;
                    emUpdate.LastName = lname;
                    emUpdate.Email = email;
                    emUpdate.Phone = phone;
                    emUpdate.Address = address;
                    emUpdate.DateOfBirth = dob;
                    emUpdate.Gender = gender;
                    emUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    ProjectPrn212Context.INSTANCE.Employees.Update(emUpdate);
                    ProjectPrn212Context.INSTANCE.SaveChanges();
                    MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                    LoadData(em.Id);
                }
            }
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
