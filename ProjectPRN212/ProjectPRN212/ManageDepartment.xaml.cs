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
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateDepartment_Click(object sender, RoutedEventArgs e)
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

        private void dgDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbAllEmployee_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbbFilterEmployeeStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbbManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
