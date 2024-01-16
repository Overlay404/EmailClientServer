using ElectronicMail.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicMail.View
{
    public partial class AuthWindow : Window
    {
        HttpClient httpClient = new HttpClient();
        bool IsGet = true;

        public AuthWindow()
        {
            InitializeComponent();

            AuthBTN.Click += (sender, e) => CreateOrGetUser();
            CreateUserBTN.MouseDown += (sender, e) => ChangeTypeForm();
        }


        async Task GetUser()
        {
            try
            {
                var query = "http://127.0.0.1:5000/get_user?login=" + LoginTB.Text + "&password=" + PasswordTB.Text;
                object data = await httpClient.GetFromJsonAsync(query, typeof(User));
                if (data != null)
                {
                    App.UserNow = data as User;
                    new MainWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            catch { }
        }

        async Task CreateUser()
        {
            try
            {
                var query = "http://127.0.0.1:5000/create_user";
                var user = new User
                {
                    Email = EmailTB.Text,
                    Login = LoginTB.Text,
                    Password = PasswordTB.Text
                };

                var jsonLetter = JsonContent.Create(user);
                var request = await httpClient.PostAsync(query, jsonLetter);
                AnswerServer answerServer = await request.Content.ReadFromJsonAsync<AnswerServer>();
                var codeServer = request.StatusCode;
                if (codeServer == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show(answerServer.Message);
                }
                else
                {
                    App.UserNow = user;
                    new MainWindow().Show();
                    this.Close();
                }
            }
            catch { }
        }

        async void CreateOrGetUser()
        {
            if (IsGet)
            {
                await GetUser();
            }
            else
            {
                await CreateUser();
            }
        }

        void ChangeTypeForm()
        {
            if (IsGet)
            {
                IsGet = false;
                EmailSP.Visibility = Visibility.Visible;
                AuthBTN.Content = "Зарегистрироваться";
                CreateUserBTN.Text = "У меня есть аккаунт";
            }
            else
            {
                IsGet = true;
                EmailSP.Visibility = Visibility.Collapsed;
                AuthBTN.Content = "Войти";
                CreateUserBTN.Text = "Создать аккаунт";
            }
        }
    }
}
