﻿using System;

namespace ProjectMgt.Model
{
    public class DepartRelatedWZFeeUser
    {
        public DepartRelatedWZFeeUser()
        {
        }

        public virtual int ID { get; set; }
        public virtual string DepartCode { get; set; }
        public virtual string UserCode { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime EffectDate { get; set; }
    }
}