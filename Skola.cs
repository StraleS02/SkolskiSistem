using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace projekat_tab1
{

    internal class Skola : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int ID;
        private string naziv;
        private string adresa;
        private ImageSource slika;
        private ObservableCollection<Ucenik> ucenici;

        public Skola() { }
        public Skola(int ID, string naziv, string adresa)
        {
            this.ID = ID;
            this.naziv = naziv;
            this.adresa = adresa;
            ucenici = new ObservableCollection<Ucenik>();
        }

        public Skola(int ID, string naziv, string adresa, ImageSource slika)
        {
            this.ID = ID;
            this.naziv = naziv;
            this.adresa = adresa;
            this.slika = slika;
            ucenici = new ObservableCollection<Ucenik>();
        }

        public void dodajUcenikaTest(Ucenik u)
        {
            ucenici.Add(u);
        }
        public void dodajUcenika(string text)
        {

        }

        public void ukloniUcenika(Ucenik u)
        {
            ucenici.Remove(u);
        }

        public ImageSource Slika
        {
            get { return slika; }
            set
            {
                if (slika != value)
                {
                    slika = value;
                    NotifyPropertyChanged("Slika");
                }
            }
        }

        public ObservableCollection<Ucenik> Ucenici
        {
            get { return ucenici; }
            set
            {
                if (ucenici != value)
                {
                    // Unsubscribe from the CollectionChanged event of the previous collection
                    if (ucenici != null) { }
                    ucenici.CollectionChanged -= Ucenici_CollectionChanged;

                    ucenici = value;

                    // Subscribe to the CollectionChanged event of the new collection
                    if (ucenici != null) { }
                    ucenici.CollectionChanged += Ucenici_CollectionChanged;

                    NotifyPropertyChanged("Ucenici");
                }
            }


        }
        public int Id
        {
            get { return ID; }
            set
            {
                if (ID != value)
                {
                    ID = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        public string Naziv
        {
            get { return naziv; }
            set
            {
                if (naziv != value)
                {
                    naziv = value;
                    NotifyPropertyChanged("Naziv");
                }
            }
        }
        public string Adresa
        {
            get { return adresa; }
            set
            {
                if (adresa != value)
                {
                    adresa = value;
                    NotifyPropertyChanged("Adresa");
                }
            }
        }

        private void NotifyPropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }

        private void Ucenici_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Ucenici");
        }
    }


}

