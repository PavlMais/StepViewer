﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DurationTime { get; set; }
    }
}
