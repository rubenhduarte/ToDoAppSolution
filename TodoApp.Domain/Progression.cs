namespace TodoApp.Domain;

public class Progression
{
    public Progression(DateTime date, 
                       decimal percent)
    {
        AccionDate = date;
        Percent = percent;
    }

    public DateTime AccionDate { get; }
    public decimal Percent { get; }
        
}
