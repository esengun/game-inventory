# Game Inventory Backend Microservices

This project demonstrates backend microservices developed for managing players' inventory in a game.

It makes use of .Net based Microservices created for handling player's inventory in a game. It includes basic CRUD operations for managing items and their categories in a game.

There are two microservices:

Inventory service: Responsible for managing items in a player's inventory.
Catalog service: Responsible for managing categories of inventory items.

Additionally, there is a common library, which includes generic repository pattern and used by both of the above services imported as a NuGet package.

Communication between services: Handled by RabbitMQ
Used DB: MongoDB

Both RabbitMQ and MongoDB is hosted on Docker. Docker Compose is used to manage these services in Docker environment.
