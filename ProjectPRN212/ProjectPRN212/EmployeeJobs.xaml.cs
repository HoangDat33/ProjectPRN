using Microsoft.VisualBasic;
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
    /// Interaction logic for EmployeeJobs.xaml
    /// </summary>
    public partial class EmployeeJobs : Window
    {
        Employee em = new Employee();
        public EmployeeJobs()
        {
            InitializeComponent();
            LoadDataJobs();
            LoadDataJobStatus();
        }
        public EmployeeJobs(Employee employ)
        {
            InitializeComponent();
            em = employ;
            if (em != null)
            {
                if (em.RoleId == 2)
                {
                    LoadIndiJobs();
                    cbIndiJob.Visibility = Visibility.Visible;
                }
                if (em.RoleId == 3)
                {
                    LoadDataJobs();
                    cbAllJob.Visibility = Visibility.Visible;
                    cbIndiJob.Visibility = Visibility.Visible;
                }

            }
            LoadDataJobStatus();
            LoadAssignBy();
            LoadEmployee();
        }

        private void LoadIndiJobs()
        {
            dgEmJobs.ItemsSource = string.Empty;
            var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.EmployeeId == em.Id && j.IsDelete == false).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmJobs.ItemsSource = job.ToList();
        }

        private void LoadDataJobs()
        {
            var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.IsDelete == false).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmJobs.ItemsSource = job.ToList();
        }

        private void LoadDataJobStatus()
        {
            var jobStatus = ProjectPrn212Context.INSTANCE.JobStatuses.ToList();
            cbbFilterJobStatus.ItemsSource = jobStatus.ToList();
            cbbFilterJobStatus.DisplayMemberPath = "Name";
            cbbFilterJobStatus.SelectedValuePath = "ID";
            cbbStatus.ItemsSource = jobStatus.ToList();
            cbbStatus.DisplayMemberPath = "Name";
            cbbStatus.SelectedValuePath = "ID";
        }

        private void LoadAssignBy()
        {
            var employeedata = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == em.Id && e.DepartmentId == em.DepartmentId && e.IsDelete == false).Select(e => new
            {
                ID = e.Id,
                FullName = $"{e.FirstName} {e.LastName}"
            }).ToList();
            cbbSelectAssign.ItemsSource = employeedata;
            cbbSelectAssign.DisplayMemberPath = "FullName";
            cbbSelectAssign.SelectedValuePath = "ID";
        }

        private void LoadEmployee()
        {
            if (em.RoleId == 2)
            {
                var employeedata = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.Id == em.Id && e.DepartmentId == em.DepartmentId && e.IsDelete == false).Select(e => new
                {
                    ID = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}"
                }
                ).ToList();

                cbbSelectEmployee.ItemsSource = employeedata;
                cbbSelectEmployee.DisplayMemberPath = "FullName";
                cbbSelectEmployee.SelectedValuePath = "ID";
            }
            if (em.RoleId == 3)
            {
                var employeedata = ProjectPrn212Context.INSTANCE.Employees.Where(e => e.DepartmentId == em.DepartmentId && e.IsDelete == false).Select(e => new
                {
                    ID = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}"
                }).ToList();
                cbbSelectEmployee.ItemsSource = employeedata.ToList();
                cbbSelectEmployee.DisplayMemberPath = "FullName";
                cbbSelectEmployee.SelectedValuePath = "ID";
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtJobID.Clear();
            txtJobName.Clear();
            txtDesription.Clear();
            txtEndDate.SelectedDate = null;
            txtStartDate.SelectedDate = null;
            cbbStatus.SelectedItem = null;
            cbbSelectAssign.SelectedItem = null;
            txtAssignDate.SelectedDate = null;
            cbbSelectEmployee.SelectedItem = null;
            txtEmployeeJobID.Clear();
        }

        private void DeleteJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jobID = txtJobID.Text;
                if (!int.TryParse(jobID, out int jobIDInt))
                {
                    MessageBox.Show("Không tồn tại JobID!", "Thông báo");
                    return;
                }
                string employeeJobId = txtEmployeeJobID.Text;
                if (!int.TryParse(employeeJobId, out int employeeJobIdInt))
                {
                    MessageBox.Show("Không tồn tại EmployeeJobID!", "Thông báo");
                    return;
                }

                //tìm data job
                Job job = ProjectPrn212Context.INSTANCE.Jobs.SingleOrDefault(j => j.Id == jobIDInt);
                //tìm data employeeJob
                EmployeeJob employeeJob = ProjectPrn212Context.INSTANCE.EmployeeJobs.SingleOrDefault(j => j.EmployeeJobId == employeeJobIdInt);

                //check xem có quyền xóa công việc này hay không
                if (job.AssignedBy == em.ManagerId && job.AssignedBy != em.Id) //công việc này do trường phòng giao và trưởng phòng không phải là mình
                {
                    MessageBox.Show("Bạn không thể xóa công việc do quản lý của bạn giao!", "Thông báo");
                }
                else
                {
                    if (employeeJob != null && job != null)
                    {
                        if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu công việc này!", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            //database.EmployeeJobs.Remove(employeeJob);
                            //database.Jobs.Remove(job);
                            job.IsDelete = true;
                            employeeJob.IsDelete = true;
                        }
                        if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                        {
                            MessageBox.Show("Xóa dữ liệu công việc thành công!", "Thông báo");
                            //LoadDataJobs();
                            if (em.RoleId == 2)
                            {
                                LoadIndiJobs();
                            }
                            else if (em.RoleId == 3)
                            {
                                LoadDataJobs();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa dữ liệu công việc thất bại!" + ex.Message, "Thông báo");
            }
        }

        private void AddNewJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Data bảng employeeJobs:
                DateOnly assignmentDate = DateOnly.FromDateTime(txtAssignDate.SelectedDate.Value);
                int idEmployee = (int)cbbSelectEmployee.SelectedValue;

                //Data bảng jobs:
                string jobName = txtJobName.Text;
                string jobDescription = txtDesription.Text;
                //(int)cbbStatus.SelectedValue;
                JobStatus selectedJobStatus = cbbStatus.SelectedItem as JobStatus;
                int idStatus = 0;
                if (selectedJobStatus != null)
                {
                    idStatus = selectedJobStatus.Id;
                }

                int idAssignBy = em.Id;

                DateOnly startDate = DateOnly.FromDateTime(txtStartDate.SelectedDate.Value);
                DateOnly endDate = DateOnly.FromDateTime(txtEndDate.SelectedDate.Value);


                if (string.IsNullOrWhiteSpace(jobName))
                {
                    MessageBox.Show("Vui lòng nhập tên công việc!", "Thông báo");
                    return;
                }

                if (string.IsNullOrWhiteSpace(jobDescription))
                {
                    MessageBox.Show("Vui lòng nhập mô tả công việc!", "Thông báo");
                    return;
                }

                if (idStatus == 0)
                {
                    MessageBox.Show("Vui lòng lựa chọn trạng thái công việc!", "Thông báo");
                    return;
                }

                if (idEmployee == 0)
                {
                    MessageBox.Show("Vui lòng lựa chọn nhân viên đảm nhận công việc!", "Thông báo");
                    return;
                }

                if (startDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày bắt đầu công việc!", "Thông báo");
                    return;
                }

                if (endDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày kết thúc công việc!", "Thông báo");
                    return;
                }

                if (assignmentDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày giao việc!", "Thông báo");
                    return;
                }

                Job newJob = new Job();
                newJob.Title = jobName;
                newJob.Description = jobDescription;
                newJob.AssignedBy = idAssignBy;
                newJob.DepartmentId = em.DepartmentId;
                newJob.StartDate = startDate;
                newJob.EndDate = endDate;
                newJob.JobStatusId = idStatus;
                newJob.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                newJob.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                ProjectPrn212Context.INSTANCE.Jobs.Add(newJob);
                ProjectPrn212Context.INSTANCE.SaveChanges();

                Job newJobInDatabase = ProjectPrn212Context.INSTANCE.Jobs.OrderByDescending(j => j.Id).FirstOrDefault();


                EmployeeJob newEmployeejob = new EmployeeJob();

                newEmployeejob.EmployeeId = idEmployee;
                newEmployeejob.JobId = newJobInDatabase.Id;
                newEmployeejob.AssignmentDate = assignmentDate;
                newEmployeejob.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                newEmployeejob.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                ProjectPrn212Context.INSTANCE.EmployeeJobs.Add(newEmployeejob);

                if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                {
                    MessageBox.Show("Thêm mới công việc thành công!", "Thông báo");
                    //LoadDataJobs();
                    LoadDataJobs();
                }
                else
                {
                    MessageBox.Show("Thêm mới công việc thất bại!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm mới công việc thất bại!" + ex.Message, "Thông báo");
            }
        }

        private void UpdateJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jobID = txtJobID.Text;
                if (!int.TryParse(jobID, out int jobIDInt))
                {
                    MessageBox.Show("Không tồn tại JobID!", "Thông báo");
                    return;
                }

                //Data bảng employeeJobs:
                string employeeJobId = txtEmployeeJobID.Text;
                if (!int.TryParse(employeeJobId, out int employeeJobIdInt))
                {
                    MessageBox.Show("Không tồn tại EmployeeJobID!", "Thông báo");
                    return;
                }

                DateOnly assignmentDate = DateOnly.FromDateTime(txtAssignDate.SelectedDate.Value);
                int idEmployee = (int)cbbSelectEmployee.SelectedValue;

                //Data bảng jobs:
                string jobName = txtJobName.Text;
                string jobDescription = txtDesription.Text;
                //(int)cbbStatus.SelectedValue;
                JobStatus selectedJobStatus = cbbStatus.SelectedItem as JobStatus;
                int idStatus = 0;
                if (selectedJobStatus != null)
                {
                    idStatus = selectedJobStatus.Id;
                }

                int idAssignBy = em.Id;

                DateOnly startDate = DateOnly.FromDateTime(txtStartDate.SelectedDate.Value);
                DateOnly endDate = DateOnly.FromDateTime(txtEndDate.SelectedDate.Value);


                if (string.IsNullOrWhiteSpace(jobName))
                {
                    MessageBox.Show("Vui lòng nhập tên công việc!", "Thông báo");
                    return;
                }

                if (string.IsNullOrWhiteSpace(jobDescription))
                {
                    MessageBox.Show("Vui lòng nhập mô tả công việc!", "Thông báo");
                    return;
                }

                if (idStatus == 0)
                {
                    MessageBox.Show("Vui lòng lựa chọn trạng thái công việc!", "Thông báo");
                    return;
                }

                if (idEmployee == 0)
                {
                    MessageBox.Show("Vui lòng lựa chọn nhân viên đảm nhận công việc!", "Thông báo");
                    return;
                }

                if (startDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày bắt đầu công việc!", "Thông báo");
                    return;
                }

                if (endDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày kết thúc công việc!", "Thông báo");
                    return;
                }

                if (assignmentDate == null)
                {
                    MessageBox.Show("Vui lòng lựa chọn ngày giao việc!", "Thông báo");
                    return;
                }

                //update bảng job;
                Job job = ProjectPrn212Context.INSTANCE.Jobs.SingleOrDefault(j => j.Id == jobIDInt);
                if (job != null)
                {
                    job.Title = jobName;
                    job.Description = jobDescription;
                    job.StartDate = startDate;
                    job.EndDate = endDate;
                    job.AssignedBy = idAssignBy;
                    job.JobStatusId = idStatus;
                    job.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                }



                //update bảng employeeJob
                EmployeeJob employeeJob = ProjectPrn212Context.INSTANCE.EmployeeJobs.SingleOrDefault(j => j.EmployeeJobId == employeeJobIdInt);
                if (employeeJob != null)
                {
                    employeeJob.EmployeeId = idEmployee;
                    employeeJob.AssignmentDate = assignmentDate;
                    employeeJob.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                }

                if (ProjectPrn212Context.INSTANCE.SaveChanges() > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu công việc thành công!", "Thông báo");
                    if (em.RoleId == 3)
                    {
                        LoadDataJobs();
                    }
                    else
                    {
                        LoadIndiJobs();
                    }

                }
                else
                {
                    MessageBox.Show("Cập nhật dữ liệu thất bại!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật dữ liệu thất bại!" + ex.Message, "Thông báo");
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

        private void txtDatasearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (em.RoleId == 3)
            {
                if (cbIndiJob.IsChecked == true)
                {
                    var filterText = txtDatasearch.Text.ToLower();
                    dgEmJobs.ItemsSource = string.Empty;
                    var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.Title.ToLower().Contains(filterText) && j.EmployeeId == em.Id && j.IsDelete == false).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                }
                else
                {
                    var filterText = txtDatasearch.Text.ToLower();
                    dgEmJobs.ItemsSource = string.Empty;
                    var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.Title.ToLower().Contains(filterText) && j.IsDelete == false).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                    cbIndiJob.IsChecked = false;
                }
            }
            if (em.RoleId == 2)
            {
                var filterText = txtDatasearch.Text.ToLower();
                dgEmJobs.ItemsSource = string.Empty;
                var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.Title.ToLower().Contains(filterText) && j.EmployeeId == em.Id && j.IsDelete == false).Select(j => new
                {
                    ID = j.EmployeeJobId,
                    JobID = j.JobId,
                    JobName = j.Job.Title,
                    Description = j.Job.Description,
                    EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                    AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                    StartDate = j.Job.StartDate,
                    EndDate = j.Job.EndDate,
                    AssigmentDate = j.AssignmentDate,
                    Status = j.Job.JobStatus.Name,
                }).ToList();
                dgEmJobs.ItemsSource = job.ToList();
                cbAllJob.IsChecked = false;
            }
        }

        private void cbAllJob_Checked(object sender, RoutedEventArgs e)
        {
            LoadDataJobs();
            cbIndiJob.IsChecked = false;
        }

        private void cbIndiJob_Checked(object sender, RoutedEventArgs e)
        {
            dgEmJobs.ItemsSource = string.Empty;
            var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.EmployeeId == em.Id).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmJobs.ItemsSource = job.ToList();
            cbAllJob.IsChecked = false;
        }

        private void cbbFilterJobStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (em.RoleId == 3)
            {
                if (cbIndiJob.IsChecked == true)
                {
                    var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                    dgEmJobs.ItemsSource = string.Empty;
                    var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId && j.EmployeeId == em.Id && j.IsDelete == false).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                }
                else
                {
                    var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                    dgEmJobs.ItemsSource = string.Empty;
                    var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId && j.IsDelete == false).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                    cbIndiJob.IsChecked = false;
                }
            }
            if (em.RoleId == 2)
            {
                var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                dgEmJobs.ItemsSource = string.Empty;
                var job = ProjectPrn212Context.INSTANCE.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId && j.EmployeeId == em.Id && j.IsDelete == false).Select(j => new
                {
                    ID = j.EmployeeJobId,
                    JobID = j.JobId,
                    JobName = j.Job.Title,
                    Description = j.Job.Description,
                    EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                    AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                    StartDate = j.Job.StartDate,
                    EndDate = j.Job.EndDate,
                    AssigmentDate = j.AssignmentDate,
                    Status = j.Job.JobStatus.Name,
                }).ToList();
                dgEmJobs.ItemsSource = job.ToList();
                cbAllJob.IsChecked = false;
            }
        }
    }
}
