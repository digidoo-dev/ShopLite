using Microsoft.EntityFrameworkCore;
using ShopLite.Data;

namespace ShopLite.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ShopLiteContext(serviceProvider.GetRequiredService<DbContextOptions<ShopLiteContext>>()))
        {

            /*
             * Seed data for demonstration purposes only.
             * 
             * Product images in this seed use URLs from Unsplash (https://unsplash.com),
             * which provides free, high-quality photos under the Unsplash License.
             * 
             * This project is a non-commercial portfolio example with minimal traffic,
             * so direct linking (hotlinking) to Unsplash images is acceptable.
             * 
             * In a production or commercial environment, all images should be downloaded
             * and hosted locally (e.g. under wwwroot/images/products) or served from a 
             * dedicated CDN to avoid external bandwidth usage.
             */


            if (!context.Category.Any())
            {
                context.Category.AddRange(
                    new Category { Name = "Laptops" },
                    new Category { Name = "Smartphones" },
                    new Category { Name = "Tablets" },
                    new Category { Name = "Monitors" },
                    new Category { Name = "Headphones" },
                    new Category { Name = "Smartwatches" },
                    new Category { Name = "Accessories" }
                );

                context.SaveChanges();
            }

            var laptops = context.Category.First(c => c.Name == "Laptops");
            var smartphones = context.Category.First(c => c.Name == "Smartphones");
            var tablets = context.Category.First(c => c.Name == "Tablets");
            var monitors = context.Category.First(c => c.Name == "Monitors");
            var headphones = context.Category.First(c => c.Name == "Headphones");
            var smartwatches = context.Category.First(c => c.Name == "Smartwatches");
            var accessories = context.Category.First(c => c.Name == "Accessories");



            if (!context.Product.Any())
            {
                context.Product.AddRange(
                    // Laptops
                    new Product { Category = laptops, Name = "Dell XPS 13", Description = "13-inch ultrabook with Intel i7 and 16GB RAM.", ImageUrl = "/images/products/laptop1.jpg", Price = 5999.99M },
                    new Product { Category = laptops, Name = "MacBook Air M3", Description = "Lightweight Apple laptop with M3 chip.", ImageUrl = "/images/products/laptop2.jpg", Price = 7499.00M },
                    new Product { Category = laptops, Name = "Lenovo ThinkPad X1 Carbon", Description = "Business-class ultrabook with 14-inch display.", ImageUrl = "/images/products/laptop1.jpg", Price = 6799.00M },
                    new Product { Category = laptops, Name = "HP Spectre x360", Description = "Convertible 2-in-1 laptop with touchscreen.", ImageUrl = "/images/products/laptop2.jpg", Price = 6299.00M },
                    new Product { Category = laptops, Name = "ASUS ROG Zephyrus G14", Description = "Gaming laptop with RTX 4060 GPU.", ImageUrl = "/images/products/laptop1.jpg", Price = 8499.00M },

                    // Smartphones
                    new Product { Category = smartphones, Name = "iPhone 15 Pro", Description = "Apple flagship smartphone with A17 Pro chip.", ImageUrl = "/images/products/smartphone1.jpg", Price = 6299.00M },
                    new Product { Category = smartphones, Name = "Samsung Galaxy S24", Description = "Android smartphone with AMOLED 120Hz display.", ImageUrl = "/images/products/smartphone2.jpg", Price = 5899.00M },
                    new Product { Category = smartphones, Name = "Google Pixel 8", Description = "AI-powered smartphone with top-tier camera.", ImageUrl = "/images/products/smartphone1.jpg", Price = 5199.00M },
                    new Product { Category = smartphones, Name = "OnePlus 12", Description = "Fast and smooth flagship Android phone.", ImageUrl = "/images/products/smartphone2.jpg", Price = 4799.00M },
                    new Product { Category = smartphones, Name = "Xiaomi 14 Pro", Description = "High-performance phone at an affordable price.", ImageUrl = "/images/products/smartphone1.jpg", Price = 3599.00M },

                    // Tablets
                    new Product { Category = tablets, Name = "iPad Pro 12.9", Description = "Apple tablet with M2 chip and Liquid Retina display.", ImageUrl = "/images/products/tablet1.jpg", Price = 7299.00M },
                    new Product { Category = tablets, Name = "Samsung Galaxy Tab S9", Description = "Android tablet with S Pen and AMOLED screen.", ImageUrl = "/images/products/tablet2.jpg", Price = 4999.00M },
                    new Product { Category = tablets, Name = "Lenovo Tab P12 Pro", Description = "Productivity tablet with detachable keyboard.", ImageUrl = "/images/products/tablet1.jpg", Price = 3899.00M },
                    new Product { Category = tablets, Name = "Microsoft Surface Go 3", Description = "Compact Windows tablet for mobility.", ImageUrl = "/images/products/tablet2.jpg", Price = 3599.00M },
                    new Product { Category = tablets, Name = "Amazon Fire HD 10", Description = "Affordable tablet for entertainment and reading.", ImageUrl = "/images/products/tablet1.jpg", Price = 899.00M },

                    // Monitors
                    new Product { Category = monitors, Name = "Dell UltraSharp 27", Description = "27-inch 4K IPS monitor for professionals.", ImageUrl = "/images/products/monitor1.jpg", Price = 2299.00M },
                    new Product { Category = monitors, Name = "LG Ultrawide 34", Description = "34-inch ultrawide monitor for productivity.", ImageUrl = "/images/products/monitor2.jpg", Price = 2999.00M },
                    new Product { Category = monitors, Name = "Samsung Odyssey G9", Description = "Curved 49-inch gaming monitor.", ImageUrl = "/images/products/monitor1.jpg", Price = 5899.00M },
                    new Product { Category = monitors, Name = "ASUS ProArt Display", Description = "Color-accurate monitor for designers.", ImageUrl = "/images/products/monitor2.jpg", Price = 2499.00M },
                    new Product { Category = monitors, Name = "BenQ PD2700U", Description = "27-inch UHD monitor with factory calibration.", ImageUrl = "/images/products/monitor1.jpg", Price = 2199.00M },

                    // Headphones
                    new Product { Category = headphones, Name = "Sony WH-1000XM5", Description = "Noise-cancelling wireless headphones.", ImageUrl = "/images/products/headphones1.jpg", Price = 1599.00M },
                    new Product { Category = headphones, Name = "Apple AirPods Pro 2", Description = "True wireless earbuds with ANC.", ImageUrl = "/images/products/headphones2.jpg", Price = 1399.00M },
                    new Product { Category = headphones, Name = "Bose QuietComfort 45", Description = "Over-ear headphones with balanced sound.", ImageUrl = "/images/products/headphones1.jpg", Price = 1499.00M },
                    new Product { Category = headphones, Name = "Sennheiser HD 560S", Description = "Open-back headphones for audiophiles.", ImageUrl = "/images/products/headphones2.jpg", Price = 1299.00M },
                    new Product { Category = headphones, Name = "JBL Tune 510BT", Description = "Affordable Bluetooth headphones.", ImageUrl = "/images/products/headphones1.jpg", Price = 299.00M },

                    // Smartwatches
                    new Product { Category = smartwatches, Name = "Apple Watch Series 9", Description = "Smartwatch with health tracking and GPS.", ImageUrl = "/images/products/smartwatch1.jpg", Price = 2699.00M },
                    new Product { Category = smartwatches, Name = "Samsung Galaxy Watch 6", Description = "Wear OS smartwatch with AMOLED display.", ImageUrl = "/images/products/smartwatch2.jpg", Price = 2199.00M },
                    new Product { Category = smartwatches, Name = "Garmin Fenix 7", Description = "Multisport smartwatch with GPS and sensors.", ImageUrl = "/images/products/smartwatch1.jpg", Price = 3299.00M },
                    new Product { Category = smartwatches, Name = "Fitbit Versa 4", Description = "Fitness smartwatch with heart-rate tracking.", ImageUrl = "/images/products/smartwatch2.jpg", Price = 999.00M },
                    new Product { Category = smartwatches, Name = "Huawei Watch GT 4", Description = "Stylish smartwatch with long battery life.", ImageUrl = "/images/products/smartwatch1.jpg", Price = 1299.00M },

                    // Accessories
                    new Product { Category = accessories, Name = "Logitech MX Master 3S", Description = "Wireless ergonomic mouse with USB-C.", ImageUrl = "/images/products/accesory1.jpg", Price = 499.00M },
                    new Product { Category = accessories, Name = "Keychron K6", Description = "Compact mechanical keyboard with Bluetooth.", ImageUrl = "/images/products/accesory2.jpg", Price = 399.00M },
                    new Product { Category = accessories, Name = "Anker PowerCore 20000", Description = "High-capacity power bank with fast charging.", ImageUrl = "/images/products/accesory1.jpg", Price = 249.00M },
                    new Product { Category = accessories, Name = "Belkin USB-C Hub", Description = "7-in-1 docking station for laptops.", ImageUrl = "/images/products/accesory2.jpg", Price = 349.00M },
                    new Product { Category = accessories, Name = "Sandisk Extreme SSD 1TB", Description = "Portable SSD drive with USB 3.2 support.", ImageUrl = "/images/products/accesory1.jpg", Price = 599.00M }
                );

                context.SaveChanges();
            }
        }
    }
}
