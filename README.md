# DataProtection
Protecting Data with IDataProtector in ASP.NET Core
Encrypting and Decrypting Data with IDataProtector
IDataProtector is an interface that provides data protection services. To be able to use its features, we have to add those data protection services to the specified IServiceCollection and then inject it using dependency injection.

If we take a look at our starting project, we can see the Index action which lists all the employees from the database. There is also a Details link next to each employee which directs us to the Details page. Of course, if we take a look at the URI of a Details page, we can see the value of the employee’s Id property

https://code-maze.com/data-protection-aspnet-core/


**in start up:**

services.AddDataProtection();


**In Controller:**


  public class EmployeesController : Controller
  {
        private readonly IEmployeeRepository _repo;
        private readonly IDataProtector _protector;

        public EmployeesController(IEmployeeRepository repo, IDataProtectionProvider provider)
        {
            _repo = repo;
            _protector = provider.CreateProtector("EmployeesApp.EmployeesController");
        }

        public IActionResult Index()
        {
            bool testProtcted = false;
            var employees = _repo.GetAll();
            foreach (var emp in employees)
            {
                var stringId = emp.Id.ToString();
                emp.EncryptedId = _protector.Protect(stringId);
            }
            //return View(employees);

            //Previous code removed for the example clarity
            if (testProtcted)
            {
                var timeLimitedProtector = _protector.ToTimeLimitedDataProtector();
                var timeLimitedData = timeLimitedProtector.Protect("Test timed protector", lifetime: TimeSpan.FromSeconds(2));
                //just to test that this action works as long as life-time hasn't expired
                var timedUnprotectedData = timeLimitedProtector.Unprotect(timeLimitedData);
                Thread.Sleep(3000);
                var anotherTimedUnprotectTry = timeLimitedProtector.Unprotect(timeLimitedData);
            }
            return View(employees);
        }

        public IActionResult Details(string id)
        {
            var guid_id = Guid.Parse(_protector.Unprotect(id));
            var employee = _repo.GetEmployee(guid_id);
            return View(employee);
        }
  }
  
  

