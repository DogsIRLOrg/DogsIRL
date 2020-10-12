![Main Logo](/assets/main.png)

## 🐶 Authors 
- Andrew Casper
- Carrington Beard
- Teddy Damian
- Brody Rebne

## 🐶 Problem Domain
Dogs IRL is a platform built on a simple principle: people like pets :). They like pictures of doggos and they want to see more pups. They want to see new pictures from pets they've met every time they log in, they want their good boy or good girl to meet other good boys and good girls, and they want to add those good dogs to their collection to see them every day. For our users, this interaction increases happiness and emotional wellbeing.

## 🐶 Software Requirements
### 🐾 **Vision**
DogsIRL allows pet owners and dog lovers to share their pet pictures, meet and interact with other doggos in the network, and collect cards of every pup they meet to add to their collection. Users can create cards for their pets, adding a picture and attaching cute and personalized doggo statistics such as floofiness, bravery, snuggability, and appetite. The long-term draw of the application is the chance for each user to build a collection of good dogs within the app, which has driven the popularity of other apps in the same space such as Pokemon Go. The difference is that Dogs IRL offers a limitless supply of new and unique good dogs to meet, as new users add their pups to the network. 

That's right: infinite dogs. ♾🐶

Dog owners are active, so it feels important to us that our application live in the mobile space. To support that mobile application, we have built a backend RESTful API in ASP.NET Core to fulfill the demand for pups. We securely store user data using Microsoft's Identity, and host our backend and databases on Microsoft Azure.

[Link to the Github Project for our backend API](https://github.com/401FinalProjectOrg/DogsIRL-API)

Our future stretch goals include geolocation support which we hope will allow users to seek out and meet dogs that are close to them, improving animations and overall appearance of every page, and the ability for users to upload additional images of their dogs for newsfeed/story functionality, as seen on other social networks.

### 🐾 **Scope In**
This mobile app will allow users to create a user profile, login, and verify their email.
Users can then upload a photo of their pet to attach to a card, and add additional information about their pet.
The app will allow pets to virtually interact with each other.
Pets will be able to have short text dialogue interactions.
Users will be able to collect the petcards of other pets once they have interacted.
A profile page shows the cards the user has created, as well as the cards they've collected through their interactions.

### 🐾 **Scope Out**
Selling user data.

Right now, our ambition for this app is for it be a portfolio piece and a demonstration of our experience with .NET and specifically with Xamarin and ASP.NET Core, not to be a commercial product.

### 🐾 **Functional Requirements**
1. Account management, including registration, signin, email confirmation, and password reset functionality.
2. Authentication/Authorization of users.
3. Creation, read, update, and delete functionality of data that represents a pet card.
4. Creation, read, update, and delete functionality of cards that a user has in their collection.

### 🐾 **Data Flow**
	
When a user first joins the app, they will enter their desired username, email, and password information and the front-end application will submit that data via an http POST as a JSON object to the backend API server. The API server will process this data through an account controller, attempt to create an account with this information via Microsoft's Identity Server, and save that data to the Azure SQL user database after salting and hashing the user's password. As part of registration, the backend also sends an email to the provided email address, with a link that will verify that email for that account. If all of this is successful, the API will send an http response message indicating success back to the frontend, and the frontend will show an alert to the user that their registration is complete, and to check their email to verify their account.

When logging in, a user enters their username and password, and the frontend sends this data via an http POST as a JSON object to the backend, to process through the login method of the account controller. The backend will attempt to sign in the user using Microsoft's Identity Server, checking against the username and salted and hashed password in the account SQL database. If this is successful, the backend creates a JSON Web Token (JWT) and sends that token in an http success message to the frontend. The frontend then saves this token to use when accessing greater API functionality, and sends the user to the profile page.

At this point a user can create a 'pet card' and upload an image and assign stats for their pet. This pet card can be taken to the virtual dog park and have an interaction with another user's dog. The user then can collect the other dog's petcard to constantly remind them of that memorable interaction. Users can log-out or create additional pet cards as they choose. Each of these actions makes requests to the backend API server, performing CRUD operations on the user's created pet cards and their collected pet cards.

### 🐾 **Non-Functional Requirements**

Security - Protecting users emails and passwords as they are stored. We will use ASP.NET Core Identity to store users information and hash passwords. Users will be able to sign up and create a profile and not worry about their information being stolen.

## 🐶 ERD Diagram
![ERD Diagram](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20ER%20Diagram.png)
*  *  *  *  *
## 🐶 Wireframe
![Wireframe](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20Wireframes.png)
*  *  *  *  *
## 🐶 Domain Modeling
![DomainModeling](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRLDomainModel.png)
*  *  *  *  *
## 🐶 Forgot Password Structure
![ForgotPassword](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/Forgot-password.png)
*  *  *  *  *

#### 🔨 Patch Notes
October 12 - 2020 || 0.8 Released for testing

August 9 - 2020 || Readme updated.

May 22 - 2020 || App Created.

July 21 - 2020 || Improved account functionality including support for resetting passwords, JWT token authorization for pet cards, enhanced styling, ability to users to edit their pet cards, and image uploads routed through the backend Web API.

#### ☕ Resources
- Blob Storage - https://www.c-sharpcorner.com/article/xamarin-forms-upload-image-to-blob-storage/
- JSON Web Token - https://www.c-sharpcorner.com/article/asp-net-core-web-api-creating-and-validating-jwt-json-web-token/
- StackOverflow - https://stackoverflow.com/
- Microsoft Docs for Xamarin Forms - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/
