# DogsIRL

Dogs IRL is social media about dogs, where you can create a card for your pup, send them to meet other pups, and collect other users' cards. How many good boys and good girls can you collect?

## Software Requirements
#### Vision
Here at Dog IRL we believe pets should have an online presence and ability to meet and make new friends. People love showing off their adorable pets and with Dog IRL now they have a social media outlet that allows them to do so. With this app your pets can meet new friends and collect their ‘id cards’ to show how all the pets are connected.

#### Scope In
This web app will allow users to create profiles for their dogs
The app will allow pets to virtually interact with each other
Pets will be able to collect the id cards of fellow pets once they have interacted
Pets will be able to have short text dialogue

#### Scope Out
Our web app will never sell the data of our users.

#### MVP
A website that allows logging in, creating a profile for your pet, and interacting with other pets.

#### Functional Requirements:

1. A user can create a profile and create a pet for that profile.
2. A user can have their pet interact with another users' pet in the virtual 'dog park'
3. Pets that have previously been interacted with will be saved on a users' profile.

#### Data Flow:
A user will sign up and create a profile. At this point a user can create a 'pet card' and upload an image and assign stats for their pet. This pet card can be taken to the virtual dog park and have an interaction with another user's dog. The met dog will have their information saved on the users profile. Users can log-out or create additional pet cards as they choose.

#### Non functional requirements:
- Testing - Unit tests for our CRUD methods, ensure that users can Create, Read, Update and Delete items from the database. This will be implemented using xUnit to test the different aspects of CRUD. We will strive for at least 85% code coverage.

- Security - Protecting users emails and passwords as they are stored. We will use ASP.NET Core Identity to store users information and hash passwords. Users will be able to sign up and create a profile and not worry about their information being stolen.


## Wireframe
[Wireframe](https://drive.google.com/file/d/11XJswLMZZBpF5W85VdL0VMDp647obemz/view)

## User Stories:
[User Story](https://trello.com/b/wQylG1k9/dogsirl)


## Domain Modeling Diagram
[Domain Modeling](https://drive.google.com/file/d/1p34mPbAOCoUien38DvbXNH40R1alwMz_/view?usp=sharing)


## ER Diagram
[ER Diagram](https://drive.google.com/file/d/1Of-FuAD_Dz1cdf47g0-0Bkk9ZcKOtaqH/view)

