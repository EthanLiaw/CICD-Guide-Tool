using CICD_Guide_Tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace CICD_Guide_Tool.Controllers
{
    public class ComponentAddController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<Platform> addedPlatforms { get; set; }

        public ComponentAddController(ILogger<HomeController> logger)
        {
            _logger = logger;
            addedPlatforms = new List<Platform>();
            XmlDocument backend = new XmlDocument();
            try
            {
                backend.Load("Data/BackendData.xml");
                var cicdcomponents = backend.GetElementsByTagName("cicd-component");
                var componentPlatforms = backend.GetElementsByTagName("component-platforms")[0];
                var platformForNewComponent = componentPlatforms.FirstChild;
                while (platformForNewComponent != null)
                {
                    addedPlatforms.Add(new Platform());
                    platformForNewComponent = platformForNewComponent.NextSibling;
                }
                _logger.LogDebug($"got some stuffs from the backend...{cicdcomponents}");
            }
            catch (System.IO.FileNotFoundException)
            {
                _logger.LogError("yea...we can't find or read the xml");
            }
        }
        public ComponentSubmission GetSubmissionFromXml()
        {
            ComponentSubmission submissionModelToReturn = new ComponentSubmission();
            XmlDocument backend = new XmlDocument();
            try
            {
                backend.Load("Data/BackendData.xml");
                var platforms = backend.GetElementsByTagName("component-platforms")[0];
                var platform = platforms.FirstChild;
                while (platform != null)
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

        public IActionResult ComponentAdd()
        {
            var submissionModel = GetSubmissionFromXml();
            _logger.LogInformation("No param passed:");
            submissionModel.draftSubmission.AvailablePlatforms = addedPlatforms;
            return View(submissionModel);
        }

        [HttpPost]
        public ActionResult AddPlatform(ComponentSubmission newComponent)
        {
            var platformToAdd = newComponent.draftSubmission.ChosenPlatformId;
            //addedPlatforms.Add(platformToAdd);
            //newComponent.draftSubmission.AvailablePlatforms.Add(platformToAdd);
            //add the new platform to xml
            WriteNewPlatformToXml(platformToAdd.ToString());
            _logger.LogInformation($"new platform {platformToAdd}");
            return RedirectToAction("ComponentAdd");
        }

        [HttpPost]
        public ActionResult AddComponent(ComponentSubmission newComponent)
        {
            newComponent.draftSubmission.AvailablePlatforms = addedPlatforms;
            if (newComponent.componentName.Length > 0)
            {
                if (newComponent.draftSubmission.AvailablePlatforms.Count > 0)
                {
                    WriteNewComponentToXml(newComponent);
                    //write the new component to xml. 
                }
            }
            _logger.LogInformation("component");
            return RedirectToAction("ComponentAdd");
        }

        [HttpPost]
        public void RemovePlatform(string platform)
        {
            _logger.LogInformation($"platform to be removed is {platform}");
            //remove the platform from xml
        }

    }
}
