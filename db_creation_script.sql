-- Assuming sam2020 created
DROP DATABASE `sam2020`;
CREATE DATABASE `sam2020`;
 
use `sam2020` ;
-- START Create Tables 

CREATE TABLE `sam2020`.`role` (
  `role_id` int(11) NOT NULL AUTO_INCREMENT,
  `role_name` varchar(45) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE KEY `role_id_UNIQUE` (`role_id`),
  UNIQUE KEY `role_name_UNIQUE` (`role_name`)
);
describe role;

CREATE TABLE `sam2020`.`user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `role_id` int(11) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  KEY `user_role_idx` (`role_id`),
  CONSTRAINT `user_role` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
);
describe user;

CREATE TABLE `sam2020`.`paper` (
  `paper_id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(100) DEFAULT NULL,
  `reference_name` varchar(100) DEFAULT NULL,
  `co_authors` varchar(100) DEFAULT NULL,
  `topic` varchar(200) DEFAULT NULL,
  `author_id` int(11) NOT NULL,
  `version` int(11) NOT NULL,
  `file_reference` varchar(100) DEFAULT NULL,
  `submission_date` datetime DEFAULT NULL,
  `final_review` varchar(500) DEFAULT NULL,
  `status` int(11),
  PRIMARY KEY (`paper_id`)
);

describe paper;

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
describe paper_authors;

CREATE TABLE `sam2020`.`notification` (
  `notification_id` int(11) NOT NULL AUTO_INCREMENT,
  `senderId` int(11) DEFAULT NULL,
  `message` varchar(500) DEFAULT NULL,
  `createdDate` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`notification_id`),
  KEY `sender_user_id_idx` (`senderId`),
  CONSTRAINT `sender_user_id` FOREIGN KEY (`senderId`) REFERENCES `user` (`user_id`)
);

describe notification;
 
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
);

describe notification_recipients;

CREATE TABLE preferences (
  preference_id int(11) NOT NULL AUTO_INCREMENT,
  paper_submission DATETIME,
  review_submission DATETIME,
  review_choice DATETIME,
  author_notification DATETIME,
  PRIMARY KEY (`preference_id`)
);

describe preferences;

CREATE TABLE `sam2020`.`preselection` (
  `preselection_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `paper_id` int(11) NOT NULL,
  PRIMARY KEY (`preselection_id`)
);

describe preselection;

CREATE TABLE `sam2020`.`review` (
  `review_id` int(11) NOT NULL AUTO_INCREMENT,
  `paper_id` int(11) ,
  `reviewer_id` int(11) NOT NULL,
  `paper_reference_name` varchar(500) DEFAULT NULL,
  `text` varchar(500) DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `edited_date` datetime DEFAULT NULL,
  PRIMARY KEY (`review_id`),
  KEY `review_reviewer_idx` (`reviewer_id`),
  CONSTRAINT `review_reviewer` FOREIGN KEY (`reviewer_id`) REFERENCES `user` (`user_id`)
) ;

describe review;
-- END Create Tables

-- START Inserting Data

INSERT INTO `sam2020`.`role`
(role_name)
VALUES
('Administrator'),
('Author'),
('PCC'),
('PCM');

-- Insert Default deadlines.
INSERT INTO PREFERENCES(paper_submission, review_submission, review_choice, author_notification)
VALUES('2019-11-03 00:00:00','2019-11-03 00:00:00','2019-11-03 00:00:00','2019-11-03 00:00:00');

-- END Inserting Data
select * from role;


insert into user (name,username,password,role_id)VALUES('admin','admin@admin.com','admin',1);
  select * from user;