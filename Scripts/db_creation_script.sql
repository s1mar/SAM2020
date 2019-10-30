-- Assuming sam2020 created
-- DROP DATABASE IF EXISTS `sam2020`;
-- CREATE DATABASE `sam2020`;

DROP TABLE IF EXISTS `sam2020`.`paper_authors`;
DROP TABLE IF EXISTS `sam2020`.`notification_recipients`;
DROP TABLE IF EXISTS `sam2020`.`paper`;
DROP TABLE IF EXISTS `sam2020`.`user`;
DROP TABLE IF EXISTS `sam2020`.`role`;
DROP TABLE IF EXISTS `sam2020`.`notification`;

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

CREATE TABLE `sam2020`.`notification` (
  `notification_id` int(11) NOT NULL AUTO_INCREMENT,
  `senderId` int(11) DEFAULT NULL,
  `message` varchar(500) DEFAULT NULL,
  `isRead` tinyint(1) NOT NULL DEFAULT '0',
  `createdDate` datetime DEFAULT NULL,
  PRIMARY KEY (`notification_id`),
  KEY `sender_user_id_idx` (`senderId`),
  CONSTRAINT `sender_user_id` FOREIGN KEY (`senderId`) REFERENCES `user` (`user_id`)
) ;

CREATE TABLE `sam2020`.`notification_recipients` (
  `notification_sender_id` int(11) NOT NULL AUTO_INCREMENT,
  `notification_id` int(11) NOT NULL,
  `recipient_id` int(11) NOT NULL,
  PRIMARY KEY (`notification_sender_id`),
  KEY `notification_sender_id_idx` (`recipient_id`),
  KEY `notification_message_id` (`notification_id`),
  CONSTRAINT `notification_message_id` FOREIGN KEY (`notification_id`) REFERENCES `notification` (`notification_id`),
  CONSTRAINT `notification_recipient_id` FOREIGN KEY (`recipient_id`) REFERENCES `user` (`user_id`)
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