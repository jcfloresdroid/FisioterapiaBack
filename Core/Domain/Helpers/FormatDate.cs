namespace Core.Domain.Helpers;

public static class FormatDate
{
    //Se usa para saber la edad
    public static int DateToYear(DateTime date)
    {
        //Resta los años
        int age = DateTime.Today.Year - date.Year;
            
        //Resta un año si el mes actual es menor al mes de nacimiento
        if(DateTime.Today.Month < date.Month)
            age -= 1;
        //Si el mes actual es igual al mes de nacimiento y si el día actual es menor o igual al día de nacimiento
        else if (DateTime.Today.Month == date.Month && DateTime.Today.Day <= date.Day){
            age -= 1;
        }
        
        return age;
    }
    
    //Lo usaremos para obtener la fecha local
    public static DateTime DateLocal()
    {
        // Obtener la hora actual en UTC
        DateTime utcNow = DateTime.UtcNow;
        
        // Obtener la hora local en Campeche (Central Standard Time)
        TimeZoneInfo campecheTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); //Distribuciones Linux
        //TimeZoneInfo campecheTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"); //Distribuciones Windows
        DateTime campecheTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, campecheTimeZone);

        return campecheTime;
    }

    public static DateTime StartOfWeek()
    {
        var today = DateLocal();
        int delta = (today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek) - (int)DayOfWeek.Monday;
        DateTime startOfWeek = today.AddDays(-delta);
    
        return startOfWeek;
    }
    
    public static DateTime EndOfWeek()
    {
        DateTime endOfWeek = StartOfWeek().AddDays(6);
        
        return endOfWeek;
    }
}