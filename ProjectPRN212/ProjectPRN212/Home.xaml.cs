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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }
        Employee em = new Employee();
        public Home(Employee employee)
        {
            InitializeComponent();
            em = employee;
            if (em != null)
            {
                if (em.RoleId == 1)
                {
                    adminFunc.Visibility = Visibility.Visible;
                }
                else if (em.RoleId == 2)
                {
                    userFunc.Visibility = Visibility.Visible;
                }
                else
                {
                    manageFunc.Visibility = Visibility.Visible;
                }
            }
        }

        private void ProfileDetail_Click(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = new UserProfile(em);
            this.Hide();
            userProfile.Show();
            this.Close();
        }

        private void EmployeeJobs_Click(object sender, RoutedEventArgs e)
        {
            EmployeeJobs employeejobs = new EmployeeJobs(em);
            this.Hide();
            employeejobs.ShowDialog();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Login login = new Login();
                this.Hide();
                login.Show();
                this.Close();
            }
        }

        private void EmployeeList_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployee mnemploy = new ManageEmployee(em);
            this.Hide();
            mnemploy.ShowDialog();
            this.Close();
        }

        private void ManageDepart_Click(object sender, RoutedEventArgs e)
        {
            ManageDepartment manageDepartment = new ManageDepartment(em);
            this.Hide();
            manageDepartment.ShowDialog();
            this.Close();
        }
    }
}