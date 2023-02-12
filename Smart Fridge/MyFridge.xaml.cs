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
using Newtonsoft.Json;

namespace Smart_Fridge
{
    /// <summary>
    /// Interaction logic for MyFridge.xaml
    /// </summary>
    /// 

    public partial class MyFridge : Window
    {

        public MyFridge()
        {
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
            InitializeComponent();
            var gridView = new GridView();
            this.StuffInMyFridge.View = gridView;
            gridView.Columns.Add(new GridViewColumn { Header = "Name", Width = 100, DisplayMemberBinding = new Binding("Ime") });
            gridView.Columns.Add(new GridViewColumn { Header = "Amount(grams)", Width = 100, DisplayMemberBinding = new Binding("Kolicina") });
            foreach(Stavka a in Stavke)
            {
                StuffInMyFridge.Items.Add(a);
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
            Add win = new Add();
            win.Show();
            this.Close();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            if (StuffInMyFridge.SelectedItem != null)
            {
                var selected = StuffInMyFridge.SelectedItem;
                Stavka temp = JsonConvert.DeserializeObject<Stavka>(JsonConvert.SerializeObject(selected));
                SakriveniLabel.Visibility = Visibility.Visible;
                SakriveniTextBox.Visibility = Visibility.Visible;
                IzbrisiStavku.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("You need to select item for it to be removed");
            }
        }

        private void IzbrisiStavku_Click(object sender, RoutedEventArgs e)
        {
            int kolicina;
            var selected = StuffInMyFridge.SelectedItem;
            Stavka temp = JsonConvert.DeserializeObject<Stavka>(JsonConvert.SerializeObject(selected));

            if (Int32.TryParse(SakriveniTextBox.Text,out kolicina) == true)
            {
                    String path = @"Stavke.csv";
                    List<String> lines = new List<String>();

                    if (File.Exists(path)) 
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[0].Contains(temp.Ime))
                                    {
                                        if (temp.Kolicina > kolicina)
                                        {
                                            split[1] = (temp.Kolicina - kolicina).ToString();
                                            line = String.Join(",", split);
                                        }
                                        else 
                                        {
                                            split[1] = " ";
                                            split[0] = " ";
                                            line = string.Join(",", split);
                                        }
                                    }
                                }

                                lines.Add(line);
                            }
                        }

                        using (StreamWriter writer = new StreamWriter(path, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }
                    }

                    StuffInMyFridge.Items.Clear();

                    List<Stavka> Stavke = new List<Stavka>();

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

                    foreach (Stavka a in Stavke)
                    {
                        StuffInMyFridge.Items.Add(a);
                    }


                }

            }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LabelCalories.Visibility = Visibility.Visible;
            TextBoxInfo.Visibility = Visibility.Visible;
        }
    }
    }

