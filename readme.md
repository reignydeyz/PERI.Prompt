# PERI.Prompt

**A simple CMS built in ASP.Net Core**

PERI.Blogger is our entry to having a simple web application that can run on Windows, Linux and Mac with the power of .Net Core. The project also aims to provide simple solution/architecture to perform algorithms using the new framework.

## Getting Started

### Prerequisites

Here are the things you need

- [Visual Studio](https://www.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2016)
- [SQL Server Management Studio](https://msdn.microsoft.com/en-us/library/mt238290.aspx)

## People to blame

The following personnel is/are responsible for managing this project.

- [actchua@periapsys.com](mailto:actchua@periapsys.com)

## Developer's Guide

Because .Net Core is not as stabilized as .Net Framework, we try to keep everything simple and readable as much as possible for the benefit of **transparency** - because we too are still learning. We cannot claim the product to be better among others but we are considering the best standards and practices.

The project uses the ff. technology:

- [ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [.Net Core 2](https://www.microsoft.com/net/core)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)	
- [JQuery/BootStrap](http://getbootstrap.com/)
- [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/)

Solution structure:

- PERI.Prompt.BLL
	- The Business Layer of the system
- PERI.Prompt.Core
	- Common functions
- PERI.Prompt.EF
	- Contains the EntityFramework module
	- All of the data-manipulations were done here
- PERI.Prompt.Web
	- The main project

Database:

1. From SSMS, create Db and run ```Senz.sql```

Additional:

1.Create ```appsettings.json``` that contains your connection. Follow the format below and edit the parameter/s accordingly:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "[MS SQL DATASOURCE]"
  }
}
```

2.Place the setting inside ```PERI.Prompt.Web``` project.