﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }

        public User Owner { get; set; }

        public List<User> Users { get; set; }
    }
}
