using EmpApp.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmpApp.Controllers
{
    public class LeaveController:Controller
    {
        private readonly AppDbContext context;

        public LeaveController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
           var leaves = (from e in context.Employees
                          join l in context.Leaves
                          on e.Id equals l.Employee.Id
                          select new
                          {
                              e.Name,
                              l.StartDate,
                              l.EndDate,
                              l.ApprovalStatus,
                              l.LeaveId
                          }).ToList();
            List<LeaveViewModel> LeaveViewModel = new List<LeaveViewModel>();
            foreach (var l in leaves){
                LeaveViewModel leave = new LeaveViewModel();
                leave.EmployeeName = l.Name;
                leave.StartDate = l.StartDate;
                leave.EndDate = l.EndDate;
                leave.ApprovalStatus = l.ApprovalStatus;
                leave.LeaveId = l.LeaveId;
                LeaveViewModel.Add(leave);

            }
            return View(LeaveViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            List<String> employee = context.Employees.Select(e => e.Name).ToList();
            ViewData["Employee"] = employee;
            return View();
        }

        [HttpPost]
        public IActionResult Create(LeaveViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //context.Add(model);
            //context.SaveChanges();
            var emp = context.Employees.Include("Leaves")
                       .Where(e => e.Name.Equals(model.EmployeeName))
                        .FirstOrDefault();
            Debug.WriteLine("emppp" + emp.Age);
            Leave leave = new Leave() {
                StartDate = model.StartDate, EndDate = model.EndDate, ApprovalStatus = model.ApprovalStatus };
            emp.Leaves.Add(leave);
            context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var emp = context.Leaves.Find(id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Leave model)
        {
            if (!ModelState.IsValid) return View(model);
            Debug.WriteLine("leaveId:" + model.LeaveId);
            context.Leaves.Update(model);
            context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var leave = context.Leaves.Find(id);
            context.Leaves.Remove(leave);
            context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
