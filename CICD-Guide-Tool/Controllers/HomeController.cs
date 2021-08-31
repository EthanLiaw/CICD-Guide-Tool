using CICD_Guide_Tool.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml;

namespace CICD_Guide_Tool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<Platform> addedPlatforms { get; set; }
        //The model describing the components and platforms to be used in the current ci/cd plan
        private CicdPlan plan { get; set; }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //intializing the ci/cd plan with default values
            plan = new CicdPlan();

            addedPlatforms = new List<Platform>();
            
            var fileName = "Data/CICDGuideToolBackend.xlsx";
            
            // For .net core, the next line requires the NuGet package, 
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            //read the platform and ci/cd component data from the excel backend
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //read the steps table
                    reader.Read();//ignore the header
                    while (reader.Read()) //Each row of the file
                    {
                        var component = new CicdStep()
                        {
                            Id = (int)reader.GetDouble(0),
                            StepName = reader.GetValue(1).ToString()
                        };
                        plan.steps[component.Id] = component;
                    }

                    //read the cicd table
                    if (reader.NextResult())
                    {
                    }

                    //read the components table, skipping the header
                    
                    if (reader.NextResult())
                    {
                        reader.Read();
                        while (reader.Read()) //Each row of the file
                        {
                            try
                            {
                                var PlatformToAdd = new Platform((int)reader.GetDouble(1))
                                {
                                    Id = (int)reader.GetDouble(0),
                                    Name = reader.GetValue(2).ToString()
                                    //Description = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : ""
                                };
                                var descriptionField = reader.GetValue(3);
                                var imagePathField = reader.GetValue(4);
                                var urlField = reader.GetValue(5);

                                //add the platform description, image, and external link if they exist
                                if(descriptionField != null)
                                {
                                    PlatformToAdd.Description = descriptionField.ToString();
                                }
                                if(imagePathField != null)
                                {
                                    PlatformToAdd.ImagePath = imagePathField.ToString();
                                }
                                if(urlField != null)
                                {
                                    PlatformToAdd.Url = urlField.ToString();
                                }
                                plan.steps[PlatformToAdd.StepId].AvailablePlatforms.Add(PlatformToAdd);
                            } catch(Exception e)
                            {
                                _logger.LogInformation("Malformed entry in table, mandatory columns were null");
                                _logger.LogWarning($" {e.Message} {e.StackTrace}");
                            }
                        }
                    }
                }
            }
        }

        //Deprecated methods due to changing backend from XML to Excel
        public ComponentSubmission GetSubmissionFromXml()
        {
            ComponentSubmission submissionModelToReturn = new ComponentSubmission();
            XmlDocument backend = new XmlDocument();
            try
            {
                backend.Load("Data/BackendData.xml");
                var platforms = backend.GetElementsByTagName("component-platforms")[0];
                var platform = platforms.FirstChild;
                while(platform != null)
                {
                    submissionModelToReturn.draftSubmission.AvailablePlatforms.Add(new Platform());
                    platform = platform.NextSibling;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                _logger.LogError("yea...we can't find or read the xml");
            }
            return submissionModelToReturn;
        }

        public int WriteNewComponentToXml(ComponentSubmission submission)
        {
            int result = -1;
            /*
            XmlDocument backend = new XmlDocument();
            try
            {
                backend.Load("Data/BackendData.xml");
                var components = backend.GetElementsByTagName("devops-components")[0];
                var componentId = (backend.GetElementsByTagName("cicd-component").Count).ToString();
                List<XmlNode> platformNodes = new List<XmlNode>();
                var platformContainerNode = backend.CreateElement("avaiilable-platforms");
                foreach (var platform in submission.draftSubmission.AvailablePlatforms)
                {
                    var platformNode = backend.CreateElement("platform");
                    platformNode.InnerText = platform;
                    platformContainerNode.AppendChild(platformNode);
                }
                var nameNode = backend.CreateElement("component-name");
                nameNode.InnerText = submission.componentName;

                var idNode = backend.CreateElement("id");
                idNode.InnerText = componentId;

                var componentNode = backend.CreateElement("cicd-component");
                componentNode.AppendChild(idNode);
                componentNode.AppendChild(nameNode);
                componentNode.AppendChild(platformContainerNode);
                components.AppendChild(componentNode);

                //XmlNode newComponentXml = new XmlNode();
                backend.Save("Data/BackendData.xml");
            }
            catch (System.IO.FileNotFoundException)
            {
                _logger.LogError("yea...we can't find or read the xml");
            }*/
            return result;
        }

        public int WriteNewPlatformToXml(string platform)
        {
            int result = -1;
            XmlDocument backend = new XmlDocument();
            try
            {
                backend.Load("Data/BackendData.xml");
                var platforms = backend.GetElementsByTagName("component-platforms")[0];
                var platformNode = backend.CreateElement("platform");
                platformNode.InnerText = platform;
                platforms.AppendChild(platformNode);
                backend.Save("Data/BackendData.xml");
                return result;
            }
            catch (System.IO.FileNotFoundException)
            {
                _logger.LogError("yea...we can't find or read the xml");
            }
            return result;
        }

        //Load homepage with no inputs specified
        public ActionResult Index()
        {
            var myReq = Request;
            var user = HttpContext.User;
            _logger.LogInformation($"The current user is {User.Identity.Name}");

            ViewData["User"] = Environment.UserName;
            return View(plan);
        }

        //Load homepage after at least one input specified
        [HttpPost]
        public ActionResult Index(CicdPlan submittedPlan)
        {
            ViewData["User"] = Environment.UserName;
            //Identify if any of the steps have had the platform specified (the platformId is nonzero)
            //If so, update the CICD plan model accordingly
            foreach (var step in submittedPlan.steps)
            {
                var platformId = step.Value.ChosenPlatformId;
                if (step.Value.ChosenPlatformId >= 0)
                {
                    
                    ViewData[step.Key.ToString()] = $"The chosen platform for {step.Value.StepName} is {platformId}.";   
                }
                else
                {
                    step.Value.ChosenPlatformId = -1;
                }
                plan.steps[step.Key].ChosenPlatformId = platformId;
                plan.steps[step.Key].ChosenPlatform = plan.steps[step.Key].AvailablePlatforms.Find(foo => foo.Id == step.Value.ChosenPlatformId);
            }
            return View(plan);
        }

        //Load the privacy page
        public IActionResult Privacy()
        {
            return View();
        }


        //Load the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
