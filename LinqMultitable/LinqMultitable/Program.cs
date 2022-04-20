using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqMultitable
{
    [Table]
    public class Customer
    {
        [Column(IsPrimaryKey = true, IsDbGenerated =true)]
        public int Id { get; set; }
        [Column]
        public string CustomerCode { get; set; }
        [Column]
        public string CustomerName { get; set; }

        //--Реализация связи "один-ко-многим"---       

        private EntitySet<CustomerAddresses> _addresses;

        [Association(Storage = "_addresses",
         ThisKey = "Id", OtherKey = "CustomerID")]
        public EntitySet<CustomerAddresses> Addresses
        {
            set
            {
                _addresses = value;
            }
            get
            {
                return _addresses;
            }
        }
        //--[end]--Реализация связи "один-ко-многим"---
    }

    [Table]
    public class CustomerAddresses
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string Address1 { get; set; }
        [Column]
        public string Address2 { get; set; }
        
        [Column]
        public int CustomerID { get; set; }

        //--Реализация связи "один-к-одному"---   

        private EntityRef<Phone> _phone;

        [Association(Storage = "_phone",
         ThisKey = "Id", OtherKey = "AddressID")]
        public Phone _Phone
        {
            set
            {
                _phone.Entity = value;
            }
            get
            {
                return _phone.Entity;
            }
        }
        //--[end]--Реализация связи "один-к-одному"---
    }

    [Table]
    public class Phone
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string MobilePhone { get; set; }
        [Column]
        public string LandLine { get; set; }        
        [Column]
        public int AddressID { get; set; }        
    }
    class Program
    {
        static void Main(string[] args)
        {
            DataContext dc = new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\LinqMultitable\LinqMultitable\Database1.mdf;Integrated Security=True");
            var customers = from c in dc.GetTable<Customer>() select c;

            foreach (Customer customer in customers)
            {
                Console.WriteLine("Имя: "+customer.CustomerName);
                Console.WriteLine("Адреса:");
                foreach (var address in customer.Addresses)
                    Console.WriteLine(address.Address1+" "+ address.Address2+" "+address._Phone.MobilePhone);
            }   
            Console.ReadLine();
        }
    }
}
