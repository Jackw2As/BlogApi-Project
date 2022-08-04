# Blog API Project
[![master](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/master.yml/badge.svg)](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/master.yml)
[![Release](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/Release.yml/badge.svg)](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/Release.yml)

## Summary
The Blog API project is a simple Blog API without any Authentication support. Built using an SQLite memory as storage the Blog API can provide a backend service to create blogs, posts, and comments for other websites.
### A Blog
A blog is a collection of posts with a name and description.
### A Post
is a part of a Blog. It has a title, summary, and a content. Note the planned implementation only accepts text and doesn't support media or file uploading.
### A Comment 
All comments are attached to a Post. Each comment has a username field and a content section. Comments are limitted to only 200 characters. 
## What this Project Contains
This project was developed using industry standard Test Driven Development in C#.
It contains three main projects:
* A Core project built originally as a .NET 6 Class Library, although it also references ASP.NET Libraries when needed. 
* A Application project built on top of ASP.NET 6 using a API Controller and Model Framework. The project doesn't create any views for the website. Instead Swagger is used to provide a frontend for testing.
* A Storage project built on top of Entity Framework Core 6 and SQLite using the repository pattern. SQLite is implemented using an inMemmory database following Microsoft's reccomendations for InMemmory Database. 
And three Test Projects
* Application.Tests
* Domain.Tests
* Storage.Tests
Each test namespace uses the same namespace as the project they test. The project was built with TDD in mind with tests designed before features were implemented. 
The Storage and Domain Tests are both Unit Tests. However The Application project mostly uses intergration tests to test Validation Logic and API support. 

# Using the Project
The database handles 5 similar requests for Blogs, Posts, and Comments:
* GET Request. Takes a valid ID returns the object connected to that ID.
* GET List Request. Lists a collection of Blogs, Posts, or Comments. For Comments Get List requires a valid Post ID and Post Requires a valid Blog ID. This means when a website requests a List of Posts they will only get the Posts connected with their websites blog. (Rather then all websites). Note Get List for blogs returns all blogs. 
* Post Request Takes an object constructed in Json, checks to make sure the object is valid, and then, if valid, creates a new Database entry and returns a location value for the new database object.
* Post Update Request takes a different object from Post Request. It requires a valid ID, and then validates the object. If valid it will update a databases entry.
* Delete Request takes a valid database ID and deletes the model requestesd. This is a hard delete. Note: Blog Delete also deletes any Posts Connected to it. Post also deletes any connected Comments. The reason this happens is because this API doesn't support free standing Posts or Comments. Posts are expected to be apart of a Blog and Comments are about a Post. It wouldn't make sense for Posts or Comments to exist by themselves. 

## Creating a Blog
![image](https://user-images.githubusercontent.com/15194163/182813052-13be2836-db7b-4f3a-ae29-b794c6bbc2cc.png)

Using Swagger you can easily fill in the Request body to create a valid request. The Blog requires a Name and Summary to be provided. The name doesn't have to be unique.

Upon hitting execute you should get back the following:
![image](https://user-images.githubusercontent.com/15194163/182813160-4a10130f-5212-45d9-a0cb-6c1c58350c03.png)

The location value provides a URL for a get request. You can either paste it into another window, or copy the ID and use the Get request to validate its successful implementation if you so wish.

## Creating a Post
Using the Blog ID, provided by creating a blog, you can easily create a new post for your blog. Copy the BlogID into the BlogID field. Then fill out the post.
![image](https://user-images.githubusercontent.com/15194163/182813900-fc30e2c9-f62a-491e-8f9c-cca31fb9b774.png)

Upon hitting execute you should get back the following:
![image](https://user-images.githubusercontent.com/15194163/182813993-08198fc2-9448-4806-9723-9e6e7aff96ce.png)

Web Applications can then use the Get Command to get the data needed to display a post on a webpage. Date Created and Date Modified are automatically created by the Web API and don't accept user input.

Like the Post, the Location Header provides a url querry that can retrieve the Post. 

### Getting a List of Post
![image](https://user-images.githubusercontent.com/15194163/182814685-fd67790b-f691-4191-98ef-cba5253e3cd8.png)

By using your blog Id you can retrive all posts you create for your blog. This means other websites don't need to store each post ID in order to display them from the user. Only the blog ID.

Example result:
![image](https://user-images.githubusercontent.com/15194163/182815428-055d4e14-9f0c-4d6a-bf7a-e61a0113b4da.png)

## Creating A Comment.
In order to create a Comment to a post the user needs to provide the Post ID of the user they are commenting on. In this API, because their is no authentication, the username must be provided. 
![image](https://user-images.githubusercontent.com/15194163/182815981-ade199f5-347d-47fd-897f-a8217778cb8f.png)

Response back:
![image](https://user-images.githubusercontent.com/15194163/182816063-81377ff3-adb4-4fdc-9d17-768a9befc893.png)

### Getting a List of Comments
![image](https://user-images.githubusercontent.com/15194163/182816647-d56f30d6-aac9-4a1a-b43c-9699e88f7f1b.png)
By using your post Id you can retrive all posts you create for your blog. This means other websites don't need to store each comment ID in order to display them from the user. Only the post ID is requred, which can be retreived from a post list.
![image](https://user-images.githubusercontent.com/15194163/182816574-d88cf597-4c61-4869-80a9-2c1c18fd254f.png)

### Updating your Comment
Updating a Comment, or Post, or Blog, requires the ID of the object being created and passing in an Update request. 
![image](https://user-images.githubusercontent.com/15194163/182817088-0c28859d-9ed2-4144-8c54-5b2c8b8bb170.png)

Note: Updating expects different values from creating a post. Thats because this API only supports modifying some of the content of a comment. For example content like usernames shouldn't change as their related to the identity of the comment creator, althought the comment itself can be editted easily. In the event that you want to change a comments username you can delete the comment the comment and create a new one. 

![image](https://user-images.githubusercontent.com/15194163/182817870-d45d6a55-4322-47f9-8972-9e3cdeb19cc2.png)

Also the server automatically updated the DateModified value to reflect the new edited changes. 

##Deleting a Blog/Comment/Post
Deleting is just as easy as creating new content. In this api everything is hard deleted and can't be retrieved once deleted. Because Comments are apart of Posts, which are apart of Blogs, deleting a Blog will also delete all Posts connected to it. Deleting a Post will also delete its comments. There is no point in the database storing Posts for a blog that no longer exists or comments on Post that no longer exist. 

So an easy way to delete the stuff we just created is to delete the blog. 
![image](https://user-images.githubusercontent.com/15194163/182819272-079bd89c-3d70-4d91-9091-ab18ca0a2781.png)

## Copyright
All Rights Reserved
