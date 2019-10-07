using LinxSolutionProiect.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinxSolutionProiect.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewAll()
        {

            return View(GetAllEmployee());
        }

        IEnumerable<Employee> GetAllEmployee()
        {
            using (DBModel db = new DBModel())
            {
                return db.Employees.ToList<Employee>();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
           
            Employee emp = new Employee();
            if (id != 0)
            {
                
                using (DBModel db = new DBModel())
                {
                    emp = db.Employees.Where(x => x.UserId == id).FirstOrDefault<Employee>();
                }
            }
            return View(emp);
        }
        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
         
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    if (emp.UserId == 0)
                    {
                        var emailExist = db.Employees.Any(x => x.EmailAddress == emp.EmailAddress);
                        if (emailExist)
                        {
                            ModelState.AddModelError("Email", "User with this email already exists");

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            db.Employees.Add(emp);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    Employee emp = db.Employees.Where(x => x.UserId == id).FirstOrDefault<Employee>();
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");

            }

        }


    }
}