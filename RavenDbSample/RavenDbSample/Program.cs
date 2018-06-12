using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;

namespace RavenDbSample
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteAll();
            using (var documentStore = new DocumentStore { Url = "http://localhost:8080" })
            {
                documentStore.Initialize();
                using (var session = documentStore.OpenSession("Shop"))
                {
                    var amm = new Customer { Name = "Aydin Mir Mohammadi", Street = "Hebelstraße 15", ZipCode = "76133", City = "Karlsruhe" };
                    session.Store(amm);

                    var cup = new Product { Title = "Cup", Price = 10.5 };
                    var phone = new Product { Title = "Phone", Price = 310.7 };
                    session.Store(cup);
                    session.Store(phone);

                    var pos1 = new OrderDetail { Product = cup, Amount = 2 };
                    var pos2 = new OrderDetail { Product = phone, Amount = 1 };
                    session.Store(pos1);
                    session.Store(pos2);
                    var order = new Order { Customer = amm, OrderDate = DateTime.Now, OrderDetail = new[] { pos1, pos2 } };
                    session.Store(order);
                    session.SaveChanges();

                    var q = session.Query<Order>().Where(o => o.Customer.Name== "Aydin Mir Mohammadi").ToList();
                    
                }
            }
        }
        private static void DeleteAll()
        {
            using (var documentStore = new DocumentStore { Url = "http://localhost:8080" })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession("Shop"))
                {

                    foreach (var c in session.Query<Customer>())
                    {
                        session.Delete(c);
                    }
                    foreach (var p in session.Query<Product>())
                    {
                        session.Delete(p);
                    }
                    foreach (var o in session.Query<Order>())
                    {
                        session.Delete(o);
                    }
                    foreach (var od in session.Query<OrderDetail>())
                    {
                        session.Delete(od);
                    }
                    session.SaveChanges();
                }

            }
        }
    }

   public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
    }
    public class Order
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }

        public OrderDetail[] OrderDetail { get; set; }
    }

    public class OrderDetail
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public Product Product { get; set; }
    }


}
