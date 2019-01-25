﻿using System.Collections.Generic;
using System.IO;

namespace ExternalDependencies.ReservationsProvider
{
    public class ReservationsProvider
    {
        private readonly Dictionary<string, BookedSeatsDto> _repository = new Dictionary<string, BookedSeatsDto>();

        public ReservationsProvider()
        {
            var directoryName = $"{GetExecutingAssemblyDirectoryFullPath()}\\AuditoriumLayouts\\";

            foreach (var fileFullName in Directory.EnumerateFiles($"{directoryName}"))

                if (fileFullName.Contains("_booked_seats.json"))
                {
                    var fileName = Path.GetFileName(fileFullName);

                    var eventId = Path.GetFileName(fileName.Split("-")[0]);

                    _repository[eventId] = JsonFile.ReadFromJsonFile<BookedSeatsDto>(fileFullName);
                }
        }

        public BookedSeatsDto GetBookedSeats(string showId)
        {
            if (_repository.ContainsKey(showId)) return _repository[showId];

            return new BookedSeatsDto();
        }

        private static string GetExecutingAssemblyDirectoryFullPath()
        {
            var directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

            if (directoryName.StartsWith(@"file:\"))
            {
                directoryName = directoryName.Substring(6);
            }

            return directoryName;
        }
    }
}