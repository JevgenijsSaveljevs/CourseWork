using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class CustomerBussines
    {
        public void CreateCustomer()
        {
            var cust = new Entities<Customer>();
            //cust.Add(
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var cust = new Entities<Customer>();
            return cust.GetAll().ToList();
        }
    }
}
