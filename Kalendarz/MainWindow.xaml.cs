using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kalendarz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        MyCalendar mojKalendarz = new MyCalendar();
        MyEvent wybraneWydarzenie;
        MyEvent wydarzenieListy;
        DateTime dataNowegoWydarzenia;
        Style s;



        public MainWindow()
        {
            InitializeComponent();
            s = (Style)this.Resources["CalendarDayButtonStyle"];
            //Ustawia czcionkę całego okna
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata{DefaultValue = FindResource(typeof(Window))});

            //Zegareczek
            timer.Tick += new EventHandler(timer_Click);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            
            //testowe wydarzenia
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 07, 1), EventType.Egzamin, "Mam egzamin");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 08, 3), EventType.Randka, "Anitka jest the best");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 06, 28), EventType.Inne, "Ide do Kosciola");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 07, 2), EventType.Szkoła, "Ahh ta szkoła");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 08, 25), EventType.Praca, "Ide do pracy zarabiać pieniądze!");
            mojKalendarz.DodajWydarzenie(DateTime.Today, EventType.Inne, "bardzo kocham Anitke");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 06, 28), EventType.Szkoła, "Ide do szkoły się uczyć");
            mojKalendarz.DodajWydarzenie(new DateTime(2018, 06, 14), EventType.Randka, "Walentynki");


            //Kolorkuje poszczególne dni
            PomalujDni(mojKalendarz.Wydarzenia, mojKalendarz.uzyteDaty);
            //Wykreśla przeszłość
            BlackoutPast();
            //Aktualizuje kontrolki
            UpdateControls(wybraneWydarzenie);
        }

        private void timer_Click(object sender, EventArgs e)
        {
            DateTime teraz = DateTime.Now;
            if(teraz.Minute<10)
                tbZegar.Text = teraz.Hour + " : 0" + teraz.Minute;
            else
                tbZegar.Text = teraz.Hour + " : " + teraz.Minute;
        }

       


        private void UpdateControls(MyEvent WydarzenieKontekstowe)
        {
            InitBindingToday(WydarzenieKontekstowe);
            NajblizszeWydarzenia();
            tbToday.Text = "Dziś jest " + DateTime.Now.ToString("dddd, dd.MM.yyyy");

            if (mojKalendarz.Wydarzenia.Contains(wybraneWydarzenie))
            {
                btKillEvent.Visibility = Visibility.Visible;
                btChangeDate.Visibility = Visibility.Visible;
                btAddEvent.Visibility = Visibility.Collapsed;
            }

            else
            {
                btKillEvent.Visibility = Visibility.Collapsed;
                btChangeDate.Visibility = Visibility.Collapsed;
                btAddEvent.Visibility = Visibility.Visible;
            }

            if (gAddEvent.Visibility == Visibility.Visible)
            GetNewEventDate();
        }


        private void PomalujDni(List<MyEvent> wydarzenia, List<DateTime> uzyteDaty)
        {
            
            foreach (MyEvent wydarzenie in wydarzenia)
            {
                s.Triggers.Add(wydarzenie.dataTrigger);
                uzyteDaty.Add(wydarzenie.Data);
            }
        }
        //private void PomalujDni2(List<MyEvent> wydarzenia, List<DateTime> uzyteDaty)
        //{
        //    Style s = (Style)this.Resources["CalendarDayButtonStyle"];
            
        //    foreach (MyEvent wydarzenie in wydarzenia)
        //    {
        //        if (wydarzenie.Rodzaj == EventType.Egzamin && !sprawdzCzyDataBylaUzyta(wydarzenie, uzyteDaty))
        //        {
        //            DateTime holidayDate = wydarzenie.Data;
        //            DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
        //            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.Red));
        //            s.Triggers.Add(dataTrigger);
        //            uzyteDaty.Add(wydarzenie.Data);
        //        }

        //        else if (wydarzenie.Rodzaj == EventType.Szkoła && !sprawdzCzyDataBylaUzyta(wydarzenie, uzyteDaty))
        //        {
        //            DateTime holidayDate = wydarzenie.Data;
        //            DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
        //            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.LawnGreen));
        //            s.Triggers.Add(dataTrigger);
        //            uzyteDaty.Add(wydarzenie.Data);
        //        }

        //        else if (wydarzenie.Rodzaj == EventType.Praca && !sprawdzCzyDataBylaUzyta(wydarzenie, uzyteDaty))
        //        {
        //            DateTime holidayDate = wydarzenie.Data;
        //            DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
        //            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.Gold));
        //            s.Triggers.Add(dataTrigger);
        //            uzyteDaty.Add(wydarzenie.Data);
        //        }

        //        else if (wydarzenie.Rodzaj == EventType.Randka && !sprawdzCzyDataBylaUzyta(wydarzenie, uzyteDaty))
        //        {
        //            DateTime holidayDate = wydarzenie.Data;
        //            DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
        //            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.HotPink));
        //            s.Triggers.Add(dataTrigger);
        //            uzyteDaty.Add(wydarzenie.Data);
        //        }

        //        else if (wydarzenie.Rodzaj == EventType.Inne && !sprawdzCzyDataBylaUzyta(wydarzenie, uzyteDaty))
        //        {
        //            DateTime holidayDate = wydarzenie.Data;
        //            DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
        //            dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.LightSkyBlue));
        //            s.Triggers.Add(dataTrigger);
        //            uzyteDaty.Add(wydarzenie.Data);
        //        }
               
        //    }
        //}


        private bool sprawdzCzyDataBylaUzyta(MyEvent wydarzenie, List<DateTime> uzyteWydarzenia)
        {
            foreach (var data in uzyteWydarzenia)
            {
                if (wydarzenie.Data == data)
                    return true;
            }
            return false;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            bool found = false;
            foreach (var wydarzenie in mojKalendarz.Wydarzenia)
            {
                if (wydarzenie.Data == calendar.SelectedDate) {
                    wybraneWydarzenie = wydarzenie;
                    found = true;
                    
                    break;
                }
            }
            if (!found)
                wybraneWydarzenie = null;
            UpdateControls(wybraneWydarzenie);
           
        }

        private void InitBindingToday(MyEvent wydarzenieKontekstowe)
        {
            //wybraneWydarzenie = mojKalendarz.OdczytajWydarzenie(DateTime.Parse(calendar.SelectedDate.ToString()));
            spWybraneWydarzenie.DataContext = wydarzenieKontekstowe;
        }

        private void BlackoutPast()
        {
            calendar.SelectedDate = DateTime.Today;
            calendar.BlackoutDates.Add(new CalendarDateRange(new DateTime(1990, 1, 1), DateTime.Now.AddDays(-1)));
        }

        private void NajblizszeWydarzenia()
        {
            EventsComparer comparer = new EventsComparer();
            comparer.SortBy = SortCriteria.DateThenType;
            mojKalendarz.Wydarzenia.Sort(comparer);
            lvCommingSoon.Items.Clear();
            foreach (var wydarzenie in mojKalendarz.Wydarzenia)
            {
                if (lvCommingSoon.Items.Count<5)
                {
                    if (wydarzenie.Data >= DateTime.Today)
                    {
                        if (!lvCommingSoon.Items.Contains(wydarzenie))
                        {
                            lvCommingSoon.Items.Add(wydarzenie);
                        }
                    }
                    
                }
            }
        }

        private void tbOpisWybranegoWydarzenia_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateControls(wybraneWydarzenie);
        }

        private void BtShowToday_Click(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDate = DateTime.Today;
            calendar.DisplayDate = DateTime.Today;

            // mojKalendarz.ZapiszNaDysku();
            //var path = @"C:\Users\Bla\Documents\Visual Studio 2017\Projects\Kalendarz\Kalendarz\ser\" + ".xml";
            //XmlSerialization.WriteToXmlFile<List<MyEvent>>("people.txt", mojKalendarz.Wydarzenia);
        }

        private void btChangeDate_Click(object sender, RoutedEventArgs e)
        {
            btAcceptDataChange.Visibility = Visibility.Visible;
            dpZmianaDaty.Visibility = Visibility.Visible;
            btChangeDate.Visibility = Visibility.Collapsed;
            btKillEvent.Visibility = Visibility.Collapsed;
        }

        private void btKillEvent_Click(object sender, RoutedEventArgs e)
        {
            //s.Triggers.Remove(wybraneWydarzenie.dataTrigger);
            mojKalendarz.UsunWydarzenie(wybraneWydarzenie);
            mojKalendarz.uzyteDaty.Remove(wybraneWydarzenie.Data);
            //Style r = (Style)this.Resources["CalendarDayButtonStyle"];
            //DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = wybraneWydarzenie.Data };
            //dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.LightSkyBlue));
            //r.Triggers.Add(dataTrigger);
            
            if (lvCommingSoon.Items.Contains(wybraneWydarzenie))
                lvCommingSoon.Items.Remove(wybraneWydarzenie);
            bool state = false;
            foreach (var item in mojKalendarz.Wydarzenia)
            {
                if (item.Data == wybraneWydarzenie.Data)
                {
                    state = true;
                    wybraneWydarzenie = item;
                    break;
                }     
            }
            if (!state)
            {
                wybraneWydarzenie = null;
            }
            UpdateControls(wybraneWydarzenie);
           

        }

        private void btAddEvent_Click(object sender, RoutedEventArgs e)
        {
            gAddEvent.Visibility = Visibility.Visible;
            GetNewEventDate();
        }

        private void GetNewEventDate()
        {
            dpNoweWydarzenie.Text = calendar.SelectedDate.ToString() ;
            dataNowegoWydarzenia = DateTime.Parse(dpNoweWydarzenie.Text);
        }

        private void lvCommingSoon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var wydarzenie in mojKalendarz.Wydarzenia)
            {
                if (wydarzenie == lvCommingSoon.SelectedItem)
                    wybraneWydarzenie = wydarzenie;
            }
            calendar.DisplayDate = wybraneWydarzenie.Data;
            spWybraneWydarzenie.DataContext = wybraneWydarzenie;
            UpdateControls(wybraneWydarzenie);
        }

        private void btAcceptNewEvent_Click(object sender, RoutedEventArgs e)
        {
            EventType nowyRodzaj = new EventType();
            switch (cbRodzaj.SelectedIndex)
            {
                case 0:
                    nowyRodzaj = EventType.Egzamin;
                    break;
                case 1:
                    nowyRodzaj = EventType.Szkoła;
                    break;
                case 2:
                    nowyRodzaj = EventType.Praca;
                    break;
                case 3:
                    nowyRodzaj = EventType.Randka;
                    break;
                case 4:
                    nowyRodzaj = EventType.Inne;
                    break;
                default:
                    break;
            }
            DateTime nowaData = dpNoweWydarzenie.SelectedDate.Value;
            mojKalendarz.DodajWydarzenie(nowaData, nowyRodzaj, tbOpisNowego.Text);
            gAddEvent.Visibility = Visibility.Hidden;
            wybraneWydarzenie = mojKalendarz.OdczytajWydarzenie(nowaData);
            UpdateControls(wybraneWydarzenie);
            mojKalendarz.uzyteDaty.Add(wybraneWydarzenie.Data);
            
        }

        private void btCancelNewEvent_Click(object sender, RoutedEventArgs e)
        {
            gAddEvent.Visibility = Visibility.Hidden;            
        }

        private void btAcceptDataChange_Click(object sender, RoutedEventArgs e)
        {
            btAcceptDataChange.Visibility = Visibility.Collapsed;
            dpZmianaDaty.Visibility = Visibility.Collapsed;
            btKillEvent.Visibility = Visibility.Visible;
            btChangeDate.Visibility = Visibility.Visible;
            mojKalendarz.ZmienDateWydarzenia(wybraneWydarzenie, dpZmianaDaty.SelectedDate.Value);
        }
    }

    
}
