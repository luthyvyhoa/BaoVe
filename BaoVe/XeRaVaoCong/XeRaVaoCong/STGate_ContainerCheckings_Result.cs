//------------------------------------------------------------------------------
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
    
    public partial class STGate_ContainerCheckings_Result
    {
        public int ContainerCheckingID { get; set; }
        public string OrderNumber { get; set; }
        public int ContInOutID { get; set; }
        public string TemperatureShow { get; set; }
        public string TemperatureSetup { get; set; }
        public Nullable<bool> Running { get; set; }
        public Nullable<bool> Thawing { get; set; }
        public Nullable<bool> Stop { get; set; }
        public Nullable<bool> Error { get; set; }
        public Nullable<bool> ProductEmpty { get; set; }
        public Nullable<bool> Seal { get; set; }
        public Nullable<bool> Lock { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string DockNumber { get; set; }
        public Nullable<bool> NoOperation { get; set; }
        public Nullable<bool> Electricity { get; set; }
        public string ContainerNum { get; set; }
        public Nullable<System.DateTime> TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string VietnamName { get; set; }
    }
}
