﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Common.Enum
{
    public class LogFolderAttribute : Attribute
    {
        public string FolderName { set; get; }
    }
}