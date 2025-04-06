namespace TodoApp.Domain.Entities;

public class Progression 
{
    public Progression(DateTime date,
                       decimal percent) 
    {
        Date = date;
        Percent = percent;
    }
    private Progression() { }

    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }

}
