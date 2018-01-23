using ElevenNote.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class AdminService
    {
        private readonly Guid _userId;

        public AdminService(Guid userId)
        {
            _userId = userId;
        }

        public bool CheckAdmin()
        {
            using(var context = new ApplicationDbContext())
            {
                //string userId = Guid.ToString(_userId);

                return false;
            };
        }
    }
}
