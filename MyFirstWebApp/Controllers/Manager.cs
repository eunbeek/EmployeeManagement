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

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerBaseViewModel>();
                cfg.CreateMap<CustomerAddViewModel, Customer>();
                cfg.CreateMap<CustomerBaseViewModel, CustomerEditContactFormViewModel>();
            });

            mapper = config.CreateMapper();

            ds.Configuration.ProxyCreationEnabled = false;


            ds.Configuration.LazyLoadingEnabled = false;
        }


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