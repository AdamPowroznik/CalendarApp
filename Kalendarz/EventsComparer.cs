using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarz
{
    public enum SortCriteria
    {
        TypeThenDate,
        DateThenType,
    }
    class EventsComparer : IComparer<MyEvent>
    {
        public SortCriteria SortBy = SortCriteria.DateThenType;

        public int Compare(MyEvent x, MyEvent y)
        {
            if (SortBy == SortCriteria.DateThenType)
            {
                if (x.Data > y.Data)
                    return 1;
                else if (x.Data < y.Data)
                    return -1;
                if (x.Rodzaj > y.Rodzaj)
                    return 1;
                else if (x.Rodzaj < y.Rodzaj)
                    return -1;
                else
                    return 0;
            }
            else
            {
                if (x.Rodzaj > y.Rodzaj)
                    return 1;
                else if (x.Rodzaj < x.Rodzaj)
                    return -1;
                else
                    if (x.Data > y.Data)
                    return 1;
                else if (x.Data < y.Data)
                    return -1;
                else
                    return 0;
            }
        }
          
    }
}
