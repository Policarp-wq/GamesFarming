using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GamesFarming.MVVM.Models
{
    internal class FilterableCollection<T>
    {
        public Func<T, bool> Filter { get; private set; }
        public ObservableCollection<T> Items { get; set; }
        public event Action FilterChanged;

        public FilterableCollection(IEnumerable<T> collection)
        {
            Items = new ObservableCollection<T>(collection);
        }
        public FilterableCollection() : this(Enumerable.Empty<T>()) { }

        public ObservableCollection<T> GetFiltered(IComparer<T> comparer = null)
        {
            var filtered = Items.Where(x => Filter is null ? true : Filter(x)).ToArray();
            if(comparer == null)
            {
                return new ObservableCollection<T>(filtered);
            }
            Array.Sort(filtered, comparer);
            return new ObservableCollection<T>(filtered);
        }

        public void SetFilter(Func<T, bool> filter)
        {
            Filter = filter;
            FilterChanged?.Invoke();
        }
        public void ClearFilter()
        {
            Filter = null;
            FilterChanged?.Invoke();
        }
    }
}
