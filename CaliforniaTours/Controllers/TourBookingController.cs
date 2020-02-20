using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaliforniaTours.Data;
using CaliforniaTours.Models;
using CaliforniaTours.Models.ViewModels;
using System.Diagnostics;

namespace CaliforniaTours.Controllers
{
    public class TourBookingController : Controller
    {
        private CaliforniaToursContext db = new CaliforniaToursContext();

        // GET bookings list
        public ActionResult List()
        {

            List<TourBooking> bookings = db.TourBookings.SqlQuery("Select * from TourBookings").ToList();
            return View(bookings);

        }
        // GET details about one booking
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            TourBooking tourbooking = db.TourBookings.SqlQuery("select * from TourBookings where bookingid=@BookingID", new SqlParameter("@BookingID", id)).FirstOrDefault();
            if (tourbooking == null)
            {
                return HttpNotFound();
            }


          
            string guest_query = "select * from Guests inner join TourBookingGuests on Guests.GuestID = TourBookingGuests.Guest_GuestID where TourBookingGuests.TourBooking_BookingID=@BookingID";
            var g_parameter = new SqlParameter("@BookingID", id);
            List<Guest> knownguests = db.Guests.SqlQuery(guest_query, g_parameter).ToList();

            string all_guests_query = "select * from Guests";
            List<Guest> AllGuests = db.Guests.SqlQuery(all_guests_query).ToList();

            // We use the AddParkGuest viewmodel so that we can show the guests that are on that booking and also so that we can see the dropdown list of guests
            // and add a guest to a booking if we want to
            AddParkGuest viewmodel = new AddParkGuest();
            viewmodel.Booking = tourbooking;
            viewmodel.Guests = knownguests;
            viewmodel.Add_Guest = AllGuests;

            return View(viewmodel);

        }
        public ActionResult Add()
        {
            // On the tour booking we can choose a guest and a park to make a booking, we get a list of both of these models.
            List<Guest> Guests = db.Guests.SqlQuery("select * from Guests").ToList();
            List<Park> Parks = db.Parks.SqlQuery("select * from Parks").ToList();

            //We use the AddParkGuest viewmodel to show the guessts and parks and be able to add them to the booking.
            AddParkGuest AddParkGuestViewModel = new AddParkGuest();
            AddParkGuestViewModel.Parks = Parks;
            AddParkGuestViewModel.Guests = Guests;

            return View(AddParkGuestViewModel);
        }
        
        [HttpPost]
        public ActionResult Add(DateTime Date, int Cost, int ParkID)
        {
            //Debug.WriteLine("Want to create a booking with a cost of " + Cost ) ;

            // We create the query to insert the values we will get into the database
            string query = "insert into TourBookings (Date, Cost, ParkID) values (@Date,@Cost,@ParkID)";
            SqlParameter[] sqlparams = new SqlParameter[3]; 

            sqlparams[0] = new SqlParameter("@Date", Date);
            sqlparams[1] = new SqlParameter("@Cost", Cost);
            sqlparams[2] = new SqlParameter("@ParkID", ParkID);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of bookings where our new booking will be
            return RedirectToAction("List");
        }

        //Now we want to add a guest to abooking already made on the show page
        [HttpPost]
        public ActionResult AddGuest(int id, int GuestID)
        {
            // Debug.WriteLine("guest id is" + GuestID + " and booking id is " + id);

            //We first check if the booking already has a guest, if not we get the list
            string check_query = "select * from Guests inner join TourBookingGuests on TourBookingGuests.Guest_GuestID = Guests.GuestID where Guest_GuestID=@GuestID and TourBooking_BookingID=@BookingID";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@BookingID", id);
            check_params[1] = new SqlParameter("@GuestID", GuestID);
            List<Guest> guests = db.Guests.SqlQuery(check_query, check_params).ToList();

            //Only adds a guest if they aren't a guest yet.
            if (guests.Count <= 0)
            {

                //query to insert the values into our bridging table between the bookings and guests
                string query = "insert into TourBookingGuests (Guest_GuestID, TourBooking_BookingID) values (@GuestID, @BookingID)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@BookingID", id);
                sqlparams[1] = new SqlParameter("@GuestID", GuestID);

                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("Show/" + id);

        }
        
        //Now if we want to delete a guest from a booking on the show page
        [HttpGet]
        public ActionResult DeleteGuest(int id, int GuestID)
        {

            //Debug.WriteLine("guest id is" + GuestID + " and booking id is " + id);

            //We run the wuery to delete it from the bridging table and this way we only remove the guest from the booking 
            //but we do not delete the guest from the guests table
            string query = "delete from TourBookingGuests where Guest_GuestID=@GuestID and TourBooking_BookingID=@BookingID";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@GuestID", GuestID);
            sqlparams[1] = new SqlParameter("@BookingID", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + id);
        }
        public ActionResult Update(int id)
        {
            //need information about a particular booking
            TourBooking selectedbooking = db.TourBookings.SqlQuery("select * from TourBookings where BookingID=@BookingID", new SqlParameter("@BookingID", id)).FirstOrDefault();
            List<Guest> Guests = db.Guests.SqlQuery("select * from Guests").ToList();
            List<Park> Parks = db.Parks.SqlQuery("select * from Parks").ToList();

            //We use the AddParkGuest viewmodel to show the guests and parks in  a dropdown.
            AddParkGuest AddParkGuestViewModel = new AddParkGuest();
            AddParkGuestViewModel.Booking = selectedbooking;
            AddParkGuestViewModel.Parks = Parks;
            AddParkGuestViewModel.Guests = Guests;

            return View(AddParkGuestViewModel);
        }

        [HttpPost]
        public ActionResult Update(DateTime Date, int Cost, int GuestID, int ParkID, int id)
        {

            //Debug.WriteLine("I want to edit a bookings cost to "+Cost );

            //We create the query to update the bookings table
            string query = "update TourBookings set Date=@Date, Cost=@Cost, ParkID=@ParkID where BookingID=@BookingID";

            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@Date", Date);
            sqlparams[1] = new SqlParameter("@Cost", Cost);
            sqlparams[2] = new SqlParameter("@ParkID", ParkID);
            sqlparams[3] = new SqlParameter("@BookingID", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            //It takes us back to the list where we can see any updates
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            //We show a specific delete confirmation with the booking information
            TourBooking booking = db.TourBookings.SqlQuery("select * from TourBookings where BookingID=@BookingID", new SqlParameter("@BookingID", id)).FirstOrDefault();

            return View(booking);
        }
       
        public ActionResult DeleteF(int id)
        {
            //Once the user confirms they wantt to delete the booking
            //We run the query to delete the booking
            string query = "delete from TourBookings where BookingID=@BookingID";
            SqlParameter sqlparam = new SqlParameter("@BookingID", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);
            
            return RedirectToAction("List");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}