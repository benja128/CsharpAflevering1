using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace GradedExercise1
{

    public class Person : INotifyPropertyChanged
    {

        private string id;
        private string name;
        private int age;
        private int score;

        public Person(string id, string name, int age, int score)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.score = score;
        }

        public string Id
        {
            set
            {
                id = value;
                NotifyPropertyChanged(nameof(id));
            }
            get
            {
                return id;
            }
        }

        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(name));
            }
            get
            {
                return name;
            }
        }

        public int Age
        {
            set
            {
                age = value;
                NotifyPropertyChanged(nameof(age));
            }
            get
            {
                return age;
            }
        }

        public int Score
        {
            set
            {
                score = value;
                NotifyPropertyChanged(nameof(score));
            }
            get
            {
                return score;
            }
        }

        public string ListBoxToString
        {
            get
            {
                return $"Id: {id} Name: {Name}";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }




    public partial class MainWindow : Window
    {

        private ObservableCollection<Person> persons;

        public MainWindow()
        {
            InitializeComponent();
            persons = new ObservableCollection<Person>();
            listbox.ItemsSource = persons;
            dataPanel.DataContext = persons;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            DateTime timeStamp = DateTime.Now;

            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = ".txt";

                if (file.ShowDialog() == true)
                {
                    var fileStream = file.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string fileContent = reader.ReadToEnd();
                   
                        try
                        {
                           foreach(var p in fileContent.Split('\n'))
                           {
                            var pString = p.Split(';');
                            string id = pString[0];
                            string name = pString[1];
                            int age = int.Parse(pString[2]);
                            int score = int.Parse(pString[3]);
                            persons.Add(new Person(id, name, age, score));
                           }
                           statusLabel.Content = $"Last loaded at {timeStamp} - Contains {persons.Count} persons";
                        }
                        catch (Exception)
                        {
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Error;
                            string caption = "Unreadable file content";
                            string text = "File content format is unreadable";
                            MessageBox.Show(text,caption,button,icon);
                        statusLabel.Content = "Failed to load file";
                        }
                    }
                }
        }

        private void Exit_app(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
