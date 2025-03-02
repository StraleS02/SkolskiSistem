using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace projekat_tab1
{
    class Citac
    {
        private string folderPath_ucenik = "slikeU";
        private string folderPath_skola = "slikeS";

        public ObservableCollection<Skola> citaj(string putanja)
        {
            ObservableCollection<Skola> skole = new ObservableCollection<Skola>();
            ObservableCollection<Ucenik> ucenicis = new ObservableCollection<Ucenik>();

            using (StreamReader sr = new StreamReader(putanja))
            {
                string linija;
                while ((linija = sr.ReadLine()) != null)
                {

                    string[] delovi = linija.Split(";");
                    if (delovi.Length == 4)
                    {

                        if (delovi[0] == "0")
                        {
                            string naziv_s = delovi[1];
                            int id_s = int.Parse(delovi[2]);
                            string adresa_s = delovi[3];

                            string nazivS = naziv_s;
                            if (nazivS != "NEUPISANI")
                            {
                                string newFileNameS = "";
                                string[] parts = nazivS.Split(' ');

                                string imeS = parts[0];
                                //string prezimeS = "";
                                //try
                                //{
                                    string prezimeS = parts[1];

                                //}
                                //catch { }

                                newFileNameS += imeS;
                                newFileNameS += "_";
                                newFileNameS += prezimeS;
                                newFileNameS += ".png";
                                string newFilePathS = Path.Combine(folderPath_skola, newFileNameS);
                                if (File.Exists(newFilePathS))
                                {
                                    BitmapImage slika = new BitmapImage();
                                    slika.BeginInit();
                                    slika.UriSource = new Uri(newFilePathS, UriKind.RelativeOrAbsolute);
                                    slika.CacheOption = BitmapCacheOption.OnLoad;
                                    slika.EndInit();

                                    Skola s = new Skola(id_s, naziv_s, adresa_s, slika);
                                    foreach (Ucenik u in ucenicis)
                                    {
                                        s.dodajUcenikaTest(u);
                                    }
                                    ucenicis.Clear();
                                    skole.Add(s);
                                }
                                else
                                {
                                    Skola s = new Skola(id_s, naziv_s, adresa_s);
                                    foreach (Ucenik u in ucenicis)
                                    {
                                        s.dodajUcenikaTest(u);
                                    }
                                    ucenicis.Clear();
                                    skole.Add(s);
                                }
                            }
                            else
                            {
                                Skola s = new Skola(id_s, naziv_s, adresa_s);
                                foreach (Ucenik u in ucenicis)
                                {
                                    s.dodajUcenikaTest(u);
                                }
                                ucenicis.Clear();
                                skole.Add(s);
                            }
                        }
                        else
                        {
                            string ime = delovi[0];
                            string prezime = delovi[1];
                            string adresa = delovi[2];
                            string jbmg = delovi[3];

                            string newFileName = "";
                            newFileName += ime;
                            newFileName += "_";
                            newFileName += prezime;
                            newFileName += ".png";
                            string newFilePath = Path.Combine(folderPath_ucenik, newFileName);
                            if (File.Exists(newFilePath))
                            {
                                BitmapImage slika = new BitmapImage();
                                slika.BeginInit();
                                slika.UriSource = new Uri(newFilePath, UriKind.RelativeOrAbsolute);
                                slika.CacheOption = BitmapCacheOption.OnLoad;
                                slika.EndInit();

                                Ucenik u = new Ucenik(jbmg, ime, prezime, adresa, slika);
                                ucenicis.Add(u);
                            }
                            else
                            {
                                Ucenik u = new Ucenik(jbmg, ime, prezime, adresa);
                                ucenicis.Add(u);
                            }
                        }
                    }
                }
            }
            return skole;
        }
        public void pisi(ObservableCollection<Skola> skolas)
        {
            StreamWriter sw = new StreamWriter("Ucenici.txt");

            foreach (Skola s in skolas)
            {
                string tekst_s = "";
                foreach (Ucenik u in s.Ucenici)
                {
                    string tekst_u = "";
                    tekst_u += u.Ime;
                    tekst_u += ";";
                    tekst_u += u.Prezime;
                    tekst_u += ";";
                    tekst_u += u.Adresa;
                    tekst_u += ";";
                    tekst_u += u.Jmbg;
                    sw.WriteLine(tekst_u);
                    if (u.Slika != null)
                    {
                        string newFileName = "";
                        newFileName += u.Ime;
                        newFileName += "_";
                        newFileName += u.Prezime;
                        newFileName += ".png";
                        string newFilePath = Path.Combine(folderPath_ucenik, newFileName);
                        ImageSource image = u.Slika;
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
                        using (FileStream fs = new FileStream(newFilePath, FileMode.Create))
                        {
                            encoder.Save(fs);
                        }
                    }
                }
                tekst_s += "0;";
                tekst_s += s.Naziv;
                tekst_s += ";";
                tekst_s += s.Id.ToString();
                tekst_s += ";";
                tekst_s += s.Adresa;
                sw.WriteLine(tekst_s);
                if (s.Slika != null)
                {
                    string newFileName = "";
                    string nazivS = s.Naziv;
                    string[] parts = nazivS.Split(' ');
                    string imeS = parts[0];
                    //string prezimeS = "";
                    //try
                    //{
                        string prezimeS = parts[1];

                    //}
                    //catch { }
                    newFileName += imeS;
                    newFileName += "_";
                    newFileName += prezimeS;
                    newFileName += ".png";
                    string newFilePath = Path.Combine(folderPath_skola, newFileName);
                    ImageSource image = s.Slika;
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
                    using (FileStream fs = new FileStream(newFilePath, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
            }
            sw.Close();
        }

        public void obrisi_sliku(Skola skola)
        {
            string naziv = skola.Naziv;
            string[] parts = naziv.Split(' ');
            string putanja = Path.Combine(folderPath_skola, parts[0] + "_" + parts[1] + ".png");
            try
            {
                File.Delete(putanja);
            }
            catch
            {
                MessageBox.Show("Greska pri brisanju");
            }
        }
    }
}
