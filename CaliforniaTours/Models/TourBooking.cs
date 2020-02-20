using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaliforniaTours.Models
{
    public class TourBooking
    {
        /*
           A Booking is an agreement between the guest and the company to provide a tour to a certain park.
           things to describe a TourBooking:
               - ID
               - Guest
               - Park
               - Date
               - Cost

           A Booking must reference a Guest and a Park.
       */
        [Key]
        public int BookingID { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        //Cost is in USD and uses integers because the cost is in cents (i.e 15000 cents  = 150 USD)

        //Representing the Relationship Many bookings to one Park
        public int ParkID { get; set; }
        [ForeignKey("ParkID")]
        public virtual Park Park { get; set; }

        //Representing the Relationship Many bookings to one Guest
        public ICollection<Guest> Guests { get; set; }


    }
}