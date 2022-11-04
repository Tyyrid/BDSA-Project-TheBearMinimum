FROM mcr.microsoft.com/azure-sql-edge
ENV ACCEPT_EULA=1
ENV MSSQL_SA_PASSWORD=Dev@password
EXPOSE 1433
