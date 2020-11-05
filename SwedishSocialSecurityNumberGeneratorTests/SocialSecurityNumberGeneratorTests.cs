using NUnit.Framework;
using SwedishSocialSecurityNumberGenerator;
using System.IO;
using System.Linq;

namespace SwedishSocialSecurityNumberGeneratorTests
{
    public class SocialSecurityNumberGeneratorTests
    {
        private static string[] ReadSocialSecurityNumbersFromFile()
        {
            /*
             * Social security numbers published by The Swedish Tax Agency for testing purpose
             * https://www.dataportal.se/sv/datasets/6_67959/testpersonnummer
             */
            var ssnList = File
                .ReadAllLines(@$"{Directory.GetCurrentDirectory()}\Testpersonnummer_170117.csv")
                .Skip(1);
            return ssnList.ToArray();
        }

        [Test]
        public void GenerateSwedishSocialSecurityNumber_Should_Generate_Valid_Social_Security_Number()
        {
            var socialSecurityNumberGenerator = new SocialSecurityNumberGenerator();
            var socialSecurityNumber = socialSecurityNumberGenerator.GenerateSwedishSocialSecurityNumber(100);
            Assert.IsTrue(socialSecurityNumberGenerator.ValidateSwedishSocialSecurityNumber(socialSecurityNumber));
        }
       
        [Test, TestCaseSource("ReadSocialSecurityNumbersFromFile")]
        public void ValidateSwedishSocialSecurityNumber_Should_Return_True(string ssn)
        {
            var result = new SocialSecurityNumberGenerator().ValidateSwedishSocialSecurityNumber(ssn);
            Assert.IsTrue(result);
        }
    }
}