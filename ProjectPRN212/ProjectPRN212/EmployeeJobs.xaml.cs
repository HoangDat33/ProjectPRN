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
            if(em != null)
            {
                if(em.RoleId == 2)
                {
                    LoadIndiJobs();
                    cbAllJob.Visibility = Visibility.Visible;
                }
                if(em.RoleId == 3)
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
            if(em.RoleId == 2)
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
            if(em.RoleId == 3)
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

        }

        private void AddNewJob_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateJob_Click(object sender, RoutedEventArgs e)
        {

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
        private void AllJobOfDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyJobs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbAllJob_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbIndiJob_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbbFilterJobStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
