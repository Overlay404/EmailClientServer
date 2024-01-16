namespace ElectronicMail.Model
{
    public class Letter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Status { get; set; }
    }
}
