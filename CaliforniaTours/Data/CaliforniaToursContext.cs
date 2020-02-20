using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaliforniaTours.Data
{
    // Code based on class example PetGrooming, this is to give the database a context and be able to connect it through the web.config file, through here we create the tables.
    public class CaliforniaToursContext : DbContext
    {
        public CaliforniaToursContext() : base("name=CaliforniaToursContext")
        {
        }

        public System.Data.Entity.DbSet<CaliforniaTours.Models.Guest> Guests { get; set; }
        public System.Data.Entity.DbSet<CaliforniaTours.Models.Park> Parks { get; set; }
        public System.Data.Entity.DbSet<CaliforniaTours.Models.TourBooking> TourBookings { get; set; }

    }
}