using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ManyToMany.Database;
using ManyToMany.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ManyToMany.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var customers = _context.Customers
                .Include(c => c.CustomerDevices)
                .ThenInclude(cd => cd.Device)
                .ToList();
            return View(customers);
        }

        public IActionResult Create()
        {
            CustomerViewModel customerVM = new CustomerViewModel
            {
                SelectedDevices = new List<int>(),
                Devices = new SelectList(_context.Devices, "Id", "DeviceType")
            };
            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    CustomerDisplayName = customerVM.CustomerDisplayName
                };

                customer.CustomerDevices = new List<CustomerDevice>();

                foreach (var devId in customerVM.SelectedDevices)
                {
                    customer.CustomerDevices.Add(new CustomerDevice
                    {
                        Customer = customer,
                        DeviceId = devId
                    });
                }

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(customerVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.Include(c => c.CustomerDevices).SingleOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerVM = new CustomerViewModel
            {
                Id = customer.Id,
                CustomerDisplayName = customer.CustomerDisplayName,
                SelectedDevices = customer.CustomerDevices.Select(cd => cd.DeviceId).ToList(),
                Devices = new SelectList(_context.Devices, "Id", "DeviceType")
            };

            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerVM)
        {
            if (id != customerVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.Include(c => c.CustomerDevices).SingleOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound();
                }

                customer.CustomerDisplayName = customerVM.CustomerDisplayName;

                foreach (var devId in customerVM.SelectedDevices)
                {
                    var customerDevice = customer.CustomerDevices.FirstOrDefault(cd => cd.DeviceId == devId);
                    if (customerDevice != null)
                    {
                        customer.CustomerDevices.Remove(customerDevice);
                    }
                    else
                    {
                        customer.CustomerDevices.Add(new CustomerDevice
                        {
                            CustomerId = customer.Id,
                            DeviceId = devId
                        });
                    }
                }

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customerVM);
        }
    }
}
