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
    public class GuestController : Controller
    {
        private CaliforniaToursContext db = new CaliforniaToursContext();

        // GET guest list
        public ActionResult List(string guestsearch)
        {
            // Debug.WriteLine("The search key is " + guestsearch);

            string query = "Select * from Guests";
            //We create a search bar for the user to look for a guest

            if (guestsearch != "")
            {
                //We modify the query to include what the user inputs into the search bar
                query = query + " where FirstName like '%" + guestsearch + "%' OR LastName like'%" + guestsearch + "%'";
                //Debug.WriteLine("The query is " + query);
            }

            List<Guest> guests = db.Guests.SqlQuery(query).ToList();
            return View(guests);

        }
        // GET details about one guest
        public ActionResult Show(int id)
        {
          

            string query = "select * from Guests where GuestID = @GuestID";
            var parameter = new SqlParameter("@GuestID", id);
            Guest guest = db.Guests.SqlQuery(query, parameter).FirstOrDefault();

            string query_bookings = "select * from TourBookings inner join TourBookingGuests on TourBookings.BookingID=TourBookingGuests.TourBooking_BookingID where TourBookingGuests.Guest_GuestID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            List<TourBooking> bookings = db.TourBookings.SqlQuery(query_bookings, param).ToList();

            // We use the ShowGuest viewmodel so that we can view all the bookings one guest has
            ShowGuest viewmodel = new ShowGuest();
            viewmodel.guest = guest;
            viewmodel.Bookings = bookings;

            return View(viewmodel);
            
        }
        public ActionResult Add()
        {
            return View();
        }

        //URL: /Guest/Add
        [HttpPost]
        public ActionResult Add(string FirstName, string LastName, string Email, string PhoneNumber)
        {
           
            //Debug.WriteLine("Want to create a guest with first name " + FirstName + " and last name " + LastName ) ;

            // We create the query to insert the values we will get into the database
            string query = "insert into Guests (FirstName, LastName, Email, PhoneNumber) values (@FirstName,@LastName,@Email,@PhoneNumber)";
            SqlParameter[] sqlparams = new SqlParameter[4]; 

            sqlparams[0] = new SqlParameter("@FirstName", FirstName);
            sqlparams[1] = new SqlParameter("@LastName", LastName);
            sqlparams[2] = new SqlParameter("@Email", Email);
            sqlparams[3] = new SqlParameter("@PhoneNumber", PhoneNumber);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            // we return to the list of guests where our new guest will be added.
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //To be able to update we need information about a particular guest
            Guest selectedguest = db.Guests.SqlQuery("select * from Guests where GuestID=@GuestID", new SqlParameter("@GuestID", id)).FirstOrDefault();

            return View(selectedguest);
        }

        [HttpPost]
        public ActionResult Update(string FirstName, string LastName, string Email, string PhoneNumber, int id)
        {

            //Debug.WriteLine("I want to change a guests first name to " + FirstName + " and their last name to " + LastName.ToString());
            //we create the query to update the infromation on the database.
            string query = "update Guests set FirstName=@FirstName, LastName=@LastName, Email=@Email, PhoneNumber=@PhoneNumber where GuestID=@GuestID";
           
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@FirstName", FirstName);
            sqlparams[1] = new SqlParameter("@LastName", LastName);
            sqlparams[2] = new SqlParameter("@Email", Email);
            sqlparams[3] = new SqlParameter("@PhoneNumber", PhoneNumber);
            sqlparams[4] = new SqlParameter("@GuestID", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            //We show a specific delete confirmation with the guest information
            Guest guest = db.Guests.SqlQuery("select * from Guests where GuestID=@GuestID", new SqlParameter("@GuestID", id)).FirstOrDefault();

            return View(guest);
        }

        public ActionResult DeleteF(int id)
        {
            //Once the user confirms they wantt to delete the guest
            //We run the query to delete the guest.
            string query = "delete from Guests where GuestID=@GuestID";
            SqlParameter sqlparam = new SqlParameter("@GuestID", id);

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