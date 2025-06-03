namespace SocialMedia.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var Today = DateTime.Today;

            var ageInYears = Today.Year - dateOfBirth.Year;

            if (dateOfBirth.Month > Today.Month)
            {
                ageInYears--;
            }

            return ageInYears;
        }
    }
}
