namespace TodoApp.Domain.Entities;

public class Progression {
    public Progression(DateTime date,
                       decimal percent) {
        Date = date;
        Percent = percent;
    }

    public DateTime Date { get; }
    public decimal Percent { get; }

}
