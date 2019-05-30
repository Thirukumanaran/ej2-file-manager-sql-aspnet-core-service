# ej2-file-manager-azure-aspnet-core-service

This example demonstrates how to utilize the file manager SQL server database service in ASP.NET Core platform.

## Prerequisites

Make the SQL server connection with SQL database file (FileManager.mdf) and specify the connection string in "Web.config" file as specified in below code example.

```

<add name="FileExplorerConnection" connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=<--Directory location point towards the SQL database fie (FileManager.mdf) -->;Integrated Security=True;Trusted_Connection=true" />

```

To configure the SQL server database connection use the `SetSQLConnection` method to set the connection name, table name and rootId of the table.

```
  
  SetSQLConnection(string name, string tableName, string tableID)

```

## How to run this application?

To run this application, clone the [`ej2-file-manager-sql-aspnet-core-service`](https://github.com/SyncfusionExamples/ej2-file-manager-sql-server-database-aspnet-core-service) repository and then navigate to its appropriate path where it has been located in your system.

To do so, open the command prompt and run the below commands one after the other.

```

git clone https://github.com/SyncfusionExamples/ej2-file-manager-sql-server-database-aspnet-core-service.git  FileManagerSQLService
cd FileManagerSQLService

```

## Running application

Once cloned, open solution file in visual studio.Then build the project after restoring the nuget packages and run it.
