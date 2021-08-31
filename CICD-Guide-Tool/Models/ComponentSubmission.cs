using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICD_Guide_Tool.Models
{
    public class ComponentSubmission
    {
        public ComponentSubmission()
        {
            finalSubmission = new CicdStep();
            draftSubmission = new CicdStep();
            componentName = "Branching Strategy";
        }
        public CicdStep finalSubmission { get; set; }
        public CicdStep draftSubmission { get; set; }
        [Required(ErrorMessage = "Please enter a name for the new component.")]
        public string componentName { get; set; }
    }
}
