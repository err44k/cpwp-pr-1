-- ============================================
-- Схема БД для складского учета маркетплейса
-- PostgreSQL
-- ============================================

DROP TABLE IF EXISTS operations;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS categories;

CREATE TABLE categories (
    id   SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE products (
    id            SERIAL PRIMARY KEY,
    name          TEXT NOT NULL,
    category_id   INT REFERENCES categories(id) ON DELETE SET NULL,
    quantity      INT NOT NULL DEFAULT 0 CHECK (quantity >= 0),
    price         NUMERIC(10,2) NOT NULL CHECK (price >= 0),
    min_quantity  INT NOT NULL DEFAULT 0 CHECK (min_quantity >= 0)
);


CREATE TABLE operations (
    id          SERIAL PRIMARY KEY,
    product_id  INT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    type        TEXT NOT NULL CHECK (type IN ('in','out')),
    quantity    INT NOT NULL CHECK (quantity > 0),
    date        TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_products_name       ON products (name);
CREATE INDEX idx_products_category   ON products (category_id);
CREATE INDEX idx_operations_date     ON operations (date);
CREATE INDEX idx_operations_product  ON operations (product_id);


INSERT INTO categories (name) VALUES
    ('Электроника'),
    ('Бытовая техника'),
    ('Канцелярия');

INSERT INTO products (name, category_id, quantity, price, min_quantity) VALUES
    ('Наушники Sony WH-1000XM5', 1,  50, 29990.00, 10),
    ('Пылесос Xiaomi Vacuum',    2,  15, 15990.00,  5),
    ('Ручка шариковая синяя',    3, 200,    25.50, 50),
    ('Монитор Dell 27"',         1,   8, 24990.00,  3);