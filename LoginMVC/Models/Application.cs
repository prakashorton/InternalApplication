using System;
using System.Collections.Generic;

namespace LoginMVC.Models
{
    public class Application
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime PlannedRelease { get; set; }

        public string Description { get; set; }

        public DateTime ActualRelease { get; set; }

        public string Type { get; set; }

        public string Environment { get; set; }

        public string Server { get; set; }

        public string Provider { get; set; }

        public string Client { get; set; }

        public string Technology { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class ApplicationType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Environment
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Server
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Technology
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UIDropDown
    {
        public List<ApplicationType> ApplicationType { get; set; }

        public List<Environment> Environment { get; set; }

        public List<Server> Server { get; set; }

        public List<Client> Client { get; set; }

        public List<Technology> Technology { get; set; }

        public UIDropDown()
        {
            this.ApplicationType = new List<Models.ApplicationType>();
            this.Environment = new List<Models.Environment>();
            this.Server = new List<Models.Server>();
            this.Client = new List<Models.Client>();
            this.Technology = new List<Models.Technology>();
        }
    }
}