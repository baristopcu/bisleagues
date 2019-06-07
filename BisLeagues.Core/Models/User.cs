using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Models
{
    public partial class User
    {
        private ICollection<UsersRoles> _usersRoles;

        public User()
        {

        }


        private User(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public ICollection<UsersRoles> UsersRoles
        {
            get => LazyLoader.Load(this, ref _usersRoles);
            set => _usersRoles = value;
        }
    }
}
