USE master;
GO

CREATE DATABASE Agenda;
GO

USE Agenda;
GO

CREATE TABLE Pessoa
(
	Codigo INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(100) NOT NULL,
	DataHoraCadastro DATETIME NOT NULL DEFAULT GETDATE(),
	Ativo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE TipoContato
(
	Codigo INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(100) NOT NULL,
	RegexValidacao VARCHAR(100) NULL,
	DataHoraCadastro DATETIME NOT NULL DEFAULT GETDATE(),
	Ativo BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO TipoContato
	(Nome, RegexValidacao)
VALUES
	('Telefone', '^\d{2}\d{8,9}$'),
	('E-mail', '^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$'),
	('WhatsApp', '^\d{2}\d{2}\d{8,9}$');

CREATE TABLE Contato
(
	Codigo INT PRIMARY KEY IDENTITY(1,1),
	CodigoPessoa INT NOT NULL,
	CodigoTipoContato INT NOT NULL,
	Valor VARCHAR(100) NULL,
	DataHoraCadastro DATETIME NOT NULL DEFAULT GETDATE(),
	Ativo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CodigoPessoa) REFERENCES Pessoa(Codigo),
    FOREIGN KEY (CodigoTipoContato) REFERENCES TipoContato(Codigo)
);
GO

SELECT * FROM Agenda..Pessoa WHERE Ativo = 1
SELECT * FROM Agenda..TipoContato WHERE Ativo = 1
SELECT * FROM Agenda..Contato WHERE Ativo = 1

