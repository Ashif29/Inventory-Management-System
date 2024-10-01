
# Inventory Management System

The Inventory Management System is a web application that simplifies product, category, purchase order, and sales order management. It features role-based access for users(Admins, Salesmen, Purchasers, Suppliers, and Consumers), enabling efficient inventory tracking and financial reporting.


## Acknowledgements

 - [This project is instructed by Onnorokom Software Ltd.](https://www.linkedin.com/company/onnorokom-software-ltd-/)

## Tech Stack

**Backend:** ASP.NET Core (C#)

**Frontend:** HTML, CSS, JavaScript, jQuery, Bootstrap, AJAX

**Database:** SQL Server

**Authentication:** ASP.NET Identity

**Logging:** Serilog

**Third-Party Libraries:**
- Image Manipulation: ImageMagick

## Features

- **User Management**: 
  - Role-based access control for Admin, Salesman, Purchaser, Supplier, and Consumer.
  - User registration and authentication.

- **Product Management**: 
  - Maintain product categories and details.
  - Track stock levels with real-time updates.

- **Purchase Order Management**: 
  - Create and manage purchase orders to suppliers.
  - Verify received orders and automatically update stock levels.
  - Update order status upon verification of received goods.

- **Sales Order Management**: 
  - Create and manage sales orders for consumers.
  - Track orders made by sales representatives.
  - Update order status upon verification of completed sales.


- **Financial Reporting**: 
  - Generate reports on total revenue, Cost of Goods Sold (COGS), profit, and loss.
  - View financial summaries based on purchase and sales orders.

- **Three-Layer Architecture**: 
  - **Data Layer**: Manages database access and data models using SQL Server.
  - **Service Layer**: Contains business logic, processes data, and communicates between the data and presentation layers.
  - **Presentation Layer**: Provides a user-friendly interface using HTML, CSS, JavaScript, jQuery, Bootstrap, and AJAX.

- **Pagination**: 
  - Implement pagination for better data navigation in lists and tables.

- **Image Management**: 
  - Utilize ImageMagick for image processing and manipulation within the application.

- **Responsive Design**: 
  - Ensure a user-friendly interface using Bootstrap for a responsive layout.

- **Logging**: 
  - Implement logging with Serilog for tracking application events and errors.

- **AJAX Support**: 
  - Use AJAX for asynchronous data retrieval, enhancing user experience without full page reloads.



## General Navigation Flow

1. **Login / Registration**:
   - Users can log in using their credentials or register for an account.
   - Role-based navigation ensures that each user type (Admin, Salesman, Purchaser, Supplier, Consumer) has access to specific features.
   
2. **Product Management**:
   - Admins can access the **Products** section to add, update, or delete products.
   - Categories can also be managed from this section.

3. **Purchase Orders**:
   - Purchasers can navigate to the **Purchase Orders** section to create, view, and manage purchase orders.
   - Upon verifying a received order, the stock is automatically updated, and the order status changes.

4. **Sales Orders**:
   - Salesmen can access the **Sales Orders** section to create new sales orders for consumers.
   - Sales orders are tracked, and their status can be updated upon verification of the sale.

5. **Inventory Management**:
   - Stock levels are updated automatically after purchase and sales order verifications.
   - Admins and Purchasers can view and manage the inventory.
   - Users can print invoices.

6. **Reports**:
   - The **Reports** section allows admins to generate financial reports (revenue, COGS, profit, and loss) based on sales and purchase order data.

7. **Access Control**:
   - Unauthorized users attempting to access restricted areas will be denied and redirected appropriately.
   - Admins can manage user roles and permissions.
8. **Logout**:
    - Users can log out from any page, returning them to the login screen.

## Screenshots

![]([https://via.placeholder.com/468x300?text=App+Screenshot+Here](https://github.com/Ashif29/Inventory-Management-System/tree/Development/images))


## Database Diagram
![]([[https://via.placeholder.com/468x300?text=App+Screenshot+Here](https://github.com/Ashif29/Inventory-Management-System/tree/Development/images](https://github.com/Ashif29/Inventory-Management-System/blob/Development/IMS%20DATABASE.PNG)))
## Demo

- [Video Presentation](https://drive.google.com/drive/folders/1IKcqUwcVSlSj1mVYH85UdU3KBvTylpp3?usp=drive_link)
## Installation

### Prerequisites

- Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Install [Visual Studio](https://visualstudio.microsoft.com/downloads/)

### Steps

- Clone the repository:

   ```bash
   git clone https://github.com/Ashif29/Inventory-Management-System.git

- Navigate to folder and open the [InventoryManagementSystem.sln] file in Visual Studio.

- Update the appsettings.json file:

    ```bash
    "DefaultSqlConnection": "Server=[Enter your server name]; Database=IMS; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate=True"

- Open Package manager console in visual studio and select InventoryManagementSystem.Data as default project.
  ```bash
  PM> Update-Database

- Run the project

- Default Admin & password:

    ```bash
        Email = admin@gmail.com
        Password = Admin@123
  

## Appendix

### Glossary


- **AJAX**: Asynchronous JavaScript and XML – a technique for creating asynchronous web applications.
- **Serilog**: A logging library for .NET that provides a simple way to log structured data.
- **ImageMagick**: A software suite to create, edit, and compose bitmap images.

### Future Enhancements

- Implement real-time notifications for order updates.
- Expand reporting features to include visual dashboards.
- Add more user roles with tailored permissions.
- And many more.

### Contact Information

For inquiries or support, please contact Md. Ashif Iqbal at [asif.duet.cse19@gamil.com].

## Authors

- [Ashif Iqbal](https://github.com/Ashif29)


## 🔗 Links
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/asif29/)

