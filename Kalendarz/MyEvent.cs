using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace Kalendarz
{
    [XmlInclude(typeof(System.Windows.Data.Binding))]
    [Serializable]
    public class MyEvent
    {
        public String Opis { get { return opis; } set { opis = value; } }
        public DateTime Data { get { return data; } }
        public EventType Rodzaj { get { return rodzaj; } }

        public String DataString { get { return data.ToShortDateString(); } }
        public String RodzajString { get { return rodzaj.ToString(); } }
        public String OpisString { get { return Opis.ToString(); } }

        private String opis;
        private DateTime data;
        private EventType rodzaj;
        private SolidColorBrush kolor;

        public DataTrigger dataTrigger;
        

        public MyEvent(String opis, DateTime data, EventType rodzaj)
        {
            this.opis = opis;
            this.data = data;
            this.rodzaj = rodzaj;
            switch (rodzaj)
            {
                case EventType.Egzamin:
                    kolor = Brushes.Red;
                    break;
                case EventType.Szkoła:
                    kolor = Brushes.LawnGreen;
                    break;
                case EventType.Praca:
                    kolor = Brushes.Gold;
                    break;
                case EventType.Randka:
                    kolor = Brushes.HotPink;
                    break;
                case EventType.Inne:
                    kolor = Brushes.LightSkyBlue;
                    break;
                default:
                    break;
            }
            dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = data };
            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, kolor));
        }

        public MyEvent()
        {

        }

        public override String ToString()
        {
            return Data.ToString("dddd, d.MM") + "  " + Rodzaj.ToString();
        }

        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
      
        }
    }
}
