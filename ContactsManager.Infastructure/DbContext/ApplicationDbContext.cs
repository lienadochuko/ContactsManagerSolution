using System;
using System.Collections.Generic;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace ContactsManager.Infastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");

            modelBuilder.Entity<Person>().ToTable("Persons");

            //sead to Countries
            string countriesJson = File.ReadAllText("countries.json");

            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize
                <List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //sead to Persons

            string personJson = File.ReadAllText("persons.json");
            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize
                <List<Person>>(personJson);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);

            }


            ////Fluent API
            //modelBuilder.Entity<Person>().Property(temp => temp.NIN)
            //	.HasColumnName("NationalIdentificationNumber")
            //	.HasColumnType("varchar(10)")
            //	.HasDefaultValue("NIN1234FB4");

            //modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

            //Table Relations
            //modelBuilder.Entity<Person>(entity =>
            //{
            //	entity.HasOne<Country>(c => c.country)
            //	.WithMany(p => p.Persons)
            //	.HasForeignKey(f => f.CountryID);
            //});
        }

        public List<Person> sp_GetAllPersons()
        {


            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email" , person.Email),
                new SqlParameter("@DOB", person.DOB),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@NIN", person.NIN),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@RecieveNewsLetter", person.RecieveNewsLetter),
            };

            return Database.ExecuteSqlRaw(" EXECUTE [dbo].[InsertPerson]" +
                "@PersonID, @PersonName, @Email, @DOB, @Gender, @Address, @CountryID, @RecieveNewsLetter, @NIN", parameters);
        }

        public int sp_UpdatePerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email" , person.Email),
                new SqlParameter("@DOB", person.DOB),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@NIN", person.NIN),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@RecieveNewsLetter", person.RecieveNewsLetter)
            };

            //Console.WriteLine(person.NIN);

            return Database.ExecuteSqlRaw(" EXECUTE [dbo].[UpdatePerson]" +
                "@PersonID, @PersonName, @Email, @DOB, @Gender, @Address, @CountryID, @RecieveNewsLetter, @NIN", parameters);
        }
        public int sp_DeletePerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID)
            };

            return Database.ExecuteSqlRaw(" EXECUTE [dbo].[DeletePerson] @PersonID", parameters);
        }
    }
}
