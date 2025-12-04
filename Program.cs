using DesignPatternsModuleHomework13.Classes.Models;
using DesignPatternsModuleHomework13.Classes.Services;
using DesignPatternsModuleHomework13.Classes.StateMachine;

namespace DesignPatternsModuleHomework13.Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new TicketCatalogService();
            catalog.Seed(
                new TicketType("S", "Стандартный билет", 100m),
                new TicketType("P", "Премиум билет", 200m),
                new TicketType("C", "Детский билет", 50m)
            );

            var inventory = new InventoryService();
            inventory.SeedStock(new[]
            {
                (catalog.Find("S")!, 10),
                (catalog.Find("P")!, 5),
                (catalog.Find("C")!, 3)
            });

            var payment = new PaymentService();
            var machine = new TicketMachine(payment, inventory);

            Console.WriteLine("=== Автомат по продаже билетов ===");
            Console.WriteLine("Доступные билеты:");
            foreach (var ticket in catalog.All())
            {
                Console.WriteLine($"{ticket.Code}: {ticket.Name} ({ticket.Price:C})");
            }

            var script = new (string command, string? argument)[]
            {
                ("select", "S"),
                ("insert", "100"),
                ("dispense", null),
                ("select", "P"),
                ("insert", "50"),
                ("cancel", null),
                ("select", "C"),
                ("insert", "20"),
                ("insert", "30"),
                ("dispense", null)
            };

            foreach (var (command, argument) in script)
            {
                Console.WriteLine($"\n> {command} {argument}".Trim());

                try
                {
                    switch (command)
                    {
                        case "select":
                            if (string.IsNullOrWhiteSpace(argument))
                            {
                                Console.WriteLine("Укажите код билета.");
                                continue;
                            }

                            var ticket = catalog.Find(argument);
                            if (ticket is null)
                            {
                                Console.WriteLine("Билет не найден.");
                                continue;
                            }

                            machine.SelectTicket(ticket);
                            Console.WriteLine($"Выбран билет {ticket.Name}. Стоимость {ticket.Price:C}");
                            break;

                        case "insert":
                            if (string.IsNullOrWhiteSpace(argument) || !decimal.TryParse(argument, out var amount))
                            {
                                Console.WriteLine("Укажите корректную сумму.");
                                continue;
                            }

                            machine.InsertMoney(amount);
                            break;

                        case "dispense":
                            machine.DispenseTicket();
                            break;

                        case "cancel":
                            machine.Cancel();
                            break;

                        default:
                            Console.WriteLine("Неизвестная команда.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.ReadKey();
        }
    }
}
