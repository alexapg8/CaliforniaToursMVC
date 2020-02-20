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
    public class ParkController : Controller
    {
        private CaliforniaToursContext db = new CaliforniaToursContext();

        // GET parks list
        public ActionResult List(string parksearch)
        {
            // Debug.WriteLine("The search key is " + parksearch);

            string query = "Select * from Parks";
            //We create a search bar for the user to use if they already have a park in mind
            if (parksearch != "")
            {
                //We modify the query to include what the user inputs into the search bar
                query = query + " where ParkName like '%" + parksearch + "%'";
                //Debug.WriteLine("The query is " + query);
            }

            List<Park> parks = db.Parks.SqlQuery(query).ToList();
            return View(parks);

        }
        // GET details about one park
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            Park park = db.Parks.SqlQuery("select * from Parks where parkid=@ParkID", new SqlParameter("@ParkID", id)).FirstOrDefault();
            if (park == null)
            {
                return HttpNotFound();
            }

            // We use the ShowPark viewmodel so that we can view all the bookings one park has
            string query = "select * from TourBookings inner join Parks on TourBookings.ParkID=Parks.ParkID where Parks.ParkID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            List<TourBooking> bookings = db.TourBookings.SqlQuery(query, param).ToList();

            ShowPark viewmodel = new ShowPark();
            viewmodel.park = park;
            viewmodel.Bookings = bookings;

            return View(viewmodel);

        }
        public ActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Add(string ParkName, string DepartureTime, string ReturningTime)
        {

            //Debug.WriteLine("Want to create a park with  name " + ParkName ) ;

            // We create the query to insert the values we will get into the database
            string query = "insert into Parks (ParkName, DepartureTime, ReturningTime) values (@ParkName,@DepartureTime,@ReturningTime)";
            SqlParameter[] sqlparams = new SqlParameter[3]; 

            sqlparams[0] = new SqlParameter("@ParkName", ParkName);
            sqlparams[1] = new SqlParameter("@DepartureTime", DepartureTime);
            sqlparams[2] = new SqlParameter("@ReturningTime", ReturningTime);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            //we return to the list to see the new added park
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //need information about a particular Park
            Park selectedpark = db.Parks.SqlQuery("select * from Parks where ParkID=@ParkID", new SqlParameter("@ParkID", id)).FirstOrDefault();

            return View(selectedpark);
        }

        [HttpPost]
        public ActionResult Update(string ParkName, string DepartureTime, string ReturningTime, int id)
        {

            // Debug.WriteLine("I am trying to edit a park's name to "+ParkName+" );
            // query to update information in the database
            string query = "update Parks set ParkName=@ParkName, DepartureTime=@DepartureTime, ReturningTime=@ReturningTime where ParkID=@ParkID";

            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@ParkName", ParkName);
            sqlparams[1] = new SqlParameter("@DepartureTime", DepartureTime);
            sqlparams[2] = new SqlParameter("@ReturningTime", ReturningTime);
            sqlparams[3] = new SqlParameter("@ParkID", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

       
        public ActionResult Delete(int id)
        {
            // Show confirmation message with park info
            Park park = db.Parks.SqlQuery("select * from Parks where ParkID=@ParkID", new SqlParameter("@ParkID", id)).FirstOrDefault();

            return View(park);
        }
       
        public ActionResult DeleteF(int id)
        {
            // Confirm delete of specific park
            //query to delete from the database the park selected

            string query = "delete from Parks where ParkID=@ParkID";
            SqlParameter sqlparam = new SqlParameter("@ParkID", id);

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