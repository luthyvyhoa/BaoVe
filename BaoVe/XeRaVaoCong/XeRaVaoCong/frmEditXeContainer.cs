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
    public partial class frmEditXeContainer : DevExpress.XtraEditors.XtraForm
    {
        string txtlydo = "";
        string txtLoaiCont = "";
        string SDT = "";
        string DauKeo = "";
        string SoLuong = "";
        int MaKH = 0;
        string TenKH = "";
        int type = 0;
        int ContInOutID = 0;
        DateTime TimeInCont;
        public frmEditXeContainer(string txtLydo,string txtLoaiCont,string DauKeo,string SoLuong, int MaKH,string TenKH,DateTime TimeIn,int type,int ContInOutID, bool UserCheckOut)
        {
            InitializeComponent();
            this.txtlydo = txtLydo;
            this.txtLoaiCont = txtLoaiCont;
            this.DauKeo = DauKeo;
            this.MaKH = MaKH;
            this.TenKH = TenKH;
            this.SoLuong = SoLuong;
            this.type = type;
            this.TimeInCont = TimeIn;
            this.ContInOutID = ContInOutID;
            btnLuu.Enabled = UserCheckOut ? false : true;
        }

        private void frmEditXeContainer_Load(object sender, EventArgs e)
        {
            lkeMaKH.Properties.DataSource = DataProcess<Customers>.Select(c => c.StoreID == AppSetting.CurrentUser.StoreID && c.CustomerDiscontinued == false).ToList();
            lkeMaKH.Properties.DisplayMember = "CustomerNumber";
            lkeMaKH.Properties.ValueMember = "CustomerID";
            lkeMaKH.EditValue = this.MaKH;


            List<DataLookup> LstLyDo = new List<DataLookup> { new DataLookup("Nhap"), new DataLookup("Xuat"), new DataLookup("Khong X-N") };
            lkeLyDo.Properties.DataSource = LstLyDo;
            lkeLyDo.Properties.DisplayMember = "name";
            lkeLyDo.Properties.ValueMember = "name";
            lkeLyDo.Text = this.txtlydo;


            List<DataLookup> LstLoaiCount = new List<DataLookup> { new DataLookup("20f"), new DataLookup("40f"), new DataLookup("N20f"), new DataLookup("N40f") };
            lkeLoaiCont.Properties.DataSource = LstLoaiCount;
            lkeLoaiCont.Properties.DisplayMember = "name";
            lkeLoaiCont.Properties.ValueMember = "name";
            lkeLoaiCont.Text=this.txtLoaiCont;

            txtTenKH.Text = this.TenKH;
            txtSoCont.Text = this.SoLuong;
            txtDauKeo.Text = this.DauKeo;
            dtTimeIn.EditValue = this.TimeInCont;
            if (type==1)
            {
                txtTenKH.ReadOnly = true;
                lkeLyDo.ReadOnly = true;
            }
            if(type == 2)
            {
                txtTenKH.ReadOnly = true;

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (lkeLyDo.EditValue == null)
            {
                MessageBox.Show("Phải nhập lý do Nhap/Xuat/Khong X-N)!", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lkeLyDo.Focus();
                lkeLyDo.ShowPopup();
                return;
            }
            int result2 = DataProcess<Customers>.ExecuteNoQuery("EXEC STGate_ContInOutEdit @varContInOutID={0},@varCustomerName={1},@varContainerNum ={2},@varReason ={3},@varTruckIn ={4},@varContainerType={5}",
                ContInOutID, txtTenKH.Text, txtSoCont.Text, lkeLyDo.EditValue,txtDauKeo.Text,lkeLoaiCont.EditValue);


            if (lkeLyDo.EditValue == "Nhap")
            {
                int result = DataProcess<Customers>.ExecuteNoQuery("UPDATE Gate_ContInOut SET StartTime = {0} WHERE ContInOutID = {1}", dtTimeIn.EditValue, ContInOutID);

            }
            this.Close();

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}