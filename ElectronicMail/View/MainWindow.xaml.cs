using ElectronicMail.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicMail.View
{
    public partial class MainWindow : Window
    {
        HttpClient httpClient = new HttpClient();

        public static MainWindow Instance { get; private set; }

        public List<Model.Letter> Letters
        {
            get { return (List<Model.Letter>)GetValue(LettersProperty); }
            set { SetValue(LettersProperty, value); }
        }

        public static readonly DependencyProperty LettersProperty =
            DependencyProperty.Register("Letters", typeof(List<Model.Letter>), typeof(MainWindow));



        public MainWindow()
        {
            InitializeComponent();

            IncomingBTN.Checked += async (sender, e) => await GetReceivedLetters();
            PostedBTN.Checked += async (sender, e) => await GetSentLetters();
            LettersLV.SelectionChanged += (sender, e) => OpenReadLetterWindow();
            SendLetterBTN.MouseDown += (sender, e) => OpenNewLetterWindow();
            IncomingBTN.IsChecked = true;
            Instance = this;
        }

        async Task GetReceivedLetters()
        {
            if (!(bool)IncomingBTN.IsChecked)
            {
                return;
            }

            var query = "http://127.0.0.1:5000/get_received_letters?email=" + App.UserNow.Email;
            object data = await httpClient.GetFromJsonAsync(query, typeof(List<Letter>));
            if (data != null)
            {
                Letters = data as List<Letter>;
            }
        }

        async Task GetSentLetters()
        {
            if (!(bool)PostedBTN.IsChecked)
            {
                return;
            }

            var query = "http://127.0.0.1:5000/get_sent_letters?email=" + App.UserNow.Email;
            object data = await httpClient.GetFromJsonAsync(query, typeof(List<Letter>));
            if (data != null)
            {
                Letters = data as List<Letter>;
            }
        }

        void OpenReadLetterWindow()
        {
            if (LettersLV.SelectedItem == null)
            {
                return;
            }

            new ReadLetterWindow(LettersLV.SelectedItem as Letter).Show();
        }

        void OpenNewLetterWindow()
        {
            new NewLetterWindow().ShowDialog();
        }
    }
}
