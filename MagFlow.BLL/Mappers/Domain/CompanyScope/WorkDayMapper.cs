using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class WorkDayMapper
    {
        public static WorkDayDTO ToDTO(this WorkDay workDay)
        {
            return new WorkDayDTO()
            {
                Id = workDay.Id,
                CloseTime = workDay.CloseTime,
                Date = workDay.Date,
                IsClosed = workDay.IsClosed,
                OpenTime = workDay.OpenTime,
                Reason = workDay.Reason,
            };
        }

        public static List<WorkDayDTO> ToDTO(this IEnumerable<WorkDay> workDays)
        {
            return workDays.Select(x => ToDTO(x)).ToList();
        }



        public static WorkDay ToEntity(this WorkDayDTO workDayDTO)
        {
            return new WorkDay()
            {
                CloseTime = workDayDTO.CloseTime,
                IsClosed = workDayDTO.IsClosed,
                OpenTime = workDayDTO.OpenTime,
                Reason = workDayDTO.Reason,
                Date = workDayDTO.Date,
            };
        }

        public static WorkDay ToEntity(this WorkDayDTO workDayDTO, WorkDay oldWorkDay)
        {
            oldWorkDay.IsClosed = workDayDTO.IsClosed;
            oldWorkDay.OpenTime = workDayDTO.OpenTime ?? oldWorkDay.OpenTime;
            oldWorkDay.CloseTime = workDayDTO.CloseTime ?? oldWorkDay.CloseTime;
            oldWorkDay.Reason = workDayDTO?.Reason ?? oldWorkDay.Reason;
            return oldWorkDay;
        }

        public static List<WorkDay> ToEntity(this IEnumerable<WorkDayDTO> workDaysDTOs)
        {
            return workDaysDTOs.Select(x => x.ToEntity()).ToList();
        }

        public static List<WorkDay> ToEntity(this IEnumerable<WorkDayDTO> workDaysDTOs, IEnumerable<WorkDay> oldWorkDays)
        {
            List<WorkDay> entities = new List<WorkDay>();
            foreach(var workDayDTO in workDaysDTOs)
            {
                var workDay = oldWorkDays.FirstOrDefault(x => x.Id == workDayDTO.Id);
                if (workDay != null && workDay.Id != 0)
                    entities.Add(workDayDTO.ToEntity(workDay));
                else
                    entities.Add(workDayDTO.ToEntity());
            }
            return entities;
        }
    }
}
