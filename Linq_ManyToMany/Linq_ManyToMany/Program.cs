using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Linq_ManyToMany
{
    class Program
    {
        [Table]
        class User
        {
            [Column(IsPrimaryKey =true, IsDbGenerated =true)]
            public int id { get; set; }
            [Column]
            public string Name { get; set; }
            [Column]
            public string Login { get; set; }
            [Column]
            public string Password { get; set; }

            private EntitySet<UserRole> _user_roles;

            [Association(Storage = "_user_roles",
             ThisKey = "id", OtherKey = "UserID")]
            public EntitySet<UserRole> UserRoles
            {
                set
                {
                    _user_roles = value;
                }
                get
                {
                    return _user_roles;
                }
            }

            private EntitySet<Role> _roles;
            public EntitySet<Role> Roles
            {
                get
                {
                    if (_roles == null)
                    {                        
                        _roles = new EntitySet<Role>(OnRolesAdd, OnRolesRemove);
                        _roles.SetSource(UserRoles.Select(c => c._Role));
                    }
                    return _roles;
                }
                set
                {
                    _roles.Assign(value);
                }
            }

            [System.Diagnostics.DebuggerNonUserCode]
            private void OnRolesAdd(Role entity)
            {
                this.UserRoles.Add(new UserRole { _User = this, _Role = entity });
                //SendPropertyChanged(null);
            }

            [System.Diagnostics.DebuggerNonUserCode]
            private void OnRolesRemove(Role entity)
            {
                var userRole = this.UserRoles.FirstOrDefault(
                    c => c.UserID == id
                    && c.RoleID == entity.id);
                this.UserRoles.Remove(userRole);
                //SendPropertyChanged(null);
            }

           
        }

        [Table]
        class UserRole
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int id { get; set; }
            [Column]
            public int UserID { get; set; }
            [Column]
            public int RoleID { get; set; }

            private EntityRef<User> _user;

            [Association(Storage = "_user",
            ThisKey = "UserID", OtherKey = "id")]
            public User _User
            {
                set
                {
                    _user.Entity = value;
                }
                get
                {
                    return _user.Entity;
                }
            }

            private EntityRef<Role> _role;

            [Association(Storage = "_role",
            ThisKey = "RoleID", OtherKey = "id")]
            public Role _Role
            {
                set
                {
                    _role.Entity = value;
                }
                get
                {
                    return _role.Entity;
                }
            }

        }

        [Table]
        class Role
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int id { get; set; }
            [Column]
            public string Name { get; set; }

            private EntitySet<UserRole> _role_users;

            [Association(Storage = "_role_users",
             ThisKey = "id", OtherKey = "UserID")]
            public EntitySet<UserRole> RoleUsers
            {
                set
                {
                    _role_users = value;
                }
                get
                {
                    return _role_users;
                }
            }
        }
        static void Main(string[] args)
        {
            DataContext dc = new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nesterenko_sy\source\repos\Linq_ManyToMany\Linq_ManyToMany\Database1.mdf;Integrated Security=True");

            dc.Log = Console.Out;

            var role = dc.GetTable<Role>().First(r => r.id == 1);

            //Console.ReadLine();

            var user = dc.GetTable<User>().First(u => u.id == 2);

            //Console.ReadLine();

            user.Roles.Add(role);

            //Console.ReadLine();

            dc.SubmitChanges();
            

            var users = from u in dc.GetTable<User>() select u;

            foreach (User u in users)
            {
                Console.WriteLine("Пользователь: " + u.Name);
                Console.WriteLine("Роли:");
                foreach(var r in u.Roles)
                {
                    Console.WriteLine(r.Name);
                }
            }

            Console.ReadLine();

        }
    }
}
