using LearnMVC.Filters;
using LearnMVC.Models;
using LearnMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMVC.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // GET: Test
        public string GetString()
        {
            return "Hello World is old now. It&rsquo;s time for wassup bro ;)";
        }

        [Authorize]
        [HeaderFooterFilter]
        public ActionResult Index()
        {
            #region Section 1
            //return View();

            #endregion

            #region Section 3
            //Employee emp = new Employee();
            //emp.FirstName = "Sukesh";
            //emp.LastName = "Marla";
            //emp.Salary = 20000;
            ////ViewData["Employee"] = emp;
            ////ViewBag.Employee = emp;


            //EmployeeViewModel vmEmp = new EmployeeViewModel();
            //vmEmp.EmployeeName = emp.FirstName + " " + emp.LastName;
            //vmEmp.Salary = emp.Salary.ToString("C");
            //if (emp.Salary > 15000)
            //{
            //    vmEmp.SalaryColor = "yellow";
            //}
            //else
            //{
            //    vmEmp.SalaryColor = "green";
            //}
            //vmEmp.UserName = "Admin";

            //return View(vmEmp);
            #endregion

            #region Section Four -- Employee List View Model

            EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel();
            EmployeeBusinessLayer empBusinessLayer = new EmployeeBusinessLayer();

            List<Employee> employees = empBusinessLayer.GetEmployees();
            List<EmployeeViewModel> empViewModels = new List<EmployeeViewModel>();
            foreach (Employee emp in employees)
            {
                EmployeeViewModel empviewModel = new EmployeeViewModel();
                empviewModel.EmployeeName = emp.FirstName + " " + emp.LastName;
                empviewModel.Salary = emp.Salary != null ? emp.Salary.ToString() : null;
                if (emp.Salary > 15000)
                {
                    empviewModel.SalaryColor = "yellow";
                }
                else
                {
                    empviewModel.SalaryColor = "green";
                }
                empViewModels.Add(empviewModel);
            }
            //employeeListViewModel.UserName = User.Identity.Name;
            employeeListViewModel.Employees = empViewModels;
            //employeeListViewModel.FooterData = new FooterViewModel();
            //employeeListViewModel.FooterData.CompanyName = "Horizontal India";
            //employeeListViewModel.FooterData.Year = DateTime.Now.Year.ToString();
            return View(employeeListViewModel);
            #endregion

        }

        public Customer GetCustomer()
        {
            Customer c = new Customer();
            c.CustomerName = "Customer 1";
            c.Address = "Address1";
            return c;
        }

        [AdminFilter]
        [HeaderFooterFilter]
        public ActionResult AddNew()
        {
            CreateEmployeeViewModel employeeListViewModel = new CreateEmployeeViewModel();
            //employeeListViewModel.FooterData = new FooterViewModel();
            //employeeListViewModel.FooterData.CompanyName = "StepByStepSchools";//Can be set to dynamic value
            //employeeListViewModel.FooterData.Year = DateTime.Now.Year.ToString();
            //employeeListViewModel.UserName = User.Identity.Name; //New Line
            return View("CreateEmployee", employeeListViewModel);
        }

        [AdminFilter]
        [ValidateAntiForgeryToken]
        [HeaderFooterFilter]
        public ActionResult SaveEmployee(Employee e, string btnSubmit)
        {
            switch (btnSubmit)
            {
                case "Save Employee":
                    if (ModelState.IsValid)
                    {
                        EmployeeBusinessLayer empBusinessLayer = new EmployeeBusinessLayer();
                        empBusinessLayer.SaveEmployee(e);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        CreateEmployeeViewModel vm = new CreateEmployeeViewModel();
                        vm.FirstName = e.FirstName;
                        vm.LastName = e.LastName;
                        if (e.Salary.HasValue)
                        {
                            vm.Salary = e.Salary.ToString();
                        }
                        else
                        {
                            vm.Salary = ModelState["Salary"].Value.AttemptedValue;
                        }
                        return View("CreateEmployee", vm);
                    }
                case "Cancel":
                    return RedirectToAction("Index");
            }
            return new EmptyResult();
        }

        [ChildActionOnly]
        public ActionResult GetAddNewLink()
        {
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                return PartialView("AddNewLink");
            }
            else
            {
                return new EmptyResult();
            }
        }

    }
    public class Customer
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return this.CustomerName + " | " + this.Address;
        }
    }


}