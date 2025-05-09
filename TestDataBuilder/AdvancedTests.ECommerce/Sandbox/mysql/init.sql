CREATE DATABASE IF NOT EXISTS orders;
use orders;
create table if not exists customers (
  id int not null primary key auto_increment,
  name varchar(255) not null,
  is_premium boolean not null
);

create table if not exists orders (
  id int not null primary key auto_increment,
  customer_id int not null,
  status int not null,
  amount decimal(10, 2) not null,
  street varchar(255) not null,
  city varchar(255) not null,
  state varchar(2) not null,
  zip_code varchar(9) not null,
  number varchar(6) not null,
  foreign key (customer_id) references customers(id)
);

DELIMITER //
CREATE PROCEDURE seed_clients(IN total INT)
BEGIN
    DECLARE i INT DEFAULT 1;
    WHILE i <= total DO
        INSERT INTO customers (name, is_premium)
        VALUES (
            CONCAT('Client ', i),
            RAND() < 0.2
        );
        SET i = i + 1;
    END WHILE;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE seed_orders(IN total INT)
BEGIN
  DECLARE i INT DEFAULT 0;
  DECLARE random_customer_id INT;
  DECLARE random_status INT;
  DECLARE random_amount DECIMAL(10,2);
  DECLARE random_state VARCHAR(2);
  DECLARE random_zip_code VARCHAR(9);
  DECLARE random_number VARCHAR(6);
  
  WHILE i < total DO
    SET random_customer_id = FLOOR(1 + RAND() * 1000);
    SET random_status = FLOOR(1 + RAND() * 6);
    SET random_amount = ROUND(10 + RAND() * 990, 2);
    SET random_number = FLOOR(1 + RAND() * 9999);
    SET random_state = RAND();
    SET random_zip_code = CONCAT(
      FLOOR(10000 + RAND() * 90000),
      '-',
      FLOOR(100 + RAND() * 900)
    );
    
    
    INSERT INTO orders (
      customer_id, status, amount, 
      street, city, state, zip_code, number
    ) VALUES (
      random_customer_id, random_status, random_amount,
      CONCAT('Street ', FLOOR(1 + RAND() * 100)),
      CONCAT('City ', FLOOR(1 + RAND() * 100)),
      CASE
        WHEN random_state < 0.2 THEN 'SP'
        WHEN random_state < 0.4 THEN 'RJ'
        WHEN random_state < 0.6 THEN 'MG'
        WHEN random_state < 0.8 THEN 'BA'
        ELSE 'RS'
      END,
      random_zip_code,
      random_number
    );
    
    SET i = i + 1;
  END WHILE;
END //
DELIMITER ;
    
CALL seed_clients(1000);
CALL seed_orders(100000);