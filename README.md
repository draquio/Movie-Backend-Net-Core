# Películas Backend .NET Core
Backend de un sistema de películas realizado con C# y .NET Core.

## Base de datos
El motor de base de datos es SQLServer con multiples tablas.
- Movie
- Category
- Actor
- Review

## Desarrollo
El sistema fue realizado con la arquitectura de N Capas implementando los siguientes patrones:
- Patrón Repositorio
- Patrón de Servicio
- Patrón DTO
- Patrón de Inyección de Dependencias

## Relaciones en la base de atos
- Movie -> MovieCategory <- Category (Relación de muchos a muchos)
- Movie -> MovieActor <- Actor (Relación de muchos a muchos)
- Movie -> Review (Relación de 1 a muchos)

## Dependencias
- EntityFramework
- SQLServer
- AutoMapper

![](https://i.ibb.co/X7XHxzW/DBDiagram.png)
