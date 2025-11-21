-- Create database
CREATE DATABASE CandidateDb;
GO

USE CandidateDb;
GO

-- Main candidate table
CREATE TABLE Candidates (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Contact NVARCHAR(50) NULL,
    Company NVARCHAR(200) NULL,
    Designation NVARCHAR(100) NULL,
    Budget NVARCHAR(50) NULL,
    Technologies NVARCHAR(MAX) NULL,
    AboutProject NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT(GETDATE())
);

-- Documents table for multiple file upload
CREATE TABLE CandidateDocuments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_CandidateDocuments_Candidates
        FOREIGN KEY (CandidateId) REFERENCES Candidates(Id) ON DELETE CASCADE
);
GO
