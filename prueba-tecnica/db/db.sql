CREATE TABLE "Products"
(
    "Id"          SERIAL PRIMARY KEY,
    "Name"        VARCHAR(255) NOT NULL,
    "Description" VARCHAR(255) NOT NULL,
    "Stock"       INT          NOT NULL DEFAULT 0
);

CREATE TABLE "StockMovements"
(
    "Id"           SERIAL PRIMARY KEY,
    "ProductId"    INT         NOT NULL,
    "Quantity"     INT         NOT NULL,
    "Type" VARCHAR(50) NOT NULL,
    "Date"         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

