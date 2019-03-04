--Drop Database project0;
--go

--Create Database project0;
--go

--Create schema project0;
--GO

CREATE TABLE customer
(
	id int Unique Not Null Identity,
	first_name nvarchar(255),
	last_name nvarchar(255),
	email nvarchar(255),	
	address_id int, 
	store_id int NOT NULL, --customer has a default store location
	Constraint customerPK Primary Key (id)
);

CREATE TABLE orders
(
	id int Unique Not Null Identity,
	customer_id int Not Null,
	store_id int Not Null,
    address_id int, --null address means carry out, not delivery
	order_time datetime2 Not Null, --set to time of order creation
	Constraint ordersPK Primary Key (id)
);

CREATE TABLE order_items  
(
	 id int Not Null Unique Identity,
	 order_id  int Not Null,
	 pizza_id  int Not Null,
	 quantity  int Not Null, --multiples of a pizza can be in an order
	 Constraint PK Primary Key (id),
	 Constraint order_pizza_unique Unique (order_id, pizza_id) 
	 --number of pizza in an order is specified by quantity, not another row with the same id combo
);

CREATE TABLE location  
(
	 id int Unique Not Null Identity,
	 location_name nvarchar(255) Not Null,
	 address_id int Not Null Unique,  --store's physical location
	 Constraint locationPK Primary Key (id)
);

CREATE TABLE pizza  
(
	 id int Unique Not Null Identity,
	 price decimal(5,2) Not Null, --assumed pizza won't cost more than 999.99
	 Constraint pizzaPK Primary Key (id)
);

CREATE TABLE ingredients  
(
	 id int Unique Not Null Identity,
	 name  nvarchar(255) Not Null,
	 Constraint ingredientsPK Primary Key (id)
);

CREATE TABLE pizza_ingredients  
(
    id int Not Null Unique Identity, 
	pizza_id int Not Null,
	ingredients_id int Not Null,
    quantity int Not Null, --number of the ingredient required to make the pizza
	Constraint pizza_ingredientsPK Primary Key (id),
    Constraint pizza_ingredients_unique Unique (pizza_id, ingredients_id)
	--number of ingredients in a pizza is specified by quantity, not another row with the same id combo
);


CREATE TABLE inventory  
(
	 id int Unique Not Null Identity,
	 store_id  int Not Null,
	 ingredients_id  int Not Null,
	 quantity  int Not Null, --number of ingredients in the inventory
	 Constraint inventoryPK Primary Key (id),
	 Constraint store_ingredients_unique Unique (store_id, ingredients_id)
	 --number of ingredients in a store's inventory is specified by quantity, not another row with the same id combo
);

CREATE TABLE address  
(
	 id int Unique Not Null Identity,
	 street  nvarchar(255) Not Null,
	 city  nvarchar(255),
	 state  nvarchar(255),
	 country nvarchar(255) Not Null,
	 zipcode  nvarchar(255),
	 Constraint addressPK Primary Key (id)
);

ALTER TABLE customer ADD Constraint FK_customer_location FOREIGN KEY ( store_id ) REFERENCES location (id);

ALTER TABLE customer ADD Constraint FK_customer_address FOREIGN KEY ( address_id ) REFERENCES address (id);

ALTER TABLE orders ADD Constraint FK_orders_location FOREIGN KEY ( store_id ) REFERENCES location (id);

ALTER TABLE orders ADD Constraint FK_orders_customer FOREIGN KEY ( customer_id ) REFERENCES customer (id);

ALTER TABLE orders ADD Constraint FK_orders_address FOREIGN KEY ( address_id ) REFERENCES address (id);

ALTER TABLE order_items ADD Constraint FK_orderitems_orders FOREIGN KEY ( order_id ) REFERENCES orders (id);

ALTER TABLE order_items ADD Constraint FK_orderitems_pizza FOREIGN KEY ( pizza_id ) REFERENCES pizza (id);

ALTER TABLE location ADD Constraint FK_location_address FOREIGN KEY (address_id) REFERENCES address (id);

ALTER TABLE pizza_ingredients ADD Constraint FK_pizzaingredients_pizza FOREIGN KEY (pizza_id) REFERENCES pizza (id);

ALTER TABLE pizza_ingredients ADD Constraint FK_pizzaingredients_ingredients FOREIGN KEY (ingredients_id) REFERENCES ingredients (id);

ALTER TABLE inventory ADD Constraint FK_inventory_ingredients FOREIGN KEY (ingredients_id) REFERENCES ingredients (id);

ALTER TABLE inventory ADD Constraint FK_inventory_location FOREIGN KEY (store_id) REFERENCES location (id);

GO

--Create Trigger NoSameCustomerStoreOrderWithinTwoHours on project0.orders
--For Insert
--AS
--Begin
--       Begin Try
--		IF EXISTS (SELECT * 
--					FROM Orders
--					Where (Select inserted.customer_id From inserted) = orders.customer_id
--					AND (Select inserted.store_id From inserted) = orders.store_id
--					AND ( (DATEDIFF(hour, orders.order_time, (Select inserted.order_time From inserted) ) >= 2 )	
--							OR DATEDIFF(hour, (Select inserted.order_time From inserted), orders.order_time) >= 2 ) )	
--		Begin
--			RAISERROR ('A customer cannot order from the same store within 2 hours', 16, 1);  
--		END
--		--else is insert normally
--		End Try
--		Begin Catch
--			Print Error_Message();
--		End Catch
--End;

--GO 

--not finished
--Create Trigger OrderDecreasesInventory on project0.orders
--For Insert
--AS
--Begin
--		Update inventory
--		SET inventory.quantity = inventory.quantity - (Select order_items.quantity From inserted join order_items on inserted.id = order_items.order_id)
--		WHERE inventory.id IN (Select inventory.id FROM inserted join location on inserted.store_id = location.id
--						join inventory on location.id = inventory.store_id)
--END;

