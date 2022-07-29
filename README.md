# Blog API Project
[![master](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/master.yml/badge.svg)](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/master.yml)[![release](https://github.com/Jackw2As/BlogApi-Project/actions/workflows/Release.yml)
## Summary
The Blog API project is a simple Blog API without any Authentication support. Built using an SQLite memory as storage the Blog API can provide a backend service to create blogs, posts, and comments.
### A Blog
A blog is a collection of posts with a name and description.
### A Post
is a part of a Blog. It has a title, summary, and a content. Note the planned implementation only accepts text and doesn't support media or file uploading.
### A Comment 
All comments are attached to a Post. Each comment has a username field and a content section. Comments are limitted to only 200 characters. 
## What this Project Contains
This project was developed using industry standard Test Driven Development in C#. This project also attempted to implement Clean Code throughout its development.
It contains three projects:
* A Core project built as a .NET 6 Class Library. 
* A Application project built on top of ASP.NET 6.
* A Storage project built on top of Entity Framework Core 6 and SQLite using the repository pattern.
## Copyright
All Rights Reserved
