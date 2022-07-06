# Blog API Project

## Summary
The Blog API project is a simple Blog API without Authentication support. Built using an SQLite in memory as storage the Blog API is capable of providing a backend service to create blogs, posts, and commnets.

### A Blog
A blog is a collection of posts with a name and description.

### A Post
is apart of a Blog. It has a title, summary, and a content. Note the planned implementation only accepts text and doesn't support media or file uploading.

### A Comment 
All comments are attached to a Post. Each comment has a username field and a content section. Comments are limitted to only 200 characters. 

## What this Project Contains

This project was developed using industry standard Test Driven Development in C#. This project also attemptted to implement Clean Code throughout its developement.

It contains three projects:
* A Core project built as a .NET 6 Class Library. 
* A Application project built on top of ASP.NET 6.
* A Storage project built ontop of Entity Framework Core 6 and MySQLite using the repository pattern.

## Copyright
All Rights Reserved
