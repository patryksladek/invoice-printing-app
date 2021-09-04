# Program do drukowania faktur

Aplikacja umożliwia wykonanie wydruku faktury PDF przy użyciu systemu enova365. Program wymaga wskazania identyfikatora faktury, nazwy wzorca wydruku oraz ścieżki do folderu, w którym zostanie zapisany plik.


## Jak uruchomić program?

Do poprawnego uruchomienia programu konieczne jest wprowadzenie odpowiednich ustawień w pliku konfiguracyjnym (appsettings.json).

- ConnectionStrings
  - **DbConnectionString** - ciąg połączenia do bazy danych
- EnovaSettings
  - **Instance** - nazwa firmy
  - **User** - nazwa użytkownika
  - **Password** - hasło dostępu

Dodatkowo istnieje możliwość skonfigurowania loggera danych (Serilog).

```json
{
  "ConnectionStrings": {
    "DbConnectionString": "Server=localhost\\SQLEXPRESS;Database=Firma;Trusted_Connection=True"
  },
  "EnovaSettings": {
    "Instance": "Firma",
    "User": "Administrator",
    "Password": ""
  },
  "Serilog": {
    "MinimumLevel": "Debug",
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "C:\\Logs\\log.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}

```
Do poprawnego działania programu niezbędna jest instalacja systemu enova365 w wersji 2104.3.8.0.

### Argumenty wywołania programu

- **--id**      &emsp;&emsp;&emsp;&emsp;&nbsp; Identyfikator faktury.
- **--pattern** &emsp;&nbsp;&nbsp;&nbsp; Nazwa wzorca wydruku.
- **--path**    &emsp;&emsp;&emsp; Ścieżka do folderu, w którym zostanie zapisany plik PDF.


### Przykład uruchomienia programu

```csharp
InvoicePrintingApp.exe --id 1 --pattern "handel/dokumenty sprzedazy.aspx" --path "C:\Faktury"
```
