using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;


namespace projekat_tab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();
        private ObservableCollection<Skola> skole=new ObservableCollection<Skola>();
        private Skola NEUPISANI;
        private Skola selected_skolice = new Skola();
        private Citac citac = new Citac();
        private Skola tb2SelectedSkola1;
        private Skola tb2SelectedSkola2;
        public MainWindow()
        {
            InitializeComponent();

            tb2ListView1.IsEnabled = false;
            tb2ListView2.IsEnabled = false;

            string exeFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string Mapa = Path.Combine(exeFolderPath, "Srbija.jpg");
            BitmapImage bitmap = new BitmapImage(new Uri(Mapa));
            slikaSrbije.Source = bitmap;

            skole = citac.citaj("Ucenici.txt");

            if(skole.Count!=0)
            {
                NEUPISANI = skole.ElementAt(0);
            }
            else
            {
                //NEUPISANI = new Skola();
                string Naziv = "NEUPISANI";
                int Id = 0;
                string Adresa = "";
                NEUPISANI = new Skola(Id, Naziv, Adresa);
                skole.Add(NEUPISANI);
                citac.pisi(skole);
            }
            
            skolice.ItemsSource = skole;
            skolice.DisplayMemberPath = "Naziv";

            tb2ComboBox1.ItemsSource = tb2ComboBox2.ItemsSource = skole;
            tb2ComboBox1.DisplayMemberPath = "Naziv";
            tb2ComboBox2.DisplayMemberPath = "Naziv";

            SkoleDataGrid.ItemsSource = skole;

            DataContext = this;
        }
        private void dodajU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imeprezime = imeU.Text;
                string[] parts = imeprezime.Split(' ');

                string ime = parts[0];
                string prezime = parts[1];
                string adresa = adresaU.Text;
                string jbmg = jmbg.Text;

                Ucenik ucenik = new Ucenik(jbmg, ime, prezime, adresa);

                NEUPISANI.dodajUcenikaTest(ucenik);

                citac.pisi(skole);

                imeU.Text = "";
                adresaU.Text = "";
                jmbg.Text = "";

                MessageBox.Show("Uspesno ste dodali ucenika!");
            }
            catch
            {
                MessageBox.Show("Greska pri unosu!");
            }
        }

        private void DataGridCell_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                Skola selected_skolice = new Skola();
                selected_skolice = (Skola)skolice.SelectedItem;

                int rowIndex = ViewDataGrid.SelectedIndex;
                string ime=selected_skolice.Ucenici[rowIndex].Ime;
                string prezime= selected_skolice.Ucenici[rowIndex].Prezime;
                string adresa= selected_skolice.Ucenici[rowIndex].Adresa;
                string jbmg= selected_skolice.Ucenici[rowIndex].Jmbg;
                ImageSource slika = selected_skolice.Ucenici[rowIndex].Slika;

                imeU.Text = ime + " " + prezime;
                adresaU.Text = adresa;
                jmbg.Text = jbmg;
                ikonicaU.Source = slika;

                if (slika != null)
                {
                    textU.IsEnabled = true;
                }
                else
                {
                    textU.IsEnabled = false;
                }

                dodajU.IsEnabled = false;
                izmeniU.IsEnabled = true;
                obrisiU.IsEnabled = true;
                dodajU_slika.IsEnabled = true;
            }
        }

        private void izmeniU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imeprezime = imeU.Text;
                string[] parts = imeprezime.Split(' ');

                string ime = parts[0];
                string prezime = parts[1];
                string adresa = adresaU.Text;
                string jbmg = jmbg.Text;
                int rowIndex = ViewDataGrid.SelectedIndex;
                Skola selected_skolice = new Skola();
                selected_skolice = (Skola)skolice.SelectedItem;

                selected_skolice.Ucenici[rowIndex].Ime = ime;
                selected_skolice.Ucenici[rowIndex].Prezime = prezime;
                selected_skolice.Ucenici[rowIndex].Adresa = adresa;
                selected_skolice.Ucenici[rowIndex].Jmbg = jbmg;

                citac.pisi(skole);

                imeU.Text = "";
                adresaU.Text = "";
                jmbg.Text = "";
                ikonicaU.Source = null;
                ViewDataGrid.UnselectAll();

                dodajU.IsEnabled = true;
                izmeniU.IsEnabled = false;
                obrisiU.IsEnabled = false;
                dodajU_slika.IsEnabled = false;

                MessageBox.Show("Uspesno ste izmenili ucenika!");
            }
            catch
            {
                MessageBox.Show("Greska");
            }
        }

        private void obrisiU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rowIndex = ViewDataGrid.SelectedIndex;
                Skola selected_skolice = new Skola();
                selected_skolice = (Skola)skolice.SelectedItem;
                selected_skolice.ukloniUcenika(selected_skolice.Ucenici[rowIndex]);

                imeU.Text = "";
                adresaU.Text = "";
                jmbg.Text = "";
                ikonicaU.Source = null;
                ViewDataGrid.UnselectAll();

                dodajU.IsEnabled = true;
                izmeniU.IsEnabled = false;
                obrisiU.IsEnabled = false;
                dodajU_slika.IsEnabled = false;

                citac.pisi(skole);
                MessageBox.Show("Uspesno ste uklonili ucenika!");
            }
            catch
            {
                MessageBox.Show("Greska");
            }
            
        }

        private void skolice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected_skolice = (Skola)skolice.SelectedItem;
            ViewDataGrid.ItemsSource = selected_skolice.Ucenici;

            if (selected_skolice.Naziv == "NEUPISANI")
            {
                imeS.Text = "";
                idS.Text = "";
                adresaS.Text = "";
                imeU.Text = "";
                adresaU.Text = "";
                jmbg.Text = "";
                ikonicaS.Source = null;
                ikonicaU.Source = null;
                dodajS.IsEnabled = true;
                izmeniS.IsEnabled = false;
                obrisiS.IsEnabled = false;
                dodajS_slika.IsEnabled = false;
                textS.IsEnabled = false;
                textU.IsEnabled = false;
                ViewDataGrid.UnselectAll();
                dodajU.IsEnabled = true;
                izmeniU.IsEnabled = false;
                obrisiU.IsEnabled = false;
                dodajU_slika.IsEnabled = false;
            }
            else
            {
                imeU.Text = "";
                adresaU.Text = "";
                jmbg.Text = "";
                imeS.Text = selected_skolice.Naziv;
                idS.Text = selected_skolice.Id.ToString();
                adresaS.Text = selected_skolice.Adresa;
                if (selected_skolice.Slika != null)
                {
                    textS.IsEnabled = true;
                }
                else
                {
                    textS.IsEnabled = false;
                }
                ikonicaS.Source = selected_skolice.Slika;
                ikonicaU.Source = null;
                dodajS.IsEnabled = false;
                izmeniS.IsEnabled = true;
                obrisiS.IsEnabled = true;
                dodajS_slika.IsEnabled = true;
                ViewDataGrid.UnselectAll();
                dodajU.IsEnabled = true;
                izmeniU.IsEnabled = false;
                obrisiU.IsEnabled = false;
                dodajU_slika.IsEnabled = false;
            }
        }

        private void dodajS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string naziv = imeS.Text;
                int id = int.Parse(idS.Text);
                string adresa = adresaS.Text;

                string[] parts = naziv.Split(' ');
                string prezime = parts[1];

                if (provera(naziv, id, adresa))
                {
                    Skola skola = new Skola(id, naziv, adresa);
                    skole.Add(skola);
                    imeS.Text = "";
                    idS.Text = "";
                    adresaS.Text = "";
                    MessageBox.Show("Uspesno ste dodali skolu!");
                }
                else
                {
                    MessageBox.Show("Neki od unetih stavki se poklapa sa vec postojecom skolom");
                }
                citac.pisi(skole);
            }
            catch
            {
                MessageBox.Show("Pogresan unos");
            }
        }

        private bool provera(string naziv, int id, string adresa)
        {
            foreach (Skola sk in skole)
            {
                if (sk.Naziv == naziv)
                {
                    return false;
                }
                if (sk.Adresa == adresa)
                {
                    return false;
                }
                if (sk.Id == id)
                {
                    return false;
                }
            }
            return true;
        }

        private void izmeniS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Skola selected_skolice = new Skola();
                selected_skolice = (Skola)skolice.SelectedItem;
                string naziv = imeS.Text;
                string adresa = adresaS.Text;
                int id = int.Parse(idS.Text);

                if (selected_skolice.Naziv != "NEUPISANI")
                {
                    if(provera(naziv, id, adresa))
                    {
                        selected_skolice.Naziv = naziv;
                        selected_skolice.Adresa = adresa;
                        selected_skolice.Id = id;
                        MessageBox.Show("Uspesno ste izmenili skolu!");
                    }
                    else
                    {
                        MessageBox.Show("Neki od unetih stavki se poklapa sa vec postojecom skolom");
                    }
                    citac.pisi(skole);
                }
                else
                {
                    MessageBox.Show("Ne mozete da izmenite NEUPISANE ucenike!");
                }
                
            }
            catch
            {
                MessageBox.Show("Greska");
            }
        }

        private void obrisiS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index_skolice = skolice.SelectedIndex;
                string ime_skolice = skolice.Text;
                if (ime_skolice!="NEUPISANI")
                {
                    skolice.SelectedItem = skole[0];
                    int br=skole[index_skolice].Ucenici.Count();
                    for(int i = 0; i < br; i++)
                    {
                        string ime = skole[index_skolice].Ucenici[i].Ime;
                        string prezime = skole[index_skolice].Ucenici[i].Prezime;
                        string adresa = skole[index_skolice].Ucenici[i].Adresa;
                        string jbmg = skole[index_skolice].Ucenici[i].Jmbg;
                        ImageSource slika = skole[index_skolice].Ucenici[i].Slika;
                        if (slika != null)
                        {
                            Ucenik u = new Ucenik(jbmg, ime, prezime, adresa, slika);
                            NEUPISANI.Ucenici.Add(u);
                        }
                        else
                        {
                            Ucenik u = new Ucenik(jbmg, ime, prezime, adresa);
                            NEUPISANI.Ucenici.Add(u);
                        }
                    }
                    MessageBox.Show("Uspesno ste obrisali skolu!");
                    if (tb2ComboBox1.SelectedItem == skole[index_skolice])
                    {
                        tb2ListView1.ItemsSource = null;
                    }
                    if (tb2ComboBox2.SelectedItem == skole[index_skolice])
                    {
                        tb2ListView2.ItemsSource = null;
                    }
                    SkoleDataGrid.SelectedItem = skole[0];
                    skole.Remove(skole[index_skolice]);
                    citac.pisi(skole);
                }
                else
                {
                    MessageBox.Show("Ne mozete ukloniti NEUPISANE ucenike!");
                }

            }
            catch
            {
                MessageBox.Show("Greska");
            }
        }

        private void dodajS_slika_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Slika";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            int index_skolice = skolice.SelectedIndex;
            if (op.ShowDialog() == true)
            {
                skole[index_skolice].Slika = new BitmapImage(new Uri(op.FileName));
                ikonicaS.Source = skole[index_skolice].Slika;
            }
            citac.pisi(skole);
        }

        private void dodajU_slika_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Slika";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +"JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +"Portable Network Graphic (*.png)|*.png";
            int rowIndex = ViewDataGrid.SelectedIndex;
            Skola selected_skolice = new Skola();
            selected_skolice = (Skola)skolice.SelectedItem;
            if (op.ShowDialog() == true)
            {
                selected_skolice.Ucenici[rowIndex].Slika = new BitmapImage(new Uri(op.FileName));
                ikonicaU.Source = selected_skolice.Ucenici[rowIndex].Slika;
            }
            citac.pisi(skole);
        }

        //-------------------------------TAB2-------------------------------------------------

        private object prevItem1 = null;
        private object prevItem2 = null;
        private void tb2ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb2ComboBox1.SelectedItem == null) return;
            if (tb2SelectedSkola2 != null && ((Skola)tb2ComboBox1.SelectedItem).Id == tb2SelectedSkola2.Id)
            {
                MessageBox.Show("Morate selektovati razlicite skole");
                if (prevItem1 != null)
                {

                    tb2ComboBox1.SelectedItem = prevItem1;
                }
                else
                {
                    e.Handled = true;
                    tb2ComboBox1.SelectedItem = null;
                    return;
                }

            }
            tb2ListView1.IsEnabled = true;
            tb2SelectedSkola1 = (Skola)tb2ComboBox1.SelectedItem;
            tb2ListView1.ItemsSource = (tb2SelectedSkola1.Ucenici);

            prevItem1 = tb2ComboBox1.SelectedItem;

        }

        private void tb2ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb2ComboBox2.SelectedItem == null) return;
            if ((tb2SelectedSkola1 != null) && ((Skola)tb2ComboBox2.SelectedItem).Id == tb2SelectedSkola1.Id)
            {
                MessageBox.Show("Morate selektovati razlicite skole");
                if (prevItem2 != null)
                    tb2ComboBox2.SelectedItem = prevItem2;
                else
                {
                    e.Handled = true;
                    tb2ComboBox2.SelectedItem = null;

                    return;
                }
            }
            tb2ListView2.IsEnabled = true;
            tb2SelectedSkola2 = (Skola)tb2ComboBox2.SelectedItem;
            tb2ListView2.ItemsSource = (tb2SelectedSkola2.Ucenici);

            prevItem2 = tb2ComboBox2.SelectedItem;
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {

            Point mousePos = e.GetPosition(null); // u odnosu na null (tj. u odnosu na ceo ekran) 
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&

                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

                // Get the dragged ListViewItem
                ListView listView = sender as ListView; //referenca na listView kontrolu u kojoj je dogadjaj pokrenut
                if (listView.SelectedItems.Count == 0)
                {
                    e.Handled = true;
                    return;
                }
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                /* FindAncestor < T > je genericka metoda koja prima trenutni DependencyObject(počevši od OriginalSource događaja)
                 * i iterativno dobija roditeljske elemente pomocu VisualTreeHelper.GetParent(current) sve dok ne pronadje element tipa T ili dodje do vrha vizualnog stabla(current == null).*/

                // Find the data behind the ListViewItem
                Ucenik ucenik = (Ucenik)listView.ItemContainerGenerator.
                ItemFromContainer(listViewItem);

                //MessageBox.Show(listView.Name);
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", ucenik);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }

        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {

            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

            }

        }

        private void ListView1_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("myFormat"))
            {

                Ucenik ucenik = e.Data.GetData("myFormat") as Ucenik;
                if (!tb2SelectedSkola1.Ucenici.Contains(ucenik))
                {
                    tb2SelectedSkola2.ukloniUcenika(ucenik);
                    tb2SelectedSkola1.dodajUcenikaTest(ucenik);
                    citac.pisi(skole);
                }
            }
        }

        private void ListView2_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("myFormat"))
            {
                Ucenik ucenik = e.Data.GetData("myFormat") as Ucenik;
                if (!tb2SelectedSkola2.Ucenici.Contains(ucenik))
                {
                    tb2SelectedSkola1.ukloniUcenika(ucenik);
                    tb2SelectedSkola2.dodajUcenikaTest(ucenik);
                    citac.pisi(skole);
                }

            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        //-------------------------TAB3------------------------------------------

        private void SkoleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UceniciDataGrid.ItemsSource = ((Skola)SkoleDataGrid.SelectedItem).Ucenici;

        }

        private DataGridRow datagridItem;
        private void UceniciDataGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null); // u odnosu na null (tj. u odnosu na ceo ekran) 
            Vector diff = startPoint - mousePos;


            if (e.LeftButton == MouseButtonState.Pressed &&

                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {


                DataGrid datagrid = sender as DataGrid; //referenca na listView kontrolu u kojoj je dogadjaj pokrenut
                datagridItem = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                /* FindAncestor < T > je genericka metoda koja prima trenutni DependencyObject(počevši od OriginalSource događaja)
                 * i iterativno dobija roditeljske elemente pomocu VisualTreeHelper.GetParent(current) sve dok ne pronadje element tipa T ili dodje do vrha vizualnog stabla(current == null).*/

                // Nadji ucenika povezanog sa tim selektovanim redom DataGrid-a
                if (datagrid.SelectedItems.Count == 0 || datagridItem == null)
                {
                    e.Handled = true;
                    return;
                }
                if (datagrid.ItemContainerGenerator.ItemFromContainer(datagridItem) is Ucenik ucenik)   //vadi objekat iz kontejnera datagridItema
                {

                    DataObject dragData = new DataObject("myFormat", ucenik);
                    DragDrop.DoDragDrop(datagridItem, dragData, DragDropEffects.Move);


                }
                else if (datagrid.ItemContainerGenerator.ItemFromContainer(datagridItem) is Skola skola)
                {


                    DataObject dragData = new DataObject("myFormat", skola);
                    DragDrop.DoDragDrop(datagridItem, dragData, DragDropEffects.Move);



                }

            }

        }


        private void GridSrbija_Drop(object sender, DragEventArgs e)
        {
            //string putanjaU = "SlikeU";
            if (e.Data.GetDataPresent("myFormat"))
            {
                object draggedData = e.Data.GetData("myFormat");

                if (draggedData is Ucenik ucenik)
                {

                    Image newImage = new Image();
                    newImage.Source = ucenik.Slika;
                    newImage.Width = 35;
                    newImage.Height = 35;

                    Point mousePos = e.GetPosition(GridSrbija);

                    newImage.HorizontalAlignment = HorizontalAlignment.Left;
                    newImage.VerticalAlignment = VerticalAlignment.Top;

                    newImage.Margin = new Thickness(mousePos.X - newImage.Width / 2, mousePos.Y - newImage.Height / 2, 0, 0);

                    ContextMenu imageContextMenu = new ContextMenu();
                    MenuItem menuItemObrisi = new MenuItem();
                    menuItemObrisi.Header = "izbrisi sa mape";
                    menuItemObrisi.Click += MenuItemObrisi_Click;
                    imageContextMenu.Items.Add(menuItemObrisi);

                    MenuItem menuItemObrisiU = new MenuItem();
                    menuItemObrisiU.Header = "Izbrisi";
                    menuItemObrisiU.Click += MenuItemObrisiU_Click;
                    imageContextMenu.Items.Add(menuItemObrisiU);


                    newImage.ContextMenu = imageContextMenu;

                    GridSrbija.Children.Add(newImage);

                    datagridItem.IsEnabled = false;
                }
                else if (draggedData is Skola skola)
                {
                    Skola draggedSkola = e.Data.GetData("myFormat") as Skola;

                    Image newImage = new Image();
                    newImage.Source = skola.Slika;
                    newImage.Width = 35;
                    newImage.Height = 35;

                    Point mousePos = e.GetPosition(GridSrbija);

                    newImage.HorizontalAlignment = HorizontalAlignment.Left;
                    newImage.VerticalAlignment = VerticalAlignment.Top;

                    newImage.Margin = new Thickness(mousePos.X - newImage.Width / 2, mousePos.Y - newImage.Height / 2, 0, 0);

                    ContextMenu imageContextMenu = new ContextMenu();
                    MenuItem menuItemObrisi = new MenuItem();
                    menuItemObrisi.Header = "Izbrisi sa mape";
                    menuItemObrisi.Click += MenuItemObrisi_Click;
                    imageContextMenu.Items.Add(menuItemObrisi);

                    MenuItem menuItemObrisiS = new MenuItem();
                    menuItemObrisiS.Header = "Izbrisi";
                    menuItemObrisiS.Click += MenuItemObrisiS_Click;
                    imageContextMenu.Items.Add(menuItemObrisiS);

                    newImage.ContextMenu = imageContextMenu;

                    GridSrbija.Children.Add(newImage);

                    datagridItem.IsEnabled = false;
                    if (skola.Naziv.Equals("NEUPISANI"))
                    {
                        datagridItem.IsEnabled = true;
                    }
                }
            }

        }

        private void MenuItemObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                // Dobijanje roditeljskog kontekstnog menija (ContextMenu)
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                
                // Dobijanje roditeljskog Image elementa iz kojeg je pokrenut kontekstni meni
                Image image = contextMenu.PlacementTarget as Image;

                // Provera da li je Image element pronađen
                if (image != null)
                {
                    Ucenik trazeniUcenik = null;
                    foreach (var item in UceniciDataGrid.Items)
                    {
                        if (item is Ucenik ucenik && ucenik.Slika == image.Source)
                        {
                            trazeniUcenik = ucenik;
                            break;
                        }
                    }

                    // Provera da li je pronađen učenik
                    if (trazeniUcenik != null)
                    {
                        // Postavljanje vrednosti polja IsEnabled za odgovarajućeg učenika
                        //trazeniUcenik.IsEnabled = true;

                        // Pronalaženje elementa u UceniciDataGrid koji predstavlja odgovarajućeg učenika
                        var ucenikItem = UceniciDataGrid.ItemContainerGenerator.ContainerFromItem(trazeniUcenik) as FrameworkElement;

                        // Provera da li je pronađen element u UceniciDataGrid
                        if (ucenikItem != null)
                        {
                            // Postavljanje vrednosti IsEnabled na elementu u UceniciDataGrid
                            ucenikItem.IsEnabled = true;
                        }
                    }
                    else
                    {
                        Skola trazenaSkola = null;
                        foreach (var item in SkoleDataGrid.Items)
                        {
                            if (item is Skola skola && skola.Slika == image.Source)
                            {
                                trazenaSkola = skola;
                                break;
                            }
                        }

                        // Provera da li je pronađen učenik
                        if (trazenaSkola != null)
                        {
                            // Postavljanje vrednosti polja IsEnabled za odgovarajućeg učenika
                            //trazenaSkola.IsEnabled = true;

                            // Pronalaženje elementa u SkoleDataGrid koji predstavlja odgovarajućeg učenika
                            var skolaItem = SkoleDataGrid.ItemContainerGenerator.ContainerFromItem(trazenaSkola) as FrameworkElement;

                            // Provera da li je pronađen element u UceniciDataGrid
                            if (skolaItem != null)
                            {
                                // Postavljanje vrednosti IsEnabled na elementu u UceniciDataGrid
                                skolaItem.IsEnabled = true;
                            }
                        }
                    }
                    // Uklanjanje Image elementa iz GridSrbija.Children
                    GridSrbija.Children.Remove(image);
                }
            }
        }

        private void MenuItemObrisiU_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;

                Image image = contextMenu.PlacementTarget as Image;

                if (image != null)
                {
                    foreach(UIElement element in GridSrbija.Children)
                    {
                        if(element is Image imageElement && imageElement.Source == image.Source)
                        {
                            Skola skola = GetSkolaFromSlikaU(imageElement.Source);

                            if (skola != null)
                            {
                                foreach(Ucenik u in skola.Ucenici)
                                {
                                    if (u.Slika == imageElement.Source)
                                    {
                                        skola.ukloniUcenika(u);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    citac.pisi(skole);
                    GridSrbija.Children.Remove(image);
                }
            }
        }

        private void MenuItemObrisiS_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;

                Image image = contextMenu.PlacementTarget as Image;

                if (image != null)
                {
                    foreach (UIElement element in GridSrbija.Children)
                    {
                        if (element is Image imageElement && imageElement.Source == image.Source)
                        {
                            Skola skola = GetSkolaFromSlikaS(imageElement.Source);

                            if (skola != null)
                            {
                                if (skola.Slika == imageElement.Source)
                                {
                                    int br = skola.Ucenici.Count();
                                    for (int i = 0; i < br; i++)
                                    {
                                        string ime = skola.Ucenici[i].Ime;
                                        string prezime = skola.Ucenici[i].Prezime;
                                        string adresa = skola.Ucenici[i].Adresa;
                                        string jbmg = skola.Ucenici[i].Jmbg;
                                        ImageSource slika = skola.Ucenici[i].Slika;
                                        if (slika != null)
                                        {
                                            Ucenik u = new Ucenik(jbmg, ime, prezime, adresa, slika);
                                            NEUPISANI.Ucenici.Add(u);
                                        }
                                        else
                                        {
                                            Ucenik u = new Ucenik(jbmg, ime, prezime, adresa);
                                            NEUPISANI.Ucenici.Add(u);
                                        }

                                    }
                                    if (tb2ComboBox1.SelectedItem == skola)
                                    {
                                        tb2ListView1.ItemsSource = null;
                                    }
                                    if (tb2ComboBox2.SelectedItem == skola)
                                    {
                                        tb2ListView2.ItemsSource = null;
                                    }
                                    SkoleDataGrid.SelectedItem=skole[0];
                                    skolice.SelectedItem=skole[0];
                                    skole.Remove(skola);
                                }
                            }
                        }
                    }
                    citac.pisi(skole);
                    GridSrbija.Children.Remove(image);
                }
            }
        }

        private Skola GetSkolaFromSlikaU(ImageSource image)
        {
            foreach(Skola s in skole)
            {
                foreach(Ucenik u in s.Ucenici)
                {
                    if (u.Slika == image)
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        private Skola GetSkolaFromSlikaS(ImageSource image)
        {
            foreach (Skola s in skole)
            {
                if (s.Slika == image)
                {
                    return s;
                }
            }
            return null;
        }
    }
}

