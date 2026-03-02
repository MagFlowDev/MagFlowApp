using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class WorkingHourMapper
    {
        public static DefaultWorkingHourDTO ToDTO(this DefaultWorkingHour workingHour)
        {
            return new DefaultWorkingHourDTO()
            {
                Id = workingHour.Id,
                CloseTime = workingHour.CloseTime,
                DayOfWeek = workingHour.DayOfWeek,
                IsClosed = workingHour.IsClosed,
                OpenTime = workingHour.OpenTime,
            };
        }

        public static List<DefaultWorkingHourDTO> ToDTO(this IEnumerable<DefaultWorkingHour> workingHours)
        {
            return workingHours.Select(x => ToDTO(x)).ToList();
        }



        public static DefaultWorkingHour ToEntity(this DefaultWorkingHourDTO workingHourDTO)
        {
            return new DefaultWorkingHour()
            {
                CloseTime = workingHourDTO.CloseTime,
                IsClosed = workingHourDTO.IsClosed,
                OpenTime = workingHourDTO.OpenTime,
                DayOfWeek = workingHourDTO.DayOfWeek,
            };
        }

        public static DefaultWorkingHour ToEntity(this DefaultWorkingHourDTO workingHourDTO, DefaultWorkingHour oldWorkingHour)
        {
            oldWorkingHour.IsClosed = workingHourDTO.IsClosed;
            oldWorkingHour.OpenTime = workingHourDTO.OpenTime ?? oldWorkingHour.OpenTime;
            oldWorkingHour.CloseTime = workingHourDTO.CloseTime ?? oldWorkingHour.CloseTime;
            return oldWorkingHour;
        }

        public static List<DefaultWorkingHour> ToEntity(this IEnumerable<DefaultWorkingHourDTO> workingHoursDTOs)
        {
            return workingHoursDTOs.Select(x => x.ToEntity()).ToList();
        }

        public static List<DefaultWorkingHour> ToEntity(this IEnumerable<DefaultWorkingHourDTO> workingHoursDTOs, IEnumerable<DefaultWorkingHour> oldWorkingHours)
        {
            List<DefaultWorkingHour> entities = new List<DefaultWorkingHour>();
            foreach (var workingHourDTO in workingHoursDTOs)
            {
                var workingHour = oldWorkingHours.FirstOrDefault(x => x.Id == workingHourDTO.Id);
                if (workingHour != null && workingHour.Id != 0)
                    entities.Add(workingHourDTO.ToEntity(workingHour));
                else
                    entities.Add(workingHourDTO.ToEntity());
            }
            return entities;
        }
    }
}
