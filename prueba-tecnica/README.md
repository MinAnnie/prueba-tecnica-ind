# Proyecto API de Productos

Este es un proyecto para gestionar productos y sus movimientos de stock, desarrollado con ASP.NET Core y PostgreSQL,
ejecutado dentro de contenedores Docker.

## Descripción

El proyecto incluye los siguientes servicios:

- **API de Productos**: Permite gestionar productos, crear entradas y salidas de stock, y realizar operaciones CRUD.
- **Base de datos PostgreSQL**: Almacena la información de los productos y los movimientos de stock.

## Estructura del Proyecto

El proyecto está dividido en las siguientes secciones:

* **Controllers**: Los controladores de la API que gestionan las solicitudes para los productos y los movimientos de
  stock.
* **Models**: Los modelos que representan la estructura de datos de los productos y los movimientos de stock.
    * **Product**: Modelo que representa la entidad de producto.
    * **StockMovement**: Modelo que representa los movimientos de stock (entradas y salidas).
* **Services**: La capa de servicios que maneja la lógica de negocio, interactúa con los repositorios y realiza
  operaciones CRUD para los productos y movimientos de stock.
    * **ProductService**: Servicio que gestiona la creación, actualización y obtención de productos.
    * **StockMovementService**: Servicio que gestiona los movimientos de stock asociados a los productos.
* **Repositories**: La capa de repositorios que se encarga de interactuar con la base de datos para acceder y modificar
  los datos de los productos y movimientos de stock.
    * **ProductRepository**: Repositorio que maneja las operaciones de base de datos para la entidad Product.
    * **StockMovementRepository**: Repositorio que maneja las operaciones de base de datos para la entidad
      StockMovement.
* **db**: Carpeta que contiene el archivo db.sql, el cual contiene el código necesario para crear las tablas Products y
  StockMovements en la base de datos PostgreSQL.

## Modelos

### Product

Representa un producto con las siguientes propiedades:

- `Id`: Identificador único del producto (autoincrementado).
- `Name`: Nombre del producto (requerido, máximo 255 caracteres).
- `Description`: Descripción del producto (opcional, máximo 255 caracteres).
- `Stock`: Cantidad de stock disponible para el producto (valor por defecto 0).

### StockMovement

Representa un movimiento de stock asociado a un producto con las siguientes propiedades:

- `Id`: Identificador único del movimiento (autoincrementado).
- `ProductId`: Identificador del producto relacionado (clave foránea).
- `Quantity`: Cantidad de stock movido (puede ser positivo para entradas y negativo para salidas).
- `Type`: Tipo de movimiento ("Entrada" o "Salida").
- `Date`: Fecha y hora del movimiento (valor por defecto es la fecha actual).

## Requisitos

- Docker y Docker Compose instalados.

## Configuración

Este proyecto usa Docker Compose para levantar los servicios de la API y la base de datos. La conexión de la API a
PostgreSQL está configurada automáticamente a través de Docker Compose.

## Instrucciones

1. **Clona el repositorio**:
    ```bash
    git clone https://github.com/MinAnnie/prueba-tecnica-ind.git
    cd prueba-tecnica-ind
    ```

2. **Construye y ejecuta los contenedores**:
    ```bash
    docker-compose up --build
    ```

3. **Accede a la API**:
    - La API estará disponible en `http://localhost:8080/api/products`.

4. **Accede a PostgreSQL**:
    - El contenedor de PostgreSQL estará corriendo en `localhost:5432` con las credenciales:
        - Usuario: `postgres`
        - Contraseña: `postgres`
        - Base de datos: `prueba_tecnica`

5. **Estructura de la base de datos**:
    - **Products**: Tabla de productos con las columnas `Id`, `Name`, `Description`, y `Stock`.
    - **StockMovements**: Tabla de movimientos de stock con las columnas `Id`, `ProductId`, `Quantity`, `Type`, y
      `Date`.

6. **Eliminar los contenedores**:
    ```bash
    docker-compose down
    ```

## Archivo `db.sql`

En la carpeta `/db` encontrarás el archivo `db.sql` que contiene el código SQL necesario para crear las tablas:

```sql
CREATE TABLE "Products"
(
    "Id"          SERIAL PRIMARY KEY,
    "Name"        VARCHAR(255) NOT NULL,
    "Description" VARCHAR(255) NOT NULL,
    "Stock"       INT          NOT NULL DEFAULT 0
);

CREATE TABLE "StockMovements"
(
    "Id"        SERIAL PRIMARY KEY,
    "ProductId" INT         NOT NULL,
    "Quantity"  INT         NOT NULL,
    "Type"      VARCHAR(50) NOT NULL,
    "Date"      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);
```

Este archivo se ejecutará automáticamente al iniciar el contenedor de PostgreSQL.

## Docker Compose

El archivo `docker-compose.yml` está configurado para levantar los siguientes servicios:

- **app**: Servicio que corre la API ASP.NET Core.
- **db**: Contenedor de PostgreSQL con las tablas necesarias.

### Ejemplo de archivo `docker-compose.yml`:

```yaml
services:
  app:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=db;Database=prueba_tecnica;Username=postgres;Password=postgres
    depends_on:
      - db
    networks:
      - app-network

  db:
    image: postgres:15
    container_name: postgres_db
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: prueba_tecnica
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./db/db.sql:/docker-entrypoint-initdb.d/db.sql
    networks:
      - app-network

volumes:
  postgres_data:
    driver: local

networks:
  app-network:
    driver: bridge
```
