using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Smart_Fridge
{
    public class Stavka
    {


        private int kolicina;

        public int Kolicina
        {
            get { return kolicina; }
            set { kolicina = value; }
        }

        private string ime;

        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }


        public Stavka()
        {
            ime = " ";
            kolicina = 0;
        }

        public Stavka(string i, int k)
        {
            ime = i;
            kolicina = k;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
    public partial class Add : Window
    {
        private string[] tipoviNamirnica = { "Meat", "Fruit", "Vegetable", "Sweets", "Others" };
        private string[] meso = { "Chicken", "Beef", "Turkey", "Fish", "Lamb" };
        private string[] voce = { "Banana", "Strawbery", "Pear", "Apple" };
        private string[] povrce = { "Tomato", "Cabbage", "Zuchini", "Potato","Garlic","Parsley","Paprika" };
        private string[] slatko = { "Candy", "Chocholate", "Ice cream" };
        private string[] ostalo = { "Flour", "Sugar", "Salt", "Baking Powder","Eggs","Milk","Oil","Olive Oil", "Butter","Vanilla Extract", "Mozzarella","Baguette" };

        public List<Stavka> stavkeFrizidera = new List<Stavka>();



        public Add()
        {
            InitializeComponent();

            //Citanje iz fajla u listu stavkeFrizidera
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
                                stavkeFrizidera.Add(stavka);
                            }
                        }
                    }
                }
            }
            //***************************************************//

            this.GrocerieType.ItemsSource = tipoviNamirnica;
            GrocerieType.IsReadOnly = true;
            Grocerie.IsReadOnly = true;
            
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var csv = new StringBuilder();
            
            foreach(Stavka stavka in stavkeFrizidera)
            {
                var first = stavka.Ime;
                var second = stavka.Kolicina;
                var newLine = string.Format("{0},{1}", first, second);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(@"Stavke.csv", csv.ToString());

             
            

            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void Grocerie_DropDownOpened(object sender, EventArgs e)
        {
            if (GrocerieType.Text == "Meat")
            {
                Grocerie.ItemsSource = meso;
            }

            if (GrocerieType.Text == "Fruit")
            {
                Grocerie.ItemsSource = voce;
            }

            if (GrocerieType.Text == "Vegetable")
            {
                Grocerie.ItemsSource = povrce;
            }

            if (GrocerieType.Text == "Sweets")
            {
                Grocerie.ItemsSource = slatko;
            }

            if (GrocerieType.Text == "Others")
            {
                Grocerie.ItemsSource = ostalo;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((Grocerie.Text != "Grocerie") && (KolicinaUGramima.Text != ""))
            {
                int kolicina;
                bool uspeh = Int32.TryParse(KolicinaUGramima.Text, out kolicina);

                if (uspeh == true)
                {
                    string namirnica = Grocerie.Text;
                    bool postoji = false;
                    for(int i = 0; i < stavkeFrizidera.Count; i++)
                    {
                        if(stavkeFrizidera[i].Ime == namirnica)
                        {
                     
                            stavkeFrizidera[i].Kolicina += kolicina;
                            postoji = true;
                            break;
                              
                        }
                    }

                    if(postoji == false)
                    {
                        Stavka temp = new Stavka(namirnica, kolicina);
                        stavkeFrizidera.Add(temp);
                    }
                    Grocerie.Text = " ";
                    KolicinaUGramima.Text = " ";
                    GrocerieType.Text = " ";
                }
                else
                {
                    MessageBox.Show("Amount has to be a number");
                }
            }
            else
            {
                MessageBox.Show("All fields must be filed");
            }
        }
    }
}
