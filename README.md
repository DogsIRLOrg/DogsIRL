![Main Logo](/assets/main.png)

## Authors
- Andrew Casper
- Carrington Beard
- Teddy Damian
- Brody Rebne

## Problem Domain
Dogs IRL is a platform built on a simple principle: people like pets :). They like pictures of doggos and they want to see more pups. They want to see new pictures from pets they've met every time they log in, they want their good boy or good girl to meet other good boys and good girls, and they want to add those good dogs to their collection to see them every day. For our users, this interaction increases happiness and emotional wellbeing.

## Software Requirements
### **Vision**
DogsIRL allows pet owners and dog lovers to share their pet pictures, meet and interact with other doggos in the network, and collect cards of every pup they meet to add to their collection. Users can create cards for their pets, adding a picture and attaching cute and personalized doggo statistics such as floofiness, bravery, snuggability, and appetite. The long-term draw of the application is the chance for each user to build a collection of good dogs within the app, which has driven the popularity of other apps in the same space such as Pokemon Go. The difference is that Dogs IRL offers a limitless supply of new and unique good dogs to meet, as new users add their pups to the network. 

That's right: infinite dogs.

Dog owners are active, so it feels important to us that our application live in the mobile space. To support that mobile application, we have built a backend RESTful API in ASP.NET Core to fulfill the demand for pups. We securely store user data using Microsoft's Identity Server, and host our backend and databases on Microsoft Azure.

Our future stretch goals include geolocation support which we hope will allow users to seek out and meet dogs that are close to them, improving animations and overall appearance of every page, and the ability for users to upload additional images of their dogs for newsfeed/story functionality.

### **Scope In**
This web app will allow users to create profiles for their dogs.
The app will allow pets to virtually interact with each other.
User will be able to collect the petcards of fellow pets once they have interacted.
Pets will be able to have short text dialogue.

### **Scope Out**
Selling user data.

Right now, our ambition for this app is for it be a portfolio piece and a demonstration of our experience with .NET and specifically with Xamarin and ASP.NET Core

### **Functional Requirements**
1. A user can create a profile and create a pet for that profile.
2. A user can have their pet interact with another users' pet in the virtual 'dog park'
3. A user can collect petcards from the other dog for their own collection.

### **Data Flow**
	
A user will sign up and create a profile. At this point a user can create a 'pet card' and upload an image and assign stats for their pet. This pet card can be taken to the virtual dog park and have an interaction with another user's dog. The user then can collect the other dog's petcard to constantly remind them of that memorable interaction. Users can log-out or create additional pet cards as they choose.

### **Non-Functional Requirements**

Security - Protecting users emails and passwords as they are stored. We will use ASP.NET Core Identity to store users information and hash passwords. Users will be able to sign up and create a profile and not worry about their information being stolen.

## ERD Diagram
![ERD Diagram](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20ER%20Diagram.png)
*  *  *  *  *
## Wireframe
![Wireframe](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20Wireframes.png)
*  *  *  *  *
## Domain Modeling
![DomainModeling](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRLDomainModel.png)
*  *  *  *  *
## Forgot Password Structure
![ForgotPassword](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/Forgot-password.png)
*  *  *  *  *

#### Patch Notes
May 22 - 2020 || App Created.
July 21 - 2020 || Improved account functionality including support for resetting passwords, JWT token authorization for pet cards, enhanced styling, ability to users to edit their pet cards, and image uploads routed through the backend Web API.

#### Resources used
- Blob Storage - https://www.c-sharpcorner.com/article/xamarin-forms-upload-image-to-blob-storage/
- JSON Web Token - https://www.c-sharpcorner.com/article/asp-net-core-web-api-creating-and-validating-jwt-json-web-token/
- StackOverflow - https://stackoverflow.com/
- Microsoft Docs for Xamarin Forms - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/
