using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyResumeAPI.Models;
using ResumeCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeAPI.Extensions {
    public static partial class IApplicationBuilderExtensions {
        /// <summary>
        ///     Seed sample data in the Registration container
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task SeedRegistrationContainerIfEmptyAsync(this IApplicationBuilder builder) {

            using IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IGenericRepository<ResumeEntity> _repo = serviceScope.ServiceProvider.GetService<IGenericRepository<ResumeEntity>>();

            // Check if empty
            string sqlQueryText = "SELECT * FROM c";
            IEnumerable<ResumeEntity> existingResumes = await _repo.GetItemsAsync(sqlQueryText);

            if ( !existingResumes.Any() ) {
                for ( int i = 0; i < 1; i++ ) { // ONLY BUILD ONE RECORD...MINE
                    ResumeEntity resEnt = new() {
                        Id = "dbbab80d-fe68-40df-9ab6-7043777c3da4", // CHANGE THIS IF EVER BUILDING MORE THAN ONE RECORD
                        Resume = new() {
                            DocumentVersion = new DocumentVersion { Major = 1, Minor = 0 },
                            Person = new Person {
                                FName = "Justin",
                                MName = "Earl",
                                LName = "Mann",
                                ContactInfo = new Address {
                                    Line1 = "3044 S Owyhee",
                                    Line2 = "Apt. 201",
                                    City = "Boise",
                                    State = "Idaho",
                                    Zip = "83705",
                                    Email = "justinmann.mail@gmail.com",
                                    Phone = "406.498.7390",
                                    Fax = null
                                },
                                ImageRef = "https://jemstorageaccount.blob.core.windows.net/jemimages/me.e.png",
                                LinkedInUrl = "https://www.linkedin.com/in/justin-mann-b3822075/",
                                WebsiteURL = "https://406jem.us",
                                GitHubUrl = "https://github.com/Justin-Mann"
                            },
                            JobTitle = "Web  Application Developer",
                            ObjectiveStatement = "I'm a computer software applications developer with a lot of different environmental knowledge and application server proficiencies. I am interested in clean modern design and learning about different technology application implementations across the board; if I can build, configure or program 'it' to do something useful or simplify a task I'm immediately interested. I love to learn new things related to problem-solving and on the other side of the coin I like to understand problems and tasks thoroughly in order to correctly design a solution. Good modern tools, knowledge of application and understanding of the task(s) are all very important to me. I like to work with and learn from people, and of recent I find I have been doing more and more work on microservices in cloud environments. Recently I have a renewed interest in block-chain and smart contracts, so I am spending some time looking at smart contract integration and explore some potential uses. All in all, I like to solve problems and I like to work with people towards a common goal; if you have an interesting problem or an intention to improve accessibility or process, I would like to help.",
                            EducationHistory = new List<EducationHistoryItem> {
                                new EducationHistoryItem {
                                    Institution = new Institution {
                                        Name = "Montana Tech U of M",
                                        Location = "Butte Montana",
                                        Type = "College/University"
                                    },
                                    StartDate = null,
                                    EndDate = new DateTime(2008, 05, 01),
                                    AreasOfStudy = "Computer Science, Software Developement, Computer Architecture, Business Management, Mathematics".Trim().Split(","),
                                    CompletedDegrees = new List<Degree> {
                                        new Degree {
                                            Name = "Bachelors of Computer Science, Math Minor",
                                            Detail = "Emphasis on Small and Medium Business Practices and Management",
                                            StartDate = null,
                                            EndDate = new DateTime(2008, 05, 01)
                                        }
                                    },
                                    Partials = new List<Degree> { }
                                },
                                new EducationHistoryItem {
                                    Institution = new Institution {
                                        Name = "Montana State University (MSU)",
                                        Location = "Bozeman Montana",
                                        Type = "College/University"
                                    },
                                    StartDate = null,
                                    EndDate= null,
                                    AreasOfStudy = "Electrical Engineering, Computer Science".Trim().Split(","),
                                    CompletedDegrees = new List<Degree> { },
                                    Partials = new List<Degree> {
                                        new Degree {
                                            Name = "Electrical Engineering",
                                            Detail = "I took 3 semesters of EE before realizing that I really liked the logic aspect of my courses and ended up switching my major to CS.",
                                            StartDate = null,
                                            EndDate = null
                                        },
                                        new Degree  {
                                            Name = "Computer Science",
                                            Detail = "I took 4 semesters of CS then transferred to Montana Tech. ",
                                            StartDate = null,
                                            EndDate = null
                                        }
                                    }
                                }
                            },
                            WorkHistory = new List<WorkHistoryItem> {
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "The MIL Corperation",
                                        Location = "Boise Idaho (Remote)",
                                        Type = "Federal Contractor"
                                    },
                                    Client = new Institution {
                                        Name = "International Trade Administration (ITA)",
                                        Location = "Washington D.C. (Remote)",
                                        Type = "Federal Agency"
                                    },
                                    JobTitle = "Web Application Developer",
                                    JobType = "Staff",
                                    StartDate = new DateTime(2020, 05, 01),
                                    EndDate = new DateTime(2021, 06, 15),
                                    DescriptionOfDuties = "After the initial contract period with The MIL Corporation they offered me a staff position as a Senior Application Analyst/Developer which I accepted happily. As I continued on with MIL and they had a better feel for my capabilities and foundations, I worked on a couple different projects hosted in AKS that were largely designed using the Domain Entity Model. Three projects actually, but only one of these projects came to fruition and was still only being released to staging when I was laid off. This was honestly some of the more interesting and engaging backend work I have done for quite a while. I was very much engaged in service and api development during the remainder of my time at MIL. With good frontend engineers to work with the hand-offs were pretty clean and the new mediums I was working with among the different projects was more than enough to keep me busy learning about and building service implementations with microservice toolkits I had never heard of before such as Akka and Dapr. My time with MIL ended prematurely due to budget changes mid contract. The last project I was working on was a collection of containerized microservice APIs hosted in AKS that used Cosmos Db for data storage and dapr to facilitate pubsub between domain microservices and external services.",
                                    ReasonForLeaving = "Layoff",
                                    CompletedProjects = new List<string> {
                                        "Major new case management platform used by multiple governments implemented in the Azure Kubernetes Service and utilizing the AKKA actor-based framework",
                                        "Public-facing business directory application",
                                        "POC Discovery process for Azure Enterprise Integration services using Service Bus, API Management and Event Grid",
                                        "Design for a distributed, cloud-native application using microservices in Azure"
                                    },
                                    References = new List<Person> {
                                        new Person {
                                            FName = "Umar",
                                            MName = null,
                                            LName = "Okeefe",
                                            Profession = "Sr. Application Developer @ MIL (Team Lead)",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "umar.okeefe@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "Luis",
                                            MName = null,
                                            LName = "Martinez",
                                            Profession = "Sr. Application Developer @ MIL (Team Lead)",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "luis@codeluiscode.info",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "Nate",
                                            MName = null,
                                            LName = "Layton",
                                            Profession = "Microservices Engineer Lead @ MIL",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "nate.layton@outlook.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "James",
                                            MName = null,
                                            LName = "Hanson",
                                            Profession = "Microservices Developer @ MIL",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "xxjhansonxx@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "Colbert",
                                            MName = null,
                                            LName = "Tew",
                                            Profession = "Project Manager @ MIL",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "colbert.tew@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "Cory",
                                            MName = null,
                                            LName = "Churches",
                                            Profession = "Sr. Project & Portfolio Manager @ MIL",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "cory.churches@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            FName = "Michael",
                                            MName = null,
                                            LName = "Brockman",
                                            Profession = "Sr. VP, Enterprise Technology & Cloud Computing (MIL)",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "mbrockman@milcorp.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        }
                                    }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Insight Global (IG)",
                                        Location = "Boise Idaho (Remote)",
                                        Type = "Contractor"
                                    },
                                    Client = new Institution {
                                        Name = "The MIL Corperation",
                                        Location = "Bozeman Montana",
                                        Type = "Federal Contractor"
                                    },
                                    JobTitle = "Web Application Developer",
                                    JobType = "Contract",
                                    StartDate = new DateTime(2020, 01, 01),
                                    EndDate = new DateTime(2020, 05, 15),
                                    DescriptionOfDuties = "The MILCorp was a 6 month contract to hire that is (at the time of this edit) onboarding me to a staff position as it is the end of my six month contractual period. Insight Global was responsible for recruiting me to this position and it has proven an excellent opportunity for me and a good match for my skillset. The work is engaging and meaningful as most project work is, but the development team attitudes and project orientations are progressive in nature with makes for a great working environment and potentially some of the most interesting scopes of work and architecture consumptions out there… Not all bleeding edge concepts, but we are at a place where and enterprise solution can adopt proven microservice and cloud driven scalable solution in a secure way… and it is exciting to work with a group that peruses this.I have not worked much with Sharepoint or within the Sharepoint framework, but I have since learned and deployed spfx webparts and learned a ton about team site and other potential deployments that can be orchestrated within a Microsoft Teams environment.I learn something new or interesting every week working here, either thru my own investigation into an engaging scope of work or via one of the many learning or working interactions with the team.I have not updated the ‘design patterns’ portion of my resume yet… but because of this job in a short six months I have at least 2 items to add; it has been and continues to be a great experience I cannot totally account for here.",
                                    ReasonForLeaving = "Staff Position w/ MIL",
                                    CompletedProjects = new List<string> {
                                        "Major new case management platform used by multiple governments implemented in the Azure Kubernetes Service and utilizing the AKKA actor-based framework",
                                        "Modernization/Migration of a Wikimedia site to SharePoint in Office 365"
                                    },
                                    References = new List<Person> { }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Vertex Consulting",
                                        Location = "Boise Idaho",
                                        Type = "Contractor"
                                    },
                                    Client = new Institution {
                                        Name = "MWI Animal Health",
                                        Location = "Boise Idaho",
                                        Type = "Aminal Medical Supplier"
                                    },
                                    JobTitle = "Web Application Developer",
                                    JobType = "Staff",
                                    StartDate = new DateTime(2019, 07, 15),
                                    EndDate = new DateTime(2020, 01, 15),
                                    DescriptionOfDuties = "MWI Animal Health was a 6 month contract with the goal of expediting some of their new Core 2.x API development goals and cleaning up redundant functionality from the existing MVC API. The web app I was working with was the oldest piece of the application stack; being written in webforms and AngularJS with an MVC API feeding data to it. The application logic was in most cases buried all the way in the database in the form of stored procedures. The main scope of my work was to cut over specific endpoints from the old API to the new Core 2 API project, more often than not, fracturing the sproc(s) that backed up the API layer and pulling as much of the logic into the service layer of the new Core 2 API as possible… Basically making it as granular as I could get away with and building new async endpoints to get them as ready as I could for the upcoming microservice implementations. Some of the patterns I encountered here were confusing to me in terms of why they were chosen, but I was exposed to a few new implementation patterns working with the MWI group and had a great experience overall. I think I was helpful to them and I picked up some things along the way.",
                                    ReasonForLeaving = "Contract was up",
                                    CompletedProjects = new List<string> {
                                        "Reworked existing Stored Proceedures where necessary and wrote middleware and restful API endpoints as part of a modernization effort for the server side components of their software stack."
                                    },
                                    References = new List<Person> { }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Montana Interactive",
                                        Location = "Helena Montana",
                                        Type = "State & Local Gov't Solution Provider"
                                    },
                                    Client = new Institution {
                                        Name = "Montana State, County and City Organizations",
                                        Location = "Montana",
                                        Type = "State & Local Gov't Agencies"
                                    },
                                    JobTitle = "Web Application Developer",
                                    JobType = "Staff",
                                    StartDate = new DateTime(2016, 11, 15),
                                    EndDate = new DateTime(2019, 02, 15),
                                    DescriptionOfDuties = "At Montana Interactive I did work on several custom web applications as well as some interesting proof of concept projects with Service Fabric, rewrote a simple mobile application; but most of my work here was on existing web applications, databases and service integrations. I also did quite a bit of documentation and discovery work while I was at MI resulting in a lot of Confluence documentation. I worked towards improving data access with restful API solutions and made use of simple stored procedures when I had an excuse. C# and javascript took up most of my time focusing on MVC, .NET CORE 2.x, Microsoft Service Fabric (native and containers), Cordova, among others. The database work was a mix of SQL Server and Oracle for the most part, also some main-frame processes that resulted in flat files we dealt with as well. The coding patterns and techniques used for new implementation at MI were quite advanced; making use of Entity code-first database techniques, in memory distributed data architectures and repository patterns.",
                                    ReasonForLeaving = null,
                                    CompletedProjects = new List<string> {
                                        "Worked on several State and Local Gov't agency web apps to do maintenaince and modernization.",
                                        "New Payment service integrations",
                                        "Custom SSO Login Portal integrations and Management GUI update",
                                        "Drag and Drop form builder module for contract namager application product that was being developed",
                                        "New Service and Web Application development as part of an Agile development team",
                                        "Ionic 2 Mobile Application, API & backend services/data and hosting"
                                    },
                                    References = new List<Person> {
                                        new Person {
                                            //Id = Guid.NewGuid(),
                                            FName = "Ben",
                                            MName = null,
                                            LName = "Hart",
                                            Profession = "System Administrator (jamf)",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "invalid.path@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        }
                                    }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Idaho State Controller's Office",
                                        Location = "Boise Idaho",
                                        Type = "State Agency Solution Providers, Affiliated w/ the State of Idaho"
                                    },
                                    Client = null,
                                    JobTitle = "IT Analyst / Programmer",
                                    JobType = "Staff",
                                    StartDate = new DateTime(2014, 10, 15),
                                    EndDate = new DateTime(2016, 08, 15),
                                    DescriptionOfDuties = "Towards the end of my contract I was offered a staff position at the SCO as there was additional work they wanted me to address. This was a continuation of the same scope of work I was doing before; building on the client-side layer to update the look and feel, user functionality, optimizing performance by improving the data transportation layer, improve cross-browser and mobile accessibility, etc... I was asked to make additional enhancements to some of the same, as well as additional web apps maintained by the State Controller's Office. I completed the web application updates and was tasked with the configuration of IBM App Scan and analysis/distribution of generated Security Exploit Remediation Reports shortly before I was laid off for budget reasons.",
                                    ReasonForLeaving = null,
                                    CompletedProjects = new List<string> {
                                        "Javascript assesment and modernization of several state agency portals as well as their core employee portal",
                                        "Some API buildout, but mainly working with client side applications that they wanted to maintain but make more accessible to more client devices (lots of Moderniz and jQuery)",
                                        "Implement datagrids in existing applications and add features such as data export or report generation"
                                    },
                                    References = new List<Person> { }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "What contractor was I working for?",
                                        Location = "Boise Idaho",
                                        Type = "Contractor"
                                    },
                                    Client = new Institution {
                                        Name = "Idaho State Controller's Office",
                                        Location = "Boise Idaho",
                                        Type = "State Agency Solution Providers, Affiliated w/ the State of Idaho"
                                    },
                                    JobTitle = "IT Analyst / Programmer",
                                    JobType = "Contract",
                                    StartDate = new DateTime(2013, 08, 15),
                                    EndDate = new DateTime(2014, 10, 15),
                                    DescriptionOfDuties = "Most of the work I did here was updating of client-side code in a Domino environment (Domino and Lotus Notes). Most of the client side work I did was updating or flat out writing native javascript client tools and making use of jQuery along with a few other select javascript libraries. I did lots of ajax to help smooth out and modernize their client-side. Not my favorite way to go, but I did what I could to optimize client-side interactions against the server, as well as updating all the existing non-compliant HTML, CSS and JS code to current standards in line with HTML5 and CSS3. Lots of Modernizr for cross browser compliance and more advanced client-side data grid integrations were built into these web apps during the client-side updates. I even went as far as writing a json API endpoint for one of the data grids in Lotus Note... which was interesting, but ultimately renewed my appreciation of Visual Studios templates.",
                                    ReasonForLeaving = "Staff Position w/ Idaho State Controller's Office",
                                    CompletedProjects = new List<string> {
                                        ""
                                    },
                                    References = new List<Person> {
                                        new Person {
                                            //Id = Guid.NewGuid(),
                                            FName = "Jermy",
                                            MName = null,
                                            LName = "Chambers",
                                            Profession = "Sr. .NET Developer @ MWI Animal Health",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "jeremy@jeremychambers.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        },
                                        new Person {
                                            //Id = Guid.NewGuid(),
                                            FName = "Richard",
                                            MName = null,
                                            LName = "Clements",
                                            Profession = "Sr. .NET Developer @ MWI Animal Health",
                                            ContactInfo = new Address {
                                                Line1 = "",
                                                Line2 = "",
                                                City = "",
                                                State = "",
                                                Zip = "",
                                                Email = "bobtjanitor@gmail.com",
                                                Phone = "",
                                                Fax = ""
                                            },
                                            ImageRef = "",
                                            LinkedInUrl = "",
                                            WebsiteURL = ""
                                        }
                                    }
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Myself under SimplePlan, LLC",
                                        Location = "Butte Montana",
                                        Type = "Contractor"
                                    },
                                    Client = new Institution {
                                        Name = "Small & Medium Local Businesses",
                                        Location = "Montana",
                                        Type = "A variety of Small To Medium Local Businesses"
                                    },
                                    JobTitle = "Owner / Developer / IT Tech",
                                    JobType = "LLC Owner",
                                    StartDate = new DateTime(2009, 05, 15),
                                    EndDate = new DateTime(2015, 05, 15),
                                    DescriptionOfDuties = "SimplePlan was the LLC I started in order to do some custom development work for local businesses and service providers. It was a way for me to pursue small intermediate roles to stand up and help configure and administer website and connectivity services for smaller business. I did a lot of custom websites and helped people to set up and configure small networks, source appropriate computer equipment for small office environments, helped integrate their businesses with social media and other service platforms to help with client accessibility and tasks like scheduling, set up hosted CMSs and e-commerce inventories as well as QuickBooks integrations. I did a lot of work driven by client need, but I focused most of my efforts on web applications and process integration.",
                                    ReasonForLeaving = "Outgrew the LLC so I disposed myself and left it to my partner.",
                                    CompletedProjects = new List<string> {
                                        "I worked with a variety of small to medium businesses in Butte and around Montana doing consulting, office IT and web and intranet application developement"
                                    },
                                    References = new List<Person>()
                                },
                                new WorkHistoryItem {
                                    Employer = new Institution {
                                        Name = "Info Of The Rockies",
                                        Location = "Butte Montana",
                                        Type = "Technology and Business Infastructure Services"
                                    },
                                    Client = new Institution {
                                        Name = "Local Business and Organizations",
                                        Location = "Montana",
                                        Type = "State Agency Solution Providers, Affiliated w/ the State of Idaho"
                                    },
                                    JobTitle = "IT Specialist",
                                    JobType = "Internship/Staff",
                                    StartDate = new DateTime(2008, 06, 15),
                                    EndDate = new DateTime(2010, 06, 15),
                                    DescriptionOfDuties = "General Client IT Services (building infrastructure for computer, telephony, network), Data Center IT Tasks, HPC Compute Cluster Administration as part of a team, Computer Virtualization and Guest OS Management, as well as work on a couple of Client Websites from time to time. I summarized this quite a bit as it was over an extended timeframe and involved several similar scopes of work for a variety of clients.",
                                    ReasonForLeaving = "Wanted to develop more and expand skill set beyond Physical IT, Infastructure and Datacenter related work.",
                                    CompletedProjects = new List<string> {
                                        "Variety of IT related tasks",
                                        "Webpages",
                                        "Datacenter IT and Management Tasks",
                                        "Server Setup and Management",
                                        "Setup and Administration Tasks on IBM 1350 Cluster Computer"
                                    },
                                    References = new List<Person>()
                                }
                            },
                            SkillGroups = new List<SkillGroup> {
                                new SkillGroup {
                                    Name = "LinkedIn Skill Assessments",
                                    Skills = new List<Skill> {
                                        new Skill { Name="C#", Competence=85 },
                                        new Skill { Name="Cascading Style Sheets (CSS)", Competence=95 },
                                        new Skill { Name="Git", Competence=95 },
                                        new Skill { Name="Javascript", Competence=85 },
                                        new Skill { Name="jQuery", Competence=95 },
                                        new Skill { Name="JSON", Competence=85 },
                                        new Skill { Name="REST APIs", Competence=70 }
                                    }
                                },
                                new SkillGroup {
                                    Name = "Pluralsight Skill Assessments",
                                    Skills = new List<Skill> {
                                        new Skill { Name="C#", Competence=80 },
                                        new Skill { Name="ASP.NET Core", Competence=73 },
                                        new Skill { Name="ASP.NET MVC5", Competence=79 },
                                        new Skill { Name="Entity Framework Core", Competence=82 },
                                        new Skill { Name="Data Modeling", Competence=81 },
                                        new Skill { Name="Query Data w/ T-SQL", Competence=69 },
                                        new Skill { Name="Visual Studios 2019", Competence=57 },
                                        new Skill { Name="Managing Source w/ Git", Competence=57 },
                                        new Skill { Name="Building Web Applications w/ Angular", Competence=36 },
                                        new Skill { Name="Javascript Core Language", Competence=86 },
                                        new Skill { Name="HTML5", Competence=85 },
                                        new Skill { Name="CSS", Competence=84 },
                                        new Skill { Name="Typescript Core Language", Competence=69 },
                                        new Skill { Name="NPM", Competence=80 },
                                        new Skill { Name="ES6", Competence=83 },
                                        new Skill { Name="RxJS", Competence=79 },
                                        new Skill { Name="Styling the Web w/ Bootstrap", Competence=76 },
                                        new Skill { Name="Building Dynamic Websites w/ jQuery", Competence=78 }
                                    }
                                }
                            }
                        }
                    };

                    await _repo.AddItemAsync(resEnt);
                }
            }
        }
    }
}
