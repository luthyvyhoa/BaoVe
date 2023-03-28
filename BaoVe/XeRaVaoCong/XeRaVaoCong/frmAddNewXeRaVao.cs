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
    public partial class frmAddNewXeRaVao : DevExpress.XtraEditors.XtraForm
    {
        private STGate_TruckInOut_Result truckInfo = null;
        public frmAddNewXeRaVao(STGate_TruckInOut_Result truckInfo = null)
        {
            InitializeComponent();
            lkeMaKH.Properties.DataSource = DataProcess<Customers>.Select(c => c.StoreID == AppSetting.CurrentUser.StoreID && c.CustomerDiscontinued == false).ToList();
            lkeMaKH.Properties.DisplayMember = "CustomerNumber";
            lkeMaKH.Properties.ValueMember = "CustomerID";

            var dataCong = DataProcess<Gates>.Select(g => g.StoreID == AppSetting.CurrentUser.StoreID);
            this.lkGateCont.Properties.DataSource = dataCong;
            this.lkGateCont.Properties.DisplayMember = "GateVietnam";
            this.lkGateCont.Properties.ValueMember = "Gate";

            List<DataLookup> LstLyDo = new List<DataLookup> { new DataLookup("Nhap"), new DataLookup("Xuat"), new DataLookup("Khong X-N") };
            lkeLyDo.Properties.DataSource = LstLyDo;
            lkeLyDo.Properties.DisplayMember = "name";
            lkeLyDo.Properties.ValueMember = "name";

            if (truckInfo ==null)
            {
                // Add New
                this.radioGroup1.SelectedIndex = 1;
                this.deGioVao.ReadOnly = false;
                this.lkeLyDo.ReadOnly = false;
            }
            else
            {
                // Edit
                this.truckInfo = truckInfo;
                this.LoadDataUpdate();
            }

        }

        private void LoadDataUpdate()
        {
            radioGroup1.ReadOnly = true;
            txtSoXe.ReadOnly = true;
            txtTaiXe.ReadOnly = true;
            txtSDT.ReadOnly = true;
            lkeMaKH.ReadOnly = true;
            txtTaiTrong.ReadOnly = true;

            int index = 0;
            if (truckInfo.TruckType == "Xe máy") index = 1;
            radioGroup1.EditValue = index;
            txtSoXe.EditValue = this.truckInfo.TruckNum;
            txtTaiXe.EditValue = this.truckInfo.DriverName;
            txtSDT.EditValue = this.truckInfo.DriverMobilePhone;
            lkeMaKH.EditValue = this.truckInfo.CustomerID;
            txtTenKH.EditValue = this.truckInfo.CustomerName;
            txtTaiTrong.EditValue = this.truckInfo.LoadingCapacity;
            lkGateCont.EditValue = this.truckInfo.Gate;
            this.lkeLyDo.EditValue = this.truckInfo.TruckReason;
            this.deGioVao.EditValue = this.truckInfo.TimeIn;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (this.truckInfo == null)
            {
                this.ProcessAddNew();
            }
            else
            {
                this.ProcessUpdate();
            }
        }

        private void ProcessUpdate()
        {
            int result = DataProcess<Customers>.ExecuteNoQuery("Update Gate_TruckInOut Set TruckReason={0},Gate={1},TimeIn={3} Where TruckInOutID={2}",
                      lkeLyDo.EditValue, lkGateCont.EditValue, this.truckInfo.TruckInOutID, this.deGioVao.EditValue);
            this.Close();
        }

        private void ProcessAddNew()
        {
            int taiTrong = 0;
            string loaiXe = "Xe máy";
            if (radioGroup1.SelectedIndex == 0)
            {
                //Oto
                loaiXe = "Ô tô";
                //if (string.IsNullOrEmpty(this.txtTaiTrong.Text))
                //{
                //    MessageBox.Show("Vui lòng nhập số tấn của xe", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
            }

            if (lkeMaKH.EditValue == null)
            {
                int result = DataProcess<Customers>.ExecuteNoQuery("STGate_TruckInOutInsert @CustomerName={0},@DriverName={1},@TruckNum={2}," +
                      "@TruckReason={3},@Gate={4},@TruckType={5},@LoadingCapacity={6},@DriverMobilePhone={7},@StoreID={8}",
                      this.txtTenKH.Text, txtTaiXe.Text, txtSoXe.Text, lkeLyDo.EditValue, lkGateCont.EditValue, loaiXe, taiTrong,
                      txtSDT.Text, AppSetting.CurrentUser.StoreID);
                this.Close();
            }
            else
            {
                int result = DataProcess<Customers>.ExecuteNoQuery("STGate_TruckInOutInsert @CustomerName={0},@DriverName={1},@TruckNum={2}," +
                    "@TruckReason={3},@Gate={4},@TruckType={5},@LoadingCapacity={6},@DriverMobilePhone={7},@StoreID={8},@CustomerID={9}",
                    this.txtTenKH.Text, txtTaiXe.Text, txtSoXe.Text, lkeLyDo.EditValue, lkGateCont.EditValue, loaiXe, taiTrong,
                    txtSDT.Text, AppSetting.CurrentUser.StoreID, this.lkeMaKH.EditValue);
                this.Close();
            }
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