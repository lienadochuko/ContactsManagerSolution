using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsManager.Core.Domain.Entities
{
    public class Person
    {
        /// <summary>
        /// Person domain Model class
        /// </summary>

        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DOB { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public Guid? CountryID { get; set; }
        public bool RecieveNewsLetter { get; set; }

        public string? NIN { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country? country { get; set; }
    }
}
