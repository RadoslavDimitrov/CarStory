# CarStory

CarStory is a web application that lets you store and access data for past car repairs. It is a centralized database that shows all repairs for a given car, based on its VIN number. You can use it to check the service history of any car, add cars to your collection, leave reviews for repair shops, and search for repair shops by name and city.

## Motivation

I created this project as a solution to the problem of paper service history of cars. I wanted to make a platform where car owners and repair shops can easily record and access the car repairs and service history online, without having to keep or carry paper documents. I also wanted to make a platform where car owners can find reliable and trustworthy repair shops, based on the reviews from other users.

## Installation

To run this project locally, you need to have the following tools installed on your machine:<br />

[Git], a version control system<br />
[Docker], a tool for building and running containerized applications<br />
[Docker Compose], a tool for defining and running multi-container applications<br />
Then, follow these steps:<br />

Clone this repository: git clone https://github.com/RadoslavDimitrov/CarStory.git<br />
Go to the project directory: cd CarStory<br />
Build the Docker images: docker-compose build<br />
Start the Docker containers: docker-compose up<br />
Open your browser and go to http://localhost:8000<br />

## Usage
To use this project, you need to register an account or log in with an existing one. You can choose to register as a regular user or as a repair shop user. Then, you can start using the features of this project. Here are some of the features of this project:

### Aa a not logged in user, you can:<br />
&middot;Search for any car by its VIN number and check its service history.<br />
&middot;Search for repair shops by name and city and read their reviews.<br />

### As a regular user, you can:<br />
&middot;Search for any car by its VIN number and check its service history.<br />
&middot;Add cars to your collection for faster access.<br />
&middot;Leave reviews for repair shops that you have visited.<br />
&middot;Search for repair shops by name and city and read their reviews.<br />

### As a repair shop user, you can:<br />
&middot;Add cars to the database with their details.<br />
&middot;Add repairs for any car in the database with their details.<br />
&middot;Edit or delete cars or repairs that you have added.<br />
&middot;View your profile and ratings from other users.<br />
<br />
### Keep in mind that if you register repair shop, admin must approve your registration before you can access it.

Citation
If you use this project in your own work, please cite it as follows:

Radoslav Dimitrov. (2023). CarHistory. GitHub. https://github.com/RadoslavDimitrov/CarStory
