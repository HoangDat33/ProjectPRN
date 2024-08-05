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
    /// Interaction logic for ManageDepartment.xaml
    /// </summary>
    public partial class ManageDepartment : Window
    {
        Employee em;
        public ManageDepartment()
        {
            InitializeComponent();
        }
        public ManageDepartment(Employee employee)
        {
            InitializeComponent();
            em = employee;
            LoadDataDepartment();
            LoadEmployee();
        }

        private void LoadDataDepartment()
        {
            var departmentDt = ProjectPrn212Context.INSTANCE.Departments.Where(d => d.IsDelete == false).Select(d => new
            {
                DepartmentID = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                Status = (bool)d.IsDelete ? "Ngừng hoạt động" : "Đang hoạt động",
            }).ToList();
            dgDepartment.ItemsSource = departmentDt.ToList();
        }

        private void LoadEmployee()
        {
            var employee = ProjectPrn212Context.INSTANCE.Employees.Select(e => new
            {
                ID = e.Id,
                Fullname = e.FirstName + " " + e.LastName,
            }).ToList();
            cbbManager.ItemsSource = employee;
            cbbManager.DisplayMemberPath = "Fullname";
            cbbManager.SelectedValuePath = "ID";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDepartmentID.Clear();
            txtDepartmentName.Clear();
            txtEmail.Clear();
            txtDescription.Clear();
            dpCreatedAt.Clear();
            dpUpdatedAt.Clear();
            cbbManager.SelectedIndex = -1;

        }

        //private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (!int.TryParse(txtDepartmentID.Text, out int Id))
        //        {
        //            MessageBox.Show("Lỗi xử lí ID!", "Thông báo");
        //            return;
        //        }
        //        var department = ProjectPrn212Context.INSTANCE.Departments.SingleOrDefault(d => d.Id == Id);
        //        if (department != null)
        //        {
        //            var employee = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == Id).ToList();
        //            var job = ProjectPrn212Context.INSTANCE.Jobs.Where(j => j.DepartmentId == Id).ToList();
        //            if (employee.Count != 0)
        //            {
        //                if (MessageBox.Show("Xóa phòng " + department.Name + " sẽ làm cho các nhân viên hiện tại của phòng ban này không có phòng làm việc và xóa hết công việc của phòng này! Chắc chắn muốn xóa phòng " + department.Name + " ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //                {
        //                    employee.ForEach(e => { e.DepartmentId = null; });
        //                    employee.ForEach(e => ProjectPrn212Context.INSTANCE.Employees.Update(e));

        //                    job.ForEach(j => { j.DepartmentId = null; });
        //                    job.ForEach(j => ProjectPrn212Context.INSTANCE.Jobs.Update(j));
        //                    ProjectPrn212Context.INSTANCE.Departments.Remove(department);
        //                    if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
        //                    {
        //                        MessageBox.Show("Xóa phòng ban thành công!", "Thông báo");
        //                        LoadDataDepartment();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Xóa phòng ban thất bại!", "Thông báo");
        //                        return;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (MessageBox.Show("Bạn chắc chắn muốn xóa phòng " + department.Name + " ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //                {
        //                    ProjectPrn212Context.INSTANCE.Departments.Remove(department);
        //                    if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
        //                    {
        //                        MessageBox.Show("Xóa phòng ban thành công!", "Thông báo");
        //                        LoadDataDepartment();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Xóa phòng ban thất bại!", "Thông báo");
        //                        return;
        //                    }
        //                }
        //            }


        //        }
        //        else
        //        {
        //            MessageBox.Show("Không tìm thấy phòng ban trong hệ thông!", "Thông báo");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
        //        return;
        //    }
        //}

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtDepartmentName.Text;
                string description = txtDescription.Text;
                bool isDelete = radioActive.IsChecked != true;

                Department newDepartment = new Department()
                {
                    Name = name,
                    Description = description,
                    IsDelete = isDelete,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                ProjectPrn212Context.INSTANCE.Departments.Add(newDepartment);
                ProjectPrn212Context.INSTANCE.SaveChanges();

                int idEmployee = 0;
                var manager = cbbManager.SelectedItem as dynamic;
                if (manager != null)
                {
                    idEmployee = manager.ID;
                }

                if (manager != null)
                {
                    var managerDp = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == idEmployee).FirstOrDefault();
                    if (managerDp != null)
                    {
                        managerDp.DepartmentId = newDepartment.Id;
                        managerDp.RoleId = 3;
                        managerDp.PositionId = 1;
                        managerDp.ManagerId = managerDp.Id;
                        managerDp.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                        ProjectPrn212Context.INSTANCE.Employees.Update(managerDp);
                    }
                }

                ProjectPrn212Context.INSTANCE.SaveChanges();

                MessageBox.Show("Thêm mới phòng ban thành công!", "Thông báo");
                LoadDataDepartment();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm mới phòng ban! " + ex.Message, "Thông báo");
            }
        }



        private void btnUpdateDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDepartmentID.Text, out int departmentId))
                {
                    MessageBox.Show("Không xử lí được ID phòng ban!", "Thông báo");
                    return;
                }
                string name = txtDepartmentName.Text;
                string description = txtDescription.Text;
                bool isDelete = true;
                if (radioActive.IsChecked == true)
                {
                    isDelete = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    isDelete = true;
                }
                //DateOnly updatedAt = DateOnly.Parse(DateTime.Now());
                if (cbbManager.SelectedItem != null)
                {
                    int idEmployee = 0;
                    var manager = cbbManager.SelectedItem as dynamic;
                    if (manager != null)
                    {
                        idEmployee = manager.ID;
                    }
                    var manageFuture = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.Id == idEmployee);
                    var manageCurrent = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.DepartmentId == departmentId && e.RoleId == 3); //trưởng phòng của phòng này
                    if (manageFuture != null && manageCurrent != null)
                    {
                        if (manageFuture != manageCurrent && manageFuture.RoleId != 3) //nếu nhân viên được chọn không phải trưởng phỏng của phòng này và cũng không phải trưởng phòng 
                        {
                            if (MessageBox.Show("Thay thế trưởng phòng \"" + manageCurrent.FirstName + " " + manageCurrent.LastName + "\" thành \"" + manageFuture.FirstName + " " + manageFuture.LastName + "\"?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                //Bãi nhiệm:
                                manageCurrent.PositionId = 1;
                                manageCurrent.RoleId = 2; //role nhân viên
                                manageCurrent.ManagerId = manageFuture.Id;
                                ProjectPrn212Context.INSTANCE.Employees.Update(manageCurrent);

                                //Lên chức:
                                manageFuture.DepartmentId = departmentId; //chuyển phòng
                                manageFuture.RoleId = 3; //cấp quyền trưởng phòng
                                manageFuture.PositionId = 1; //trưởng phòng
                                manageFuture.ManagerId = manageFuture.Id;
                                ProjectPrn212Context.INSTANCE.Employees.Update(manageFuture);

                                //tất cả nhân viên phòng hiện tại có quản lí mới
                                var employeeOfDepart = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == departmentId).ToList();
                                employeeOfDepart.ForEach(e => e.ManagerId = manageFuture.Id);
                                employeeOfDepart.ForEach(e => ProjectPrn212Context.INSTANCE.Employees.Update(e));

                            }
                            else
                            {
                                return;
                            }
                        }
                        else if (manageFuture != manageCurrent && manageFuture.RoleId == 3)// khác và cũng là trưởng phòng
                        {
                            if (MessageBox.Show("\"" + manageFuture.FirstName + " " + manageFuture.LastName + "\" hiện đang là trưởng phòng của phòng của 1 phòng ban khác, xác nhận thay thế ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                //Bãi nhiệm:
                                manageCurrent.PositionId = 1;
                                manageCurrent.RoleId = 2; //role nhân viên
                                manageCurrent.ManagerId = manageFuture.Id;
                                ProjectPrn212Context.INSTANCE.Employees.Update(manageCurrent);

                                //tất cả nhiên viên ở phòng của trưởng phòng mới bị mất trưởng phòng
                                var employeeOfDepartOfManageFuture = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == manageFuture.DepartmentId).ToList();
                                employeeOfDepartOfManageFuture.ForEach(e => e.ManagerId = null);
                                employeeOfDepartOfManageFuture.ForEach(e => ProjectPrn212Context.INSTANCE.Employees.Update(e));

                                //Lên chức:
                                manageFuture.DepartmentId = departmentId; //chuyển phòng
                                manageFuture.ManagerId = manageFuture.Id;
                                ProjectPrn212Context.INSTANCE.Employees.Update(manageFuture);


                                //tất cả nhân viên phòng hiện tại có quản lí mới
                                var employeeOfDepart = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == departmentId).ToList();
                                employeeOfDepart.ForEach(e => e.ManagerId = manageFuture.Id);
                                employeeOfDepart.ForEach(e => ProjectPrn212Context.INSTANCE.Employees.Update(e));
                                cbbManager.SelectedItem = manageFuture;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }

                var departmentUpdate = ProjectPrn212Context.INSTANCE.Departments.SingleOrDefault(d => d.Id == departmentId);
                if (departmentUpdate != null)
                {
                    departmentUpdate.Name = name;
                    departmentUpdate.Description = description;
                    departmentUpdate.IsDelete = isDelete;
                    var emOfDepa = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == departmentId).ToList();
                    foreach (var item in emOfDepa)
                    {
                        item.IsDelete = isDelete;
                        item.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                        ProjectPrn212Context.INSTANCE.Employees.Update(item);
                    }
                    departmentUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    ProjectPrn212Context.INSTANCE.Departments.Update(departmentUpdate);
                    if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin phòng ban thành công!", "Thông báo");
                        LoadDataDepartment();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thông tin phòng ban thất bại", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Không thể tìm thấy phòng ban này!", "Thông báo");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật phòng ban!" + ex.Message, "Thông báo");
                return;
            }
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

        private void dgDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDepartment.SelectedItem == null)
            {
                return;
            }

            var department = dgDepartment.SelectedItem as dynamic;
            if (department != null)
            {
                if (department.Status == "Đang hoạt động")
                {
                    radioActive.IsChecked = true;
                }
                else
                {
                    radioInactive.IsChecked = true;
                }
                int idDepartment = department.DepartmentID;
                var manager = ProjectPrn212Context.INSTANCE.Employees.Where(m => m.RoleId == 3 && m.DepartmentId == idDepartment).Select(e => new
                {
                    ID = e.Id,
                    Fullname = e.FirstName + " " + e.LastName,
                }).FirstOrDefault();

                if (manager != null && manager.Fullname != null)
                {
                    cbbManager.SelectedItem = manager;
                }
                else
                {
                    cbbManager.SelectedIndex = -1;
                }
                var manager2 = ProjectPrn212Context.INSTANCE.Employees.Where(m => m.RoleId == 3 && m.DepartmentId == idDepartment).FirstOrDefault();
                if (manager2 != null)
                {
                    txtEmail.Text = manager2.Email;
                }

            }
            else
            {
                MessageBox.Show("Null");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string datasearch = txtDatasearch.Text;
                if (datasearch.IsNullOrEmpty())
                {
                    MessageBox.Show("Vui lòng điền gợi í tìm kiếm!", "Thông báo");
                    return;
                }
                var department = ProjectPrn212Context.INSTANCE.Departments.Where(d => d.Name.ToLower().Contains(datasearch) || d.Description.ToLower().Contains(datasearch)).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = (bool)d.IsDelete ? "Ngừng hoạt động" : "Đang hoạt động",
                }).ToList();
                if (department != null)
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng ban!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm " + ex.Message, "Thông báo");
            }
        }

        private void cbAllEmployee_Checked(object sender, RoutedEventArgs e)
        {
            if (cbAllDepartment.IsChecked == true)
            {
                LoadDataDepartment();
            }
            txtDatasearch.Clear();
        }

        private void cbbFilterEmployeeStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbFilterStatus.Items == null)
            {
                return;
            }
            if (cbbFilterStatus.SelectedIndex == 0)
            {
                var department = ProjectPrn212Context.INSTANCE.Departments.Where(d => d.IsDelete == false).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = (bool)d.IsDelete ? "Ngừng hoạt động" : "Đang hoạt động",
                }).ToList();
                if (department.Count > 0)
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                    txtDatasearch.Clear();
                }
                else
                {
                    MessageBox.Show("Không có phòng ban nào hoạt động!", "Thông báo");
                    return;
                }
            }
            else
            {
                var department = ProjectPrn212Context.INSTANCE.Departments.Where(d => d.IsDelete == true).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = (bool)d.IsDelete ? "Ngừng hoạt động" : "Đang hoạt động",
                }).ToList();
                if (department.Count > 0)
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                    txtDatasearch.Clear();
                }
                else
                {
                    MessageBox.Show("Không có phòng ban nào ngừng hoạt động!", "Thông báo");
                    return;
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
                    txtEmail.Text = employee.Email;
                }
                else
                {
                    MessageBox.Show("Không thể tìm thấy email của nhân viên " + manager.Fullname, "Thông báo");
                }
            }
        }
    }
}
