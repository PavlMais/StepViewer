using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient;

namespace UI.Models
{
    public class ActiveSession : IdBindable
    {

        public int GroupId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan DurationTime { get => StartTime - DateTime.Now; }

        public ObservableCollection<User> Members { get; set; }

        public ActiveSession()
        {
        }

        
    }
}
