using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CICD_Guide_Tool.Models
{
    public class CicdStep
    {
        public CicdStep()
        {
            AvailablePlatforms = new List<Platform>();
            ChosenPlatformId = -1;
        }
        public int Id { get; set; }
        [Required]
        public string StepName { get; set; }

        public int ChosenPlatformId { get; set; }

        public Platform ChosenPlatform { get; set; }

        public List<Platform> AvailablePlatforms { get; set; }
    }
}
