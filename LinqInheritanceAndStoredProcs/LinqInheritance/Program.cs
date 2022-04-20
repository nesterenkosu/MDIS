using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace LinqInheritance
{
    [Table]
    [InheritanceMapping(Code = 1, Type = typeof(PermanentEmployeeHelper))]
    [InheritanceMapping(Code = 2, Type = typeof(ContractEmployeeHelper), IsDefault = true)]
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
    class ContractEmployeeHelper : Employee
    {
        private EntityRef<Contract> _contract;

        [Association(Storage = "_contract",
        ThisKey = "id", OtherKey = "EmployeeID")]
        public Contract AdditionalData
        {
            set
            {
                _contract.Entity = value;
            }
            get
            {
                return _contract.Entity;
            }
        }
    }

    [Table]
    class Contract
    {
        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public int EmployeeID;
        [Column]
        public int HourlyPay;
        [Column]
        public int HoursWorked;
    }

    class PermanentEmployeeHelper : Employee
    {
        private EntityRef<Permanent> _permanent;

        [Association(Storage = "_permanent",
        ThisKey = "id", OtherKey = "EmployeeID")]
        public Permanent AdditionalData
        {
            set
            {
                _permanent.Entity = value;
            }
            get
            {
                return _permanent.Entity;
            }
        }
    }

    [Table]
    class Permanent
    {
        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public int EmployeeID;
        [Column]
        public int AnnualSalary;
    }

    public class MyDataContext: DataContext
    {
        public MyDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "CreateContractEmployee")] 
        [return: Parameter(DbType = "Int")]
        public int CreateContractEmployee(
            [Parameter(Name = "Name", DbType = "NVarChar(50)")] string Name,
            [Parameter(Name = "Gender", DbType = "Int")] int Gender,
            [Parameter(Name = "HourlyPay", DbType = "Int")] int HourlyPay,
            [Parameter(Name = "HoursWorked", DbType = "Int")] int HoursWorked
        )
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), Name, Gender, HourlyPay, HoursWorked);
            return ((int)(result.ReturnValue));
        }

        [Function(Name = "ReadContractEmployee")]
        [return: Parameter(DbType = "Int")]
        public int ReadContractEmployee(
            [Parameter(Name = "id", DbType = "Int")] int id,
            [Parameter(Name = "Name", DbType = "NVarChar(50)")] ref string Name,
            [Parameter(Name = "Gender", DbType = "Int")] ref int Gender,
            [Parameter(Name = "HourlyPay", DbType = "Int")] ref int HourlyPay,
            [Parameter(Name = "HoursWorked", DbType = "Int")] ref int HoursWorked
        )
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),id, Name, Gender, HourlyPay, HoursWorked);
            Name = ((string)(result.GetParameterValue(1)));
            Gender = ((int)(result.GetParameterValue(2)));
            HourlyPay = ((int)(result.GetParameterValue(3)));
            HoursWorked = ((int)(result.GetParameterValue(4)));

            return ((int)(result.ReturnValue));
        }

        [Function(Name = "UpdateContractEmployee")]
        [return: Parameter(DbType = "Int")]
        public int UpdateContractEmployee(
            [Parameter(Name = "id", DbType = "Int")] int id,
            [Parameter(Name = "Name", DbType = "NVarChar(50)")] string Name,
            [Parameter(Name = "Gender", DbType = "Int")] int Gender,
            [Parameter(Name = "HourlyPay", DbType = "Int")] int HourlyPay,
            [Parameter(Name = "HoursWorked", DbType = "Int")] int HoursWorked
        )
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),id, Name, Gender, HourlyPay, HoursWorked);
            return ((int)(result.ReturnValue));
        }

        [Function(Name = "DeleteContractEmployee")]
        [return: Parameter(DbType = "Int")]
        public int DeleteContractEmployee(
            [Parameter(Name = "id", DbType = "Int")] int id            
        )
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
            return ((int)(result.ReturnValue));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyDataContext mydb = new MyDataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\LinqInheritance2\LinqInheritance\Database1.mdf;Integrated Security=True");

            mydb.CreateContractEmployee("AndrewFormProcedure",1,200,5);

            mydb.UpdateContractEmployee(2,"SteveUpdated", 1, 200, 5);

            int id;
            string Name="";
            int Gender=0;
            int HourlyPay = 0;
            int HoursWorked = 0;

            mydb.ReadContractEmployee(2, ref Name, ref Gender, ref HourlyPay, ref HoursWorked);
            Console.WriteLine(Name+" "+Gender + " " + HourlyPay + " " + HoursWorked);

            mydb.DeleteContractEmployee(2);
            /*DataContext db = new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\LinqInheritance2\LinqInheritance\Database1.mdf;Integrated Security=True");


            Table<Employee> emp = db.GetTable<Employee>();

            var permanent = from pp in emp.OfType<PermanentEmployeeHelper>()
                           select pp;
            Console.WriteLine("Постоянные сотрудники:");
            foreach (var p in permanent)
                Console.WriteLine(p.Name+" "+p.AdditionalData.AnnualSalary);



            var contract = from cc in emp.OfType<ContractEmployeeHelper>()
                           select cc;
            Console.WriteLine("\nСотрудники-совместители:");
            foreach (var c in contract)
                Console.WriteLine(c.Name+" "+ c.AdditionalData.HourlyPay+" "+c.AdditionalData.HoursWorked);*/

            /*foreach (var c in contract)
                Console.WriteLine(c.Name + " " + c.AdditionalData.HourlyPay+" "+ c.AdditionalData.HoursWorked);*/

            Console.ReadLine();
        }
    }
}
