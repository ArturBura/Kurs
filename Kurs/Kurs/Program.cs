using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Ввод данных о машинах
        List<Car> cars = InputCars();

        // Ввод данных о водителях
        List<Driver> drivers = InputDrivers(cars);

        // Ввод данных о таксопарках
        List<TaxiPark> parks = InputParks();

        // Сохранение данных в файлы
        string carFilePath = "cars.txt";
        string driverFilePath = "drivers.txt";
        string taxiParkFilePath = "parks.txt";

        SaveCars(carFilePath, cars);
        SaveDrivers(driverFilePath, drivers);
        SaveParks(taxiParkFilePath, parks);

        // Вывод информации
        Console.WriteLine("\nСписок автомобилей:");
        foreach (var car in cars)
        {
            Console.WriteLine($"ID: {car.Id}, Марка: {car.Model}, Номер: {car.LicensePlate}, В рабочем состоянии: {car.IsOperational}");
        }

        Console.WriteLine("\nСписок водителей:");
        foreach (var driver in drivers)
        {
            Console.WriteLine($"ID: {driver.Id}, ФИО: {driver.FullName}, Выручка: {driver.EarningsByDate.Sum(e => e.Value)}");
        }

        Console.WriteLine("\nСписок таксопарков:");
        foreach (var park in parks)
        {
            Console.WriteLine($"ID: {park.Id}, Название: {park.Name}, Адрес: {park.Address}");
        }

        Console.WriteLine("\nДанные были сохранены в файлы.");
    }

    // Метод для ввода данных о машинах
    public static List<Car> InputCars()
    {
        List<Car> cars = new List<Car>();
        Console.Write("Введите количество автомобилей: ");
        int carCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < carCount; i++)
        {
            Console.WriteLine($"\nВведите данные для автомобиля {i + 1}:");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Марка: ");
            string model = Console.ReadLine();

            Console.Write("Тип: ");
            string type = Console.ReadLine();

            Console.Write("Номер: ");
            string licensePlate = Console.ReadLine();

            Console.Write("В рабочем состоянии (true/false): ");
            bool isOperational = bool.Parse(Console.ReadLine());

            cars.Add(new Car
            {
                Id = id,
                Model = model,
                Type = type,
                LicensePlate = licensePlate,
                IsOperational = isOperational
            });
        }

        return cars;
    }

    // Метод для ввода данных о водителях
    public static List<Driver> InputDrivers(List<Car> cars)
    {
        List<Driver> drivers = new List<Driver>();
        Console.Write("Введите количество водителей: ");
        int driverCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < driverCount; i++)
        {
            Console.WriteLine($"\nВведите данные для водителя {i + 1}:");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("ФИО: ");
            string fullName = Console.ReadLine();

            Console.Write("Номер паспорта: ");
            string passportNumber = Console.ReadLine();

            Console.Write("Опыт работы (в годах): ");
            int experience = int.Parse(Console.ReadLine());

            Console.Write("Номер автомобиля (License Plate): ");
            string licensePlate = Console.ReadLine();
            Car assignedCar = cars.FirstOrDefault(c => c.LicensePlate == licensePlate);

            Console.Write("Введите выручку (в формате YYYY-MM-DD: сумма, например 2023-01-01:100.00): ");
            string earningsInput = Console.ReadLine();
            Dictionary<DateTime, decimal> earningsByDate = new Dictionary<DateTime, decimal>();

            if (!string.IsNullOrEmpty(earningsInput))
            {
                var earnings = earningsInput.Split(',');
                foreach (var earning in earnings)
                {
                    var parts = earning.Split(':');
                    if (parts.Length == 2 && DateTime.TryParse(parts[0], out DateTime date) && decimal.TryParse(parts[1], out decimal value))
                    {
                        earningsByDate[date] = value;
                    }
                }
            }

            drivers.Add(new Driver
            {
                Id = id,
                FullName = fullName,
                PassportNumber = passportNumber,
                Experience = experience,
                AssignedCar = assignedCar,
                EarningsByDate = earningsByDate
            });
        }

        return drivers;
    }

    // Метод для ввода данных о таксопарках
    public static List<TaxiPark> InputParks()
    {
        List<TaxiPark> parks = new List<TaxiPark>();
        Console.Write("Введите количество таксопарков: ");
        int parkCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < parkCount; i++)
        {
            Console.WriteLine($"\nВведите данные для таксопарка {i + 1}:");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Название: ");
            string name = Console.ReadLine();

            Console.Write("Адрес: ");
            string address = Console.ReadLine();

            parks.Add(new TaxiPark
            {
                Id = id,
                Name = name,
                Address = address
            });
        }

        return parks;
    }

    // Метод для сохранения данных о машинах в файл
    public static void SaveCars(string path, List<Car> cars)
    {
        var lines = cars.Select(car =>
            $"{car.Id}|{car.Model}|{car.Type}|{car.LicensePlate}|{car.IsOperational}").ToList();

        File.WriteAllLines(path, lines);
    }

    // Метод для сохранения данных о водителях в файл
    public static void SaveDrivers(string path, List<Driver> drivers)
    {
        var lines = drivers.Select(driver =>
        {
            var earnings = string.Join(",", driver.EarningsByDate.Select(kv => $"{kv.Key:yyyy-MM-dd}:{kv.Value}"));
            return $"{driver.Id}|{driver.FullName}|{driver.PassportNumber}|{driver.Experience}|{driver.AssignedCar.LicensePlate}|{earnings}";
        }).ToList();

        File.WriteAllLines(path, lines);
    }

    // Метод для сохранения данных о таксопарках в файл
    public static void SaveParks(string path, List<TaxiPark> parks)
    {
        var lines = parks.Select(taxiPark =>
            $"{taxiPark.Id}|{taxiPark.Name}|{taxiPark.Address}").ToList();

        File.WriteAllLines(path, lines);
    }
}

// Классы для представления данных
public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string Type { get; set; }
    public string LicensePlate { get; set; }
    public bool IsOperational { get; set; }
}

public class Driver
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public int Experience { get; set; }
    public Dictionary<DateTime, decimal> EarningsByDate { get; set; } = new Dictionary<DateTime, decimal>();
    public Car AssignedCar { get; set; }
}

public class TaxiPark
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}
