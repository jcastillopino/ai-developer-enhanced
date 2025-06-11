using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace BlazorAI.Plugins;

class TimePlugin
{

    // Return the current Date Time
    [KernelFunction("get_current_time")]
    [Description("Returns the current date and time.")]
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }    // Return the Year for a date passed in as a parameter
    [KernelFunction("get_year")]
    [Description("Returns the year for a given date. Input must be a valid date string in ISO format (yyyy-MM-dd) or a standard date format.")]
    public int GetYear([Description("The date to get the year for. Use ISO format like '2025-06-11' or standard date format.")] string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            return date.Year;
        }
        throw new ArgumentException($"Could not parse '{dateString}' as a valid date. Please use ISO format (yyyy-MM-dd) or a standard date format.");
    }

    // Return the Month for a date passed in as a parameter
    [KernelFunction("get_month")]
    [Description("Returns the name of the month for a given date. Input must be a valid date string in ISO format (yyyy-MM-dd) or a standard date format.")]
    public string GetMonth([Description("The date to get the month for. Use ISO format like '2025-06-11' or standard date format.")] string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            return date.ToString("MMMM");
        }
        throw new ArgumentException($"Could not parse '{dateString}' as a valid date. Please use ISO format (yyyy-MM-dd) or a standard date format.");
    }

    // Return the name of the Day of Week for a date passed in as a parameter
    [KernelFunction("get_day_of_week")]
    [Description("Returns the day of the week for a given date. Input must be a valid date string in ISO format (yyyy-MM-dd) or a standard date format.")]
    public string GetDayOfWeek([Description("The date to get the day of week for. Use ISO format like '2025-06-11' or standard date format.")] string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            // Return the name of the day of the week
            return date.DayOfWeek.ToString();
        }
        throw new ArgumentException($"Could not parse '{dateString}' as a valid date. Please use ISO format (yyyy-MM-dd) or a standard date format.");
    }

}