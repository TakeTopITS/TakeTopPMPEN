﻿using System;

namespace ProjectMgt.Model
{
    public class AssetApplicationReport
    {
        public AssetApplicationReport()
        {
        }

        private int id;
        private string assetCode;
        private string assetName;
        private string modelNumber;
        private string spec;
        private decimal number;
        private string unit;
        private string ip;

        private int aaID;
        private string aaName;
        private string type;
        private string applicantCode;
        private string applicantName;
        private DateTime applyTime;
        private DateTime finishTime;
        private string applyReason;
        private string status;
        private string relatedType;
        private int relatedID;

        public virtual int ID
        {
            get { return id; }
            set { id = value; }
        }

        public virtual string AssetCode
        {
            get { return assetCode; }
            set { assetCode = value; }
        }

        public virtual string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }

        public virtual string ModelNumber
        {
            get { return modelNumber; }
            set { modelNumber = value; }
        }

        public virtual string Spec
        {
            get { return spec; }
            set { spec = value; }
        }

        public virtual decimal Number
        {
            get { return number; }
            set { number = value; }
        }

        public virtual string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public virtual string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public virtual int AAID
        {
            get { return aaID; }
            set { aaID = value; }
        }

        public virtual string AAName
        {
            get { return aaName; }
            set { aaName = value; }
        }

        public virtual string Type
        {
            get { return type; }
            set { type = value; }
        }

        public virtual string ApplicantCode
        {
            get { return applicantCode; }
            set { applicantCode = value; }
        }

        public virtual string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }

        public virtual DateTime ApplyTime
        {
            get { return applyTime; }
            set { applyTime = value; }
        }

        public virtual DateTime FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }

        public virtual string ApplyReason
        {
            get { return applyReason; }
            set { applyReason = value; }
        }

        public virtual string Status
        {
            get { return status; }
            set { status = value; }
        }

        public virtual string RelatedType
        {
            get { return relatedType; }
            set { relatedType = value; }
        }

        public virtual int RelatedID
        {
            get { return relatedID; }
            set { relatedID = value; }
        }
    }
}