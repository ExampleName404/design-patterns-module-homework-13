namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class MoneyReceivedState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public MoneyReceivedState(TicketMachine context) => _context = context;

    public void SelectTicket(Classes.Models.TicketType ticket) => throw new InvalidOperationException("Билет уже оплачен. Отмените операцию для смены билета.");

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Достаточно средств, внесение дополнительных денег не требуется.");
    }

    public void DispenseTicket()
    {
        if (_context.SelectedTicket is null)
        {
            throw new InvalidOperationException("Билет не выбран.");
        }

        _context.Payments.DeductCost(_context.SelectedTicket);
        _context.Inventory.Reserve(_context.SelectedTicket);
        var change = _context.Payments.ReturnChange();
        Console.WriteLine(change > 0 ? $"Выдан билет и сдача {change:C}." : "Выдан билет без сдачи.");
        _context.SetState(new TicketDispensedState(_context));
    }

    public void Cancel()
    {
        var change = _context.Payments.ReturnChange();
        Console.WriteLine($"Транзакция отменена. Возвращено {change:C}");
        _context.SetState(new TransactionCanceledState(_context));
    }
}
