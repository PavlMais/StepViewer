using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WCFClient;
namespace UI.Models
{
    public static class APIProxy
    {
        public static API api = new API();
        public static IMapper mapper;
        static APIProxy()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, WCFClient.ServiceReference1.User>();
                cfg.CreateMap<Group, WCFClient.ServiceReference1.Group>();
                cfg.CreateMap<WCFClient.ServiceReference1.Group, Group> ();
            });
            mapper = config.CreateMapper();

        }

        public static IEnumerable<Group> GetGroups()
        {
            IEnumerable<WCFClient.ServiceReference1.Group> groups = api.GetGroups();




            return mapper.Map<IEnumerable<Group>>(groups);
        }


    }
}
