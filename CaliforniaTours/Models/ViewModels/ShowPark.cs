using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaliforniaTours.Models.ViewModels
{
    public class ShowPark
    {
        //information about an individual park
        public virtual Park park { get; set; }

        //information about multiple bookings
        public List<TourBooking> Bookings { get; set; }
    }
}