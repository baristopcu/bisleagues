using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class UserRole
    {
        private ICollection<UsersRoles> _usersRoles;

        private UserRole(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UsersRoles> UsersRoles
        {
            get => LazyLoader.Load(this, ref _usersRoles);
            set => _usersRoles = value;
        }

    }
}
