using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kurs
{
    using Modules;

    public static class FileStorage
    {
        public static List<Car> LoadCars(string path)
        {
            var line = File.Exists(path) ? File.ReadAllText(path) : "";

            return line.Split(';')
                       .Where(s => !string.IsNullOrWhiteSpace(s))
                       .Select(s =>
                       {
                           var p = s.Split('|');
                           return new Car
                           {
                               Id = int.Parse(p[0]),
                               Firm = p[1],
                               Model = p[2],
                               Type = p[3],
                               LicensePlate = p[4],
                               IsOperational = bool.Parse(p[5])
                           };
                       })
                       .ToList();
        }

        public static void SaveCars(string path, List<Car> cars)
        {
            var line = string.Join(";", cars.Select(c => $"{c.Id}|{c.Firm}|{c.Model}|{c.Type}|{c.LicensePlate}|{c.IsOperational}"));
            File.WriteAllText(path, line);
        }

        public static List<Driver> LoadDrivers(string path, List<Car> cars)
        {
            var line = File.Exists(path) ? File.ReadAllText(path) : "";

            return line.Split(';')
                       .Where(s => !string.IsNullOrWhiteSpace(s))
                       .Select(s =>
                       {
                           var p = s.Split('|');
                           var driver = new Driver
                           {
                               Id = int.Parse(p[0]),
                               FullName = p[1],
                               PassportNumber = p[2],
                               Experience = int.Parse(p[3]),
                               AssignedCar = cars.FirstOrDefault(c => c.LicensePlate == p[4])
                           };

                           var earnings = p[5].Split(',')
                                               .Where(e => !string.IsNullOrWhiteSpace(e))
                                               .ToArray();

                           foreach (var e in earnings)
                           {
                               var kv = e.Split(':');
                               driver.EarningsByDate[DateTime.Parse(kv[0])] = decimal.Parse(kv[1]);
                           }

                           return driver;
                       })
                       .ToList();
        }

        public static List<TaxiPark> LoadParks(string path)
        {
            var line = File.Exists(path) ? File.ReadAllText(path) : "";

            return line.Split(';')
                       .Where(s => !string.IsNullOrWhiteSpace(s))
                       .Select(s =>
                       {
                           var p = s.Split('|');
                           return new TaxiPark { Id = int.Parse(p[0]), Name = p[1], Address = p[2] };
                       })
                       .ToList();
        }
    }

    public class FileStorageHandler
    {
        private readonly string _driversFilePath;
        private readonly string _carsFilePath;
        private readonly string _taxiParkFilePath;

        public FileStorageHandler(string driversFilePath, string carsFilePath, string taxiParkFilePath)
        {
            _driversFilePath = driversFilePath;
            _carsFilePath = carsFilePath;
            _taxiParkFilePath = taxiParkFilePath;
        }

        public List<Car> LoadCars()
        {
            var line = File.Exists(_carsFilePath) ? File.ReadAllText(_carsFilePath) : "";
            return line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s =>
            {
                var p = s.Split('|');
                return new Car
                {
                    Id = int.Parse(p[0]),
                    Firm = p[1],
                    Model = p[2],
                    Type = p[3],
                    LicensePlate = p[4],
                    IsOperational = bool.Parse(p[5])
                };
            }).ToList();
        }

        public List<Driver> LoadDrivers(List<Car> cars)
        {
            var line = File.Exists(_driversFilePath) ? File.ReadAllText(_driversFilePath) : "";
            return line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s =>
            {
                var p = s.Split('|');
                var driver = new Driver
                {
                    Id = int.Parse(p[0]),
                    FullName = p[1],
                    PassportNumber = p[2],
                    Experience = int.Parse(p[3]),
                    AssignedCar = cars.FirstOrDefault(c => c.LicensePlate == p[4])
                };

                if (p.Length > 5)
                {
                    var earnings = p[5].Split(',').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
                    foreach (var e in earnings)
                    {
                        var kv = e.Split(':');
                        driver.EarningsByDate[DateTime.Parse(kv[0])] = decimal.Parse(kv[1]);
                    }
                }

                return driver;
            }).ToList();
        }

        public List<TaxiPark> LoadTaxiParks()
        {
            var line = File.Exists(_taxiParkFilePath) ? File.ReadAllText(_taxiParkFilePath) : "";
            return line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s =>
            {
                var p = s.Split('|');
                return new TaxiPark
                {
                    Id = int.Parse(p[0]),
                    Name = p[1],
                    Address = p[2]
                };
            }).ToList();
        }

        public void SaveDrivers(List<Driver> drivers)
        {
            var lines = drivers.Select(driver =>
            {
                var earnings = string.Join(",", driver.EarningsByDate.Select(kv => $"{kv.Key:yyyy-MM-dd}:{kv.Value}"));
                return $"{driver.Id}|{driver.FullName}|{driver.PassportNumber}|{driver.Experience}|{driver.AssignedCar.LicensePlate}|{earnings}";
            }).ToList();

            File.WriteAllLines(_driversFilePath, lines);
        }

        public void SaveCars(List<Car> cars)
        {
            var lines = cars.Select(car =>
                $"{car.Id}|{car.Firm}|{car.Model}|{car.Type}|{car.LicensePlate}|{car.IsOperational}").ToList();

            File.WriteAllLines(_carsFilePath, lines);
        }

        public void SaveTaxiParks(List<TaxiPark> taxiParks)
        {
            var lines = taxiParks.Select(taxiPark =>
                $"{taxiPark.Id}|{taxiPark.Name}|{taxiPark.Address}").ToList();

            File.WriteAllLines(_taxiParkFilePath, lines);
        }
    }
}
