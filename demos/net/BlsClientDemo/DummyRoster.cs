using System;
using System.Collections.Generic;
using System.Text;

namespace BlsClientDemo
{
    internal static class DummyRoster
    {
        static DateTime StartDate = DateTime.UtcNow.Date;
        static int NextFlightNumber = 1000;
        static int NextHour = 0;
        static int NextMinute = 0;
        static int NumberOfActivities = 2;
        private static IEnumerable<RosterActivity> CreateDummyRoster(string crewName)
        {
            for (var i = 0; i < NumberOfActivities; i++)
            {
                StartDate = StartDate.AddDays(i);

                NextFlightNumber++;
                yield return new RosterActivity()
                {
                    ActivityId = Guid.NewGuid(),
                    Start = StartDate.AddHours(NextHour++).AddMinutes(NextMinute++ * 5),
                    End = StartDate.AddHours(NextHour++).AddMinutes(NextMinute++ * 15),
                    FlightNumber = NextFlightNumber,
                    Code = "Act" + NextFlightNumber,
                    CrewNumer = crewName,

                };
            }
            NumberOfActivities *= 2;
        }
        public static Dictionary<Guid, RosterActivity> Create(params string[] crewNames)
        {
            Dictionary<Guid, RosterActivity> ret = [];
            foreach (var crewName in crewNames)
            {
                foreach (var dummyRosterItem in CreateDummyRoster(crewName))
                {
                    ret.Add(dummyRosterItem.ActivityId, dummyRosterItem);
                }

            }
            return ret;
        }
    }
}
