using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICD_Guide_Tool.Models
{
    //Model to represent a technology that may be used in a CI/CD step. Models of this class are populated by the Excel Backend.
    public class Platform
    {
        
        public int Id { get; set; }
        public int StepId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public string Url { get; set; }

        public Platform()
        {
            Id = -1;
            StepId = -1;
            Name = "NotFound";
            ImagePath = "/images/default-image.jpg";
            Description = "There is no description available for this platform";
            Url = "#";
        }

        public Platform(int stepId)
        {
            Id = -1;
            Name = "NotFound";
            StepId = stepId;
            ImagePath = "/images/default-image.jpg";
            if(stepId == 1)
            {
                ImagePath = "images/default-version-control-image.jpg";
            }
            else if(stepId == 2)
            {
                ImagePath = "images/default-language-image.jpg";
            }
            else if (stepId == 3)
            {
                ImagePath = "images/default-deployment-image.jpg";
            }
            Description = "There is no description available for this platform";
            Url = "#";
        }
    }
}
