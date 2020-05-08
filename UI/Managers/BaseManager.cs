using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Models;
using WCFClient;

namespace UI.Managers
{
    public abstract class BaseManager<TManager, TItem> : BindableBase where TManager : new() where TItem : IdBindable
    {
        protected static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static TManager instance;
        private ObservableCollection<TItem> s_collection = new ObservableCollection<TItem>();

        public ObservableCollection<TItem> Collection { get => s_collection; set => SetProperty(ref s_collection, value); }

        public static TManager GetInstance()
        {
            Logger.Debug($"Inti manager {typeof(TManager)}");
            if (instance == null)
                instance = new TManager();
            return instance;
        }
        protected BaseManager()
        {
            Init();
        }

        protected void AddItem(object item)
        {
            Logger.Debug($"Add item {item.GetType()}");
            Collection.Add(Mpr.Map<TItem>(item));
        }
        protected void Update(IEnumerable<object> items)
        {
            Logger.Debug($"Update items {items.GetType()} {items.Count()}");
            Collection = new ObservableCollection<TItem>(Mpr.Map<ICollection<TItem>>(items));
        }
        public TItem GetItem(int id)
        {
            if(Collection.Any(i => i.Id == id))
                return Collection.First(i => i.Id == id);
            
            TItem item = GetFromService(new List<int>() { id }).First();
            Collection.Add(item);
            return item;
        }

        public ICollection<TItem> GetItems(ICollection<int> ids)
        {
            
            List<TItem> found = new List<TItem>();
            List<int> notFoundIds = new List<int>();

            found.AddRange(Collection.Where(i => ids.Contains(i.Id)));
            notFoundIds.AddRange(ids.Where(i => !found.Any(f => f.Id == i)));


            ICollection<TItem> fromService = GetFromService(notFoundIds);

            foreach (var item in fromService)
            {
                Collection.Add(item);
            }

            found.AddRange(fromService);

            return found;
        }

        protected abstract ICollection<TItem> GetFromService(ICollection<int> ids);
        protected abstract void Init();
    }



    public class UserManager : BaseManager<UserManager, User>
    {
        protected override void Init()
        {
        }


        protected override ICollection<User> GetFromService(ICollection<int> ids)
        {
            return Mpr.Map<List<User>>(API.proxy.GetUsers(ids.ToArray()).Data);
        }

    }
}
