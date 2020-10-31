using System;
using System.Linq;

namespace SwedishSocialSecurityNumberGenerator
{
    public class SocialSecurityNumberGenerator
    {
        private static Random random;
        private readonly int[] CheckSumDigits = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

        public string GenerateSwedishSocialSecurityNumber(int maxAge = 100, int? seed = null)
        {
            random = seed.HasValue ? new Random(seed.Value) : random = new Random();

            var now = DateTime.Now;
            var year = random.Next(now.Year - maxAge, now.Year);
            var month = random.Next(1, 13);
            var day = random.Next(1, DateTime.DaysInMonth(year, month));
            var county = random.Next(0, 100);
            var gender = random.Next(0, 10);
            var ssnDigitsWithoutChecksum = new int[9];

            ssnDigitsWithoutChecksum[0] = year % 100 / 10;              // |Y|YMMDDNNNN
            ssnDigitsWithoutChecksum[1] = year % 100 % 10;              // Y|Y|MMDDNNNN
            ssnDigitsWithoutChecksum[2] = month < 10 ? 0 : 1;           // YY|M|MDDNNNN
            ssnDigitsWithoutChecksum[3] = month % 10;                   // YYM|M|DDNNNN
            ssnDigitsWithoutChecksum[4] = day < 10 ? 0 : day / 10;      // YYMM|D|DNNNN
            ssnDigitsWithoutChecksum[5] = day % 10;                     // YYMMD|D|NNNN
            ssnDigitsWithoutChecksum[6] = county < 10 ? 0 : county / 10;// YYMMDD|N|NNN
            ssnDigitsWithoutChecksum[7] = county % 10;                  // YYMMDDN|N|NN
            ssnDigitsWithoutChecksum[8] = gender;                       // YYMMDDNN|N|N

            int checksumValue = CalculateChecksumValue(ssnDigitsWithoutChecksum);

            return string.Join(string.Empty, ssnDigitsWithoutChecksum.Append(checksumValue));
        }

        public bool ValidateSwedishSocialSecurityNumber(string ssn)
        {
            var trimmedSsn = ssn.Length == 12 ? ssn.Substring(2, 10) : ssn.Substring(0);
            var trimmedSsnWithoutChecksum = trimmedSsn.Substring(0, 9);
            var digits = trimmedSsnWithoutChecksum.Select(value => (int)char.GetNumericValue(value)).ToArray();
            int checkSum = CalculateChecksumValue(digits);
            var ssnWithValidationNumber = string.Join(string.Empty, digits.Append(checkSum));

            return ssnWithValidationNumber == trimmedSsn;
        }

        private int CalculateChecksumValue(int[] ssnWithoutChecksum)
        {
            var totalChecksumValue = 0;
            for (var index = 0; index < CheckSumDigits.Length; index++)
            {
                var ssnDigit = ssnWithoutChecksum[index];
                var checksumDigit = CheckSumDigits[index];

                var sum = ssnDigit * checksumDigit;
                if (sum >= 10)
                {
                    totalChecksumValue += sum / 10;
                    totalChecksumValue += sum % 10;
                }
                else
                {
                    totalChecksumValue += sum;
                }
            }

            var totalChecksumValueDigit = totalChecksumValue % 10 == 0 ? 10 : totalChecksumValue % 10;
            var checksum = 10 - totalChecksumValueDigit;

            return checksum;
        }
    }
}
