using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace planlekcji
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "plan_lekcji.txt";

        private readonly string[] LessonTimes = new string[]
        {
            "7:10 - 7:55", "8:00 - 8:45", "8:50 - 9:35", "9:40 - 10:25", "10:35 - 11:20",
            "11:30 - 12:15", "12:30 - 13:15", "13:25 - 14:10", "14:20 - 15:05",
            "15:10 - 15:55", "16:00 - 16:45"
        };

        private List<LessonRow> Schedule = new List<LessonRow>();

        public MainWindow()
        {
            InitializeComponent();
            InitGrid();
            LoadSchedule();
            PopulateGridWithData();
        }

        private void InitGrid()
        {
            for (int i = Grids.Children.Count - 1; i >= 0; i--)
            {
                UIElement element = Grids.Children[i];
                if (element is Border border && border.Child is TextBlock tb && tb.Name != null && tb.Name.StartsWith("L_"))
                {
                    Grids.Children.Remove(element);
                }
            }

            for (int row = 1; row <= LessonTimes.Length; row++)
            {
                for (int col = 2; col <= 6; col++)
                {
                    TextBlock tb = new TextBlock
                    {
                        Name = $"L_{row}_{GetDayShort(col)}",
                        Text = "",
                        Padding = new Thickness(5),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Background = Brushes.Transparent,
                        IsHitTestVisible = false
                    };

                    Border border = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        Child = tb,
                        Margin = new Thickness(1),
                        Background = Brushes.White,
                        Cursor = Cursors.Hand,
                        IsHitTestVisible = true
                    };

                    border.MouseDown += LessonCell_Click;

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    Grids.Children.Add(border);

                    Debug.WriteLine($"Created and added Border with TextBlock for Row: {row}, Col: {col} ({GetDayShort(col)})");
                }
            }
        }

        private void LoadSchedule()
        {
            Schedule.Clear();

            for (int i = 0; i < LessonTimes.Length; i++)
            {
                Schedule.Add(new LessonRow
                {
                    Number = i + 1,
                    Time = LessonTimes[i],
                    Monday = "",
                    Tuesday = "",
                    Wednesday = "",
                    Thursday = "",
                    Friday = ""
                });
            }

            if (File.Exists(FilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(FilePath);

                    for (int i = 0; i < Math.Min(lines.Length, Schedule.Count); i++)
                    {
                        string line = lines[i];
                        string[] parts = line.Split(';');

                        if (parts.Length == 5)
                        {
                            Schedule[i].Monday = parts[0].Trim();
                            Schedule[i].Tuesday = parts[1].Trim();
                            Schedule[i].Wednesday = parts[2].Trim();
                            Schedule[i].Thursday = parts[3].Trim();
                            Schedule[i].Friday = parts[4].Trim();
                        }
                        else
                        {
                            Debug.WriteLine($"Skipping malformed line in {FilePath}: {line}");
                        }
                    }
                    Debug.WriteLine($"Schedule loaded from {FilePath}. Item count: {Schedule.Count}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd odczytu planu z pliku TXT: {ex.Message}\nPlan zostanie zresetowany.");
                    Debug.WriteLine($"Error loading schedule from TXT: {ex.Message}. Resetting schedule.");
                    SaveSchedule();
                }
            }
            else
            {
                SaveSchedule();
            }
        }

        private void SaveSchedule()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var lessonRow in Schedule)
                {
                    lines.Add($"{lessonRow.Monday};{lessonRow.Tuesday};{lessonRow.Wednesday};{lessonRow.Thursday};{lessonRow.Friday}");
                }
                File.WriteAllLines(FilePath, lines);
                Debug.WriteLine($"Schedule saved to {FilePath}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd zapisu planu do pliku TXT: {ex.Message}");
                Debug.WriteLine($"Error saving schedule to TXT: {ex.Message}");
            }
        }

        private void PopulateGridWithData()
        {
            foreach (UIElement element in Grids.Children)
            {
                if (element is Border border && border.Child is TextBlock tb && tb.Name != null && tb.Name.StartsWith("L_"))
                {
                    var parts = tb.Name.Split('_');
                    if (parts.Length == 3 && int.TryParse(parts[1], out int row))
                    {
                        string day = parts[2];
                        string rawValue = GetLessonValue(row, day);
                        tb.Text = FormatLessonDisplay(rawValue);

                        border.Background = string.IsNullOrWhiteSpace(rawValue)
                            ? Brushes.White
                            : Brushes.MediumAquamarine;

                        tb.Background = Brushes.Transparent;
                    }
                }
            }
            Debug.WriteLine("Grid populated with data.");
        }

        private string FormatLessonDisplay(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "";

            var parts = raw.Split(new[] { ',' }, 2);
            string subject = parts[0].Trim();
            string room = parts.Length > 1 ? parts[1].Trim() : "";

            return !string.IsNullOrEmpty(room)
                ? $"{subject}\ns. {room}"
                : subject;
        }

        private string GetLessonValue(int row, string dayShort)
        {
            if (row < 1 || row > Schedule.Count) return "";

            var lesson = Schedule[row - 1];
            return dayShort switch
            {
                "Pon" => lesson.Monday,
                "Wto" => lesson.Tuesday,
                "Sro" => lesson.Wednesday,
                "Czw" => lesson.Thursday,
                "Pia" => lesson.Friday,
                _ => ""
            };
        }

        private void LessonCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (sender is Border border && border.Child is TextBlock tb)
                {
                    Debug.WriteLine($"Border clicked for TextBlock: {tb.Name}");
                    var parts = tb.Name.Split('_');
                    if (parts.Length != 3)
                    {
                        Debug.WriteLine($"Invalid TextBlock name format in Border: {tb.Name}");
                        return;
                    }

                    if (!int.TryParse(parts[1], out int row))
                    {
                        Debug.WriteLine($"Could not parse row from TextBlock name in Border: {tb.Name}");
                        return;
                    }
                    string dayShort = parts[2];

                    if (row < 1 || row > Schedule.Count)
                    {
                        Debug.WriteLine($"Row out of bounds: {row}");
                        return;
                    }

                    var lessonRow = Schedule[row - 1];
                    string currentValue = GetLessonValue(row, dayShort);

                    string subject = "";
                    string room = "";

                    if (!string.IsNullOrWhiteSpace(currentValue))
                    {
                        var vals = currentValue.Split(new[] { ',' }, 2);
                        subject = vals[0].Trim();
                        if (vals.Length > 1)
                            room = vals[1].Trim();
                    }

                    ShowEditDialog(lessonRow, dayShort, subject, room);
                }
                else
                {
                    Debug.WriteLine("Click sender was not a Border containing a TextBlock, or was null.");
                }
            }
        }

        private void ShowEditDialog(LessonRow lessonRow, string dayShort, string subject, string room)
        {
            Window editWindow = new Window
            {
                Title = $"Edytuj lekcję - {GetDayName(dayShort)}, lekcja nr {lessonRow.Number}",
                Width = 350,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this
            };

            var panel = new StackPanel { Margin = new Thickness(10) };

            panel.Children.Add(new TextBlock { Text = "Przedmiot:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox subjectBox = new TextBox { Text = subject, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(subjectBox);

            panel.Children.Add(new TextBlock { Text = "Sala:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox roomBox = new TextBox { Text = room, Margin = new Thickness(0, 0, 0, 10) };
            panel.Children.Add(roomBox);

            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            Button saveBtn = new Button { Content = "Zapisz", Width = 75, Margin = new Thickness(0, 0, 10, 0) };
            Button deleteBtn = new Button { Content = "Usuń", Width = 75 };

            buttonsPanel.Children.Add(saveBtn);
            buttonsPanel.Children.Add(deleteBtn);

            panel.Children.Add(buttonsPanel);

            editWindow.Content = panel;

            saveBtn.Click += (_, __) =>
            {
                string newSubject = subjectBox.Text.Trim();
                string newRoom = roomBox.Text.Trim();

                string newValue = newSubject;
                if (!string.IsNullOrEmpty(newRoom))
                    newValue += ", " + newRoom;

                switch (dayShort)
                {
                    case "Pon": lessonRow.Monday = newValue; break;
                    case "Wto": lessonRow.Tuesday = newValue; break;
                    case "Sro": lessonRow.Wednesday = newValue; break;
                    case "Czw": lessonRow.Thursday = newValue; break;
                    case "Pia": lessonRow.Friday = newValue; break;
                }

                PopulateGridWithData();
                SaveSchedule();
                editWindow.Close();
                Debug.WriteLine($"Lesson updated: {GetDayName(dayShort)}, No. {lessonRow.Number} to '{newValue}'");
            };

            deleteBtn.Click += (_, __) =>
            {
                switch (dayShort)
                {
                    case "Pon": lessonRow.Monday = ""; break;
                    case "Wto": lessonRow.Tuesday = ""; break;
                    case "Sro": lessonRow.Wednesday = ""; break;
                    case "Czw": lessonRow.Thursday = ""; break;
                    case "Pia": lessonRow.Friday = ""; break;
                }

                PopulateGridWithData();
                SaveSchedule();
                editWindow.Close();
                Debug.WriteLine($"Lesson deleted: {GetDayName(dayShort)}, No. {lessonRow.Number}");
            };

            editWindow.ShowDialog();
        }

        private string GetDayShort(int column)
        {
            return column switch
            {
                2 => "Pon",
                3 => "Wto",
                4 => "Sro",
                5 => "Czw",
                6 => "Pia",
                _ => ""
            };
        }

        private string GetDayName(string shortName)
        {
            return shortName switch
            {
                "Pon" => "Poniedziałek",
                "Wto" => "Wtorek",
                "Sro" => "Środa",
                "Czw" => "Czwartek",
                "Pia" => "Piątek",
                _ => ""
            };
        }
    }

    public class LessonRow
    {
        public int Number { get; set; }
        public string Time { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
    }
}