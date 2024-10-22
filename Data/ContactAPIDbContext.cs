using Microsoft.EntityFrameworkCore;
using PageSignup.Models;

namespace PageSignup.Data
{
    public class ContactAPIDbContext : DbContext
    {
        public ContactAPIDbContext(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<Contact> Contacts { get; set; }
    }
}
