namespace Core.Domain.Helpers;

public class FormatHour
{
    public static TimeSpan More10Minutes(TimeSpan hour)
    {
        var newTime = hour.Add(new TimeSpan(0, 10, 0)); // Suma 10 minutos
        return newTime; // Devolver el nuevo TimeSpan
    }
    
    public static TimeSpan MoreHours(TimeSpan hour)
    {
        var newTime = hour.Add(new TimeSpan(1, 0, 0)); // Sumar una hora
        return newTime; // Devolver el nuevo TimeSpan
    }

    public static TimeSpan LessHour(TimeSpan hour)
    {
        var newTime = hour.Subtract(new TimeSpan(1, 0, 0)); // Resta una hora
        return newTime;
    }

}