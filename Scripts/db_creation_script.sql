-- DROP schema IF ExISTS
DROP DATABASE IF EXISTS `sam2020`;

-- DROP Constraints. ONLY if modifying existing DB rather than dropping

-- ALTER TABLE `sam2020`.`paper` 
-- DROP FOREIGN KEY `document_entry`;
-- ALTER TABLE `sam2020`.`paper` 
-- DROP INDEX `document_entry_idx` ;

-- ALTER TABLE `sam2020`.`paper_documents` 
-- DROP FOREIGN KEY `paper_document`;
-- ALTER TABLE `sam2020`.`paper_documents` 
-- DROP INDEX `paper_document_idx` ;

-- CREATE schema

CREATE DATABASE `sam2020`;

-- DROP Tables if exist

DROP TABLE IF EXISTS `sam2020`.`preferences`;
DROP TABLE IF EXISTS `sam2020`.`reviews`;
DROP TABLE IF EXISTS `sam2020`.`review_requests`;
DROP TABLE IF EXISTS `sam2020`.`paper_documents`;
DROP TABLE IF EXISTS `sam2020`.`paper_authors`;
DROP TABLE IF EXISTS `sam2020`.`notification_recipients`;
DROP TABLE IF EXISTS `sam2020`.`paper`;
DROP TABLE IF EXISTS `sam2020`.`notification`;
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
  `documentId` int(11) DEFAULT NULL,
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

CREATE TABLE `sam2020`.`paper_documents` (
  `document_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` int(11) NOT NULL,
  `document` blob,
  `filename` varchar(100) NOT NULL,
  `versionNum` int(11) NOT NULL,
  `extension` varchar(10) NOT NULL,
  `createdDate` datetime NOT NULL,
  PRIMARY KEY (`document_id`),
  KEY `paper_document_idx` (`paper_id`),
  CONSTRAINT `paper_document` FOREIGN KEY (`paper_id`) REFERENCES `paper` (`paper_id`)
) ;

CREATE TABLE `sam2020`.`review_requests` (
  `review_request_id` INT NOT NULL AUTO_INCREMENT,
  `paper_id` INT NOT NULL,
  `assignee_id` INT NOT NULL,
  PRIMARY KEY (`review_request_id`),
  INDEX `requested_paper_id_idx` (`paper_id` ASC) VISIBLE,
  INDEX `requesting_user_id_idx` (`assignee_id` ASC) VISIBLE,
  CONSTRAINT `requested_paper_id`
    FOREIGN KEY (`paper_id`)
    REFERENCES `sam2020`.`paper` (`paper_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `requesting_user_id`
    FOREIGN KEY (`assignee_id`)
    REFERENCES `sam2020`.`user` (`user_id`)
) ;

CREATE TABLE `sam2020`.`reviews` (
  `review_id` INT NOT NULL AUTO_INCREMENT,
  `paper_id` INT NOT NULL,
  `reviewer_id` INT NOT NULL,
  `text` VARCHAR(500) NULL,
  `createdDate` DATETIME NOT NULL,
  `editedDate` DATETIME NULL,
  PRIMARY KEY (`review_id`),
  INDEX `review_paper_idx` (`paper_id` ASC) VISIBLE,
  INDEX `reviewer_review_idx` (`reviewer_id` ASC) VISIBLE,
  CONSTRAINT `review_paper`
    FOREIGN KEY (`paper_id`)
    REFERENCES `sam2020`.`paper` (`paper_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `reviewer_review`
    FOREIGN KEY (`reviewer_id`)
    REFERENCES `sam2020`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);

CREATE TABLE `sam2020`.`notification` (
  `notification_id` int(11) NOT NULL AUTO_INCREMENT,
  `senderId` int(11) DEFAULT NULL,
  `message` varchar(500) DEFAULT NULL,
  `createdDate` datetime DEFAULT NULL,
  PRIMARY KEY (`notification_id`),
  KEY `sender_user_id_idx` (`senderId`),
  CONSTRAINT `sender_user_id` FOREIGN KEY (`senderId`) REFERENCES `user` (`user_id`)
) ;

CREATE TABLE `sam2020`.`notification_recipients` (
  `notification_sender_id` int(11) NOT NULL AUTO_INCREMENT,
  `notification_id` int(11) NOT NULL,
  `recipient_id` int(11) NOT NULL,
  `isRead` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`notification_sender_id`),
  KEY `notification_sender_id_idx` (`recipient_id`),
  KEY `notification_message_id` (`notification_id`),
  CONSTRAINT `notification_message_id` FOREIGN KEY (`notification_id`) REFERENCES `notification` (`notification_id`),
  CONSTRAINT `notification_recipient_id` FOREIGN KEY (`recipient_id`) REFERENCES `user` (`user_id`)
) ;

CREATE TABLE `sam2020`.`preferences` (
  `preference_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_submission` DATETIME,
  `review_submission` DATETIME,
  `review_choice` DATETIME,
  `author_notification` DATETIME,
  PRIMARY KEY (`preference_id`)
);

-- END Create Tables

-- START ALTER Tables

ALTER TABLE `sam2020`.`paper` 
ADD INDEX `document_entry_idx` (`documentId` ASC) VISIBLE;

ALTER TABLE `sam2020`.`paper` 
ADD CONSTRAINT `document_entry`
  FOREIGN KEY (`documentId`)
  REFERENCES `sam2020`.`paper_documents` (`document_id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;


-- END ALTER Tables

-- START Inserting Data

INSERT INTO `sam2020`.`role`
(role_name)
VALUES
('Administrator'),
('Author'),
('PCC'),
('PCM');

-- INSERT Users

INSERT INTO `sam2020`.`user`
(username, password, role_id)
VALUES
('admin@admin.com','admin',1);

INSERT INTO `sam2020`.`preferences`
(paper_submission, review_submission, review_choice, author_notification)
VALUES('2019-11-03 00:00:00','2019-11-03 00:00:00','2019-11-03 00:00:00','2019-11-03 00:00:00');

-- END Inserting Data