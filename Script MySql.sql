-- DROP DATABASE IF EXISTS Agenda;
CREATE DATABASE Agenda;
USE Agenda;

-- DROP TABLE IF EXISTS Pessoa;
CREATE TABLE Pessoa
(
Codigo INT PRIMARY KEY AUTO_INCREMENT,
Nome VARCHAR(100) NOT NULL,
DataHoraCadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
Ativo BIT NOT NULL DEFAULT 1
);

-- DROP TABLE IF EXISTS TipoContato;
CREATE TABLE TipoContato
(
Codigo INT PRIMARY KEY AUTO_INCREMENT,
Nome VARCHAR(100) NOT NULL,
RegexValidacao VARCHAR(100) NULL,
DataHoraCadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
Ativo BIT NOT NULL DEFAULT 1
);

INSERT INTO TipoContato
(Nome, RegexValidacao)
VALUES
	('Telefone', '^\\d{2}\\d{8,9}$'),
	('E-mail', '^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$'),
	('WhatsApp', '^\\d{2}\\d{2}\\d{8,9}$');

-- DROP TABLE IF EXISTS Contato;
CREATE TABLE Contato
(
Codigo INT PRIMARY KEY AUTO_INCREMENT,
CodigoPessoa INT NOT NULL,
CodigoTipoContato INT NOT NULL,
Valor VARCHAR(100) NULL,
DataHoraCadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
Ativo BIT NOT NULL DEFAULT 1,
FOREIGN KEY (CodigoPessoa) REFERENCES Pessoa(Codigo),
FOREIGN KEY (CodigoTipoContato) REFERENCES TipoContato(Codigo)
);

SELECT * FROM Pessoa WHERE Ativo = 1;
SELECT * FROM TipoContato WHERE Ativo = 1;
SELECT * FROM Contato WHERE Ativo = 1;