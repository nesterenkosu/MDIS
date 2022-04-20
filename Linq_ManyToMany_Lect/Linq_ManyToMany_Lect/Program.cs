using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_ManyToMany_Lect
{
    class Program
    {
        static void Main(string[] args)
        {
            DataClasses1DataContext db = new DataClasses1DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\Linq_ManyToMany_Lect\Linq_ManyToMany_Lect\Database1.mdf;Integrated Security=True");
            db.Log = Console.Out;
            //Выбрать роль с Id = 1 (Администратор)
            var role = db.Role.First(r => r.Id == 1);
            //Извлечь пользователя с Id = 1 (Андрей)
            var user = db.User.First(u => u.Id == 1);
            //полученному пользователю добавить роль
            user.Roles.Add(role);
            //отправить изменения в базу данных
            db.SubmitChanges();

            Console.ReadLine();
        }
    }
}
