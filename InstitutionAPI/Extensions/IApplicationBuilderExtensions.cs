using InstitutionAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ResumeCore.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionAPI.Extensions {
    public static partial class IApplicationBuilderExtensions {
        /// <summary>
        ///     Seed sample data in the Registration container
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task SeedInstitutionContainerIfEmptyAsync(this IApplicationBuilder builder) {

            using IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IGenericRepository<InstitutionEntity> _repo = serviceScope.ServiceProvider.GetService<IGenericRepository<InstitutionEntity>>();

            // Check if empty
            string sqlQueryText = "SELECT * FROM c";
            IEnumerable<InstitutionEntity> existingInstitutions = await _repo.GetItemsAsync(sqlQueryText);

            if ( !existingInstitutions.Any() ) {

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000001",
                    Institution = new() {
                        Name = "Montana Tech U of M",
                        Location = "Butte Montana",
                        Type = "College/University"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000002",
                    Institution = new() {
                        Name = "Montana State University (MSU)",
                        Location = "Bozeman Montana",
                        Type = "College/University"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000003",
                    Institution = new() {
                        Name = "The MIL Corperation",
                        Location = "Boise Idaho (Remote)",
                        Type = "Federal Contractor"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000004",
                    Institution = new() {
                        Name = "International Trade Administration (ITA)",
                        Location = "Washington D.C. (Remote)",
                        Type = "Federal Agency"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000005",
                    Institution = new() {
                        Name = "Insight Global (IG)",
                        Location = "Boise Idaho (Remote)",
                        Type = "Contractor"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000006",
                    Institution = new() {
                        Name = "Vertex Consulting",
                        Location = "Boise Idaho",
                        Type = "Contractor"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000007",
                    Institution = new() {
                        Name = "MWI Animal Health",
                        Location = "Boise Idaho",
                        Type = "Aminal Medical Supplier"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000008",
                    Institution = new() {
                        Name = "Montana Interactive",
                        Location = "Helena Montana",
                        Type = "State & Local Gov't Solution Provider"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000009",
                    Institution = new() {
                        Name = "Montana State, County and City Organizations",
                        Location = "Montana",
                        Type = "State & Local Gov't Agencies"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000010",
                    Institution = new() {
                        Name = "Idaho State Controller's Office",
                        Location = "Boise Idaho",
                        Type = "State Agency Solution Providers, Affiliated w/ the State of Idaho"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000011",
                    Institution = new() {
                        Name = "3rd Party Contract (cannot remember which honestly)",
                        Location = "Boise Idaho",
                        Type = "Contractor"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000012",
                    Institution = new() {
                        Name = "Myself; under SimplePlan, LLC",
                        Location = "Butte Montana",
                        Type = "Contractor"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000013",
                    Institution = new() {
                        Name = "Small & Medium Local Businesses",
                        Location = "Montana",
                        Type = "A variety of Small To Medium Local Businesses"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000014",
                    Institution = new() {
                        Name = "InfoMine Of The Rockies",
                        Location = "Butte Montana",
                        Type = "Technology and Business Infastructure Services"
                    }
                });

                await _repo.AddItemAsync(new() {
                    Id = "20000000-0000-0000-0000-000000000015",
                    Institution = new() {
                        Name = "Local Business and Organizations",
                        Location = "Montana",
                        Type = "State Agency Solution Providers, Affiliated w/ the State of Idaho"
                    }
                });
            }
        }
    }
}
