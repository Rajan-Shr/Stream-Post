CREATE DATABASE StreamPost;

SELECT * FROM AspNetUsers;
SELECT * FROM Posts
SELECT * FROM Categories
SELECT * FROM Comments		
	
INSERT INTO users(Email,UserName,Password,DateOfBirth,Phone,Gender)
VALUES('xyz@gmail.com','xyz','1234','2024/01/5','9812345678','M');

INSERT INTO posts(Title,Description,PublishedDate,LikeNumber,CommentNumber,UserID,Author,CategoryID,CategoryName)
VALUES ('A','ABCD','2024/01/02',2,4,1,'XYZ',4,'Politics');

INSERT INTO categories(CategoryName)VALUES('Politics');

INSERT INTO comments(CommentText,UserID,PostID)VALUES('Nigga',1,8),('Good',1,8);

DELETE FROM Categories;

ALTER TABLE Posts DROP CONSTRAINT FK_Posts_Categories_CategoryID

ALTER TABLE Posts ADD CONSTRAINT FK_Posts_Categories_CategoryID FOREIGN KEY(CategoryID) REFERENCES Categories(CategoryID) ON DELETE NO ACTION;

SELECT name,delete_referential_action_desc FROM sys.foreign_keys WHERE name = 'FK_Posts_Categories_CategoryID'

DELETE FROM Users;

DROP TABLE Users;
DROP TABLE Categories;
DROP TABLE Posts;
DROP TABLE Comments;
