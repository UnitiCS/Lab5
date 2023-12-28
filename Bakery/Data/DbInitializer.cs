using Bakery.Models;
using System;
using System.Linq;

namespace Bakery.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BakeryDBContext db)
        {
            db.Database.EnsureCreated();

            int single_link = 50;
            int many_link = 100;
            Random random = new Random(1);

            string[] productNames = { "Croissant", "Baguette", "Sourdough Bread", "Chocolate Chip Cookie", "Cinnamon Roll" };
            string[] productTypes = { "Pastry", "Bread", "Cookie", "Roll" };
            string[] descriptions = { "Delicious croissant filled with chocolate", "Crusty French baguette", "Soft and chewy cookie with chocolate chips", "Sweet and gooey cinnamon roll" };

            // Generate random product data
            if (!db.BakeryProducts.Any())
            {
                for (int i = 0; i < single_link; i++)
                {
                    db.BakeryProducts.Add(new BakeryProduct
                    {
                        Name = productNames[random.Next(productNames.Length)],
                        Type = productTypes[random.Next(productTypes.Length)],
                        Description = descriptions[random.Next(descriptions.Length)]
                    });
                }
                db.SaveChanges();
            }

            // Generate random ingredient data
            if (!db.Ingredients.Any())
            {
                for (int i = 0; i < single_link; i++)
                {
                    db.Ingredients.Add(new Ingredient
                    {
                        Name = $"Ingredient{i + 1}",
                        Type = "Type" + GetRandomNumbers(2),
                        Quantity = random.Next(10, 100)
                    });
                }
                db.SaveChanges();
            }

            // Generate random supply data
            if (!db.Supplies.Any())
            {
                int ingredientCount = db.Ingredients.Count();

                for (int i = 0; i < many_link; i++)
                {
                    int ingredientId = random.Next(1, ingredientCount + 1);
                    string supplier = $"Supplier{i + 1}";
                    string productName = $"Product{i + 1}";
                    int quantity = random.Next(10, 100);
                    decimal price = random.Next(1, 10);

                    db.Supplies.Add(new Supply
                    {
                        IngredientId = ingredientId,
                        Supplier = supplier,
                        ProductName = productName,
                        Quantity = quantity,
                        Price = price,
                        SupplyDate = GetRandomDate()
                    });
                }
                db.SaveChanges();
            }

            // Generate random bread recipe data
            if (!db.BreadRecipes.Any())
            {
                int bakeryProductCount = db.BakeryProducts.Count();
                int ingredientCount = db.Ingredients.Count();

                for (int i = 0; i < many_link; i++)
                {
                    int ingredientId = random.Next(1, ingredientCount + 1);
                    int bakeryProductId = random.Next(1, bakeryProductCount + 1);
                    string productName = $"Product{i + 1}";
                    string ingredientName = $"Ingredient{i + 1}";
                    int quantityPerUnit = random.Next(1, 10);
                    decimal price = random.Next(1, 10);

                    db.BreadRecipes.Add(new BreadRecipe
                    {
                        IngredientId = ingredientId,
                        BakeryProductId = bakeryProductId,
                        ProductName = productName,
                        IngredientName =(string?) ingredientName,
                        QuantityPerUnit = quantityPerUnit,
                        Price = price
                    });
                }
                db.SaveChanges();
            }

            // Генерация случайных данных для сотрудников
            if (!db.Employees.Any())
            {
                for (int i = 0; i < single_link; i++)
                {
                    db.Employees.Add(new Employee
                    {
                        FirstName = $"EmployeeFirstName{i + 1}",
                        LastName = $"EmployeeLastName{i + 1}",
                        Position = $"Position{i + 1}",
                        Salary = random.Next(30000, 80000)
                    });
                }
                db.SaveChanges();
            }

            // Генерация случайных данных для заказов
            if (!db.Orders.Any())
            {
                int bakeryProductCount = db.BakeryProducts.Count();
                int employeeCount = db.Employees.Count();

                for (int i = 0; i < many_link; i++)
                {
                    int bakeryProductId = random.Next(1, bakeryProductCount + 1);
                    int employeeId = random.Next(1, employeeCount + 1);
                    string customerName = $"Customer{i + 1}";
                    string productName = $"Product{i + 1}";
                    string productType = $"Type{i + 1}";
                    int quantity = random.Next(1, 10);
                    decimal price = random.Next(10, 100);
                    DateTime orderDate = GetRandomDate();
                    DateTime deliveryDate = orderDate.AddDays(random.Next(1, 10));

                    bool isCompleted = random.Next(0, 2) == 1; // Случайное значение true/false
                    bool isDamaged = random.Next(0, 2) == 1;   // Случайное значение true/false

                    db.Orders.Add(new Order
                    {
                        BakeryProductId = bakeryProductId,
                        EmployeeId = employeeId,
                        CustomerName = customerName,
                        ProductName = productName,
                        ProductType = productType,
                        Quantity = quantity,
                        Price = price,
                        OrderDate = orderDate,
                        DeliveryDate = deliveryDate,
                        IsCompleted = isCompleted,
                        IsDamaged = isDamaged
                    });
                }
                db.SaveChanges();
            }
        }

        // Helper methods for random data generation

        private static string GetRandomNumbers(int length)
        {
            Random random = new Random();
            string number = "";

            for (int i = 0; i < length; i++)
            {
                number += random.Next(0, 9).ToString();
            }

            return number;
        }

        private static DateTime GetRandomDate()
        {
            Random random = new Random();
            DateTime startDate = new DateTime(2010, 1, 1);
            DateTime endDate = DateTime.Now;
            int totalDays = (endDate - startDate).Days;

            return startDate.AddDays(random.Next(totalDays));
        }
    }
}
