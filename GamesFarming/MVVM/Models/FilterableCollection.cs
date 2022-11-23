using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ObservableCollection<T> GetFiltered()
            => new ObservableCollection<T>(Items.Where(x => Filter is null ? true : Filter(x)));

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
