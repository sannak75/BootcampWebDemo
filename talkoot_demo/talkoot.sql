-- tietokannan luonti:
DROP DATABASE IF EXISTS talkoot;
CREATE DATABASE talkoot;
USE talkoot;

DROP TABLE IF EXISTS henkilot;
CREATE TABLE henkilot (
    henkiloid int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    sukunimi varchar(255) NOT NULL,
    etunimi varchar(255),
    email varchar(255),
    tunnus varchar(255),
    pelaajanumero int);


DROP TABLE IF EXISTS talkootyot;
CREATE TABLE talkootyot (
    tyoid int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    henkiloid int NOT NULL FOREIGN KEY REFERENCES henkilot(henkiloid),
    talkoo_tekopva date,
    talkoo_tyo varchar(255),
    talkoo_pisteet int);

-- tallennetaan dataa henkilot-tauluun:
INSERT INTO henkilot (sukunimi,etunimi,email,tunnus,pelaajanumero) 
VALUES  ('Salonen','Raimo','sara@huu.fi','salonen.raimo',10),
('Linna','Pekka','linna.pekka@huu.fi','linpek',15),
('Klem','Sanna','klem@huu.fi','sannak',17),
('Latva','Nina','klem@huu.fi','ninal',33); 

--/ tallennetaan dataa talkootyo-tauluun:
INSERT INTO talkootyot (henkiloid,talkoo_tekopva,talkoo_tyo,talkoo_pisteet) 
VALUES  (1,'2020-02-29','siivous',2),
(1,'2020-01-20','koulutus',4),
(1,'2020-02-01','siivous',2),
(3,'2020-06-01','kahvila',6),
(4,'2020-06-01','koulutus',4);