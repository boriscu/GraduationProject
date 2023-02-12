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
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;

namespace Smart_Fridge
{
    public class Recipe
    {
        public List<Stavka> potrebneNamirnice = new List<Stavka>();

        private string uputstvo;

        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public string Uputstvo
        {
            get { return uputstvo; }
            set { uputstvo = value; }
        }

        private string tezina;

        public string Tezina
        {
            get { return tezina; }
            set { tezina = value; }
        }


    }
    public partial class Recipes : Window
    {
        public Recipes()
        {
            InitializeComponent();
            List<Recipe> Recepti = new List<Recipe>();

            if(File.Exists("Recepti.csv") == true)
            {
                using(var reader = new StreamReader(@"Recepti.csv"))
                {
                    while(!reader.EndOfStream)
                    {
                        Recipe recept = new Recipe();
                        var line = reader.ReadLine();
                        if(line.Contains(",") == true)
                        {
                            var values = line.Split(',');
                            recept.Naziv = values[0];
                            recept.Tezina = values[2];
                            recept.Uputstvo = values[3];
                            var potrebneNamirnice = values[1].Split('&');

                            //Stavke se stavljaju u listu stavkeRecept koja je zatim prosledjena kao properti Receptu recept
                            List<Stavka> stavkeRecept = new List<Stavka>();

                            for (int i = 0; i <potrebneNamirnice.Length; i++)
                            {
                                if(potrebneNamirnice[i].Contains("#") == true)
                                {
                                    var temp = potrebneNamirnice[i].Split('#');
                                    string nazivNamirnice = temp[0];
                                    int kolicinaNamirnice = Convert.ToInt32(temp[1]);
                                    Stavka t = new Stavka(nazivNamirnice, kolicinaNamirnice);
                                    stavkeRecept.Add(t);
                                }
                            }
                            recept.potrebneNamirnice = stavkeRecept;
                            //***************************************************//

                            Recepti.Add(recept);

                        }

                    }
                }
            }


            var gridView = new GridView();
            this.ListaRecepata.View = gridView;
            gridView.Columns.Add(new GridViewColumn { Header = "Name", Width = 200, DisplayMemberBinding = new Binding("Naziv") });
            gridView.Columns.Add(new GridViewColumn { Header = "Difficulty", Width = 100, DisplayMemberBinding = new Binding("Tezina") });


            foreach (Recipe a in Recepti)
            {
                ListaRecepata.Items.Add(a);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ListaRecepata.SelectedItem != null)
            {
                LabelSastojci.Visibility = Visibility.Visible;
                LabelUputstva.Visibility = Visibility.Visible;
                TextBoxSastojci.Visibility = Visibility.Visible;
                TextBoxUputstva.Visibility = Visibility.Visible;
                LabelMogucnost.Visibility = Visibility.Visible;;

                //Popunjavanje texboxova uputsvo i potrebne namirnice
                var selected = ListaRecepata.SelectedItem;
                Recipe temp = JsonConvert.DeserializeObject<Recipe>(JsonConvert.SerializeObject(selected));

                TextBoxUputstva.Text = temp.Uputstvo.ToString();

                string sastojci = string.Empty;

                for(int i = 0; i < temp.potrebneNamirnice.Count; i++)
                {
                    sastojci = sastojci + " " + temp.potrebneNamirnice[i].Kolicina  + "g " + temp.potrebneNamirnice[i].Ime + "\n";
                }

                string ispis = "For this recipe you will need: " + sastojci;
                TextBoxSastojci.Text = ispis;
                //***************************************************//

                //Ucitavanje nasih namirnica iz csv fajla u listu Stavke
                List<Stavka> Stavke = new List<Stavka>();
                if (File.Exists("Stavke.csv") == true)
                {
                    using (var reader = new StreamReader(@"Stavke.csv"))
                    {

                        while (!reader.EndOfStream)
                        {
                            Stavka stavka = new Stavka();
                            var line = reader.ReadLine();
                            if (line.Contains(",") == true)
                            {

                                var values = line.Split(',');
                                stavka.Ime = values[0];
                                int x;
                                if (Int32.TryParse(values[1], out x) == true)
                                {
                                    stavka.Kolicina = Convert.ToInt32(values[1]);
                                    Stavke.Add(stavka);
                                }
                            }
                        }
                    }
                }
                //***************************************************//

                int count = 0;

                for(int i = 0; i < temp.potrebneNamirnice.Count; i++)
                {
                    for(int j = 0; j < Stavke.Count; j++)
                    {
                        if(temp.potrebneNamirnice[i].Ime == Stavke[j].Ime)
                        {
                            if(temp.potrebneNamirnice[i].Kolicina <= Stavke[j].Kolicina)
                            {
                                count++;
                            }
                        }
                    }
                }

                if (count == temp.potrebneNamirnice.Count)
                {
                    LabelMogucnost.Content = "You have all the ingridients";
                }
                else
                {
                    LabelMogucnost.Content = "You dont have all the ingridients";
                }
            }

        }
    }
}
