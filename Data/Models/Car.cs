﻿using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class Car
    {
        public Car()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Repairs = new List<Repair>();
        }
        [Key]
        public string Id { get; set; }

        [RegularExpression(ModelConstants.ModelConstants.VinNumberRegex)]
        public string VinNumber { get; set; }

        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }

        public DateTime YearManufactured { get; set; }

        public int Milleage { get; set; }


        public DateTime? NextRepair { get; set; }

        public string? NextRepairInfo { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
