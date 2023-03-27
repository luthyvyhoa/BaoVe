using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XeRaVaoCong
{
    public partial class frmAddNewXeContainer : DevExpress.XtraEditors.XtraForm
    {
        public frmAddNewXeContainer()
        {
            InitializeComponent();

            var dataCong = DataProcess<Gates>.Select(g => g.StoreID == AppSetting.CurrentUser.StoreID);
            this.lkGateCont.Properties.DataSource = dataCong;
            this.lkGateCont.Properties.DisplayMember = "GateVietnam";
            this.lkGateCont.Properties.ValueMember = "Gate";
        }

        private void frmAddNewXeContainer_Load(object sender, EventArgs e)
        {
            lkeMaKH.Properties.DataSource = DataProcess<Customers>.Select(c => c.StoreID == AppSetting.CurrentUser.StoreID && c.CustomerDiscontinued == false).ToList();
            lkeMaKH.Properties.DisplayMember = "CustomerNumber";
            lkeMaKH.Properties.ValueMember = "CustomerID";

            List<DataLookup> LstLyDo = new List<DataLookup> { new DataLookup("Nhap"), new DataLookup("Xuat"), new DataLookup("Khong X-N") };
            lkeLyDo.Properties.DataSource = LstLyDo;
            lkeLyDo.Properties.DisplayMember = "name";
            lkeLyDo.Properties.ValueMember = "name";

            List<DataLookup> LstLoaiCount = new List<DataLookup> { new DataLookup("20f"), new DataLookup("40f"), new DataLookup("N20f"), new DataLookup("N40f") };
            lkeLoaiCont.Properties.DataSource = LstLoaiCount;
            lkeLoaiCont.Properties.DisplayMember = "name";
            lkeLoaiCont.Properties.ValueMember = "name";
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(lkeLyDo.EditValue==null)
            {
                MessageBox.Show("Phải nhập lý do Nhap/Xuat/Khong X-N)!", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lkeLyDo.Focus();
                lkeLyDo.ShowPopup();
                return;
            }

            if(lkeLyDo.EditValue=="Nhap"|| lkeLyDo.EditValue == "Khong X-N")
            {
                    if(lkeMaKH.EditValue==null)
                    {
                    int result = DataProcess<Customers>.ExecuteNoQuery("INSERT INTO Gate_ContInOut (CustomerName,ContainerNum,ContainerType,TruckIn,Reason,TimeIn,StartTime,Gate,DriverMobilePhone, StoreID) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                    this.txtTenKH.Text, txtSoCont.Text, lkeLoaiCont.EditValue, txtDauKeo.Text, lkeLyDo.EditValue, DateTime.Now,DateTime.Now, lkGateCont.EditValue,txtSDT.Text, AppSetting.CurrentUser.StoreID);
                    this.Close();

                      }
                    else
                    {
                    int result = DataProcess<Customers>.ExecuteNoQuery("INSERT INTO Gate_ContInOut (CustomerID,CustomerName,ContainerNum,ContainerType,TruckIn,Reason,TimeIn,StartTime,Gate,DriverMobilePhone, StoreID) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                   lkeMaKH.EditValue, this.txtTenKH.Text, txtSoCont.Text, lkeLoaiCont.EditValue, txtDauKeo.Text, lkeLyDo.EditValue, DateTime.Now, DateTime.Now, lkGateCont.EditValue, txtSDT.Text, AppSetting.CurrentUser.StoreID);
                    this.Close();

                    }
            }
            else
            {
                if (lkeMaKH.EditValue == null)
                {
                    int result = DataProcess<Customers>.ExecuteNoQuery("INSERT INTO Gate_ContInOut (CustomerName,ContainerNum,ContainerType,TruckIn,Reason,TimeIn,Gate,DriverMobilePhone, StoreID) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                    this.txtTenKH.Text, txtSoCont.Text, lkeLoaiCont.EditValue, txtDauKeo.Text, lkeLyDo.EditValue, DateTime.Now, lkGateCont.EditValue, txtSDT.Text, AppSetting.CurrentUser.StoreID);
                    this.Close();

                } 
                else
                {
                    int result = DataProcess<Customers>.ExecuteNoQuery("INSERT INTO Gate_ContInOut (CustomerID,CustomerName,ContainerNum,ContainerType,TruckIn,Reason,TimeIn,Gate,DriverMobilePhone, StoreID) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                   lkeMaKH.EditValue, this.txtTenKH.Text, txtSoCont.Text, lkeLoaiCont.EditValue, txtDauKeo.Text, lkeLyDo.EditValue, DateTime.Now, lkGateCont.EditValue, txtSDT.Text, AppSetting.CurrentUser.StoreID);
                    this.Close();

                }
            }
            }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lkeMaKH_EditValueChanged(object sender, EventArgs e)
        {
            if (lkeMaKH.ItemIndex < 0)
            {
                txtTenKH.Text = string.Empty;
                return;
            }
            string customerName = lkeMaKH.Properties.GetDataSourceValue("CustomerName", lkeMaKH.ItemIndex).ToString();
            txtTenKH.Text = customerName;
        }
    }

 


      
    
   
}

public class DataLookup
{
    public DataLookup(string name)
    {
        this.name = name;
    }
    public string name { get; set; }

}