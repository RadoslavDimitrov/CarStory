#CarStory

CarStory is a web application that lets you store and access data for past car repairs. It is a centralized database that shows all repairs for a given car, based on its VIN number. You can use it to check the service history of any car, add cars to your collection, leave reviews for repair shops, and search for repair shops by name and city.

#Motivation

I created this project as a solution to the problem of paper service history of cars. I wanted to make a platform where car owners and repair shops can easily record and access the car repairs and service history online, without having to keep or carry paper documents. I also wanted to make a platform where car owners can find reliable and trustworthy repair shops, based on the reviews from other users.

#Installation

To run this project locally, you need to have the following tools installed on your machine:

[Git], a version control system
[Docker], a tool for building and running containerized applications
[Docker Compose], a tool for defining and running multi-container applications
Then, follow these steps:

Clone this repository: git clone https://github.com/RadoslavDimitrov/CarStory.git
Go to the project directory: cd CarStory
Build the Docker images: docker-compose build
Start the Docker containers: docker-compose up
Open your browser and go to http://localhost:8000

#Usage
To use this project, you need to register an account or log in with an existing one. You can choose to register as a regular user or as a repair shop user. Then, you can start using the features of this project. Here are some of the features of this project:

Aa a not logged in user, you can:
Search for any car by its VIN number and check its service history.
Search for repair shops by name and city and read their reviews.

As a regular user, you can:
Search for any car by its VIN number and check its service history.
Add cars to your collection for faster access.
Leave reviews for repair shops that you have visited.
Search for repair shops by name and city and read their reviews.

As a repair shop user, you can:
Add cars to the database with their details.
Add repairs for any car in the database with their details.
Edit or delete cars or repairs that you have added.
View your profile and ratings from other users.

Citation
If you use this project in your own work, please cite it as follows:

Radoslav Dimitrov. (2023). CarHistory. GitHub. https://github.com/RadoslavDimitrov/CarStory
