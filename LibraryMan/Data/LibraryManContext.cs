using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryMan.Models;

namespace LibraryMan.Data
{
    public class LibraryManContext : DbContext
    {
        public LibraryManContext (DbContextOptions<LibraryManContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryMan.Models.KsiazkaModel> KsiazkaModel { get; set; } = default!;

        public DbSet<LibraryMan.Models.RecenzjaModel> RecenzjaModel { get; set; } = default!;

        public DbSet<LibraryMan.Models.UzytkownikModel> UzytkownikModel { get; set; } = default!;

        public DbSet<LibraryMan.Models.WydawnictwoModel> WydawnictwoModel { get; set; } = default!;
    }
}
