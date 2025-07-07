using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Task46.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthDateController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Post(
            [FromForm] string? name,
            [FromForm] int? years,
            [FromForm] int? months,
            [FromForm] int? days)
        {
            string personName = string.IsNullOrWhiteSpace(name) ? "anonymous" : name;

            if (!years.HasValue || !months.HasValue || !days.HasValue)
            {
                return $"Hello {personName}, I can’t calculate your age without knowing your birthdate!";
            }

            if (months.Value < 1 || months.Value > 12 || days.Value < 1 || days.Value > 31)
            {
                return $"Hello {personName}, please provide a valid birthdate!";
            }

            DateTime birthDate;
            try
            {
                birthDate = new DateTime(years.Value, months.Value, days.Value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return $"Hello {personName}, the provided birthdate ({years.Value}-{months.Value}-{days.Value}) is not a valid date.";
            }

            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            string response = $"Hello {personName}, you are {age} years old.";

            if (birthDate.Month == today.Month && birthDate.Day == today.Day)
            {
                response += $" Also, Happy Birthday!";
            }

            return response;
        }
    }
}
