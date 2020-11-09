using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using TutoringService.data;

namespace TutoringService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void InitSchulstufe()
        {
            foreach (var stufe in Configuration.Levels)
            {
                var rb = new RadioButton { Content = stufe.ToString(), Name = $"btnSchulstufe{stufe}" };
                panelSchulstufe.Children.Add(rb);
            }
        }

        private void InitGegenstand()
        {
            for (int i = 0; i < Configuration.Subjects.Count; i++)
            {
                var rb = new RadioButton { Content = Configuration.Subjects[i], Name = $"btnGegenstand{i}" };
                panelGegenstand.Children.Add(rb);
            }
        }

        private void InitSelection()
        {
            chk4b.IsChecked = true;
            ClickUpdateStudentFilter(this, null);
            cmboNames.SelectedIndex = 0;
        }

        private void UpdateUiForStudent(Student student)
        {
            lblStudentName.Content = student.Name;

            lblTutoringAmount.Content = student.Tutorings.Count().ToString();

            lstTutorings.Items.Clear();
            student.Tutorings.ForEach(tutoring => lstTutorings.Items.Add(tutoring));

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(student.ImagePath, UriKind.Relative);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            imgStudent.Source = image;
        }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Window
            InitSchulstufe();
            InitGegenstand();
            InitSelection();
        }

        private void MenuItemSave(object sender, RoutedEventArgs e)
        {
            var tutorings = StudentsSingleton.Instance.Students
                .Select(student => student.TutoringsCsv())
                .Aggregate((a, b) => {
                    var aggregated = new List<string>();
                    aggregated.AddRange(a);
                    aggregated.AddRange(b);
                    return aggregated;
                })
                .ToList();

            File.WriteAllLines(Configuration.TutoringsPath, tutorings);
        }

        private void MenuItemLoad(object sender, RoutedEventArgs e)
        {
            StudentsSingleton.Instance.LoadData();
            ClickUpdateStudentFilter(this, null);
            UpdateUiForStudent(cmboNames.SelectedItem as Student);
        }

        private void ClickUpdateStudentFilter(object sender, RoutedEventArgs e)
        {
            var currentSelectedStudent = cmboNames.SelectedItem;

            var checked4a = chk4a.IsChecked;
            var checked4b = chk4b.IsChecked;
            var checked4c = chk4c.IsChecked;

            System.Diagnostics.Debug.WriteLine($"checked4a: {checked4a}; checked4b: {checked4b}; checked4c: {checked4c}");

            cmboNames.Items.Clear();
            var filteredStudents = from std in StudentsSingleton.Instance.Students
                                   where ((std.Clazz == "4a") && (checked4a == true)) ||
                                         ((std.Clazz == "4b") && (checked4b == true)) ||
                                         ((std.Clazz == "4c") && (checked4c == true))
                                   select std;
            filteredStudents.ToList().ForEach(student => cmboNames.Items.Add(student));

            cmboNames.SelectedItem = filteredStudents
                .Where(student => student.Equals(currentSelectedStudent) || currentSelectedStudent == null)
                .First();
        }

        private void SelectionChangedComboNames(object sender, SelectionChangedEventArgs e)
        {
            if (cmboNames.SelectedItem != null)
            {
                UpdateUiForStudent(cmboNames.SelectedItem as Student);
            }
        }

        private void ClickAddTutoring(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve currently selected level
                var level = int.Parse(panelSchulstufe.Children.OfType<RadioButton>()
                    .Where(button => button.IsChecked == true)
                    .First().Name.Substring(13));

                // Retrieve currently selected subject
                var subject = panelGegenstand.Children.OfType<RadioButton>()
                    .Where(button => button.IsChecked == true)
                    .First().Name.Substring(13);

                // Instantiate new tutoring
                var tutoring = new Tutoring { Level = level, Subject = subject };

                // Add new tutoring instance to currently selected student tutorings
                (cmboNames.SelectedItem as Student).Tutorings.Add(tutoring);

                // Refresh UI
                UpdateUiForStudent(cmboNames.SelectedItem as Student);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte wählen Sie einen Schüler, sowie Werte für Schulstufe und Gegenstand", "Ungültige Eingabe", MessageBoxButton.OK);
            }
        }
    }
}
