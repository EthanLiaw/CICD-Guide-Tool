using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CICD_Guide_Tool.Models
{
    //Model to represent the entire CI/CD plan of a given project. This is populated by specifying the platforms for each step by the user. 
    public class CicdPlan
    {
        public CicdPlan()
        {
            steps = new Dictionary<int, CicdStep>();
        }
        public int Id { get; set; }
        public string creator { get; set; }
        public Dictionary<int, CicdStep> steps { get; set; }

        public bool NoStepsFilled()
        {
            foreach (var step in steps)
            {
                if(step.Value.ChosenPlatformId >= 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
