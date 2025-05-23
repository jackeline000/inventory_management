using System;
using System.Collections.Generic; //use generic lists
using System.IO;  //standard way to handle basic file I/O

namespace InventoryApp
{
    class Product  // Product class to hold inventory item details
    {
        public string? Name { get; set; }  // Product name
        public int Code { get; set; }  // Product code
        public int Quantity { get; set; }   // Quantity in stock
        public float Price { get; set; }  // Price per unit
        public string? MfgDate { get; set; }  // Manufacturing date (MM-YYYY)

        public float TotalValue => Quantity * Price;  // Calculates the total value of this product's stock

        public override string ToString() // Custom string format for displaying product info
        {
            return $"{Name} | Code: {Code} | Qty: {Quantity} | Price: {Price:C} | MFG: {MfgDate}";
        }
    }

    class Program
    {
        static List<Product> inventory = new List<Product>();
        const string filePath = "inventory.txt"; // File to save/load data

        static void Main()
        {
            LoadFromFile();
            int choice;

            do
            // Show menu
            {
                Console.WriteLine("\n=== Inventory Management System ===");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Edit Product");
                Console.WriteLine("3. View Products");
                Console.WriteLine("4. Save Inventory");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                // Menu options
                switch (choice)
                {
                    case 1: AddProduct(); break;
                    case 2: EditProduct(); break;
                    case 3: ViewProducts(); break;
                    case 4: SaveToFile(); break;
                    case 0: Console.WriteLine("Goodbye!"); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }

            } while (choice != 0);
        }

        static void AddProduct() // Add a new product to inventory
        {
            Console.Write("Enter product name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter product code: ");
            int code = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter quantity: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter price: ");
            float price = float.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter manufacturing date (MM-YYYY): ");
            string? date = Console.ReadLine();

            inventory.Add(new Product { Name = name, Code = code, Quantity = qty, Price = price, MfgDate = date });
            Console.WriteLine(" Product added!");
        }

        static void EditProduct() // Edit an existing product
        {
            Console.Write("Enter product code to edit: ");
            int code = int.Parse(Console.ReadLine() ?? "0");

            var product = inventory.Find(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("⚠️ Product not found.");
                return;
            }

            Console.WriteLine("Editing: " + product);

            Console.Write("New Name (leave blank to keep current): ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

            Console.Write("New Quantity: ");
            if (int.TryParse(Console.ReadLine(), out int qty)) product.Quantity = qty;

            Console.Write("New Price: ");
            if (float.TryParse(Console.ReadLine(), out float price)) product.Price = price;

            Console.Write("New MFG Date: ");
            string? date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date)) product.MfgDate = date;

            Console.WriteLine("Product updated!");
        }

        static void ViewProducts()    // Display all products in inventory
        {
            if (inventory.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
                return;
            }

            Console.WriteLine("\n=== Product List ===");
            foreach (var product in inventory)
            {
                Console.WriteLine(product);
            }

            float total = 0;
            foreach (var p in inventory) total += p.TotalValue;
            Console.WriteLine($"\nTotal Inventory Value: {total:C}");
        }

        static void SaveToFile() // Save inventory to a file
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var p in inventory)
                {
                    writer.WriteLine($"{p.Name}|{p.Code}|{p.Quantity}|{p.Price}|{p.MfgDate}");
                }
            }
            Console.WriteLine("Inventory saved to file.");
        }

        static void LoadFromFile() // Load inventory from a file
        {
            if (!File.Exists(filePath)) return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('|');
                if (parts.Length == 5)
                {
                    inventory.Add(new Product
                    {
                        Name = parts[0],
                        Code = int.Parse(parts[1]),
                        Quantity = int.Parse(parts[2]),
                        Price = float.Parse(parts[3]),
                        MfgDate = parts[4]
                    });
                }
            }
            Console.WriteLine("Inventory loaded.");
        }
    }
}
