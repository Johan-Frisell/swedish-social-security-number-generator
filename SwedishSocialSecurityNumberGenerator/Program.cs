using System;

namespace SwedishSocialSecurityNumberGenerator
{
    public class Program
    {
        static void Main()
        {
            var socialSecurityNumberGenerator = new SocialSecurityNumberGenerator();
            Console.WriteLine(socialSecurityNumberGenerator.GenerateSwedishSocialSecurityNumber(100));
        }
    }
}
