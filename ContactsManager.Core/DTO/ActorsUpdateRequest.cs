using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO
{
    public class ActorsUpdateRequest
    {
        [Required(ErrorMessage = "Actors ID can't be blank")]
        public int ActorID { get; set; }

        [Required(ErrorMessage = "FirstName can't be blank")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "FamilyName can't be blank")]
        public string? FamilyName { get; set; }

        [Required(ErrorMessage = "FullName can't be blank")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Dirth of Birth can't be blank")]
        public DateTime? DOB { get; set; }

        public DateTime? DOD { get; set; }

        [Required(ErrorMessage = "Gender can't be blank")]
        public string? Gender { get; set; }

        public ActorsDto ToActorsDto()
        {
            return new ActorsDto()
            {
               ActorID = ActorID,
               FirstName = FirstName,
               FamilyName = FamilyName,
               FullName =  FullName,
               DoB = DOB,
               DoD = DOD,
               Gender = Gender
            };
        }
    }
}
