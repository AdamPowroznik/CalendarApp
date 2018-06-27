using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Kalendarz
{
    public enum EventType
    {
        Egzamin,
        Szkoła,
        Praca,
        Randka,
        Inne,
    }
    [XmlInclude(typeof(System.Windows.Data.Binding))]
    [Serializable]
    public  class MyCalendar 
    {
        EventsComparer comparer = new EventsComparer();
        public List<MyEvent> Wydarzenia = new List<MyEvent>();
        public List<DateTime> uzyteDaty = new List<DateTime>();
        

        
       

        public bool DodajWydarzenie(DateTime data, EventType rodzaj, string opis)
        {
            if (!SprawdzCzyWydarzenieIstnieje(data, rodzaj))
            {
                Wydarzenia.Add(new MyEvent(opis, data, rodzaj));
                comparer.SortBy = SortCriteria.DateThenType;
                Wydarzenia.Sort(comparer);
                return true;
            }
            else
                return false;
        }

        public void UsunWydarzenie(MyEvent wydarzenie)
        {
            Wydarzenia.Remove(wydarzenie);
            
        }

        public MyEvent OdczytajWydarzenie(DateTime dataWydarzenia)
        {
            foreach (MyEvent wydarzenie in Wydarzenia)
            {
                if (wydarzenie.Data == dataWydarzenia)
                    return wydarzenie; 
            }
            return null;
        }

        
        public void ZapiszNaDysku()
        {
          
        }

        public bool SprawdzCzyWydarzenieIstnieje(DateTime data, EventType rodzaj)
        {
            foreach (var wydarzenie in Wydarzenia)
            {
                if (wydarzenie.Data == data && wydarzenie.Rodzaj == rodzaj)
                    return true;
            } 
            return false;
        }
        
        public void ZmienDateWydarzenia(MyEvent wydarzenie, DateTime nowaData)
        {

        }

        

    }
}
