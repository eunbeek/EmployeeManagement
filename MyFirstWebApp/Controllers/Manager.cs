// **************************************************
// WEB524 Project Template V1 == b9210814-4f17-4492-a60c-272aa5dc9699
// Do not change this header.
// **************************************************

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyFirstWebApp.EntityModels;
using MyFirstWebApp.Models;

namespace MyFirstWebApp.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private DataContext ds = new DataContext();

        // AutoMapper instance
        public IMapper mapper;

        public Manager()
        {
            // If necessary, add more constructor code here...

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();
                cfg.CreateMap<Customer, CustomerBaseViewModel>();
                cfg.CreateMap<CustomerAddViewModel, Customer>();
                cfg.CreateMap<CustomerBaseViewModel, CustomerEditContactFormViewModel>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // Add your methods and call them from controllers.  Use the suggested naming convention.
        // Ensure that your methods accept and deliver ONLY view model objects and collections.
        // When working with collections, the return type is almost always IEnumerable<T>.

        public IEnumerable<CustomerBaseViewModel> CustomerGetAll()
        {
            return mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerBaseViewModel>>(ds.Customers);
        }

        public CustomerBaseViewModel CustomerGetById(int id)
        {
            var obj = ds.Customers.Find(id);
            return obj == null ? null : mapper.Map<Customer, CustomerBaseViewModel>(obj);
        }

        public CustomerBaseViewModel CustomerAdd(CustomerAddViewModel newCustomer)
        {
            var addedItem = ds.Customers.Add(mapper.Map<CustomerAddViewModel, Customer>(newCustomer));
            ds.SaveChanges();

            return addedItem == null ? null : mapper.Map<Customer, CustomerBaseViewModel>(addedItem);
        }

        public CustomerBaseViewModel CustomerEditContactInfo(CustomerEditContactViewModel customer)
        {
            // Attempt to fetch the object.
            var obj = ds.Customers.Find(customer.CustomerId);
            if (obj == null)
            {
                // Customer was not found, return null.
                return null;
            }
            else
            {
                // Customer was found. Update the entity object
                // with the incoming values then save the changes.
                ds.Entry(obj).CurrentValues.SetValues(customer);
                ds.SaveChanges();
                // Prepare and return the object.
                return mapper.Map<Customer, CustomerBaseViewModel>(obj);
            }
        }

        public bool CustomerDelete(int id)
        {
            // Attempt to fetch the object to be deleted
            var itemToDelete = ds.Customers.Find(id);
            if (itemToDelete == null)
                return false;
            else
            {
                // Remove the object
                ds.Customers.Remove(itemToDelete);
                ds.SaveChanges();
                return true;
            }
        }
    }
}