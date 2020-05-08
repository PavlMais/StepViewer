using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    class GroupInvite : IdBindable
    {
        public int FromUserId { get; set; }
        public User FromUser { get; set; }

        public int ToGroupId { get; set; }
        public Group ToGroup { get; set; }
    }
}
