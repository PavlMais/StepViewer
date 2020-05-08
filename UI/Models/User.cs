using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class User : IdBindable
    {
        private string _username;

        public string Username { get => _username; set => SetProperty(ref _username, value); }
    }
}
