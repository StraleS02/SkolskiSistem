using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input.Manipulations;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace projekat_tab1
{
    //JMBG, ime, prezime, adresu i ikonicu
    public class Ucenik : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string JMBG;
        private string ime;
        private string prezime;
        private string adresa;
        private ImageSource slika;

        public Ucenik(string JMBG, string ime, string prezime, string adresa)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.adresa = adresa;
            this.JMBG = JMBG;
        }

        public Ucenik(string JMBG, string ime, string prezime, string adresa, ImageSource slika)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.adresa = adresa;
            this.JMBG = JMBG;
            this.slika = slika;
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

        public string Jmbg
        {
            get { return JMBG; }
            set
            {
                if (JMBG != value)
                {
                    JMBG = value;
                    NotifyPropertyChanged("Jmbg");
                }
            }
        }
        public string Ime
        {
            get { return ime; }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    NotifyPropertyChanged("Ime");
                }
            }
        }

        public string Prezime
        {
            get { return prezime; }
            set
            {
                if (prezime != value)
                {
                    prezime = value;
                    NotifyPropertyChanged("Prezime");
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

    }

}

