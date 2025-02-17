# Fisiolabs

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

### Para hacer un Scaffold

```
Scaffold-DbContext "Server=localhost;DataBase=fiolabs_bd;User=root;Password=admin;Port=3306;" MySql.EntityFrameworkCore
```

### Crear las migraciones

**.NetCore CLI**

Comando para installar .NetCore CLI

```bash
dotnet tool install --global dotnet-ef
```

Comando para crear una nueva migración

```bash
dotnet ef migrations add "MigracionInicial" --project Core --startup-project Presentation --output-dir Infraestructure\Persistance\Migrations
```

### Aplicar las migraciones a la base de datos

Desde NetCore CLI

**.NetCore CLI**
```bash
dotnet ef database update --project Core --startup-project Presentation
```
