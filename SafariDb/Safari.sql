DROP DATABASE SafariDb;
CREATE DATABASE SafariDb;

USE SafariDb;

CREATE TABLE BasicUser(
    Id int IDENTITY(1,1) PRIMARY KEY,
    Username varchar(50) NOT NULL,
    LastLogin DateTime DEFAULT GetDate(),
    RegisterDate DateTime DEFAULT GetDate()
);