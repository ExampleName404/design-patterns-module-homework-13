namespace DesignPatternsModuleHomework13.Classes.StateMachine.States;

public class TicketDispensedState : ITicketMachineState
{
    private readonly TicketMachine _context;

    public TicketDispensedState(TicketMachine context)
    {
        _context = context;
        Console.WriteLine("Билет выдан. Возврат в исходное состояние.");
    }

    public void SelectTicket(Classes.Models.TicketType ticket)
    {
        Console.WriteLine("Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
        _context.SelectTicket(ticket);
    }

    public void InsertMoney(decimal amount)
    {
        Console.WriteLine("Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
        _context.InsertMoney(amount);
    }

    public void DispenseTicket() => Console.WriteLine("Билет уже выдан.");

    public void Cancel()
    {
        Console.WriteLine("Нечего отменять. Автомат готов к новой покупке.");
        _context.SetState(new IdleState(_context));
    }
}
