﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialObisis.ObisisServiceReference {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://obisis.erciyes.edu.tr", ConfigurationName="ObisisServiceReference.IObisisMobileService")]
    public interface IObisisMobileService {
        
        // CODEGEN: Parameter 'OgrenciBilgiGetirResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://obisis.erciyes.edu.tr/IObisisMobileService/OgrenciBilgiGetir", ReplyAction="http://obisis.erciyes.edu.tr/IObisisMobileService/OgrenciBilgiGetirResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        SocialObisis.ObisisServiceReference.OgrenciBilgiGetirResponse OgrenciBilgiGetir(SocialObisis.ObisisServiceReference.OgrenciBilgiGetirRequest request);
        
        // CODEGEN: Parameter 'OgrenciDonemGetirResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://obisis.erciyes.edu.tr/IObisisMobileService/OgrenciDonemGetir", ReplyAction="http://obisis.erciyes.edu.tr/IObisisMobileService/OgrenciDonemGetirResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        SocialObisis.ObisisServiceReference.OgrenciDonemGetirResponse OgrenciDonemGetir(SocialObisis.ObisisServiceReference.OgrenciDonemGetirRequest request);
        
        // CODEGEN: Parameter 'GetDataUsingDataContractResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://obisis.erciyes.edu.tr/IObisisMobileService/GetDataUsingDataContract", ReplyAction="http://obisis.erciyes.edu.tr/IObisisMobileService/GetDataUsingDataContractRespons" +
            "e")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        SocialObisis.ObisisServiceReference.GetDataUsingDataContractResponse GetDataUsingDataContract(SocialObisis.ObisisServiceReference.GetDataUsingDataContractRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/ObisisMobileWCF")]
    public partial class OgrenciResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Data.DataSet dataField;
        
        private string messageField;
        
        private int resultCodeField;
        
        private bool resultCodeFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public System.Data.DataSet Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
                this.RaisePropertyChanged("Data");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int ResultCode {
            get {
                return this.resultCodeField;
            }
            set {
                this.resultCodeField = value;
                this.RaisePropertyChanged("ResultCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ResultCodeSpecified {
            get {
                return this.resultCodeFieldSpecified;
            }
            set {
                this.resultCodeFieldSpecified = value;
                this.RaisePropertyChanged("ResultCodeSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/ObisisMobileWCF")]
    public partial class CompositeType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool boolValueField;
        
        private bool boolValueFieldSpecified;
        
        private string stringValueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool BoolValue {
            get {
                return this.boolValueField;
            }
            set {
                this.boolValueField = value;
                this.RaisePropertyChanged("BoolValue");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BoolValueSpecified {
            get {
                return this.boolValueFieldSpecified;
            }
            set {
                this.boolValueFieldSpecified = value;
                this.RaisePropertyChanged("BoolValueSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string StringValue {
            get {
                return this.stringValueField;
            }
            set {
                this.stringValueField = value;
                this.RaisePropertyChanged("StringValue");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="OgrenciBilgiGetir", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class OgrenciBilgiGetirRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ogrenciNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string sifre;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> ogretimYili;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> donem;
        
        public OgrenciBilgiGetirRequest() {
        }
        
        public OgrenciBilgiGetirRequest(string ogrenciNo, string sifre, System.Nullable<int> ogretimYili, System.Nullable<int> donem) {
            this.ogrenciNo = ogrenciNo;
            this.sifre = sifre;
            this.ogretimYili = ogretimYili;
            this.donem = donem;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="OgrenciBilgiGetirResponse", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class OgrenciBilgiGetirResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SocialObisis.ObisisServiceReference.OgrenciResult OgrenciBilgiGetirResult;
        
        public OgrenciBilgiGetirResponse() {
        }
        
        public OgrenciBilgiGetirResponse(SocialObisis.ObisisServiceReference.OgrenciResult OgrenciBilgiGetirResult) {
            this.OgrenciBilgiGetirResult = OgrenciBilgiGetirResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="OgrenciDonemGetir", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class OgrenciDonemGetirRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ogrenciNo;
        
        public OgrenciDonemGetirRequest() {
        }
        
        public OgrenciDonemGetirRequest(string ogrenciNo) {
            this.ogrenciNo = ogrenciNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="OgrenciDonemGetirResponse", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class OgrenciDonemGetirResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SocialObisis.ObisisServiceReference.OgrenciResult OgrenciDonemGetirResult;
        
        public OgrenciDonemGetirResponse() {
        }
        
        public OgrenciDonemGetirResponse(SocialObisis.ObisisServiceReference.OgrenciResult OgrenciDonemGetirResult) {
            this.OgrenciDonemGetirResult = OgrenciDonemGetirResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetDataUsingDataContract", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class GetDataUsingDataContractRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SocialObisis.ObisisServiceReference.CompositeType composite;
        
        public GetDataUsingDataContractRequest() {
        }
        
        public GetDataUsingDataContractRequest(SocialObisis.ObisisServiceReference.CompositeType composite) {
            this.composite = composite;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetDataUsingDataContractResponse", WrapperNamespace="http://obisis.erciyes.edu.tr", IsWrapped=true)]
    public partial class GetDataUsingDataContractResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://obisis.erciyes.edu.tr", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SocialObisis.ObisisServiceReference.CompositeType GetDataUsingDataContractResult;
        
        public GetDataUsingDataContractResponse() {
        }
        
        public GetDataUsingDataContractResponse(SocialObisis.ObisisServiceReference.CompositeType GetDataUsingDataContractResult) {
            this.GetDataUsingDataContractResult = GetDataUsingDataContractResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IObisisMobileServiceChannel : SocialObisis.ObisisServiceReference.IObisisMobileService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ObisisMobileServiceClient : System.ServiceModel.ClientBase<SocialObisis.ObisisServiceReference.IObisisMobileService>, SocialObisis.ObisisServiceReference.IObisisMobileService {
        
        public ObisisMobileServiceClient() {
        }
        
        public ObisisMobileServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ObisisMobileServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ObisisMobileServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ObisisMobileServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SocialObisis.ObisisServiceReference.OgrenciBilgiGetirResponse SocialObisis.ObisisServiceReference.IObisisMobileService.OgrenciBilgiGetir(SocialObisis.ObisisServiceReference.OgrenciBilgiGetirRequest request) {
            return base.Channel.OgrenciBilgiGetir(request);
        }
        
        public SocialObisis.ObisisServiceReference.OgrenciResult OgrenciBilgiGetir(string ogrenciNo, string sifre, System.Nullable<int> ogretimYili, System.Nullable<int> donem) {
            SocialObisis.ObisisServiceReference.OgrenciBilgiGetirRequest inValue = new SocialObisis.ObisisServiceReference.OgrenciBilgiGetirRequest();
            inValue.ogrenciNo = ogrenciNo;
            inValue.sifre = sifre;
            inValue.ogretimYili = ogretimYili;
            inValue.donem = donem;
            SocialObisis.ObisisServiceReference.OgrenciBilgiGetirResponse retVal = ((SocialObisis.ObisisServiceReference.IObisisMobileService)(this)).OgrenciBilgiGetir(inValue);
            return retVal.OgrenciBilgiGetirResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SocialObisis.ObisisServiceReference.OgrenciDonemGetirResponse SocialObisis.ObisisServiceReference.IObisisMobileService.OgrenciDonemGetir(SocialObisis.ObisisServiceReference.OgrenciDonemGetirRequest request) {
            return base.Channel.OgrenciDonemGetir(request);
        }
        
        public SocialObisis.ObisisServiceReference.OgrenciResult OgrenciDonemGetir(string ogrenciNo) {
            SocialObisis.ObisisServiceReference.OgrenciDonemGetirRequest inValue = new SocialObisis.ObisisServiceReference.OgrenciDonemGetirRequest();
            inValue.ogrenciNo = ogrenciNo;
            SocialObisis.ObisisServiceReference.OgrenciDonemGetirResponse retVal = ((SocialObisis.ObisisServiceReference.IObisisMobileService)(this)).OgrenciDonemGetir(inValue);
            return retVal.OgrenciDonemGetirResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SocialObisis.ObisisServiceReference.GetDataUsingDataContractResponse SocialObisis.ObisisServiceReference.IObisisMobileService.GetDataUsingDataContract(SocialObisis.ObisisServiceReference.GetDataUsingDataContractRequest request) {
            return base.Channel.GetDataUsingDataContract(request);
        }
        
        public SocialObisis.ObisisServiceReference.CompositeType GetDataUsingDataContract(SocialObisis.ObisisServiceReference.CompositeType composite) {
            SocialObisis.ObisisServiceReference.GetDataUsingDataContractRequest inValue = new SocialObisis.ObisisServiceReference.GetDataUsingDataContractRequest();
            inValue.composite = composite;
            SocialObisis.ObisisServiceReference.GetDataUsingDataContractResponse retVal = ((SocialObisis.ObisisServiceReference.IObisisMobileService)(this)).GetDataUsingDataContract(inValue);
            return retVal.GetDataUsingDataContractResult;
        }
    }
}
