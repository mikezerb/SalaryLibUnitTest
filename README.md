# C# Unit Teting on SalaryLib 

The aim of this work is to create a .dll library called SalaryLib and run the appropriate Unit Tests for each method.
The library applied in the context of the development of the Financial Information System
Part of a service and concerns a number of methods and functions for
payroll management of employees.

The library contains a struct called `Employee` that
will contain the details of an employee.

The properties of Employee are as follows:
* string Category 
* string Studies 
* int WorkExperience
* int Children

The SalaryLib contains the following functions/methods:
* bool ValidEMAIL(string Email)
* void CheckIBAN(string IBAN, ref bool ValidIBAN, ref string Bank)
* void CalculateSalary(Employee EmplX, ref double GrossSalary, ref double NetIncome)
* void CalculateMK(string HiringDate, string Studies, ref int MK, ref int ExcessYears, ref int ExcessMonths, ref int ExcessDays)
* int NonAdultChildren(string[] ChildrenBirthday)


