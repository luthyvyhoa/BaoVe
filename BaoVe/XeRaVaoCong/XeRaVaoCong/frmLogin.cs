
using DevExpress.XtraEditors;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace XeRaVaoCong
{
    public partial class frmLogin : XtraForm
    {
        private bool hasLogin = false;
        private readonly string fileName = "LoginName.txt";

        public frmLogin()
        {
            InitializeComponent();
            this.txtName.Focus();
            string machinName = System.Environment.MachineName;
            if (machinName == "XUANTRI")
            {
                this.txtName.Text = "itttp";
                this.txtPassword.Text = "@Ttp1234@";
            }
        }

        /// <summary>
        /// Get status login of current user
        /// <para>True : Login has succeeded</para>
        /// <para>False : Login is fail</para>
        /// </summary>
        public bool HasLogin
        {
            get
            {
                return this.hasLogin;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (dxValidationProvider1.Validate())
            {
                string code = "itttp";
                string pass = "@Ttp1234@";
                //code = txtName.Text.Trim();
                //pass = txtPassword.Text.Trim();

                //Validating user login
                var userLogin = this.LookupUserNameLogin(code, pass);
                if (userLogin != null)
                {
                    // Update login info
                    DataProcess<UserApplicationActivities>.ExecuteNoQuery("Insert into UserApplicationActivities values({0},{1},{2},{3},getdate(),{4},{5},NULL)",
                        userLogin.LoginName, userLogin.EmployeeID,"WMS", Application.ProductVersion, System.Environment.MachineName, GetLocalIPAddress());

                    this.hasLogin = true;
                    AppSetting.CurrentUser = userLogin;
                    AppSetting.Stores= DataProcess<Stores>.Select(s => s.StoreID == userLogin.StoreID).FirstOrDefault();
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show("Login failed", "TPWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txtName.SelectAll();
                    this.txtName.Focus();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Not detected";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private UserAccounts LookupUserNameLogin(string userName, string pass)
        {
            var listUser = DataProcess<STUserAccountLogin_Result>.Executing("STUserAccountLogin @UserName='" + userName + "', @Password='" + pass + "'").FirstOrDefault();
            if (listUser == null) return null;
            UserAccounts userSelected = new UserAccounts();
            userSelected.EmployeeID = listUser.EmployeeID;
            userSelected.Gate = listUser.Gate;
            userSelected.LoginName = listUser.LoginName;
            userSelected.Password = listUser.Password;
            userSelected.StoreID = listUser.StoreID;
            userSelected.WarehouseID = listUser.WarehouseID;
            return userSelected;
        }
    }
}