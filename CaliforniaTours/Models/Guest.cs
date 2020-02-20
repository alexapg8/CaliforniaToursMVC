using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CaliforniaTours.Models
{
    public class Guest
    {
        /*
          A Guest is the person who goes on the booking.
          things to describe a guest:
              - ID
              - First Name
              - Last Name
              - Email
              - Phone Number

          A Guest has multiple bookings.
      */
        [Key]
        public int GuestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }



        //Representing the Relationship One Guest to many Bookings.
        public ICollection<TourBooking> Bookings { get; set; }
    }
}