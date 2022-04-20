using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqInheritance
{
    [Table]
    [InheritanceMapping(Code = 1, Type = typeof(PermanentEmployee))]
    [InheritanceMapping(Code = 2, Type = typeof(ContractEmployee), IsDefault = true)]
    class Employee
    {
        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public string Name;
        [Column]
        public int Gender;
        [Column(IsDiscriminator = true)]
        public int Discriminator;
    }

    class ContractEmployee: Employee
    {
        [Column]
        public int HourlyPay;
        [Column]
        public int HoursWorked;
    }

    class PermanentEmployee: Employee
    {
        [Column]
        public int AnuualSalary;
    }
    class Program
    {
        static void Main(string[] args)
        {
            DataContext db = new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\LinqInheritance2\LinqInheritance\Database1.mdf;Integrated Security=True");

            Table<Employee> emp = db.GetTable<Employee>();

            var contract = from c in emp.OfType<PermanentEmployee>()
                           select c;

            foreach (var c in contract)
                Console.WriteLine(c.Name);

            Console.ReadLine();
        }
    }
}
