using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project0.DataAccess
{
    public partial class project0Context : DbContext
    {
        public project0Context()
        {
        }

        public project0Context(DbContextOptions<project0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizza> Pizza { get; set; }
        public virtual DbSet<PizzaIngredients> PizzaIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__address__3213E83E33BD5B00")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(255);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(255);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(255);

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__customer__3213E83E96B2AFB8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(255);

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_customer_address");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_location");
            });

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.ToTable("ingredients");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__ingredie__3213E83EB74E3CC8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__inventor__3213E83ED313485C")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.IngredientsId })
                    .HasName("store_ingredients_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IngredientsId).HasColumnName("ingredients_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Ingredients)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.IngredientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_ingredients");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_location");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.HasIndex(e => e.AddressId)
                    .HasName("UQ__location__CAA247C9D9E1E1B5")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__location__3213E83EC1763E03")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasColumnName("location_name")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.Location)
                    .HasForeignKey<Location>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_location_address");
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.ToTable("order_items");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__order_it__3213E83EB8558632")
                    .IsUnique();

                entity.HasIndex(e => new { e.OrderId, e.PizzaId })
                    .HasName("order_pizza_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.PizzaId).HasColumnName("pizza_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orderitems_orders");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.PizzaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orderitems_pizza");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__orders__3213E83EBE8A0872")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.OrderTime).HasColumnName("order_time");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_orders_address");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_customer");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_location");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("pizza");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__pizza__3213E83EED8D9C3B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<PizzaIngredients>(entity =>
            {
                entity.ToTable("pizza_ingredients");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__pizza_in__3213E83E24088C10")
                    .IsUnique();

                entity.HasIndex(e => new { e.PizzaId, e.IngredientsId })
                    .HasName("pizza_ingredients_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IngredientsId).HasColumnName("ingredients_id");

                entity.Property(e => e.PizzaId).HasColumnName("pizza_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Ingredients)
                    .WithMany(p => p.PizzaIngredients)
                    .HasForeignKey(d => d.IngredientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pizzaingredients_ingredients");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.PizzaIngredients)
                    .HasForeignKey(d => d.PizzaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pizzaingredients_pizza");
            });
        }
    }
}
