using Microsoft.AspNetCore.Identity;
using Fundo.Applications.WebApi.Constants;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Data
{
    public class DbSeeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FundoLoanContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                /* #1
                 * We start first with the authentication and authorization issues, since they are the 
                 * most complex, at least for this particular exercise. */

                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                // Check if any users exist to prevent duplicate seeding
                if (userManager.Users.Any() == false)
                {
                    var user = new ApplicationUser
                    {
                        Name = "Admin",
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    // Create Admin role if it doesn't exist
                    if ((await roleManager.RoleExistsAsync(Roles.Admin)) == false)
                    {
                        logger.LogInformation("Admin role is creating");
                        var roleResult = await roleManager
                          .CreateAsync(new IdentityRole(Roles.Admin));

                        if (roleResult.Succeeded == false)
                        {
                            var roleErros = roleResult.Errors.Select(e => e.Description);
                            logger.LogError($"Failed to create admin role. Errors : {string.Join(",", roleErros)}");

                            return;
                        }
                        logger.LogInformation("Admin role is created");
                    }

                    // Attempt to create admin user
                    var createUserResult = await userManager
                          .CreateAsync(user: user, password: "Admin@123");

                    // Validate user creation
                    if (createUserResult.Succeeded == false)
                    {
                        var errors = createUserResult.Errors.Select(e => e.Description);
                        logger.LogError(
                            $"Failed to create admin user. Errors: {string.Join(", ", errors)}"
                        );
                        return;
                    }

                    // adding role to user
                    var addUserToRoleResult = await userManager
                                    .AddToRoleAsync(user: user, role: Roles.Admin);

                    if (addUserToRoleResult.Succeeded == false)
                    {
                        var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                        logger.LogError($"Failed to add admin role to user. Errors : {string.Join(",", errors)}");
                    }
                    logger.LogInformation("Admin user is created");
                }

                /* #2
                 * We add a few loans to have something to look at and manipulate when testing the api, 
                 * or validating the web application.
                 */

                if (context.Loans.Any() == false)
                {
                    List<Loan> loans = new List<Loan>();
                    loans.Add(new Loan() { ApplicantName = "John Doe", Amount = (decimal?)25000.00, CurrentBalance = (decimal?)18750.00, Status = "Active" });
                    loans.Add(new Loan() { ApplicantName = "Jane Smith", Amount = (decimal?)15000.00, CurrentBalance = (decimal?)0, Status = "Paid" });
                    loans.Add(new Loan() { ApplicantName = "Robert Johnson", Amount = (decimal?)50000.00, CurrentBalance = (decimal?)32500.00, Status = "Active" });
                    loans.Add(new Loan() { ApplicantName = "Emily Williams", Amount = (decimal?)10000.00, CurrentBalance = (decimal?)0, Status = "Paid" });
                    loans.Add(new Loan() { ApplicantName = "Michael Brown", Amount = (decimal?)75000.00, CurrentBalance = (decimal?)72000.00, Status = "Active" });

                    foreach (var loan in loans)
                    {
                        context.Loans.Add(loan);
                    }

                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
            }

        }
    }
}
