using ElectronicMail.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ElectronicMail.View
{
    public partial class NewLetterWindow : Window
    {
        public NewLetterWindow()
        {
            InitializeComponent();

            SendLetterBTN.MouseDown += async (sender, e) => await SendLetterRequest();
        }

        async Task SendLetterRequest()
        {
            HttpClient httpClient = new HttpClient();

            if (EmailTB.Text == App.UserNow.Email)
            {
                MessageBox.Show("Почта не должна совпадать");
                return;
            }

            var query = "http://127.0.0.1:5000/send_letter";
            var letter = new Letter()
            {
                Title = TitleTB.Text,
                Body = new TextRange(BodyTB.Document.ContentStart, BodyTB.Document.ContentEnd).Text,
                Sender = App.UserNow.Email,
                Recipient = EmailTB.Text,
                Status = "sent"
            };

            var jsonLetter = JsonContent.Create(letter);
            var request = await httpClient.PostAsync(query, jsonLetter);
            AnswerServer answerServer = await request.Content.ReadFromJsonAsync<AnswerServer>();
            if (answerServer.Message != "Letter send")
            {
                MessageBox.Show("Сообщение не отправлено, возможно указанной почты " + letter.Recipient + " не существует");
            }
            else
            {
                MessageBox.Show("Сообщение отправлено");
                this.Close();
            }
        }
    }

    public class AnswerServer
    {
        public string Message { get; set; }
    }
}
