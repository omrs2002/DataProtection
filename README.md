# DataProtection
Protecting Data with IDataProtector in ASP.NET Core
Encrypting and Decrypting Data with IDataProtector
IDataProtector is an interface that provides data protection services. To be able to use its features, we have to add those data protection services to the specified IServiceCollection and then inject it using dependency injection.

If we take a look at our starting project, we can see the Index action which lists all the employees from the database. There is also a Details link next to each employee which directs us to the Details page. Of course, if we take a look at the URI of a Details page, we can see the value of the employee’s Id property

https://code-maze.com/data-protection-aspnet-core/
