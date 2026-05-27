# dotnetiis

ASP.NET demo application that fetches database credentials at runtime from the **CyberArk Application Access Manager (AAM) Credential Provider**, instead of storing them in `appsettings.json` or environment variables. The app then connects to a MariaDB/MySQL database and serves a small product catalog page.

This is the IIS / ASP.NET sibling of [mysqlclientdotnet](https://github.com/aslancarlos/mysqlclientdotnet), which shows the same Credential Provider pattern from a console application on Linux.

## What this demonstrates

| Capability | How |
|---|---|
| Zero stored DB credentials | Server, user, and password loaded at runtime from CCP |
| Strong host attestation | CCP enforces the AppId, Safe, and machine identity |
| Reflection-loaded CCP SDK | The CCP DLL is loaded dynamically so the project can target Linux and Windows |
| Standard ASP.NET MVC | No bespoke framework — the integration sits behind `IConfiguration` |

## Configuration

Edit `appsettings.json` with the values that match your CyberArk environment:

```json
{
  "Database": { "Server": "<db-host>", "Port": 3306, "Name": "<db>", "User": "<user>" },
  "CyberArk": { "AppId": "<your-app-id>", "Safe": "<safe>", "Folder": "Root", "Object": "<object>" }
}
```

The DB password is fetched by the app at startup via the CCP — it is never written to the config file.

## Prerequisites

- .NET 8 SDK
- CyberArk Credential Provider installed and configured on the host
- A Safe / Account / AppId configured in CyberArk PAM that maps to the DB credentials this app needs
- A MariaDB / MySQL instance reachable from the host

## Running

```bash
dotnet restore
dotnet run
```

## Related

- [mysqlclientdotnet](https://github.com/aslancarlos/mysqlclientdotnet) — Same CCP pattern from a console / Linux app
- [conjur-explainer](https://github.com/aslancarlos/conjur-explainer) — For workloads that need a centralized, audited secrets backend instead of CCP-on-host

## License

Apache License 2.0 — see [LICENSE](LICENSE).
