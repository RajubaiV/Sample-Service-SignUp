namespace PageSignup.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class ContactRequest
    {
        public Contact? Text { get; set; }
    }

    public class EmailSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }        
        public string Host { get; set; }        
        public string DisplayName { get; set; }        
        public int Port { get; set; }
    }

    public class Mailrequest
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Emailbody { get; set; }

    }
}
