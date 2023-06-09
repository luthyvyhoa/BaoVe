﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XeRaVaoCong
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WMSDBEntities : DbContext
    {
        public WMSDBEntities()
            : base("name=WMSDBEntities")
        {
            this.Database.Connection.ConnectionString = "data source=10.0.41.11;initial catalog=WMSDB;user id=WMSUser;password=WMSU!@#2022;MultipleActiveResultSets=True;App=EntityFramework";
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<UserAccounts> UserAccounts { get; set; }
        public virtual DbSet<UserApplicationActivities> UserApplicationActivities { get; set; }
        public virtual DbSet<Gates> Gates { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }
    
        public virtual ObjectResult<STUserAccountLogin_Result> STUserAccountLogin(string userName, string password)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STUserAccountLogin_Result>("STUserAccountLogin", userNameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<STGate_TruckInOut_Result> STGate_TruckInOut(Nullable<int> storeID, Nullable<int> gate, Nullable<byte> flag, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate)
        {
            var storeIDParameter = storeID.HasValue ?
                new ObjectParameter("StoreID", storeID) :
                new ObjectParameter("StoreID", typeof(int));
    
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(int));
    
            var flagParameter = flag.HasValue ?
                new ObjectParameter("Flag", flag) :
                new ObjectParameter("Flag", typeof(byte));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var todateParameter = todate.HasValue ?
                new ObjectParameter("Todate", todate) :
                new ObjectParameter("Todate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_TruckInOut_Result>("STGate_TruckInOut", storeIDParameter, gateParameter, flagParameter, fromDateParameter, todateParameter);
        }
    
        public virtual ObjectResult<STGate_TruckInOutRecent_Result> STGate_TruckInOutRecent(Nullable<byte> gate)
        {
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_TruckInOutRecent_Result>("STGate_TruckInOutRecent", gateParameter);
        }
    
        public virtual ObjectResult<STGate_ContainerCheckings_Result> STGate_ContainerCheckings(string orderNumber)
        {
            var orderNumberParameter = orderNumber != null ?
                new ObjectParameter("OrderNumber", orderNumber) :
                new ObjectParameter("OrderNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContainerCheckings_Result>("STGate_ContainerCheckings", orderNumberParameter);
        }
    
        public virtual ObjectResult<STGate_ContainerCheckingsByContInOutID_Result> STGate_ContainerCheckingsByContInOutID(Nullable<int> gate_ContInOutID)
        {
            var gate_ContInOutIDParameter = gate_ContInOutID.HasValue ?
                new ObjectParameter("Gate_ContInOutID", gate_ContInOutID) :
                new ObjectParameter("Gate_ContInOutID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContainerCheckingsByContInOutID_Result>("STGate_ContainerCheckingsByContInOutID", gate_ContInOutIDParameter);
        }
    
        public virtual ObjectResult<STGate_ContInOut_Result> STGate_ContInOut(Nullable<int> storeID, Nullable<int> gate, Nullable<byte> flag, Nullable<System.DateTime> reportDate)
        {
            var storeIDParameter = storeID.HasValue ?
                new ObjectParameter("StoreID", storeID) :
                new ObjectParameter("StoreID", typeof(int));
    
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(int));
    
            var flagParameter = flag.HasValue ?
                new ObjectParameter("Flag", flag) :
                new ObjectParameter("Flag", typeof(byte));
    
            var reportDateParameter = reportDate.HasValue ?
                new ObjectParameter("ReportDate", reportDate) :
                new ObjectParameter("ReportDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContInOut_Result>("STGate_ContInOut", storeIDParameter, gateParameter, flagParameter, reportDateParameter);
        }
    
        public virtual ObjectResult<STGate_ContInOutByCustomer_Result> STGate_ContInOutByCustomer(Nullable<int> customerID, Nullable<int> varStoreID)
        {
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var varStoreIDParameter = varStoreID.HasValue ?
                new ObjectParameter("varStoreID", varStoreID) :
                new ObjectParameter("varStoreID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContInOutByCustomer_Result>("STGate_ContInOutByCustomer", customerIDParameter, varStoreIDParameter);
        }
    
        public virtual ObjectResult<STGate_ContInOutByDate_Result> STGate_ContInOutByDate(Nullable<System.DateTime> reportDate, Nullable<int> gate, Nullable<int> varStoreID)
        {
            var reportDateParameter = reportDate.HasValue ?
                new ObjectParameter("ReportDate", reportDate) :
                new ObjectParameter("ReportDate", typeof(System.DateTime));
    
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(int));
    
            var varStoreIDParameter = varStoreID.HasValue ?
                new ObjectParameter("varStoreID", varStoreID) :
                new ObjectParameter("varStoreID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContInOutByDate_Result>("STGate_ContInOutByDate", reportDateParameter, gateParameter, varStoreIDParameter);
        }
    
        public virtual int STGate_ContInOutEdit(Nullable<int> varContInOutID, string varCustomerName, string varContainerNum, string varReason, string varTruckIn, string varContainerType)
        {
            var varContInOutIDParameter = varContInOutID.HasValue ?
                new ObjectParameter("varContInOutID", varContInOutID) :
                new ObjectParameter("varContInOutID", typeof(int));
    
            var varCustomerNameParameter = varCustomerName != null ?
                new ObjectParameter("varCustomerName", varCustomerName) :
                new ObjectParameter("varCustomerName", typeof(string));
    
            var varContainerNumParameter = varContainerNum != null ?
                new ObjectParameter("varContainerNum", varContainerNum) :
                new ObjectParameter("varContainerNum", typeof(string));
    
            var varReasonParameter = varReason != null ?
                new ObjectParameter("varReason", varReason) :
                new ObjectParameter("varReason", typeof(string));
    
            var varTruckInParameter = varTruckIn != null ?
                new ObjectParameter("varTruckIn", varTruckIn) :
                new ObjectParameter("varTruckIn", typeof(string));
    
            var varContainerTypeParameter = varContainerType != null ?
                new ObjectParameter("varContainerType", varContainerType) :
                new ObjectParameter("varContainerType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STGate_ContInOutEdit", varContInOutIDParameter, varCustomerNameParameter, varContainerNumParameter, varReasonParameter, varTruckInParameter, varContainerTypeParameter);
        }
    
        public virtual ObjectResult<STGate_ContInOutRecent_Result> STGate_ContInOutRecent(Nullable<int> gate, Nullable<int> varStoreID)
        {
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(int));
    
            var varStoreIDParameter = varStoreID.HasValue ?
                new ObjectParameter("varStoreID", varStoreID) :
                new ObjectParameter("varStoreID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContInOutRecent_Result>("STGate_ContInOutRecent", gateParameter, varStoreIDParameter);
        }
    
        public virtual ObjectResult<STGate_ContInOutRemain_Result> STGate_ContInOutRemain(Nullable<byte> gate, Nullable<int> varStoreID)
        {
            var gateParameter = gate.HasValue ?
                new ObjectParameter("Gate", gate) :
                new ObjectParameter("Gate", typeof(byte));
    
            var varStoreIDParameter = varStoreID.HasValue ?
                new ObjectParameter("varStoreID", varStoreID) :
                new ObjectParameter("varStoreID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STGate_ContInOutRemain_Result>("STGate_ContInOutRemain", gateParameter, varStoreIDParameter);
        }
    
        public virtual int STGate_ContInOutUpdate(Nullable<int> varContInOutID)
        {
            var varContInOutIDParameter = varContInOutID.HasValue ?
                new ObjectParameter("varContInOutID", varContInOutID) :
                new ObjectParameter("varContInOutID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STGate_ContInOutUpdate", varContInOutIDParameter);
        }
    
        public virtual int STGate_ContInOutUpdate_1(Nullable<int> varContInOutID)
        {
            var varContInOutIDParameter = varContInOutID.HasValue ?
                new ObjectParameter("varContInOutID", varContInOutID) :
                new ObjectParameter("varContInOutID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STGate_ContInOutUpdate_1", varContInOutIDParameter);
        }
    }
}
