using ElectronicMail.Model;
using System.Windows;

namespace ElectronicMail.View
{
    public partial class ReadLetterWindow : Window
    {


        public string TitleLetter
        {
            get { return (string)GetValue(TitleLetterProperty); }
            set { SetValue(TitleLetterProperty, value); }
        }

        public static readonly DependencyProperty TitleLetterProperty =
            DependencyProperty.Register("TitleLetter", typeof(string), typeof(ReadLetterWindow));


        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(string), typeof(ReadLetterWindow));



        public ReadLetterWindow(Letter letter)
        {
            InitializeComponent();

            TitleLetter = letter.Title;
            Body = letter.Body;
        }
    }
}
