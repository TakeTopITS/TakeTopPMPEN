﻿using System;

namespace ProjectMgt.Model
{
    public class WFTemplateRelatedJSCode
    {
        public WFTemplateRelatedJSCode()
        {
        }

        public virtual int ID { get; set; }
        public virtual string TemName { get; set; }
        public virtual string JSCode { get; set; }
        public virtual string Comment { get; set; }
        public virtual string CreatorCode { get; set; }
        public virtual string CreatorName { get; set; }
        public virtual DateTime CreateTime { get; set; }
    }
}