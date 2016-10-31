﻿using System.Collections.Generic;

namespace LightBlue.Database
{
    public class BlobContainer
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public virtual List<BlobDirectory> Directories { get; set; }
    }
}