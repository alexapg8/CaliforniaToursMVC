using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaliforniaTours.Models.ViewModels
{
    public class ShowGuest
    {
        //information about an individual guest
        public virtual Guest guest { get; set; }

        //information about multiple bookings
        public List<TourBooking> Bookings { get; set; }
    }
}