using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace XeRaVaoCong
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {

        public frmMain()
        {
            InitializeComponent();

            // Init data
            this.InitData();

            // Get version
            this.Text = "Xe Ra Vào Cổng - " + Application.ProductVersion;
        }
        private int typeFilter = 0;
        private void InitData()
        {
            // Load gate

            var dataCong = DataProcess<Gates>.Select(g => g.StoreID == AppSetting.CurrentUser.StoreID);
            this.lkeGates.Properties.DataSource = dataCong;
            this.lkeGates.Properties.DisplayMember = "GateVietnam";
            this.lkeGates.Properties.ValueMember = "Gate";


            this.lkGateCont.Properties.DataSource = dataCong;
            this.lkGateCont.Properties.DisplayMember = "GateVietnam";
            this.lkGateCont.Properties.ValueMember = "Gate";

            // set default date
            this.dTheoNgay.DateTime = DateTime.Now;
            this.lkeGates.EditValue = AppSetting.CurrentUser.Gate;
            this.lkGateCont.EditValue = AppSetting.CurrentUser.Gate;

            this.lblStoreName.Text = AppSetting.Stores.StoreVietnam;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmAddNewXeRaVao frm = new frmAddNewXeRaVao();
            frm.ShowDialog();
            this.radioGroup1.SelectedIndex = 0;
            this.radioGroup2.SelectedIndex = 0;
            this.LoadData(this.radioGroup1.SelectedIndex, this.radioGroup2.SelectedIndex, this.lkeGates.EditValue);
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexFilter = this.radioGroup1.SelectedIndex;
            if (indexFilter < 0) return;
            if (indexFilter == 0 || indexFilter == 2 || indexFilter == 3)
            {
                this.LoadData(this.radioGroup1.SelectedIndex, -1, null);
            }
            else if (indexFilter == 1)
            {
                this.typeFilter = 2;
                this.lkeGates.Focus();
                this.lkeGates.ShowPopup();
            }
            this.radioGroup2.SelectedIndex = -1;
            this.lkeGates.EditValue = null;
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexFilter = this.radioGroup2.SelectedIndex;
            if (indexFilter < 0) return;
            if (indexFilter == 0)
            {
                this.LoadData(-1, this.radioGroup2.SelectedIndex, null);
            }
            else if (indexFilter == 1)
            {
                this.typeFilter = 1;
                this.lkeGates.Focus();
                this.lkeGates.ShowPopup();
            }
            this.radioGroup1.SelectedIndex = -1;
            this.lkeGates.EditValue = null;
        }

        private void lkeGates_EditValueChanged(object sender, EventArgs e)
        {
            if (this.lkeGates.EditValue == null) return;
            this.LoadData(-1, -1, this.lkeGates.EditValue);
            if (this.typeFilter == 1)
            {
                this.radioGroup1.SelectedIndex = -1;
            }
            else if (this.typeFilter == 2)
            {
                this.radioGroup2.SelectedIndex = -1;
            }
        }

        private void dTheoNgay_EditValueChanged(object sender, EventArgs e)
        {
            this.LoadData(2, -1, null);
            this.lkeGates.EditValue = null;
            this.radioGroup1.SelectedIndex = 2;
            this.radioGroup2.SelectedIndex = -1;
        }

        public void LoadData(int indexFilterRadioGroup1, int indexFilterRadioGroup2, object gate)
        {
            try
            {
                IEnumerable<STGate_TruckInOut_Result> dataSource = null;
                if (gate != null)
                {
                    if (this.typeFilter == 1)
                    {
                        dataSource = DataProcess<STGate_TruckInOut_Result>.Executing("STGate_TruckInOut @Gate={0},@Flag=1", this.lkeGates.EditValue);
                    }
                    else if (this.typeFilter == 2)
                    {
                        dataSource = DataProcess<STGate_TruckInOut_Result>.Executing("STGate_TruckInOutRecent @Gate={0}", this.lkeGates.EditValue);
                    }
                }
                else if (indexFilterRadioGroup1 == 0 || indexFilterRadioGroup2 == 0)
                {
                    dataSource = DataProcess<STGate_TruckInOut_Result>.Executing("STGate_TruckInOut @StoreID={0},@Gate={1},@Flag=1",
                        AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate);
                }
                else if (indexFilterRadioGroup1 == 2)
                {
                    dataSource = DataProcess<STGate_TruckInOut_Result>.Executing("STGate_TruckInOut @StoreID={0},@Gate={1},@Flag=3,@FromDate={2}",
                        AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate, this.dTheoNgay.DateTime);
                }
                else if (indexFilterRadioGroup1 == 3)
                {
                    dataSource = DataProcess<STGate_TruckInOut_Result>.Executing("STGate_TruckInOut @StoreID={0},@Gate={1},@Flag=4,@FromDate={2}",
                        AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate, this.dTheoNgay.DateTime);
                }
                this.grdTruckInout.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error is unexpected");
            }
        }

        private void grvTruckInout_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.CellValue != null && e.Column.FieldName.Equals("TruckReason"))
            {
                if (e.CellValue.Equals("Nhap")) e.Appearance.BackColor = Color.DarkOrange;
                else e.Appearance.BackColor = Color.DeepSkyBlue;
            }
        }

        private void rpi_btn_DeleteTruck_Click(object sender, EventArgs e)
        {
            string userOut = Convert.ToString(this.grvTruckInout.GetFocusedRowCellValue("UserOut"));
            if (string.IsNullOrEmpty(userOut) == false && userOut != "Canceled")
            {
                MessageBox.Show("Không thể xóa xe này, kế toán đã xác nhận !", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn muốn xóa xe này ?", "TWMS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm.Equals(DialogResult.No)) return;
            int truckInoutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));
            DataProcess<STGate_TruckInOut_Result>.ExecuteNoQuery("STDelete_Cont_Truck_InOut_Insert @ContInOutID={0},@IPAddressDeleted={1},@Flag={2}",
               truckInoutID, AppSetting.CurrentUser.LoginName, 1);
        }

        private void grvTruckInout_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0) return;
            int truckInoutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));
            switch (e.Column.FieldName)
            {
                case "CustomerName":
                case "TimeOut":
                    DataProcess<STGate_TruckInOut_Result>.ExecuteNoQuery($"Update Gate_TruckInOut SET { e.Column.FieldName}=N'" + e.Value + "' WHERE truckInoutID =" + truckInoutID);
                    break;
                case "DriverName":
                    DataProcess<STGate_TruckInOut_Result>.ExecuteNoQuery($"Update Gate_TruckInOut SET { e.Column.FieldName}=N'" + e.Value + "' WHERE truckInoutID =" + truckInoutID);
                    break;
                default:
                    break;
            }
        }

        private void rpi_CamDien_EditValueChanged(object sender, EventArgs e)
        {
            int truckInoutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));

            DateTime GioVao = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("TimeIn"));
            DateTime GioRa = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("TimeOut"));

            DateTime editcamdien = (sender as DevExpress.XtraEditors.DateEdit).DateTime;

            if (editcamdien < GioVao)
            {
                this.grvTruckInout.SetFocusedRowCellValue("StartTime", null);
                MessageBox.Show("Ban da nhap gio khong hop le. Gio Cam dien phai >= Gio vao!");
                return;
            }
            if (GioRa.Year != 1)
            {
                if (editcamdien < GioRa)
                {
                    this.grvTruckInout.SetFocusedRowCellValue("StartTime", null);
                    MessageBox.Show("Ban da nhap gio khong hop le. Gio Cam dien phai < Gio Ra!");
                    return;
                }
            }

            int result = DataProcess<object>.ExecuteNoQuery("Update Gate_TruckInOut SET StartTime = '" + editcamdien + "' WHERE TruckInOutID = " + truckInoutID);
            if (result < 1)
            {
                MessageBox.Show("Lỗi");

            }

            //            DispatchingProducts DODetail = (DispatchingProducts)gridViewOrderDetail.GetFocusedRow();

        }

        private void rpi_RutDien_EditValueChanged(object sender, EventArgs e)
        {
            int truckInoutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));

            DateTime GioVao = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("TimeIn"));
            DateTime GioRa = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("TimeOut"));

            DateTime editrutdien = (sender as DevExpress.XtraEditors.DateEdit).DateTime;

            if (editrutdien < GioRa)
            {
                this.grvTruckInout.SetFocusedRowCellValue("EndTime", null);
                MessageBox.Show("Ban da nhap gio khong hop le. Gio Rut dien phai <= Gio Ra!");
                return;
            }
            if (GioVao.Year != 1)
            {
                if (editrutdien < GioVao)
                {
                    this.grvTruckInout.SetFocusedRowCellValue("EndTime", null);
                    MessageBox.Show("Ban da nhap gio khong hop le. Gio Rut dien phai > Gio Vao!");
                    return;

                }

            }

            int result = DataProcess<object>.ExecuteNoQuery("Update Gate_TruckInOut SET EndTime = '" + editrutdien + "' WHERE TruckInOutID = " + truckInoutID);
            if (result < 1)
            {
                MessageBox.Show("Lỗi");

            }
        }

        private void rpi_LinkQty_Click(object sender, EventArgs e)
        {
            int truckInoutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));

            //string link = "http://192.168.104.29:809/VehicleDetails.aspx?VehicleID=" + truckInoutID + "&VehicleType=TR";
            //ProcessStartInfo sInfo = new ProcessStartInfo(link);
            //Process.Start(sInfo);


        }

        private void rpi_Delete_Container_Click(object sender, EventArgs e)
        {
            string qty = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("ProductQty"));
            string timeout = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("TimeOut"));
            bool UserCheckOut = Convert.ToBoolean(this.grvConInOut.GetFocusedRowCellValue("UserCheckOut"));

            if (qty != "" && timeout != "")
            {
                MessageBox.Show("Không thể xóa xe này, kế toán đã xác nhận !", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (UserCheckOut)
            {
                MessageBox.Show("Không thể xóa xe này, kế toán đã cho ra kho!", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn muốn xóa xe này ?", "TWMS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm.Equals(DialogResult.No)) return;
            int ContInOutID = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("ContInOutID"));
            int result = DataProcess<STGate_TruckInOut_Result>.ExecuteNoQuery("EXEC STDelete_Cont_Truck_InOut_Insert @ContInOutID={0},@IPAddressDeleted={1}",
               ContInOutID, AppSetting.CurrentUser.LoginName);
            lkGateCont_EditValueChanged(sender, e);
        }

        private void btnThemCont_Click(object sender, EventArgs e)
        {

            frmAddNewXeContainer frm = new frmAddNewXeContainer();
            frm.FormClosed += lkGateCont_EditValueChanged;
            frm.ShowDialog();

        }

        private void lkGateCont_EditValueChanged(object sender, EventArgs e)
        {
            IEnumerable<STGate_ContInOut_Result> dataSourceCont = null;
            dataSourceCont = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=1", AppSetting.CurrentUser.StoreID, lkGateCont.EditValue);
            this.grdConInOut.DataSource = dataSourceCont;
        }

        private void grvConInOut_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int ContInOutID = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("ContInOutID"));

            if (grvConInOut.FocusedColumn.Name == "colDock")
            {
                int result = DataProcess<object>.ExecuteNoQuery("UPDATE Gate_ContInOut SET DockNumber='" + grvConInOut.GetFocusedRowCellValue("DockNumber") + "' WHERE ContInOutID = " + ContInOutID);
                if (result < 1)
                {
                    MessageBox.Show("Lỗi");

                }
            }
            if (grvConInOut.FocusedColumn.Name == "ColTempOut")
            {
                int result = DataProcess<object>.ExecuteNoQuery("UPDATE Gate_ContInOut SET TempOut='" + grvConInOut.GetFocusedRowCellValue("TempOut") + "' WHERE ContInOutID = " + ContInOutID);
                if (result < 1)
                {
                    MessageBox.Show("Lỗi");

                }
            }

        }

        private void rpi_txtdock_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void rpi_Link_Click(object sender, EventArgs e)
        {
            int ContInOutID = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("ContInOutID"));

            //string link = "http://192.168.104.29:809/VehicleDetails.aspx?VehicleID=" + ContInOutID + "&VehicleType=CO";
            //ProcessStartInfo sInfo = new ProcessStartInfo(link);
            //Process.Start(sInfo);
        }

        private void rpi_GioRa_EditValueChanged(object sender, EventArgs e)
        {
            DateTime RpiGioRa = (sender as DevExpress.XtraEditors.DateEdit).DateTime;

            int ContInOutID = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("ContInOutID"));
            int result = 0;
            //if (grvConInOut.GetFocusedRowCellValue("Gate").ToString() != AppSetting.CurrentUser.Gate.ToString())
            //{
            //    MessageBox.Show("Container vào cổng số "+ grvConInOut.GetFocusedRowCellValue("Gate"), "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    this.grvConInOut.SetFocusedRowCellValue("TimeOut", null);
            //    return;

            //}
            if (grvConInOut.GetFocusedRowCellValue("TempOut") == null || string.IsNullOrEmpty(grvConInOut.GetFocusedRowCellValue("TempOut").ToString()))
            {
                MessageBox.Show("Container nay chua ghi nhiet do, khong cho phep ra cong !", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.grvConInOut.SetFocusedRowCellValue("TimeOut", null);
                return;
            }
            else
            {

                DateTime StartTime = Convert.ToDateTime(this.grvConInOut.GetFocusedRowCellValue("StartTime"));
                DateTime EndTime = Convert.ToDateTime(this.grvConInOut.GetFocusedRowCellValue("EndTime"));
                DateTime TimeIn = Convert.ToDateTime(this.grvConInOut.GetFocusedRowCellValue("TimeIn"));

                string ContainerType = this.grvConInOut.GetFocusedRowCellValue("ContainerType").ToString();


                string type = grvConInOut.GetFocusedRowCellValue("Reason").ToString();
                if (type == "Xuat")
                {
                    if (ContainerType != "N20f" && ContainerType != "N40f")
                    {
                        result = DataProcess<object>.ExecuteNoQuery("EXEC STGate_ContInOutUpdate @varContInOutID=" + ContInOutID);
                        if (result < 1)
                        {
                            MessageBox.Show("Lỗi");

                        }
                    }
                    else
                    {
                        result = DataProcess<object>.ExecuteNoQuery("UPDATE Gate_ContInOut SET TimeOut = {0}, EndTime = '',CheckOut = 1 WHERE ContInOutID ={1}", DateTime.Now, ContInOutID);
                        if (result < 1)
                        {
                            MessageBox.Show("Lỗi");

                        }

                    }

                }
                else if (type == "Khong X-N")
                {
                    result = DataProcess<object>.ExecuteNoQuery("EXEC STGate_ContInOutUpdate @varContInOutID=" + ContInOutID);
                    if (result < 1)
                    {
                        MessageBox.Show("Lỗi");

                    }
                }
                else
                {
                    if (ContainerType != "N20f" && ContainerType != "N40f")
                    {
                        result = DataProcess<object>.ExecuteNoQuery("UPDATE Gate_ContInOut SET TimeOut ={0}, StartTime = {1}, CheckOut = 1 WHERE ContInOutID = {2}", DateTime.Now, TimeIn, ContInOutID);
                        if (result < 1)
                        {
                            MessageBox.Show("Lỗi");

                        }
                    }
                    else
                    {
                        result = DataProcess<object>.ExecuteNoQuery("UPDATE Gate_ContInOut SET TimeOut = {0},StartTime = '', CheckOut = 1 WHERE ContInOutID = {1}", DateTime.Now, ContInOutID);
                        if (result < 1)
                        {
                            MessageBox.Show("Lỗi");

                        }

                    }

                }


            }


        }

        private void rpi_edit_cont_Click(object sender, EventArgs e)
        {
            DateTime TimeIn = Convert.ToDateTime(this.grvConInOut.GetFocusedRowCellValue("TimeIn"));
            DateTime TimeOut = Convert.ToDateTime(this.grvConInOut.GetFocusedRowCellValue("TimeOut"));

            String UserOut = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("UserOut"));
            String CustomerID = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("CustomerID"));
            String LyDo = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("Reason"));
            String LoaiCont = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("ContainerType"));
            String SoLuong = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("ContainerNum"));
            String DauKeo = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("TruckIn"));
            int MaKH = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("CustomerID"));
            String TenKH = Convert.ToString(this.grvConInOut.GetFocusedRowCellValue("CustomerName"));
            int ContInOutID = Convert.ToInt32(this.grvConInOut.GetFocusedRowCellValue("ContInOutID"));
            bool UserCheckOut = Convert.ToBoolean(this.grvConInOut.GetFocusedRowCellValue("UserCheckOut"));

            if (TimeOut.Year != 1)
            {
                MessageBox.Show("Không thể sửa container đã ra khỏi cổng!", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.grvConInOut.SetFocusedRowCellValue("TimeOut", null);
                return;
            }
            else if (String.IsNullOrEmpty(UserOut) || UserOut.Equals(""))
            {
                frmEditXeContainer frm = new frmEditXeContainer(LyDo, LoaiCont, DauKeo, SoLuong, MaKH, TenKH, TimeIn, 1, ContInOutID, UserCheckOut);
                frm.FormClosed += lkGateCont_EditValueChanged;
                frm.Show();
            }
            else if (String.IsNullOrEmpty(CustomerID))
            {
                frmEditXeContainer frm = new frmEditXeContainer(LyDo, LoaiCont, DauKeo, SoLuong, MaKH, TenKH, TimeIn, 1, ContInOutID, UserCheckOut);
                frm.FormClosed += lkGateCont_EditValueChanged;
                frm.Show();
            }
            else
            {
                frmEditXeContainer frm = new frmEditXeContainer(LyDo, LoaiCont, DauKeo, SoLuong, MaKH, TenKH, TimeIn, 1, ContInOutID, UserCheckOut);
                frm.FormClosed += lkGateCont_EditValueChanged;
                frm.Show();
            }

        }

        private void rdKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexFilter = this.rdKhoCont.SelectedIndex + 1;
            IEnumerable<STGate_ContInOut_Result> dataSource = null;
            switch (indexFilter)
            {
                //Remain
                case 1:
                    dataSource = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=1", AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate);
                    this.grdConInOut.DataSource = dataSource;

                    break;

                //Recent
                case 2:
                    this.lkGateCont.Focus();
                    this.lkGateCont.ShowPopup();

                    break;


                default:
                    break;
            }


        }

        private void rdFillterCont_SelectedIndexChanged(object sender, EventArgs e)
        {

            int indexFilter = this.rdFillterCont.SelectedIndex + 1;
            IEnumerable<STGate_ContInOut_Result> dataSource = null;
            switch (indexFilter)
            {
                //Remain
                case 1:
                    dataSource = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=1", AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate);
                    this.grdConInOut.DataSource = dataSource;

                    break;

                //Recent
                case 2:
                    dataSource = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=2", AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate);
                    this.grdConInOut.DataSource = dataSource;
                    break;
                //by Date
                case 3:
                    dataSource = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=3,@ReportDate={2}",
                        AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate, this.dTheoNgayCont.DateTime);
                    this.grdConInOut.DataSource = dataSource;
                    break;


                default:
                    break;
            }

        }

        private void rpi_btn_TruckEdit_Click(object sender, EventArgs e)
        {
            if (this.grvTruckInout.FocusedRowHandle < 0) return;
            STGate_TruckInOut_Result truckInfo = (STGate_TruckInOut_Result)this.grvTruckInout.GetRow(this.grvTruckInout.FocusedRowHandle);
            frmAddNewXeRaVao frm = new frmAddNewXeRaVao(truckInfo);
            frm.ShowDialog();
            this.LoadData(this.radioGroup1.SelectedIndex, this.radioGroup2.SelectedIndex, this.lkeGates.EditValue);
        }

        private void dTheoNgayCont_EditValueChanged(object sender, EventArgs e)
        {
            IEnumerable<STGate_ContInOut_Result> dataSource = DataProcess<STGate_ContInOut_Result>.Executing("EXEC STGate_ContInOut @StoreID={0},@Gate={1},@Flag=3,@ReportDate={2}",
                                AppSetting.CurrentUser.StoreID, AppSetting.CurrentUser.Gate, this.dTheoNgayCont.DateTime);
            this.grdConInOut.DataSource = dataSource;
        }

        private void rpi_GioRa2_EditValueChanged(object sender, EventArgs e)
        {
            var outTimeValueChange = (DevExpress.XtraEditors.Controls.ChangingEventArgs)e;
            DateTime RpiGioRa = (sender as DevExpress.XtraEditors.DateEdit).DateTime;

            int TruckInOutID = Convert.ToInt32(this.grvTruckInout.GetFocusedRowCellValue("TruckInOutID"));
            int result = 0;


            DateTime StartTime = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("StartTime"));
            DateTime EndTime = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("EndTime"));
            DateTime TimeIn = Convert.ToDateTime(this.grvTruckInout.GetFocusedRowCellValue("TimeIn"));
            DateTime TimeOut = Convert.ToDateTime(outTimeValueChange.NewValue);
            if (TimeOut < DateTime.Now.AddMinutes(-5) || TimeOut < TimeIn)
            {
                MessageBox.Show("Giờ ra không hợp lệ!", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.grvTruckInout.SetFocusedRowCellValue("TimeOut", null);
                return;
            }


            string type =Convert.ToString( grvTruckInout.GetFocusedRowCellValue("TruckReason"));
            if (type == "Nhan vien")
            {
                result = DataProcess<object>.ExecuteNoQuery("Update Gate_TruckInOut Set CheckOut = 1, TimeOut = GETDATE() Where TruckInOutID = " + TruckInOutID);
                this.radioGroup1.SelectedIndex = 0;
                return;
            }

            if (this.grvTruckInout.GetFocusedRowCellValue("ProductQty") == null || string.IsNullOrEmpty(Convert.ToString(this.grvTruckInout.GetFocusedRowCellValue("ProductQty"))))
            {
                MessageBox.Show("Xe này chưa được phép ra cổng !", "TWMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.grvTruckInout.SetFocusedRowCellValue("TimeOut", null);
                return;
            }

            result = DataProcess<object>.ExecuteNoQuery("Update Gate_TruckInOut Set CheckOut = 1, TimeOut = GETDATE() Where TruckInOutID = " + TruckInOutID);
            this.radioGroup1.SelectedIndex = 0;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.radioGroup1.SelectedIndex = 0;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (e.Page.Name)
            {
                case "xtraTabPage2":
                    this.rdFillterCont.SelectedIndex = 0;
                    break;
                default:
                    this.radioGroup1.SelectedIndex = 0;
                    break;
            }
        }
    }
}
