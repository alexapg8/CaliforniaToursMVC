using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaliforniaTours.Models.ViewModels
{
    public class AddParkGuest
    {
        public TourBooking Booking{ get; set; }
        // to reference the guests list
        public List<Guest> Guests { get; set; }
        // to reference the parks list
        public List<Park> Parks { get; set; }
        // to reference a guest list on our AddGuest method
        public List<Guest> Add_Guest { get; set; }
    }
}