using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SalaryLib
{
    public class SalaryLib
    {
        // Employee
        public struct Employee
        {
            public string Category;     // PE/TE
            public string Studies;      // NoMSc, MSc, PhD
            public int WorkExperience;  // 0-38
            public int Children;        // 0-6

            // Constructor
            public Employee(string Category, string Studies, int WorkExperience, int Children)
            {
                this.Category = Category;
                this.Studies = Studies;
                this.WorkExperience = WorkExperience;
                this.Children = Children;
            }
        }

        // - Validate Email -
        // This function takes a text as a parameter and returns true or false
        // if the parameter corresponds to email or not, respectively.
        public bool ValidEMAIL(string Email)
        {
            // Check if email is empty
            if (string.IsNullOrWhiteSpace(Email))
                return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        // - Check IBAN -
        // This method accepts the IBAN number as string and returns if it is
        // valid and the bank to which the account belongs
        // Accepted banks:
        //                  * Εθνική Τράπεζα της Ελλάδος Α.Ε.
        //                  * ALPHA BANK
        //                  * ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
        public void CheckIBAN(string IBAN, ref bool ValidIBAN, ref string Bank)
        {
            // Check if value is missing
            if (string.IsNullOrEmpty(IBAN))
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            // Check if IBAN is too small
            if (IBAN.Length < 5)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            // Check the country code (G-16)
            var countryCode = IBAN.Substring(0, 2).ToUpper();

            // Dictionary of each country iban length (only use Greece)
            IDictionary<string, int> countryLengths = new Dictionary<string, int>
            {
                {"GR", 27}
            };

            // Get IBAN Length of given country
            int countryCodeLength;

            var countryCodeKnown = countryLengths.TryGetValue(countryCode, out countryCodeLength);

            // if IBAN not known 
            if (!countryCodeKnown)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            // Check IBAN length (too small or too long)
            if (IBAN.Length < countryCodeLength)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }
            if (IBAN.Length > countryCodeLength)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            // Dictionary of the codes of available Banks (Εθνική - Alpha - Πειραιώς)
            IDictionary<string, string> bankBICCodes = new Dictionary<string, string>
            {
                {"011", "Εθνική Τράπεζα της Ελλάδος Α.Ε."},
                {"014", "ALPHA BANK"},
                {"017", "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε."},
            };

            // Check digits for the Bank code of IBAN
            var BankCode = IBAN.Substring(4, 3).ToUpper();
            string BankName;

            // Get the bank code from the available banks
            var bankCodeKnown = bankBICCodes.TryGetValue(BankCode, out BankName);

            // Check if the bank code exists
            if (!bankCodeKnown)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            /* we put the first four characters to the end of the iban string 
             * GR: 'G' = 16, 'R' = 27  
             */
            string iban_code = "1627" + IBAN.Substring(2, 2);
            string newIban = IBAN.Substring(4, countryCodeLength - 4) + iban_code;

            // Get the modulo 97 of the iban number 
            var remainder = BigInteger.Parse(newIban) % 97;

            // if remainder is not 1 iban is not valid
            if (remainder != 1)
            {
                ValidIBAN = false;
                Bank = "";
                return;
            }

            // Return valid data 
            ValidIBAN = true;
            Bank = BankName;
        }

        // - Calculate Employee Salary -
        public void CalculateSalary(Employee EmplX, ref double GrossSalary, ref double NetIncome)
        {
            // Get Employee info 
            double basicSalary = 0.0;
            double salaryDif = 0.0;
            double startingSal = 0.0;
            int salaryScale = 0;

            // Find the MK level 
            if (EmplX.WorkExperience >= 0 && EmplX.WorkExperience <= 38)
                salaryScale = EmplX.WorkExperience / 2 + 1;
            else
                throw new ArgumentOutOfRangeException("Error at years of work experience (years must be >= 0 and <= 38)");
            

            // Add MK level from studies
            switch (EmplX.Studies)
            {
                case "NoMSc": break;    // no change
                case "MSc":
                    salaryScale += 2;   // Add 2 levels to MK
                    break;
                case "PhD":
                    salaryScale += 6;   // Add 6 levels to MK
                    break;
                default:
                    throw new ArgumentException("Error Unknown Employee Studies (must be 'NoMSc', 'MSc' or 'PhD')");
            }

            // Setting starting salary for PE/ΠΕ, TE/ΤΕ 
            if (EmplX.Category == "PE")
            {
                startingSal = 1092;
                salaryDif = 59; // each level is + 59 euro
            }
            else if (EmplX.Category == "TE")
            {
                startingSal = 1037;
                salaryDif = 55; // each level is + 55 euro
            }
            else
            {
                throw new ArgumentException("Error Unknown Employee Category");
            }
            // Calculate basic salary 
            basicSalary = startingSal + ((salaryScale - 1) * salaryDif);

            // Calculate the Gross Earnings (family benefits - Deductions)
            double GrossEarnings = 0.0;
            double familyBenefit = 0.0;

            switch (EmplX.Children)
            {
                case 0:
                    break;
                case 1:
                    familyBenefit = 50;
                    break;
                case 2:
                    familyBenefit = 70;
                    break;
                case 3:
                    familyBenefit = 120;
                    break;
                case 4:
                    familyBenefit = 170;
                    break;
                case 5:
                    familyBenefit = 240;
                    break;
                case 6:
                    familyBenefit = 310;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Error at number of Children must be (0-6)");
            }

            // Μικτές αποδοχές
            GrossEarnings = basicSalary + familyBenefit;
            GrossSalary = GrossEarnings;

            // Deductions 
            double IKAReserv = GrossEarnings * 0.16;  // I.K.A
            double OAEDReserv = GrossEarnings * 0.01;  // O.A.E.D

            double Reservations = IKAReserv + OAEDReserv;   // sum of reservations

            // calculate the yearly gross earnings
            double yearlyEarnings = (GrossEarnings - Reservations) * 12;

            double taxRate = 0.9;   // default tax rate (< 10.000)

            if (yearlyEarnings > 10000 && yearlyEarnings < 20000)
            {
                taxRate = 0.22;
            }
            else if (yearlyEarnings > 20000 && yearlyEarnings < 30000)
            {
                taxRate = 0.28;
            }
            else if (yearlyEarnings > 30000 && yearlyEarnings < 40000)
            {
                taxRate = 0.36;
            }
            else if (yearlyEarnings > 40000)
            {
                taxRate = 0.44;
            }

            double incomeTax = yearlyEarnings - (yearlyEarnings * taxRate);

            switch (EmplX.Children)
            {
                case 0:
                    incomeTax += 777;
                    break;
                case 1:
                    incomeTax += 810;
                    break;
                case 2:
                    incomeTax += 900;
                    break;
                case 3:
                    incomeTax += 1120;
                    break;
                case 4:
                    incomeTax += 1340;
                    break;
                case 5:
                case 6:
                    incomeTax += (EmplX.Children - 4) * 220;
                    break;
                default:
                    break;
            }
            // Καθαρό εισόδημα
            NetIncome = incomeTax / 12;
        }

        public void CalculateMK(string HiringDate, string Studies, ref int MK, ref int ExcessYears, 
                         ref int ExcessMonths, ref int ExcessDays)
        {
            DateTime HirDate = new DateTime();

            // A list of possible Greek and American date formats
            string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "dd/M/yyyy",
                                 "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

            try
            {
                HirDate = DateTime.ParseExact(HiringDate, formats, new CultureInfo("gr-GR"), DateTimeStyles.None);
            }
            catch (FormatException)
            {
                ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                throw new ArgumentException("Unable to parse date:" + HiringDate);
            }

            // Get the current date.
            DateTime thisDay = DateTime.Today;

            if (HirDate > thisDay)
            {
                ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                throw new ArgumentException(nameof(HiringDate), "Hiring Date exceeds Current Day!");
            }
            var ExpTime = thisDay - HirDate;
            int months = 12 * (thisDay.Year - HirDate.Year) + (thisDay.Month - HirDate.Month);

            if (thisDay.CompareTo(HirDate.AddMonths(months).AddMilliseconds(-500)) <= 0)
            {
                --months;
            }
            int years = months / 12;
            if (years > 38)
            {
                ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                throw new ArgumentException("Work Years exceed 38 (must be between 0 and 38)");
            }
                
            months -= years * 12;
            int days = 0;

            if (months == 0 && years == 0)
            {
                days = ExpTime.Days;
            }
            else
            {
                days = thisDay.Day;
                if (HirDate.Day > thisDay.Day)
                    days += (DateTime.DaysInMonth(thisDay.Year, thisDay.Month - 1) - HirDate.Day);
                else
                    days -= HirDate.Day;
            }
            if (ExcessYears > 38)
            {
                ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                throw new ArgumentException("Years of experience exceed 38");
            }

            int salaryScale = years / 2 + 1;
            var excess = salaryScale * 2 - 2;
            switch (Studies)
            {
                case "NoMSc": break;
                case "MSc":
                    salaryScale += 2;
                    break;
                case "PhD":
                    salaryScale += 6;
                    break;
                case "":
                    ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                    throw new ArgumentException("Employee Studies must not be empty ('NoMSc', 'MSc' or 'PhD')");
                default:
                    ExcessYears = ExcessMonths = ExcessDays = MK = -1;
                    throw new ArgumentException("Unknown Employee Studies (must be 'NoMSc', 'MSc' or 'PhD')");
            }

            MK = salaryScale;

            if (years == excess)
            {
                ExcessYears = 0;
                ExcessMonths = months;
                ExcessDays = days;
            }
            else if (years > excess)
            {
                ExcessYears = years - excess;
                ExcessMonths = months;
                ExcessDays = days;
            }

        }

        public int NonAdultChildren(string[] ChildrenBirthday)
        {
            var cultureInfo = new CultureInfo("gr-GR");
            // A list of possible Greek date formats
            string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "dd/M/yyyy",
                                 "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

            DateTime Now = DateTime.Now;

            int countAge = 0;
            int Years, Months, Days = 0;

            foreach (string dateString in ChildrenBirthday)
            {
                try
                {
                    var BirthdateTime = DateTime.ParseExact(dateString, formats, cultureInfo, DateTimeStyles.None);
                    if ((Now.Year - BirthdateTime.Year) > 0 || (((Now.Year - BirthdateTime.Year) == 0)
                        && ((BirthdateTime.Month < Now.Month) || ((BirthdateTime.Month == Now.Month)
                        && (BirthdateTime.Day <= Now.Day)))))
                    {

                        int DaysInBdayMonth = DateTime.DaysInMonth(BirthdateTime.Year, BirthdateTime.Month);
                        int DaysRemain = Now.Day + (DaysInBdayMonth - BirthdateTime.Day);

                        if (Now.Month > BirthdateTime.Month)
                        {
                            Years = Now.Year - BirthdateTime.Year;
                            Months = Now.Month - (BirthdateTime.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                        else if (Now.Month == BirthdateTime.Month)
                        {
                            if (Now.Day >= BirthdateTime.Day)
                            {
                                Years = Now.Year - BirthdateTime.Year;
                                Months = 0;
                                Days = Now.Day - BirthdateTime.Day;
                            }
                            else
                            {
                                Years = (Now.Year - 1) - BirthdateTime.Year;
                                Months = 11;
                                Days = DateTime.DaysInMonth(BirthdateTime.Year, BirthdateTime.Month) - (BirthdateTime.Day - Now.Day);
                            }
                        }
                        else
                        {
                            Years = (Now.Year - 1) - BirthdateTime.Year;
                            Months = Now.Month + (11 - BirthdateTime.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                        if (Years < 18) countAge++;
                    }
                    else
                    {
                        throw new ArgumentException("Birthday date must be earlier than current date");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Unable to parse '{0}'", dateString);
                    throw new ArgumentException("Birthday date format is not valid");
                }
            }
            return countAge;
        }

        public double MaxNetIncome(Employee[] Empls)
        {
            double maxNetInc = -1.0;
            foreach (Employee emp in Empls)
            {
                // initialize vars for gross and net income
                double CurrNetInc = 0.0;
                double CurrGrossInc = 0.0;
                // Calculate each employee salary:
                CalculateSalary(emp, ref CurrGrossInc, ref CurrNetInc);

                if (CurrNetInc > maxNetInc)
                    maxNetInc = CurrNetInc;
            }
            return maxNetInc;
        }
    }
}
