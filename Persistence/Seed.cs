using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var superUser = new AppUser
                {
                    FirstName = "Admin",
                    MiddleInitial = "Super",
                    LastName = "User",
                    Department = "System Admin",
                    UserName = "admin",
                    Email = "admin@test.com",
                    IsSuperUser = true
                };

                await userManager.CreateAsync(superUser, "Pa$$w0rd");

                var user = new AppUser
                {
                    FirstName = "Juan",
                    MiddleInitial = "C.",
                    LastName = "Dela Cruz",
                    Department = "Finance",
                    UserName = "jcdelacruz",
                    Email = "jcdelacruz@test.com"
                };

                await userManager.CreateAsync(user, "TestPa$$w0rd");

                for (int i = 1; i <= 356; i++)
                {

                    DateTime day = new DateTime(2020, 3, 1,8,0,0).AddDays(i);
                    var attendance = new Attendance
                    {
                        TimeIn = day,
                        TimeOut = day.AddHours(9),
                        Accomplishments = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AppUser = user
                    };

                    await context.Attendances.AddAsync(attendance);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}