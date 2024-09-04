using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SalaryLib.SalaryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryLib.Tests
{
    [TestClass()]
    public class SalaryLibTests
    {
        // Hint Messages 

        // - Email Test - 
        public static string email_test_valid = "Expected valid email";
        public static string[] email_test_invalid = { "Email is Empty", "Email with missing @ symbol.", "Email contains Whitespace",
                                                      "Email contains multiple @ symbols (must contain only one @)", "Email local-part is too long (must be <= 64 characters)",
                                                      "Email subdomain too long (must be <= 64 characters)",
                                                      "Email too long (must be <= 256 characters)", "Email IP literal is Empty",
                                                      "Email contains invalid characters in Local part", "Email local-part starts with dot",
                                                      "Email local-part ends with dot", "Email contains invalid characters", "Email missing local part"};
        // - IBAN Test - 
        public static string[] iban_test_length = { "IBAN length too short (< 27)", "IBAN length too long (> 27)" };
        public static string[] iban_valid_test_bank = { "IBAN has valid length, bank code and account number" ,
                                                        "Expected Bank: Εθνική Τράπεζα της Ελλάδος Α.Ε." , 
                                                         "Expected Bank: ALPHA BANK", "Expected Bank: ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε." };
        public static string iban_test_bank = "Bank code not Found (Not supported)";
        public static string iban_test_country = "Country code not Found (Not supported)";
        public static string[] iban_test_iban = { "IBAN not valid", "IBAN not digit" };

        // - Calculate Salary Test - 
        public static string salary_test_valid = "Expected valid Salary calculations";
        public static string[] salary_test_msg = { "Employee values are empty", "Employee values of work experience or kids are negative.",
                                                   "Employee values exceed range (workExp 0-38, children 0-6).", "Employee has invalid data.",
                                                   "Wrong Gross Earnings calculation.", "Wrong Net Income calculation." };


        // - Calculate MK Test -
        public static string calc_mk_tes_valid = "MK level and Excess date is valid.";
        public static string[] calc_mk_test_msg = { "Work years are more than 38.", "Hiring Date is future date.", "Invalid Hiring Date",
                                                    "Invalid day in Hiring Date", "Invalid month in Hiring Date", "Invalid year in Hiring Date",
                                                    "Studies attribute is invalid", "Studies attribute is empty", "Hiring date is empty"};


        // - Children BirthDate Test -
        public static string birthdate_child_valid = "Expected valid number of children";

        public static string[] birthdate_child_test_msg = { "The date values must consist of positive numbers (> 0).",
                                                            "Birthdate extends current date", 
                                                            "Days must be lower than 31", "Months must be lower than 12", "Months must be 1900-2022" };

        // - Max Net Income Test -
        public static string max_netincome_valid = "Expected valid maximum Net Income";

        public static string[] max_netincome_test_msg = { "Incorrect calculation of max Net Income.", 
                                                            "Values of Employee are invalid (must contain valid data)" };



        // TEST METHODS: 
        [TestMethod()]
        public void ValidEMAILTestValid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib testEmail = new SalaryLib();

            // Creating array with valid emails 
            object[,] testcases =
            {
                // Test Cases for valid emails
                {1, "cs171059@uniwa.gr", true, email_test_valid },
                {2, "\"Abc\\@def\"@example.com", true, email_test_valid },
                {3, "\"Abc@def\"@example.com", true, email_test_valid },
                {4, "valid@" + new string ('a', 64) + ".com", true, email_test_valid },  //max subdomain length (64 characters)
                {5, "$A12345@example.com", true, email_test_valid },
                
                {6,"!def!xyz%abc@example.com", true, email_test_valid },
                {7,"\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@strange.example.com", true, email_test_valid },
                {8, "very.common@example.com", true, email_test_valid },
                {9, "local@sub.domains.com", true, email_test_valid },
                {10,"\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@example.org", true, email_test_valid },

                {11,"country-code-tld@sld.uk", true, email_test_valid },
                {12,"01234567890@numbers-in-local.net", true, email_test_valid },
                {13,"the-character-limit@for-each-part.of-the-domain.is-sixty-three-characters.this-is-exactly-sixty-three-characters-so-it-is-valid-blah-blah.com", true, email_test_valid },
                {14,"\"quoted-at-sign@sld.org\"@sld.com", true, email_test_valid },
                {15, "user@[IPv6:2001:db8:1ff::a0b:dbd0]", true, email_test_valid },
                {15, "εμαιλ@εχαμπλε.κομ", true, email_test_valid }
            };

            bool failed = false;

            for (int i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    Assert.IsTrue(testEmail.ValidEMAIL((string)testcases[i, 1]), (string)testcases[i, 3]);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Email): EMAILVAL{0}: {1} - {2} \n \t Hint: {3}\n \t Reason: {4} ",
                        (int)testcases[i, 0], (string)testcases[i, 1], (bool)testcases[i, 2], (string)testcases[i, 3], e.Message);
                };
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail("Valid Email Tests failed.");
        }
        [TestMethod()]
        public void ValidEMAILTestInvalid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib testEmail = new SalaryLib();

            // Creating array with invalid emails 
            object[,] testcases =
            {
                // Test Cases for invalid emails
                {1, string.Empty, false, email_test_invalid[0] }, // Empty email 
                {2, "invalid", false, email_test_invalid[1] },    // missing @ sign
                {3, "invalid @", false, email_test_invalid[2] },  // Contains white space
                {4, "invalid@domain1.com@domain2.com", false, email_test_invalid[3] },   // contains multiple @ 
                {5, new string ('a', 65) + "@example.com", false, email_test_invalid[4] },   // local-part too long
                {6, "invalid@" + new string ('a', 65) + ".com", false, email_test_invalid[5] },   // subdomain too long
                {7, "invalid@" + new string ('a', 60) + "." + new string ('b', 60) + "." + new string ('c', 60) + "." + new string ('d', 62) + ".com", false, email_test_invalid[6] },   // too long (256 characters)
                {8, "invalid@[]", false, email_test_invalid[7] },   // empty IP literal
                // Examples from wikipedia
                {9, "this is\"not\\allowed@example.com", false, email_test_invalid[2] },
                {10, "* .local-starts-with-dot@sld.com", false, email_test_invalid[9] },  // local part starts with dot
                {11, "local-ends-with-dot.@sld.com", false, email_test_invalid[10] }, // // local part ends with dot
                {12, "@missing-local.org" , false, email_test_invalid[12] },  // missing local part
                {13, "invalid-characters-in-sld@! \"#$%(),/;<>_[]`|.org", false, email_test_invalid[11] },    // invalid characters in sld
                {14, "! #$%`|@invalid-characters-in-local.org", false, email_test_invalid[11] }    // Invalid characters
            };

            bool failed = false;

            for (int i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    Assert.IsFalse(testEmail.ValidEMAIL((string)testcases[i, 1]), (string)testcases[i, 3]);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Email): EMAILINVAL{0}: {1} - {2} \n \t Reason: {3}",
                        (int)testcases[i, 0], (string)testcases[i, 1], (bool)testcases[i, 2], e.Message);
                };
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail("Invalid Email Tests failed.");
        }

        [TestMethod()]
        public void CheckIBANTest_Length()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Creating array with INVALID (iban too short)
            object[,] testcases =
            {
                // Test Cases for too short iban code
                {1, "", false, "", iban_test_length[0] },  // Empty iban
                {2, "GR16", false, "", iban_test_length[0] },  // Very short iban
                {3, "GR16011012500000456123695", false, "", iban_test_length[0] },  // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {4, "GR16011012500006123695", false, "", iban_test_length[0] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {5, "GR1601401250554646361695", false, "", iban_test_length[0] },     // ALPHA BANK
                {6, "GR16014055405660000018995", false, "", iban_test_length[0] },    // ALPHA BANK
                {7, "GR16017088955000405695", false, "", iban_test_length[0] },    // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {8, "GR1601708655000695", false, "", iban_test_length[0] },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.

                // Test Cases for too long iban code
                {9, "GR160110125000005465654645612369015", false, "", iban_test_length[1] },  // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {10, "GR1601101250000623621651616516123695", false, "", iban_test_length[1] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {11, "GR1601401250554646315616156156156161695", false, "", iban_test_length[1] },     // ALPHA BANK
                {12, "GR16014055405660000000000000000000018995", false, "", iban_test_length[1] },    // ALPHA BANK
                {13, "GR16017088955000400000000000000005695", false, "", iban_test_length[1] },    // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {14, "GR1601708655000690000000000000000005", false, "", iban_test_length[1] }        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    bool validiban = false;
                    string ibanbank = "";

                    // Run tests
                    salaryLib.CheckIBAN((string)testcases[i, 1], ref validiban, ref ibanbank);

                    // Assert
                    Assert.AreEqual((bool)testcases[i, 2], validiban);
                    Assert.AreEqual((string)testcases[i, 3], ibanbank);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (IBAN LENGTH): IBANLEN{0}: {1} - {2} - {3}. \n \t Hint: {4}\n \t Reason: {5} ",
                        (int)testcases[i, 0], (string)testcases[i, 1], (bool)testcases[i, 2], (string)testcases[i, 3], (string)testcases[i, 4], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }

        [TestMethod()]
        public void CheckIBANTest_Bank()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Creating array with valid/invalid ibans 
            object[,] testcases =
            {
                // Test Cases for valid banks
                {1, "GR1001101230000012312424242", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[1]  },  // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {2, "GR1701102410000024141242141", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[1] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {3, "GR8301105160000051654648641", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[1] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {4, "GR3101109870000098700045623", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[1] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {5, "GR2101108650000086546894364", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[1] },     // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {6, "GR3801429737727391142718189", true, "ALPHA BANK", iban_valid_test_bank[0] + " " + iban_valid_test_bank[2] },     // ALPHA BANK
                {7, "GR8201484838463855261928469", true, "ALPHA BANK", iban_valid_test_bank[0] + " " + iban_valid_test_bank[2] },     // ALPHA BANK
                {8, "GR9001413986511399855493691", true, "ALPHA BANK", iban_valid_test_bank[0] + " " + iban_valid_test_bank[2] },     // ALPHA BANK
                {9, "GR7501472634743366951287392", true, "ALPHA BANK", iban_valid_test_bank[0] + " " + iban_valid_test_bank[2] },    // ALPHA BANK
                {10, "GR4301434216911913519328388", true, "ALPHA BANK", iban_valid_test_bank[0] + " " + iban_valid_test_bank[2] },    // ALPHA BANK
                {11, "GR9701774276922566488866522", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[3] },    // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {12, "GR4901721824992442414157674", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[3] },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {13, "GR7101751774233265368878352", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[3] },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {14, "GR5201721451242695239225197", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[3] },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.
                {15, "GR1301726554183122419497187", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_valid_test_bank[0] + " " + iban_valid_test_bank[3] },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.

                // Test Cases for invalid bank codes
                {16, "GR1601301250000000012300695", false, "", iban_test_bank }, 
                {17, "GR1602301250000623621651616", false, "", iban_test_bank },    
                {18, "GR1601001250554646315616156", false, "", iban_test_bank },   
                {19, "GR1600401251450554615845695", false, "", iban_test_bank },  
                {20, "GR1608401251450554615845695", false, "", iban_test_bank },    
                {21, "GR1601301251450554615845695", false, "", iban_test_bank }     
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    bool validiban = false;
                    string ibanbank = "";

                    // Run tests
                    salaryLib.CheckIBAN((string)testcases[i, 1], ref validiban, ref ibanbank);

                    // Assert
                    Assert.AreEqual((bool)testcases[i, 2], validiban);
                    Assert.AreEqual((string)testcases[i, 3], ibanbank);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Bank Code): IBANBANK{0}: {1} - {2} - {3}. \n \t Hint: {4}\n \t Reason: {5} ",
                        (int)testcases[i, 0], (string)testcases[i, 1], (bool)testcases[i, 2], (string)testcases[i, 3], (string)testcases[i, 4], e.Message);
                };
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }

        [TestMethod()]
        public void CheckIBANTest_CountryCode()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Creating array with valid/invalid ibans 
            object[,] testcases =
            {
                // Test Cases for country banks
                {1, "GR1001101230000012312424242", true, "Εθνική Τράπεζα της Ελλάδος Α.Ε.", iban_test_country },  // Εθνική Τράπεζα της Ελλάδος Α.Ε.
                {2, "GR1101401240124000000001230", true, "ALPHA BANK", iban_test_country },     // ALPHA BANK
                {3, "GR9701774276922566488866522", true, "ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.", iban_test_country },        // ΤΡΑΠΕΖΑ ΠΕΙΡΑΙΩΣ Α.Ε.

                // Test Cases for invalid country codes
                {4, "GB33BUKB20201555555555", false, "", iban_test_country },
                {5, "DE75512108001245126199", false, "", iban_test_country },
                {6, "FR7630006000011234567890189", false, "", iban_test_country },
                {7, "SE5995167383681295524945", false, "", iban_test_country },
                {8, "IR181668918351737159256364", false, "", iban_test_country },
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    bool validiban = false;
                    string ibanbank = "";

                    // Run tests
                    salaryLib.CheckIBAN((string)testcases[i, 1], ref validiban, ref ibanbank);

                    // Assert
                    Assert.AreEqual((bool)testcases[i, 2], validiban);
                    Assert.AreEqual((string)testcases[i, 3], ibanbank);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Country Code): IBANCOUNTRY{0}: {1} - {2} - {3}. \n \t Hint: {4}\n \t Reason: {5} ",
                        (int)testcases[i, 0], (string)testcases[i, 1], (bool)testcases[i, 2], (string)testcases[i, 3], (string)testcases[i, 4], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }
        [TestMethod()]
        public void CalculateSalaryTestCategory()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            Employee[] employees = new Employee[]
            {
                new Employee("TE", "NoMSc", 1, 0),
                new Employee("PE", "NoMSc", 3, 0),
                new Employee("TE", "NoMSc", 5, 0),
                new Employee("PE", "NoMSc", 7, 0),
                new Employee("TE", "NoMSc", 9, 0),
                new Employee("PE", "NoMSc", 11, 0),
                new Employee("TE", "NoMSc", 13, 0),
                new Employee("PE", "NoMSc", 15, 0),
                new Employee("TE", "NoMSc", 17, 0),
                new Employee("PE", "NoMSc", 19, 0),
                new Employee("TE", "NoMSc", 21, 0),
                new Employee("PE", "NoMSc", 23, 0),
                new Employee("TE", "NoMSc", 25, 0),
                new Employee("PE", "NoMSc", 27, 0),
                new Employee("PE", "NoMSc", 29, 0),
                new Employee("TE", "NoMSc", 31, 0),
                new Employee("PE", "NoMSc", 33, 0),
                new Employee("TE", "NoMSc", 35, 0),
                new Employee("PE", "NoMSc", 37, 0)
            };

            // Creating array of testcases
            object[,] testcases =
            {
                // Test Cases for Eployees with TE/PE Category
                {1, employees[0], (double)1037, (double)736.1038, salary_test_valid },
                {2, employees[1], (double)1151, (double)809.9074, salary_test_valid },
                {3, employees[2], (double)1147, (double)807.3178, salary_test_valid },
                {4, employees[3], (double)1269, (double)886.3006, salary_test_valid },
                {5, employees[4], (double)1257, (double)878.5318, salary_test_valid },
                {6, employees[5], (double)1387, (double)962.6938, salary_test_valid },
                {7, employees[6], (double)1367, (double)949.7458, salary_test_valid },
                {8, employees[7], (double)1505, (double)1039.087, salary_test_valid },
                {9, employees[8], (double)1477, (double)1020.9598, salary_test_valid },
                {10, employees[9], (double)1623, (double)1115.4802, salary_test_valid },
                {11, employees[10], (double)1587, (double)1092.1738, salary_test_valid },
                {12, employees[11], (double)1741, (double)1191.8734, salary_test_valid },
                {13, employees[12], (double)1697, (double)1163.3878, salary_test_valid },
                {14, employees[13], (double)1859, (double)1268.2666, salary_test_valid },
                {15, employees[14], (double)1918, (double)1306.4632, salary_test_valid },
                {16, employees[15], (double)1862, (double)1270.2088, salary_test_valid },
                {17, employees[16], (double)2036, (double)1281.4636, salary_test_valid },
                {18, employees[17], (double)1972, (double)1341.4228, salary_test_valid },
                {19, employees[18], (double)2154, (double)1351.9804, salary_test_valid }
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    double grossSalary = 0.0;
                    double netIncome = 0.0;

                    // Run tests
                    salaryLib.CalculateSalary((Employee)testcases[i, 1], ref grossSalary, ref netIncome);

                    // Assert
                    Assert.AreEqual((double)testcases[i, 2], (double)grossSalary, 0.0001, "Wrong Gross Earnings calculation.");
                    Assert.AreEqual((double)testcases[i, 3], (double)netIncome, 0.0001, "Wrong Net Income calculation.");
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Salary Calculation): SALCATEG{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }

        // Επίπεδο μεταπτυχιακών σπουδών
        [TestMethod()]
        public void CalculateSalaryTestStudies()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Array of employees 
            Employee[] employees = new Employee[]
            {
                new Employee("TE", "NoMSc", 0, 0),
                new Employee("PE", "NoMSc", 0, 0),
                new Employee("TE", "MSc", 0, 0),
                new Employee("PE", "MSc", 0, 0),
                new Employee("TE", "PhD", 0, 0),
                new Employee("PE", "PhD", 0, 0),
                new Employee("TE", "NoMSc", 5, 1),
                new Employee("PE", "NoMSc", 10, 2),
                new Employee("TE", "MSc", 5, 1),
                new Employee("PE", "MSc", 10, 2),
                new Employee("TE", "PhD", 5, 1),
                new Employee("PE", "PhD", 10, 2), 
                new Employee("TE", "NoMSc", 5, 1),
                new Employee("PE", "NoMSc", 10, 2),
            };

            // Creating array of testcases
            object[,] testcases =
            {
                // Test Cases for Eployees with Studies
                {1, employees[0], (double)1037, (double)736.1038, salary_test_msg[0] },
                {2, employees[1], (double)1092, (double)771.7108, salary_test_msg[0] },
                {3, employees[2], (double)1147, (double)807.31779, salary_test_msg[0] },
                {4, employees[3], (double)1210, (double)848.1039, salary_test_msg[0] },
                {5, employees[4], (double)1367, (double)949.7458, salary_test_msg[0] },
                {6, employees[5], (double)1446, (double)1000.8904, salary_test_msg[0] },
                {7, employees[6], (double)1197, (double)842.4378, salary_test_msg[0] },
                {8, employees[7], (double)1457, (double)1018.2618, salary_test_msg[0] },
                {9, employees[8], (double)1307, (double)913.6518, salary_test_msg[0] },
                {10, employees[9], (double)1575, (double)1094.655, salary_test_msg[0] },
                {11, employees[10], (double)1527, (double)1056.0798, salary_test_msg[0] },
                {12, employees[11], (double)1811, (double)1247.4414, salary_test_msg[0] },
                {13, employees[12], (double)1197, (double)842.4378, salary_test_msg[0] },
                {14, employees[13], (double)1457, (double)1018.2618, salary_test_msg[0] },
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    double grossSalary = 0.0;
                    double netIncome = 0.0;

                    // Run tests
                    salaryLib.CalculateSalary((Employee)testcases[i, 1], ref grossSalary, ref netIncome);

                    // Assert
                    Assert.AreEqual((double)testcases[i, 2], grossSalary, 0.0001, "Wrong Gross Earnings calculation.");
                    Assert.AreEqual((double)testcases[i, 3], netIncome, 0.0001, "Wrong Net Income calculation.");
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Salary Calculation): SALSTUD{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }
        [TestMethod()]
        public void CalculateSalaryTestChildren()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            Employee[] employees = new Employee[]
            {
                new Employee("TE", "NoMSc", 0, 0),
                new Employee("PE", "NoMSc", 0, 0),
                new Employee("TE", "MSc", 0, 0),
                new Employee("PE", "MSc", 0, 0),
                new Employee("TE", "PhD", 0, 0),
                new Employee("PE", "PhD", 0, 0),
                new Employee("TE", "NoMSc", 0, 1),
                new Employee("PE", "NoMSc", 0, 2),
                new Employee("TE", "MSc", 0, 3),
                new Employee("PE", "MSc", 0, 4),
                new Employee("TE", "PhD", 0, 5),
                new Employee("PE", "PhD", 0, 6),
                new Employee("TE", "NoMSc", 0, 1),
                new Employee("PE", "NoMSc", 0, 2),
            };

            // Creating array of testcases
            object[,] testcases =
            {
                // Test Cases for Employees with Children
                {1, employees[0], (double)1037, (double)736.1038, salary_test_msg[0] },
                {2, employees[1], (double)1092, (double)771.7108, salary_test_msg[0] },
                {3, employees[2], (double)1147, (double)807.3178, salary_test_msg[0] },
                {4, employees[3], (double)1210, (double)848.104, salary_test_msg[0] },
                {5, employees[4], (double)1367, (double)949.7458, salary_test_msg[0] },
                {6, employees[5], (double)1446, (double)1000.8904, salary_test_msg[0] },
                {7, employees[6], (double)1087, (double)771.2238, salary_test_msg[0] },
                {8, employees[7], (double)1162, (double)827.2788, salary_test_msg[0] },
                {9, employees[8], (double)1267, (double)913.5891, salary_test_msg[0] },
                {10, employees[9], (double)1380, (double)1005.0786, salary_test_msg[0] },
                {11, employees[10], (double)1607, (double)1058.7051, salary_test_msg[0] },
                {12, employees[11], (double)1756, (double)1173.501, salary_test_msg[0] },
                {13, employees[12], (double)1087, (double)771.2238, salary_test_msg[0] },
                {14, employees[13], (double)1162, (double)827.2788, salary_test_msg[0] },
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    double grossSalary = 0.0;
                    double netIncome = 0.0;

                    // Run tests
                    salaryLib.CalculateSalary((Employee)testcases[i, 1], ref grossSalary, ref netIncome);

                    // Assert
                    Assert.AreEqual((double)testcases[i, 2], grossSalary, 0.0001, "Wrong Gross Earnings calculation.");
                    Assert.AreEqual((double)testcases[i, 3], netIncome, 0.0001, "Wrong Net Income calculation.");
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Salary Calculation): SALCHILD{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], e.Message);
                };
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }

        [TestMethod()]
        public void CalculateSalaryInvalidTests()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            Employee[] employees = new Employee[]
            {
                // Testing with empty values of category and studies
                new Employee(string.Empty, string.Empty, 0, 0),
                // Testing with negative values
                new Employee("TE", "NoMSc", -1, 0),
                new Employee("PE", "NoMSc", 0, -1),
                new Employee("PE", "NoMSc", -1, -1),
                // Testing values extend the range (WorkExperience 0 - 38, Children 0-6)
                new Employee("TE", "NoMSc", 10, 10),
                new Employee("PE", "NoMSc", 40, 40),
                new Employee("PE", "NoMSc", 50, 20),
                // Testing with invalid values 
                new Employee("Invalid", "MSc", 0, 0),
                new Employee("PE", "Invalid", 5, 2),
                new Employee("Invalid", "Invalid", 0, 0),
                new Employee("Invalid", "Invalid", 40, 5),
                new Employee("Invalid", "Invalid", 12, 8),
                new Employee("PE", "PhD", 0, 0),
                new Employee("PE", "PhD", 0, 0)
            };

            // Creating array of testcases
            object[,] testcases =
            {
                // Test Cases for Employees with invalid values
                {1, employees[0], (double)-1.0, (double)-1.0, salary_test_msg[0] },
                {2, employees[1], (double)-1.0, (double)-1.0, salary_test_msg[1] },
                {3, employees[2], (double)-1.0, (double)-1.0, salary_test_msg[1] },
                {4, employees[3], (double)-1.0, (double)-1.0, salary_test_msg[1] },
                {5, employees[4], (double)-1.0, (double)-1.0, salary_test_msg[2] },
                {6, employees[5], (double)-1.0, (double)-1.0, salary_test_msg[2] },
                {7, employees[6], (double)-1.0, (double)-1.0, salary_test_msg[2] },
                {8, employees[7], (double)-1.0, (double)-1.0, salary_test_msg[3] },
                {9, employees[8], (double)-1.0, (double)-1.0, salary_test_msg[3] },
                {10, employees[9], (double)-1.0, (double)-1.0, salary_test_msg[3] },
                {11, employees[10], (double)-1.0, (double)-1.0, salary_test_msg[3] },
                {12, employees[11], (double)-1.0, (double)-1.0, salary_test_msg[3] },
                {13, employees[12], (double)-1.0, (double)1000.8903, salary_test_msg[4] },
                {14, employees[13], (double)1446, (double)-1.0, salary_test_msg[5] }
            };

            // Init control cases index
            int i = 0;
            bool failed = false;

            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    double grossSalary = 0.0;
                    double netIncome = 0.0;

                    // Run tests
                    salaryLib.CalculateSalary((Employee)testcases[i, 1], ref grossSalary, ref netIncome);

                    // Assert
                    Assert.AreEqual((double)testcases[i, 2], grossSalary);
                    Assert.AreEqual((double)testcases[i, 3], netIncome);
                    // if an exception isn't thrown then there is an error 
                    Console.WriteLine("Failed Test Case Passed(Salary Calculation): SALINVALID{0}\n \t Hint: {1}",
                                       (int)testcases[i, 0], (string)testcases[i, 4]);
                    Assert.Fail("Invalid Test passed.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    //Do nothing, test passes if Assert.Fail() was not called
                    Console.WriteLine("Invalid Test Case (Salary Calculation Out Of Range): SALINVALID{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], ex.Message);
                }
                catch (ArgumentException ex)
                {
                    //Do nothing, test passes if Assert.Fail() was not called
                    Console.WriteLine("Invalid Test Case (Salary Calculation): SALINVALID{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], ex.Message);
                }
                
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Salary Calculation): SALINVALID{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 4], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();
        }

        [TestMethod()]
        public void CalculateMKTestValid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Creating array of testcases
            object[,] testcases =
            {
                {1, "19/10/2008", "NoMSc", 7, 1, 4, 8, calc_mk_tes_valid},    // 13 years, 4 months, 7 days - MK = 7 | 1 year, 4 months, 7 days.
                {2, "30/09/2017", "MSc", 5, 0, 4, 28, calc_mk_tes_valid },    // 2 years,  4 months, 7 days - MK = 3 | 4 months, 27 days.
                {3, "16/08/2010", "PhD", 12, 1, 6, 11, calc_mk_tes_valid },   // 11 years, 6 months, 7 days - MK = 7 | 1 year, 6 months, 10 days.
                {4, "1/1/2015", "NoMSc", 4, 1, 1, 26, calc_mk_tes_valid },    // 7 years, 1 months, 25 days - MK = 4 | 1 year, 1 months, 25 days.
                {5, "23/02/2019", "MSc", 4, 1, 0, 4, calc_mk_tes_valid }      // 3 years, 0 months, 3 days  - MK = 4 | 1 year, 0 months, 3 days.
            };

            // Init control cases index
            bool failed = false;

            // Access and execute control cases
            for (int i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    int mk = 0;
                    int excessYears = 0;
                    int excessMonths = 0;
                    int excessDay = 0;

                    // Run tests
                    salaryLib.CalculateMK((string)testcases[i, 1], (string)testcases[i, 2], ref mk, ref excessYears, ref excessMonths, ref excessDay);

                    // Assert
                    Assert.AreEqual((int)testcases[i, 3], mk, "Wrong MK calculation.");
                    Assert.AreEqual((int)testcases[i, 4], excessYears, "Wrong excess years calculation.");
                    Assert.AreEqual((int)testcases[i, 5], excessMonths, "Wrong excess months calculation.");
                    Assert.AreEqual((int)testcases[i, 6], excessDay, "Wrong excess days calculation.");
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Salary Calculation): MKVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 7], e.Message);
                }
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();

        }

        [TestMethod()]
        public void CalculateMKTestInvalid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // Creating array of testcases
            object[,] testcases =
            {
                {1, "10/04/1978", "NoMSc", 1, 1, 1, 1, calc_mk_test_msg[0] },   // work years > 38
                {2, "28/02/2022", "MSc", 2, 2, 2, 2, calc_mk_test_msg[1] },    // work years < 0
                {3, "16/08/2035", "PhD", 3, 3, 3, 3, calc_mk_test_msg[1] },    // work years < 0
                {4, "33/1/2015", "NoMSc", 4, 4, 4, 4, calc_mk_test_msg[3] },   // day > 31
                {5, "30/15/2015", "NoMSc", 5, 5, 5, 5, calc_mk_test_msg[4] },   // month > 12
                {6, "23/02/-2000", "NoMSc", 6, 6, 6, 6, calc_mk_test_msg[5] },     // year negative num
                // Invalid values in parameters
                {7, "23/02/2019", "Invalid", 7, 7, 7, 7, calc_mk_test_msg[6] },     // invalid studies
                {8, "Invalid", "PhD", 8, 8, 8, 8, calc_mk_test_msg[2] },     // invalid studies
                {9, "23/02/2021", string.Empty, 9, 9, 9, 9, calc_mk_test_msg[6]  },     // empty studies
                {10, string.Empty, "MSc", 10, 10, 10, 10,  calc_mk_test_msg[7] }     // // empty hiring date

            };

            // Init control cases index
            bool failed = false;

            // Access and execute control cases
            for (int i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // ref variable declaration
                    int mk = 0;
                    int excessYears = 0;
                    int excessMonths = 0;
                    int excessDay = 0;

                    // Run tests
                    salaryLib.CalculateMK((string)testcases[i, 1], (string)testcases[i, 2], ref mk, ref excessYears, ref excessMonths, ref excessDay);

                    // Assert
                    Assert.AreEqual((int)testcases[i, 3], mk, "Wrong MK calculation.");
                    Assert.AreEqual((int)testcases[i, 4], excessYears, "Wrong excess years calculation.");
                    Assert.AreEqual((int)testcases[i, 5], excessMonths, "Wrong excess months calculation.");
                    Assert.AreEqual((int)testcases[i, 6], excessDay, "Wrong excess days calculation.");

                    // if an exception isn't thrown then there is an error 
                    Console.WriteLine("Failed Test Case Passed(MK Calculation): {0}\n \t Hint: {1}",
                                       (int)testcases[i, 0], (string)testcases[i, 7]);
                    Assert.Fail("Invalid Test passed.");
                }
                catch (ArgumentException ex)
                {
                    //Do nothing, test passes if Assert.Fail() was not called
                    Console.WriteLine("Invalid Test Case (MK Calculation): MKINVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 7], ex.Message);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (MK Calculation): MKINVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 7], e.Message);
                };
            }
            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail();

        }

            [TestMethod()]
        public void NonAdultChildrenTestValid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // All adult dates (valid)
            string[] BirthDatesAdults = new string[]
            {
                "01/01/2000",
                "7/2/1998", 
                "15/3/1989", 
                "1/2/2004"
            };  // 0 adults

            string[] BirthDatesNonAdults = new string[]
            {
                "01/05/2013",
                "25/4/2008",
                "4/5/2011",
                "30/5/2010"
            };  // 4 children

            string[] BirthDatesHalfAdults = new string[]
            {
                "01/05/2013",
                "25/09/2000",
                "4/5/2011",
                "1/4/1998"
            };  // 2 children


            // Creating array of testcases
            object[,] testcases =
            {
                {1, BirthDatesAdults, 0, birthdate_child_valid },
                {2, BirthDatesNonAdults, 4, birthdate_child_valid },
                {3, BirthDatesHalfAdults, 2, birthdate_child_valid }
            };

            // Init control cases index
            int i = 0;
            bool failed = false;


            // Access and execute control cases
            for (i = 0; i < testcases.GetLength(0); i++)
            {
                try
                {
                    // Run tests and Assert
                    Assert.AreEqual((int)testcases[i, 2], salaryLib.NonAdultChildren((string[])testcases[i, 1]), "No adults in dates");  
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (BirthDate Test): BIRTHVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 3], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail("Children BirthDate Valid Test Failed");
        }

        [TestMethod()]
        public void NonAdultChildrenTestInvalid()
        {
            // Create SalaryLib (dll) Object for testing (or static in dll)
            SalaryLib salaryLib = new SalaryLib();

            // All adult dates (invalid)
            // Birthdates > Current day
            string[] BirthDatesAdultsExceedCurrent = new string[]
            {
                "01/01/2345",
                "7/2/2023",
                "15/3/3000",
                "10/2/4000"
            };  // 0 adults

            // Birthdates with negative numbers
            string[] BirthDatesNegativeNums= new string[]
            {
                "01/05/-2013",
                "1/-9/2008",
                "-7/5/2011",
                "-7/0/2011",
                "0/5/2011",
                "-7/5/-8"
            };

            // Invalid Birthdates by day
            string[] BirthDatesDaysInvalid = new string[]
            {
                "35/05/2013",
                "32/5/2008",
                "0/5/2011",
                "invalid/5/2011"
            };
            // Invalid Birthdates by Month
            string[] BirthDatesMonthsInvalid = new string[]
                        {
                "01/20/2012",
                "1/13/2008",
                "5/0/2011",
                "5/invalid/2011"
            };

            // Invalid Birthdates by year
            string[] BirthDatesYearInvalid = new string[]
                        {
                "01/20/year",
                "1/13/0",
                "5/0/2011"
            };
            // Creating array of testcases
            object[,] testcases =
            {
                {1, BirthDatesNegativeNums, 0, birthdate_child_test_msg[0] },
                {2, BirthDatesAdultsExceedCurrent, 0, birthdate_child_test_msg[1] },
                {3, BirthDatesDaysInvalid, 0, birthdate_child_test_msg[2] },
                {4, BirthDatesMonthsInvalid, 0, birthdate_child_test_msg[3] },
                {5, BirthDatesYearInvalid, 0, birthdate_child_test_msg[4] }
            };

            // Init control cases index
            bool failed = false;

            // Access and execute control cases
            for (int i = 0; i < testcases.GetLength(0); i++)
            {
                try 
                { 
                    // Run tests and Assert
                    Assert.AreEqual((int)testcases[i, 2], salaryLib.NonAdultChildren((string[])testcases[i, 1]));
                    // exception was not thrown for invalid arguments
                    Console.WriteLine("Invalid Test Case Passed(Non Adult Children Birthdates): {0}\n \t Hint: {1}",
                                       (int)testcases[i, 0], (string)testcases[i, 4]);
                    Assert.Fail("Invalid Birthdates passed.");
                }
                catch (ArgumentException e) 
                {
                        Console.WriteLine("Failed Test Case (BirthDate Invalid Test): BIRTHINVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                         (int)testcases[i, 0], (string)testcases[i, 3], e.Message);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (BirthDate Invalid Test): {0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)testcases[i, 0], (string)testcases[i, 3], e.Message);
                }
            }

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail("Children BirthDate Invalid Test Failed");
        }


        [TestMethod()]
        public void MaxNetIncomeTestValid()
        {
            SalaryLib salaryLib = new SalaryLib();

            Employee[] employees = new Employee[]
            {
                new Employee("PE", "NoMSc", 0, 0),
                new Employee("PE", "NoMSc", 2, 1),
                new Employee("PE", "NoMSc", 4, 2),
                new Employee("PE", "NoMSc", 6, 3),
                new Employee("PE", "NoMSc", 10, 4),
                new Employee("PE", "NoMSc", 14, 5),
                new Employee("PE", "NoMSc", 20, 6)
            };

            Employee[] employees1 = new Employee[]
            {
                new Employee("PE", "MSc", 4, 2),
                new Employee("PE", "NoMSc", 12, 4)
            };
            
            Employee[] employees2 = new Employee[]
            {
                new Employee("TE", "MSc", 6, 3),
                new Employee("PE", "NoMSc", 18, 5)
            };

            object[,] test_cases =
            {
                {1, employees, (double)1326.287, max_netincome_valid },
                {2, employees1, (double)1157.865, max_netincome_valid },
                {3, employees2, (double)1224.439, max_netincome_valid }
            };

            // Init control cases index
            bool failed = false;

            for (int i = 0; i < test_cases.GetLength(0); i++)
            {
                try
                {
                    // Using Assert.AreEqual(double,double,double), where the last value is the precision
                    Assert.AreEqual((double)test_cases[i, 2], (double)salaryLib.MaxNetIncome((Employee[])test_cases[i, 1]), 0.001);
                }
                catch (Exception e)
                {
                    failed = true;
                    Console.WriteLine("Failed Test Case: MAXNETVAL{0} \n \t Hint: {1} \n \t Reason: {2} ",
                        (int)test_cases[i, 0], (string)test_cases[i, 3], e.Message);
                };
            };

            // In the event that a control case fails, throw exception
            if (failed) Assert.Fail("Max Net Income Test Failed");
        }

        [TestMethod()]
        public void MaxNetIncomeTestInvalid()
        {

            SalaryLib salaryLib = new SalaryLib();

            // invalid Employee values
            Employee[] employees1 = new Employee[]
            {
                new Employee("invalid", "NoMSc", 0, 0),
                new Employee("PE", "invalid", 2, 1),
                new Employee("", "MSc", 4, 2),
                new Employee("PE", "", 15, 3)
            };

            // values out of range 
            Employee[] employees2 = new Employee[]
            {
                new Employee("TE", "MSc", 0, 8),
                new Employee("PE", "NoMSc", 45, 3)
            };

            Employee[] employees3 = new Employee[]
            {
                new Employee("PE", "NoMSc", 45, 3),
                new Employee("TE", "MSc", 5, 5)
            };

            // negative values
            Employee[] employees4 = new Employee[]
            {
                new Employee("PE", "PhD", 10, -1),
                new Employee("TE", "NoMSc", 1, 5)
            };

            Employee[] employees5 = new Employee[]
            {
                new Employee("PE", "PhD", 10, 1),
                new Employee("TE", "NoMSc", -1, 5)
            };

            // valid data wrong calculation result
            Employee[] employees0 = new Employee[]
            {
                new Employee("PE", "PhD", 0, 0),
                new Employee("TE", "NoMSc", 30, 6)
            };

            object[,] test_cases =
            {
                {1, employees0, (double)-123, max_netincome_test_msg[0] },
                {2, employees1, (double)-1, max_netincome_test_msg[1] },
                {3, employees2, (double)-1, max_netincome_test_msg[1] },
                {4, employees3, (double)-1, max_netincome_test_msg[1] },
                {5, employees4, (double)-1, max_netincome_test_msg[1] },
                {6, employees5, (double)-1, max_netincome_test_msg[1] }
            };

            // Init control cases index
            bool failed = false;

            // Access and execute control cases
            for (int i = 0; i < test_cases.GetLength(0); i++)
            {
                try
                {
                    // Run tests and Assert
                    Assert.AreEqual((double)test_cases[i, 2], (double)salaryLib.MaxNetIncome((Employee[])test_cases[i, 1]), 0.001);
                    // exception was not thrown for invalid arguments
                    Console.WriteLine("Invalid Test Case Passed(Max Income Calculation): MAXNETINVAL{0}\n \t Hint: {1}",
                                       (int)test_cases[i, 0], (string)test_cases[i, 3]);
                    Assert.Fail("Invalid test_cases passed.");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Failed Test Case (Max Income Calculation Test): MAXNETINVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                     (int)test_cases[i, 0], (string)test_cases[i, 3], e.Message);
                }
                catch (Exception e)
                {
                    // control case failed
                    failed = true;
                    // printing control case that failed
                    Console.WriteLine("Failed Test Case (Max Income Calculation Test): MAXNETINVAL{0}\n \t Hint: {1}\n \t Reason: {2} ",
                                       (int)test_cases[i, 0], (string)test_cases[i, 3], e.Message);
                }

            }

            if (failed) Assert.Fail("Invalid Max Net Income Test");
        }
    }
 }