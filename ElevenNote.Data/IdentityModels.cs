using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

/*
 * 
 * Moved this file from ElevenNote.Web, We have to change the namespace to the one for this project
 * which is ElevenNote.Data
 * Alot of this are broken right when you move.
 * the first change is in line 1 with the entity using, so we need to bring in a Nuget package to
 * this file add in the Microsoft.AspNet.Identity.EntityFramework and select this(ElevenNote.Data) project. 
 * 
 * All of the red lines should have gone away.
 * 
 */
namespace ElevenNote.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
        //this an override for our webAPI
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        //DefaultConncetion is reading to the Web.Config that is within the Web side of things
        {
        }

        public static ApplicationDbContext Create()
        {
            //now we need to instruct this to use our classes that are down low
            return new ApplicationDbContext();
        }

        //Adding properties 
        //Entity framework treats this as a table of notes because of the DbSet<Note>
        //This is our portal through the services down to the database.
        public DbSet<Note> Notes { get; set; }

        /*
         * using override [space] it gave us this block of code. 
         * 
         */
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //we need to delete this line of code!
            //base.OnModelCreating(modelBuilder); 

            // first thing we want to do. 
            //Enitity wants to pluralize the names of our tables role = roles
            //controlling how the tables names are generated 
            modelBuilder
                .Conventions
                .Remove<PluralizingTableNameConvention>();

            /*
             * this is the second thing 
             * we now want to add our configurations that are down low.
             * 
             * Data integrity is the most important thing!!
             */

            modelBuilder
                .Configurations
                .Add(new IndentityUserLoginConfiguration())
                .Add(new IndentityUserRoleConfiguration());
        }
    }
    /* 
     * iul = identityUserLogin 
     * HasKey== says that this should be the primary key in our database
     * 
     */
    public class IndentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IndentityUserLoginConfiguration()
        {
            HasKey(iul => iul.UserId);
        }
    }


    /*
     * This is how we track user roles in our db base on their roles Manager, customer... etc.
     * 
     * Entitiy framework is giving us the inheriting classes and Haskey and also IdentityUserRole
     * 
     */
    public class IndentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        public IndentityUserRoleConfiguration()
        {
            //This is the identifer for our data that has to be presented
            HasKey(iur => iur.RoleId);
        }
    }

    public class SecurityRoles
    {
        public const string Admin = "admin";
    }
}