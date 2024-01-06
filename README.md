# ParkAssist

## An application with features to help parking lot customers, valets, and owners

### Built with:
- .NET 8 / C# 12
- SQL Server database
- API: ASP.NET Core, minimal API, Entity Framework Core 8
- Security: Class library for hashing passwords and generating JWTs
- UI: TBD
- Programming techniques:
	- Asynchronous programming
	- Dependency injection
	- Immutable objects and collections

## Features:
- Register as a new user (customer, valet, or owner)
- Log in
- Log out
- Portals for customers, valets, and owners, administration
	- Customers
		- See registered vehicles
		- Add, update, delete registered vehicles
		- View parking availability by parking lot
		- Request parking at a specific parking lot (if that lot has available parking); creates parking slip with status 'ParkingRequested'
		- See status of a vehicle for which parking has been requested (e.g., has it been parked, if so what spot is it in)
		- Request pickup of a parked vehicle (advances status from VehicleParked to PickupRequested)
		- See parking history by vehicle, parking lot, and all history
	- Valets
		- See available and occupied parking for assigned parking lot, including vehicle and customer details for occupied parking spots
		- See parking requests
		- Process parking requests (advances status from 'ParkingRequested' to 'VehicleParked')
		- See vehicle pickup requests
		- Process vehicle pickup requests (advances status from 'PickupRequested' to 'VehiclePickedUp', time out, amount due, and amount paid are calculated)
	- Owners
		- See info on owned parking lots
		- Create, update, and delete owned parking lots/spots
		- See financial information for any or all owned parking lots
	- Administration
		- View account details, including assigned roles
		- Change password
		- Change username
		- Change email
		- Change phone
		- Owners and valets can add the customer role
		- Valets can change their assigned parking lot

## Business rules:
- Valets can also be customers, but they cannot request parking at their assigned lot
- Owners can also be customers, but they cannot request parking at any lot they own
- Pricing schedules vary by lot but follow the same structure:
	- Hourly rates for weekdays, weekends, and special events
	- Beyond the 24th hour, all hours parked are charged at a 10% discount
	- Discounts (including the beyond 24 hour discount, which is applied before any others) are stackable, but discounts beyond the first are taken off the remaining amount, not the original total

## UI conventions:
- TBD

## Instructions for running the application:
- Note that SQL server and Visual Studio are required for running the application
- Clone or download the repo
- Browse to \ParkAssist\Database
- Run the database script 'parkassist-create-db-and-initialize-data.sql'
	- This script will drop (if exists) and re-create the database and tables
	- The script inserts a small amount of data required by the application into the database 
	- Note that there is optional sample data that can be inserted into the database as well; it can be found throughout the database script and it is commented out
- Browse to \ParkAssist\ParkAssist.API\appsettings.json
	- There is a database connection string in this config file that needs to point to your database
- Run the solution in Visual Studio

## Improvement opportunities:
- TBD




- 