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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Tên đăng nhập và mật khẩu không được để trống!", "Thông báo", MessageBoxButton.OK);
                    return;
                }

                Authentication account = ProjectPrn212Context.INSTANCE.Authentications.FirstOrDefault(a => a.Username.Equals(username) && a.PassWord.Equals(password) && a.IsDelete == false);
                if (account == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại!", "Thông báo", MessageBoxButton.OK);
                }
                else
                {
                    Employee employee = ProjectPrn212Context.INSTANCE.Employees.FirstOrDefault(e => e.Id == account.EmployeeId);
                    if (employee != null)
                    {
                        Home home = new Home(employee);

                        if (employee.RoleId == 2)
                        {
                            home.userFunc.Visibility = Visibility.Visible;
                        }
                        else if (employee.RoleId == 3)
                        {
                            home.manageFunc.Visibility = Visibility.Visible;
                        }
                        else if (employee.RoleId == 1)
                        {
                            home.adminFunc.Visibility = Visibility.Visible;
                        }

                        this.Hide();
                        home.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không có nhân viên nào sử dụng tài khoản này!", "Thông báo", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng nhập lỗi! {ex.Message}");
            }
        }


        private void ExistButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát app?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
