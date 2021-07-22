using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PersonAPI.Models;
using ResumeCore.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonAPI.Extensions {
    public static partial class IApplicationBuilderExtensions {
        /// <summary>
        ///     Seed sample data in the Registration container
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task SeedPersonContainerIfEmptyAsync(this IApplicationBuilder builder) {

            using IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IGenericRepository<PersonEntity> _repo = serviceScope.ServiceProvider.GetService<IGenericRepository<PersonEntity>>();

            // Check if empty
            string sqlQueryText = "SELECT * FROM c";
            IEnumerable<PersonEntity> existingPeople = await _repo.GetItemsAsync(sqlQueryText);

            if ( !existingPeople.Any() ) {

                #region Me Data
                PersonEntity pEntity = new() {
                    Id = "10000000-0000-0000-0000-000000000001",
                    Person = new Person {
                        //Id = System.Guid.Parse("10000000-0000-0000-0000-000000000001"),
                        FName = "Justin",
                        MName = "Earl",
                        LName = "Mann",
                        ImageRef = "https://jemstorageaccount.blob.core.windows.net/jemimages/me.e.png",
                        LinkedInUrl = "https://www.linkedin.com/in/justin-mann-b3822075/",
                        WebsiteURL = "https://406jem.us",
                        GitHubUrl = "https://github.com/Justin-Mann",
                        ContactInfo = new Address {
                            Line1 = "3044 S Owyhee",
                            Line2 = "Apt. 201",
                            City = "Boise",
                            State = "Idaho",
                            Zip = "83705",
                            Email = "justinmann.mail@gmail.com",
                            Phone = "406.498.7390",
                            Fax = null
                        }
                    }
                };
                await _repo.AddItemAsync(pEntity);
                #endregion

                #region My Reference People
                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000002",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000003",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000004",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000005",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000006",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000007",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000008",
                    Person = new Person {
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
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000009",
                    Person = new Person {
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
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000010",
                    Person = new Person {
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
                    }
                });

                await _repo.AddItemAsync(new PersonEntity {
                    Id = "10000000-0000-0000-0000-000000000011",
                    Person = new Person {
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
                });
                #endregion

                #region Generate Some Test PersonEntity Records w/ Predictable Property Values
                for ( int i = 0; i < 10; i++ ) {
                    pEntity = new() { 
                        Id = $"00000000-0000-0000-0000-{i:D12}",
                        Person = new Person { 
                            FName = $"TestFName{i}",
                            MName = $"TestMName{i}",
                            LName = $"TestLName{i}",
                            ImageRef = "https://jemstorageaccount.blob.core.windows.net/jemimages/me.e.png",
                            LinkedInUrl = "https://www.linkedin.com/in/justin-mann-b3822075/",
                            WebsiteURL = "https://406jem.us",
                            GitHubUrl = "https://github.com/Justin-Mann",
                            Profession = $"Test Profession {i}",
                            ContactInfo = new Address {
                                Line1 = $"Test Address {i} Ln.1",
                                Line2 = $"Test Address {i} Ln.2",
                                City = $"Test Address {i} City",
                                State = $"Test Address {i} State",
                                Zip = $"Test Address {i} Zip",
                                Email = $"Test Address {i} Email",
                                Phone = $"Test Address {i} Phone",
                                Fax = $"Test Address {i} Fax Number"
                            }
                        }
                    };
                    await _repo.AddItemAsync(pEntity);
                }
                #endregion
            }
        }
    }
}
