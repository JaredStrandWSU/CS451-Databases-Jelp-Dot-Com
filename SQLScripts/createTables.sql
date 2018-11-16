CREATE TABLE Users(
	user_id VARCHAR(50) PRIMARY KEY,
	name VARCHAR(40),
	yelping_since DATE,
	average_stars FLOAT,
	review_count INTEGER,
	cool INTEGER,
	funny INTEGER,
	useful INTEGER
);

CREATE TABLE Businesses(
	business_id VARCHAR(50) PRIMARY KEY,
	name VARCHAR(100),
	address VARCHAR(100),
	state VARCHAR(20),
	city VARCHAR(30),
	postal_code CHAR(5),
	longitude FLOAT,
	latitude FLOAT,
	review_rating FLOAT,
	review_count INTEGER,
	open_status BOOLEAN,
	num_checkIns INTEGER
);

CREATE TABLE Friends(
	user_id VARCHAR(50),
	friend_id VARCHAR(50),
	PRIMARY KEY(user_id, friend_id),
	FOREIGN KEY (user_id) REFERENCES Users(user_id),
	FOREIGN KEY (friend_id) REFERENCES Users(user_id)
);

CREATE TABLE Reviews(
	review_id VARCHAR(50) PRIMARY KEY,
	user_id VARCHAR(50),
	business_id VARCHAR(50),
	stars INTEGER,
	date DATE,
	cool INTEGER,
	funny INTEGER,
	useful INTEGER,
	text VARCHAR(2000),
	FOREIGN KEY (user_id) REFERENCES Users(user_id),
	FOREIGN KEY (business_id) REFERENCES Businesses(business_id)
);

CREATE TABLE CheckIns(
	business_id VARCHAR(50),
	day VARCHAR(10),
	morning INTEGER,
	afternoon INTEGER,
	evening INTEGER,
	night INTEGER,
	PRIMARY KEY(business_id, day),
	FOREIGN KEY (business_id) REFERENCES Businesses(business_id)
);

CREATE TABLE Hours(
	business_id VARCHAR(50),
	day VARCHAR(10),
	open VARCHAR(5),
    close VARCHAR(5),
	PRIMARY KEY(business_id, day),
	FOREIGN KEY (business_id) REFERENCES Businesses(business_id)
);

CREATE TABLE Categories(
	name VARCHAR(60) PRIMARY KEY
);

CREATE TABLE businessCategories(
	business_id VARCHAR(50),
	category VARCHAR(60),
	PRIMARY KEY (business_id, category),
	FOREIGN KEY (business_id) REFERENCES Businesses(business_id)
	FOREIGN KEY (category) REFERENCES Categories(name)
);


