using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class UsersRoles
    {
        private User _user;
        private UserRole _userRole;

        public UsersRoles()
        {

        }

        private UsersRoles(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public User User
        {
            get => LazyLoader.Load(this, ref _user);
            set => _user = value;
        }
        [ForeignKey("RoleId")]
        public UserRole UserRole
        {
            get => LazyLoader.Load(this, ref _userRole);
            set => _userRole = value;
        }
    }
}
