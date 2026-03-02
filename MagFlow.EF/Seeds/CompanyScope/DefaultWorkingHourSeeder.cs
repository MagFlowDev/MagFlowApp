using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF.Seeds.CompanyScope
{
    public class DefaultWorkingHourSeeder : ICompanySeeder
    {
        public int Step => 3;

        public void Seed(CompanyDbContext context)
        {
            Task.Run(async () => await SeedAsync(context, CancellationToken.None));
        }

        public async Task SeedAsync(CompanyDbContext context, CancellationToken cancellationToken)
        {
            bool seed = false;

            List<DefaultWorkingHour> defaultWorkingHours = new List<DefaultWorkingHour>();
            foreach(var dayOfWeek in (Enums.DayOfWeek[]) Enum.GetValues(typeof(Enums.DayOfWeek)))
            {
                defaultWorkingHours.Add(new DefaultWorkingHour()
                {
                    DayOfWeek = dayOfWeek,
                    OpenTime = dayOfWeek != Enums.DayOfWeek.Saturday && dayOfWeek != Enums.DayOfWeek.Sunday ? new TimeSpan(8,0,0) : null,
                    CloseTime = dayOfWeek != Enums.DayOfWeek.Saturday && dayOfWeek != Enums.DayOfWeek.Sunday ? new TimeSpan(16, 0, 0) : null,
                    IsClosed = dayOfWeek == Enums.DayOfWeek.Saturday || dayOfWeek == Enums.DayOfWeek.Sunday,
                });
            }
            foreach(var defaultWorkingHour in defaultWorkingHours)
            {
                if(await context.DefaultWorkingHours.AnyAsync(x => x.DayOfWeek == defaultWorkingHour.DayOfWeek))
                    await context.DefaultWorkingHours.AddAsync(defaultWorkingHour);
            }

            if (seed)
                await context.SaveChangesAsync();
        }
    }
}
