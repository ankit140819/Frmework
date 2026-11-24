using Frmework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Frmework.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
