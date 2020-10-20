using MyFirstWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private Manager m = new Manager();

        // GET: Customer
        public ActionResult Index()
        {
            var c = m.CustomerGetAll();

            return View(c);
        }

        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var obj = m.CustomerGetById(id.GetValueOrDefault());
            if (obj == null)
                return HttpNotFound();
            else
                return View(obj);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerAddViewModel newItem)
        {
            if(!ModelState.IsValid)
            {
                return View(newItem);
            }

            try
            {
                // TODO: Add insert logic here
                var addedItem = m.CustomerAdd(newItem);

                if(addedItem == null)
                {
                    return View(newItem);
                }
                else
                {
                    return RedirectToAction("Details", new { id = addedItem.CustomerId });
                }
               
            }
            catch
            {
                return View(newItem);
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            // Attempt to fetch the matching object
            var obj = m.CustomerGetById(id.GetValueOrDefault());
            if (obj == null)
                return HttpNotFound();
            else
            {
                // Create and configure an "edit form"
                // Notice that obj is a CustomerBaseViewModel object so
                // we must map it to a CustomerEditContactFormViewModel object
                // Notice that we can use AutoMapper anywhere,
                // and not just in the Manager class.
                var formObj = m.mapper.Map<CustomerBaseViewModel, CustomerEditContactFormViewModel>(obj);
                return View(formObj);
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, CustomerEditContactViewModel model)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("Edit", new { id = model.CustomerId });
            }
            if (id.GetValueOrDefault() != model.CustomerId)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("Index");
            }
            // Attempt to do the update
            var editedItem = m.CustomerEditContactInfo(model);
            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("Edit", new { id = model.CustomerId });
            }
            else
            {
                // Show the details view, which will show the updated data
                return RedirectToAction("Details", new { id = model.CustomerId });
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.CustomerGetById(id.GetValueOrDefault());
            if (itemToDelete == null)
            {
                // Don't leak info about the delete attempt
                // Simply redirect
                return RedirectToAction("Index");
            }
            else
                return View(itemToDelete);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var result = m.CustomerDelete(id.GetValueOrDefault());
            // "result" will be true or false
            // We probably won't do much with the result, because
            // we don't want to leak info about the delete attempt
            // In the end, we should just redirect to the list view
            return RedirectToAction("Index");
        }
    }
}
