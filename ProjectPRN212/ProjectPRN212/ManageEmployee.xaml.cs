using ProjectPRN212.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;


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
            popup.IsOpen = true;

            var department = ProjectPrn212Context.INSTANCE.Departments.ToList();
            cbbDepart.ItemsSource = department.ToList();
            cbbDepart.DisplayMemberPath = "Name";
            cbbDepart.SelectedValuePath = "ID";

            var positions = ProjectPrn212Context.INSTANCE.Positions.Where(p => p.Id != 1).ToList();
            cbbPosition.ItemsSource = positions;
            cbbPosition.DisplayMemberPath = "Name";
            cbbPosition.SelectedValuePath = "ID";

            var managers = ProjectPrn212Context.INSTANCE.Employees.Where(m => m.RoleId == 3).Select(m => new
            {
                ID = m.Id,
                Fullname = m.FirstName + " " + m.LastName
            }).ToList();
            cbbManager.ItemsSource = managers.ToList();
            cbbManager.DisplayMemberPath = "Fullname";
            cbbManager.SelectedValuePath = "ID";
        }

        private void InsertPopupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = fName.Text;
                string lastName = lName.Text;
                string email = txtEmail.Text;
                string phone = txtPhone.Text;
                string address = txtAddress.Text;
                string birthdate = txtBirthdate.Text;
                DateOnly dateofbirth = DateOnly.Parse(birthdate);
                bool gender = true;
                if (MaleRadioButton.IsChecked == true)
                {
                    gender = true;
                }
                else if (MaleRadioButton.IsChecked == true)
                {
                    gender = false;
                }
                bool active = true;
                if (radioActive.IsChecked == true)
                {
                    active = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    active = true;
                }

                if (!double.TryParse(txtSalary.Text, out double salary))
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Vui lòng nhập lương của nhân viên!", "Thông báo");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        popup.IsOpen = true;
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                    return;
                }
                if (!IsEmailFormatValid(email))
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Email không hợp lệ!", "Thông báo");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        popup.IsOpen = true;
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                    return;
                }
                if (CheckDuplicateEmail(email))
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Email đã tồn tại!", "Thông báo");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        popup.IsOpen = true;
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                    return;
                }
                if (!IsValidPhoneNumber(phone))
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        popup.IsOpen = true;
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                    return;
                }

                var selectedDepartment = cbbDepart.SelectedItem as Department;
                int idDepartment = selectedDepartment.Id;

                var selectedPosition = cbbPosition.SelectedItem as Position;
                int idPosition = selectedPosition.Id;

                var selectedManager = cbbManager.SelectedItem as dynamic;
                int idManager = selectedManager.ID;
                Employee newEmployee = new Employee();
                newEmployee.FirstName = firstName;
                newEmployee.LastName = lastName;
                newEmployee.Email = email;
                newEmployee.Phone = phone;
                newEmployee.Address = address;
                newEmployee.DateOfBirth = dateofbirth;
                newEmployee.Gender = gender;
                newEmployee.Salary = (decimal)salary;
                newEmployee.PositionId = idPosition;
                newEmployee.DepartmentId = idDepartment;
                newEmployee.ManagerId = idManager;
                newEmployee.IsDelete = active;
                newEmployee.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                newEmployee.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                ProjectPrn212Context.INSTANCE.Employees.Add(newEmployee);
                ProjectPrn212Context.INSTANCE.SaveChanges();

                Authentication empAuthentication = new Authentication();
                empAuthentication.EmployeeId = newEmployee.Id;
                empAuthentication.Username = email;
                empAuthentication.PassWord = "12345678";
                empAuthentication.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                empAuthentication.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                ProjectPrn212Context.INSTANCE.Authentications.Add(empAuthentication);
                if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Nhân viên mới đã được thêm thành công!", "Thông báo");
                    ClearPop();
                    LoadDataEmployee();
                }
                else
                {
                    popup.IsOpen = false;
                    MessageBox.Show("Lỗi thêm nhân viên thất bại!", "Thông báo");
                }


            }
            catch (Exception ex)
            {
                popup.IsOpen = false;
                MessageBox.Show("Lỗi thêm mới nhân viên!", "Thông báo");
            }
        }

        private void ClearPop()
        {
            try
            {
                fName.Clear();
                lName.Clear();
                txtAddress.Clear();
                txtBirthdate.SelectedDate = null;
                txtEmail.Clear();
                txtSalary.Clear();
                txtPhone.Clear();
            }
            catch (Exception ex)
            {

            }
        }
        private static bool IsEmailFormatValid(string email)
        {
            // Biểu thức chính quy kiểm tra định dạng email phổ biến
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        private bool CheckDuplicateEmail(string email)
        {
            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Email.Equals(email));
            if (employee.Any())
            {
                return true;
            }
            return false;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            string pattern = @"^0\d{9}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void ExportFile_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Microsoft.Win32.OpenFileDialog saveFileDialog = new Microsoft.Win32.OpenFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Tạo mới tệp Excel";
            bool? result = saveFileDialog.ShowDialog();
            var employee = ProjectPrn212Context.INSTANCE.Employees.ToList();
            if (result == true)
            {
                foreach (var item in employee)
                {

                }
                try
                {
                    string filePath = saveFileDialog.FileName;
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Employees");
                        worksheet.Cells[1, 1].Value = "Employee ID";
                        worksheet.Cells[1, 2].Value = "First Name";
                        worksheet.Cells[1, 3].Value = "Last Name";
                        worksheet.Cells[1, 4].Value = "Email";
                        worksheet.Cells[1, 5].Value = "Phone";
                        worksheet.Cells[1, 6].Value = "Salary";
                        worksheet.Cells[1, 7].Value = "RoleID";
                        worksheet.Cells[1, 8].Value = "DepartmentID";
                        worksheet.Cells[1, 9].Value = "ManagerID";
                        worksheet.Cells[1, 10].Value = "PositionID";
                        worksheet.Cells[1, 11].Value = "DateOfBirth";
                        worksheet.Cells[1, 12].Value = "Gender";
                        worksheet.Cells[1, 13].Value = "Address";
                        worksheet.Cells[1, 14].Value = "CreatedAt";
                        worksheet.Cells[1, 15].Value = "UpdatedAt";
                        worksheet.Cells[1, 16].Value = "DeletedAt";
                        worksheet.Cells[1, 17].Value = "IsDelete";
                        worksheet.Cells[1, 18].Value = "DeleteById";
                        // Bạn có thể thêm các cột khác tùy vào yêu cầu của bạn

                        // Lấy danh sách nhân viên từ cơ sở dữ liệu
                        var employees = ProjectPrn212Context.INSTANCE.Employees.ToList();

                        // Dữ liệu các nhân viên
                        int rowIndex = 2;
                        foreach (var emp in employees)
                        {
                            worksheet.Cells[rowIndex, 1].Value = emp.Id;
                            worksheet.Cells[rowIndex, 2].Value = emp.FirstName;
                            worksheet.Cells[rowIndex, 3].Value = emp.LastName;
                            worksheet.Cells[rowIndex, 4].Value = emp.Email;
                            worksheet.Cells[rowIndex, 5].Value = emp.Phone;
                            worksheet.Cells[rowIndex, 6].Value = emp.Salary;
                            worksheet.Cells[rowIndex, 7].Value = emp.RoleId;
                            worksheet.Cells[rowIndex, 8].Value = emp.DepartmentId;
                            worksheet.Cells[rowIndex, 9].Value = emp.ManagerId;
                            worksheet.Cells[rowIndex, 10].Value = emp.PositionId;
                            worksheet.Cells[rowIndex, 11].Value = emp.DateOfBirth;
                            worksheet.Cells[rowIndex, 12].Value = emp.Gender;
                            worksheet.Cells[rowIndex, 13].Value = emp.Address;
                            worksheet.Cells[rowIndex, 14].Value = emp.CreatedAt;
                            worksheet.Cells[rowIndex, 15].Value = emp.UpdatedAt;
                            worksheet.Cells[rowIndex, 16].Value = emp.DeletedAt;
                            worksheet.Cells[rowIndex, 17].Value = emp.IsDelete;
                            worksheet.Cells[rowIndex, 18].Value = emp.DeletedById;
                            rowIndex++;
                        }

                        // Lưu tệp Excel vào đường dẫn đã chọn
                        FileInfo excelFile = new FileInfo(filePath);
                        package.SaveAs(excelFile);

                        MessageBox.Show("Đã lưu danh sách nhân viên vào tệp Excel thành công.", "Thông báo", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi lưu tệp Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearPopupButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPop();
        }

        private void DeleteNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtEmployeeID.Text, out int idEmployee))
            {
                MessageBox.Show("Vui lòng lựa chọn nhân viên muốn xóa!", "Thông báo");
                return;
            }
            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == idEmployee).SingleOrDefault();
            if (employee != null)
            {
                if(MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var authEm = ProjectPrn212Context.INSTANCE.Authentications.Where(a => a.EmployeeId == employee.Id).SingleOrDefault();
                    if (authEm != null)
                    {
                        authEm.IsDelete = true;
                        employee.IsDelete = true;
                        ProjectPrn212Context.INSTANCE.Authentications.Update(authEm);
                        ProjectPrn212Context.INSTANCE.Employees.Update(employee);
                        ProjectPrn212Context.INSTANCE.SaveChanges();
                    }
                    MessageBox.Show("Xóa nhân viên thành công", "Thông báo");
                    LoadDataEmployee();
                }
            }
        }
    }
}
