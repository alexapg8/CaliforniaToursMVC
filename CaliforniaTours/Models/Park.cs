using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaliforniaTours.Models
{
    public class Park
    {
        /*
          A Park is the place that the guest will book to go.
          things to describe a park:
              - ID
              - Name
              - Departure Time
              - Returning Time

          A Park has multiple bookings.
      */
        [Key]
        public int ParkID { get; set; }
        public string ParkName { get; set; }
        public string DepartureTime { get; set; }
        // I'm using string because I want to set a value of just the time, it won't need to grab it for the system and the date is not needed.
        public string ReturningTime { get; set; }
    


        //Representing the Relationship One Park to many Bookings.
        public ICollection<TourBooking> Bookings { get; set; }
    }
}