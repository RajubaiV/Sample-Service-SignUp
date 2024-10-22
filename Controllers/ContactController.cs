using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PageSignup.Data;
using PageSignup.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace PageSignup.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly ContactAPIDbContext dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService emailService;

        // In-memory store for OTPs; for production, consider using a more secure storage
        private static readonly Dictionary<string, string> OtpStore = new Dictionary<string, string>();
        public ContactController(ContactAPIDbContext dbContext, IEmailService emailService)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
        }

        [HttpPost("sendOTP")]
        public async Task<bool> SendOtpMail(string useremail, string OtpText, string Name)
        {
            var mailrequest = new Mailrequest();
            mailrequest.Email = useremail;
            mailrequest.Subject = "Thanks for registering : OTP";
            mailrequest.Emailbody = GenerateEmailBody(Name, OtpText);

            await this.emailService.SendEmail(mailrequest);

            return true;
        }

        private string GenerateEmailBody(string name, string otptext)        
        {
            string emailbody = "Thanks for Regitser.";
            return emailbody;
        }

        //public async Task<IActionResult> SendOtp([FromBody] EmailSettings emailSettings)
        //{
        //    if (string.IsNullOrEmpty(emailSettings.Email))
        //    {
        //        return BadRequest("Email is required");
        //    }

        //    // Generate OTP and send email logic
        //    var otp = GenerateOtp();
        //    var isSent = await SendOtpMail(emailSettings.Email, otp, emailSettings.DisplayName);

        //    if (isSent)
        //    {
        //        return Ok(new { Message = "OTP sent successfully" });
        //    }
        //    return StatusCode(500, "Failed to send OTP");
        //}

        private string GenerateOtp()
        {
            // Implement OTP generation logic here
            return "123456";
        }

        private async Task<bool> SendOtpEmailAsync(string email, string otp)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("srisneha.chandran02@gmail.com"),
                    Subject = "Your OTP Code",
                    Body = $"Your OTP code is {otp}",
                    IsBodyHtml = false
                };
                mailMessage.To.Add(email);

                //using var smtpClient = new System.Net.Mail.SmtpClient("your-smtp-server");
                using var smtpClient = new System.Net.Mail.SmtpClient("rajubaiv08.gmail.com")
                {
                    Port = 587, // Use the appropriate port (587 for TLS, 465 for SSL, etc.)
                    Credentials = new NetworkCredential("srisneha.chandran02@gmail.com", "raju@baii@2000"),
                    EnableSsl = true // Set to true if using SSL/TLS
                };
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }


        [HttpPost("Authentication")]
        public async Task<IActionResult> GetContacts([FromBody] Contact user)
        {
            var usr = dbContext.Contacts.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            if (usr == null)
            {
                return Ok(new
                {
                    Message = "User Not Found"
                });
            }
            return Ok(new
            {
                Message = "Login Success"
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddContacts([FromBody] ContactRequest contactRequest)
        {
            var contactInfo = contactRequest.Text;
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                UserName = contactInfo.UserName,
                Email = contactInfo.Email,
                Password = contactInfo.Password
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.UserName = updateContactRequest.UserName;
                contact.Email = updateContactRequest.Email;
                contact.Password = updateContactRequest.Password;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }

    }

}
