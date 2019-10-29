-- Assuming sam2020 created
-- DROP DATABASE IF EXISTS `sam2020`;
-- CREATE DATABASE `sam2020`;

DROP TABLE IF EXISTS `sam2020`.`paper_authors`;
DROP TABLE IF EXISTS `sam2020`.`paper`;
DROP TABLE IF EXISTS `sam2020`.`user`;
DROP TABLE IF EXISTS `sam2020`.`role`;

-- START Create Tables 

CREATE TABLE `sam2020`.`role` (
  `role_id` int(11) NOT NULL AUTO_INCREMENT,
  `role_name` varchar(45) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE KEY `role_id_UNIQUE` (`role_id`),
  UNIQUE KEY `role_name_UNIQUE` (`role_name`)
);

CREATE TABLE `sam2020`.`user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `role_id` int(11) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  KEY `user_role_idx` (`role_id`),
  CONSTRAINT `user_role` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
);

CREATE TABLE `sam2020`.`paper` (
  `paper_id` int(11) NOT NULL AUTO_INCREMENT,
  `versionNum` int(11) NOT NULL,
  `document` blob,
  `submissionDate` datetime DEFAULT NULL,
  `rating` decimal(2,1) DEFAULT NULL,
  `contactEmail` varchar(100) DEFAULT NULL,
  `approvalStatus` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`paper_id`)
);

CREATE TABLE `sam2020`.`paper_authors` (
  `paper_author_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` int(11) NOT NULL,
  `author_id` int(11) NOT NULL,
  `is_contact` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`paper_author_id`),
  KEY `paper_id_idx` (`paper_id`),
  KEY `author_id_idx` (`author_id`),
  CONSTRAINT `author_id` FOREIGN KEY (`author_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `paper_id` FOREIGN KEY (`paper_id`) REFERENCES `paper` (`paper_id`)
) ;

-- END Create Tables

-- START Inserting Data

INSERT INTO `sam2020`.`role`
(role_name)
VALUES
('Administrator'),
('Author'),
('PCC'),
('PCM');

-- END Inserting Data