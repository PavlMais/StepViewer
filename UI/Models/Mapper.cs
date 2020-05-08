using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient.ServiceReference1;
using NLog;

namespace UI.Models
{
    public static class Mpr
    {
        public static IMapper mapper;
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static Mpr()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, WCFClient.ServiceReference1.User>();
                cfg.CreateMap<WCFClient.ServiceReference1.User, User>();
                cfg.CreateMap<Group, WCFClient.ServiceReference1.Group>();
                cfg.CreateMap<WCFClient.ServiceReference1.Group, Group>();
                cfg.CreateMap<ActiveSession, WCFClient.ServiceReference1.ActiveSession>();
                cfg.CreateMap<WCFClient.ServiceReference1.ActiveSession, ActiveSession>();
                cfg.CreateMap<GroupInvite, WCFClient.ServiceReference1.GroupInvite>();
                cfg.CreateMap<WCFClient.ServiceReference1.GroupInvite, GroupInvite>();
            });
            mapper = config.CreateMapper();
            Logger.Debug("Init automapper done.");
        }

        public static T Map<T>(object obj)
        {

            Logger.Debug($"Map {typeof(T)} < {obj?.GetType()}");
            try
            {
                return mapper.Map<T>(obj);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
            }
        }
        public static TDest Map<TSource, TDest>(TSource obj)
        {
            return mapper.Map<TSource, TDest>(obj);
        }
    }
}
