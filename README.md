Comando para crear una nueva migración

```bash
dotnet ef migrations add "MigracionInicial" --project Core --startup-project Presentation --output-dir Infraestructure\Persistance\Migrations
```

**.NetCore CLI**
```bash
dotnet ef database update --project Core --startup-project Presentation
```
