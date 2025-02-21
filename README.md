# 📦 Inventory Management System (IMS)

**🔗 A Robust Inventory Management System using Onion Architecture & .NET Core**

## 📝 Overview  
The **Inventory Management System (IMS)** is a web-based solution designed to help businesses efficiently **manage inventory, track stock levels, and handle product transactions** (sales & purchases). It supports **user authentication, role-based access control, low-stock alerts, product categorization, and detailed reports & analytics**.  

Built with **ASP.NET Core Web API, Entity Framework Core, and ASP.NET Identity**, this project ensures high performance, scalability, and security.  

---

## 🚀 Key Features  
✅ **User Authentication & Role Management** (Admin, Manager, Staff)  
✅ **Product Management** (Add, Update, Delete)  
✅ **Stock Transactions** (Record Sales & Purchases)  
✅ **Low-Stock Alerts & Notifications**  
✅ **Category Management** (Organize products efficiently)  
✅ **Comprehensive Reporting & Analytics** (Sales, Revenue, Inventory Trends)  
✅ **CSV Import/Export for Bulk Data Handling**  
✅ **Email Notifications for Stock Alerts & Transactions**  

---

## 🏗️ Tech Stack  
- **ASP.NET Core Web API** 🔗 (Backend Services)  
- **Entity Framework Core** 🗄️ (Database Management)  
- **ASP.NET Core Identity** 🔑 (User Authentication & Role-Based Access)  
- **SignalR** ⚡ (Real-Time Low-Stock Alerts)  
- **SQL Server** 🏢 (Data Storage)  
- **SMTP Service** 📩 (Email Notifications)  

---

## 📊 Reporting & Analytics  
The system provides **detailed insights** into inventory and sales performance, including:  
📌 **Total Stock Value** – Track inventory worth in real time  
📌 **Sales Reports** – Analyze revenue, top-selling products, and order trends  
📌 **Purchase Reports** – Monitor supplier transactions and restocking history  
📌 **Low-Stock Reports** – Identify products that need urgent restocking  

---

## 🔌 RESTful API Endpoints  
🔹 **GET** `/api/products` → Retrieve all products  
🔹 **POST** `/api/products` → Add a new product  
🔹 **PUT** `/api/products/{id}` → Update product details  
🔹 **DELETE** `/api/products/{id}` → Delete a product  
🔹 **POST** `/api/transactions` → Record a sale or purchase transaction  
🔹 **GET** `/api/reports` → Generate inventory & sales reports  

---

## 🌍 System Architecture (Onion Architecture)  
📌 **Presentation Layer** → ASP.NET Core MVC (User Interface)  
📌 **Application Layer** → Business Logic & Service Interfaces  
📌 **Domain Layer** → Core Entities & Business Rules  
📌 **Infrastructure Layer** → Database Management, Email Service, External APIs  

---

## ⚡ How to Run  
1️⃣ **Clone the Repository:**  
```bash
git clone https://github.com/AdelMohamed/InventoryManagementSystem.git
```
2️⃣ **Update Database Settings in `appsettings.json`**  
3️⃣ **Run the Application:**  
```bash
dotnet run
```

---

## 💡 Contributing  
Got ideas or improvements? Feel free to submit a **Pull Request**! 🚀  

🔗 **Developed by**: Adel Mohamed  

---

**Ready to take inventory management to the next level? 🚀**  
**⭐ Don't forget to Star the project on GitHub!**
